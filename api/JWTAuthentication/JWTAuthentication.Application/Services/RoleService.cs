using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.Repository;
using JWTAuthentication.Domain.Usuarios.Roles.Service;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace JWTAuthentication.Application.Services
{
    public class RoleService(IRoleRepository roleRepository) : IRoleService
    {
        public async Task<Role> Create(Role role, CancellationToken cancellationToken)
        {

            if (!role.IsValid())
                throw new Exception(JsonSerializer
                    .Serialize(role
                    .ValidationResult
                    .Errors));

            role.Name = role.Name.ToUpper();
            await roleRepository.AddAsync(role, cancellationToken);
            await roleRepository.SaveChangesAsync(cancellationToken);

            return role;
        }

        public async Task Delete(Guid uuid, CancellationToken cancellationToken)
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(roleRepository.RemoveAsync(uuid, cancellationToken));

        }

        public async Task<Role> GetById(Guid uuid, CancellationToken cancellationToken)
        {
            var roleFound = (await roleRepository
                .FindAllWhereAsync(i => i.Uuid == uuid && !i.Removed, cancellationToken)).FirstOrDefault();

            if (roleFound! == null!)
            {
                throw new Exception("Item não encontrado");
            }

            return roleFound;
        }

        public async Task<List<Role>> GetAll(CancellationToken cancellationToken)
        {
            var roleList = new List<Role>();
            roleList.AddRange(await roleRepository.FindAllWhereAsync(i => !i.Removed, cancellationToken));

            return roleList;
        }
    }
}
