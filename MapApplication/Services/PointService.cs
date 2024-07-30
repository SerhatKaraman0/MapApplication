using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapApplication.Data;
using MapApplication.Interfaces;
using MapApplication.Models;

namespace MapApplication.Services
{
    public class PointService : IPointService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseService _responseService;

        public PointService(IUnitOfWork unitOfWork, IResponseService responseService)
        {
            _unitOfWork = unitOfWork;
            _responseService = responseService;
        }

        // GET Requests
        public async Task<Response> GetAll()
        {
            try
            {
                var points = await _unitOfWork.Points.GetAllAsync();
                if (points.success)
                {
                    return _responseService.SuccessResponse(points.point, "Points retrieved successfully.", true);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), points.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error fetching points: {ex.Message}", false);
            }
        }

        public async Task<Response> GetById(int id)
        {
            try
            {
                var point = await _unitOfWork.Points.GetByIdAsync(id);
                if (point.success)
                {
                    return _responseService.SuccessResponse(point.point, $"Point with id: {id} retrieved successfully.", true);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error retrieving point with id {id}: {point.ResponseMessage}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error retrieving point with id {id}: {ex.Message}", false);
            }
        }

        public async Task<Response> GetPointsCount()
        {
            try
            {
                var points = await _unitOfWork.Points.GetAllAsync();
                if (points.success)
                {
                    var count = ((List<PointDb>)points.point).Count;
                    return _responseService.SuccessResponse(new List<PointDb>(), $"{count} points retrieved successfully", true);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), "Error retrieving points count", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb> { }, $"Error retrieving points count: {ex.Message}", false);
            }
        }

        public async Task<Response> SearchPointsByCoordinates(double X, double Y, double range)
        {
            try
            {
                var points = await _unitOfWork.Points.FindAsync(p => Math.Sqrt(Math.Pow(p.X_coordinate - X, 2) + Math.Pow(p.Y_coordinate - Y, 2)) <= range);
                if (points.success)
                {
                    return _responseService.SuccessResponse(points.point, $"{((List<PointDb>)points.point).Count} points found within the range.", true);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), points.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error searching points by coordinates: {ex.Message}", false);
            }
        }

        public async Task<Response> GetPointsInRadius(double centerX, double centerY, double radius)
        {
            try
            {
                var points = await _unitOfWork.Points.FindAsync(p => Math.Sqrt(Math.Pow(centerX - p.X_coordinate, 2) + Math.Pow(centerY - p.Y_coordinate, 2)) <= radius);
                if (points.success)
                {
                    return _responseService.SuccessResponse(points.point, $"{((List<PointDb>)points.point).Count} points found within the radius.", true);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), points.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error retrieving points in radius: {ex.Message}", false);
            }
        }

