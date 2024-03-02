using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Security.Claims;
using System.Text;
using UserMgtSysApi.Data;
using UserMgtSysApi.Data.Contexts;
using UserMgtSysApi.Services;
using UserMgtSysApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

	// Add JWT Authentication
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme.",
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]??"")),
		RoleClaimType = ClaimTypes.Role		
	};
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Configure roles
ConfigureRoles(app);

app.MapControllers();

app.Run();

static void ConfigureRoles(WebApplication app)
{
	using var serviceScope = app.Services.CreateScope();
	var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

	// Define your roles here
	var roles = new[] { "Admin", "Initial User" };

	foreach (var role in roles)
	{
		if (!roleManager.RoleExistsAsync(role).Result)
		{
			var newRole = new IdentityRole { Name = role };
			_ = roleManager.CreateAsync(newRole).Result;
		}
	}

	//// we can also do it as below:
	//if (!roleManager.RoleExistsAsync("Admin").Result)
	//{
	//	var role = new IdentityRole { Name = "Admin" };
	//	_ = roleManager.CreateAsync(role).Result;
	//}
}
//void ConfigureRolesAndInitialUser(WebApplication app)
//{
//	using var serviceScope = app.Services.CreateScope();
//	var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//	var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

//	// Define your roles here
//	var roles = new[] { "Admin", "User" };

//	foreach (var role in roles)
//	{
//		if (!roleManager.RoleExistsAsync(role).Result)
//		{
//			var newRole = new IdentityRole { Name = role };
//			_ = roleManager.CreateAsync(newRole).Result;
//		}
//	}

//	// Check if the initial admin user exists
//	var adminUser = userManager.FindByEmailAsync("admin@example.com").Result;

//	if (adminUser == null)
//	{
//		// Create the initial admin user
//		var admin = new ApplicationUser
//		{
//			UserName = "admin",
//			Email = "admin@example.com",
//			// Add additional user properties here if needed
//		};

//		var result = userManager.CreateAsync(admin, "AdminPassword_123").Result;

//		if (result.Succeeded)
//		{
//			// Assign the "Admin" role to the admin user
//			_ = userManager.AddToRoleAsync(admin, "Admin").Result;
//		}
//		else
//		{
//			// Handle errors, log, or throw an exception if user creation fails
//		}
//	}
//}