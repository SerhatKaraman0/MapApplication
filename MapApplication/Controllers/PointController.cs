using System.Collections.Generic;
using System.Threading.Tasks;
using MapApplication.Data;
using MapApplication.Interfaces;
using MapApplication.Models;
using MapApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace MapApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPointService _pointService;
        private readonly IWktService _wktService;

        public ValuesController(IUnitOfWork unitOfWork, IPointService pointService, IWktService wktService)
        {
            _unitOfWork = unitOfWork;
            _pointService = pointService;
            _wktService = wktService;
        }

        [HttpGet("{ownerId}/generate")]
        public Task<Response> GeneratePoints([FromRoute] int ownerId)
        {
            var response = _pointService.GeneratePoints(ownerId);
            return response;
        }

        [HttpGet("{ownerId}/getAll")]
        public Task<Response> GetAll([FromRoute] int ownerId)
        {
            var response = _pointService.GetAll(ownerId);
            return response;
        }

        [HttpGet("{ownerId}/point/{id}")]
        public Task<Response> GetById([FromRoute] int ownerId, [FromRoute] int id)
        {
            var response = _pointService.GetById(ownerId, id);
            return response;
        }

        [HttpGet("{ownerId}/pointsInRadius")]
        public Task<Response> GetPointsInRadius([FromRoute] int ownerId, double circleX, double circleY, double radius)
        {
            var response = _pointService.GetPointsInRadius(ownerId, circleX, circleY, radius);
            return response;
        }

        [HttpGet("{ownerId}/getNearestPoint")]
        public Task<Response> GetNearestPoint([FromRoute] int ownerId, double X, double Y)
        {
            var response = _pointService.GetNearestPoint(ownerId, X, Y);
            return response;
        }

        [HttpGet("{ownerId}/search")]
        public Task<Response> SearchPointsByCoordinates([FromRoute] int ownerId, [FromQuery] int x, [FromQuery] int y, [FromQuery] int range)
        {
            var response = _pointService.SearchPointsByCoordinates(ownerId, x, y, range);
            return response;
        }

        [HttpGet("{ownerId}/count")]
        public Task<Response> GetPointsCount([FromRoute] int ownerId)
        {
            var response = _pointService.GetPointsCount(ownerId);
            return response;
        }

        [HttpGet("{ownerId}/getPointsCategorizedByDate")]
        public async Task<Dictionary<string, Dictionary<string, List<PointDb>>>> GetPointsCategorizedByDate([FromRoute] int ownerId)
        {
            try
            {
                var pointsCategorized = await _pointService.GetPointsCategorizedByDate(ownerId);
                return pointsCategorized;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, Dictionary<string, List<PointDb>>>();
            }
        }

        [HttpGet("{ownerId}/getPointsInTheSameDay")]
        public async Task<Response> GetPointsInTheSameDay([FromRoute] int ownerId, [FromQuery] string date)
        {
           
            var response = await _pointService.GetPointsInTheSameDay(ownerId, date);
            return response;
            
        }

        [HttpGet("{ownerId}/distance")]
        public Task<Response> Distance([FromRoute] int ownerId, string pointName1, string pointName2)
        {
            var response = _pointService.Distance(ownerId, pointName1, pointName2);
            return response;
        }

        [HttpPost("{ownerId}/add")]
        public async Task<Response> Add([FromRoute] int ownerId, PointDb point)
        {
            var response = await _pointService.Add(ownerId, point);
            await _unitOfWork.CommitAsync();  
            return response;
        }

        [HttpPut("{ownerId}/point/{id}")]
        public async Task<Response> Update([FromRoute] int ownerId, [FromRoute] int id, [FromBody] PointDb updatedPoint)
        {
            var response = await _pointService.Update(ownerId, id, updatedPoint);
            await _unitOfWork.CommitAsync();  
            return response;
        }

        [HttpPut("{ownerId}/updateByName/{name}")]
        public async Task<Response> UpdateByName([FromRoute] int ownerId, [FromRoute] string name, [FromBody] PointDb updatedPoint)
        {
            var response = await _pointService.UpdateByName(ownerId, name, updatedPoint);
            await _unitOfWork.CommitAsync(); 
            return response;
        }

        [HttpDelete("{ownerId}/all")]
        public async Task<Response> DeleteAll([FromRoute] int ownerId)
        {
            var response = await _pointService.DeleteAll(ownerId);
            await _unitOfWork.CommitAsync();  
            return response;
        }

        [HttpDelete("{ownerId}/deleteByRange")]
        public async Task<Response> DeleteInRange([FromRoute] int ownerId, double minX, double minY, double max_X, double maxY)
        {
            var response = await _pointService.DeleteInRange(ownerId, minX, minY, max_X, maxY);
            await _unitOfWork.CommitAsync();  
            return response;
        }

        [HttpDelete("{ownerId}/name/{name}")]
        public async Task<Response> DeleteByName([FromRoute] int ownerId, [FromRoute] string name)
        {
            var response = await _pointService.DeleteByName(ownerId, name);
            await _unitOfWork.CommitAsync();  
            return response;
        }

        [HttpDelete("{ownerId}/point/{id}")]
        public async Task<Response> DeleteById([FromRoute] int ownerId, [FromRoute] int id)
        {
            var response = await _pointService.DeleteById(ownerId, id);
            await _unitOfWork.CommitAsync();  
            return response;
        }

        // Wkt endpoints

        [HttpGet("{ownerId}/wkt/all")]
        public async Task<WktResponse> GetAllWkt([FromRoute] int ownerId) {
            var response = await _wktService.GetAllWkt(ownerId);
            return response;
        }

        [HttpGet("{ownerId}/wkt/{id}")]
        public async Task<WktResponse> GetWktById([FromRoute] int ownerId, [FromRoute] int id) {
            var response = await _wktService.GetWktById(ownerId, id);
            return response;
        }

        [HttpPost("{ownerId}/wkt/create")]
        public async Task<WktResponse> AddWkt([FromRoute] int ownerId, WktDb wkt) {
            var response = await _wktService.CreateWkt(ownerId, wkt);
            return response;
        }

        [HttpPut("{ownerId}/wkt/update/{id}")]
        public async Task<WktResponse> UpdateWkt([FromRoute] int ownerId, [FromRoute] int id, [FromBody] WktDb wkt)
        {
            var response = await _wktService.UpdateWkt(ownerId, id, wkt);
            return response;
        }

        [HttpDelete("{ownerId}/wkt/delete/{id}")]
        public async Task<WktResponse> DeleteWkt([FromRoute] int ownerId, [FromRoute] int id)
        {
            var response = await _wktService.DeleteWktById(ownerId, id);
            return response;
        }



    }
}

