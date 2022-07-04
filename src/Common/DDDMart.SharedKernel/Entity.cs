
namespace DDDMart.SharedKernel
{
    public abstract class Entity : IEqualityComparer<Entity>, IEquatable<Entity>
    {
        protected Entity(Guid id)
        {
            Id = id;
            CreatedDate = DateTime.UtcNow;
        }

        protected Entity() : this(Guid.NewGuid())
        {

        }

        public Guid Id { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public override bool Equals(object obj) => this.Equals(obj as Entity);

        public bool Equals(Entity other)
        {
            if (other is null)
            {
                return false;
            }

            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != other.GetType())
            {
                return false;
            }

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return Id.Equals(other.Id);
        }

        public bool Equals(Entity x, Entity y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode() => (this.GetType().ToString() + Id.ToString()).GetHashCode();

        public int GetHashCode(Entity obj)
        {
            return obj.GetHashCode();
        }
    }
}
