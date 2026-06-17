using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.JwsClaims;
using JWTAuthentication.Domain.Usuarios.Repository;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.Repository;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims.Repository;
using JWTAuthentication.Domain.Usuarios.Service;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace JWTAuthentication.Application.Services
{
    public class UsuarioService(IUsuarioRepository usuarioRepository,
        IRoleRepository roleRepository,
        IRoleJwtClaimRepository roleJwtClaimRepository) : IUsuarioService
    {

        public async Task<Usuario> Create(Usuario usuario, CancellationToken cancellationToken)
        {
            if (!usuario.IsValid())
                throw new Exception(JsonSerializer
                    .Serialize(usuario
                    .ValidationResult
                    .Errors));

            usuario.PasswordHash = ComputeSha1(usuario.PasswordHash);
            await usuarioRepository.AddAsync(usuario, cancellationToken);
            await usuarioRepository.SaveChangesAsync(cancellationToken);

            return usuario;
        }

        public Task Delete(Guid uuid, CancellationToken cancellationToken)
        {
            return usuarioRepository.RemoveAsync(uuid, cancellationToken);
        }

        public async Task<List<Usuario>> GetAll(CancellationToken cancellationToken)
        {

            var usuairoList = new List<Usuario>();

            usuairoList.AddRange(await usuarioRepository.FindAllWhereAsync(i => !i.Removed, cancellationToken));

            return usuairoList;
        }

        public async Task<Usuario> GetById(Guid Uuid, CancellationToken cancellationToken)
        {


            var usuarioFound = (await usuarioRepository
                .FindAllWhereAsync(i => i.Uuid == Uuid && !i.Removed, cancellationToken, "JwtClaims")).FirstOrDefault();
            if (usuarioFound! == null!)
            {
                throw new Exception("Item não encontrado");
            }
            if (usuarioFound.JwtClaims! != null!)
                usuarioFound.JwtClaims!.RoleJwtClaims = await roleJwtClaimRepository
                    .FindAllWhereAsync(rjc => rjc.JwtClaimUuid == usuarioFound.JwtClaimUuid, cancellationToken, "Role", "JwtClaim");

            return usuarioFound;

        }

        public async Task<Usuario> SetRoleList(Guid uuid, ICollection<Guid> roleIdList, CancellationToken cancellationToken)
        {
            var roles = new List<Role>();
            Usuario usuario = null!;
            var usuarioTask = usuarioRepository.GetByIdAsync(uuid, cancellationToken);
            var rolesTask = roleRepository.FindAllWhereAsync(r => roleIdList.Contains(r.Uuid), cancellationToken);
            var roleJwtClaim = new List<RoleJwtClaim>();

            await Task.WhenAll(usuarioTask, rolesTask);

            roles.AddRange(rolesTask.Result);
            usuario = usuarioTask.Result!;

            if (roles.IsNullOrEmpty())
            {
                throw new ArgumentException("Uma ou mais roles não encontradas");
            }
            if (usuario == null!)
            {
                throw new ArgumentException("Usuário não encontrado");
            }

            JwtClaim claim = await roleJwtClaimRepository.FindRoleJwtClaimExisting(roles.ToList(), CancellationToken.None);

            if (claim != null!)
            {
                usuario.JwtClaimUuid = claim.Uuid;
                usuario.JwtClaims = claim;
                await usuarioRepository.UpdateAsync(usuario, cancellationToken);
                return usuario;
            }
            usuario.JwtClaims = new JwtClaim()
            {
                Subject = BuildSubjectClaim(roles.ToList())
            };

            usuario
                .JwtClaims
                .RoleJwtClaims = roles.Select(r => new RoleJwtClaim()
                {
                    RoleUuid = r.Uuid,
                    Role = r,
                }).ToList();
            await usuarioRepository.UpdateAsync(usuario, cancellationToken);

            await usuarioRepository.SaveChangesAsync(cancellationToken);
            return usuario;
        }

        private static string ComputeSha1(string input)
        {
            var bytes = SHA1.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        private static string BuildSubjectClaim(List<Role> roles)
        {
            var subject = roles.First().Name;
            roles.Remove(roles.First());
            roles.Select(r => r.Name).ToList().ForEach(n => subject += $"|{n}");
            return subject;
        }

        public async Task<Usuario> Update(Guid uuid, Usuario usuario, CancellationToken cancellationToken)
        {
            Usuario? usuarioFound = null;

            usuarioFound = await usuarioRepository.GetByIdAsync(uuid, cancellationToken)!;
            usuarioFound!.Email = usuario.Email;
            usuarioFound!.Username = usuario.Username;
            usuarioFound.PasswordHash = ComputeSha1(usuario.PasswordHash);
            await usuarioRepository.UpdateAsync(usuarioFound, cancellationToken);
            await usuarioRepository.SaveChangesAsync(cancellationToken);

            return usuarioFound;
        }
    }
}
