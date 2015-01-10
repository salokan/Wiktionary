using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Wiktionary.Models
{
    public class Webservices
    {
        private HttpClient httpClient;
        private HttpResponseMessage response;

        public Webservices()
        {
            InitWebservice();
        }

        private void InitWebservice()
        {
            httpClient = new HttpClient();

            // Add a user-agent header
            var headers = httpClient.DefaultRequestHeaders;

            // HttpProductInfoHeaderValueCollection is a collection of 
            // HttpProductInfoHeaderValue items used for the user-agent header

            // The safe way to check a header value from the user is the TryParseAdd method
            // Since we know this header is okay, we use ParseAdd with will throw an exception
            // with a bad value 
            headers.UserAgent.ParseAdd("ie");
            headers.UserAgent.ParseAdd("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
        }

        public async Task<string> GetAllDefinitions()
        {
            string adresse = "http://wiktionary.azurewebsites.net/Wiktionary.svc/GetAllDefinitions";

            string definitions = "";

            response = new HttpResponseMessage();

            Uri resourceUri;
            if (!Uri.TryCreate(adresse.Trim(), UriKind.Absolute, out resourceUri))
            {
                definitions = "Invalid URI, please re-enter a valid URI";
            }
            if (resourceUri.Scheme != "http" && resourceUri.Scheme != "https")
            {
                definitions = "Only 'http' and 'https' schemes supported. Please re-enter URI";
            }

            string responseBodyAsText;

            try
            {
                response = await httpClient.GetAsync(resourceUri);

                response.EnsureSuccessStatusCode();

                responseBodyAsText = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                // Need to convert int HResult to hex string
                definitions = "Error = " + ex.HResult.ToString("X") +
                    "  Message: " + ex.Message;
                responseBodyAsText = "";
            }
            definitions = responseBodyAsText;

            return definitions;
        }
    }
}
