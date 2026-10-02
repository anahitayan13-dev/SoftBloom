using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBloom.Servicios.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Login(string correo, string contrasenia);

        Task<bool> Register(string nombre,
            string apellido,
            string correo,
            string nombreUsuario,
            string contrasenia);
    }
}
