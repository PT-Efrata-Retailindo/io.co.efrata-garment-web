using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.External.DanLirisClient.Microservice.HttpClientService
{
	public class HttpClientService : IHttpClientService
	{
		static HttpClient _client = new HttpClient();

		//public HttpClientService(string token)
		//{
		//    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		//}

		public async Task<HttpResponseMessage> GetAsync(string url)
		{
			return await _client.GetAsync(url);
		}

		public async Task<HttpResponseMessage> GetAsync(string url, string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			return await _client.GetAsync(url);
		}

		public async Task<HttpResponseMessage> PostAsync(string url, string token, HttpContent content)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			return await _client.PostAsync(url, content);
		}

		public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
		{
			return await _client.PostAsync(url, content);
		}

		public async Task<HttpResponseMessage> PutAsync(string url, string token, HttpContent content)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			return await _client.PutAsync(url, content);
		}

		public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
		{
			return await _client.PutAsync(url, content);
		}


		public async Task<HttpResponseMessage> SendAsync(HttpMethod method, string url,string token, HttpContent content)
		{
			var request = new HttpRequestMessage(method, url)
			{
				Content = content
			};
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			return await _client.SendAsync(request);

		}
	}
}
