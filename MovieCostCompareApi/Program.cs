using MovieCostCompareApi.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(
//        policy =>
//        {
//            policy.WithOrigins("http://localhost")
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//        });
//});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
// custom services
builder.Services.AddTransient<IExternalApiLoadService, JsonExternalApiLoadService>();
builder.Services.AddTransient<IMovieDataService, MovieDataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));
app.UseAuthorization();

app.MapControllers();

app.Run();
