using TowerSoft.Repository.Attributes;

namespace TowerSoft.RepositoryTests.TestObjects {
    [TableName("datetest")]
    public class IrisDateTest {
        [Autonumber]
        public int ID { get; set; }

        public string Title { get; set; }

        [ColumnMap("Fileman_DateTime")]
        public DateTime? FilemanDateTime { get; set; }

        [ColumnMap("Fileman_Date"), CacheFilemanDate]
        public DateTime? FilemanDate { get; set; }

        [ColumnMap("Sql_DateTime")]
        public DateTime? SqlDateTime { get; set; }

        [ColumnMap("Sql_Date"), CacheHorologDate]
        public DateTime? SqlDate { get; set; }
    }
}
