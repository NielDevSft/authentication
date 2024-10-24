﻿using JWTAuthentication.Domain.Usuarios.JwsClaims;
using JWTAuthentication.Domain.Usuarios.JwsClaims.Repository;
using JWTAuthentication.Persistence.Abstractions;
using JWTAuthentication.Persistence.Contexts;
using Microsoft.Extensions.Logging;

namespace JWTAuthentication.Persistence.Repositories.PostgreSQL
{
    public class JwtClaimRepository : Repository<JwtClaim>, IJwtClaimRepository
    {
        public JwtClaimRepository(AuthenticationOrganizationContextPostgreSQL context, ILogger<Repository<JwtClaim>> logger) : base(context, logger)
        {

        }
    }
}