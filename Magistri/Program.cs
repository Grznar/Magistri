using Magistri.Application.Common.Interfaces;
using Magistri.Domain.Entities;
using Magistri.Infrastracture.Data;
using Magistri.Infrastracture.Repository;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
AppDomain.CurrentDomain.UnhandledException += (_, e) =>
{
    File.WriteAllText("/home/site/wwwroot/startup-error.txt",
                      e.ExceptionObject.ToString());
};
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(o =>
{
    o.Password.RequiredLength = 6;
    o.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(o =>
{
    o.Cookie.Name = "MagistriAuth";
    o.Cookie.HttpOnly = true;
    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    o.Cookie.SameSite = SameSiteMode.None;
    o.LoginPath = "/Auth/Login";
    o.AccessDeniedPath = "/Shared/Error";
    o.ExpireTimeSpan = TimeSpan.FromHours(1);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

   
}





if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
