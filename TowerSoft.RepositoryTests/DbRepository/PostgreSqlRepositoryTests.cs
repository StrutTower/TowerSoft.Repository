using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.PostgreSql;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.DbRepository {
    [TestClass]
    public class PostgreSqlRepositoryTests : DbRepositoryTests {
        private static IUnitOfWork uow;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) {
            // Do not use a unit of work like this in a normal project. It must be disposed after use.
            uow = new UnitOfWork();
            uow.DbAdapter.DbConnection.Execute("CREATE TABLE IF NOT EXISTS repotest.testobject (" +
                "\"ID\" bigint NOT NULL GENERATED ALWAYS AS IDENTITY," +
                "\"Title\" character varying(45) NOT NULL," +
                "\"Description\" text," +
                "\"StatusID\" integer NOT NULL," +
                "\"InputOn\" timestamp without time zone NOT NULL," +
                "\"InputByID\" integer NOT NULL," +
                "\"IsActive\" boolean NOT NULL," +
                "PRIMARY KEY (\"ID\"));");
            uow.DbAdapter.DbConnection.Execute("TRUNCATE TABLE testobject");
            uow.DbAdapter.DbConnection.Execute("CREATE TABLE IF NOT EXISTS counttest (" +
                "Number INT PRIMARY KEY," +
                "Name VARCHAR(45) NOT NULL UNIQUE);");
            uow.DbAdapter.DbConnection.Execute("TRUNCATE TABLE counttest");
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
    }
}
