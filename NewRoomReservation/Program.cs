using Microsoft.EntityFrameworkCore;
using NewRoomReservation.Models;

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
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();



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

    if (await db.Users.FindAsync(1) == null)
    {
        await db.Users.AddAsync(new User
        {
           Id = 1,
           UserName = "Алеша",
           Balance = 500,
           Password = "123456",
           IsAdmin = false,
           IsBanned = false,
        });
        await db.SaveChangesAsync();
    }
}