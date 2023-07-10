using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class UsuarioSpecification : BaseSpecification<Usuarios>
    {
        public UsuarioSpecification(UsuarioSpecificationParams usuarioParams, string rol)
        :base (x =>
        (  string.IsNullOrEmpty(usuarioParams.Search) || x.Nombre.Contains(usuarioParams.Search)) &&
        (string.IsNullOrEmpty(usuarioParams.Nombre)|| x.Nombre.Contains(usuarioParams.Nombre))&&
        (string.IsNullOrEmpty(usuarioParams.Apellido) || x.Apellido.Contains(usuarioParams.Apellido))  && rol=="Administrador"? x.Role != rol && x.Role !=  "Admin Jefe": x.Role == x.Role  && x.Role != rol

        )
        {
         
            ApplyPaging(usuarioParams.PageSize * (usuarioParams.PageIndex - 1), usuarioParams.PageSize);
            if (string.IsNullOrEmpty(usuarioParams.Sort))
            {
                switch (usuarioParams.Sort)
                {
                    case "nombreAsc":
                        AddOrderBy(u => u.Nombre);
                        break;
                    case "nombreDesc":
                        AddOrderByDescending(u => u.Nombre);
                        break;
                    case "emailAsc":
                        AddOrderBy(u => u.Email);
                        break;
                    case "emailDesc":
                        AddOrderByDescending(u => u.Email);
                        break;
                    default:
                        AddOrderBy(u => u.Nombre);
                        break;

                }
            }
        }
        public UsuarioSpecification(UsuarioSpecificationParams usuarioParams, string rol,string id)
        : base(x =>
        (string.IsNullOrEmpty(usuarioParams.Search) || x.Nombre.Contains(usuarioParams.Search)) &&
        (string.IsNullOrEmpty(usuarioParams.Nombre) || x.Nombre.Contains(usuarioParams.Nombre)) &&
        (string.IsNullOrEmpty(usuarioParams.Apellido) || x.Apellido.Contains(usuarioParams.Apellido)) &&
        ((x.Role != rol && x.Id != id) || x.Id == id)   &&  (rol == "Administrador" ? x.Role != "Admin Jefe": x.Role == x.Role)

        )
        {

            ApplyPaging(usuarioParams.PageSize * (usuarioParams.PageIndex - 1), usuarioParams.PageSize);
            if (string.IsNullOrEmpty(usuarioParams.Sort))
            {
                switch (usuarioParams.Sort)
                {
                    case "nombreAsc":
                        AddOrderBy(u => u.Nombre);
                        break;
                    case "nombreDesc":
                        AddOrderByDescending(u => u.Nombre);
                        break;
                    case "emailAsc":
                        AddOrderBy(u => u.Email);
                        break;
                    case "emailDesc":
                        AddOrderByDescending(u => u.Email);
                        break;
                    default:
                        AddOrderBy(u => u.Nombre);
                        break;

                }
            }
        }
        public UsuarioSpecification( string rol, string id)
       : base(x =>
       ((x.Role != rol && x.Id != id) || x.Id == id) && (rol == "Administrador" ? x.Role != "Admin Jefe" : x.Role == x.Role)

       )
        {

           
        }


    }
}
