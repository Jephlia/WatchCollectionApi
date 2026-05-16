using Microsoft.EntityFrameworkCore;
using WatchCollectionApi;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("WatchesDb"));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Create DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// GET all
app.MapGet("/watches", async (AppDbContext db) =>
{
    return await db.Watches.ToListAsync();
});


// GET by id
app.MapGet("/watches/{id}", async (int id, AppDbContext db) =>
{
    var watch = await db.Watches.FindAsync(id);

    return watch is not null
        ? Results.Ok(watch)
        : Results.NotFound();
});


// POST
app.MapPost("/watches", async (Watch watch, AppDbContext db) =>
{
    db.Watches.Add(watch);

    await db.SaveChangesAsync();

    return Results.Created($"/watches/{watch.Id}", watch);
});


// PUT
app.MapPut("/watches/{id}", async (int id, Watch inputWatch, AppDbContext db) =>
{
    var watch = await db.Watches.FindAsync(id);

    if (watch is null)
        return Results.NotFound();

    watch.Brand = inputWatch.Brand;
    watch.Model = inputWatch.Model;
    watch.Year = inputWatch.Year;

    await db.SaveChangesAsync();

    return Results.NoContent();
});


// DELETE
app.MapDelete("/watches/{id}", async (int id, AppDbContext db) =>
{
    var watch = await db.Watches.FindAsync(id);

    if (watch is null)
        return Results.NotFound();

    db.Watches.Remove(watch);

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();