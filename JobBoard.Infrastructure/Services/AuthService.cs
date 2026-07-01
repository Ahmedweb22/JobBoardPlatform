using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JobBoard.Core.DTOs.Auth.Requests;
using JobBoard.Core.DTOs.Auth.Responses;
using JobBoard.Core.Interfaces.IUnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JobBoard.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<RefreshToken> _refreshToken;
        private readonly IRepository<Employer> _employer;
        private readonly IRepository<Candidate> _candidate;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IRepository<RefreshToken> refreshToken, IRepository<Employer> employer, IRepository<Candidate> candidate , IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _refreshToken = refreshToken;
            _employer = employer;
            _candidate = candidate;
            _unitOfWork = unitOfWork;
        }
        public async Task<AuthResponse> LogoutAsync(LogoutRequest request)
        {
            var existingToken = (await _refreshToken.GetAllAsync()).FirstOrDefault(x => x.Token == request.RefreshToken);

            if (existingToken == null)
            {
                return new AuthResponse
                {
                    Message = "Invalid refresh token"
                };
            }

            _refreshToken.Delete(existingToken);

            await _unitOfWork.CommitAsync();

            return new AuthResponse
            {
                Message = "Logout successfully"
            };
        }
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return new AuthResponse
                {
                    Message = "Email is already registered."
                };
            }

            var user = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Email
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
                return new AuthResponse
                {
                    Message = $"Registration failed : {string.Join(", ", errors)}"
                };
            }

            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(request.Role));
            }
            await _userManager.AddToRoleAsync(user, request.Role);

            if (request.Role == "Employer")
            {
                var newEmployer = new Employer
                {
                    UserId = user.Id,
                    CompanyName = request.CompanyName ?? "New Company",
                    CompanyDescription = request.Description ?? string.Empty
                };
                await _employer.AddAsync(newEmployer);
            }
            else if (request.Role == "Candidate")
            {
                var newCandidate = new Candidate
                {
                    UserId = user.Id,
                    FullName = request.FullName ?? "New Candidate",
                    PhoneNumber = request.PhoneNumber ?? string.Empty
                };
                await _candidate.AddAsync(newCandidate);

            }
            await _unitOfWork.CommitAsync();

            var jwtToken = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();
            var userRefreshToken = new RefreshToken
            {
                Token = refreshToken.Token,
                Expires = refreshToken.Expires,
                UserId = user.Id

            };
            await _refreshToken.AddAsync(userRefreshToken);
            await _unitOfWork.CommitAsync();
            return new AuthResponse
            {
                IsAuthenticated = true,
                Message = "User registered successfully.",
                Email = user.Email,
                Role = request.Role,
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.Expires
            };


        }
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new AuthResponse
                {
                    Message = "Invalid email or password."
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Candidate";
            var jwtToken = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();
            var userRefreshToken = new RefreshToken
            {
                Token = refreshToken.Token,
                Expires = refreshToken.Expires,
                UserId = user.Id
            };
            await _refreshToken.AddAsync(userRefreshToken);
            await _unitOfWork.CommitAsync();
            var userRoles = await _userManager.GetRolesAsync(user);
            return new AuthResponse
            {
                IsAuthenticated = true,
                Message = "User logged in successfully.",
                Email = user.Email,
                Role = userRoles.FirstOrDefault(),
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.Expires
            };
        }
        public async Task<AuthResponse> RefreshTokenAsync(string token)
        {
            var existingRefreshToken = (await _refreshToken.GetAllAsync()).FirstOrDefault(rt => rt.Token == token);
            if (existingRefreshToken == null || !existingRefreshToken.IsActive)
            {
                return new AuthResponse
                {
                    Message = "Invalid or expired refresh token."
                };
            }
            var user = await _userManager.FindByIdAsync(existingRefreshToken.UserId);
            if (user == null)
            {
                return new AuthResponse
                {
                    Message = "User not found."
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Candidate";
            var newJwtToken = await GenerateJwtTokenAsync(user);
            var newRefreshToken = GenerateRefreshToken();
            _refreshToken.Delete(existingRefreshToken);
            var userRefreshToken = new RefreshToken
            {
                Token = newRefreshToken.Token,
                Expires = newRefreshToken.Expires,
                UserId = user.Id
            };
            await _refreshToken.AddAsync(userRefreshToken);
            await _unitOfWork.CommitAsync();
            return new AuthResponse
            {
                IsAuthenticated = true,
                Message = "Token refreshed successfully.",
                Email = user.Email,
                Role = role,
                Token = newJwtToken,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.Expires
            };
        }
        public async Task<bool> RevokeTokenAsync(string token)
        {
            var existingRefreshToken = (await _refreshToken.GetAllAsync()).FirstOrDefault(rt => rt.Token == token);
            if (existingRefreshToken == null || !existingRefreshToken.IsActive)
            {
                return false;
            }
            _refreshToken.Delete(existingRefreshToken);
            await _unitOfWork.CommitAsync();
            return true;
        }

        private async Task<string> GenerateJwtTokenAsync(IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            foreach (var claim in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, claim));
            }
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? "SJF8BMehVmHiC1VpepCM6iTLgRmvOmzwpDltYwMTbaB"));

            var token = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:DurationInMinutes"] ?? "60")),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(token);
            return tokenHandler.WriteToken(securityToken);
        }
        private (string Token, DateTime Expires) GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return (Convert.ToBase64String(randomNumber), DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:RefreshTokenDurationInDays"] ?? "7")));
        }
    }
}
