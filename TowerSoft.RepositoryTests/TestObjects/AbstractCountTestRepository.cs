using TowerSoft.Repository;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class AbstractCountTestRepository : DbRepository<CountTest>, ICountTestRepository {
        public AbstractCountTestRepository(IUnitOfWork uow) : base(uow.DbAdapter) { }

        public CountTest GetByID(int id) {
            return GetSingleEntity(WhereEqual(x => x.ID, id));
        }

        public CountTest GetByName(string name) {
            return GetSingleEntity(WhereEqual(x => x.Name, name));
        }
    }
}
