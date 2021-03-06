using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using NUnit.Framework;
using NServiceKit.Common.Web;
using NServiceKit.ServiceClient.Web;
using NServiceKit.Text;
using NServiceKit.WebHost.IntegrationTests.Services;

namespace NServiceKit.WebHost.IntegrationTests.Tests
{
    /// <summary>A CSV content type filter tests.</summary>
	[TestFixture]
	public class CsvContentTypeFilterTests
	{
		const int HeaderRowCount = 1;
		private const string ServiceClientBaseUri = Config.NServiceKitBaseUri + "/";

		private static void FailOnAsyncError<T>(T response, Exception ex)
		{
			Assert.Fail(ex.Message);
		}

        /// <summary>Sets the up.</summary>
        [SetUp]
        public void SetUp()
        {
            // make sure that movies db is not modified
            RestsTestBase.GetWebResponse(HttpMethods.Post, ServiceClientBaseUri + "reset-movies", ContentType.Xml, 0);
        }

        /// <summary>Can download movies in CSV.</summary>
		[Test]
		[Ignore("Fails because CSV Deserializer is not implemented")]
		public void Can_download_movies_in_Csv()
		{
			var asyncClient = new AsyncServiceClient
			{
				ContentType = ContentType.Csv,
				StreamSerializer = (r, o, s) => CsvSerializer.SerializeToStream(o, s),
				StreamDeserializer = CsvSerializer.DeserializeFromStream,
			};

			MoviesResponse response = null;
			asyncClient.SendAsync<MoviesResponse>(HttpMethods.Get, ServiceClientBaseUri + "/movies", null,
												  r => response = r, FailOnAsyncError);

			Thread.Sleep(1000);

			Assert.That(response, Is.Not.Null, "No response received");
		}

        /// <summary>Can download CSV movies using CSV syncreply endpoint.</summary>
		[Test]
		public void Can_download_CSV_movies_using_csv_syncreply_endpoint()
		{
			var req = (HttpWebRequest)WebRequest.Create(ServiceClientBaseUri + "csv/syncreply/Movies");

			var res = req.GetResponse();
			Assert.That(res.ContentType, Is.EqualTo(ContentType.Csv));
			Assert.That(res.Headers[HttpHeaders.ContentDisposition], Is.EqualTo("attachment;filename=Movies.csv"));

			var csvRows = new StreamReader(res.GetResponseStream()).ReadLines().ToList();

			const int headerRowCount = 1;
			Assert.That(csvRows, Has.Count.EqualTo(headerRowCount + ResetMoviesService.Top5Movies.Count));
			//Console.WriteLine(csvRows.Join("\n"));
		}

        /// <summary>Can download CSV movies using CSV synchronise reply path and alternate XML accept header.</summary>
		[Test]
		public void Can_download_CSV_movies_using_csv_SyncReply_Path_and_alternate_XML_Accept_Header()
		{
			var req = (HttpWebRequest)WebRequest.Create(ServiceClientBaseUri + "csv/syncreply/Movies");
			req.Accept = "application/xml";

			var res = req.GetResponse();
			Assert.That(res.ContentType, Is.EqualTo(ContentType.Csv));
			Assert.That(res.Headers[HttpHeaders.ContentDisposition], Is.EqualTo("attachment;filename=Movies.csv"));

			var csvRows = new StreamReader(res.GetResponseStream()).ReadLines().ToList();

			Assert.That(csvRows, Has.Count.EqualTo(HeaderRowCount + ResetMoviesService.Top5Movies.Count));
			Console.WriteLine(csvRows.Join("\n"));
		}

        /// <summary>Can download CSV movies using CSV accept and rest path.</summary>
		[Test]
		public void Can_download_CSV_movies_using_csv_Accept_and_RestPath()
		{
			var req = (HttpWebRequest)WebRequest.Create(ServiceClientBaseUri + "movies");
			req.Accept = ContentType.Csv;

			var res = req.GetResponse();
			Assert.That(res.ContentType, Is.EqualTo(ContentType.Csv));
			Assert.That(res.Headers[HttpHeaders.ContentDisposition], Is.EqualTo("attachment;filename=Movies.csv"));

			var csvRows = new StreamReader(res.GetResponseStream()).ReadLines().ToList();

			Assert.That(csvRows, Has.Count.EqualTo(HeaderRowCount + ResetMoviesService.Top5Movies.Count));
			//Console.WriteLine(csvRows.Join("\n"));
		}

        /// <summary>Can download CSV hello using CSV syncreply endpoint.</summary>
		[Test]
		public void Can_download_CSV_Hello_using_csv_syncreply_endpoint()
		{
			var req = (HttpWebRequest)WebRequest.Create(ServiceClientBaseUri + "csv/syncreply/Hello?Name=World!");

			var res = req.GetResponse();
			Assert.That(res.ContentType, Is.EqualTo(ContentType.Csv));
			Assert.That(res.Headers[HttpHeaders.ContentDisposition], Is.EqualTo("attachment;filename=Hello.csv"));

			var csv = new StreamReader(res.GetResponseStream()).ReadToEnd();
			Assert.That(csv, Is.EqualTo("Result\r\n\"Hello, World!\"\r\n"));

			Console.WriteLine(csv);
		}

        /// <summary>Can download CSV hello using CSV accept and rest path.</summary>
		[Test]
		public void Can_download_CSV_Hello_using_csv_Accept_and_RestPath()
		{
			var req = (HttpWebRequest)WebRequest.Create(ServiceClientBaseUri + "hello/World!");
			req.Accept = ContentType.Csv;

			var res = req.GetResponse();
			Assert.That(res.ContentType, Is.EqualTo(ContentType.Csv));
			Assert.That(res.Headers[HttpHeaders.ContentDisposition], Is.EqualTo("attachment;filename=Hello.csv"));

			var csv = new StreamReader(res.GetResponseStream()).ReadToEnd();
			Assert.That(csv, Is.EqualTo("Result\r\n\"Hello, World!\"\r\n"));

			Console.WriteLine(csv);
		}

	}
}