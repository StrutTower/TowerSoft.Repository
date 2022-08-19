using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.Repository.Cache;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.Databases.Iris {
    public class DateTestRepository : DbRepository<IrisDateTest> {
        public DateTestRepository(IUnitOfWork uow) : base(uow.DbAdapter) { }

        public IrisDateTest GetByTitle(string title) {
            return GetSingleEntity(WhereEqual(x => x.Title, title));
        }

        public IrisDateTest GetByFilemanDateTime(DateTime datetime) {
            return GetSingleEntity(WhereEqual(x => x.FilemanDateTime, datetime));
        }

        public IrisDateTest GetByFilemanDate(DateTime datetime) {
            var logicalDate = CacheDateUtilities.DateTimeToLogicalDate(datetime);
            return GetSingleEntity(WhereEqual(x => x.FilemanDate, logicalDate));
        }

        public List<IrisDateTest> GetByFilemanDateTimeBetweenDates(DateTime dateTime1, DateTime dateTime2) {
            return GetEntities(QueryBuilder
                .Where(x => x.FilemanDateTime, Comparison.GreaterThanOrEqual, dateTime1)
                .Where(x => x.FilemanDateTime, Comparison.LessThan, dateTime2));
        }
    }
}
