using Microsoft.EntityFrameworkCore;
using MiSalonBellezaNicteHa.Data;

var builder = WebApplication.CreateBuilder(args);

// Agregar soporte para Controladores y Vistas
builder.Services.AddControllersWithViews();

// Agregar soporte para Sesiones (Bloqueo de Seguridad)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Conectar DbContext con SQL Server / InMemory
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
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();