namespace JWTAuthentication.API.Dtos.Usuarios
{
    public record UsuarioDto(
        string Email,
        string Username,
        string PasswordHash,
        int? Id = null,
        DateTime? CreateAt = null,
        DateTime? UpdateAt = null)
    {

    }
}
