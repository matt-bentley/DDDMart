
namespace DDDMart.SharedKernel
{
	public abstract class ValueObject<T> where T : ValueObject<T>
	{
		public override bool Equals(object obj)
		{
			var valueObject = obj as T;

			if (ReferenceEquals(valueObject, null))
				return false;

			if (GetType() != obj.GetType())
				return false;

			return ValueEquals(valueObject);
		}

		protected abstract bool ValueEquals(T other);

		public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
		{
			if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
				return true;

			if (ReferenceEquals(a, null))
			{
				return ReferenceEquals(b, null);
			}

			return a.Equals(b);
		}

		public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
		{
			return !(a == b);
		}

		public override int GetHashCode()
		{
			return GetValueHashCode();
		}

		protected abstract int GetValueHashCode();
	}
}
