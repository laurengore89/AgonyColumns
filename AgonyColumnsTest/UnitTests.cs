using AgonyColumns;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace AgonyColummnsTest
{
	[TestClass]
	public class UnitTests
	{
		public string errorResponseString;
		public string successResponseString;

		[TestInitialize]
		public void Initialize()
		{
			var errorResponse = new Response
			{
				errors = new List<Error>
				{
					new Error
					{
						code = 140,
						message = "An error occurred."
					}
				}
			};
			errorResponseString = JsonConvert.SerializeObject(errorResponse);

			var successResponse = new Response
			{
				id = 1,
				user = new User
				{
					screen_name = "TestUser"
				},
				created_at = "Wed Aug 27 13:08:45 +0000 2008"
			};
			successResponseString = JsonConvert.SerializeObject(successResponse);
		}

		[TestMethod]
		public void ErrorTest()
		{
			var processor = new ResponseProcessor();
			string errorResponse = processor.ProcessResponseString(errorResponseString);
			Assert.IsTrue(errorResponse.Contains("Error 140"));
			Assert.IsTrue(errorResponse.Contains("An error occurred."));
		}

		[TestMethod]
		public void SuccessTest()
		{
			var processor = new ResponseProcessor();
			string successResponse = processor.ProcessResponseString(successResponseString);
			Assert.IsTrue(successResponse.Contains("from user @TestUser"));
			Assert.IsTrue(successResponse.Contains("at 14:08."));
		}
	}
}
