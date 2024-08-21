using System;
using MapApplication.Models;
using MapApplication.Data;

namespace MapApplication.Interfaces
{
	public interface IWktResponseService
	{
		WktResponse ErrorResponse(List<WktDb> wkt, string ResponseMessage, bool success);
        WktResponse SuccessResponse(List<WktDb> wkt, string ResponseMessage, bool success);
    }
}

