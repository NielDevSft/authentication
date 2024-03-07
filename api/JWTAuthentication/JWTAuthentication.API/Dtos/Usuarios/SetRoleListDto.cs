namespace JWTAuthentication.API.Dtos.Usuarios
{
    public record SetRoleListDto(int id, IEnumerable<int> roles)
    {
    }
}
