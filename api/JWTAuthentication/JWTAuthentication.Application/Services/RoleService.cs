using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.Repository;
using JWTAuthentication.Domain.Usuarios.Roles.Service;
using Newtonsoft.Json;

namespace JWTAuthentication.Application.Services
{
    public class RoleService(IRoleRepository roleRepository) : IRoleService
    {
        public async Task<Role> Create(Role role)
        {
            try
            {
                if (!role.IsValid())
                    throw new Exception(JsonConvert
                        .SerializeObject(role
                        .ValidationResult
                        .Errors));

                role.Name = role.Name.ToUpper();
                roleRepository.Add(role);
                roleRepository.SaveChanges();

                roleRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { roleRepository.Dispose(); }
            return role;
        }

        public async Task Delete(int id)
        {
            try
            {
                roleRepository.Remove(await GetById(id));
                roleRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                roleRepository.Dispose();
            }
        }

        public async Task<Role> GetById(int id)
        {
            try
            {
                var roleFound = roleRepository
                    .FirstOrDefault(i => i.Id == id && !i.Removed);

                if (roleFound! == null!)
                {
                    throw new Exception("Item não encontrado");
                }
                return roleFound;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                roleRepository.Dispose();
            }
        }

        public async Task<List<Role>> GetAll()
        {
            var roleList = new List<Role>();
            try
            {
                roleList.AddRange(await roleRepository.FindAllWhereAsync(i => !i.Removed));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                roleRepository.Dispose();
            }
            return roleList;
        }
    }
}
