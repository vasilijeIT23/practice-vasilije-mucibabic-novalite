using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using MovieStoreApi.Repositories;
using MovieStoreCore;
using MovieStoreInfrastructure;
using System.Text.Json.Serialization;
using ToDoApi.Infrastructure;

var AllowCors = "_allowCors";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(builder.Configuration);

builder.Services.AddDbContext<MovieStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieStoreDatabase")));

//builder.Services.BuildServiceProvider().GetService<MovieStoreContext>().Database.Migrate();

builder.Services.AddScoped<IRepository<Movie>, MovieRepository>();
builder.Services.AddScoped<IRepository<PurchasedMovie>, PurchasedMovieRepository>();
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers().AddJsonOptions(options =>
{
    //TODO remove this
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowCors,
    builder =>
    {
        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
    });
});

builder.Services.AddOpenApiDocument(config =>
{
    config.SchemaNameGenerator = new CustomSwaggerSchemaNameGenerator();
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(AllowCors);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseOpenApi();
app.UseSwaggerUi3();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<MovieStoreContext>();
    context.Database.Migrate();
}

app.Run();
