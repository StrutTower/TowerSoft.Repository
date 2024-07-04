using TowerSoft.Repository;
using TowerSoft.RepositoryExample.Domain;

namespace TowerSoft.RepositoryExample.Repository {
    public class TransactionRepository : DbRepositoryKey<Transaction, int> {
        public TransactionRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public List<Transaction> GetByPersonID(int personID) {
            return GetEntities(WhereEqual(x => x.PersonID, personID));
        }
    }
}
