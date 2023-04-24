//using System;
//using System.Collections.Generic;
//using System.Text;
//using TowerSoft.Repository;
//using TowerSoft.Repository.Cache;
//using TowerSoft.RepositoryTests.Interfaces;
//using TowerSoft.RepositoryTests.TestObjects;

//namespace TowerSoft.RepositoryTests.Databases.Cache {
//    public class DateTestRepository : DbRepository<CacheDateTest> {
//        public DateTestRepository(IUnitOfWork uow) : base(uow.DbAdapter) { }

//        public CacheDateTest GetByTitle(string title) {
//            return GetSingleEntity(WhereEqual(x => x.Title, title));
//        }

//        public CacheDateTest GetByFilemanDateTime(DateTime datetime) {
//            var logicalDate = CacheDateUtilities.DateTimeToLogicalDateTime(datetime);
//            return GetSingleEntity(WhereEqual(x => x.FilemanDateTime, logicalDate));
//        }

//        public CacheDateTest GetByFilemanDate(DateTime datetime) {
//            var logicalDate = CacheDateUtilities.DateTimeToLogicalDate(datetime);
//            return GetSingleEntity(WhereEqual(x => x.FilemanDate, logicalDate));
//        }

//        public List<CacheDateTest> GetByFilemanDateTimeBetweenDates(DateTime dateTime1, DateTime dateTime2) {
//            var date1logical = CacheDateUtilities.DateTimeToLogicalDateTime(dateTime1);
//            var date2logical = CacheDateUtilities.DateTimeToLogicalDateTime(dateTime2);
//            return GetEntities(QueryBuilder
//                .Where(x => x.FilemanDateTime, Comparison.GreaterThanOrEqual, date1logical)
//                .Where(x => x.FilemanDateTime, Comparison.LessThan, date2logical));
//        }
//    }
//}
