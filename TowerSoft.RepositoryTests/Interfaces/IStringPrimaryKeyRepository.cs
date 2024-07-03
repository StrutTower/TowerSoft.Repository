using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.Interfaces {
    public interface IStringPrimaryKeyRepository {
        void Add(StringPrimaryKey entity);
        void Add(IEnumerable<StringPrimaryKey> entities);
        void Update(StringPrimaryKey entity);
        void Remove(StringPrimaryKey entity);
        StringPrimaryKey GetByID(string id);
        Task<StringPrimaryKey> GetByIDAsync(string id);
        List<StringPrimaryKey> GetByIDs(IEnumerable<string> ids);
        Task<List<StringPrimaryKey>> GetByIDsAsync(IEnumerable<string> id);
        Dictionary<string, StringPrimaryKey> GetAllDictionary();
        Task<Dictionary<string, StringPrimaryKey>> GetAllDictionaryAsync();
    }
}
