using System;
using MapApplication.Data;

namespace MapApplication.Models
{
	public class Wkt
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string WKT { get; set; }
		public string PhotoLocation { get; set; }
        public string Color { get; set; }
        public List<FeatureDb> Features { get; set; }
    }
}

