using System;
using System.Collections.Generic;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.Interfaces {
    public interface ITestObjectRepository {
        public void Add(TestObject entity);
        public void Add(IEnumerable<TestObject> entity);
        public void Update(TestObject entity);
        public void Remove(TestObject entity);
        public List<TestObject> GetAll();
        public List<TestObject> GetAllSorted();
        public long GetCount();
        TestObject GetByID(long id);
        TestObject GetByTitle(string title);
        List<TestObject> GetByDescription(string description);
        List<TestObject> GetByDescriptionWithInputOnOrderAsc(string description);
        List<TestObject> GetByDescriptionWithInputOnOrderDesc(string description);
        List<TestObject> GetByDescriptionWithLimit(string description, int limit);
        List<TestObject> GetByDescriptionWithLimitAndOffset(string description, int limit, int offset);
        List<TestObject> GetByDescriptionWithLimitOffsetAndSort(string description, int limit, int offset);
        List<TestObject> GetByInputOnDateRange(DateTime dateTime1, DateTime dateTime2);
    }
}