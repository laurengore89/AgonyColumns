using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AgonyColumns
{
	class TweetSender
	{
		private static async Task<string> MakeTweetCall(string baseUrl, string authHeader, string status)
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

		public static string SendTweet(string consumKey, string consumSecret, string userToken, string userSecret, string status)
		{
			var nonce = (new EransOAuth.OAuthBase()).GenerateNonce();
			var timestamp = (new EransOAuth.OAuthBase()).GenerateTimeStamp();

			var siggie = new EransOAuth.OAuthBase();
			var baseUrl = "https://api.twitter.com/1.1/statuses/update.json";

			var method = "POST";
			var baseUrlUri = new Uri(baseUrl + "?" + "status=" + status);

			var signature = siggie.GenerateSignature(baseUrlUri, consumKey, consumSecret, userToken, userSecret, method, timestamp, nonce, out string normedUrl, out string normedParams);

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

			var outStr = MakeTweetCall(baseUrl, dst, status);
			outStr.Wait();

			return outStr.Result;
		}

	}
}
