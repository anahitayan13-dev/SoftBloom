using Microsoft.EntityFrameworkCore;
using SoftBloom.Modelos;

public class SoftBloomAPIContext(
    DbContextOptions<SoftBloomAPIContext> options
) : DbContext(options)
{
    public DbSet<Categoria> Categorias { get; set; } = default!;
    public DbSet<Cliente> Clientes { get; set; } = default!;
    public DbSet<Material> Materiales { get; set; } = default!;
    public DbSet<Producto> Productos { get; set; } = default!;
    public DbSet<Pedido> Pedidos { get; set; } = default!;
    public DbSet<DetallePedido> DetallesPedidos { get; set; } = default!;
}