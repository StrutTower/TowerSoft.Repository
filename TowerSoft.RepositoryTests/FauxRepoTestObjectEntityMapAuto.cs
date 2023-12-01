using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests {
    public class FauxRepoTestObjectEntityMapAuto : EntityMap<FauxRepoTestObject> {
        public FauxRepoTestObjectEntityMapAuto() : base("", AutoMappingOption.UseNonObjectPropertyMaps) { }

        public override IEnumerable<IMap> GetMaps() {
            return new[] {
                MapProperty(x => x.ID).AsAutonumber().ToSameName(),
                MapProperty(x => x.StatusID).To("Status"),
                MapProperty(x => x.NotMappedProp).NotMapped()
            };
        }
    }
}
