using JWTAuthentication.API.Dtos.Roles;

namespace JWTAuthentication.API.Dtos.Usuarios
{
    public record UsuarioDto(
        string Email,
        string Username,
        string PasswordHash,
        ICollection<RoleDto> roles,
        int? Id = null,
        DateTime? CreateAt = null,
        DateTime? UpdateAt = null)
    {

    }
}
