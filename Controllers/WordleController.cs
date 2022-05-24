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
    /// View all the inventories
    /// </summary>
    [HttpGet("checkHistory")]
    public async Task<List<History>> Get() =>
        await _wordleService.GetAsync();

    /// <summary>
    /// View a specific inventory by ID 
    /// </summary>
    [HttpGet("getKey")]
    public async Task<ActionResult<string>> GetKey()
    {
         return await _wordleService.GetKey();
    
    }

    /// <summary>
    /// Create a new inventory, donot input the product_id value, just delete the first line when you try this API
    /// </summary>
    [HttpPost("guess")]
    public async Task<ActionResult<string>> Post(string guess)
    {
       string answer =  await _wordleService.Guess(guess);

        return answer;
    }

    
}
