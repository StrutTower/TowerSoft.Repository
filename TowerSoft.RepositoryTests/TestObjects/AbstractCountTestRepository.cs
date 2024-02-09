using TowerSoft.Repository;
using TowerSoft.RepositoryTests.Interfaces;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class AbstractCountTestRepository : DbRepository<CountTest>, ICountTestRepository {
        public AbstractCountTestRepository(UnitOfWorkBase uow) : base(uow.DbAdapter) { }

        public CountTest GetByID(int id) {
            return GetSingleEntity(WhereEqual(x => x.Number, id));
        }

        public CountTest GetByName(string name) {
            return GetSingleEntity(WhereEqual(x => x.Name, name));
        }
    }
}
