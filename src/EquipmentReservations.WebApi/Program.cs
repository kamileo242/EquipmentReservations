using EquipmentReservations.DataLayer;
using EquipmentReservations.DataLayer.Repositories;
using EquipmentReservations.Domain;
using EquipmentReservations.Domain.Services;
using EquipmentReservations.Models;
using EquipmentReservations.WebApi.Converters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TimeProvider = EquipmentReservations.Domain.Services.TimeProvider;

namespace EquipmentReservations.WebApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      var configuration = builder.Configuration;

      configuration.AddJsonFile("appsettings.Docker.json", optional: true);

      ConfigureServices(builder.Services, configuration, builder.Environment);

      var app = builder.Build();

      using (var scope = app.Services.CreateScope())
      {
        var dbContext = scope.ServiceProvider.GetRequiredService<EquipmentReservationsDbContext>();
        dbContext.Database.Migrate();
      }

      ConfigureMiddleware(app, builder.Environment);

      app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
      services.AddDbContext<EquipmentReservationsDbContext>(options =>
          options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

      services.AddMemoryCache();

      services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
      {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
      })
      .AddEntityFrameworkStores<EquipmentReservationsDbContext>()
      .AddDefaultTokenProviders();

      var jwtSettings = configuration.GetSection("JwtSettings");
      var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = jwtSettings["Issuer"],
          ValidAudience = jwtSettings["Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(key)
        };
      });

      var publisherUrl = configuration["ReservationPublisher:Url"]
                   ?? throw new InvalidOperationException("ReservationPublisher:Url nie jest ustawione!");

      services.AddHttpClient<IReservationPublisher, ReservationPublisher>(client =>
      {
        client.BaseAddress = new Uri(publisherUrl);
      });

      services.AddAuthorization();
      services.AddHttpContextAccessor();

      services.AddScoped<IUserContext, UserContext>();
      services.AddScoped<AuditSaveChangeInterceptor>();
      services.AddScoped<IDboConverter, DboConverter>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      services.AddScoped<ICategoryRepository, CategoryRepository>();
      services.AddScoped<IAuditLogRepository, AuditLogRepository>();
      services.AddScoped<IEquipmentRepository, EquipmentRepository>();
      services.AddScoped<IReservationRepository, ReservationRepository>();

      services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
      services.AddScoped<ICategoryService, CategoryService>();
      services.AddScoped<IEquipmentService, EquipmentService>();
      services.AddScoped<IReservationService, ReservationService>();
      services.AddScoped<ITimeProvider, TimeProvider>();
      services.AddScoped<IDtoConverter, DtoConverter>();
      services.AddScoped<ICacheService, MemoryCacheService>();

      services.Configure<RequestLocalizationOptions>(options =>
      {
        options.DefaultRequestCulture = new RequestCulture("pl-PL");
      });

      services.AddControllers().AddJsonOptions(options =>
      {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
      });

      services.AddEndpointsApiExplorer();

      services.AddCors(options =>
      {
        options.AddPolicy("CustomPolicy", builder =>
              {
                builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
              });
      });

      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc("1.0", new OpenApiInfo
        {
          Title = "EquipmentReservations",
          Description = "Aplikacja do rezerwacji sprzętu firmowego.",
          Version = "1.0"
        });

        if (env.IsDevelopment() || Debugger.IsAttached)
        {
          options.ExampleFilters();
        }

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        options.EnableAnnotations();
        options.DescribeAllParametersInCamelCase();
        options.SupportNonNullableReferenceTypes();
        options.UseAllOfToExtendReferenceSchemas();
      });

      services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
    }

    private static void ConfigureMiddleware(WebApplication app, IWebHostEnvironment env)
    {
      app.UseRouting();
      app.UseCors("CustomPolicy");
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseRequestLocalization();
      app.UseHttpsRedirection();

      if (env.IsDevelopment() || Debugger.IsAttached)
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
          options.SwaggerEndpoint("/swagger/1.0/swagger.json", "EquipmentReservations 1.0");
          options.EnableFilter();
          options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
          options.DisplayOperationId();
        });
      }
      else
      {
        app.UseExceptionHandler("/error");
      }

      app.MapControllers();
    }
  }
}
