using JapaneseStudyApi.Model;
using Microsoft.EntityFrameworkCore;

namespace JapaneseStudyApi.Data;

public class JapaneseStudyContext : DbContext
{
    public JapaneseStudyContext(DbContextOptions<JapaneseStudyContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Word> Words { get; set; }
}
