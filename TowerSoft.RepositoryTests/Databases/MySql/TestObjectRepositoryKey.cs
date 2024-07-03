using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.MySql {
    public class TestObjectRepositoryKey : DbRepositoryKey<TestObject, long>, ITestObjectRepositoryKey {
        public TestObjectRepositoryKey(UnitOfWork uow) : base(uow.DbAdapter) { }
    }
}
