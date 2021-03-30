using System;
using System.Collections.Generic;
using TowerSoft.Repository;
using TowerSoft.RepositoryTests.Interfaces;

namespace TowerSoft.RepositoryTests.Databases.Cache {
    public class FMCacheDotNetRepository : DbRepository<FMCacheDotNet> {
        public FMCacheDotNetRepository(IUnitOfWork uow) : base(uow.DbAdapter) { }

        public FMCacheDotNet GetByIEN(int ien) {
            return GetSingleEntity(WhereEqual(x => x.IEN, ien));
        }

        public FMCacheDotNet GetByTitle(string title) {
            return GetSingleEntity(WhereEqual(x => x.Title, title));
        }

        public List<FMCacheDotNet> GetByInputByID(int inputByID) {
            return GetEntities(WhereEqual(x => x.InputByID, inputByID));
        }
    }
}
