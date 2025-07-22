using Microsoft.EntityFrameworkCore;
using NewRoomReservation.Models;
using NewRoomReservation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NewRoomReservation.Infrastructure.HostelDb>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection(); 
app.Run();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NewRoomReservation.Infrastructure.HostelDb>();

}
