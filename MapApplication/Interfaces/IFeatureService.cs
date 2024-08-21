using System;
using MapApplication.Models;
using MapApplication.Data;

namespace MapApplication.Interfaces
{
	public interface IFeatureService
	{
		Task<FeatureResponse> AddFeatureToPoint(int ownerId, int pointId, string featureName, string featureData);
        Task<FeatureResponse> AddFeatureToWkt(int ownerId, int pointId, string featureName, string featureData);
		Task<FeatureResponse> AddFeature(int ownerId, int shapeId, string shapeType, string featureName, string featureData);
		Task<FeatureResponse> GetFeaturesOfPointById(int ownerId);
		Task<FeatureResponse> GetFeaturesOfWktById(int ownerId);
		Task<FeatureResponse> RemoveFeatureOfPointById(int ownerId, int featureId);
		Task<FeatureResponse> RemoveFeatureOfWktById(int ownerId, int featureId);
		Task<FeatureResponse> UpdateFeatureOfPointById(int ownerId, int featureId, FeatureDb updatedFeature);
        Task<FeatureResponse> UpdateFeatureOfWktById(int ownerId, int featureId, FeatureDb updatedFeature);
    }
}

