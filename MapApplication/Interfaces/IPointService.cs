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
        Task<Response> GeneratePoints(int ownerId);
        Task<Response> GetAll(int ownerId);
        Task<Response> GetById(int ownerId, int id);
        Task<Response> Add(int ownerId, PointDb point);
        Task<Response> DeleteById(int ownerId, int id);
        Task<Response> DeleteByName(int ownerId, string name);
        Task<Response> Update(int ownerId, int id, PointDb updatedPoint);
        Task<Response> SearchPointsByCoordinates(int ownerId, double x, double y, double range);
        Task<Response> UpdateByName(int ownerId, string name, PointDb updatedPoint);
        Task<Response> GetPointsCount(int ownerId);
        Task<Response> Distance(int ownerId, string pointName1, string pointName2);
        Task<Response> DeleteAll(int ownerId);
        Task<Response> DeleteInRange(int ownerId, double minX, double minY, double max_X, double maxY);
        Task<Response> GetPointsInRadius(int ownerId, double circleX, double circleY, double radius);
        Task<Response> GetNearestPoint(int ownerId, double X, double Y);
        Task<Response> GetPointsInTheSameDay(int ownerId, string date);
        Task<Dictionary<string, Dictionary<string, List<PointDb>>>> GetPointsCategorizedByDate(int ownerId);
    }

}

