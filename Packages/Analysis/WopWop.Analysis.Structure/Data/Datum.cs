namespace WopWop.Analysis.Structure.Data;

public class Datum
{
    private Datum()
    {
        
    }
    
    public string Id { get; private set; }
    public DatumKind Kind { get; private set; }

    #region "Equality"
    
    protected bool Equals(Datum other)
    {
        return Id == other.Id && Kind == other.Kind;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Datum)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Id != null ? Id.GetHashCode() : 0) * 397) ^ (int)Kind;
        }
    }
    

    public static bool operator ==(Datum left, Datum right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Datum left, Datum right)
    {
        return !Equals(left, right);
    }

    #endregion

    public override string ToString() => Kind switch
    {
        DatumKind.Parameter => $"param {Id}",
        DatumKind.LocalVariable => $"var {Id}",
        DatumKind.MemberField => $"field {Id}",
        DatumKind.MemberProperty => $"prop {Id}",
    };

    public static Datum New(string id, DatumKind kind) => new Datum()
    {
        Id = id,
        Kind = kind
    };
}