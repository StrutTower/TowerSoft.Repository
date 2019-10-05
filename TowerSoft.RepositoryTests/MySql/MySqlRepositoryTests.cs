using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TowerSoft.Repository;
using TowerSoft.Repository.MySql;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.MySql {
    [TestClass]
    public class MySqlRepositoryTests {
        //private static string _connectionString = "server=towerserver;uid=unittests;password=$1letmein;database=unittests";

        //[ClassInitialize]
        //public static void ClassInitialize(TestContext testContext) {
        //    using (IUnitOfWork uow = new MySqlUnitOfWork(_connectionString)) {
        //        uow.DbConnection.Execute("TRUNCATE TABLE ticket;");
        //        uow.DbConnection.Execute("TRUNCATE TABLE status;");
        //        uow.DbConnection.Execute("TRUNCATE TABLE person;");

        //        #region Seed
        //        new StatusRepository(uow).Add(new Status { Name = "Active" });
        //        new StatusRepository(uow).Add(new Status { Name = "Inactive" });
        //        new StatusRepository(uow).Add(new Status { Name = "Completed" });
        //        new PersonRepository(uow).Add(new Person { FirstName = "John", LastName = "Doe" });
        //        #endregion
        //    }
        //}

        //[TestMethod]
        //public void Add_TestObject_ShouldAdd() {
        //    using (IUnitOfWork uow = new MySqlUnitOfWork(_connectionString)) {
        //        TestObjectRepository repo = new TestObjectRepository(uow);
        //        Ticket ticket = new Ticket {
        //            Title = "Add Test Object",
        //            Description = "Description Here",
        //            StatusID = 1,
        //            InputOn = DateTime.Now,
        //            InputByID = 1,
        //            IsActive = true
        //        };
        //        repo.Add(ticket);

        //        var result = repo.GetByTitle(ticket.Title);
        //        Assert.IsNotNull(result);
        //        Assert.AreNotEqual(null, result.ID);
        //        Assert.AreNotEqual(0, result.ID);
        //        Assert.AreEqual(ticket.Title, result.Title);
        //        Assert.AreEqual(ticket.Description, result.Description);
        //        Assert.AreEqual(ticket.StatusID, result.StatusID);
        //        // Seconds sometimes get rounded differently I think so ignore the seconds in the assert
        //        Assert.AreEqual(ticket.InputOn.ToString("yyyy/M/d HH:mm"), result.InputOn.ToString("yyyy/M/d HH:mm"));
        //        Assert.AreEqual(ticket.InputByID, result.InputByID);
        //        Assert.AreEqual(ticket.IsActive, result.IsActive);
        //    }
        //}

        //[TestMethod]
        //public void Update_TestObject_ShouldUpdate() {
        //    using (IUnitOfWork uow = new MySqlUnitOfWork(_connectionString)) {
        //        TestObjectRepository repo = new TestObjectRepository(uow);
        //        Ticket ticket = new Ticket {
        //            Title = "Update Test Object",
        //            Description = null,
        //            StatusID = 1,
        //            InputOn = DateTime.Now,
        //            InputByID = 1,
        //            IsActive = true
        //        };
        //        repo.Add(ticket);

        //        ticket.Description = "New Description";
        //        repo.Update(ticket);
        //        Ticket result = repo.GetByTitle("Update Test Object");

        //        Assert.AreEqual(ticket.Description, result.Description);
        //    }
        //}

        //[TestMethod]
        //public void Remove_TestObject_ShouldRemove() {
        //    using (IUnitOfWork uow = new MySqlUnitOfWork(_connectionString)) {
        //        TestObjectRepository repo = new TestObjectRepository(uow);
        //        Ticket ticket = new Ticket {
        //            Title = "Remove Test Object",
        //            Description = null,
        //            StatusID = 1,
        //            InputOn = DateTime.Now,
        //            InputByID = 1,
        //            IsActive = true
        //        };
        //        repo.Add(ticket);

        //        repo.Remove(ticket);

        //        Ticket result = repo.GetByTitle("Remove Test Object");

        //        Assert.AreEqual(null, result);
        //    }
        //}
        
        //[TestMethod]
        //public void GetAll_Statuses_ShouldGetAll() {

        //}
    }
}
