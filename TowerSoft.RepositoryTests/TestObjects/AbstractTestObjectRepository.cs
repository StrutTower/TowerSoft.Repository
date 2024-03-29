﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.Repository.Builders;
using TowerSoft.RepositoryTests.Interfaces;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class AbstractTestObjectRepository : DbRepository<TestObject> {
        public AbstractTestObjectRepository(UnitOfWorkBase uow) : base(uow.DbAdapter, "testobject", GetMaps()) { }

        private static List<IMap> GetMaps() {
            return new MapBuilder<TestObject>()
                .AutonumberMap(x => x.ID)
                .Map(x => x.Title)
                .Map(x => x.Description)
                .Map(x => x.StatusID)
                .Map(x => x.InputOn)
                .Map(x => x.InputByID)
                .Map(x => x.IsActive);
        }

        public List<TestObject> GetAllSorted() {
            return GetEntities(Query
                .OrderBy(x => x.InputOn)
                .OrderBy(x => x.Title));
        }

        public TestObject GetByID(long id) {
            return GetSingleEntity(WhereEqual(x => x.ID, id));
        }

        public TestObject GetByTitle(string title) {
            return GetSingleEntity(WhereEqual(x => x.Title, title));
        }

        public List<TestObject> GetByDescription(string description) {
            return GetEntities(Query
                .WhereEqual(x => x.Description, description)
                .OrderBy(x => x.Title));
        }

        public List<TestObject> GetActiveByDescription(string description) {
            return GetEntities(Query
                .WhereEqual(x => x.Description, description)
                .WhereEqual(x => x.StatusID, Status.Active)
                .OrderBy(x => x.Title));
        }

        public List<TestObject> GetByDescriptionWithInputOnOrderAsc(string description) {
            return GetEntities(Query
                .WhereEqual(x => x.Description, description)
                .WhereEqual(x => x.StatusID, Status.Active)
                .OrderBy(x => x.InputOn));
        }

        public List<TestObject> GetByDescriptionWithInputOnOrderDesc(string description) {
            return GetEntities(Query
                .WhereEqual(x => x.Description, description)
                .WhereEqual(x => x.StatusID, Status.Active)
                .OrderByDescending(x => x.InputOn));
        }

        public List<TestObject> GetByDescriptionWithLimit(string description, int limit) {
            return GetEntities(Query
                .WhereEqual(x => x.Description, description)
                .OrderByDescending(x => x.ID)
                .LimitTo(limit));
        }

        public List<TestObject> GetByDescriptionWithLimitAndOffset(string description, int limit, int offset) {
            return GetEntities(Query
                .WhereEqual(x => x.Description, description)
                .OrderBy(x => x.ID)
                .LimitTo(limit)
                .OffsetBy(offset));
        }

        public List<TestObject> GetByDescriptionWithLimitOffsetAndSort(string description, int limit, int offset) {
            return GetEntities(Query
                .WhereEqual(x => x.Description, description)
                .OrderBy(x => x.InputOn)
                .LimitTo(limit)
                .OffsetBy(offset));
        }

        public void UpdateTitleAndStatus(TestObject testObject) {
            UpdateColumns(testObject,
                x => x.Title,
                x => x.StatusID);
        }

        public async Task UpdateTitleAndStatusAsync(TestObject testObject) {
            await UpdateColumnsAsync(testObject,
                x => x.Title,
                x => x.StatusID);
        }
    }
}
