namespace JWTAuthentication.Domain.Authenticatios.Services
{
    public interface IAuthenticationService
    {
        public Task<string> Login(Authentication authentication);
    }
}
