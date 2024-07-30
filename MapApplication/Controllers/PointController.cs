﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MapApplication.Data;
using MapApplication.Interfaces;
using MapApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace MapApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPointService _pointService;

        public ValuesController(IUnitOfWork unitOfWork, IPointService pointService)
        {
            _unitOfWork = unitOfWork;
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
        public async Task<Response> Add(PointDb point)
        {
            var response = await _pointService.Add(point);
            await _unitOfWork.CommitAsync();  
            return response;
        }

        [HttpPut("{id}")]
        public async Task<Response> Update([FromRoute] int id, [FromBody] PointDb updatedPoint)
        {
            var response = await _pointService.Update(id, updatedPoint);
            await _unitOfWork.CommitAsync();  
            return response;
        }

        [HttpPut("updateByName/{name}")]
        public async Task<Response> UpdateByName([FromRoute] string name, [FromBody] PointDb updatedPoint)
        {
            var response = await _pointService.UpdateByName(name, updatedPoint);
            await _unitOfWork.CommitAsync(); 
            return response;
        }

        [HttpDelete("all")]
        public async Task<Response> DeleteAll()
        {
            var response = await _pointService.DeleteAll();
            await _unitOfWork.CommitAsync();  
            return response;
        }

        [HttpDelete("deleteByRange")]
        public async Task<Response> DeleteInRange(double minX, double minY, double max_X, double maxY)
        {
            var response = await _pointService.DeleteInRange(minX, minY, max_X, maxY);
            await _unitOfWork.CommitAsync();  
            return response;
        }

        [HttpDelete("name/{name}")]
        public async Task<Response> DeleteByName([FromRoute] string name)
        {
            var response = await _pointService.DeleteByName(name);
            await _unitOfWork.CommitAsync();  
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<Response> DeleteById([FromRoute] int id)
        {
            var response = await _pointService.DeleteById(id);
            await _unitOfWork.CommitAsync();  
            return response;
        }
    }
}
