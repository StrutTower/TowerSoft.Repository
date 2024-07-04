using TowerSoft.Repository;
using TowerSoft.RepositoryExample.Domain;

namespace TowerSoft.RepositoryExample.Repository {
    public class PersonRepository : DbRepository<Person> {
        public PersonRepository(UnitOfWork uow) : base(uow.DbAdapter, "Person", GetMaps()) { }

        private static List<IMap> GetMaps() {
            return new MapBuilder<Person>()
                .AutonumberMap(x => x.ID)
                .Map(x => x.DisplayName)
                .Map(x => x.CreatedOn)
                .Map(x => x.DateOfBirth, "Birthdate")
                .Map(x => x.IsActive);
        }

        #region Query by single column
        // Returns a single entity with a matching ID
        public Person GetByID(int id) {
            return GetSingleEntity(WhereEqual(x => x.ID, id));
        }

        // Returns all entities with IsActive equal to true
        public List<Person> GetActive() {
            return GetEntities(WhereEqual(x => x.IsActive, true));
        }

        // Returns all entities with a CreatedOn date before the supplied DateTime
        public List<Person> GetCreatedBefore(DateTime datetime) {
            return GetEntities(Where(x => x.CreatedOn, Comparison.LessThan, datetime));
        }

        // Return all entities that contain the supplied string in the DisplayName field
        public List<Person> SearchDisplayName(string q) {
            return GetEntities(Where(x => x.DisplayName, Comparison.LikeBothSidesWildcard, q));
        }
        #endregion

        #region Query by multiple columns
        // Returns all active entities with a display name
        public List<Person> GetActiveWithDisplayName() {
            return GetEntities(Query
                .WhereEqual(x => x.IsActive, true)
                .Where(x => x.DisplayName, Comparison.NotEquals, null));
        }

        // Return all entities that contain the supplied string in the DisplayName field.
        // Limit and offset the results by the supplied numbers.
        public List<Person> SearchDisplayName(string q, int limit, int offset) {
            return GetEntities(Query
                .Where(x => x.DisplayName, Comparison.LikeBothSidesWildcard, q)
                .LimitTo(limit)
                .OffsetBy(offset));
        }

        // Returns all entities created between the two dates and sort by a descending order based on CreatedOn
        public List<Person> GetCreatedBetweenDates(DateTime start, DateTime end) {
            return GetEntities(Query
                .Where(x => x.CreatedOn, Comparison.GreaterThan, start)
                .Where(x => x.CreatedOn, Comparison.LessThanOrEqual, end)
                .OrderByDescending(x => x.CreatedOn));
        }
        #endregion

        #region Custom SQL Queries
        // Get all entities that are inactive or the display name is NULL
        public List<Person> GetInactiveOrWithoutDisplayName() {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery +=
                $"WHERE t.IsActive = 0 " +
                $"OR DisplayName IS NULL ";
            return GetEntities(query);
        }

        // Get all entities based on a join to another table. This does not SELECT any data from the joined table.
        public List<Person> GetWithTransactionsAfterDate(DateTime datetime) {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery +=
                $"INNER JOIN transaction t ON {TableName}.ID = t.PersonID " +
                $"WHERE t.DateTime > @DateTime ";
            query.AddParameter("@DateTime", datetime);
            return GetEntities(query);
        }

        #endregion
    }
}
