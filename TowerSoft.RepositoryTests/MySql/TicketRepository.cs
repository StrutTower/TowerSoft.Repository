using TowerSoft.Repository;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.MySql {
    public class TestObjectRepository : Repository<Ticket> {
        public TestObjectRepository(IUnitOfWork uow) : base(uow) { }

        public Ticket GetByID(int id) {
            return GetSingleEntity(new WhereCondition("ID", id));
        }

        public Ticket GetByTitle(string title) {
            return GetSingleEntity(new WhereCondition("Title", title));
        }
    }
}
