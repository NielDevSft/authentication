namespace JWTAuthentication.API.Dtos.Roles
{
    public record RoleDto(string Name, DateTime CreateAt, DateTime UpdateAt, string Uuid = null!)
    {
    }
}
