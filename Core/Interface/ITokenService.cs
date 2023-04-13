using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface ITokenService
    {
        string CreateToken(Usuarios usuarios,string rol);

    }
}
