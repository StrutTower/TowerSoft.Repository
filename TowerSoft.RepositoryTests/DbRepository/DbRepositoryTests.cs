using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.DbRepository {
    [TestClass]
    public abstract class DbRepositoryTests {
        protected abstract ITestObjectRepository GetTestObjectRepository();
        protected abstract ICountTestRepository GetCountTestRepository();

        #region Add/Update/Remove
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
        public async Task Async_Add_TestObject_ShouldAdd() {
            ITestObjectRepository repo = GetTestObjectRepository();
            TestObject expected = new TestObject {
                Title = "AddAsync Test",
                Description = "AddAsync Test Description",
                StatusID = Status.Active,
                InputOn = DateTime.Now,
                InputByID = 1,
                IsActive = true
            };

            await repo.AddAsync(expected);

            TestObject actual = repo.GetByTitle(expected.Title);

            Assert.IsNotNull(actual); // Make sure object was returned
            Assert.AreNotEqual(0, actual); // Make sure autonumber was assigned
            Assert.IsTrue(expected.AllPropsEqual(actual), "The object returned from the database does not match the original");
        }

        [TestMethod]
        public virtual void Add_MultipleTestObjects_ShouldAdd() {
            ITestObjectRepository repo = GetTestObjectRepository();
            List<TestObject> objects = new List<TestObject>();
            for (int i = 0; i < 1000; i++) {
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

            Assert.AreEqual(objects.Count, testObjects.Count); // Make sure the same number of objects are returned
            foreach (TestObject testObject in testObjects) {
                Assert.AreEqual("Multiple Insert Test", testObject.Description);
            }
        }

        [TestMethod]
        public virtual async Task Async_Add_MultipleTestObjects_ShouldAdd() {
            ITestObjectRepository repo = GetTestObjectRepository();
            List<TestObject> objects = new List<TestObject>();
            for (int i = 0; i < 1000; i++) {
                objects.Add(new TestObject {
                    Title = "AddAsync Multiple Test " + i,
                    Description = "MultipleAsync Insert Test",
                    StatusID = Status.Active,
                    InputOn = DateTime.Now,
                    InputByID = 1,
                    IsActive = true
                });
            }

            await repo.AddAsync(objects);

            List<TestObject> testObjects = repo.GetByDescription("MultipleAsync Insert Test");

            Assert.AreEqual(objects.Count, testObjects.Count); // Make sure the same number of objects are returned
            foreach (TestObject testObject in testObjects) {
                Assert.AreEqual("MultipleAsync Insert Test", testObject.Description);
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
        public async Task Async_Update_TestObject_ShouldUpdate() {
            ITestObjectRepository repo = GetTestObjectRepository();
            TestObject newObj = new TestObject {
                Title = "UpdateAsync Test",
                Description = "UpdateAsync Test Description",
                StatusID = Status.Active,
                InputOn = DateTime.Now,
                InputByID = 1,
                IsActive = true
            };
            repo.Add(newObj);

            newObj.Title += " - Updated";
            newObj.StatusID = Status.Closed;
            await repo.UpdateAsync(newObj);
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
        public async Task Async_Remove_TestObject_ShouldRemove() {
            ITestObjectRepository repo = GetTestObjectRepository();
            TestObject newObj = new TestObject {
                Title = "RemoveAsync Test",
                Description = "RemoveAsync Test Description",
                StatusID = Status.Pending,
                InputOn = DateTime.Now,
                InputByID = 1,
                IsActive = true
            };
            repo.Add(newObj);

            TestObject fromDbNotNull = repo.GetByID(newObj.ID);
            await repo.RemoveAsync(newObj);
            TestObject fromDbNull = repo.GetByID(newObj.ID);

            Assert.IsNotNull(fromDbNotNull);
            Assert.IsNull(fromDbNull);
        }
        #endregion

        [TestMethod]
        public void GetAll_CountTest_ShouldGetAll() {
            ICountTestRepository repo = GetCountTestRepository();
            List<CountTest> all = repo.GetAll();

            Assert.AreEqual(4, all.Count);
            Assert.AreEqual("Object 2", all.SingleOrDefault(x => x.Number == 2).Name);
        }

        [TestMethod]
        public async Task Async_GetAll_CountTest_ShouldGetAll() {
            ICountTestRepository repo = GetCountTestRepository();
            List<CountTest> all = await repo.GetAllAsync();

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
        public async Task Async_GetCount_Count_ShouldGetCount() {
            ICountTestRepository repo = GetCountTestRepository();
            long count = await repo.GetCountAsync();
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

        [TestMethod]
        public async Task Async_GetBetweenDates_ShouldReturnValidResults() {
            ITestObjectRepository repo = GetTestObjectRepository();
            TestObject newObj1 = new TestObject {
                Title = "Async Date Range Test 1",
                Description = "Async Remove Test Description",
                StatusID = Status.Active,
                InputOn = new DateTime(1939, 1, 4, 12, 34, 20),
                InputByID = 1,
                IsActive = true
            };
            TestObject newObj2 = new TestObject {
                Title = "Async Date Range Test 2",
                Description = "Async Remove Test Description",
                StatusID = Status.Active,
                InputOn = new DateTime(1939, 1, 6, 8, 4, 56),
                InputByID = 1,
                IsActive = true
            };
            TestObject newObj3 = new TestObject {
                Title = "Async Date Range Test 3",
                Description = "Async Remove Test Description",
                StatusID = Status.Active,
                InputOn = new DateTime(1939, 1, 9, 18, 14, 0),
                InputByID = 1,
                IsActive = true
            };
            await repo.AddAsync(newObj1);
            await repo.AddAsync(newObj2);
            await repo.AddAsync(newObj3);

            List<TestObject> actual = await repo.GetByInputOnDateRangeAsync(new DateTime(1939, 1, 2), new DateTime(1939, 1, 7));

            Assert.AreEqual(2, actual.Count);
        }

        [TestMethod]
        public void OrderBy_ShouldOrderAscending() {
            ITestObjectRepository repo = GetTestObjectRepository();
            string description = "OrderBy Test";
            List<TestObject> expected = GetLimitTestObject(description, 5);
            repo.Add(expected);

            List<TestObject> actual = repo.GetByDescriptionWithInputOnOrderAsc(description);
            Assert.AreEqual(expected.Count, actual.Count);

            expected = expected.OrderBy(x => x.InputOn).ToList();

            for(int i= 0; i < actual.Count; i++) {
                Assert.AreEqual(actual[i].InputOn, expected[i].InputOn);
            }
        }

        [TestMethod]
        public void OrderBy_ShouldOrderDescending() {
            ITestObjectRepository repo = GetTestObjectRepository();
            string description = "OrderByDescending Test";
            List<TestObject> expected = GetLimitTestObject(description, 5);
            repo.Add(expected);

            List<TestObject> actual = repo.GetByDescriptionWithInputOnOrderDesc(description);
            Assert.AreEqual(expected.Count, actual.Count);

            expected = expected.OrderByDescending(x => x.InputOn).ToList();

            for (int i = 0; i < actual.Count; i++) {
                Assert.AreEqual(actual[i].InputOn, expected[i].InputOn);
            }
        }

        [TestMethod]
        public void OrderByWithoutWhereStatements_ShouldNotError() {
            ITestObjectRepository repo = GetTestObjectRepository();
            repo.GetAllSorted();
        }

        [TestMethod]
        public virtual void Limit_ShouldLimitResults() {
            ITestObjectRepository repo = GetTestObjectRepository();
            string description = "Limit Test";

            List<TestObject> expected = GetLimitTestObject(description, 10);
            expected.ForEach(x => repo.Add(x));

            List<TestObject> actual1 = repo.GetByDescriptionWithLimit(description, 4);
            Assert.AreEqual(4, actual1.Count);
        }

        [TestMethod]
        public virtual void LimitOffset_ShouldLimitAndOffsetResults() {
            ITestObjectRepository repo = GetTestObjectRepository();
            string description = "Limit and Offset Test";
            List<TestObject> expected = GetLimitTestObject(description, 10);
            expected.ForEach(x => repo.Add(x));

            List<TestObject> actual1 = repo.GetByDescriptionWithLimitAndOffset(description, 4, 0);
            Assert.AreEqual(4, actual1.Count);


            List<TestObject> actual2 = repo.GetByDescriptionWithLimitAndOffset(description, 4, 4);
            Assert.AreEqual(4, actual2.Count);

            foreach (TestObject testObject1 in actual1) {
                foreach (TestObject testObject2 in actual2) {
                    Assert.AreNotEqual(testObject1.ID, testObject2.ID);
                }
            }
        }

        private List<TestObject> GetLimitTestObject(string description, int itemCount) {
            List<TestObject> output = new List<TestObject>();
            Random r = new Random();
            for (int i = 0; i < itemCount; i++) {
                output.Add(new TestObject {
                    Title = description + " " + i,
                    Description = description,
                    StatusID = Status.Active,
                    InputOn = new DateTime(2021, r.Next(1, 12), r.Next(1, 28), r.Next(1, 23), r.Next(0, 59), r.Next(0, 59)),
                    InputByID = i,
                    IsActive = true
                });
            }
            return output;
        }
    }
}
