using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TowerSoft.Repository;
using TowerSoft.Repository.Maps;
using TowerSoft.Repository.MySql;
using TowerSoft.RepositoryTests.MySql;

namespace TowerSoft.RepositoryTests {
    [TestClass]
    public class RepositoryTests {

        [TestMethod]
        public void PropertyDataAttributeMapping_PersonTestObject_ShouldReturnCorrectMaps() {
            PersonRepository personRepo;
            using (IUnitOfWork uow = new MySqlUnitOfWork("")) {
                personRepo = new PersonRepository(uow);
            }

            IMap expectedAutonumberMap = new AutonumberMap("ID");
            Assert.AreEqual(expectedAutonumberMap.PropertyName, personRepo.GetAutonumberMap().PropertyName);
            Assert.AreEqual(expectedAutonumberMap.ColumnName, personRepo.GetAutonumberMap().ColumnName);

            Assert.AreEqual(1, personRepo.GetPrimaryKeyMaps().Count());
            Assert.AreEqual(expectedAutonumberMap.PropertyName, personRepo.GetPrimaryKeyMaps().First().PropertyName);
            Assert.AreEqual(expectedAutonumberMap.ColumnName, personRepo.GetPrimaryKeyMaps().First().ColumnName);

            Assert.AreEqual(3, personRepo.GetAllMaps().Count());

            IMap firstNameMap = personRepo.GetAllMaps().SingleOrDefault(x => x.PropertyName == "FirstName");
            Assert.AreEqual("FirstName", firstNameMap.ColumnName);

            IMap lastNameMap = personRepo.GetAllMaps().SingleOrDefault(x => x.PropertyName == "LastName");
            Assert.AreEqual("Last_Name", lastNameMap.ColumnName);
        }

        [TestMethod]
        public void EntityMapMapping_PersonTestObject_ShouldReturnCorrectMaps() {
            PersonRepository personRepo;
            using (IUnitOfWork uow = new MySqlUnitOfWork("")) {
                personRepo = new PersonRepository("");
            }

            AutonumberMap expectedAutonumberMap = new AutonumberMap("ID");
            Assert.AreEqual(expectedAutonumberMap.PropertyName, personRepo.GetAutonumberMap().PropertyName);
            Assert.AreEqual(expectedAutonumberMap.ColumnName, personRepo.GetAutonumberMap().ColumnName);

            Assert.AreEqual(1, personRepo.GetPrimaryKeyMaps().Count());
            Assert.AreEqual(expectedAutonumberMap.PropertyName, personRepo.GetPrimaryKeyMaps().First().PropertyName);
            Assert.AreEqual(expectedAutonumberMap.ColumnName, personRepo.GetPrimaryKeyMaps().First().ColumnName);

            Assert.AreEqual(3, personRepo.GetAllMaps().Count());

            IMap firstNameMap = personRepo.GetAllMaps().SingleOrDefault(x => x.PropertyName == "FirstName");
            Assert.AreEqual("FirstName", firstNameMap.ColumnName);

            IMap lastNameMap = personRepo.GetAllMaps().SingleOrDefault(x => x.PropertyName == "LastName");
            Assert.AreEqual("Last_Name", lastNameMap.ColumnName);
        }
    }
}
