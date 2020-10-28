using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.PostgreSql {
    [TestClass]
    public class PostgreSqlRepositoryTests {
        private static UnitOfWork uow;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) {
            uow = new UnitOfWork();
            uow.DbAdapter.DbConnection.Execute("CREATE TABLE IF NOT EXISTS testobject (" +
                "ID BIGINT(20) AUTO_INCREMENT PRIMARY KEY," +
                "Title VARCHAR(45) NOT NULL UNIQUE," +
                "Description MEDIUMTEXT," +
                "StatusID INT NOT NULL," +
                "InputOn DATETIME NOT NULL," +
                "InputByID INT NOT NULL," +
                "IsActive TINYINT(1) NOT NULL) " +
                "ENGINE=InnoDB;");
            uow.DbAdapter.DbConnection.Execute("TRUNCATE TABLE testobject");
            uow.DbAdapter.DbConnection.Execute("CREATE TABLE IF NOT EXISTS counttest (" +
                "ID INT AUTO_INCREMENT PRIMARY KEY," +
                "Name VARCHAR(45) NOT NULL UNIQUE) " +
                "ENGINE=InnoDB;");
            uow.DbAdapter.DbConnection.Execute("TRUNCATE TABLE counttest");
            CountTestRepository repo = uow.GetRepo<CountTestRepository>();
            repo.Add(new CountTest { ID = 1, Name = "Object 1" });
            repo.Add(new CountTest { ID = 2, Name = "Object 2" });
            repo.Add(new CountTest { ID = 3, Name = "Object 3" });
            repo.Add(new CountTest { ID = 4, Name = "Object 4" });
        }
    }
}
