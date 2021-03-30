using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TowerSoft.Repository.Attributes;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class DateTest {
        [Autonumber]
        public int ID { get; set; }

        public string Title { get; set; }

        [Column("Fileman_DateTime"), CacheFilemanDate]
        public DateTime? FilemanDateTime { get; set; }

        [Column("Fileman_Date"), CacheFilemanDate]
        public DateTime? FilemanDate { get; set; }

        [Column("Sql_DateTime")]
        public DateTime? SqlDateTime { get; set; }

        [Column("Sql_Date"), CacheHorologDate]
        public DateTime? SqlDate { get; set; }
    }
}
