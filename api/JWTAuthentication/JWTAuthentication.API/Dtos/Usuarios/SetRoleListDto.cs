namespace JWTAuthentication.API.Dtos.Usuarios
{
    public record SetRoleListDto(string uuid, IEnumerable<string> roles)
    {
    }
}
