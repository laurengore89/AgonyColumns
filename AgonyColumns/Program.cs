namespace AgonyColumns
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Configuration;

	public class Program
    {
        static async Task<string> MakeTweetCall(string baseUrl, string authHeader, string status)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                    {
                       { "status", status }
                    };

                var content = new FormUrlEncodedContent(values);

                AuthenticationHeaderValue authHeaders = new AuthenticationHeaderValue("OAuth", authHeader);

                client.DefaultRequestHeaders.Authorization = authHeaders;

                var responseRaw = await client.PostAsync(baseUrl, content);

                var responseString = await responseRaw.Content.ReadAsStringAsync();

				var processor = new ResponseProcessor();
				return processor.ProcessResponseString(responseString);
            }
        }

        static string MakeTweet(string consumKey, string consumSecret, string userToken, string userSecret, string nonce, string timestamp, string status)
        {
            var siggie = new EransOAuth.OAuthBase();
            var baseUrl = "https://api.twitter.com/1.1/statuses/update.json";

            var method = "POST";
            var baseUrlUri = new Uri(baseUrl + "?" + "status=" + status);
                        
            string normedUrl;
            string normedParams;

			// Console.WriteLine(siggie.GenerateSignatureBase(baseUrlUri,consumKey,userToken,userSecret,method,timestamp,nonce,"HMAC-SHA1",out normedUrl,out normedParams));

			Console.WriteLine("Sending tweet:");
			Console.WriteLine(status);

            var signature = siggie.GenerateSignature(baseUrlUri, consumKey, consumSecret, userToken, userSecret, method, timestamp, nonce, out normedUrl, out normedParams);

            var dst = string.Empty;

            dst += "oauth_consumer_key";
            dst += "=";
            dst += '"' + siggie.UrlEncode(consumKey) + '"';

            dst += ", ";

            dst += "oauth_nonce";
            dst += "=";
            dst += '"' + siggie.UrlEncode(nonce) + '"';

            dst += ", ";

            dst += "oauth_signature";
            dst += "=";
            dst += '"' + siggie.UrlEncode(signature) + '"';

            dst += ", ";

            dst += "oauth_signature_method";
            dst += "=";
            dst += '"' + siggie.UrlEncode("HMAC-SHA1") + '"';

            dst += ", ";

            dst += "oauth_timestamp";
            dst += "=";
            dst += '"' + siggie.UrlEncode(timestamp) + '"';

            dst += ", ";

            dst += "oauth_token";
            dst += "=";
            dst += '"' + siggie.UrlEncode(userToken) + '"';

            dst += ", ";

            dst += "oauth_version";
            dst += "=";
            dst += '"' + "1.0" + '"';

            Console.WriteLine();
            //Console.WriteLine(dst);
            //Console.WriteLine();

            var outStr = MakeTweetCall(baseUrl, dst, status);
            outStr.Wait();

            return outStr.Result;
        }

        static void Main(string[] args)
        {
            var consumKey = ConfigurationManager.AppSettings["consumKey"];
            var consumSecret = ConfigurationManager.AppSettings["consumSecret"];

            var userToken = ConfigurationManager.AppSettings["userToken"];
            var userSecret = ConfigurationManager.AppSettings["userSecret"];

            var nonce = (new EransOAuth.OAuthBase()).GenerateNonce();
            var timestamp = (new EransOAuth.OAuthBase()).GenerateTimeStamp();

			var status = "This is some text which exceeds 140 characters. This is used for testing to prevent actually sending the tweet, while confirming that the OAuth solution is working and all is as it should be.";
			//var status = "Ignore this, still testing.";

            Console.WriteLine(MakeTweet(consumKey, consumSecret, userToken, userSecret, nonce, timestamp, status));
            
            Console.ReadLine();
        }
    }
}
