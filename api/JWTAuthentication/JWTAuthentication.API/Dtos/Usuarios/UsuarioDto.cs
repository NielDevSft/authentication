using JWTAuthentication.API.Dtos.Roles;

namespace JWTAuthentication.API.Dtos.Usuarios
{
    public record UsuarioDto(
        string Email,
        string Username,
        string PasswordHash,
        ICollection<RoleDto> roles,
        string? Uuid = null,
        DateTime? CreateAt = null,
        DateTime? UpdateAt = null)
    {

    }
}
