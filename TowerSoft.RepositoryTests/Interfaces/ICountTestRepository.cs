using System.Collections.Generic;
using System.Threading.Tasks;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.Interfaces {
    public interface ICountTestRepository {
        public void Add(CountTest entity);
        public void Add(IEnumerable<CountTest> entity);
        public void Update(CountTest entity);
        public void Remove(CountTest entity);
        public List<CountTest> GetAll();
        public Task<List<CountTest>> GetAllAsync();
        public long GetCount();
        public Task<long> GetCountAsync();
        CountTest GetByID(int id);
        CountTest GetByName(string name);
    }
}