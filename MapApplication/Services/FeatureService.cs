using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapApplication.Data;
using MapApplication.Interfaces;
using MapApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace MapApplication.Services
{
    public class FeatureService : IFeatureService
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFeatureResponseService _featureResponseService;

        public FeatureService(AppDbContext context, IUnitOfWork unitOfWork, IFeatureResponseService featureResponseService)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _featureResponseService = featureResponseService;
        }

        public async Task<FeatureResponse> AddFeatureToPoint(int ownerId, int pointId, string featureName, string featureData)
        {
            // Check if the Point exists
            var pointExists = await _unitOfWork.Points.FindAsync(p => p.Id == pointId);
            if (!pointExists.success)
            {
                return _featureResponseService.ErrorResponse(new List<FeatureDb>(), "Point does not exist", false);
            }

            return await AddFeature(ownerId, pointId, "Point", featureName, featureData);
        }

        public async Task<FeatureResponse> AddFeatureToWkt(int ownerId, int wktId, string featureName, string featureData)
        {
            // Check if the Wkt exists
            var wktExists = await _context.Wkt.AnyAsync(w => w.Id == wktId);
            if (!wktExists)
            {
                return _featureResponseService.ErrorResponse(new List<FeatureDb>(), "Wkt does not exist", false);
            }

            return await AddFeature(ownerId, wktId, "Wkt", featureName, featureData);
        }

        public async Task<FeatureResponse> AddFeature(int ownerId, int shapeId, string shapeType, string featureName, string featureData)
        {
            try
            {
                var newFeature = new FeatureDb
                {
                    OwnerId = ownerId,
                    OwnerShapeId = shapeId,
                    OwnerShapeType = shapeType,
                    FeatureName = featureName,
                    FeatureData = featureData,
                    createdDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                };

                await _context.Features.AddAsync(newFeature);
                await _context.SaveChangesAsync();

                return _featureResponseService.SuccessResponse(new List<FeatureDb> { newFeature }, $"Feature added successfully to {shapeType}", true);
            }
            catch (Exception ex)
            {
                return _featureResponseService.ErrorResponse(new List<FeatureDb>(), $"Error with message: {ex.Message}", false);
            }
        }

        public async Task<FeatureResponse> GetFeaturesOfPointById(int ownerId)
        {
            try
            {
                var features = await _context.Features
                    .Where(p => p.OwnerId == ownerId && p.OwnerShapeType == "Point")
                    .ToListAsync();

                if (!features.Any())
                {
                    return _featureResponseService.ErrorResponse(new List<FeatureDb>(), "No features found", false);
                }

                return _featureResponseService.SuccessResponse(features, "Features returned successfully", true);
            }
            catch (Exception ex)
            {
                return _featureResponseService.ErrorResponse(new List<FeatureDb>(), $"Error occurred with message: {ex.Message}", false);
            }
        }

        public async Task<FeatureResponse> GetFeaturesOfWktById(int ownerId)
        {
            try
            {
                var features = await _context.Features
                    .Where(p => p.OwnerId == ownerId && p.OwnerShapeType == "Wkt")
                    .ToListAsync();

                if (!features.Any())
                {
                    return _featureResponseService.ErrorResponse(new List<FeatureDb>(), "No features found", false);
                }

                return _featureResponseService.SuccessResponse(features, "Features retrieved successfully", true);
            }
            catch (Exception ex)
            {
                return _featureResponseService.ErrorResponse(new List<FeatureDb>(), $"Can't get features with error message: {ex.Message}", false);
            }
        }

        public async Task<FeatureResponse> RemoveFeatureOfPointById(int ownerId, int featureId)
        {
            try
            {
                var feature = await _context.Features
                    .FirstOrDefaultAsync(f => f.OwnerId == ownerId && f.FeatureId == featureId && f.OwnerShapeType == "Point");

                if (feature == null)
                {
                    return _featureResponseService.ErrorResponse(new List<FeatureDb>(), "No features found", false);
                }

                _context.Features.Remove(feature);
                await _context.SaveChangesAsync();

                return _featureResponseService.SuccessResponse(new List<FeatureDb> { feature }, "Feature deleted successfully", true);
            }
            catch (Exception ex)
            {
                return _featureResponseService.ErrorResponse(new List<FeatureDb>(), $"Error with message: {ex.Message}", false);
            }
        }

        public async Task<FeatureResponse> RemoveFeatureOfWktById(int ownerId, int featureId)
        {
            try
            {
                var feature = await _context.Features
                    .FirstOrDefaultAsync(f => f.OwnerId == ownerId && f.FeatureId == featureId && f.OwnerShapeType == "Wkt");

                if (feature == null)
                {
                    return _featureResponseService.ErrorResponse(new List<FeatureDb>(), "No features found", false);
                }

                _context.Features.Remove(feature);
                await _context.SaveChangesAsync();

                return _featureResponseService.SuccessResponse(new List<FeatureDb> { feature }, "Feature deleted successfully", true);
            }
            catch (Exception ex)
            {
                return _featureResponseService.ErrorResponse(new List<FeatureDb>(), $"Error with message: {ex.Message}", false);
            }
        }

        public async Task<FeatureResponse> UpdateFeatureOfPointById(int ownerId, int featureId, FeatureDb updatedFeature)
        {
            try
            {
                var feature = await _context.Features
                    .FirstOrDefaultAsync(f => f.OwnerId == ownerId && f.FeatureId == featureId && f.OwnerShapeType == "Point");

                if (feature == null)
                {
                    return _featureResponseService.ErrorResponse(new List<FeatureDb>(), "No features found", false);
                }

                feature.FeatureName = updatedFeature.FeatureName;
                feature.FeatureData = updatedFeature.FeatureData;
                feature.createdDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                _context.Features.Update(feature);
                await _context.SaveChangesAsync();

                return _featureResponseService.SuccessResponse(new List<FeatureDb> { feature }, "Feature updated successfully", true);
            }
            catch (Exception ex)
            {
                return _featureResponseService.ErrorResponse(new List<FeatureDb>(), $"Error with message: {ex.Message}", false);
            }
        }

        public async Task<FeatureResponse> UpdateFeatureOfWktById(int ownerId, int featureId, FeatureDb updatedFeature)
        {
            try
            {
                var feature = await _context.Features
                    .FirstOrDefaultAsync(f => f.OwnerId == ownerId && f.FeatureId == featureId && f.OwnerShapeType == "Wkt");

                if (feature == null)
                {
                    return _featureResponseService.ErrorResponse(new List<FeatureDb>(), "No features found", false);
                }

                feature.FeatureName = updatedFeature.FeatureName;
                feature.FeatureData = updatedFeature.FeatureData;
                feature.createdDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                _context.Features.Update(feature);
                await _context.SaveChangesAsync();

                return _featureResponseService.SuccessResponse(new List<FeatureDb> { feature }, "Feature updated successfully", true);
            }
            catch (Exception ex)
            {
                return _featureResponseService.ErrorResponse(new List<FeatureDb>(), $"Error with message: {ex.Message}", false);
            }
        }
    }
}
