using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.Iris {
    public class CountTestRepository : AbstractCountTestRepository, ICountTestRepository {
        public CountTestRepository(IUnitOfWork uow) : base(uow) { }
    }
}
