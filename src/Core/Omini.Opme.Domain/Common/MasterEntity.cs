namespace Omini.Opme.Domain.Common;

public abstract class MasterEntity : IAuditable, IEquatable<MasterEntity>
{
    public string Code { get; protected set; }
    public string Name { get; protected set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public virtual string GetSearchableString()
    {
        return string.Empty;
    }

    public static bool operator ==(MasterEntity left, MasterEntity right)
    {
        return left is not null && right is not null && left.Equals(right);
    }

    public static bool operator !=(MasterEntity left, MasterEntity right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;

        if (obj.GetType() != GetType()) return false;

        if (obj is not MasterEntity entity) return false;

        return entity.Code == Code;
    }

    public bool Equals(MasterEntity? other)
    {
        if (other is null) return false;

        if (other.GetType() != GetType()) return false;

        if (other is not MasterEntity entity) return false;

        return entity.Code == Code;
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode() * 7;
    }
}