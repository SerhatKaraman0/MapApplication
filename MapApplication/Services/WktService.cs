using System;
using System.Threading.Tasks;
using MapApplication.Interfaces;
using MapApplication.Models;
using MapApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace MapApplication.Services
{
    public class WktService : IWktService
    {
        private readonly IWktResponseService _responseService;
        private readonly AppDbContext _context;

        public WktService(AppDbContext context, IWktResponseService responseService)
        {
            _context = context;
            _responseService = responseService;
        }

        public  async Task<WktResponse> CreateWkt(int ownerId, WktDb wkt)
        {
            try
            {
                wkt.Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                await _context.Wkt.AddAsync(wkt);
                await _context.SaveChangesAsync();
                return _responseService.SuccessResponse(new List<WktDb> { wkt }, "Wkt retrieved successfully", true);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<WktDb> { }, ex.Message, false);
            }
        }

        public Task<WktResponse> DeleteFeatureById(int ownerId, int featureId)
        {
            throw new NotImplementedException();
        }

        public async Task<WktResponse> DeleteWktById(int ownerId, int id)
        {
            try
            {
                var wkt = await _context.Wkt.FindAsync(id);
                if (wkt == null)
                {
                    return _responseService.ErrorResponse(new List<WktDb> {  }, "Wkt not found", false);
                }

                _context.Wkt.Remove(wkt);
                await _context.SaveChangesAsync();

                return _responseService.SuccessResponse(new List<WktDb> { wkt }, "Wkt retrieved successfully", true);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<WktDb> {  }, ex.Message, false);
            }
        }

        public async Task<WktResponse> GetAllWkt(int ownerId)
        {
            try
            {
                var wktList = await _context.Wkt.ToListAsync();
                return _responseService.SuccessResponse(wktList, "Wkt retrieved successfully", true);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<WktDb> { }, ex.Message, false);
            }
        }

        public Task<WktResponse> GetFeatureById(int ownerId, int featureId)
        {
            throw new NotImplementedException();
        }

        public async Task<WktResponse> GetWktById(int ownerId, int id)
        {
            try
            {
                var wkt = await _context.Wkt.FindAsync(id);
                if (wkt == null)
                {
                    return _responseService.ErrorResponse(new List<WktDb> { }, "Wkt not found", false);
                }

                return _responseService.SuccessResponse(new List<WktDb> { wkt }, "Wkt retrieved successfully", true);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<WktDb> { }, ex.Message, false);
            }
        }

        public Task<WktResponse> UpdateFeatureById(int ownerId, int featureId)
        {
            throw new NotImplementedException();
        }

        public async Task<WktResponse> UpdateWkt(int ownerId, int id, WktDb updatedWkt)
        {
            try
            {
                var wkt = await _context.Wkt.FindAsync(id);
                if (wkt == null)
                {
                    return _responseService.ErrorResponse(new List<WktDb> { }, "Wkt not found", false);
                }

                wkt.Name = updatedWkt.Name;
                wkt.Description = updatedWkt.Description;
                wkt.WKT = updatedWkt.WKT;
                wkt.PhotoLocation = updatedWkt.PhotoLocation;
                wkt.Color = updatedWkt.Color;
                wkt.Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                _context.Wkt.Update(wkt);
                await _context.SaveChangesAsync();

                return _responseService.SuccessResponse(new List<WktDb> { wkt }, "Wkt retrieved successfully", true);
            }
            catch (Exception ex)
            {
                return _responseService.ErrorResponse(new List<WktDb> { }, ex.Message, false);
            }
        }
    }
}

