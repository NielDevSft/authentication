using JWTAuthentication.Application.Abstractions;
using JWTAuthentication.Domain.Authentications;
using JWTAuthentication.Domain.Authentications.Services;
using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole.Repository;
using JWTAuthentication.Domain.Usuarios.Repository;

namespace JWTAuthentication.Application.Services
{
    public class AuthenticationService(IUsuarioRepository usuarioRepository,
        IRoleJwtClaimRepository roleJwtClaimRepository,
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
                    .FindAllWhereAsync(ur => ur.JwtClaim!.Id == user.JwtClaimId);

                string token = jwtProvider.GenerateToken(user,
                    roleClaims.Select(ur => ur.Role).ToList()!);

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