        public async Task<Response> GetNearestPoint(double X, double Y)
        {
            try
            {
                var points = await _unitOfWork.Points.GetAllAsync();
                if (points.success)
                {
                    var nearestPoint = ((List<PointDb>)points.point)
                        .OrderBy(p => Math.Sqrt(Math.Pow(X - p.X_coordinate, 2) + Math.Pow(Y - p.Y_coordinate, 2)))
                        .FirstOrDefault();
                    if (nearestPoint != null)
                    {
                        return _responseService.SuccessResponse(new List<PointDb> { nearestPoint }, $"Nearest point to {X}, {Y} is {nearestPoint.Name} at ({nearestPoint.X_coordinate}, {nearestPoint.Y_coordinate})", true);
                    }
                    return _responseService.ErrorResponse(new List<PointDb>(), "No point found", false);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), points.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb> { }, $"Error retrieving nearest point: {ex.Message}", false);
            }
        }

        public async Task<Response> Distance(string pointName1, string pointName2)
        {
            try
            {
                var point1 = await _unitOfWork.Points.FindAsync(p => p.Name == pointName1);
                var point2 = await _unitOfWork.Points.FindAsync(p => p.Name == pointName2);

                if (point1.success && point2.success)
                {
                    var p1 = ((List<PointDb>)point1.point).FirstOrDefault();
                    var p2 = ((List<PointDb>)point2.point).FirstOrDefault();
                    if (p1 != null && p2 != null)
                    {
                        var distance = Math.Sqrt(Math.Pow(p1.X_coordinate - p2.X_coordinate, 2) + Math.Pow(p1.Y_coordinate - p2.Y_coordinate, 2));
                        return _responseService.SuccessResponse(new List<PointDb> { p1, p2 }, $"Distance between {p1.Name} and {p2.Name} is {distance}", true);
                    }
                    return _responseService.ErrorResponse(new List<PointDb>(), "One or both points not found.", false);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), $"{point1.ResponseMessage} {point2.ResponseMessage}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb> { }, $"Error calculating distance: {ex.Message}", false);
            }
        }

        // POST Requests
        public async Task<Response> GeneratePoints()
        {
            try
            {
                Random rnd = new Random();
                List<string> cities = new List<string>
                {
                    "Ankara", "İstanbul", "İzmir", "Antalya",
                    "Muğla", "Adana", "Eskişehir", "Mersin",
                    "Samsun", "Kocaeli"
                };

                var newPoints = new List<PointDb>();
                for (int i = 0; i < 10; i++)
                {
                    var item = new PointDb()
                    {
                        X_coordinate = rnd.Next(2800000, 4970000),
                        Y_coordinate = rnd.Next(4100000, 5199000),
                        Name = cities[i]
                    };
                    newPoints.Add(item);
                }

                await _unitOfWork.Points.AddRangeAsync(newPoints);
                await _unitOfWork.CommitAsync(); // Commit the transaction
                return _responseService.SuccessResponse(newPoints, "10 Points generated successfully.", true);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb> { }, $"Error generating points: {ex.Message}", false);
            }
        }

        public async Task<Response> Add(PointDb point)
        {
            try
            {
                var result = await _unitOfWork.Points.AddAsync(point);
                if (result.success)
                {
                    await _unitOfWork.CommitAsync(); // Commit the transaction
                }
                return result;
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error adding point: {ex.Message}", false);
            }
        }

        // PUT Requests
        public async Task<Response> Update(int id, PointDb updatedPoint)
        {
            try
            {
                var point = await _unitOfWork.Points.GetByIdAsync(id);
                if (point.success)
                {
                    var existingPoint = ((List<PointDb>)point.point).FirstOrDefault();
                    if (existingPoint != null)
                    {
                        existingPoint.X_coordinate = updatedPoint.X_coordinate;
                        existingPoint.Y_coordinate = updatedPoint.Y_coordinate;
                        existingPoint.Name = updatedPoint.Name;
                        var result = await _unitOfWork.Points.UpdateAsync(existingPoint);
                        if (result.success)
                        {
                            await _unitOfWork.CommitAsync(); // Commit the transaction
                        }
                        return result;
                    }
                    return _responseService.ErrorResponse(new List<PointDb>(), $"Point with id {id} not found.", false);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), point.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error updating point: {ex.Message}", false);
            }
        }

        public async Task<Response> UpdateByName(string Name, PointDb updatedPoint)
        {
            try
            {
                var point = await _unitOfWork.Points.FindAsync(p => p.Name == Name);
                if (point.success)
                {
                    var existingPoint = ((List<PointDb>)point.point).FirstOrDefault();
                    if (existingPoint != null)
                    {
                        existingPoint.X_coordinate = updatedPoint.X_coordinate;
                        existingPoint.Y_coordinate = updatedPoint.Y_coordinate;
                        existingPoint.Name = updatedPoint.Name;
                        var result = await _unitOfWork.Points.UpdateAsync(existingPoint);
                        if (result.success)
                        {
                            await _unitOfWork.CommitAsync(); // Commit the transaction
                        }
                        return result;
                    }
                    return _responseService.ErrorResponse(new List<PointDb>(), $"Point with name {Name} not found.", false);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), point.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error updating point: {ex.Message}", false);
            }
        }

        // DELETE Requests
        public async Task<Response> DeleteById(int id)
        {
            try
            {
                var point = await _unitOfWork.Points.GetByIdAsync(id);
                if (point.success)
                {
                    var existingPoint = ((List<PointDb>)point.point).FirstOrDefault();
                    if (existingPoint != null)
                    {
                        var result = await _unitOfWork.Points.DeleteAsync(existingPoint.Id);
                        if (result.success)
                        {
                            await _unitOfWork.CommitAsync(); // Commit the transaction
                        }
                        return result;
                    }
                    return _responseService.ErrorResponse(new List<PointDb>(), $"Point with id {id} not found.", false);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), point.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb> { }, $"Error deleting point by id: {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteByName(string name)
        {
            try
            {
                var point = await _unitOfWork.Points.FindAsync(p => p.Name == name);
                if (point.success)
                {
                    var existingPoint = ((List<PointDb>)point.point).FirstOrDefault();
                    if (existingPoint != null)
                    {
                        var result = await _unitOfWork.Points.DeleteAsync(existingPoint.Id);
                        if (result.success)
                        {
                            await _unitOfWork.CommitAsync(); // Commit the transaction
                        }
                        return result;
                    }
                    return _responseService.ErrorResponse(new List<PointDb> { }, $"Point with name {name} not found.", false);
                }
                return _responseService.ErrorResponse(new List<PointDb> { }, point.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb> { }, $"Error deleting point by name: {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteAll()
        {
            try
            {
                var points = await _unitOfWork.Points.GetAllAsync();
                if (points.success)
                {
                    var allPoints = (List<PointDb>)points.point;
                    var result = await _unitOfWork.Points.DeleteRangeAsync(allPoints);
                    if (result.success)
                    {
                        await _unitOfWork.CommitAsync(); 
                    }
                    return result;
                }
                return _responseService.ErrorResponse(new List<PointDb>(), "Error deleting all points", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error deleting all points: {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteInRange(double minX, double minY, double max_X, double maxY)
        {
            try
            {
                var points = await _unitOfWork.Points.FindAsync(p => (minX <= p.X_coordinate) && (minY <= p.Y_coordinate) && (p.X_coordinate <= max_X) && (p.Y_coordinate <= maxY));
                if (points.success)
                {
                    var pointsInRange = (List<PointDb>)points.point;
                    var initialLength = pointsInRange.Count;
                    var result = await _unitOfWork.Points.DeleteRangeAsync(pointsInRange);
                    if (result.success)
                    {
                        await _unitOfWork.CommitAsync(); // Commit the transaction
                        var remainingLength = pointsInRange.Count;
                        var deletedCount = initialLength - remainingLength;
                        return _responseService.SuccessResponse(new List<PointDb>(), $"{deletedCount} Points deleted successfully", true);
                    }
                    return _responseService.ErrorResponse(new List<PointDb>(), "Error deleting points in range", false);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), points.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error deleting points in range: {ex.Message}", false);
            }
        }
    }
}
