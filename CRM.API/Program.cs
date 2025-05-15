using CRM.Domain.Entities;
using CRM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.WriteTo.Seq("http://localhost:5341") // آدرس Seq
	.CreateLogger();

builder.Host.UseSerilog();


// PostgreSQL Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = "Bearer";
	options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
	options.Authority = "http://localhost:5000"; // IdentityServer URL
	options.RequireHttpsMetadata = false;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateAudience = false
	};
});

builder.Services.AddAuthorizationBuilder()
	.SetDefaultPolicy(new AuthorizationPolicyBuilder("Bearer")
		.RequireAuthenticatedUser()
		.Build())
	.AddPolicy("ApiScope", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireClaim("scope", "crm_api");
	});


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CRM API", Version = "v1" });

	// JWT Auth definition
	c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Description = "توکن JWT رو اینجا وارد کن: \r\n\r\nBearer {token}"
	});

	// JWT Auth requirement
	c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
	{
		{
			new Microsoft.OpenApi.Models.OpenApiSecurityScheme
			{
				Reference = new Microsoft.OpenApi.Models.OpenApiReference
				{
					Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});


// Razor Pages
builder.Services.AddRazorPages();


// Controllers
builder.Services.AddControllers();


builder.Services.AddOpenApi();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.Run();

