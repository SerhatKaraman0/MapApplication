using System;
using System.Collections.Generic;
using MapApplication.Data;
using MapApplication.Interfaces;
using MapApplication.Models;

namespace MapApplication.Services
{
    public class ResponseService : IResponseService
    {
        // Error Response for specific PointDb type
        public Response ErrorResponse(List<PointDb> points, string responseMessage, bool success)
        {
            return new Response
            {
                point = points,
                ResponseMessage = responseMessage,
                success = success
            };
        }

        // Success Response for specific PointDb type
        public Response SuccessResponse(List<PointDb> points, string responseMessage, bool success)
        {
            return new Response
            {
                point = points,
                ResponseMessage = responseMessage,
                success = success
            };
        }

        // Generic Error Response
        public Response ErrorResponse<T>(List<T> points, string responseMessage, bool success)
        {
            // Convert List<T> to List<PointDb> for Response compatibility
            var pointDbList = points.OfType<PointDb>().ToList();
            return new Response
            {
                point = pointDbList,
                ResponseMessage = responseMessage,
                success = success
            };
        }

        // Generic Success Response
        public Response SuccessResponse<T>(List<T> points, string responseMessage, bool success)
        {
            // Convert List<T> to List<PointDb> for Response compatibility
            var pointDbList = points.OfType<PointDb>().ToList();
            return new Response
            {
                point = pointDbList,
                ResponseMessage = responseMessage,
                success = success
            };
        }
    }
}
