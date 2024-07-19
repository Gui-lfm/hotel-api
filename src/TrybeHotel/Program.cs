using System.Text;
using TrybeHotel.Repository;
using TrybeHotel.Repository.Interfaces;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TrybeHotel.Models;
using TrybeHotel.Services;
using System.Security.Claims;
using TrybeHotel.Utils;
using TrybeHotel.Utils.interfaces;
using TrybeHotel.Context;
using TrybeHotel.Context.Interfaces;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TrybeHotelContext>();
builder.Services.AddScoped<ITrybeHotelContext, TrybeHotelContext>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IEntityUtils, EntityUtils>();
builder.Services.AddHttpClient<IGeoService, GeoService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    string file = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string path = Path.Combine(AppContext.BaseDirectory, file);
    options.IncludeXmlComments(path);

    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel Api", Description = "Aplicação de reservas de uma rede de hotéis", Version = "v1" });
});
builder.Services.AddControllersWithViews()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddHttpClient();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://nominatim.openstreetmap.org",
                                              "https://openstreetmap.org");
                      });
});


builder.Services.Configure<TokenOptions>(
    builder.Configuration.GetSection(TokenOptions.Token)
);

var tokenOptions = builder.Configuration.GetSection(TokenOptions.Token);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenOptions.GetValue<string>("Secret")))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Client", policy => policy.RequireClaim(ClaimTypes.Email));
    options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Email).RequireClaim(ClaimTypes.Role, "admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseHttpsRedirection();
app.UseRouting();

// app.UseCors(MyAllowSpecificOrigins);
app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
