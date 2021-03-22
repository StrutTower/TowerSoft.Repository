using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.MySql {
    public class CountTestRepository : AbstractCountTestRepository, ICountTestRepository {
        public CountTestRepository(UnitOfWork uow) : base(uow) { }
    }
}
