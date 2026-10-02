using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SoftBloom.Modelos
{
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        [Column("id_pedido")]
        public int IdPedido { get; set; }

        [Column("fecha_pedido", TypeName = "timestamp without time zone")]
        [Required]
        public DateTime FechaPedido { get; set; }

        [Column("fecha_entrega", TypeName = "date")]
        public DateOnly? FechaEntrega { get; set; }

        [Column("estado_pedido")]
        [MaxLength(20)]
        public string EstadoPedido { get; set; } = "Pendiente";

        [Column("observaciones")]
        [MaxLength(200)]
        public string? Observaciones { get; set; }

        [Column("id_cliente")]
        [Required]
        public int IdCliente { get; set; }

        // Navegación
        [JsonIgnore]
        [ForeignKey("IdCliente")]
        public Cliente? Cliente { get; set; }

        [JsonIgnore]
        public List<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();
    }
}