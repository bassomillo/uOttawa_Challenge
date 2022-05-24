using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WordleBackEndApi.Models;

namespace WordleBackEndApi.Services;
public class WordleService
{
    private readonly IMongoCollection<Word> _wordCollection;

    private readonly IMongoCollection<History> _historyCollection;

    public WordleService(
        IOptions<WordleDataBaseSettings> wordleDataBaseSettings)
    {
        var mongoClient = new MongoClient(
            wordleDataBaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            wordleDataBaseSettings.Value.DatabaseName);

        _wordCollection = mongoDatabase.GetCollection<Word>(
            wordleDataBaseSettings.Value.WordCollectionName);
        _historyCollection = mongoDatabase.GetCollection<History>(
            wordleDataBaseSettings.Value.HistoryCollectionName);
    }

    public async Task<ActionResult<string>> GetKey()
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string word = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
        List<Word> findWord = await _wordCollection.Find(x => x.word == word).ToListAsync();
        if (findWord.Any())
        {
            return findWord[0].key;
        }
        else
        {
            var md5Hash = MD5.Create();
            var sourceBytes = Encoding.UTF8.GetBytes(word);
            var hashBytes = md5Hash.ComputeHash(sourceBytes);
            string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            Word newWord = new Word();
            newWord.key = hash;
            newWord.word = word;
            await _wordCollection.InsertOneAsync(newWord);
            return hash;
        }
    }



    public async Task<List<Inventory>> GetAsync() =>
        await _inventoryCollection.Find(_ => true).ToListAsync();

    public async Task<Inventory?> GetAsync(string id) =>
        await _inventoryCollection.Find(x => x.product_id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Inventory newInventory) =>
        await _inventoryCollection.InsertOneAsync(newInventory);

    public async Task UpdateAsync(string id, Inventory updatedInventory) =>
        await _inventoryCollection.ReplaceOneAsync(x => x.product_id == id, updatedInventory);

    public async Task RemoveAsync(string id) =>
        await _inventoryCollection.DeleteOneAsync(x => x.product_id == id);
}