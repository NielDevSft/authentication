using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.Repository;
using JWTAuthentication.Domain.Usuarios.Service;

namespace JWTAuthentication.Application.Services
{
    public class UsuarioService(IUsuarioRepository usuarioRepository) : IUsuarioService
    {
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
