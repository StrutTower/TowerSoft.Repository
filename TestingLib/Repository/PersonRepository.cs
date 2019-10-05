using TestingLib.Domain;
using TowerSoft.Repository;

namespace TestingLib.Repository {
    public class PersonRepository : Repository<Person> {

        public PersonRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public Person GetByID(int id) {
            return GetSingleEntity(WhereEqual(x => x.ID, id));
        }
    }
}
