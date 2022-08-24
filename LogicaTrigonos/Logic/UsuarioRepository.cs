using Core.Entities;
using Core.Interface;
using LogicaTrigonos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaTrigonos.Logic
{
    public class UsuarioRepository : IRepositoryUsuario
    {
        private readonly TrigonosDBContext _bd;

        public UsuarioRepository(TrigonosDBContext bd)
        {
            _bd = bd;
        }
        public bool ExisteUsuario(string usuario)
        {
            if (_bd.Usuario.Any(x => x.UsuarioA == usuario))
            {
                return true;
            }
            return false;
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _bd.Usuario.OrderBy(c => c.UsuarioA).ToList();
        }

        public Usuario GetUsuario(int Id)
        {
            return _bd.Usuario.FirstOrDefault(c => c.ID == Id);
        }

       

        

        public bool Save()
        {
            return _bd.SaveChanges()>= 0 ? true : false;
        }
        public Usuario Login(string usuario, string password)
        {
            var user = _bd.Usuario.FirstOrDefault(x => x.UsuarioA == usuario);
            if (user == null)
            {
                return null;
            }

            if (! VerificaPasswordHash(password,user.PasswordHash,user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        private bool VerificaPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var hashComputado = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i<hashComputado.Length;i++)
                {
                    if (hashComputado[i] != passwordHash[i]) return false;
                    
                }
            }
            return true;
        }
        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            };
        }

        public Usuario Registro(Usuario usuario, string password)
        {
            byte[] passwordHash, passwordSalt;
            CrearPasswordHash(password, out passwordHash, out passwordSalt);
            usuario.PasswordHash = passwordHash;
            usuario.PasswordSalt = passwordSalt;

            _bd.Usuario.Add(usuario);
            Save();
            return usuario;
        }
    }
}
