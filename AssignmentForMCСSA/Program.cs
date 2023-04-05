using AssignmentForMCÑSA.Data.Db;
using AssignmentForMCÑSA.Data.Models;
using AssignmentForMCÑSA.Repositories.Abstract;
using AssignmentForMCÑSA.Repositories.Implementation;
using AssignmentForMCÑSA.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
    builder.Services.AddIdentity<ApplicationUserModel, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.AddScoped<IAuth, AuthRepository>();
    builder.Services.AddScoped<IAccount, AccountRepository>();
    builder.Services.AddScoped<IProduct, ProductRepository>();
    builder.Services.AddScoped<ISaveImg, SaveImgService>();
    builder.Services.AddScoped<ILanguage, LanguageService>();

    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
    builder.Services.AddMvc()
        .AddViewLocalization()
        .AddDataAnnotationsLocalization(options => {
            options.DataAnnotationLocalizerProvider = (type, factory) => {
                var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                return factory.Create("SharedResource", assemblyName.Name);
            };
        });

    builder.Services.Configure<RequestLocalizationOptions>(options => 
        {
            var supportedCultures = new List<CultureInfo> 
            {
                new CultureInfo("ru-RU"),
                new CultureInfo("en-US")
            };

            options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
    });

    builder.Services.AddControllersWithViews();
}

var app = builder.Build();
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Products}/{action=GetProducts}/{id?}");

    using (var scope = app.Services.CreateAsyncScope())
    {
        var serviceProvider = scope.ServiceProvider;
        await DbObjects.InitialAdmin(serviceProvider);
    };

    app.Run();
}