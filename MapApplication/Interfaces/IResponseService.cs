using System;
using MapApplication.Models;
using MapApplication.Data;

namespace MapApplication.Interfaces
{
	public interface IResponseService
	{
        Response ErrorResponse(List<PointDb> points, string v1, bool v2);
        Response SuccessResponse(List<PointDb> points, string v1, bool v2);
    }
}

