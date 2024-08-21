using System;
using MapApplication.Data;

namespace MapApplication.Models
{
	public class WktResponse
	{
        public List<WktDb> wkt { get; set; }
        public string ResponseMessage { get; set; }
        public bool success { get; set; }
    }
}

