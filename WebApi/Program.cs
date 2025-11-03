using Refit;
var builder = WebApplication.CreateBuilder(args);
// Register Refit Http Client and include possible http client options like base address, default request headers, authorization jwt token etc.
builder.Services.AddRefitClient<IDogService>()
    .ConfigureHttpClient((p, client) =>
    {
        client.BaseAddress = new Uri("https://dog.ceo/api");
        client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
    });

var app = builder.Build();

app.MapGet("/", async (HttpContext context, IDogService _dogService)  => {
    //var data = (await _dogService.GetBreedImages("retriever-golden")).Message;
    var data = (await _dogService.GetBreedImages("retriever")).Message;

    string output = "<!DOCTYPE html> <html> <body>";
    foreach (string url in data)
    {
        output += url+"<br> <img src = \""+url+"\">";
        output += "<br><br>";
    }
    output += "<hr> </body> </html>";
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(output);
});

app.Run();

public interface IDogService
{
    // Dog Service GET Breed image API
    [Get("/breed/{breedName}/images/random/3")]
    Task<BreedImageModel> GetBreedImages(string breedName);
}

public record BreedImageModel(string[]? Message, string? Status);
