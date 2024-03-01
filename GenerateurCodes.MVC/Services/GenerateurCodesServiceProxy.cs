using GenerateurCodes.MVC.Interfaces;
using GenerateurCodes.MVC.Models;
using Newtonsoft.Json;
using System.Text;

namespace GenerateurCodes.MVC.Services
{
    public class GenerateurCodesServiceProxy : IGenerateurCodesService
    {
        private readonly HttpClient _httpClient;
        private const string _generateurCodesApiUrl = "api/generateurcodes/";
        public GenerateurCodesServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public Task<HttpResponseMessage> DemanderCode(DemandeCodeAcces demandeCodeAcces)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(demandeCodeAcces), Encoding.UTF8, "application/json");

            return _httpClient.PostAsync(_generateurCodesApiUrl, content);
        }
    }
}
