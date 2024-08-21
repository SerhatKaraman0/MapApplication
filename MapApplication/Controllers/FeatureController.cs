using System;
using MapApplication.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MapApplication.Models;
using MapApplication.Data;

namespace MapApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPointService _pointService;
        private readonly IWktService _wktService;
        private readonly IFeatureService _featureService;

        public FeaturesController(IUnitOfWork unitOfWork, IPointService pointService, IWktService wktService, IFeatureService featureService)
        {
            _unitOfWork = unitOfWork;
            _pointService = pointService;
            _wktService = wktService;
            _featureService = featureService;
        }

        [HttpGet("point/{ownerId}/features")]
        public async Task<FeatureResponse> GetFeaturesByPointId([FromRoute] int ownerId)
        {
            var response = await _featureService.GetFeaturesOfPointById(ownerId);
            return response;
        }

        [HttpGet("shape/{ownerId}/features")]
        public async Task<FeatureResponse> GetFeaturesByWkttId([FromRoute] int ownerId)
        {
            var response = await _featureService.GetFeaturesOfWktById(ownerId);
            return response;
        }

        [HttpDelete("point/{ownerId}/features/delete/{featureId}")]
        public async Task<FeatureResponse> RemoveFeatureOfPointById([FromRoute] int ownerId, [FromRoute] int featureId)
        {
            var response = await _featureService.RemoveFeatureOfPointById(ownerId, featureId);
            return response;
        }

        [HttpDelete("wkt/{ownerId}/features/delete/{featureId}")]
        public async Task<FeatureResponse> RemoveFeatureOfWktById([FromRoute] int ownerId, [FromRoute] int featureId)
        {
            var response = await _featureService.RemoveFeatureOfWktById(ownerId, featureId);
            return response;
        }

        [HttpPut("point/{ownerId}/features/update/{featureId}")]
        public async Task<FeatureResponse> UpdateFeatureOfPointById([FromRoute] int ownerId, [FromRoute] int featureId, [FromBody] FeatureDb updatedFeature)
        {
            var response = await _featureService.UpdateFeatureOfPointById(ownerId, featureId, updatedFeature);
            return response;
        }

        [HttpPut("wkt/{ownerId}/features/update/{featureId}")]
        public async Task<FeatureResponse> UpdateFeatureOfWktById([FromRoute] int ownerId, [FromRoute] int featureId, [FromBody] FeatureDb updatedFeature)
        {
            var response = await _featureService.UpdateFeatureOfWktById(ownerId, featureId, updatedFeature);
            return response;
        }

        [HttpPost("{ownerId}/point/{pointId}/features/add/featureName/{featureName}/featureData/{featureData}")]
        public async Task<FeatureResponse> AddFeatureToPoint([FromRoute] int ownerId, [FromRoute] int pointId, [FromRoute] string featureName, [FromRoute] string featureData)
        {
            var response = await _featureService.AddFeatureToPoint(ownerId, pointId, featureName, featureData);
            return response;
        }

        [HttpPost("{ownerId}/wkt/{wktId}/features/add/featureName/{featureName}/featureData/{featureData}")]
        public async Task<FeatureResponse> AddFeatureToWkt([FromRoute] int ownerId, [FromRoute] int wktId, [FromRoute] string featureName, [FromRoute] string featureData)
        {
            var response = await _featureService.AddFeatureToWkt(ownerId, wktId, featureName, featureData);
            return response;
        }

    }

}