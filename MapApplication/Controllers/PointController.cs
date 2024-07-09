using System.Collections.Generic;
using MapApplication.Interfaces;
using MapApplication.Models;
using Microsoft.AspNetCore.Mvc;
using MapApplication.Data;

namespace MapApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IPointService _pointService;

        public ValuesController(IPointService pointService)
        {
            _pointService = pointService;
        }

        [HttpGet("generate")]
        public Task<Response> GeneratePoints()
        {
            var response = _pointService.GeneratePoints();
            return response;
        }

        [HttpGet]
        public Task<Response> GetAll()
        {
            var response = _pointService.GetAll();
            return response;
        }

        [HttpGet("{id}")]
        public Task<Response> GetById([FromRoute] int id)
        {
            var response = _pointService.GetById(id);
            return response;
        }


        [HttpGet("pointsInRadius")]
        public Task<Response> GetPointsInRadius(double circleX, double circleY, double radius)
        {
            var response = _pointService.GetPointsInRadius(circleX, circleY, radius);
            return response;
        }

        [HttpGet("getNearestPoint")]
        public Task<Response> GetNearestPoint(double X, double Y)
        {
            var response = _pointService.GetNearestPoint(X, Y);
            return response;
        }

        [HttpGet("search")]
        public Task<Response> SearchPointsByCoordinates([FromQuery] int x, [FromQuery] int y, [FromQuery] int range)
        {
            var response = _pointService.SearchPointsByCoordinates(x, y, range);
            return response;
        }

        [HttpGet("count")]
        public Task<Response> GetPointsCount()
        {
            var response = _pointService.GetPointsCount();
            return response;
        }

        [HttpGet("distance")]
        public Task<Response> Distance(string pointName1, string pointName2)
        {
            var response = _pointService.Distance(pointName1, pointName2);
            return response;
        }

        [HttpPost]
        public Task<Response> Add(PointDb point)
        {
            var response = _pointService.Add(point);
            return response;
        }


        [HttpPut("{id}")]
        public Task<Response> Update([FromRoute] int id, [FromBody] PointDb updatedPoint)
        {
            var response = _pointService.Update(id, updatedPoint);
            return response;
        }

        

        [HttpPut("updateByName/{name}")]
        public Task<Response> UpdateByName([FromRoute] string name, [FromBody] PointDb updatedPoint)
        {
            var response = _pointService.UpdateByName(name, updatedPoint);
            return response;
        }

        

        [HttpDelete("all")]
        public Task<Response> DeleteAll()
        {
            var response = _pointService.DeleteAll();
            return response;
        }

        [HttpDelete("deleteByRange")]
        public Task<Response> DeleteInRange(double minX, double minY, double max_X, double maxY)
        {
            var response = _pointService.DeleteInRange(minX, minY, max_X, maxY);
            return response;
        }

        [HttpDelete("name/{name}")]
        public Task<Response> DeleteByName([FromRoute] string name)
        {
            var response = _pointService.DeleteByName(name);
            return response;
        }

        [HttpDelete("{id}")]
        public Task<Response> DeleteById([FromRoute] int id)
        {
            var response = _pointService.DeleteById(id);
            return response;
        }

    }
}
