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
            string oneUrl = "Merhaba bu linkten konum bilginizi paylaşınız https://tkaccurate.netlify.com/?wodId=1334&processCode=3457 iyi günler dilerim";
            string oneShorten = string.Empty;
            string twoUrl = "Hey look at this example https://www.w3schools.com/test/names.asp?n=John&n=Susan, in www.w3school.com site https://rl.telekurye.com/merhaba?data=123123123_1231231";
            string twoShorten = string.Empty;
            string example = "http:/twitter.com http:twitter.com http::/twitter.com www.twitter.com https://twitter.com";
            string exampleShorten = string.Empty;
            const string key = "AIzaSyB_CuchBvjq9C2AcbSj3r3EIIm1dAIyHIA";
            var getUrl = new UrlShortener(key);
            var linkParseOne  = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach (Match m in linkParseOne.Matches(oneUrl))
            {
                var result = getUrl.Shorten(m.Value);
                if (!result.Result.Ok)
                    break;

                oneShorten = oneUrl.Replace(m.Value, result.Result.Id);
            }
            Console.WriteLine(oneUrl);
            Console.WriteLine(oneShorten);


            var linkParseTwo = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var list = new List<dynamic>();
            foreach (Match m in linkParseOne.Matches(twoUrl))
            {
                var result = getUrl.Shorten(m.Value);
                if (!result.Result.Ok)
                    break;

                twoShorten = twoUrl.Replace(m.Value, result.Result.Id);
                list.Add(new { LongUrl = m.Value, ShortUrl = result.Result.Id });
                twoUrl = twoShorten;
            }
            Console.WriteLine(twoUrl);
            Console.WriteLine(twoShorten);
            Console.WriteLine(JsonConvert.SerializeObject(list));

            //var linkParseExp = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //foreach (Match m in linkParseOne.Matches(example))
            //    Console.WriteLine(m.Value);

            Console.ReadLine();

        }
    }
}
