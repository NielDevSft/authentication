using JWTAuthentication.Domain.Core.Interfaces;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Persistence.Abstractions;
using JWTAuthentication.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Persistence.Repositories
{
    public class UsuarioRepository : Repository<Usuario>
    {
        public UsuarioRepository(AuthenticationOrganizationContext context) : base(context)
        {
        }
    }
}
