using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Google.Api;

namespace Virtual_health_api.Services;

public class DynamoDBService<T> where T: class
{
    private readonly IDynamoDBContext _context;

    public DynamoDBService(IConfiguration config)
    {
        var aws = config.GetSection("AWS");
        var client = new AmazonDynamoDBClient(
            aws["AccessKey"],
            aws["SecretKey"],
            Amazon.RegionEndpoint.GetBySystemName(aws["Region"])
        );

        _context = new DynamoDBContext(client);
    }

    public async Task AddAsync(T item) => await _context.SaveAsync(item);
    public async Task<T?> GetAsync(object hashKey) => await _context.LoadAsync<T>(hashKey);
    public async Task UpdateAsync(T item) => await _context.SaveAsync(item);
    public async Task DeleteAsync(object hashKey) => await _context.DeleteAsync<T>(hashKey);
}
