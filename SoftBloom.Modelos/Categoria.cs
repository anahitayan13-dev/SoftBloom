using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SoftBloom.Modelos
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        [Column("id_categoria")]
        public int IdCategoria { get; set; }

        [Column("nombre_categoria")]
        [MaxLength(50)]
        [Required]
        public string NombreCategoria { get; set; }

        [Column("descripcion")]
        [MaxLength(200)]
        public string? Descripcion { get; set; }

        // Navegación
        [JsonIgnore]
        public List<Producto> Productos { get; set; } = new List<Producto>();
    }
}
