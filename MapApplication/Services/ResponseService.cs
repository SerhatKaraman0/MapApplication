using System;
using System.Collections.Generic;
using System.Linq;
using MapApplication.Interfaces;
using MapApplication.Models;
using Microsoft.AspNetCore.Mvc;
using MapApplication.Data;

namespace MapApplication.Services
{
	public class ResponseService: IResponseService
	{
		public Response ErrorResponse(List<PointDb> point, string ResponseMessage, bool success) {
			Response response = new Response();
			response.point = point;
			response.ResponseMessage = ResponseMessage;
			response.success = false;

			return response;
		}

        public Response SuccessResponse(List<PointDb> point, string ResponseMessage, bool success)
        {
            Response response = new Response();
            response.point = point;
            response.ResponseMessage = ResponseMessage;
            response.success = true;

            return response;
        }


    }
}

