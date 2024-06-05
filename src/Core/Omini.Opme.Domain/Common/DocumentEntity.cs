namespace Omini.Opme.Domain.Common;

public abstract class DocumentEntity : IAuditable, ISoftDeletable, IEquatable<DocumentEntity>
{
    public Guid Id { get; init; }
    public long Number { get; private set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }

    public static bool operator ==(DocumentEntity left, DocumentEntity right)
    {
        return left is not null && right is not null && left.Equals(right);
    }

    public static bool operator !=(DocumentEntity left, DocumentEntity right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;

        if (obj.GetType() != GetType()) return false;

        if (obj is not DocumentEntity documentData) return false;

        return documentData.Id == Id;
    }

    public bool Equals(DocumentEntity? other)
    {
        if (other is null) return false;

        if (other.GetType() != GetType()) return false;

        if (other is not DocumentEntity documentData) return false;

        return documentData.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 3;
    }
}