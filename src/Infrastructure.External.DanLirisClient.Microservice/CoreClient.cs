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
    public class CoreClient : ICoreClient
    {
        private readonly IHttpClientService _http;
        private readonly IMemoryCacheManager cacheManager;
        private readonly string Token;

        public CoreClient(IHttpClientService http, IServiceProvider serviceProvider)
        {
            _http = http;
            cacheManager = serviceProvider.GetService<IMemoryCacheManager>();
            Token = GetTokenAsync().Result;
        }

        public Task<dynamic> RetrieveUnitDepartments()
        {
            throw new System.NotImplementedException();
        }

        //public void SetMachineSpinning()
        //{
        //    var masterMachineSpinningUri = MasterDataSettings.Endpoint + $"machine-spinnings/simple";
        //    //var masterUnitUri = $"https://com-danliris-service-core-dev.azurewebsites.net/v1/master/units/simple";
        //    var machineSpinningResponse = _http.GetAsync(masterMachineSpinningUri, Token).Result;

        //    var machineSpinningResult = new MachineSpinningResult();
        //    if (machineSpinningResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
        //    {
        //        machineSpinningResult = JsonConvert.DeserializeObject<MachineSpinningResult>(machineSpinningResponse.Content.ReadAsStringAsync().Result);
        //    }
        //    else
        //    {
        //        SetMachineSpinning();
        //    }

        //    if (machineSpinningResult.data.Count > 0)
        //        cacheManager.Set("MachineSpinnings", machineSpinningResult.data);

        //}

        public void SetMachineType()
        {
            var masterMachineSpinningUri = MasterDataSettings.Endpoint + $"machine-spinnings/machine/types";
            //var masterUnitUri = $"https://com-danliris-service-core-dev.azurewebsites.net/v1/master/units/simple";
            var machineSpinningResponse = _http.GetAsync(masterMachineSpinningUri, Token).Result;

            List<string> result = new List<string>();
            if (machineSpinningResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<List<string>>(machineSpinningResponse.Content.ReadAsStringAsync().Result);
            }
            else
            {
                SetMachineType();
            }

            if (result.Count > 0)
                cacheManager.Set("MachineTypes", result);
        }

        public void SetProduct()
        {
            var masterProductUri = MasterDataSettings.Endpoint + $"master/products/simple";
            //var masterUnitUri = $"https://com-danliris-service-core-dev.azurewebsites.net/v1/master/products/simple";
            var productResponse = _http.GetAsync(masterProductUri).Result;

            var productResult = new ProductResult();
            if (productResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                productResult = JsonConvert.DeserializeObject<ProductResult>(productResponse.Content.ReadAsStringAsync().Result);
            }
            else
            {
                SetProduct();
            }
            if (productResult.data.Count > 0)
                cacheManager.Set("Products", productResult.data);
        }

        public void SetUnitDepartments()
        {
            var masterUnitUri = MasterDataSettings.Endpoint + $"master/units/simple";
            //var masterUnitUri = $"https://com-danliris-service-core-dev.azurewebsites.net/v1/master/units/simple";
            var unitResponse = _http.GetAsync(masterUnitUri).Result;

            var unitResult = new UnitResult();
            if (unitResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                unitResult = JsonConvert.DeserializeObject<UnitResult>(unitResponse.Content.ReadAsStringAsync().Result);
            }
            else
            {
                SetUnitDepartments();
            }
            if (unitResult.data.Count > 0)
                cacheManager.Set("Units", unitResult.data);
        }

        public void SetUoms()
        {
            var masterUomUri = MasterDataSettings.Endpoint + $"master/uoms/simple";
            //var masterUnitUri = $"https://com-danliris-service-core-dev.azurewebsites.net/v1/master/units/simple";
            var uomResponse = _http.GetAsync(masterUomUri).Result;

            var uomResult = new UnitResult();
            if (uomResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                uomResult = JsonConvert.DeserializeObject<UnitResult>(uomResponse.Content.ReadAsStringAsync().Result);
            }
            else
            {
                SetUoms();
            }
            if (uomResult.data.Count > 0)
                cacheManager.Set("Uoms", uomResult.data);
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

        public void SetGarmentProducts()
        {
            var masterGarmentProductUri = MasterDataSettings.Endpoint + $"master/garmentProducts";
            var garmentProductResponse = _http.GetAsync(masterGarmentProductUri).Result;

            var garmentProductResult = new ProductResult();
            if (garmentProductResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                garmentProductResult = JsonConvert.DeserializeObject<ProductResult>(garmentProductResponse.Content.ReadAsStringAsync().Result);
            }
            else
            {
                SetGarmentProducts();
            }
            if (garmentProductResult.data.Count > 0)
                cacheManager.Set("GarmentProducts", garmentProductResult.data);
        }

        public void SetStorages()
        {
            var masterStorageUri = MasterDataSettings.Endpoint + $"master/storages";
            var storageResponse = _http.GetAsync(masterStorageUri).Result;

            var storageResult = new UnitResult();
            if (storageResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                storageResult = JsonConvert.DeserializeObject<UnitResult>(storageResponse.Content.ReadAsStringAsync().Result);
            }
            else
            {
                SetStorages();
            }
            if (storageResult.data.Count > 0)
                cacheManager.Set("Storages", storageResult.data);
        }

        public void SetGarmentComodities()
        {
            var masterGarmentComodityUri = MasterDataSettings.Endpoint + $"master/garment-comodities";
            var garmentComodityResponse = _http.GetAsync(masterGarmentComodityUri).Result;

            var garmentComodityResult = new GarmentComodityResult();
            if (garmentComodityResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                garmentComodityResult = JsonConvert.DeserializeObject<GarmentComodityResult>(garmentComodityResponse.Content.ReadAsStringAsync().Result);
            }
            else
            {
                SetGarmentComodities();
            }
            if (garmentComodityResult.data.Count > 0)
                cacheManager.Set("GarmentComodities", garmentComodityResult.data);
        }
    }
}