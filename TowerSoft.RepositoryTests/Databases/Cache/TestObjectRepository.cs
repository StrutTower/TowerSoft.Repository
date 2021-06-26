using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.Cache {
    public class TestObjectRepository : AbstractTestObjectRepository, ITestObjectRepository {
        public TestObjectRepository(IUnitOfWork uow) : base(uow) { }

        public List<TestObject> GetByInputOnDateRange(DateTime dateTime1, DateTime dateTime2) {
            return GetEntities(QueryBuilder
                .Where(x => x.InputOn, Comparison.GreaterThanOrEqual, dateTime1)
                .Where(x => x.InputOn, Comparison.LessThan, dateTime2));
        }
    }
}
