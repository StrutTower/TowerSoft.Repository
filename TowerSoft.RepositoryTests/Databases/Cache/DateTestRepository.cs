using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.Repository.Cache;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.Databases.Cache {
    public class DateTestRepository : DbRepository<DateTest> {
        public DateTestRepository(IUnitOfWork uow) : base(uow.DbAdapter) { }

        public DateTest GetByTitle(string title) {
            return GetSingleEntity(WhereEqual(x => x.Title, title));
        }

        public DateTest GetByFilemanDateTime(DateTime datetime) {
            var logicalDate = CacheDateUtilities.DateTimeToLogicalDate(datetime);
            return GetSingleEntity(WhereEqual(x => x.FilemanDateTime, logicalDate));
        }

        public DateTest GetByFilemanDate(DateTime datetime) {
            var logicalDate = CacheDateUtilities.DateTimeToLogicalDate(datetime);
            return GetSingleEntity(WhereEqual(x => x.FilemanDate, logicalDate));
        }

        public List<DateTest> GetByFilemanDateTimeBetweenDates(DateTime dateTime1, DateTime dateTime2) {
            var date1logical = CacheDateUtilities.DateTimeToLogicalDate(dateTime1);
            var date2logical = CacheDateUtilities.DateTimeToLogicalDate(dateTime2);
            return GetEntities(QueryBuilder
                .Where(x => x.FilemanDateTime, Comparison.GreaterThanOrEqual, date1logical)
                .Where(x => x.FilemanDateTime, Comparison.LessThan, date2logical));
        }
    }
}
