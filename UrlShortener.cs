using System.Linq;
using RestSharp;
using System.Net;
using ShortenUrl.Models;

namespace ShortenUrl
{
    public class UrlShortener
    {
        private readonly string _apiKey;
        private readonly string _uri;

        private const string DefaultUri = "https://www.googleapis.com/urlshortener/v1/url";

        public UrlShortener(string apiKey, string uri = DefaultUri)
        {
            _apiKey = apiKey;
            _uri = uri;
        }

        public ApiResponse Shorten(string longUrl)
        {
            var uri = _uri + "?key=" + _apiKey;
            var client = new RestClient(uri);
            var request = new RestRequest { Method = Method.POST };
            var content = new { longUrl };
            request.AddJsonBody(content);
            var response = client.Execute<ApiResponse>(request);


            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new ApiResponse
                {
                    Ok = true,

                    Kind = response.Data.Kind,
                    Id = response.Data.Id,
                    LongUrl = response.Data.LongUrl
                    //Status = jObject.Value<string>("status")
                };
            }

            var error = response.Data.Error;
            var errors = response.Data.Error.Errors;

            return new ApiResponse
            {
                Ok = false,
                Error = new Error
                {
                    Errors = errors.Select(x => new ErrorDescription
                    {
                        Domain = x.Domain,
                        Reason = x.Reason,
                        Message = x.Message,
                        Location = x.Location,
                        LocationType = x.LocationType
                    }).ToArray(),
                    Code = error.Code,
                    Message = error.Message
                }
            };
        }

        public ApiResponse Tracker(string shortUrl)
        {
            //https://www.googleapis.com/urlshortener/v1/url?shortUrl=https://goo.gl/L2NRzo&projection=FULL&key=AIzaSyB_CuchBvjq9C2AcbSj3r3EIIm1dAIyHIA

            var uri = _uri + "?shortUrl=" + shortUrl + "&projection=FULL&key=" + _apiKey;
            var client = new RestClient(uri);
            var request = new RestRequest { Method = Method.GET };
            var response = client.Execute<ApiResponse>(request);


            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
                //    new ApiResponse
                //{
                //    Ok = true,
                //    Kind = response.Data.Kind,
                //    Id = response.Data.Id,
                //    LongUrl = response.Data.LongUrl,
                //    Status = response.Data.Status
                //};
            }

            var error = response.Data.Error;
            var errors = response.Data.Error.Errors;

            return new ApiResponse
            {
                Ok = false,
                Error = new Error
                {
                    Errors = errors.Select(x => new ErrorDescription
                    {
                        Domain = x.Domain,
                        Reason = x.Reason,
                        Message = x.Message,
                        Location = x.Location,
                        LocationType = x.LocationType
                    }).ToArray(),
                    Code = error.Code,
                    Message = error.Message
                }
            };
        }

    }
}
