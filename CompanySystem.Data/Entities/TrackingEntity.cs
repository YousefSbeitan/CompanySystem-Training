namespace CompanySystem.Data.Entities;

public abstract class TrackingEntity
{
    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; } = false;
}