using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GimManager.Data;
using GimManager.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    // Configuraciones adicionales de identidad si las necesitas
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Agregar soporte para controllers (por si necesitas APIs más adelante)
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages(options => {
    // Configuración específica para Razor Pages
    options.Conventions.AuthorizeFolder("/Inventario");
});

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

// Habilitar redirección a login para áreas protegidas
app.Use(async (context, next) => {
    if (!context.User.Identity.IsAuthenticated &&
        !context.Request.Path.StartsWithSegments("/Identity/Account/Login") &&
        !context.Request.Path.StartsWithSegments("/Identity/Account/Register") &&
        !context.Request.Path.StartsWithSegments("/Error"))
    {
        context.Response.Redirect("/Identity/Account/Login?returnUrl=" + Uri.EscapeDataString(context.Request.Path));
        return;
    }
    await next();
});

app.MapRazorPages();
app.MapControllers(); // Para APIs

app.UseSession();

// Asegurar que la base de datos esté creada
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.Run();