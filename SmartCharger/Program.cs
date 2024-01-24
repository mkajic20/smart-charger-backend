using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartCharger.Business.Interfaces;
using SmartCharger.Business.Services;
using SmartCharger.Data;
using System.Text;


string GetEnvironment()
{
    var isRender = Environment.GetEnvironmentVariable("RENDER") == "true";
    return isRender ? "production" : "development";
}

var environment = GetEnvironment();
var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(environment == "production" ? "appsettings.json" : "appsettings.Development.json", optional: false);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token. Enter 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("roleId", "1"));
    options.AddPolicy("Customer", policy => policy.RequireClaim("roleId", "2"));
    options.AddPolicy("AdminOrCustomer", policy =>
       policy.RequireAssertion(context =>
           context.User.HasClaim(c =>
               (c.Type == "roleId" && (c.Value == "1" || c.Value == "2"))
           )
       )
   );
});


builder.Services.AddDbContext<SmartChargerContext>(options =>
{
    if (environment == "production")
    {
        options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
    }
    else
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("Connection"));

    }

});
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IChargerService, ChargerService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<IGoogleLoginService, GoogleLoginService>();



var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<SmartChargerContext>();
    await context.Database.MigrateAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(c => c.AllowAnyHeader().WithMethods("GET", "PUT", "POST", "PATCH", "DELETE", "HEAD", "OPTIONS").AllowAnyOrigin().WithExposedHeaders("Content-Disposition"));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
