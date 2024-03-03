using JWTAuthentication.Domain.Core.Interfaces;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole.Repository;
using JWTAuthentication.Domain.Usuarios.Repository;
using JWTAuthentication.Persistence.Abstractions;
using JWTAuthentication.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Persistence.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AuthenticationOrganizationContext context) : base(context)
        {
        }

        public bool TryGetValue(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string username)
        {
            throw new NotImplementedException();
        }
    }
}
