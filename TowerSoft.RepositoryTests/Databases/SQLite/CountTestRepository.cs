using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.SQLite {
    public class CountTestRepository : AbstractCountTestRepository, ICountTestRepository {
        public CountTestRepository(UnitOfWork uow) : base(uow) { }
    }
}
