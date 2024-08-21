using System;
using MapApplication.Models;
using MapApplication.Data;

namespace MapApplication.Interfaces
{
	public interface IUserResponseService
	{
		UserResponse ErrorResponse(List<UsersDb> users, string ResponseMessage, bool success);
		UserResponse SuccessResponse(List<UsersDb> users, string ResponseMessage, bool success);
	}
}

