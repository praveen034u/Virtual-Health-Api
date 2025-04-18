using Google.Cloud.Firestore;
using Grpc.Auth;
using Google.Api.Gax.Grpc;
using Google.Cloud.Firestore.V1;
using Virtual_health_api.Models;

namespace Virtual_health_api.Services;

public class FirestoreDBService<T> where T : class
{
    private readonly FirestoreDb _firestore;
    private readonly string _collection;

    public FirestoreDBService(IConfiguration config)
    {
        var projectId = config["GoogleCloud:ProjectId"];
        var credentialPath = config["GoogleCloud:CredentialsFile"];
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
        //_firestore = FirestoreDb.Create(projectId);

        _collection = typeof(T).Name;// + "s"; // auto-naming
        if (!File.Exists(credentialPath))
        {
            throw new FileNotFoundException($"❌ Google Cloud credentials file not found: {credentialPath}");
        }
        var builder = new FirestoreClientBuilder
        {
            CredentialsPath = credentialPath
        };

        var client = builder.Build();
        _firestore = FirestoreDb.Create(projectId);
    }

    public async Task AddAsync(string id, T item)
    {
        await _firestore.Collection(_collection).Document(id).SetAsync(item);
    }

    public async Task<T?> GetAsync(string id)
    {
        var doc = await _firestore.Collection(_collection).Document(id).GetSnapshotAsync();
        return doc.Exists ? doc.ConvertTo<T>() : null;
    }

    public async Task UpdateAsync(string id, T item)
    {
        await _firestore.Collection(_collection).Document(id).SetAsync(item, SetOptions.MergeAll);
    }

    public async Task DeleteAsync(string id)
    {
        await _firestore.Collection(_collection).Document(id).DeleteAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        var snapshot = await _firestore.Collection(_collection).GetSnapshotAsync();
        return snapshot.Documents.Select(d => d.ConvertTo<T>()).ToList();
    }

    public async Task<List<Vitals>> GetVitalsByUserAndDay(string userId, DateTime date)
    {
        string dateStr = date.ToString("yyyy-MM-dd");
        Query query = _firestore
            .Collection(_collection)
            .WhereEqualTo("user_id", userId)
            .WhereEqualTo("timestamp", dateStr);

        return await RunQuery(query);
    }

    public async Task<List<Vitals>> GetVitalsByUserAndDateRange(string userId, DateTime startDate, DateTime endDate)
    {
        string startStr = startDate.ToString("yyyy-MM-dd");
        string endStr = endDate.ToString("yyyy-MM-dd");

        Query query = _firestore
            .Collection(_collection)
            .WhereEqualTo("user_id", userId)
            .WhereGreaterThanOrEqualTo("timestamp", startStr)
            .WhereLessThanOrEqualTo("timestamp", endStr);

        return await RunQuery(query);
    }

    private async Task<List<Vitals>> RunQuery(Query query)
    {
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        return snapshot.Documents.Select(doc => doc.ConvertTo<Vitals>()).ToList();
    }
}
