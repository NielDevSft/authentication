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
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JWTAuthentication.Application.Services
{
    public class UsuarioService(IUsuarioRepository usuarioRepository,
        IRoleRepository roleRepository, IRoleJwtClaimRepository roleJwtClaimRepository) : IUsuarioService
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
                usuarioRepository.Remove(await GetById(id));
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

        public async Task<Usuario> GetById(int id)
        {
            
            try
            {
                var usuarioFound = usuarioRepository
                    .FirstOrDefault(i => i.Id == id && !i.Removed);

                if (usuarioFound! == null)
                {
                    throw new Exception("Item não encontrado");
                }
                return usuarioFound;

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

        public async Task<Usuario> SetRoleList(int id, ICollection<int> roleIdList)
        {
            var roles = new List<Role>();
            Usuario usuario = null;
            var usuarioTask = usuarioRepository.GetByIdAsync(id);
            var rolesTask = roleRepository.FindAllWhereAsync(r => roleIdList.Contains(r.Id));
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
            try
            {
                JwtClaim claim = await roleJwtClaimRepository.FindRoleJwtClaimExisting(roles.ToList());

                if (claim != null!)
                {
                    usuario.JwtClaimId = claim.Id;
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
                        RoleId = r.Id,
                        Role = r,
                    }).ToList();
                usuarioRepository.Update(usuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                usuarioRepository.SaveChanges();
            }
            return usuario;
        }

        private static string BuildSubjectClaim(List<Role> roles)
        {
            var subject = roles.First().Name;
            roles.Remove(roles.First());
            roles.Select(r => r.Name).ToList().ForEach(n => subject += $"|{n}");
            return subject;
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
