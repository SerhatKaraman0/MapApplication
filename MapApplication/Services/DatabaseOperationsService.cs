using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MapApplication.Models;
using MapApplication.Interfaces;
using MapApplication.Data;

namespace MapApplication.Services
{
    public class DatabaseOperationsService : IDatabaseOperationsService
    {
        private readonly IResponseService _responseService;
        private readonly string _connectionString;

        public DatabaseOperationsService(IResponseService responseService, IConfiguration configuration)
        {
            _responseService = responseService;
            _connectionString = configuration.GetConnectionString("WebApiDatabase");
        }

        public async Task<Response> SelectAll()
        {
            try
            {
                await using var dataSource = NpgsqlDataSource.Create(_connectionString);
                await using var command = dataSource.CreateCommand("SELECT * FROM points");
                await using var reader = await command.ExecuteReaderAsync();

                var points = new List<PointDb>();

                while (await reader.ReadAsync())
                {
                    var point = new PointDb
                    {
                        Id = reader.GetInt32(0),
                        X_coordinate = reader.GetDouble(1),
                        Y_coordinate = reader.GetDouble(2),
                        Name = reader.GetString(3)
                    };

                    points.Add(point);
                }

                return _responseService.SuccessResponse(points, "Points retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"An error occurred: {ex.Message}", false);
            }
        }

        public async Task<Response> FindById(int id)
        {
            try
            {
                await using var dataSource = NpgsqlDataSource.Create(_connectionString);
                await using var command = dataSource.CreateCommand("SELECT * FROM points WHERE \"Id\" = @p0");
                command.Parameters.AddWithValue("p0", id);
                await using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var point = new PointDb
                    {
                        Id = reader.GetInt32(0),
                        X_coordinate = reader.GetDouble(1),
                        Y_coordinate = reader.GetDouble(2),
                        Name = reader.GetString(3)
                    };

                    return _responseService.SuccessResponse(new List<PointDb> { point }, "Point retrieved successfully.", true);
                }

                return _responseService.ErrorResponse(new List<PointDb>(), $"No point found with id: {id}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"An error occurred: {ex.Message}", false);
            }
        }

        public async Task<Response> FindByName(string name)
        {
            try
            {
                await using var dataSource = NpgsqlDataSource.Create(_connectionString);
                await using var command = dataSource.CreateCommand("SELECT * FROM points WHERE \"Name\" = @p0");
                command.Parameters.AddWithValue("p0", name);
                await using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var point = new PointDb
                    {
                        Id = reader.GetInt32(0),
                        X_coordinate = reader.GetDouble(1),
                        Y_coordinate = reader.GetDouble(2),
                        Name = reader.GetString(3)
                    };

                    return _responseService.SuccessResponse(new List<PointDb> { point }, $"Point retrieved successfully with name: {name}", true);
                }

                return _responseService.ErrorResponse(new List<PointDb>(), $"No point found with name: {name}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"An error occurred: {ex.Message}", false);
            }
        }

        public async Task<Response> Create(PointDb point)
        {
            try
            {
                Console.WriteLine(_connectionString);
                await using var dataSource = NpgsqlDataSource.Create(_connectionString);
                await using var command = dataSource.CreateCommand("INSERT INTO points (\"X_coordinate\", \"Y_coordinate\", \"Name\") VALUES (@p0, @p1, @p2)");
                command.Parameters.AddWithValue("p0", point.X_coordinate);
                command.Parameters.AddWithValue("p1", point.Y_coordinate);
                command.Parameters.AddWithValue("p2", point.Name);

                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    return _responseService.SuccessResponse(new List<PointDb> { point }, "Point created successfully.", true);
                }

                return _responseService.ErrorResponse(new List<PointDb>(), "Failed to create point.", false);
            }
            catch (PostgresException ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"PostgreSQL error: {ex.Message}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"An error occurred: {ex.Message}", false);
            }
        }

