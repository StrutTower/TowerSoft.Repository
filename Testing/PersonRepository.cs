using TowerSoft.Repository;

namespace Testing {
    public class PersonRepository : Repository<Person> {

        public PersonRepository(IUnitOfWork uow) : base(uow, new PersonMap()) { }

        public Person GetByID(int id) {
            return GetSingleEntity(WhereEqual(x => x.ID, id));
        }
    }
}
