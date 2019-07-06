using System.Collections.Generic;
using TowerSoft.Repository;
using TowerSoft.Repository.MySql;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.MySql {
    public class PersonRepository : Repository<Person> {
        public PersonRepository(IUnitOfWork uow) : base(uow) { }

        public PersonRepository(string connectionString) : base(connectionString, new MySqlDbAdapter(), new PersonMaps()) { }

        public Person GetByID(int id) {
            return GetSingleEntity(new WhereCondition("ID", id));
        }

        //Expose Maps for unit tests only
        public IMap GetAutonumberMap() {
            return Mappings.AutonumberMap;
        }

        public IEnumerable<IMap> GetPrimaryKeyMaps() {
            return Mappings.PrimaryKeyMaps;
        }

        public IEnumerable<IMap> GetStandardMaps() {
            return Mappings.StandardMaps;
        }
        
        public IEnumerable<IMap> GetAllMaps() {
            return Mappings.AllMaps;
        }
    }
}
