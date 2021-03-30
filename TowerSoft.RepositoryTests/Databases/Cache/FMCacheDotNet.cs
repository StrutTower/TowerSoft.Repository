using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TowerSoft.Repository.Attributes;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.Databases.Cache {
    [Table("FM.CacheDotNet")]
    public class FMCacheDotNet {
        [Autonumber]
        public int IEN { get; set; }

        public int Name { get; set; }

        public string Title { get; set; }

        public Status? StatusID { get; set; }

        [CacheFilemanDate]
        public DateTime InputOn { get; set; }

        public int InputByID { get; set; }

        public bool IsActive { get; set; }


        public bool AllPropsEqual(FMCacheDotNet other) {
            return IEN == other.IEN &&
                Title == other.Title &&
                StatusID == other.StatusID &&
                InputOn.ToString("yyyy/MM/dd HH:mm:ss") == other.InputOn.ToString("yyyy/MM/dd HH:mm:ss") &&
                InputByID == other.InputByID &&
                IsActive == other.IsActive;
        }
    }
}
