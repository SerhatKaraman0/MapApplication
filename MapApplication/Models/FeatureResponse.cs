using System;
using MapApplication.Data;

namespace MapApplication.Models
{
	public class FeatureResponse
	{
        public List<FeatureDb> features { get; set; }
        public string ResponseMessage { get; set; }
        public bool success { get; set; }
    }
}

