using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.Repository;
using JWTAuthentication.Domain.Usuarios.Roles.Service;

namespace JWTAuthentication.Application.Services
{
    public class RoleService(IRoleRepository roleRepository) : IRoleService
    {
        public async Task<Role> Create(Role role)
        {
            try
            {
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
                roleRepository.Remove(id);
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
