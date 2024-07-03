using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.SQLite {
    public class TestObjectRepositoryKey : DbRepositoryKey<TestObject, long>, ITestObjectRepositoryKey {
        public TestObjectRepositoryKey(UnitOfWork uow) : base(uow.DbAdapter) { }
    }
}
