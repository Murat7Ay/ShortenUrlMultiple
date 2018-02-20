using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenUrl.Models
{
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
