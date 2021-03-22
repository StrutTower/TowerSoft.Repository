using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.SQLite {
    public class TestObjectRepository : AbstractTestObjectRepository, ITestObjectRepository {
        public TestObjectRepository(UnitOfWork uow) : base(uow) { }
    }
    }
