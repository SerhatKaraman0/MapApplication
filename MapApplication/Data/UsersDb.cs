using System;
using MapApplication.Models;

namespace MapApplication.Data
{
	public class UsersDb
	{
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string createdDate { get; set; }
        public List<WktDb> UserShapes { get; set; }
        public List<PointDb> UserPoints { get; set; }
        public List<TabsDb> UserTabs { get; set; }
    }
}

