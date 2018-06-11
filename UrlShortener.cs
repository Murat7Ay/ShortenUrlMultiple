using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using RestSharp;

namespace ShortenUrl
{
    /// <summary>
    /// This class has two methods
    /// It makes if your text message has urls than convert them shorten urls like goo.gl/.... OR
    /// It tracks your shorten Urls statistics
    /// </summary>
    public class UrlShortener
    {
        private static UrlShortener _instance;

        private const string ApiKey = "------";
        
        private static readonly RestClient Client = new RestClient("https://www.googleapis.com/urlshortener/v1/url");
        private static readonly CultureInfo Culture = new CultureInfo("en-US");


        private UrlShortener()
        {
            
        }

        public static UrlShortener Instance
        {
            get { return _instance ?? (_instance = new UrlShortener()); }
        }

        private static string Shorten(string longUrl)
        {
            var request = new RestRequest("?key=" + ApiKey, Method.POST);
            request.AddJsonBody(new { longUrl });
            var response = Client.Execute<ApiResponse>(request);
            return response.StatusCode == HttpStatusCode.OK ? response.Data.Id : null;
        }
        /// <summary>
        /// All track info
        /// </summary>
        /// <param name="shortUrl"></param>
        /// <returns></returns>
        public AllTime Tracker(string shortUrl)
        {
            var request = new RestRequest("?shortUrl=" + shortUrl + "&projection=FULL&key=" + ApiKey, Method.GET);
            var response = Client.Execute<ApiResponse>(request);
            return response.StatusCode == HttpStatusCode.OK ? response.Data.analytics.allTime : null;
        }

        /// <summary>
        /// <para>Track your shorten urls clicks</para>
        /// <para>All time shorten url clicks count or null if error occurs</para>
        /// </summary>
        /// <param name="shortUrl">Short url like goo.gl/.....</param>
        public string TrackerAllTimeShortUrlClicks(string shortUrl)
        {
            var request = new RestRequest("?shortUrl=" + shortUrl + "&projection=FULL&key=" + ApiKey, Method.GET);
            var response = Client.Execute<ApiResponse>(request);

            return response.StatusCode == HttpStatusCode.OK ? response.Data.analytics.allTime.shortUrlClicks : null;
        }
        /// <summary>
        /// <para>It finds the URLs in the message text and converts it into short URLs.</para>
        /// <para>If the message has no URLs returns the message</para>
        /// <para>(Item1=>true if messageText has urls and successfully convert to shorten or messageText has no urls, false =>messageText's urls can not convert to shorten, orginal text returns)</para>
        /// <para>(Item2 => message with no urls or message with shorten urls, if key is false message will be error)</para>
        /// <para>(Item3 => List of key value pair Key is long url, value is shorten url, Check if it is null)</para>
        /// </summary>
        /// <param name="messageText">Message content</param>
        public Tuple<bool,string,List<KeyValuePair<string,string>>> ConvertToShortenUrl(string messageText)
        {
            //var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //var linkParser = new Regex(@"(?:(?:https?|ftp|file)::?\/\/|www\.|ftp\.)(?:\([-A-Z0-9+&@#\/%=~_|$?!:,.]*\)|[-A-Z0-9+&@#\/%=~_|$?!:,.])*(?:\([-A-Z0-9+&@#\/%=~_|$?!:,.]*\)|[A-Z0-9+&@#\/%=~_|$])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //var linkParser = new Regex(@"(?:(?:https?|ftp|file):\/\/?)(?:\([-A-Z0-9+&@#/%=~_|$?!:,.]*\)|[-A-Z0-9+&@#/%=~_|$?!:,.])*(?:\([-A-Z0-9+&@#/%=~_|$?!:,.]*\)|[A-Z0-9+&@#/%=~_|$])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var linkParser = new Regex(@"(?:(?:https?|ftp):\/\/|\b(?:[a-z\d]+\.))(?:(?:[^\s()<>]+|\((?:[^\s()<>]+|(?:\([^\s()<>]+\)))?\))+(?:\((?:[^\s()<>]+|(?:\(?:[^\s()<>]+\)))?\)|[^\s`!()\[\]{};:'"".,<>?«»“”‘’]))?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //var linkParser = new Regex(@"(?i)\b((?:[a-z][\w-]+:(?:/{1,3}|[a-z0-9%])|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'"".,<>?«»“”‘’]))");
            //var linkParser = new Regex(@"^(https?|ftp)://[^\s/$.?#].[^\s]*$");
            //var linkParser = new Regex(@"^(?:(http[s]?|ftp[s]):\/\/)?([^:\/\s]+)(:[0-9]+)?((?:\/\w+)*\/)([\w\-\.]+[^#?\s]+)([^#\s]*)?(#[\w\-]+)?");
            //var linkParser = new Regex(@"[(http(s)?):\/\/?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)");
            var orginalText = messageText;
            var matchesArray = linkParser.Matches(messageText).Cast<Match>().Where(x => Culture.CompareInfo.IndexOf(x.Value, "telekurye", CompareOptions.IgnoreCase) >= 0)
                .Select(s => s.Value).ToArray();
            
            var keyValueList = new List<KeyValuePair<string,string>>();

            if (matchesArray.Length == 0)
                return new Tuple<bool, string, List<KeyValuePair<string, string>>>(false,messageText, null);

            foreach (var match in matchesArray)
            {
                var shortenUrl = Shorten(match);

                if (!string.IsNullOrEmpty(shortenUrl))
                {
                    keyValueList.Add(new KeyValuePair<string, string>(match, shortenUrl));
                    messageText = messageText.Replace(match, shortenUrl);
                }
                else
                    return new Tuple<bool, string, List<KeyValuePair<string, string>>>(false,orginalText, null);
            }
            return new Tuple<bool, string, List<KeyValuePair<string, string>>>(true, messageText, keyValueList);
        }
    }

