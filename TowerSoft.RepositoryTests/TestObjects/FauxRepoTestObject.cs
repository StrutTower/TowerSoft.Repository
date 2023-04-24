using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TowerSoft.Repository.Attributes;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class FauxRepoTestObject : IEquatable<FauxRepoTestObject> {
        [Autonumber]
        public long ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [ColumnMap("Status")]
        public Status StatusID { get; set; }

        public DateTime InputOn { get; set; }

        public int InputByID { get; set; }

        public bool IsActive { get; set; }

        public CountTest CountTest_Object { get; set; }

        public bool Equals(FauxRepoTestObject other) {
            return other != null &&
               ID == other.ID;
        }

        public bool AllPropsEqual(FauxRepoTestObject other) {
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
