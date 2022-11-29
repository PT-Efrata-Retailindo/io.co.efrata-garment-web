using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice
{
	public class SalesDataSettings
	{
		public static string Endpoint { get; set; }

		public static string TokenEndpoint { get; set; }

		public static string Password { get; set; }

		public static string Username { get; set; }
	}
}
