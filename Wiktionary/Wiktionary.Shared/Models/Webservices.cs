using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace Wiktionary.Models
{
    public class Webservices
    {
        private HttpClient _httpClient;
        private HttpResponseMessage _response;

        public Webservices()
        {
            InitWebservice();
        }

        //On initialise le web service
        private void InitWebservice()
        {
            HttpBaseProtocolFilter rootFilter = new HttpBaseProtocolFilter();
            rootFilter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;
            rootFilter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;
            _httpClient = new HttpClient(rootFilter);

            var headers = _httpClient.DefaultRequestHeaders;

            headers.UserAgent.ParseAdd("ie");
            headers.UserAgent.ParseAdd("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
        }

        //Web service qui permet d'obtenir toutes les définitions
        public async Task<string> GetAllDefinitions()
        {
            InitWebservice();

            string adresse = "http://wiktionary.azurewebsites.net/Wiktionary.svc/GetAllDefinitions";

            string definitions;

            _response = new HttpResponseMessage();

            Uri resourceUri;
            if (!Uri.TryCreate(adresse.Trim(), UriKind.Absolute, out resourceUri))
            {

            }
            if (resourceUri.Scheme != "http" && resourceUri.Scheme != "https")
            {

            }

            string responseBodyAsText;

            try
            {
                _response = await _httpClient.GetAsync(resourceUri);

                _response.EnsureSuccessStatusCode();

                responseBodyAsText = await _response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                responseBodyAsText = "";
            }
            definitions = responseBodyAsText;

            return definitions;
        }

        //Web service qui permet d'obtenir une définition précise
        public async Task<string> GetDefinition(string mot)
        {
            InitWebservice();

            string adresse = "http://wiktionary.azurewebsites.net/Wiktionary.svc/Get/" + mot;

            string definitions;

            _response = new HttpResponseMessage();

            Uri resourceUri;
            if (!Uri.TryCreate(adresse.Trim(), UriKind.Absolute, out resourceUri))
            {
            }
            if (resourceUri.Scheme != "http" && resourceUri.Scheme != "https")
            {
            }

            string responseBodyAsText;

            try
            {
                _response = await _httpClient.GetAsync(resourceUri);

                _response.EnsureSuccessStatusCode();

                responseBodyAsText = await _response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                responseBodyAsText = "";
            }
            definitions = responseBodyAsText;

            return definitions;
        }

        //Web service qui permet d'ajouter une définition précise
        public async Task<string> AddDefinition(string mot, string definition, string username)
        {
            InitWebservice();

            string adresse = "http://wiktionary.azurewebsites.net/Wiktionary.svc/AddDefinition/" + mot + "/" + definition + "/" + username;

            string definitions;

            _response = new HttpResponseMessage();

            Uri resourceUri;
            if (!Uri.TryCreate(adresse.Trim(), UriKind.Absolute, out resourceUri))
            {
            }
            if (resourceUri.Scheme != "http" && resourceUri.Scheme != "https")
            {
            }

            string responseBodyAsText;

            try
            {
                _response = await _httpClient.GetAsync(resourceUri);

                _response.EnsureSuccessStatusCode();

                responseBodyAsText = await _response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                responseBodyAsText = "";
            }
            definitions = responseBodyAsText;

            return definitions;
        }

        //Web service qui permet de supprimer une définition précise
        public async Task<string> DeleteDefinition(string mot, string username)
        {
            InitWebservice();

            string adresse = "http://wiktionary.azurewebsites.net/Wiktionary.svc/RemoveDefinition/" + mot + "/" + username;

            string definitions;

            _response = new HttpResponseMessage();

            Uri resourceUri;
            if (!Uri.TryCreate(adresse.Trim(), UriKind.Absolute, out resourceUri))
            {
            }
            if (resourceUri.Scheme != "http" && resourceUri.Scheme != "https")
            {
            }

            string responseBodyAsText;

            try
            {
                _response = await _httpClient.GetAsync(resourceUri);

                _response.EnsureSuccessStatusCode();

                responseBodyAsText = await _response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                responseBodyAsText = "";
            }
            definitions = responseBodyAsText;

            return definitions;
        }
    }
}
