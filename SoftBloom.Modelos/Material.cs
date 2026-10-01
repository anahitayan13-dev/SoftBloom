using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SoftBloomModelos
{
    [Table("Materiales")]
    public class Material
    {
        [Key]
        [Column("id_material")]
        public int IdMaterial { get; set; }

        [Column("nombre_material")]
        [MaxLength(100)]
        [Required]
        public string NombreMaterial { get; set; }

        [Column("tipo_material")]
        [MaxLength(50)]
        public string? TipoMaterial { get; set; }

        [Column("cantidad_disponible", TypeName = "numeric(10,2)")]
        [Required]
        public decimal CantidadDisponible { get; set; }

        [Column("unidad_medida")]
        [MaxLength(20)]
        public string? UnidadMedida { get; set; }

        [Column("costo", TypeName = "numeric(10,2)")]
        public decimal? Costo { get; set; }

        // Navegación
        [JsonIgnore]
        public List<Producto> Productos { get; set; } = new List<Producto>();
    }
}