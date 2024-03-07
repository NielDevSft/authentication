using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.JwsClaims;
using JWTAuthentication.Domain.Usuarios.JwsClaims.Repository;
using JWTAuthentication.Domain.Usuarios.Repository;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.Repository;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims;
using JWTAuthentication.Domain.Usuarios.Service;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace JWTAuthentication.Application.Services
{
    public class UsuarioService(IUsuarioRepository usuarioRepository,
        IRoleRepository roleRepository,
        IJwtClaimRepository jwtClaimRepository) : IUsuarioService
    {
        private Task<dynamic> usuario;

        public async Task<Usuario> Create(Usuario usuario)
        {
            try
            {
                usuarioRepository.Add(usuario);
                usuarioRepository.SaveChanges();

                usuarioRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { usuarioRepository.Dispose(); }
            return usuario;
        }

        public async Task Delete(int id)
        {
            try
            {
                usuarioRepository.Remove(id);

                usuarioRepository.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                usuarioRepository.Dispose();
            }
        }

        public async Task<List<Usuario>> GetAll()
        {

            var usuairoList = new List<Usuario>();
            try
            {
                usuairoList.AddRange(usuarioRepository.FindAllWhere(i => !i.Removed));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                usuarioRepository.Dispose();
            }
            return usuairoList;
        }

        public async Task<Usuario> GetById(int id)
        {
            Usuario? usuarioFound = null;
            try
            {
                usuarioFound = usuarioRepository
                    .FirstOrDefault(i => i.Id == id && !i.Removed);

                if (usuarioFound! == null)
                {
                    throw new Exception("Item não encontrado");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                usuarioRepository.Dispose();
            }
            return usuarioFound;
        }

        public async Task<Usuario> SetRoleList(int id, ICollection<int> roleIdList)
        {

            var usuarioTask = usuarioRepository.GetByIdAsync(id);
            var rolesTask = roleRepository.GetManyAllRolesByIdAsync(roleIdList);
            var roleJwtClaim = new List<RoleJwtClaim>();

            await Task.WhenAll(usuarioTask, rolesTask);
            var roles = new List<Role>();
            roles.AddRange(rolesTask.Result);
            var usuario = usuarioTask.Result;

            if (roles.IsNullOrEmpty())
            {
                throw new ArgumentException("Uma ou mais roles não encontradas");
            }
            roles = roles.ToList();
            if (usuario is { Removed: false, Active: true })
            {
                var subject = roles.First().Name;
                roles.Remove(roles.First());
                roles.Select(r => r.Name).ToList().ForEach(n => subject += $"|{n}");
                usuario.JwtClaims = new JwtClaim()
                {
                    Subject = subject,
                };

                roles.ToList().ForEach(r =>
                {
                    usuario.JwtClaims.RoleJwtClaims.Add(new RoleJwtClaim()
                    {
                        RoleId = r.Id,
                        Role = r,
                    });
                });
            }
            else
            {
                throw new ArgumentException("Usuário não encontrado");
            }



            usuarioRepository.Update(usuario);
            usuarioRepository.SaveChanges();
            return usuario;


        }

        public async Task<Usuario> Update(int id, Usuario usuario)
        {
            Usuario? usuarioFound = null;
            try
            {
                usuarioFound = usuarioRepository.GetById(id)!;
                usuarioFound.Email = usuario.Email;
                usuarioFound.Username = usuario.Username;
                usuarioFound.PasswordHash = usuario.PasswordHash;
                usuarioRepository.Update(usuarioFound);
                usuarioRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                usuarioRepository.Dispose();
            }

            return usuarioFound;
        }
    }
}
