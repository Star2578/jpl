using System.Security.Claims;
using JapaneseStudyApi.Data;
using JapaneseStudyApi.Global;
using JapaneseStudyApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JapaneseStudyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WordsController : ControllerBase
{
    private readonly JapaneseStudyContext _context;

    public WordsController(JapaneseStudyContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetWords()
    {
        if (!AuthorizationHelper.IsAdmin(User))
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var words = await _context.Words.Where(w => w.UserId.ToString() == userId).ToListAsync();
            return Ok(words);
        }

        var all = await _context.Words.ToListAsync();
        return Ok(all);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Word>> GetWord(int id)
    {
        var word = await _context.Words.FirstOrDefaultAsync(w => w.Id == id);

        if (word == null || !AuthorizationHelper.IsAuthorized(User, word.UserId))
        {
            return NotFound();
        }

        return word;
    }

    [HttpPost]
    public async Task<ActionResult<Word>> PostWord(Word word)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        word.UserId = int.Parse(userId);
        
        _context.Words.Add(word);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetWord), new { id = word.Id }, word);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutWord(int id, Word word)
    {
        if (id != word.Id)
        {
            return BadRequest();
        }

        var existingWord = await _context.Words.FirstOrDefaultAsync(w => w.Id == id);

        if (existingWord == null || !AuthorizationHelper.IsAuthorized(User, existingWord.UserId))
        {
            return NotFound();
        }

        existingWord.JpWord = word.JpWord;
        existingWord.Pronunciation = word.Pronunciation;
        existingWord.EnMeaning = word.EnMeaning;
        existingWord.ThMeaning = word.ThMeaning;

        _context.Entry(existingWord).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!WordExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWord(int id)
    {
        var word = await _context.Words.FirstOrDefaultAsync(w => w.Id == id);

        if (word == null || !AuthorizationHelper.IsAuthorized(User, word.UserId))
        {
            return NotFound();
        }

        _context.Words.Remove(word);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    private bool WordExists(int id)
    {
        return _context.Words.Any(e => e.Id == id);
    }
}
