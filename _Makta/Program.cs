using Makta;
using Common;
using Data;
using Data.Repositories;
using Entities;
using Makta.Areas._Setting.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Email;
using System;
using WebFramework.Configuration;
using WebFramework.CustomMapping;
using Makta.api;


var builder = WebApplication.CreateBuilder(args);

var _siteSetting = builder.Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
WebSiteSetting.Common = _siteSetting.CommonSetting;

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
    policy =>
        {
            ////policy.WithOrigins(_siteSetting.CommonSetting.Url, $"https://www.{_siteSetting.CommonSetting.Domain}");
        });
});

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.InitializeAutoMapper();

//*************************

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(identityOptions =>
{
    identityOptions.Password.RequireDigit = _siteSetting.IdentitySetting.PasswordRequireDigit;
    identityOptions.Password.RequiredLength = _siteSetting.IdentitySetting.PasswordRequiredLength;
    identityOptions.Password.RequireNonAlphanumeric = _siteSetting.IdentitySetting.PasswordRequireNonAlphanumic;
    identityOptions.Password.RequireUppercase = _siteSetting.IdentitySetting.PasswordRequireUppercase;
    identityOptions.Password.RequireLowercase = _siteSetting.IdentitySetting.PasswordRequireLowercase;
    identityOptions.User.RequireUniqueEmail = _siteSetting.IdentitySetting.RequireUniqueEmail;
    identityOptions.SignIn.RequireConfirmedEmail = false;
    identityOptions.SignIn.RequireConfirmedPhoneNumber = false;
    identityOptions.Lockout.MaxFailedAccessAttempts = 5;
    identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    identityOptions.Lockout.AllowedForNewUsers = false;
}).AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.AccessDeniedPath = "/";
    options.LoginPath = "/";
});

//*************************


builder.Services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 268435456; });
builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

builder.Services.AddRazorPages();

builder.Services.AddSingleton(_siteSetting.EmailSetting);

builder.Services.AddSingleton(_siteSetting.CommonSetting);

builder.Services.AddSingleton(_siteSetting.SMSSetting);

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddMvc(options =>
{
    ////options.Filters.Add(new CheckRolesAsyncFilter());
});

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.AreaViewLocationFormats.Add($"/Areas/SiteUser/Views/Shared/{{0}}.cshtml");
    options.AreaPageViewLocationFormats.Add($"/Areas/SitePublic/Views/Shared/{{0}}.cshtml");
});

builder.Services.AddDataProtection();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();

app.IntializeDatabase();

//app.UseCustomExceptionHandler();

app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "UserAreaRoute",
        areaName: $"SiteUser",
        pattern: "User/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapAreaControllerRoute(
        name: "PublicAreaRoute",
        areaName: $"SitePublic",
        pattern: "/{controller=Home}/{action=Index}/{id?}");


    endpoints.MapControllerRoute(
        name: "default",
        pattern: $"{{area=Site}}Public/{{controller=Home}}/{{action=Index}}/{{id?}}"
    );

    endpoints.MapRazorPages();
});

app.Run();