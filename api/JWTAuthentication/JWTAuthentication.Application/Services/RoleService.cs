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

        public async Task Delete(Guid uuid)
        {
            await Task.Run(() => roleRepository.Remove(uuid));
            roleRepository.SaveChanges();
            roleRepository.Dispose();
        }

        public async Task<Role> GetById(Guid uuid)
        {
            var roleFound = (await roleRepository
                .FindAllWhereAsync(i => i.Uuid == uuid && !i.Removed)).FirstOrDefault();

            if (roleFound! == null!)
            {
                throw new Exception("Item não encontrado");
            }
            roleRepository.Dispose();
            return roleFound;



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
