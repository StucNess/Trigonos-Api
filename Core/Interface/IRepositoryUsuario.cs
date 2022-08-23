using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IRepositoryUsuario
    {
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int Id);
        bool ExisteUsuario(string usuario);
        Usuario Registro (Usuario usuario, string password);
        Usuario Login(string usuario, string password);
        bool Save();
    }
}
