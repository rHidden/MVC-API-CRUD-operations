using DavidExercise.Repositories;
using DavidExercise.Repositories.RepositoryInterfaces;
using DavidExercise.Services;
using DavidExercise.Services.ServiceInterfaces;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(host => true)
            .AllowCredentials();
    });
});

// Dependency Injection
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ILeaderService, LeaderService>();
builder.Services.AddTransient<IMemberRepository, MemberRepository>();
builder.Services.AddTransient<ILeaderRepository, LeaderRepository>();
builder.Services.AddScoped<IDatabaseConnectionFactory, DatabaseConnectionFactory>(_ => new DatabaseConnectionFactory(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
