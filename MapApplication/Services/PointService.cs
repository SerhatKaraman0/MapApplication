using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapApplication.Interfaces;
using MapApplication.Models;
using MapApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace MapApplication.Services
{
    public class PointService : IPointService
    {
        private readonly IResponseService _responseService;
        private readonly AppDbContext _context;

        public PointService(AppDbContext context, IResponseService responseService)
        {
            _responseService = responseService;
            _context = context;
        }

        // GET Requests
        public async Task<Response> GetAll()
        {
            try
            {
                var points = await _context.Points.ToListAsync();
                return _responseService.SuccessResponse(points, "Points retrieved successfully", true);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), "Error fetching points", false);
            }
        }

        public async Task<Response> GetById(int id)
        {
            try
            {
                var point = await _context.Points.FindAsync(id);
                if (point != null)
                {
                    return _responseService.SuccessResponse(new List<PointDb> { point }, $"Point with id: {id} retrieved successfully", true);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), $"Can't find point with the id: {id}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Something went wrong with error {ex.Message}", false);
            }
        }

        public async Task<Response> GetPointsCount()
        {
            try
            {
                var pointsCount = await _context.Points.CountAsync();
                return _responseService.SuccessResponse(new List<PointDb> { }, $"{pointsCount} points retrieved successfully", true);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb> { }, "An error occurred", false);
            }
        }

        public async Task<Response> SearchPointsByCoordinates(double X, double Y, double range)
        {
            try
            {
                var pointList = await _context.Points
                    .Where(p => Math.Sqrt(Math.Pow(p.X_coordinate - X, 2) + Math.Pow(p.Y_coordinate - Y, 2)) <= range)
                    .ToListAsync();

                if (pointList.Any())
                {
                    return _responseService.SuccessResponse(pointList, $"{pointList.Count} points in range returned successfully.", true);
                }
                return _responseService.ErrorResponse(new List<PointDb> { }, "Couldn't find points in range.", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb> { }, $"Error retrieving points: {ex.Message}", false);
            }
        }

        public async Task<Response> GetPointsInRadius(double centerX, double centerY, double radius)
        {
            try
            {
                var points = await _context.Points
                    .Where(p => Math.Sqrt(Math.Pow(centerX - p.X_coordinate, 2) + Math.Pow(centerY - p.Y_coordinate, 2)) <= radius)
                    .ToListAsync();
                if (points != null)
                {
                    return _responseService.SuccessResponse(points, $"{points.Count} Points in radius {radius} to {centerX}, {centerY} retrieved successfully", true);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), $"Can't find Points in radius {radius} to {centerX}, {centerY}", false);

            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error occured while retrieving points with error message {ex.Message}", false);
            }
        }

        public async Task<Response> GetNearestPoint(double X, double Y)
        {
            try
            {
                var nearestPoint = await _context.Points.OrderBy(p => Math.Sqrt(Math.Pow(X - p.X_coordinate, 2) + Math.Pow(Y - p.Y_coordinate, 2)))
                                                    .FirstOrDefaultAsync();
                if (nearestPoint != null)
                {
                    return _responseService.SuccessResponse(new List<PointDb>(), $"Successfully returned nearest point to {X}, {Y} is {nearestPoint.X_coordinate}, {nearestPoint.Y_coordinate}", true);
                }

                return _responseService.ErrorResponse(new List<PointDb>(), "No point found", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error retrieving points with error message {ex.Message}", false);
            }
        }

        public async Task<Response> Distance(string pointName1, string pointName2)
        {
            try
            {
                var p1 = await _context.Points.FirstOrDefaultAsync(p => p.Name == pointName1);
                var p2 = await _context.Points.FirstOrDefaultAsync(p => p.Name == pointName2);

                if (p1 != null && p2 != null)
                {
                    var distance = Math.Sqrt(Math.Pow(p1.X_coordinate - p2.X_coordinate, 2) + Math.Pow(p1.Y_coordinate - p2.Y_coordinate, 2));
                    return _responseService.SuccessResponse(new List<PointDb> { p1, p2 }, $"Distance between {p1.Name} and {p2.Name} is {distance}", true);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), "Error occurred calculating distance", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error {ex.Message} occurred calculating distance", false);
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

                for (int i = 0; i < 10; i++)
                {
                    var item = new Data.PointDb()
                    {
                        X_coordinate = rnd.Next(1, 99999),
                        Y_coordinate = rnd.Next(1, 99999),
                        Name = cities[i]
                    };
                    _context.Points.Add(item);
                }
                await _context.SaveChangesAsync();

                var points = await _context.Points.ToListAsync();
                return _responseService.SuccessResponse(points, "Points generated successfully.", true);
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
                await _context.Points.AddAsync(point);
                await _context.SaveChangesAsync();
                var pointList = new List<PointDb> { point };
                return _responseService.SuccessResponse(pointList, "Point added successfully.", true);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb> { }, $"Error adding point: {ex.Message}", false);
            }
        }

        // PUT Requests
        public async Task<Response> Update(int id, PointDb updatedPoint)
        {
            try
            {
                var point = await _context.Points.FindAsync(id);
                if (point != null)
                {
                    point.X_coordinate = updatedPoint.X_coordinate;
                    point.Y_coordinate = updatedPoint.Y_coordinate;
                    point.Name = updatedPoint.Name;
                    await _context.SaveChangesAsync();
                    var pointList = new List<PointDb> { point };
                    return _responseService.SuccessResponse(pointList, "Point updated successfully.", true);
                }
                return _responseService.ErrorResponse(new List<PointDb> { }, $"Point with id {id} not found.", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb> { }, $"Error updating point: {ex.Message}", false);
            }
        }

        public async Task<Response> UpdateByName(string Name, PointDb updatedPoint)
        {
            try
            {
                var point = await _context.Points.FirstOrDefaultAsync(p => p.Name == Name);
                if (point != null)
                {
                    point.X_coordinate = updatedPoint.X_coordinate;
                    point.Y_coordinate = updatedPoint.Y_coordinate;
                    point.Name = updatedPoint.Name;
                    await _context.SaveChangesAsync();
                    var pointList = new List<PointDb> { point };
                    return _responseService.SuccessResponse(pointList, "Point updated successfully.", true);
                }
                return _responseService.ErrorResponse(new List<PointDb> { }, $"Point with name {Name} not found.", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb> { }, $"Error updating point: {ex.Message}", false);
            }
        }

        // DELETE Requests
        public async Task<Response> DeleteById(int id)
        {
            try
            {
                var point = await _context.Points.FindAsync(id);
                if (point != null)
                {
                    _context.Points.Remove(point);
                    await _context.SaveChangesAsync();
                    var pointsList = new List<PointDb> { point };
                    return _responseService.SuccessResponse(pointsList, $"Success deleting the point with id: {id}", true);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), $"Can't find the point with id: {id}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Problem occured when deleting the point with id: {id} with Error message {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteByName(string name)
        {
            try
            {
                var point = await _context.Points.FirstOrDefaultAsync(p => p.Name == name);
                if (point != null)
                {
                    _context.Points.Remove(point);
                    await _context.SaveChangesAsync();
                    var pointsList = new List<PointDb> { point };
                    return _responseService.SuccessResponse(pointsList, $"Success deleting the point with name: {name}", true);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), $"Can't find the point with name: {name}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Problem occured when deleting the point with name: {name} with Error message {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteAll()
        {
            try
            {
                var points = _context.Points;

                if (points != null)
                {
                    _context.Points.RemoveRange(points);
                    await _context.SaveChangesAsync();
                    return _responseService.SuccessResponse(new List<PointDb>(), "Success removing all data from database", true);
                }

                return _responseService.ErrorResponse(new List<PointDb>(), "Error deleting points", false);

            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), "Error deleting points", false);
            }
        }

        public async Task<Response> DeleteInRange(double minX, double minY, double max_X, double maxY)
        {
            try
            {
                var points = await _context.Points.Where(p => (minX <= p.X_coordinate) && (minY <= p.Y_coordinate) && (p.X_coordinate <= max_X) && (p.Y_coordinate <= maxY)).ToListAsync();
                var initialLength = points.Count;
                if (points != null)
                {
                    _context.Points.RemoveRange(points);
                    await _context.SaveChangesAsync();
                    var remainingLength = points.Count;
                    var resLength = initialLength - remainingLength;
                    return _responseService.SuccessResponse(new List<PointDb>(), $"{remainingLength} Points deleted successfully", true);
                }
                return _responseService.ErrorResponse(new List<PointDb>(), "Error deleting points", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Something went wrong with error message {ex.Message}", false);
            }
        }
    }
}

