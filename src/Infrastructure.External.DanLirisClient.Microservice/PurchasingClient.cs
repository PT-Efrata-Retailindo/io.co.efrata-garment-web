using Infrastructure.External.DanLirisClient.Microservice.Cache;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.External.DanLirisClient.Microservice
{
    public class PurchasingClient : IPurchasingClient
    {
        private readonly IHttpClientService _http;
        private readonly IMemoryCacheManager cacheManager;
        private readonly string Token;

        public PurchasingClient(IHttpClientService http, IServiceProvider serviceProvider)
        {
            _http = http;
            cacheManager = serviceProvider.GetService<IMemoryCacheManager>();
            Token = GetTokenAsync().Result;
        }

        public Task<dynamic> RetrieveUnitDepartments()
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> SetGarmentUnitExpenditureNote(int id)
        {
            var garmentUnitExpenditureNoteUri = PurchasingDataSettings.Endpoint + $"garment-unit-expenditure-notes/isPreparingFalse/{id}";
            var garmentUnitExpenditureNoteResponse = await _http.PutAsync(garmentUnitExpenditureNoteUri, PurchasingDataSettings.TokenEndpoint, new StringContent(JsonConvert.SerializeObject(new { username = PurchasingDataSettings.Username, password = PurchasingDataSettings.Password }), Encoding.UTF8, "application/json"));

            TokenResult tokenResult = new TokenResult();
            if (garmentUnitExpenditureNoteResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                tokenResult = JsonConvert.DeserializeObject<TokenResult>(await garmentUnitExpenditureNoteResponse.Content.ReadAsStringAsync());

            }
            else
            {
                await GetTokenAsync();
            }
            return tokenResult.data;
        }

        protected async Task<string> GetTokenAsync()
        {

            var response = await _http.PostAsync(MasterDataSettings.TokenEndpoint,
                new StringContent(JsonConvert.SerializeObject(new { username = MasterDataSettings.Username, password = MasterDataSettings.Password }), Encoding.UTF8, "application/json"));
            TokenResult tokenResult = new TokenResult();
            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                tokenResult = JsonConvert.DeserializeObject<TokenResult>(await response.Content.ReadAsStringAsync());

            }
            else
            {
                await GetTokenAsync();
            }
            return tokenResult.data;
        }
    }
}
