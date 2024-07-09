using System;
using System.Collections.Generic;
using System.Linq;
using MapApplication.Interfaces;
using MapApplication.Models;
using MapApplication.Data;

namespace MapApplication.Interfaces
{

    public interface IPointService
    {
        Task<Response> GeneratePoints();
        Task<Response> GetAll();
        Task<Response> GetById(int id);
        Task<Response> Add(PointDb point);
        Task<Response> DeleteById(int id);
        Task<Response> DeleteByName(string name);
        Task<Response> Update(int id, PointDb updatedPoint);
        Task<Response> SearchPointsByCoordinates(double x, double y, double range);
        Task<Response> UpdateByName(string name, PointDb updatedPoint);
        Task<Response> GetPointsCount();
        Task<Response> Distance(string pointName1, string pointName2);
        Task<Response> DeleteAll();
        Task<Response> DeleteInRange(double minX, double minY, double max_X, double maxY);
        Task<Response> GetPointsInRadius(double circleX, double circleY, double radius);
        Task<Response> GetNearestPoint(double X, double Y);
    }

}

