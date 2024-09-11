using System.Collections.Generic;
using System.Threading.Tasks;
using MapApplication.Data;
using MapApplication.Interfaces;
using MapApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace MapApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPointService _pointService;
        private readonly IWktService _wktService;
        private readonly IUserService _userService;
        private readonly ITabService _tabService;

        public UsersController(IUnitOfWork unitOfWork, IPointService pointService, IWktService wktService, ITabService tabService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _pointService = pointService;
            _wktService = wktService;
            _userService = userService;
            _tabService = tabService;

        }

        [HttpGet]
        public async Task<UserResponse> GetAllUsers() {
            var response = await _userService.GetAllUsers();
            return response;
        }

        [HttpGet("{id}")]
        public async Task<UserResponse> GetUserById([FromRoute] int id) {
            var response = await _userService.GetUserById(id);
            return response;
        }

        [HttpPost("create")]
        public async Task<UserResponse> CreateUser(UsersDb User) {
            var response = await _userService.CreateUser(User);
            return response;
        }

        [HttpPost("{userId}/add/point")]
        public async Task<UserResponse> AddPointToUser([FromRoute] int userId, PointDb point)
        {
            var response = await _userService.AddToUsersPoints(userId, point);
            return response;
        }

        [HttpPost("{userId}/add/shape")]
        public async Task<UserResponse> AddShapeToUser([FromRoute] int userId, WktDb shape)
        {
            var response = await _userService.AddToUsersShapes(userId, shape);
            return response;
        }

        [HttpPost("{userId}/add/tab")]
        public async Task<UserResponse> AddTabToUser([FromRoute] int userId, TabsDb newTab)
        {
            var response = await _userService.AddToUsersTabs(userId, newTab);
            return response;
        }

        [HttpDelete("{userId}/delete")]
        public async Task<UserResponse> RemoveUserById([FromRoute] int userId) {
            var response = await _userService.RemoveUserById(userId);
            return response;
        }

        [HttpPut("{userId}/update")]
        public async Task<UserResponse> UpdateUserById([FromRoute] int userId, [FromBody] UsersDb updatedUser) {
            var response = await _userService.UpdateUserById(userId, updatedUser);
            return response;
        }

        [HttpGet("email/{email}/pwd/{password}")]
        public async Task<UsersDb> GetUserByEmailAndPassword([FromRoute] string email, [FromRoute] string password)
        {
            var response = await _userService.GetUserByEmailAndPassword(email, password);
            return response;
        }

    }
}

