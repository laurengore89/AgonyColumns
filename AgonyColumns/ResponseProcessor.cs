using System;
using System.Linq;
using Newtonsoft.Json;

namespace AgonyColumns
{
	public class ResponseProcessor
	{
		public string ProcessResponseString(string responseString)
		{
			var returnString = "";

			Response response = JsonConvert.DeserializeObject<Response>(responseString);
			if (response.errors != null && response.errors.Any())
			{
				foreach (var e in response.errors)
				{
					returnString += "Error " + response.errors.FirstOrDefault().code + ": " + response.errors.FirstOrDefault().message;
				}
			}
			else if (response.id > 0)
			{
				System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
				returnString = "Sent tweet from user @" + response.user.screen_name + " at " + DateTime.ParseExact(response.created_at, "ddd MMM dd HH:mm:ss zzz yyyy", provider).ToShortTimeString() + ".";
			}
			else
			{
				returnString = responseString;
			}

			return returnString;
		}
	}
}
