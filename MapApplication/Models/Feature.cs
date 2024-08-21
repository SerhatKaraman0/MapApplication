using System;
namespace MapApplication.Models
{
	public class Feature
	{
		public int FeatureId { get; set; }
		public int OwnerId { get; set; }
		public int OwnerShapeId { get; set; }
		public int OwnerShapeType { get; set; }
		public string FeatureName { get; set; }
		public string FeatureData { get; set; }
		public string createdDate { get; set; }
	}
}