    public class ApiResponse
    {
        public bool Ok { get; set; }
        public string Kind { get; set; }
        public string Id { get; set; }
        public string LongUrl { get; set; }
        public string Status { get; set; }
        public Error Error { get; set; }
        public DateTime created { get; set; }
        public Analytics analytics { get; set; }
    }

    public class Error
    {
        public ErrorDescription[] Errors { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class ErrorDescription
    {
        public string Domain { get; set; }
        public string Reason { get; set; }
        public string Message { get; set; }
        public string LocationType { get; set; }
        public string Location { get; set; }
    }

    public class Referrer
    {
        public string count { get; set; }
        public string id { get; set; }
    }

    public class Country
    {
        public string count { get; set; }
        public string id { get; set; }
    }

    public class Browser
    {
        public string count { get; set; }
        public string id { get; set; }
    }

    public class Platform
    {
        public string count { get; set; }
        public string id { get; set; }
    }

    public class AllTime
    {
        public string shortUrlClicks { get; set; }
        public string longUrlClicks { get; set; }
        public List<Referrer> referrers { get; set; }
        public List<Country> countries { get; set; }
        public List<Browser> browsers { get; set; }
        public List<Platform> platforms { get; set; }
    }

    public class ReferrerInMonth
    {
        public string count { get; set; }
        public string id { get; set; }
    }

    public class CountryInMonth
    {
        public string count { get; set; }
        public string id { get; set; }
    }

    public class BrowserInMonth
    {
        public string count { get; set; }
        public string id { get; set; }
    }

    public class PlatformInMonth
    {
        public string count { get; set; }
        public string id { get; set; }
    }

    public class Month
    {
        public string shortUrlClicks { get; set; }
        public string longUrlClicks { get; set; }
        public List<ReferrerInMonth> referrers { get; set; }
        public List<CountryInMonth> countries { get; set; }
        public List<BrowserInMonth> browsers { get; set; }
        public List<PlatformInMonth> platforms { get; set; }
    }

    public class ReferrerInWeek
    {
        public string count { get; set; }
        public string id { get; set; }
    }

    public class CountryInWeek
    {
        public string count { get; set; }
        public string id { get; set; }
    }

    public class BrowserInWeek
    {
        public string count { get; set; }
        public string id { get; set; }
    }

    public class PlatformInWeek
    {
        public string count { get; set; }
        public string id { get; set; }
    }

    public class Week
    {
        public string shortUrlClicks { get; set; }
        public string longUrlClicks { get; set; }
        public List<ReferrerInWeek> referrers { get; set; }
        public List<CountryInWeek> countries { get; set; }
        public List<BrowserInWeek> browsers { get; set; }
        public List<PlatformInWeek> platforms { get; set; }
    }

    public class Day
    {
        public string shortUrlClicks { get; set; }
        public string longUrlClicks { get; set; }
    }

    public class TwoHours
    {
        public string shortUrlClicks { get; set; }
        public string longUrlClicks { get; set; }
    }

    public class Analytics
    {
        public AllTime allTime { get; set; }
        public Month month { get; set; }
        public Week week { get; set; }
        public Day day { get; set; }
        public TwoHours twoHours { get; set; }
    }
}
