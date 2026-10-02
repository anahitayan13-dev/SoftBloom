using SoftBloom.Modelos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SoftBloom.Modelos
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        [Column("id_producto")]
        public int IdProducto { get; set; }

        [Column("nombre_producto")]
        [MaxLength(100)]
        [Required]
        public string NombreProducto { get; set; }

        [Column("descripcion")]
        [MaxLength(200)]
        public string? Descripcion { get; set; }

        [Column("precio", TypeName = "numeric(10,2)")]
        [Required]
        public decimal Precio { get; set; }

        [Column("stock")]
        public int Stock { get; set; } = 0;

        [Column("id_categoria")]
        [Required]
        public int IdCategoria { get; set; }

        [Column("id_material")]
        [Required]
        public int IdMaterial { get; set; }

        // Navegación
        [JsonIgnore]
        [ForeignKey("IdCategoria")]
        public Categoria? Categoria { get; set; }

        [JsonIgnore]
        [ForeignKey("IdMaterial")]
        public Material? Material { get; set; }

        [JsonIgnore]
        public List<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();
    }
}