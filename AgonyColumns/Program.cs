namespace AgonyColumns
{
    using System;
    using System.Configuration;

	public class Program
    {
        static void Main(string[] args)
        {
            var consumKey = ConfigurationManager.AppSettings["consumKey"];
            var consumSecret = ConfigurationManager.AppSettings["consumSecret"];

            var userToken = ConfigurationManager.AppSettings["userToken"];
            var userSecret = ConfigurationManager.AppSettings["userSecret"];

			var status = "This is some text which exceeds 140 characters. This is used for testing to prevent actually sending the tweet, while confirming that the OAuth solution is working and all is as it should be.";
			//var status = "Ignore this, still testing.";
			
			Console.WriteLine("Sending tweet:");
			Console.WriteLine(status);

			Console.WriteLine();

			Console.WriteLine(TweetSender.SendTweet(consumKey, consumSecret, userToken, userSecret, status));
            
            Console.ReadLine();
        }
    }
}
