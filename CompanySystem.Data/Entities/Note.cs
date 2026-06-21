namespace CompanySystem.Data.Entities;

public class Note
{
    public int NoteId { get; set; }

    public int UserId { get; set; }

    public int CreatedBy { get; set; }

    public string NoteType { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime CreatedDate { get; set; }
}