#pragma warning disable CS8765, CS8604 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
namespace Omini.Opme.Be.Domain;

public abstract class BaseValueObject
{
    protected static bool EqualOperator(BaseValueObject left, BaseValueObject right)
    {
        if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
        {
            return false;
        }
        return ReferenceEquals(left, right) || left.Equals(right);
    }

    protected static bool NotEqualOperator(BaseValueObject left, BaseValueObject right)
    {
        return !(EqualOperator(left, right));
    }

    protected abstract IEnumerable<object> GetEqualityComponents();


    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (BaseValueObject)obj;

        return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    public static bool operator ==(BaseValueObject one, BaseValueObject two)
    {
        return EqualOperator(one, two);
    }

    public static bool operator !=(BaseValueObject one, BaseValueObject two)
    {
        return NotEqualOperator(one, two);
    }
}
#pragma warning restore CS8765, CS8604 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).