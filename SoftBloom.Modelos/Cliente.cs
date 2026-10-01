using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SoftBloomModelos
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [Column("cedula")]
        [MaxLength(10)]
        [Required]
        public string Cedula { get; set; }

        [Column("nombres")]
        [MaxLength(50)]
        [Required]
        public string Nombres { get; set; }

        [Column("apellidos")]
        [MaxLength(50)]
        [Required]
        public string Apellidos { get; set; }

        [Column("telefono")]
        [MaxLength(15)]
        public string? Telefono { get; set; }

        [Column("email")]
        [MaxLength(100)]
        public string? Email { get; set; }

        // Navegación
        [JsonIgnore]
        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}