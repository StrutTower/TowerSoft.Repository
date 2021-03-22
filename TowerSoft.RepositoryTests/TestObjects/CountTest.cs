using System;
using System.ComponentModel.DataAnnotations;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class CountTest : IEquatable<CountTest> {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public bool Equals(CountTest other) {
            return other != null &&
               ID == other.ID;
        }

        public bool AllPropsEqual(CountTest other) {
            return other != null &&
               ID == other.ID &&
               Name == other.Name;
        }
    }
}
