using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Notes.Application;
using Notes.Persistence;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Notes.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;
using Notes.WebApi;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

internal class Program
{
    private static void Main(string[] args)
    {
        var cfgbuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = cfgbuilder.Build();

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddRazorPages();

        builder.Services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
        }).AddVersionedApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>,ConfigureSwaggerOptions>();

        builder.Services.AddSwaggerGen(cfg =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            cfg.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            cfg.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
        });

        builder.Services.AddApplication();
        builder.Services.AddPersistence(configuration);

        builder.Services.AddCors(options => options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        }));

        builder.Services.AddControllers();

        builder.Services.AddAuthentication(cfg =>
        {
            cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer("Bearer",opt =>
            {
                opt.Authority = "https://localhost:44344/";
                opt.Audience = "NotesWebAPI";
                opt.RequireHttpsMetadata = false;
            });

        

        var app = builder.Build();

        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            try
            {
                var context = serviceProvider.GetRequiredService<NotesDbContext>();
                DbInitializer.Initialize(context);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

        }

        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            foreach(var description in provider.ApiVersionDescriptions)
            {
                config.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                config.RoutePrefix = string.Empty;
            }
        });

        app.UseCustomExceptionHandler();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseCors("AllowAll");

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(cfg =>
        {
            cfg.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        app.MapRazorPages();

        app.Run();
    }

}