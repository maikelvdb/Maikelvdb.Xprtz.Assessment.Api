using System.Text.Json;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Maikelvdb.Xprtz.Assessment.Api.Framework.Filters;
using Microsoft.AspNetCore.Identity;
using Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Commands;
using Maikelvdb.Xprtz.Assessment.Api.Framework.HostedService;

namespace Maikelvdb.Xprtz.Assessment.Api
{
    public static class Startup
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssemblyContaining<CreateShowCommand>();

            services.AddHostedService<DailyDataRetrieverHostedService>();

            services.AddMemoryCache();
            builder.Services.AddControllers(options => {
                options.Filters.Add<FluentValidationActionFilter>();
            })
                .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.SerializerOptions.PropertyNameCaseInsensitive = true;
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.SerializerOptions.WriteIndented = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        public static void ConfigureApplication(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }

        public static async Task EnsureDatabaseAsync(this WebApplicationBuilder builder)
        {
            using var provider = builder.Services.BuildServiceProvider();
            using var context = provider.GetRequiredService<DataContext>();

            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await context.Database.MigrateAsync();
            }
        }

        private static bool IsMigrationOperationExecuting()
        {
            var commandLineArguments = Environment.GetCommandLineArgs();
            if (commandLineArguments.Any(x => x.ToLower() == "migrations"))
            {
                return true;
            }

            return false;
        }
    }
}
