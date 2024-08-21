using System;
using MapApplication.Data;
using MapApplication.Interfaces;
using MapApplication.Models;

namespace MapApplication.Services
{
    public class WktResponseService : IWktResponseService
    {
        public WktResponse ErrorResponse(List<WktDb> wkt, string ResponseMessage, bool success)
        {
            return new WktResponse
            {
                wkt = wkt,
                ResponseMessage = ResponseMessage,
                success = false
            };
        }

        public WktResponse SuccessResponse(List<WktDb> wkt, string ResponseMessage, bool success)
        {
            return new WktResponse
            {
                wkt = wkt,
                ResponseMessage = ResponseMessage,
                success = true
            };
        }
    }
}

