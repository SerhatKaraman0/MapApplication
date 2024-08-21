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
    public class PointService : IPointService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseService _responseService;
        private readonly AppDbContext _context;

        public PointService(IUnitOfWork unitOfWork, IResponseService responseService, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _responseService = responseService;
            _context = context;
        }

        // GET Requests
        public async Task<Response> GetAll(int ownerId)
        {
            try
            {
                var points = await _unitOfWork.Points.FindAsync(p => p.OwnerId == ownerId);
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

        public async Task<Response> GetById(int ownerId, int id)
        {
            try
            {
                var point = await _unitOfWork.Points.FindAsync(p => p.Id == id && p.OwnerId == ownerId);
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

        public async Task<Response> GetPointsCount(int ownerId)
        {
            try
            {
                var points = await _unitOfWork.Points.FindAsync(p => p.OwnerId == ownerId);
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

        public async Task<Response> SearchPointsByCoordinates(int ownerId, double X, double Y, double range)
        {
            try
            {
                var points = await _unitOfWork.Points.FindAsync(p => p.OwnerId == ownerId && Math.Sqrt(Math.Pow(p.X_coordinate - X, 2) + Math.Pow(p.Y_coordinate - Y, 2)) <= range);
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

        public async Task<Response> GetPointsInRadius(int ownerId, double centerX, double centerY, double radius)
        {
            try
            {
                var points = await _unitOfWork.Points.FindAsync(p => p.OwnerId == ownerId && Math.Sqrt(Math.Pow(centerX - p.X_coordinate, 2) + Math.Pow(centerY - p.Y_coordinate, 2)) <= radius);
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

        public async Task<Response> GetNearestPoint(int ownerId, double X, double Y)
        {
            try
            {
                var points = await _unitOfWork.Points.FindAsync(p => p.OwnerId == ownerId);
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

        public async Task<Response> Distance(int ownerId, string pointName1, string pointName2)
        {
            try
            {
                var point1 = await _unitOfWork.Points.FindAsync(p => p.OwnerId == ownerId && p.Name == pointName1);
                var point2 = await _unitOfWork.Points.FindAsync(p => p.OwnerId == ownerId && p.Name == pointName2);

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
        public async Task<Response> GeneratePoints(int ownerId)
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
                        Name = cities[i],
                        Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                        OwnerId = ownerId
                    };
                    newPoints.Add(item);
                }

                var user = await _context.Users.FindAsync(ownerId);
                user.UserPoints.AddRange(newPoints);
                await _unitOfWork.Points.AddRangeAsync(newPoints);
                await _unitOfWork.CommitAsync(); // Commit the transaction
                return _responseService.SuccessResponse(newPoints, "10 Points generated successfully.", true);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb> { }, $"Error generating points: {ex.Message}", false);
            }
        }

        public async Task<Response> Add(int ownerId, PointDb point)
        {
            try
            {
                point.Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                point.OwnerId = ownerId;
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
        public async Task<Response> Update(int ownerId, int id, PointDb updatedPoint)
        {
            try
            {
                var point = await _unitOfWork.Points.FindAsync(p => p.Id == id && p.OwnerId == ownerId);
                if (point.success)
                {
                    var existingPoint = ((List<PointDb>)point.point).FirstOrDefault();
                    if (existingPoint != null)
                    {
                        existingPoint.X_coordinate = updatedPoint.X_coordinate;
                        existingPoint.Y_coordinate = updatedPoint.Y_coordinate;
                        existingPoint.Name = updatedPoint.Name;
                        existingPoint.Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        var result = await _unitOfWork.Points.UpdateAsync(existingPoint);
                        if (result.success)
                        {
                            await _unitOfWork.CommitAsync(); // Commit the transaction
                        }
                        return result;
                    }
                    return _responseService.ErrorResponse(new List<PointDb>(), "Point not found", false);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), point.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error updating point: {ex.Message}", false);
            }
        }

        // DELETE Requests
        public async Task<Response> Delete(int ownerId, int id)
        {
            try
            {
                var point = await _unitOfWork.Points.FindAsync(p => p.Id == id && p.OwnerId == ownerId);
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
                    return _responseService.ErrorResponse(new List<PointDb>(), "Point not found", false);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), point.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error deleting point: {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteById(int ownerId, int id)
        {
            try
            {
                var point = await _unitOfWork.Points.FindAsync(p => p.Id == id && p.OwnerId == ownerId);
                if (point.success)
                {
                    var existingPoint = ((List<PointDb>)point.point).FirstOrDefault();
                    if (existingPoint != null)
                    {
                        var result = await _unitOfWork.Points.DeleteAsync(existingPoint.Id);
                        if (result.success)
                        {
                            await _unitOfWork.CommitAsync(); // Commit the transaction
                            return _responseService.SuccessResponse(new List<PointDb> { existingPoint }, "Point deleted successfully.", true);
                        }
                    }
                    return _responseService.ErrorResponse(new List<PointDb>(), "Point not found", false);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), point.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error deleting point: {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteByName(int ownerId, string name)
        {
            try
            {
                var points = await _unitOfWork.Points.FindAsync(p => p.Name == name && p.OwnerId == ownerId);
                if (points.success)
                {
                    var pointsToRemove = ((List<PointDb>)points.point);
                    if (pointsToRemove.Any())
                    {
                        foreach (var point in pointsToRemove)
                        {
                            await _unitOfWork.Points.DeleteAsync(point.Id);
                        }
                        await _unitOfWork.CommitAsync(); // Commit the transaction
                        return _responseService.SuccessResponse(pointsToRemove, "Points deleted successfully.", true);
                    }
                    return _responseService.ErrorResponse(new List<PointDb>(), "No points found with the specified name.", false);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), points.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error deleting points: {ex.Message}", false);
            }
        }

        public async Task<Response> UpdateByName(int ownerId, string name, PointDb updatedPoint)
        {
            try
            {
                var points = await _unitOfWork.Points.FindAsync(p => p.Name == name && p.OwnerId == ownerId);
                if (points.success)
                {
                    var pointsToUpdate = ((List<PointDb>)points.point);
                    if (pointsToUpdate.Any())
                    {
                        foreach (var point in pointsToUpdate)
                        {
                            point.X_coordinate = updatedPoint.X_coordinate;
                            point.Y_coordinate = updatedPoint.Y_coordinate;
                            point.Name = updatedPoint.Name;
                            point.Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                            await _unitOfWork.Points.UpdateAsync(point);
                        }
                        await _unitOfWork.CommitAsync(); // Commit the transaction
                        return _responseService.SuccessResponse(pointsToUpdate, "Points updated successfully.", true);
                    }
                    return _responseService.ErrorResponse(new List<PointDb>(), "No points found with the specified name.", false);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), points.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error updating points: {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteAll(int ownerId)
        {
            try
            {
                var points = await _unitOfWork.Points.FindAsync(p => p.OwnerId == ownerId);
                if (points.success)
                {
                    var pointsToRemove = ((List<PointDb>)points.point);
                    if (pointsToRemove.Any())
                    {
                        foreach (var point in pointsToRemove)
                        {
                            await _unitOfWork.Points.DeleteAsync(point.Id);
                        }
                        await _unitOfWork.CommitAsync(); // Commit the transaction
                        return _responseService.SuccessResponse(pointsToRemove, "All points deleted successfully.", true);
                    }
                    return _responseService.ErrorResponse(new List<PointDb>(), "No points found.", false);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), points.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error deleting all points: {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteInRange(int ownerId, double minX, double minY, double maxX, double maxY)
        {
            try
            {
                var points = await _unitOfWork.Points.FindAsync(p => p.OwnerId == ownerId &&
                    p.X_coordinate >= minX && p.X_coordinate <= maxX &&
                    p.Y_coordinate >= minY && p.Y_coordinate <= maxY);
                if (points.success)
                {
                    var pointsToRemove = ((List<PointDb>)points.point);
                    if (pointsToRemove.Any())
                    {
                        foreach (var point in pointsToRemove)
                        {
                            await _unitOfWork.Points.DeleteAsync(point.Id);
                        }
                        await _unitOfWork.CommitAsync(); // Commit the transaction
                        return _responseService.SuccessResponse(pointsToRemove, "Points in range deleted successfully.", true);
                    }
                    return _responseService.ErrorResponse(new List<PointDb>(), "No points found in the specified range.", false);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), points.ResponseMessage, false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error deleting points in range: {ex.Message}", false);
            }
        }

        public async Task<Response> GetFeatureById(int ownerId, int featureId)
        {
            try
            {
                var feature = await _context.Features
                    .Where(f => f.FeatureId == featureId && f.OwnerId == ownerId)
                    .FirstOrDefaultAsync();

                if (feature != null)
                {
                    return _responseService.SuccessResponse(new List<PointDb>(), $"Feature with ID {featureId} retrieved successfully.", true);
                }
                return _responseService.ErrorResponse(new List<FeatureDb>(), "Feature not found", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<FeatureDb>(), $"Error retrieving feature: {ex.Message}", false);
            }
        }

        public async Task<Response> UpdateFeatureById(int ownerId, int featureId, FeatureDb updatedFeature)
        {
            try
            {
                var feature = await _context.Features
                    .Where(f => f.FeatureId == featureId && f.OwnerId == ownerId)
                    .FirstOrDefaultAsync();

                if (feature != null)
                {
                    feature.FeatureName = updatedFeature.FeatureName;
                    feature.FeatureData = updatedFeature.FeatureData;
                    feature.createdDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                    _context.Features.Update(feature);
                    await _context.SaveChangesAsync(); // Commit the transaction
                    return _responseService.SuccessResponse(new List<PointDb>(), $"Feature with ID {featureId} updated successfully.", true);
                }
                return _responseService.ErrorResponse(new List<FeatureDb>(), "Feature not found", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<FeatureDb>(), $"Error updating feature: {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteFeatureById(int ownerId, int featureId)
        {
            try
            {
                var feature = await _context.Features
                    .Where(f => f.FeatureId == featureId && f.OwnerId == ownerId)
                    .FirstOrDefaultAsync();

                if (feature != null)
                {
                    _context.Features.Remove(feature);
                    await _context.SaveChangesAsync(); // Commit the transaction
                    return _responseService.SuccessResponse(new List<PointDb>(), $"Feature with ID {featureId} deleted successfully.", true);
                }
                return _responseService.ErrorResponse(new List<FeatureDb>(), "Feature not found", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<FeatureDb>(), $"Error deleting feature: {ex.Message}", false);
            }
        }

        public Task<Response> UpdateFeatureById(int ownerId, int featureId)
        {
            throw new NotImplementedException();
        }
    }
}