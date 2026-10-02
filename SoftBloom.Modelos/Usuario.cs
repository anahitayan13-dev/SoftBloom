using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftBloom.Modelos
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        public string nombre { get; set; }

        public string apellido { get; set; }

        public string correo { get; set; }

        public string nombreUsuario { get; set; }

        public string contrasenia { get; set; }
        public string rol { get; set; } = "Cliente";

    }
}
