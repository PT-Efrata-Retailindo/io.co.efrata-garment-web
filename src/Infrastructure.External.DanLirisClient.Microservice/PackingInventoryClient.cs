using Infrastructure.External.DanLirisClient.Microservice.Cache;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Newtonsoft.Json;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;

namespace Infrastructure.External.DanLirisClient.Microservice
{
    public class PackingInventoryClient : IPackingInventoryClient
    {
        private readonly IHttpClientService _http;
        private readonly IMemoryCacheManager cacheManager;
        private readonly string Token;

        public PackingInventoryClient(IHttpClientService http, IServiceProvider serviceProvider)
        {
            _http = http;
            cacheManager = serviceProvider.GetService<IMemoryCacheManager>();
            Token = GetTokenAsync().Result;
        }

        public async Task<bool> SetIsSampleExpenditureGood(string invoiceNo, bool isSampleExpenditureGood)
        {
            var garmentPackingListUri = PackingInventoryDataSettings.Endpoint + $"garment-shipping/packing-lists/setIsSampleExpenditureGood/{invoiceNo}";
            var garmentPackingListResponse = await _http.PutAsync(garmentPackingListUri, PackingInventoryDataSettings.TokenEndpoint, new StringContent(JsonConvert.SerializeObject(new { isSampleExpenditureGood = isSampleExpenditureGood }), Encoding.UTF8, "application/json"));

            SingleGarmentPackingListResult packingListResult = new SingleGarmentPackingListResult();
            if (garmentPackingListResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                packingListResult = JsonConvert.DeserializeObject<SingleGarmentPackingListResult>(await garmentPackingListResponse.Content.ReadAsStringAsync());
            }
            else
            {
                await SetIsSampleExpenditureGood(invoiceNo, isSampleExpenditureGood);
            }
            return packingListResult.data.IsGarmentExpenditureGood;
        }

        protected async Task<string> GetTokenAsync()
        {
            var response = await _http.PostAsync(MasterDataSettings.TokenEndpoint, new StringContent(JsonConvert.SerializeObject(new { username = MasterDataSettings.Username, password = MasterDataSettings.Password }), Encoding.UTF8, "application/json"));
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
