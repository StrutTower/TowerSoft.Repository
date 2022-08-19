using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TowerSoft.Repository;
using TowerSoft.Repository.Maps;

namespace TowerSoft.RepositoryTests {
    [TestClass]
    public class RepositoryTests {
        [TestMethod]
        public void OverrideTableName_FauxRepository_ShouldReturnOverriddenTableName() {
            string tableName = "overriddenTableName";
            FauxRepository repo = new FauxRepository(tableName);
            Assert.AreEqual(tableName, repo.GetTableName());
        }

        [TestMethod]
        public void PropertyDataAttributeMapping_TestObject_ShouldReturnCorrectMaps() {
            FauxRepository repo = new FauxRepository();
            ValidateMaps(repo);
        }

        [TestMethod]
        public void EntityMapMapping_TestObject_ShouldReturnCorrectMaps() {
            FauxRepository repo = new FauxRepository(new TestObjectEntityMap());
            ValidateMaps(repo);
        }

        [TestMethod]
        public void ExplicitMapping_TestObject_ShouldReturnCorrectMaps() {
            FauxRepository repo = new FauxRepository(true);
            ValidateMaps(repo);
        }

        [TestMethod]
        public void GetQueryBuilder_Get_ShouldReturnSelectStatement() {
            string expected = "SELECT " +
                "TestObject.Title Title," +
                "TestObject.Description Description," +
                "TestObject.StatusID StatusID," +
                "TestObject.InputOn InputOn," +
                "TestObject.InputByID InputByID," +
                "TestObject.IsActive IsActive," +
                "TestObject.ID ID " +
                "FROM TestObject ";
            string query = new FauxRepository().GetQueryBuilderSqlQuery();
            Assert.AreEqual(expected, query);
        }

        [TestMethod]
        public void GetQueryBuilder_GetWithCustomColumns_ShouldReturnSelectStatement() {
            string expected = "SELECT " +
                "TestObject.Title Title," +
                "TestObject.Description Description," +
                "TestObject.StatusID StatusID," +
                "TestObject.InputOn InputOn," +
                "TestObject.InputByID InputByID," +
                "TestObject.IsActive IsActive," +
                "TestObject.ID ID," +
                "CustomColumn," +
                "$INTERNAL(CustomColumn2) " +
                "FROM TestObject ";
            string query = new FauxRepository().GetQueryBuilderSqlQueryWithCustomColumns();
            Assert.AreEqual(expected, query);
        }

        private void ValidateMaps(FauxRepository repo) {
            IMap expectedAutonumberMap = new AutonumberMap("ID");
            Assert.AreEqual(expectedAutonumberMap.PropertyName, repo.GetAutonumberMap().PropertyName);
            Assert.AreEqual(expectedAutonumberMap.ColumnName, repo.GetAutonumberMap().ColumnName);

            Assert.AreEqual(1, repo.GetPrimaryKeyMaps().Count());
            Assert.AreEqual(expectedAutonumberMap.PropertyName, repo.GetPrimaryKeyMaps().First().PropertyName);
            Assert.AreEqual(expectedAutonumberMap.ColumnName, repo.GetPrimaryKeyMaps().First().ColumnName);

            Assert.AreEqual(7, repo.GetAllMaps().Count());

            IMap firstNameMap = repo.GetAllMaps().SingleOrDefault(x => x.PropertyName == "Title");
            Assert.AreEqual("Title", firstNameMap.ColumnName);

            IMap lastNameMap = repo.GetAllMaps().SingleOrDefault(x => x.PropertyName == "Description");
            Assert.AreEqual("Description", lastNameMap.ColumnName);

            IMap statusMap = repo.GetAllMaps().SingleOrDefault(x => x.PropertyName == "StatusID");
            Assert.AreEqual("StatusID", statusMap.ColumnName);

            IMap inputOnMap = repo.GetAllMaps().SingleOrDefault(x => x.PropertyName == "InputOn");
            Assert.AreEqual("InputOn", inputOnMap.ColumnName);

            IMap inputByIDMap = repo.GetAllMaps().SingleOrDefault(x => x.PropertyName == "InputByID");
            Assert.AreEqual("InputByID", inputByIDMap.ColumnName);

            IMap isActiveMap = repo.GetAllMaps().SingleOrDefault(x => x.PropertyName == "IsActive");
            Assert.AreEqual("IsActive", isActiveMap.ColumnName);
        }
    }
}
