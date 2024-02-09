using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SQLite;
using System.IO;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.SQLite;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.DbRepository {
    [TestClass]
    public class SQLiteRepositoryTests : DbRepositoryTests {
        private static IUnitOfWork uow;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) {
            string dbPath = $"{Environment.CurrentDirectory}\\unittest.db";
            // Do not use a unit of work like this in a normal project. It must be disposed after use.
            uow = new UnitOfWork(dbPath);
            if (!File.Exists(dbPath))
                SQLiteConnection.CreateFile(dbPath);

            uow.DbAdapter.DbConnection.Execute("CREATE TABLE IF NOT EXISTS testobject (" +
                "ID INTEGER PRIMARY KEY AUTOINCREMENT," +
                "Title TEXT NOT NULL UNIQUE," +
                "Description TEXT," +
                "StatusID INTEGER NOT NULL," +
                "InputOn TEXT NOT NULL," +
                "InputByID INTEGER NOT NULL," +
                "IsActive INTEGER NOT NULL);");
            uow.DbAdapter.DbConnection.Execute("DELETE FROM testobject");
            uow.DbAdapter.DbConnection.Execute("CREATE TABLE IF NOT EXISTS counttest (" +
                "Number INTEGER PRIMARY KEY," +
                "Name TEXT NOT NULL UNIQUE);");
            uow.DbAdapter.DbConnection.Execute("DELETE FROM counttest");
            CountTestRepository repo = uow.GetRepo<CountTestRepository>();
            repo.Add(new CountTest { Number = 1, Name = "Object 1" });
            repo.Add(new CountTest { Number = 2, Name = "Object 2" });
            repo.Add(new CountTest { Number = 3, Name = "Object 3" });
            repo.Add(new CountTest { Number = 4, Name = "Object 4" });
        }

        protected override ITestObjectRepository GetTestObjectRepository() {
            return uow.GetRepo<TestObjectRepository>();
        }

        protected override ICountTestRepository GetCountTestRepository() {
            return uow.GetRepo<CountTestRepository>();
        }
    }
}
