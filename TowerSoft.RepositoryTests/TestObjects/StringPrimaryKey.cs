using TowerSoft.Repository.Attributes;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class StringPrimaryKey : IEquatable<StringPrimaryKey> {
        [KeyMap, ColumnMap("IEN")]
        public string PrimaryKey { get; set; }

        public string Name { get; set; }

        public bool Equals(StringPrimaryKey? other) =>
            other != null && PrimaryKey == other.PrimaryKey && Name == other.Name;

        public override int GetHashCode() =>
            PrimaryKey.GetHashCode() ^ Name.GetHashCode();
    }
}
