using System;
namespace MapApplication.Models
{
	public class Users
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string UserEmail { get; set; }
		public string UserPassword { get; set; }
		public string createdDate { get; set; }
		public List<Wkt> UserShapes { get; set; }
		public List<Point> UserPoints { get; set; }
		public List<Tabs> UserTabs { get; set; }
	}
}

