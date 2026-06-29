using CompanySystem.Data.Entities.Enums;

namespace CompanySystem.Data.Entities;

public class Note : TrackingEntity
{
    public int NoteId { get; set; }

    public string UserId { get; set; }

    public NoteType NoteType { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
}
