using System;
using MapApplication.Models;
using MapApplication.Data;

namespace MapApplication.Interfaces
{
	public interface IUserService
	{
		Task<UserResponse> GetAllUsers();
		Task<UserResponse> GetUserById(int id);
		Task<UserResponse> CreateUser(UsersDb user);
		Task<UserResponse> UpdateUserById(int id, UsersDb updatedUser);
		Task<UserResponse> RemoveUserById(int id);
		Task<UserResponse> AddToUsersPoints(int userId, PointDb point);
		Task<UserResponse> AddToUsersShapes(int userId, WktDb wkt);
		Task<UserResponse> AddToUsersTabs(int userId, TabsDb newTab);
	}
}

