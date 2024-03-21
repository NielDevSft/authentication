using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.JwsClaims;
using JWTAuthentication.Domain.Usuarios.Repository;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.Repository;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims.Repository;
using JWTAuthentication.Domain.Usuarios.Service;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;

namespace JWTAuthentication.Application.Services
{
    public class UsuarioService(IUsuarioRepository usuarioRepository,
        IRoleRepository roleRepository,
        IRoleJwtClaimRepository roleJwtClaimRepository) : IUsuarioService
    {

        public async Task<Usuario> Create(Usuario usuario)
        {
            try
            {
                if (!usuario.IsValid())
                    throw new Exception(JsonConvert
                        .SerializeObject(usuario
                        .ValidationResult
                        .Errors));

                usuarioRepository.Add(usuario);
                usuarioRepository.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { usuarioRepository.Dispose(); }
            return usuario;
        }

        public async Task Delete(Guid uuid)
        {

            usuarioRepository.Remove(await GetById(uuid));
            usuarioRepository.SaveChanges();
            usuarioRepository.Dispose();

        }

        public async Task<List<Usuario>> GetAll()
        {

            var usuairoList = new List<Usuario>();
            try
            {
                usuairoList.AddRange(await usuarioRepository.FindAllWhereAsync(i => !i.Removed));
            }
            catch (Exception ex)
            {
                return usuairoList;
            }
            finally
            {
                usuarioRepository.Dispose();
            }
            return usuairoList;
        }

        public async Task<Usuario> GetById(Guid Uuid)
        {


            var usuarioFound = (await usuarioRepository
                .FindAllWhereAsync(i => i.Uuid == Uuid && !i.Removed)).FirstOrDefault();

            if (usuarioFound! == null!)
            {
                throw new Exception("Item não encontrado");
            }
            usuarioRepository.Dispose();
            return usuarioFound;



        }

        public async Task<Usuario> SetRoleList(Guid uuid, ICollection<Guid> roleIdList)
        {
            var roles = new List<Role>();
            Usuario usuario = null!;
            var usuarioTask = usuarioRepository.GetByIdAsync(uuid);
            var rolesTask = roleRepository.FindAllWhereAsync(r => roleIdList.Contains(r.Uuid));
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

            JwtClaim claim = await roleJwtClaimRepository.FindRoleJwtClaimExisting(roles.ToList());

            if (claim != null!)
            {
                usuario.JwtClaimUuid = claim.Uuid;
                usuario.JwtClaims = claim;
                usuarioRepository.Update(usuario);
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
            usuarioRepository.Update(usuario);

            usuarioRepository.SaveChanges();
            return usuario;
        }

        private static string BuildSubjectClaim(List<Role> roles)
        {
            var subject = roles.First().Name;
            roles.Remove(roles.First());
            roles.Select(r => r.Name).ToList().ForEach(n => subject += $"|{n}");
            return subject;
        }

        public async Task<Usuario> Update(Guid uuid, Usuario usuario)
        {
            Usuario? usuarioFound = null;

            usuarioFound = await usuarioRepository.GetByIdAsync(uuid)!;
            usuarioFound!.Email = usuario.Email;
            usuarioFound!.Username = usuario.Username;
            usuarioFound.PasswordHash = usuario.PasswordHash;
            usuarioRepository.Update(usuarioFound);
            usuarioRepository.SaveChanges();

            usuarioRepository.Dispose();

            return usuarioFound;
        }



    }
}
