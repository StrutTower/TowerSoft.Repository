using TowerSoft.Repository.Attributes;

namespace TestingLib.Domain {
    public class Person {
        [Autonumber]
        public string ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int TitleID { get; set; }

    }
}
