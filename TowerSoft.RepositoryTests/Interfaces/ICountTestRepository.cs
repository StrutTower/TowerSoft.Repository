using System.Collections.Generic;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.Interfaces {
    public interface ICountTestRepository {
        public void Add(CountTest entity);
        public void Add(IEnumerable<CountTest> entity);
        public void Update(CountTest entity);
        public void Remove(CountTest entity);
        public List<CountTest> GetAll();
        public long GetCount();
        CountTest GetByID(int id);
        CountTest GetByName(string name);
    }
}