namespace AgonyColumns
{
    using System;
    using System.Configuration;

	public class Program
    {
        static void Main(string[] args)
        {
			var status = "";

			if (args.Length > 0)
			{
				status = args[0];
			}
			else
			{
				status = "This is some text which exceeds 140 characters. This is used for testing to prevent actually sending the tweet, while confirming that the OAuth solution is working and all is as it should be.";
				//status = "Ignore this, still testing.";
			}

			var consumKey = ConfigurationManager.AppSettings["consumKey"];
            var consumSecret = ConfigurationManager.AppSettings["consumSecret"];

            var userToken = ConfigurationManager.AppSettings["userToken"];
            var userSecret = ConfigurationManager.AppSettings["userSecret"];

			Console.WriteLine("Sending tweet:");
			Console.WriteLine(status);

			Console.WriteLine();

			Console.WriteLine(TweetSender.SendTweet(consumKey, consumSecret, userToken, userSecret, status));

			if (args.Length == 0)
			{
				// we're probably testing from inside Visual Studio
				Console.ReadLine();
			}
        }
    }
}
