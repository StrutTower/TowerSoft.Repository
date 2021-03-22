using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.MicrosoftSql {
    public class TestObjectRepository : AbstractTestObjectRepository, ITestObjectRepository {
        public TestObjectRepository(IUnitOfWork uow) : base(uow) { }
    }
}
