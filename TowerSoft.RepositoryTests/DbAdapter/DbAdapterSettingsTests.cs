using Dapper;
using System.Data.SQLite;
using TowerSoft.RepositoryTests.Databases.SQLite;
using TowerSoft.RepositoryTests.SQLite;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.DbAdapter {
    [TestClass]
    public class DbAdapterSettingsTests {
        private static UnitOfWork uow;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) {
            string dbPath = $"{Environment.CurrentDirectory}\\unittest.db";
            // Do not use a unit of work like this in a normal project. It must be disposed after use.
            uow = new UnitOfWork(dbPath);
            if (!File.Exists(dbPath))
                SQLiteConnection.CreateFile(dbPath);

            //uow.DbAdapter.DbConnection.Execute("DROP TABLE AdapterTestObject");
            uow.DbAdapter.DbConnection.Execute("CREATE TABLE IF NOT EXISTS AdapterTestObject (" +
                "ID INTEGER PRIMARY KEY AUTOINCREMENT," +
                "Title TEXT NOT NULL UNIQUE," +
                "Subtitle TEXT NULL," +
                "Description TEXT," +
                "StatusID INTEGER NOT NULL," +
                "InputOn TEXT NOT NULL," +
                "InputByID INTEGER NOT NULL," +
                "IsActive INTEGER NOT NULL);");
            uow.DbAdapter.DbConnection.Execute("DELETE FROM AdapterTestObject");
        }

        #region Add
        [TestMethod]
        public void Add_WithTrimSetting_ShouldTrimStrings() {
            uow.SetTrimSetting(true);

            AdapterTestObjectRepository repo = uow.GetRepo<AdapterTestObjectRepository>();

            AdapterTestObject obj = new() {
                Title = "AddTrimTrue",
                Description = "  Test   "
            };
            repo.Add(obj);

            AdapterTestObject result = repo.GetByTitle(obj.Title);

            Assert.AreEqual("Test", result.Description);
        }

        [TestMethod]
        public void Add_WithoutTrimSetting_ShouldNotTrimStrings() {
            uow.SetTrimSetting(false);

            AdapterTestObjectRepository repo = uow.GetRepo<AdapterTestObjectRepository>();

            AdapterTestObject obj = new() {
                Title = "AddTrimFalse",
                Description = "  Test   "
            };
            repo.Add(obj);

            AdapterTestObject result = repo.GetByTitle(obj.Title);

            Assert.AreEqual("  Test   ", result.Description);
        }

        [TestMethod]
        public void Add_WithNullSetting_ShouldNullEmptyStrings() {
            uow.SetNullStringSetting(true);

            AdapterTestObjectRepository repo = uow.GetRepo<AdapterTestObjectRepository>();

            AdapterTestObject obj = new() {
                Title = "AddNullStringTrue",
                Description = ""
            };
            repo.Add(obj);

            AdapterTestObject result = repo.GetByTitle(obj.Title);

            Assert.IsNull(result.Description);
        }

        [TestMethod]
        public void Add_WithoutNullSetting_ShouldNotNullEmptyStrings() {
            uow.SetNullStringSetting(false);

            AdapterTestObjectRepository repo = uow.GetRepo<AdapterTestObjectRepository>();

            AdapterTestObject obj = new() {
                Title = "AddNullStringFalse",
                Description = ""
            };
            repo.Add(obj);

            AdapterTestObject result = repo.GetByTitle(obj.Title);

            Assert.AreEqual("", result.Description);
        }
        #endregion

        #region AddList
        [TestMethod]
        public void AddList_WithTrimSetting_ShouldTrimStrings() {
            uow.SetTrimSetting(true);

            AdapterTestObjectRepository repo = uow.GetRepo<AdapterTestObjectRepository>();

            List<AdapterTestObject> items = [];
            items.Add(new() {
                Title = "AddListTrimTrue1",
                Subtitle = "  Test  ",
                Description = "AddListTrimTrue"
            });
            items.Add(new() {
                Title = "AddListTrimTrue2",
                Subtitle = "    Test",
                Description = "AddListTrimTrue"
            });
            items.Add(new() {
                Title = "AddListTrimTrue3",
                Subtitle = "Test    ",
                Description = "AddListTrimTrue"
            });
            repo.Add(items);

            List<AdapterTestObject> results = repo.GetByDescription("AddListTrimTrue");

            foreach (AdapterTestObject item in results) {
                Assert.AreEqual("Test", item.Subtitle);
            }
        }

        [TestMethod]
        public void AddList_WithoutTrimSetting_ShouldNotTrimStrings() {
            uow.SetTrimSetting(false);

            AdapterTestObjectRepository repo = uow.GetRepo<AdapterTestObjectRepository>();

            List<AdapterTestObject> items = [];
            items.Add(new() {
                Title = "AddListTrimFalse1",
                Subtitle = "  Test  ",
                Description = "AddListTrimFalse"
            });
            items.Add(new() {
                Title = "AddListTrimFalse2",
                Subtitle = "    Test",
                Description = "AddListTrimFalse"
            });
            items.Add(new() {
                Title = "AddListTrimFalse3",
                Subtitle = "Test    ",
                Description = "AddListTrimFalse"
            });
            repo.Add(items);

            List<AdapterTestObject> results = repo.GetByDescription("AddListTrimFalse");

            foreach (AdapterTestObject item in results) {
                Assert.AreNotEqual("Test", item.Subtitle);
            }
        }

        [TestMethod]
        public void AddList_WithNullSetting_ShouldNullEmptyStrings() {
            uow.SetNullStringSetting(true);

            AdapterTestObjectRepository repo = uow.GetRepo<AdapterTestObjectRepository>();

            List<AdapterTestObject> items = [];
            items.Add(new() {
                Title = "AddListNullTrue1",
                Subtitle = "",
                Description = "AddListNullTrue"
            });
            items.Add(new() {
                Title = "AddListNullTrue2",
                Subtitle = "    ",
                Description = "AddListNullTrue"
            });
            items.Add(new() {
                Title = "AddListNullTrue3",
                Subtitle = null,
                Description = "AddListNullTrue"
            });
            repo.Add(items);

            List<AdapterTestObject> results = repo.GetByDescription("AddListTrimFalse");

            foreach (AdapterTestObject item in results) {
                Assert.IsNull(item.Subtitle);
            }
        }

        [TestMethod]
        public void AddList_WithoutNullSetting_ShouldNotNullEmptyStrings() {
            uow.SetNullStringSetting(false);

            AdapterTestObjectRepository repo = uow.GetRepo<AdapterTestObjectRepository>();

            List<AdapterTestObject> items = [];
            items.Add(new() {
                Title = "AddListNullFalse1",
                Subtitle = "",
                Description = "AddListNullFalse"
            });
            items.Add(new() {
                Title = "AddListNullFalse2",
                Subtitle = "    ",
                Description = "AddListNullFalse"
            });
            items.Add(new() {
                Title = "AddListNullFalse3",
                Subtitle = null,
                Description = "AddListNullFalse"
            });
            repo.Add(items);

            List<AdapterTestObject> results = repo.GetByDescription("AddListTrimFalse");

            foreach (AdapterTestObject item in results) {
                if (item.Title.EndsWith("3"))
                    Assert.IsNull(item.Subtitle);
                else
                    Assert.IsNotNull(item.Subtitle);
            }
        }
        #endregion

        #region Update
        [TestMethod]
        public void Update_WithTrimSetting_ShouldTrimStrings() {
            uow.SetTrimSetting(true);

            AdapterTestObjectRepository repo = uow.GetRepo<AdapterTestObjectRepository>();

            AdapterTestObject obj = new() {
                Title = "UpdateTrimTrue",
                Description = "Test"
            };
            repo.Add(obj);

            obj.Description = "     Test    ";
            repo.Update(obj);

            AdapterTestObject result = repo.GetByTitle(obj.Title);

            Assert.AreEqual("Test", result.Description);
        }

        [TestMethod]
        public void Update_WithoutTrimSetting_ShouldNotTrimStrings() {
            uow.SetTrimSetting(false);

            AdapterTestObjectRepository repo = uow.GetRepo<AdapterTestObjectRepository>();

            AdapterTestObject obj = new() {
                Title = "UpdateTrimFalse",
                Description = "  Test   "
            };
            repo.Add(obj);

            obj.Description = "     Test    ";
            repo.Update(obj);

            AdapterTestObject result = repo.GetByTitle(obj.Title);

            Assert.AreEqual("     Test    ", result.Description);
        }

        [TestMethod]
        public void Update_WithNullSetting_ShouldNullEmptyStrings() {
            uow.SetNullStringSetting(true);

            AdapterTestObjectRepository repo = uow.GetRepo<AdapterTestObjectRepository>();

            AdapterTestObject obj = new() {
                Title = "UpdateNullStringTrue",
                Description = "Test"
            };
            repo.Add(obj);

            obj.Description = "";
            repo.Update(obj);

            AdapterTestObject result = repo.GetByTitle(obj.Title);

            Assert.IsNull(result.Description);
        }

        [TestMethod]
        public void Update_WithoutNullSetting_ShouldNotNullEmptyStrings() {
            uow.SetNullStringSetting(false);

            AdapterTestObjectRepository repo = uow.GetRepo<AdapterTestObjectRepository>();

            AdapterTestObject obj = new() {
                Title = "UpdateNullStringFalse",
                Description = "Test"
            };
            repo.Add(obj);

            obj.Description = "";
            repo.Update(obj);

            AdapterTestObject result = repo.GetByTitle(obj.Title);

            Assert.AreEqual("", result.Description);
        }
        #endregion
    }
}
