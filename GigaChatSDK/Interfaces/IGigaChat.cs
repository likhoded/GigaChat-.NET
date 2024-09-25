using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LD.Sber.GigaChatSDK.Models;
using LD.Sber.GigaChatSDK.Utils;

namespace LD.Sber.GigaChatSDK.Interfaces
{
    public interface IGigaChat
    {
        Task<Token> CreateTokenAsync();
        Task<Response?> CompletionsAsync(MessageQuery query);
        Task<Response?> CompletionsAsync(string role, string message);
        Task<EmbeddingResponse?> EmbeddingAsync(EmbeddingRequest request);
        Task<byte[]?> GetImageAsByteAsync(string fileId);
        string? GetFileId(string messageContent);
    }
}
