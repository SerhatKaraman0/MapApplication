using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapApplication.Interfaces;
using MapApplication.Models;
using MapApplication.Data;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Reflection;

namespace MapApplication.Services
{
    public class PointService : IPointService
    {
        private readonly IResponseService _responseService;
        private readonly AppDbContext _context;
        private readonly IDatabaseOperationsService _databaseService;

        public PointService(AppDbContext context, IResponseService responseService, IDatabaseOperationsService databaseOperationsService)
        {
            _responseService = responseService;
            _context = context;
            _databaseService = databaseOperationsService;
        }

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

                var points = new List<PointDb>();

                foreach (var city in cities)
                {
                    var item = new PointDb()
                    {
                        X_coordinate = rnd.NextDouble() * 99999,  // X as double
                        Y_coordinate = rnd.NextDouble() * 99999,  // Y as double
                        Name = city
                    };
                    var response = await _databaseService.Create(item);

                    if (response.success)
                    {
                        points.Add(item);
                    }
                    Console.WriteLine($"Failed to create point for point: {response.ResponseMessage}");
                }

                return _responseService.SuccessResponse(points, "Points generated successfully.", true);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error generating points: {ex.Message}", false);
            }
        }


        public async Task<Response> Add(PointDb point)
        {
            try
            {
                var response = await _databaseService.Create(point);
                if (response.success)
                {
                    return _responseService.SuccessResponse(new List<PointDb> { point }, "Point added successfully.", true);
                }
                Console.WriteLine($"Failed to create point for point: {response.ResponseMessage}");
                return _responseService.ErrorResponse(new List<PointDb>(), "Failed to add point.", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error adding point: {ex.Message}", false);
            }
        }

        public async Task<Response> Update(int id, PointDb updatedPoint)
        {
            try
            {
                var response = await _databaseService.Update(id, updatedPoint);
                if (response.success)
                {
                    return _responseService.SuccessResponse(new List<PointDb> { updatedPoint }, "Point updated successfully.", true);
                }
                Console.WriteLine($"Failed to create point for {id}: {response.ResponseMessage}");
                return _responseService.ErrorResponse(new List<PointDb>(), $"Failed to update point with id {id}.", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error updating point: {ex.Message}", false);
            }
        }

        public async Task<Response> UpdateByName(string name, PointDb updatedPoint)
        {
            try
            {
                var response = await _databaseService.UpdateByName(name, updatedPoint);
                if (response.success)
                {
                    return _responseService.SuccessResponse(new List<PointDb> { updatedPoint }, "Point updated successfully.", true);
                }
                Console.WriteLine($"Failed to create point for {name}: {response.ResponseMessage}");
                return _responseService.ErrorResponse(new List<PointDb>(), $"Failed to update point with name {name}.", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error updating point: {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteById(int id)
        {
            try
            {
                var response = await _databaseService.Delete(id);
                if (response.success)
                {
                    return _responseService.SuccessResponse(null, $"Success deleting the point with id: {id}", true);
                }
                Console.WriteLine($"Failed to create point for {id}: {response.ResponseMessage}");
                return _responseService.ErrorResponse(new List<PointDb>(), $"Can't find the point with id: {id}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Problem occurred when deleting the point with id: {id} with Error message {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteByName(string name)
        {
            try
            {
                var response = await _databaseService.DeleteByName(name);
                if (response.success)
                {
                    return _responseService.SuccessResponse(null, $"Success deleting the point with name: {name}", true);
                }
                Console.WriteLine($"Failed to create point for : {response.ResponseMessage}");
                return _responseService.ErrorResponse(new List<PointDb>(), $"Can't find the point with name: {name}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Problem occurred when deleting the point with name: {name} with Error message {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteAll()
        {
            try
            {
                var response = await _databaseService.DeleteAll();
                if (response.success)
                {
                    return _responseService.SuccessResponse(null, "Success removing all data from database", true);
                }
                Console.WriteLine($"Failed to create point for : {response.ResponseMessage}");
                return _responseService.ErrorResponse(new List<PointDb>(), "Error deleting points", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error deleting points: {ex.Message}", false);
            }
        }


        public async Task<Response> GetAll()
        {
            try
            {
                var response = await _databaseService.SelectAll();
                if (response.success)
                {
                    return _responseService.SuccessResponse(response.point, "Points retrieved successfully", true);
                }
                Console.WriteLine($"Failed to create point for: {response.ResponseMessage}");
                return _responseService.ErrorResponse(new List<PointDb>(), response.ResponseMessage, false);
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
                var response = await _databaseService.FindById(id);
                if (response.success)
                {
                    return _responseService.SuccessResponse(response.point, $"Point with id: {id} retrieved successfully", true);
                }
                Console.WriteLine($"Failed to create point for {id}: {response.ResponseMessage}");
                return _responseService.ErrorResponse(new List<PointDb>(), $"Can't find point with the id: {id}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"Error fetching point: {ex.Message}", false);
            }
        }

        public async Task<Response> GetPointsCount()
        {
            try
            {
                var response = await _databaseService.Count();
                if (response.success)
                {
                    return _responseService.SuccessResponse(null, response.ResponseMessage, true);
                }
                Console.WriteLine($"Failed to create point for : {response.ResponseMessage}");
                return _responseService.ErrorResponse(new List<PointDb>(), "Error fetching points count.", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb> { }, $"Error fetching points count: {ex.Message}", false);
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


        ///
        ///
        /// EXTRAS
        /// 
        ///

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

        public Task<Response> DeleteInRange(double minX, double minY, double max_X, double maxY)
        {
            throw new NotImplementedException();
        }
    }
}



