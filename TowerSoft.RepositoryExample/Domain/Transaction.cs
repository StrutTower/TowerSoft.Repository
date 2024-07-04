using TowerSoft.Repository.Attributes;

namespace TowerSoft.RepositoryExample.Domain {
    [TableName("TransactionEntry")]
    public class Transaction {
        [Autonumber]
        public int ID { get; set; }

        public int PersonID { get; set; }

        public DateTime DateTime { get; set; }

        public double Amount { get; set; }
    }
}
