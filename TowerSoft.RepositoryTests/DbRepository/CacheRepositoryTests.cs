using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TowerSoft.RepositoryTests.Cache;
using TowerSoft.RepositoryTests.Databases.Cache;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.DbRepository {
    [TestClass]
    public class CacheRepositoryTests : DbRepositoryTests {
        private static IUnitOfWork uow;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) {
            uow = new UnitOfWork();
            uow.DbAdapter.DbConnection.Execute("DROP TABLE testobject");
            uow.DbAdapter.DbConnection.Execute("DROP TABLE counttest");
            uow.DbAdapter.DbConnection.Execute("DROP TABLE datetest");
            uow.DbAdapter.DbConnection.Execute("DELETE FROM FM.CACHEDOTNET");
            uow.DbAdapter.DbConnection.Execute("" +
                    "CREATE TABLE testobject (" +
                    //"ID INT PRIMARY KEY, " + // ID column is added automatically
                    "Title VARCHAR(45) NOT NULL UNIQUE," +
                    "Description %String," +
                    "StatusID INT NOT NULL," +
                    "InputOn DATETIME NOT NULL," +
                    "InputByID INT NOT NULL," +
                    "IsActive TINYINT(1) NOT NULL) ");
            uow.DbAdapter.DbConnection.Execute("" +
                    "CREATE TABLE counttest (" +
                    "Number INT UNIQUE," +
                    "Name VARCHAR(45) NOT NULL UNIQUE) ");
            uow.DbAdapter.DbConnection.Execute("" +
                    "CREATE TABLE datetest (" +
                    "Title VARCHAR(45) NOT NULL UNIQUE," +
                    "Fileman_DateTime %FilemanTimeStamp," +
                    "Fileman_Date %FilemanDate," +
                    "Sql_DateTime DATETIME," +
                    "Sql_Date DATE) ");
            CountTestRepository repo = uow.GetRepo<CountTestRepository>();
            repo.Add(new CountTest { Number = 1, Name = "Object 1" });
            repo.Add(new CountTest { Number = 2, Name = "Object 2" });
            repo.Add(new CountTest { Number = 3, Name = "Object 3" });
            repo.Add(new CountTest { Number = 4, Name = "Object 4" });
        }

        protected override ICountTestRepository GetCountTestRepository() {
            return uow.GetRepo<CountTestRepository>();
        }

        protected override ITestObjectRepository GetTestObjectRepository() {
            return uow.GetRepo<TestObjectRepository>();
        }

        [TestMethod]
        public void AddDate_ShouldAddAndReturnDates() {
            DateTest expected = new DateTest {
                Title = "AddDate",
                FilemanDateTime = new DateTime(2021, 3, 31, 10, 10, 30),
                FilemanDate = new DateTime(2020, 1, 15),
                SqlDateTime = new DateTime(2000, 6, 20, 10, 13, 45),
                SqlDate = new DateTime(2010, 12, 1)
            };
            DateTestRepository repo = uow.GetRepo<DateTestRepository>();
            repo.Add(expected);

            DateTest actual = repo.GetByTitle(expected.Title);

            Assert.AreEqual(expected.FilemanDateTime, actual.FilemanDateTime);
            Assert.AreEqual(expected.FilemanDate, actual.FilemanDate);
            Assert.AreEqual(expected.SqlDateTime, actual.SqlDateTime);
            Assert.AreEqual(expected.SqlDate, actual.SqlDate);
        }

        [TestMethod]
        public void UpdateDate_ShouldUpdateDates() {
            DateTest initial = new DateTest {
                Title = "UpdateDate",
                FilemanDateTime = new DateTime(2021, 3, 31, 10, 10, 30),
                FilemanDate = new DateTime(2020, 1, 15),
                SqlDateTime = new DateTime(2000, 6, 20, 10, 13, 45),
                SqlDate = new DateTime(2010, 12, 1)
            };
            DateTestRepository repo = uow.GetRepo<DateTestRepository>();
            repo.Add(initial);

            DateTest expected = repo.GetByTitle("UpdateDate");
            expected.FilemanDateTime = expected.FilemanDateTime.Value.AddDays(1);
            expected.FilemanDate = expected.FilemanDate.Value.AddDays(1);
            expected.SqlDateTime = expected.SqlDateTime.Value.AddDays(1);
            expected.SqlDate = expected.SqlDate.Value.AddDays(1);
            repo.Update(expected);

            DateTest actual = repo.GetByTitle("UpdateDate");
            Assert.AreEqual(expected.FilemanDateTime, actual.FilemanDateTime, "FilemanDateTime");
            Assert.AreEqual(expected.FilemanDate, actual.FilemanDate, "FilemanDate");
            Assert.AreEqual(expected.SqlDateTime, actual.SqlDateTime, "SqlDateTime");
            Assert.AreEqual(expected.SqlDate, actual.SqlDate, "SqlDate");
        }

        [TestMethod]
        public void GetByFilemanDate() {
            DateTime expected = new DateTime(2020, 12, 15);
            DateTest expectedDateTime = new DateTest {
                Title = "FilemanDate",
                FilemanDate = expected
            };
            DateTestRepository repo = uow.GetRepo<DateTestRepository>();
            repo.Add(expectedDateTime);

            DateTest actual = repo.GetByFilemanDate(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedDateTime.Title, actual.Title, "Title");
            Assert.AreEqual(expectedDateTime.FilemanDate, actual.FilemanDate, "FilemanDate");
        }

        [TestMethod]
        public void GetByFilemanDateTime() {
            DateTime expected = new DateTime(1990, 1, 31, 10, 00, 30);
            DateTest expectedDateTime = new DateTest {
                Title = "FilemanDateTime",
                FilemanDateTime = expected
            };
            DateTestRepository repo = uow.GetRepo<DateTestRepository>();
            repo.Add(expectedDateTime);

            DateTest actual = repo.GetByFilemanDateTime(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedDateTime.Title, actual.Title, "Title");
            Assert.AreEqual(expectedDateTime.FilemanDateTime, actual.FilemanDateTime, "FilemanDateTime");
        }

        [TestMethod]
        public void GetByDatesBetweenDateRange() {
            DateTest datetime1 = new DateTest {
                Title = "FilemanDateTimeRange1",
                FilemanDateTime = new DateTime(1971, 2, 4, 12, 30, 00)
            };
            DateTest datetime2 = new DateTest {
                Title = "FilemanDateTimeRange2",
                FilemanDateTime = new DateTime(1971, 2, 13, 10, 15, 00)
            };
            DateTest datetime3 = new DateTest {
                Title = "FilemanDateTimeRange3",
                FilemanDateTime = new DateTime(1971, 2, 18, 11, 15, 00)
            };
            DateTestRepository repo = uow.GetRepo<DateTestRepository>();
            repo.Add(datetime1);
            repo.Add(datetime2);
            repo.Add(datetime3);

            List<DateTest> results = repo.GetByFilemanDateTimeBetweenDates(new DateTime(1971, 2, 3), new DateTime(1971, 2, 14));
            Assert.AreEqual(2, results.Count);
        }

        #region Fileman Tests
        [TestMethod]
        public void Add_FMCacheDotNet_ShouldAdd() {
            FMCacheDotNetRepository repo = uow.GetRepo<FMCacheDotNetRepository>();
            FMCacheDotNet expected = new FMCacheDotNet {
                Name = 1,
                Title = "Add Test",
                StatusID = Status.Active,
                InputOn = DateTime.Now,
                InputByID = 1,
                IsActive = true
            };
            repo.Add(expected);

            FMCacheDotNet actual = repo.GetByTitle(expected.Title);

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.AllPropsEqual(actual));
        }

        [TestMethod]
        public void AddMultiple_FMCacheDotNet_ShouldAdd() {
            string title = "Multiple Insert Test";
            FMCacheDotNetRepository repo = uow.GetRepo<FMCacheDotNetRepository>();
            List<FMCacheDotNet> expected = new List<FMCacheDotNet>();
            for (int i = 0; i < 3; i++) {
                expected.Add(new FMCacheDotNet {
                    Name = 99,
                    Title = $"{title}",
                    StatusID = (Status)(i + 1),
                    InputOn = DateTime.Now,
                    InputByID = 99,
                    IsActive = true
                });
            }

            repo.Add(expected);

            List<FMCacheDotNet> actual = repo.GetByInputByID(99);

            Assert.AreEqual(expected.Count, actual.Count); // Make sure the same number of objects are returns
            foreach (FMCacheDotNet testObject in actual) {
                Assert.IsTrue(testObject.Title.StartsWith(title));
            }
        }
        #endregion
    }
}
