using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LD.Sber.GigaChatSDK.Interfaces
{
    public interface IHttpService
    {
        Task<string> SendAsync(HttpRequestMessage request);
        HttpClientHandler CreateHttpClientHandler(bool ignoreTLS);
    }
}
