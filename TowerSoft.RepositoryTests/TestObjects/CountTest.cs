using TowerSoft.Repository.Attributes;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class CountTest : IEquatable<CountTest> {
        [KeyMap]
        public int Number { get; set; }

        public string Name { get; set; }

        public bool Equals(CountTest other) {
            return other != null &&
               Number == other.Number;
        }

        public bool AllPropsEqual(CountTest other) {
            return other != null &&
               Number == other.Number &&
               Name == other.Name;
        }
    }
}
