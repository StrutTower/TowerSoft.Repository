using TowerSoft.Repository.Utilities;

namespace TowerSoft.RepositoryTests {
    [TestClass]
    public class CacheUtilitiesTests {
        [TestMethod]
        public void LogicalDate_ShouldConvertToLogicalDate() {
            decimal expectedLogical = 2950425;
            DateTime expectedDateTime = new DateTime(1995, 4, 25);

            decimal actual = InternalCacheUtilities.DateTimeToLogicalDate(expectedDateTime);

            Assert.AreEqual(expectedLogical, actual);
        }

        [TestMethod]
        public void HorologDate_DayOne_ShouldConvertToHorologDate() {
            int expectedHorologDate = 1;
            DateTime expectedDateTime = new DateTime(1841, 1, 1);

            int actual = InternalCacheUtilities.DateTimeToHorologDate(expectedDateTime);

            Assert.AreEqual(expectedHorologDate, actual);
        }

        [TestMethod]
        public void HorologDate_ShouldConvertToHorologDate() {
            int expectedHorologDate = 60023;
            DateTime expectedDateTime = new DateTime(2005, 5, 3);

            int actual = InternalCacheUtilities.DateTimeToHorologDate(expectedDateTime);

            Assert.AreEqual(expectedHorologDate, actual);
        }
    }
}
