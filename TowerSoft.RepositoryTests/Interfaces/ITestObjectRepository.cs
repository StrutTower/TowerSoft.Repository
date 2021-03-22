using System.Collections.Generic;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.Interfaces {
    public interface ITestObjectRepository {
        public void Add(TestObject entity);
        public void Add(IEnumerable<TestObject> entity);
        public void Update(TestObject entity);
        public void Remove(TestObject entity);
        public List<TestObject> GetAll();
        public long GetCount();
        TestObject GetByID(long id);
        TestObject GetByTitle(string title);
        List<TestObject> GetByDescription(string description);
    }
}