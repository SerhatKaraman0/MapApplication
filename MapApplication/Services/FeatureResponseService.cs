using System;
using MapApplication.Data;
using MapApplication.Interfaces;
using MapApplication.Models;

namespace MapApplication.Services
{
	public class FeatureResponseService: IFeatureResponseService
	{
		public FeatureResponse ErrorResponse(List<FeatureDb> features, string responseMessage, bool success)
		{
			return new FeatureResponse
			{
				features = features,
				ResponseMessage = responseMessage,
				success = false
			};
		}

        public FeatureResponse SuccessResponse(List<FeatureDb> features, string responseMessage, bool success)
        {
            return new FeatureResponse
            {
                features = features,
                ResponseMessage = responseMessage,
                success = true
            };
        }
    }
}
