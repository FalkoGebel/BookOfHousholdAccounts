namespace BohaLibrary
{
    public class Category
    {
        public BookEntryType EntryType { get; set; }
        public string Name { get; set; } = string.Empty;

        public override bool Equals(object? obj) => Equals(obj as Category);

        public bool Equals(Category? other)
            => other != null && EntryType == other.EntryType && Name == other.Name;

        public override int GetHashCode() => HashCode.Combine(EntryType, Name);
    }
}