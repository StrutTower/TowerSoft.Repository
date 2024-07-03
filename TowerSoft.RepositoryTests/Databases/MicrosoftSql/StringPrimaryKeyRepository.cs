using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.MicrosoftSql {
    public class StringPrimaryKeyRepository : DbRepositoryKey<StringPrimaryKey, string>, IStringPrimaryKeyRepository {
        public StringPrimaryKeyRepository(UnitOfWork uow) : base(uow.DbAdapter) { }
    }
}
