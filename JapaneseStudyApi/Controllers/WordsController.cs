using JapaneseStudyApi.Data;
using JapaneseStudyApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JapaneseStudyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WordsController : ControllerBase
{
    private readonly JapaneseStudyContext _context;

    public WordsController(JapaneseStudyContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Word>>> GetWords()
    {
        return await _context.Words.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Word>> GetWord(int id)
    {
        var word = await _context.Words.FindAsync(id);
        if (word == null)
        {
            return NotFound();
        }
        return word;
    }

    [HttpPost]
    public async Task<ActionResult<Word>> PostWord(Word word)
    {
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

        _context.Entry(word).State = EntityState.Modified;
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
        var word = await _context.Words.FindAsync(id);
        if (word == null)
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
