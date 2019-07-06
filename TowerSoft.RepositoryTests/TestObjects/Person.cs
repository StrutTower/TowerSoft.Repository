using System.ComponentModel.DataAnnotations.Schema;
using TowerSoft.Repository;
using TowerSoft.Repository.Attributes;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class Person {
        [Autonumber]
        public int ID { get; set; }

        public string FirstName { get; set; }

        [Column("Last_Name")]
        public string LastName { get; set; }

        [NotMapped]
        public string NotMappedProperty { get; set; }
    }
}
