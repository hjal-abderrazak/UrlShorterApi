using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Url_Shorter;
using Url_Shorter.Entities;
using Url_Shorter.Extension;
using Url_Shorter.Models;
using Url_Shorter.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<UrlShorteningService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}


app.MapPost("api/shorter", async (
    ShortenUrlRequest request,
    UrlShorteningService urlShorteningService,
    ApplicationDbContext context,
    HttpContext httpContext
    ) =>
{
    if(!Uri.TryCreate(request.Url,UriKind.Absolute,out _))
    {
        return Results.BadRequest("the url is invalid !");
    }
    var code = await urlShorteningService.GenerateUniqueCode();
    var shortenUrl = new ShortenUrl()
    {
        Id = new Guid(),
        LongUrl = request.Url,
        Code = code,
        ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
        CreatedOnUtc=DateTime.UtcNow
    };
    context.ShortenUrls.Add(shortenUrl);
    await context.SaveChangesAsync();


    return Results.Ok(shortenUrl.ShortUrl);
});


app.MapGet("api/{code}",async( string code,
    ApplicationDbContext context)=>
{
    var  shortenedUrl = await context.ShortenUrls.FirstOrDefaultAsync(s => s.Code == code);
    if(shortenedUrl is null)
    {
        return Results.NotFound("URL Not Found");
    }
    var uri =new Uri(shortenedUrl.LongUrl);
    //var decodedUrl = System.Net.WebUtility.UrlDecode(shortenedUrl.LongUrl);
    return Results.Redirect(uri.AbsoluteUri);

});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
