using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.DbRepository {
    [TestClass]
    public abstract class DbRepositoryTests {
        protected abstract ITestObjectRepository GetTestObjectRepository();
        protected abstract ICountTestRepository GetCountTestRepository();

        [TestMethod]
        public void Add_TestObject_ShouldAdd() {
            ITestObjectRepository repo = GetTestObjectRepository();
            TestObject newObj = new TestObject {
                Title = "Add Test",
                Description = "Add Test Description",
                StatusID = Status.Active,
                InputOn = DateTime.Now,
                InputByID = 1,
                IsActive = true
            };

            repo.Add(newObj);

            TestObject fromDB = repo.GetByTitle(newObj.Title);

            Assert.IsNotNull(fromDB); // Make sure object was returned
            Assert.AreNotEqual(0, fromDB); // Make sure autonumber was assigned
            Assert.IsTrue(newObj.AllPropsEqual(fromDB), "The object returned from the database does not match the original");
        }

        [TestMethod]
        public void Add_MultipleTestObjects_ShouldAdd() {
            ITestObjectRepository repo = GetTestObjectRepository();
            List<TestObject> objects = new List<TestObject>();
            for (int i = 0; i < 5000; i++) {
                objects.Add(new TestObject {
                    Title = "Add Multiple Test " + i,
                    Description = "Multiple Insert Test",
                    StatusID = Status.Active,
                    InputOn = DateTime.Now,
                    InputByID = 1,
                    IsActive = true
                });
            }

            repo.Add(objects);

            List<TestObject> testObjects = repo.GetByDescription("Multiple Insert Test");

            Assert.AreEqual(objects.Count, testObjects.Count); // Make sure the same number of objects are returns
            foreach (TestObject testObject in testObjects) {
                Assert.AreEqual("Multiple Insert Test", testObject.Description);
            }
        }

        [TestMethod]
        public void Update_TestObject_ShouldUpdate() {
            ITestObjectRepository repo = GetTestObjectRepository();
            TestObject newObj = new TestObject {
                Title = "Update Test",
                Description = "Update Test Description",
                StatusID = Status.Active,
                InputOn = DateTime.Now,
                InputByID = 1,
                IsActive = true
            };
            repo.Add(newObj);

            newObj.Title += " - Updated";
            newObj.StatusID = Status.Closed;
            repo.Update(newObj);
            TestObject fromDB = repo.GetByID(newObj.ID);

            Assert.IsNotNull(fromDB); // Make sure object was returned
            Assert.IsTrue(newObj.AllPropsEqual(fromDB), "The object returned from the database does not match the updated original");
        }

        [TestMethod]
        public void Remove_TestObject_ShouldRemove() {
            ITestObjectRepository repo = GetTestObjectRepository();
            TestObject newObj = new TestObject {
                Title = "Remove Test",
                Description = "Remove Test Description",
                StatusID = Status.Pending,
                InputOn = DateTime.Now,
                InputByID = 1,
                IsActive = true
            };
            repo.Add(newObj);

            TestObject fromDbNotNull = repo.GetByID(newObj.ID);
            repo.Remove(newObj);
            TestObject fromDbNull = repo.GetByID(newObj.ID);

            Assert.IsNotNull(fromDbNotNull);
            Assert.IsNull(fromDbNull);
        }

        [TestMethod]
        public void GetAll_CountTest_ShouldGetAll() {
            ICountTestRepository repo = GetCountTestRepository();
            List<CountTest> all = repo.GetAll();

            Assert.AreEqual(4, all.Count);
            Assert.AreEqual("Object 2", all.SingleOrDefault(x => x.ID == 2).Name);
        }

        [TestMethod]
        public void GetCount_Count_ShouldGetCount() {
            ICountTestRepository repo = GetCountTestRepository();
            long count = repo.GetCount();
            Assert.AreEqual(4, count);
        }
    }
}
