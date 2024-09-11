using System;
using MapApplication.Data;
using MapApplication.Interfaces;
using MapApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace MapApplication.Services
{
    
    public class UserService : IUserService
    {
        private readonly IUserResponseService _userResponseService;
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(AppDbContext context, IUserResponseService userResponseService, IUnitOfWork unitOfWork)
        {
            _userResponseService = userResponseService;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        
        public async Task<UserResponse> AddToUsersPoints(int userId, PointDb point)
        {
            try
            {
                var user = await _context.Users
                                          .Include(u => u.UserPoints)
                                          .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    return _userResponseService.ErrorResponse(new List<UsersDb>(), "No user found", false);
                }

                if (user.UserPoints == null)
                {
                    user.UserPoints = new List<PointDb>();
                }

                point.OwnerId = userId;
                await _unitOfWork.Points.AddAsync(point);  // Ensure the point is tracked by the context
                await _unitOfWork.CommitAsync();

                return _userResponseService.SuccessResponse(new List<UsersDb> { user }, "Point added successfully", true);
            }
            catch (Exception ex)
            {
                return _userResponseService.ErrorResponse(new List<UsersDb>(), $"Error adding point: {ex.Message}", false);
            }
        }


        public async Task<UserResponse> AddToUsersShapes(int userId, WktDb wkt)
        {
            try
            {
                var user = await _context.Users
                                          .Include(u => u.UserShapes)
                                          .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    return _userResponseService.ErrorResponse(new List<UsersDb>(), "No user found", false);
                }

                if (user.UserShapes == null)
                {
                    user.UserShapes = new List<WktDb>();
                }

                wkt.OwnerId = userId;
                _context.Wkt.Add(wkt);  // Ensure the shape is tracked by the context
               
                await _context.SaveChangesAsync();

                return _userResponseService.SuccessResponse(new List<UsersDb> { user }, "Shape added successfully", true);
            }
            catch (Exception ex)
            {
                return _userResponseService.ErrorResponse(new List<UsersDb>(), $"Error adding shape: {ex.Message}", false);
            }
        }


        public async Task<UserResponse> AddToUsersTabs(int userId, TabsDb newTab)
        {
            try
            {
                var user = await _context.Users
                                          .Include(u => u.UserTabs)
                                          .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    return _userResponseService.ErrorResponse(new List<UsersDb>(), "No user found", false);
                }

                if (user.UserTabs == null)
                {
                    user.UserTabs = new List<TabsDb>();
                }

                newTab.OwnerId = userId;
                newTab.createdDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                _context.Tabs.Add(newTab);  // Ensure the tab is tracked by the context
                await _context.SaveChangesAsync();

                return _userResponseService.SuccessResponse(new List<UsersDb> { user }, "Tab added successfully", true);
            }
            catch (Exception ex)
            {
                return _userResponseService.ErrorResponse(new List<UsersDb>(), $"Error adding tab: {ex.Message}", false);
            }
        }



        public async Task<UserResponse> CreateUser(UsersDb user)
        {
            try
            {
                user.createdDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                user.UserPoints[0].OwnerId = user.UserId;
                user.UserShapes[0].OwnerId = user.UserId;
                user.UserTabs[0].OwnerId = user.UserId;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return _userResponseService.SuccessResponse(new List<UsersDb> { user }, "User created successfully", true);
            }
            catch (Exception ex)
            {
                return _userResponseService.ErrorResponse(new List<UsersDb>(), ex.Message, false);
            }
        }

        public async Task<UserResponse> GetAllUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return _userResponseService.SuccessResponse(users, "Users returned successfully", true);
            }
            catch (Exception ex)
            {
                return _userResponseService.ErrorResponse(new List<UsersDb>(), ex.Message, false);
            }
        }

        public async Task<UserResponse> GetUserById(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                var usersTabs = await _context.Tabs.Where(p=> p.OwnerId == user.UserId).ToListAsync();
                var usersShapes = await _context.Wkt.Where(p => p.OwnerId == user.UserId).ToListAsync();
                var usersPoints = await _unitOfWork.Points.FindAsync(p => p.OwnerId == user.UserId);

                if (user == null) {
                    return _userResponseService.ErrorResponse(new List<UsersDb>(), "User with id not found", false);
                }

                user.UserTabs = usersTabs;
                user.UserShapes = usersShapes;
                user.UserPoints = usersPoints.point;
                return _userResponseService.SuccessResponse(new List<UsersDb> { user }, "User received successfully", true);
            }
            catch (Exception ex)
            {
                return _userResponseService.ErrorResponse(new List<UsersDb>(), "An Error occured fetching user", false);
            }
        }

        public async Task<UserResponse> RemoveUserById(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return _userResponseService.ErrorResponse(new List<UsersDb>(), "User not found", false);
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return _userResponseService.SuccessResponse(new List<UsersDb> { user }, "User deleted successfully", true);
            }
            catch (Exception ex)
            {
                return _userResponseService.ErrorResponse(new List<UsersDb>(), "Error deleting user", false);
            }
        }

        public async Task<UserResponse> UpdateUserById(int id, UsersDb updatedUser)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return _userResponseService.ErrorResponse(new List<UsersDb>(), "User with id not found", false);
                }
                user.UserEmail = updatedUser.UserEmail;
                user.UserPassword = updatedUser.UserPassword;
                user.UserPoints = updatedUser.UserPoints;
                user.UserShapes = updatedUser.UserShapes;
                user.UserTabs = updatedUser.UserTabs;
                _context.Users.Update(user);

                return _userResponseService.SuccessResponse(new List<UsersDb> { user }, "User updated successfully", true);
            }
            catch (Exception ex)
            {
                return _userResponseService.ErrorResponse(new List<UsersDb>(), ex.Message, false);
            }
        }

        public async Task<UsersDb> GetUserByEmailAndPassword(string email, string password)
        {
            // This example assumes plain text password storage, which is not recommended
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmail == email && u.UserPassword == password);
        }
    }
}

