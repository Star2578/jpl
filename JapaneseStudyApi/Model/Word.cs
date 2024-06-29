namespace JapaneseStudyApi.Model;

public class Word
{
    public int Id { get; set; }
    public required string JpWord { get; set; }
    public required string Pronunciation { get; set; }
    public required string EnMeaning { get; set; }
    public required string ThMeaning { get; set; }
    public required int UserId { get; set; }
    // public User User { get; set; }
}
