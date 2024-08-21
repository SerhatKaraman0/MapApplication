using System;
namespace MapApplication.Data
{
	public class FeatureDb
	{
		public int FeatureId { get; set; }
		public int OwnerId { get; set; }
		public int OwnerShapeId { get; set; }
		public string OwnerShapeType { get; set; }
		public string FeatureName { get; set; }
		public string FeatureData { get; set; }
		public string createdDate { get; set; }
	}
}

