namespace Omini.Opme.Domain.Entities;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; set; }

    public static bool operator ==(Entity left, Entity right)
    {
        return left is not null && right is not null && left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;

        if (obj.GetType() != GetType()) return false;

        if (obj is not Entity entity) return false;

        return entity.Id == Id;
    }

    public bool Equals(Entity? other)
    {
        if (other is null) return false;

        if (other.GetType() != GetType()) return false;

        if (other is not Entity entity) return false;

        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 11;
    }
}