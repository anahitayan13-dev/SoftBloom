
using SoftBloom.Consumer;
using SoftBloom.Modelos;
using SoftBloom.Servicios.Interfaces;
using SoftBloom.Servicios;


var builder = WebApplication.CreateBuilder(args);

CRUD<Categoria>.Endpoint = "https://localhost:7217/api/Categorias";
CRUD<Material>.Endpoint = "https://localhost:7217/api/Materiales";
CRUD<Cliente>.Endpoint = "https://localhost:7217/api/Clientes";
CRUD<Producto>.Endpoint = "https://localhost:7217/api/Productos";
CRUD<Pedido>.Endpoint = "https://localhost:7217/api/Pedidos";
CRUD<DetallePedido>.Endpoint = "https://localhost:7217/api/DetallesPedidos";
CRUD<Usuario>.Endpoint = "https://localhost:7217/api/Usuarios";



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Index";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for product
    // scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.Run();
