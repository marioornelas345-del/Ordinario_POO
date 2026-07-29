using Microsoft.EntityFrameworkCore;
using MiSalonBellezaNicteHa.Data;

var builder = WebApplication.CreateBuilder(args);

// Agregar soporte para Controladores y Vistas
builder.Services.AddControllersWithViews();

// AQUÍ ESTÁ LA MAGIA: Conectar tu DbContext con SQL Server
builder.Services.AddDbContext<SalonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configurar las rutas HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();