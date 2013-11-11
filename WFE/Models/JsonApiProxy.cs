using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WFE.Models
{
    public abstract class JsonApiProxy
    {
        public abstract string BaseUri { get; }

        public virtual HttpClient NewClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUri);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public void HandleError(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            else
                throw new InvalidOperationException(
                    response.Content.ReadAsStringAsync().Result);
        }
    }
}
