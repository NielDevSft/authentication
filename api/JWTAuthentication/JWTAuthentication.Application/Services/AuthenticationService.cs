﻿using JWTAuthentication.Application.Abstractions;
using JWTAuthentication.Domain.Authentications;
using JWTAuthentication.Domain.Authentications.Jwts;
using JWTAuthentication.Domain.Authentications.Services;
using JWTAuthentication.Domain.Usuarios.Repository;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JWTAuthentication.Application.Services
{
    public class AuthenticationService(IUsuarioRepository usuarioRepository,
        IRoleJwtClaimRepository roleJwtClaimRepository,
        IJWTProvider jwtProvider) : IAuthenticationJwtService
    {
        public async Task<JwtAuthResult> Login(Authentication authentication)
        {
            try
            {
                if (!authentication.IsValid())
                {
                    throw new ArgumentException(authentication
                        .ValidationResult
                        .Errors.First().ToString()); ;
                }
                var user = (await usuarioRepository
                    .FindAllWhereAsync(x => x.Email == authentication.Email))
                    .First();
                if (user == null || user.PasswordHash != authentication.Password)
                {
                    throw new ArgumentException("" +
                        "Usuário ou senha inválidos");
                }
                var roleClaims = await roleJwtClaimRepository
                    .FindAllWhereAsync(ur => ur.JwtClaim!.Id == user.JwtClaimId, "Role", "JwtClaim");

                var roles = roleClaims
                    .Select(ur => ur.Role);
                var rolesTxt = roles.Select(r => r.Name).First();

                var claims = new List<Claim>() {
                            new Claim(ClaimTypes.Name ,user.Username),
                            new Claim(ClaimTypes.Email ,user.Email),
                            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
                };
                claims.AddRange(roles
                    .Select(r => new Claim(ClaimTypes.Role, r.Name)));

                var token = jwtProvider.GenerateTokens(user.Username,
                   claims.ToArray(), DateTime.UtcNow);

                return token;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                usuarioRepository.Dispose();
            }
        }

        public async Task Logout(string userName)
        {
            jwtProvider.RemoveRefreshTokenByUserName(userName);
        }

        public async Task<JwtAuthResult> RefreshToken(string refreshToken, string accessToken)
        {
            var jwtResult = jwtProvider.Refresh(refreshToken, accessToken, DateTime.UtcNow);
            return jwtResult;
        }
    }
}
