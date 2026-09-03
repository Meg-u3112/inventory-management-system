using Microsoft.EntityFrameworkCore;
using Day21_Inventory_System.Data;

var builder = WebApplication.CreateBuilder(args);


// =====================================================
// 1. Register Database Context
// =====================================================

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


// =====================================================
// 2. Add Controllers
// =====================================================

builder.Services.AddControllers();


// =====================================================
// 3. Add Swagger
// =====================================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173",
            "http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
// =====================================================
// 4. Build Application
// =====================================================

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    context.Database.Migrate();

    DbSeeder.Seed(context);
}

// =====================================================
// 5. Swagger
// =====================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// CORS
app.UseCors("ReactPolicy");

// =====================================================
// 6. Authorization
// =====================================================

app.UseAuthorization();


// =====================================================
// 7. Map Controllers
// =====================================================

app.MapControllers();


// =====================================================
// 8. Start Application
// =====================================================

app.Run();