        public async Task<Response> Delete(int id)
        {
            try
            {
                await using var dataSource = NpgsqlDataSource.Create(_connectionString);
                await using var command = dataSource.CreateCommand("DELETE FROM points WHERE \"Id\" = @p0");
                command.Parameters.AddWithValue("p0", id);

                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    return _responseService.SuccessResponse(new List<PointDb>(), "Point deleted successfully.", true);
                }

                return _responseService.ErrorResponse(new List<PointDb>(), $"Can't find point with id: {id} to delete", false);
            }
            catch (PostgresException ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"PostgreSQL error: {ex.Message}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"An error occurred: {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteByName(string name)
        {
            try
            {
                await using var dataSource = NpgsqlDataSource.Create(_connectionString);
                await using var command = dataSource.CreateCommand("DELETE FROM points WHERE \"Name\" = @p0");
                command.Parameters.AddWithValue("p0", name);

                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    return _responseService.SuccessResponse(new List<PointDb>(), "Point deleted successfully.", true);
                }

                return _responseService.ErrorResponse(new List<PointDb>(), $"Can't find point with name: {name} to delete", false);
            }
            catch (PostgresException ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"PostgreSQL error: {ex.Message}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"An error occurred: {ex.Message}", false);
            }
        }

        public async Task<Response> DeleteAll()
        {
            try
            {
                await using var dataSource = NpgsqlDataSource.Create(_connectionString);
                await using var command = dataSource.CreateCommand("DELETE FROM points");

                var rowsAffected = await command.ExecuteNonQueryAsync();

                return _responseService.SuccessResponse(null, $"{rowsAffected} points deleted successfully.", true);
            }
            catch (PostgresException ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"PostgreSQL error: {ex.Message}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"An error occurred: {ex.Message}", false);
            }
        }

        public async Task<Response> Update(int id, PointDb point)
        {
            try
            {
                await using var dataSource = NpgsqlDataSource.Create(_connectionString);
                await using var command = dataSource.CreateCommand("UPDATE points SET \"X_coordinate\" = @p0, \"Y_coordinate\" = @p1, \"Name\" = @p2 WHERE Id = @p3");
                command.Parameters.AddWithValue("p0", point.X_coordinate);
                command.Parameters.AddWithValue("p1", point.Y_coordinate);
                command.Parameters.AddWithValue("p2", point.Name);
                command.Parameters.AddWithValue("p3", id);

                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    return _responseService.SuccessResponse(new List<PointDb> { point }, "Point updated successfully.", true);
                }

                return _responseService.ErrorResponse(new List<PointDb>(), $"Can't find point with id: {id}", false);
            }
            catch (PostgresException ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"PostgreSQL error: {ex.Message}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"An error occurred: {ex.Message}", false);
            }
        }

        public async Task<Response> UpdateByName(string name, PointDb point)
        {
            try
            {
                await using var dataSource = NpgsqlDataSource.Create(_connectionString);
                await using var command = dataSource.CreateCommand("UPDATE points SET \"X_coordinate\" = @p0, \"Y_coordinate\" = @p1, \"Name\" = @p2 WHERE \"Name\" = @p3");
                command.Parameters.AddWithValue("p0", point.X_coordinate);
                command.Parameters.AddWithValue("p1", point.Y_coordinate);
                command.Parameters.AddWithValue("p2", point.Name);
                command.Parameters.AddWithValue("p3", name);

                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    return _responseService.SuccessResponse(new List<PointDb> { point }, "Point updated successfully.", true);
                }

                return _responseService.ErrorResponse(new List<PointDb>(), $"Can't find point with name: {name}", false);
            }
            catch (PostgresException ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"PostgreSQL error: {ex.Message}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"An error occurred: {ex.Message}", false);
            }
        }

        public async Task<Response> Count()
        {
            try
            {
                await using var dataSource = NpgsqlDataSource.Create(_connectionString);
                await using var command = dataSource.CreateCommand("SELECT COUNT(*) FROM points");

                var count = (long)await command.ExecuteScalarAsync();

                return _responseService.SuccessResponse(new List<PointDb>(), $"{count} points found.", true);
            }
            catch (PostgresException ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"PostgreSQL error: {ex.Message}", false);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<PointDb>(), $"An error occurred: {ex.Message}", false);
            }
        }
    }
}
