using TowerSoft.Repository.Attributes;

namespace TowerSoft.RepositoryTests.TestObjects {
    internal class AdapterTestObject : IEquatable<AdapterTestObject> {
        [Autonumber]
        public long ID { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Description { get; set; }

        public Status StatusID { get; set; }

        public DateTime InputOn { get; set; }

        public int InputByID { get; set; }

        public bool IsActive { get; set; }

        public bool Equals(AdapterTestObject other) {
            return other != null &&
               ID == other.ID;
        }

        public bool AllPropsEqual(AdapterTestObject other) {
            return other != null &&
               ID == other.ID &&
               Title == other.Title &&
               Description == other.Description &&
               StatusID == other.StatusID &&
               InputOn.ToString("MM/dd/yyyy HH:mm") == other.InputOn.ToString("MM/dd/yyyy HH:mm") &&
               InputByID == other.InputByID &&
               IsActive == other.IsActive;
        }
    }
}