using System;
using System.ComponentModel.DataAnnotations;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class CountTest : IEquatable<CountTest> {
        [Key]
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
