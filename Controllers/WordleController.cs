using WordleBackEndApi.Models;
using Microsoft.AspNetCore.Mvc;
using WordleBackEndApi.Services;

namespace WordleBackEndApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WordleController : ControllerBase
{
    private readonly WordleService _wordleService;

    public WordleController(WordleService wordleService)
    {
        _wordleService = wordleService;
    }

    /// <summary>
    /// Check the guess history by key
    /// </summary>
    [HttpGet("checkHistory")]
    public async Task<List<History>> CheckHistory(string key) =>
        await _wordleService.CheckHistory(key);

    /// <summary>
    /// Get a key
    /// </summary>
    [HttpGet("getKey")]
    public async Task<ActionResult<string>> GetKey()
    {
         return await _wordleService.GetKey();
    
    }

    /// <summary>
    /// Input the Key and guess word and return the result
    /// </summary>
    [HttpPost("guess")]
    public async Task<ActionResult<string>> Guess(Guess guess)
    {
       return  await _wordleService.Guess(guess);
        
    }

    
}
