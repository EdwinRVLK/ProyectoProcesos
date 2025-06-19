using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GimManager.Data;
using GimManager.Models;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde el archivo de configuración
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configurar el DbContext para usar SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Habilitar las excepciones de desarrollo para la base de datos
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configuración de Identity
builder.Services.AddDefaultIdentity<AppUser>(options => {
    options.SignIn.RequireConfirmedAccount = false; // Configuración adicional
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Agregar soporte para controllers (si necesitas APIs más adelante)
builder.Services.AddControllersWithViews();

// Configuración para Razor Pages, protegiendo ciertas carpetas
builder.Services.AddRazorPages(options => {
    options.Conventions.AuthorizeFolder("/Inventario"); // Protege la carpeta de Inventario
});

// Habilitar la sesión de usuario
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

// Construir la aplicación
var app = builder.Build();

// Configuración del pipeline de la aplicación
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Middleware personalizado para permitir el acceso a ForgotPassword sin autenticación
app.Use(async (context, next) => {
    // Permitir acceso a páginas como Login, Register, ForgotPassword sin autenticación
    if (!context.User.Identity.IsAuthenticated &&
        !context.Request.Path.StartsWithSegments("/Identity/Account/Login") &&
        !context.Request.Path.StartsWithSegments("/Identity/Account/Register") &&
        !context.Request.Path.StartsWithSegments("/Identity/Account/ForgotPassword") && // Permitir ForgotPassword
        !context.Request.Path.StartsWithSegments("/Error"))
    {
        // Redirigir al login si no está autenticado
        context.Response.Redirect("/Identity/Account/Login?returnUrl=" + Uri.EscapeDataString(context.Request.Path));
        return;
    }
    await next();
});

// Configuración de sesiones
app.UseSession();

// Mapear las páginas Razor y controladores
app.MapRazorPages();
app.MapControllers(); // Para APIs

// Asegurar que la base de datos esté creada
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// Ejecutar la aplicación
app.Run();
