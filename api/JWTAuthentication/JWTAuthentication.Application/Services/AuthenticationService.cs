using JWTAuthentication.Application.Abstractions;
using JWTAuthentication.Domain.Authenticatios;
using JWTAuthentication.Domain.Authenticatios.Services;
using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole.Repository;
using JWTAuthentication.Domain.Usuarios.Repository;

namespace JWTAuthentication.Application.Services
{
    public class AuthenticationService(IUsuarioRepository usuarioRepository,
        IUsuarioRoleRepository usuarioRoleRepository,
        IJWTProvider jwtProvider) : IAuthenticationService
    {
        public async Task<string> Login(Authentication authentication)
        {
            try
            {
                if (!authentication.IsValid())
                {
                    throw new ArgumentException(authentication
                        .ValidationResult
                        .Errors
                        .ToString()); ;
                }
                var user = (await usuarioRepository
                    .FindAllWhereAsync(x => x.Email == authentication.Email))
                    .First();
                if (user == null || user.PasswordHash != authentication.Password)
                {
                    throw new ArgumentException("" +
                        "Usuário ou senha inválidos");
                }
                var userRoles = await usuarioRoleRepository
                    .FindAllWhereAsync(ur => ur.UsuarioId == user.Id);

                string token = jwtProvider.GenerateToken(user,
                    userRoles.Select(ur => ur.Role).ToList()!);

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

    }
}
