using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.Interfaces {
    public interface ITestObjectRepositoryKey {
        void Add(TestObject entity);
        void Add(IEnumerable<TestObject> entities);
        void Update(TestObject entity);
        void Remove(TestObject entity);
        TestObject GetByID(long id);
        Task<TestObject> GetByIDAsync(long id);
        List<TestObject> GetByIDs(IEnumerable<long> ids);
        Task<List<TestObject>> GetByIDsAsync(IEnumerable<long> id);
        Dictionary<long, TestObject> GetAllDictionary();
        Task<Dictionary<long, TestObject>> GetAllDictionaryAsync();
    }
}
