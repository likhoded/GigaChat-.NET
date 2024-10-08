﻿using LD.Sber.GigaChatSDK.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http;

namespace GigaChatSDK
{
    public class HttpService : IHttpService
    {
        private readonly bool ignoreTLS;

        public HttpService(bool ignoreTLS)
        {
            this.ignoreTLS = ignoreTLS;
        }

        public HttpClientHandler CreateHttpClientHandler(bool ignoreTLS)
        {
            var handler = new HttpClientHandler();
            if (ignoreTLS)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            }
            return handler;
        }

        public async Task<string> SendAsync(HttpRequestMessage request)
        {
            try
            {
                using (var client = new HttpClient(CreateHttpClientHandler(ignoreTLS)))
                {
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Ошибка при отправке HTTP запроса: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Неизвестная ошибка: {ex.Message}", ex);
            }
        }
    }
}
