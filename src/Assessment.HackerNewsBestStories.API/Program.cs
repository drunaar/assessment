var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var memoryCache = new MemoryCache(new MemoryCacheOptions());
var cacheProvider = new MemoryCacheProvider(memoryCache);

var policies = services.AddPolicyRegistry();
policies.Add("StandardBulkheadPolicy", Policy.BulkheadAsync<HttpResponseMessage>(10, 3000));
policies.Add(GetBestStoriesHandler.HackerNewsCachePolicyKey,
    Policy.CacheAsync(cacheProvider.AsyncFor<HackerNewsItem>(), TimeSpan.FromMinutes(3)));

services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions
            .Converters.Add(new CustomDateTimeConverter("yyyy-MM-ddTHH:mm:ssK"));
    });
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options => {
    options.SupportNonNullableReferenceTypes();
});
services.AddMediatR(config => {
    config.RegisterServicesFromAssemblyContaining<Program>();
});
services.AddRefitClient<IHackerNewsAPI>()
    .ConfigureHttpClient(client => {
        client.BaseAddress = new Uri("https://hacker-news.firebaseio.com");
    })
    .AddPolicyHandlerFromRegistry("StandardBulkheadPolicy");

TypeAdapterConfig.GlobalSettings.Apply(new MappingsProfile());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();

public partial class Program { }
