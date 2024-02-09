using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.PostgreSql {
    public class TestObjectRepository : AbstractTestObjectRepository, ITestObjectRepository {
        public TestObjectRepository(UnitOfWork uow) : base(uow) { }

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
