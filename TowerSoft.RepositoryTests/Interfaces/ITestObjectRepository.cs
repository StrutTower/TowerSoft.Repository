using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.Interfaces {
    public interface ITestObjectRepository {
        void Add(TestObject entity);
        Task AddAsync(TestObject entity);
        void Add(IEnumerable<TestObject> entity);
        Task AddAsync(IEnumerable<TestObject> entity);
        void Update(TestObject entity);
        Task UpdateAsync(TestObject entity);
        void UpdateTitleAndStatus(TestObject entity);
        Task UpdateTitleAndStatusAsync(TestObject entity);
        void Remove(TestObject entity);
        Task RemoveAsync(TestObject entity);
        List<TestObject> GetAll();
        List<TestObject> GetAllSorted();
        long GetCount();
        TestObject GetByID(long id);
        TestObject GetByTitle(string title);
        List<TestObject> GetByDescription(string description);
        List<TestObject> GetActiveByDescription(string description);
        List<TestObject> GetByDescriptionWithInputOnOrderAsc(string description);
        List<TestObject> GetByDescriptionWithInputOnOrderDesc(string description);
        List<TestObject> GetByDescriptionWithLimit(string description, int limit);
        List<TestObject> GetByDescriptionWithLimitAndOffset(string description, int limit, int offset);
        List<TestObject> GetByDescriptionWithLimitOffsetAndSort(string description, int limit, int offset);
        List<TestObject> GetByInputOnDateRange(DateTime dateTime1, DateTime dateTime2);
        Task<List<TestObject>> GetByInputOnDateRangeAsync(DateTime dateTime1, DateTime dateTime2);
    }
}