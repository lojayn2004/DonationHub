namespace DonationHub.Domain.Models.Shared
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        public virtual bool Equals(ValueObject? other) => other is not null && ValuesAreEqual(other);


        public static bool operator !=(ValueObject? a, ValueObject? b) => !(a == b);


        public override int GetHashCode() => GetAtomicValues().Aggregate(
            0, (hashcode, value) => HashCode.Combine(hashcode, value.GetHashCode())
        );

        public static bool operator ==(ValueObject? a, ValueObject? b)
        {
            if (a is null && b is null)
                return true;
            if (a is null || b is null)
                return false;
            return a.Equals(b);

        }
        
        

        protected abstract IEnumerable<object> GetAtomicValues();

        private bool ValuesAreEqual(ValueObject valueObject) => GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());

        public override bool Equals(object obj)
        {
            return Equals(obj as ValueObject);
        }
    }


}
