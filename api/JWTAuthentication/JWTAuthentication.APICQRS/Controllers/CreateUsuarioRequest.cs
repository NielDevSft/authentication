﻿namespace JWTAuthentication.APICQRS.Controllers
{
    public class CreateUsuarioRequest
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}