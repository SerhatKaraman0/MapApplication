using System;
using MapApplication.Data;
using MapApplication.Interfaces;
using MapApplication.Models;

namespace MapApplication.Services
{
    public class UserResponseService : IUserResponseService
    {
        public UserResponse ErrorResponse(List<UsersDb> users, string ResponseMessage, bool success)
        {
            return new UserResponse {
                Users = users,
                ResponseMessage = ResponseMessage,
                success = false
            };
        }

        public UserResponse SuccessResponse(List<UsersDb> users, string ResponseMessage, bool success)
        {
            return new UserResponse {
                Users = users,
                ResponseMessage = ResponseMessage,
                success = true
            };
        }
    }
}

