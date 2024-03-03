namespace JWTAuthentication.Domain.Authentications.Services
{
    public interface IAuthenticationService
    {
        public Task<string> Login(Authentication authentication);
    }
}
