using CommBank.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CommBank.Services;

public class GoalsService : IGoalsService
{
    private readonly IMongoCollection<Goal> _goalsCollection;

    public GoalsService(IMongoDatabase mongoDatabase)
    {
        _goalsCollection = mongoDatabase.GetCollection<Goal>("Goals");
        var documents = _goalsCollection.Find(FilterDefinition<Goal>.Empty).ToList();
        foreach (var doc in documents)
        {
            Console.WriteLine(doc.ToJson());
        }
    }

    public async Task<List<Goal>> GetAsync() =>
        await _goalsCollection.Find(_ => true).ToListAsync();

    public async Task<List<Goal>?> GetForUserAsync(string id) =>
        await _goalsCollection.Find(x => x.UserId == id).ToListAsync();

    public async Task<Goal?> GetAsync(string id) =>
        await _goalsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Goal newGoal) =>
        await _goalsCollection.InsertOneAsync(newGoal);

    public async Task UpdateAsync(string id, Goal updatedGoal) =>
        await _goalsCollection.ReplaceOneAsync(x => x.Id == id, updatedGoal);

    public async Task RemoveAsync(string id) =>
        await _goalsCollection.DeleteOneAsync(x => x.Id == id);
}