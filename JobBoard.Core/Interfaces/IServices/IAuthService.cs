using System;
using System.Collections.Generic;
using System.Text;
using JobBoard.Core.DTOs.Auth.Requests;
using JobBoard.Core.DTOs.Auth.Responses;

namespace JobBoard.Core.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<AuthResponse> LogoutAsync(LogoutRequest request);
            Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}
