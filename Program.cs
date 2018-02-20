using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShortenUrl
{
    
    class Program
    {
        static void Main(string[] args)
        {
            string text =
                "Konum bilginizi paylaşmak için lütfen bu adresi  https://tkaccurate.netlify.com/?wodId=8848848&processCode=111 ziyaret ediniz. İletişim ve görüşleriniz için http://www.telekurye.com.tr/";

            var googlApi = new UrlShortener("AIzaSyB_CuchBvjq9C2AcbSj3r3EIIm1dAIyHIA");

            var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Console.WriteLine(text);
            foreach (Match m in linkParser.Matches(text))
            {
                var serviceResult = googlApi.Shorten(m.Value);

                if (serviceResult.Ok)
                {
                    Console.WriteLine(m.Value + "----" + serviceResult.Id);
                    text = text.Replace(m.Value, serviceResult.Id);
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine(googlApi.Tracker("https://goo.gl/L2NRzo").analytics.allTime.shortUrlClicks);
            Console.ReadLine();

        }
    }
}
