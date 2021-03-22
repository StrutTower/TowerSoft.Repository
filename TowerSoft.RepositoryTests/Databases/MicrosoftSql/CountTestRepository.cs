using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.MicrosoftSql {
    public class CountTestRepository : AbstractCountTestRepository, ICountTestRepository {
        public CountTestRepository(UnitOfWork uow) : base(uow) { }
    }
}
