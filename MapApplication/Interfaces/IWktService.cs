using System;
using MapApplication.Models;
using MapApplication.Data;
namespace MapApplication.Interfaces
{
	public interface IWktService
	{
		Task<WktResponse> GetAllWkt(int ownerId);
        Task<WktResponse> GetWktById(int ownerId, int id);
        Task<WktResponse> CreateWkt(int ownerId, WktDb wkt);
        Task<WktResponse> UpdateWkt(int ownerId, int id, WktDb updatedWkt);
		Task<WktResponse> DeleteWktById(int ownerId, int id);
        Task<WktResponse> GetFeatureById(int ownerId, int featureId);
        Task<WktResponse> UpdateFeatureById(int ownerId, int featureId);
        Task<WktResponse> DeleteFeatureById(int ownerId, int featureId);
    }
}

