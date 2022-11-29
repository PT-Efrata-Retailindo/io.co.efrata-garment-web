using System.Linq;

namespace System.Collections.Generic
{
    public class ListX<T> : List<T>
    {
        public ListX() : base()
        {
        }

        public ListX(IEnumerable<T> collection) : base(collection)
        {
        }

        public override bool Equals(object obj)
        {
            ListX<T> other = (ListX<T>)obj;
            IEnumerator<T> thisValues = this.GetEnumerator();
            IEnumerator<T> otherValues = other.GetEnumerator();

            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
                {
                    return false;
                }
                if (!thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }

            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode()
        {
            return this.Select(x => x.GetHashCode())
             .Aggregate((x, y) => x ^ y);
        }

        public static bool operator ==(ListX<T> left, ListX<T> right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        public static bool operator !=(ListX<T> left, ListX<T> right)
        {
            return !(left == right);
        }
    }
}