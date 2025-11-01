using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.MySqlConnector {
    public class TestObjectRepository(UnitOfWork uow) : AbstractTestObjectRepository(uow), ITestObjectRepository {
        public List<TestObject> GetByInputOnDateRange(DateTime dateTime1, DateTime dateTime2) {
            return GetEntities(Query
                .Where(x => x.InputOn, Comparison.GreaterThanOrEqual, dateTime1)
                .Where(x => x.InputOn, Comparison.LessThan, dateTime2));
        }

        public async Task<List<TestObject>> GetByInputOnDateRangeAsync(DateTime dateTime1, DateTime dateTime2) {
            return await GetEntitiesAsync(Query
                .Where(x => x.InputOn, Comparison.GreaterThanOrEqual, dateTime1)
                .Where(x => x.InputOn, Comparison.LessThan, dateTime2));
        }
    }
}
