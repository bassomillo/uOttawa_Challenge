using System;
using System.Collections;
using System.Linq;
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
        const string chars = "abcdefghijklmnopqrstuvwxyz";
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


    public async Task<string> Guess(Guess guessWord){
        string key = guessWord.key;
        string guess = guessWord.guess;
        History history =  new History(); 
        Word word = await _wordCollection.Find(x => x.key == key).FirstOrDefaultAsync();
        string answer = word.word;
         history.key = key;
        history.date = DateTime.Now.ToString();
        history.word = answer;
        history.guess = guess;
        await _historyCollection.InsertOneAsync(history);
        char[] guessList = guess.ToCharArray();
        char[] answerList = answer.ToCharArray();
        Hashtable answerTable = new Hashtable();
        for(int i = 0; i<answerList.Length; i++){
            answerTable.Add(answerList[i], i);
        }
        List<string> result = new List<string>();
        for(int i = 0; i<answerList.Length; i++){
            if(answerList[i] == guessList[i]){
                result.Add("y");
            }else if(answerTable.ContainsKey(guessList[i])){
                result.Add("m");
            }else{
                result.Add("n");
            }
        }
        return string.Join(",", result);

}
       public async Task<List<History>> CheckHistory(string key) =>
        await _historyCollection.Find(x => x.key == key).ToListAsync();

}