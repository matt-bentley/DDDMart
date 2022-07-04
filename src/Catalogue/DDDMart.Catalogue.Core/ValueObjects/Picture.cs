using DDDMart.SharedKernel;
using DDDMart.SharedKernel.Guards;

namespace DDDMart.Catalogue.Core.ValueObjects
{
    public class Picture : ValueObject<Picture>
    {
        private Picture(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }

        public static Picture Create(string name, string uri)
        {
            Guard.Against.NullOrEmpty(name, "Name");
            Guard.Against.InvalidUrl(uri, "Uri");
            return new Picture(name, uri);
        }

        public string Name { get; private set; }
        public string Uri { get; private set; }

        protected override int GetValueHashCode()
        {
            return HashCode.Combine(Name, Uri);
        }

        protected override bool ValueEquals(Picture other)
        {
            return Name.Equals(other.Name) && Uri.Equals(other.Uri);
        }
    }
}
