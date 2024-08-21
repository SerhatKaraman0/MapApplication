using System;
using MapApplication.Data;

namespace MapApplication.Models
{
	public class UserResponse
	{
		public List<UsersDb> Users { get; set; }
		public string ResponseMessage { get; set; }
		public bool success { get; set; }
	}
}

