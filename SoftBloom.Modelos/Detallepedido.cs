using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SoftBloomModelos
{
    [Table("DetallesPedido")]
    public class DetallePedido
    {
        [Key]
        [Column("id_detalle")]
        public int IdDetalle { get; set; }

        [Column("id_pedido")]
        [Required]
        public int IdPedido { get; set; }

        [Column("id_producto")]
        [Required]
        public int IdProducto { get; set; }

        [Column("cantidad")]
        [Required]
        public int Cantidad { get; set; }

        [Column("precio_unitario", TypeName = "numeric(10,2)")]
        [Required]
        public decimal PrecioUnitario { get; set; }

        // Navegación
        [JsonIgnore]
        [ForeignKey("IdPedido")]
        public Pedido? Pedido { get; set; }

        [JsonIgnore]
        [ForeignKey("IdProducto")]
        public Producto? Producto { get; set; }
    }
}