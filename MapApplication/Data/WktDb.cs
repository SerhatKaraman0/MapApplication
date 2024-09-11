using System;
namespace MapApplication.Data
{
	public class WktDb
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WKT { get; set; }
        public string PhotoLocation { get; set; }
        public string Color { get; set; }
        public string Date { get; set; }
        public int OwnerId { get; set; }
    }
}

