using System;
using MapApplication.Data;
using MapApplication.Models;

namespace MapApplication.Interfaces
{
	public interface IFeatureResponseService
	{
        FeatureResponse ErrorResponse(List<FeatureDb> features, string v1, bool v2);
        FeatureResponse SuccessResponse(List<FeatureDb> features, string v1, bool v2);
    }
}

