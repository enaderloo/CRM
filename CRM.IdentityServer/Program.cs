using CRM.Domain.Entities;
using CRM.IdentityServer.Config;
using CRM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddIdentityServer(options =>
//{
//	options.EmitStaticAudienceClaim = true;
//})
//.AddInMemoryIdentityResources(IdentityConfig.GetApiResources())
//.AddInMemoryApiScopes(IdentityConfig.GetApiScopes())
//.AddInMemoryClients(IdentityConfig.GetClients())
//.AddAspNetIdentity<ApplicationUser>();


builder.Services.AddIdentityServer()
	.AddInMemoryApiScopes(IdentityConfig.GetApiScopes())
	.AddInMemoryApiResources(IdentityConfig.GetApiResources())
	.AddInMemoryClients(IdentityConfig.GetClients())
	.AddDeveloperSigningCredential();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();


builder.Services.AddOpenApi();

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

app.UseIdentityServer();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


