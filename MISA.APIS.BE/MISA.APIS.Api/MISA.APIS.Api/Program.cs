using Dapper;
using MISA.APIS.Core.Entities;
using MISA.APIS.Core.Interfaces.Infrastructure;
using MISA.APIS.Core.Interfaces.Services;
using MISA.APIS.Core.Services;
using MISA.APIS.Infrastructure.Repository;

SqlMapper.AddTypeHandler(typeof(List<UserOption>), new JsonObjectTypeHandle()); 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJobPositionRepository, JobPositionRepository>();
builder.Services.AddScoped<IUserGroupRepository, UserGroupRepository>();
builder.Services.AddScoped<IOrganizationUnitRepository, OrganizationUnitRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserGroupService, UserGroupService>();
builder.Services.AddScoped<IJobPositionService, JobPositionService>();
builder.Services.AddScoped<IOrganizationUnitService, OrganizationUnitService>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));



//config return JSON PascalCase
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(o => o.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());

app.MapControllers();

app.Run();
