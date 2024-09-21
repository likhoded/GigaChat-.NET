using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using LD.Sber.GigaChatSDK.Models;

namespace LD.Sber.GigaChatSDK.Interfaces
{
    public interface ITokenService
    {
        Task<Token> CreateTokenAsync();
        long? ExpiresAt { get; }
        Token Token { get; }
        void SetToken(Token token);
    }
}
