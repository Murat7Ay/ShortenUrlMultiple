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
                       const string textWithUrl = "This text has URLs https://twitter.com/murat7ay," +
                                       " https://murat7ay.netlify.com/#/. " +
                                       "Extra if something comes https:// ," +
                                       "something like twitter.com ," +
                                       "http https ftp " +
                                       "http://www.google.com " +
                                       "http://foo:bar@w1.superman.com/very/long/path.html?p1=V1&p2=v2#more-details " +
                                       "https://secured.com:443 " +
                                       "ftp://ftp.bogus.com/~some/path/to/a/file.txt " +
                                       "username:password@host.com/ " +
                                       "subdomain.domain.com " +
                                       "www.google.com.tr " +
                                       "www.superaddress.com:8080 " +
                                       "http://www.foo.bar/segment1/segment2/some-resource.html " +
                                       "http://www.foo.bar/image-2.html?w=100&hI=50 " +
                                       "ftp://ftp.foo.bar/~john/doe?w=100&h=50 " +
                                       "http://www.foo.bar/image.jpg?height=150&widTth=100 " +
                                       "https://www.secured.com:443/resource.html?id=6e8bc430-9c3a-11d9-9669-0800200c9a66#some-header " +
                                       "ftp://ftp.is.co.za/rfc/rfc1808.txt " +
                                       "http://www.ietf.org/rfc/rfc2396.txt " +
                                       "ldap://[2001:db8::7]/c=GB?objectClass?one " +
                                       "news:comp.infosystems.www.servers.unix " +
                                       "tel:+1-816-555-1212 " +
                                       "telnet://192.0.2.16:80/ " +
                                       "urn:oasis:names:specification:docbook:dtd:xml:4.1.2 " +
                                       "google.com.tr  " +
                                       "https://rl.telekurye.com.tr/sorgula.aspx?wodid=00000&password=000 " +
                                       "https://rl.telekurye.com.tr/sorgula.aspx?wodIaAbBd=00000&password=000 " +
                                       "https://uygulama.telekurye.com.tr/SmartPol/smartPolAppointment?data=xxxx_xxxxxx " +
                                       "https://amaksdkdaskdaskk.TELEKURYE.gov.tr/#asdadsasd/?key=asdsad%&?asdasd=1123...,,,,!!!";
            const string textWithNoUrl =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." +
                " Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                " Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur." +
                " Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

            var googlApi = UrlShortener.Instance;

            var result = googlApi.ConvertToShortenUrl(textWithUrl);

            var resultnoUrl = googlApi.ConvertToShortenUrl(textWithNoUrl);

            if (resultnoUrl.Item1)
            {
                Console.WriteLine(resultnoUrl.Item2);
                if (resultnoUrl.Item3 != null)
                {
                    foreach (var i in resultnoUrl.Item3)
                    {
                        Console.WriteLine(i.Key + " - " + i.Value);
                        Console.WriteLine("Statistics in all time click => " + googlApi.TrackerAllTimeShortUrlClicks(i.Value));
                    }
                }
            }

            if (result.Item1)
            {
                Console.WriteLine(result.Item2);
                if (result.Item3 != null)
                {
                    foreach (var i in result.Item3)
                    {
                        Console.WriteLine(i.Key + " - " + i.Value);
                        Console.WriteLine("Statistics in all time click =>" + googlApi.TrackerAllTimeShortUrlClicks(i.Value));
                    }
                }
            }

            //txt file about 1 GB
            //var linkParser = new Regex(@"(?:(?:https?|ftp|file):\/\/?)(?:\([-A-Z0-9+&@#/%=~_|$?!:,.]*\)|[-A-Z0-9+&@#/%=~_|$?!:,.])*(?:\([-A-Z0-9+&@#/%=~_|$?!:,.]*\)|[A-Z0-9+&@#/%=~_|$])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //using (FileStream fs = File.Open(@"C:\apideneme\fullSmsMessage.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //using (BufferedStream bs = new BufferedStream(fs))
            //using (StreamReader sr = new StreamReader(bs))
            //{
            //    string line;
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        var m = linkParser.Matches(line);
            //        if (m.Count == 0)
            //            continue;
            //        foreach (Match a in m)
            //        {
            //            googlApi.ConvertToShortenUrl(a.Value);
            //            Console.WriteLine(a.Value);
            //        }
            //    }
            //}


        }
    }
}
