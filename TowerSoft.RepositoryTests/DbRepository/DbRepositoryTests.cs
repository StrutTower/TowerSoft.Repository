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
            TestObject expected = new TestObject {
                Title = "Add Test",
                Description = "Add Test Description",
                StatusID = Status.Active,
                InputOn = DateTime.Now,
                InputByID = 1,
                IsActive = true
            };

            repo.Add(expected);

            TestObject actual = repo.GetByTitle(expected.Title);

            Assert.IsNotNull(actual); // Make sure object was returned
            Assert.AreNotEqual(0, actual); // Make sure autonumber was assigned
            Assert.IsTrue(expected.AllPropsEqual(actual), "The object returned from the database does not match the original");
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
            Assert.AreEqual("Object 2", all.SingleOrDefault(x => x.Number == 2).Name);
        }

        [TestMethod]
        public void GetCount_Count_ShouldGetCount() {
            ICountTestRepository repo = GetCountTestRepository();
            long count = repo.GetCount();
            Assert.AreEqual(4, count);
        }

        [TestMethod]
        public void GetBetweenDates_ShouldReturnValidResults() {
            ITestObjectRepository repo = GetTestObjectRepository();
            TestObject newObj1 = new TestObject {
                Title = "Date Range Test 1",
                Description = "Remove Test Description",
                StatusID = Status.Active,
                InputOn = new DateTime(1999, 1, 4, 12, 34, 20),
                InputByID = 1,
                IsActive = true
            };
            TestObject newObj2 = new TestObject {
                Title = "Date Range Test 2",
                Description = "Remove Test Description",
                StatusID = Status.Active,
                InputOn = new DateTime(1999, 1, 6, 8, 4, 56),
                InputByID = 1,
                IsActive = true
            }; 
            TestObject newObj3 = new TestObject {
                Title = "Date Range Test 3",
                Description = "Remove Test Description",
                StatusID = Status.Active,
                InputOn = new DateTime(1999, 1, 9, 18, 14, 0),
                InputByID = 1,
                IsActive = true
            };
            repo.Add(newObj1);
            repo.Add(newObj2);
            repo.Add(newObj3);

            List<TestObject> actual = repo.GetByInputOnDateRange(new DateTime(1999, 1, 2), new DateTime(1999, 1, 7));

            Assert.AreEqual(2, actual.Count);
        }
    }
}
