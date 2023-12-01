using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests {
    public class FauxRepoTestObjectEntityMap : EntityMap<FauxRepoTestObject> {
        public FauxRepoTestObjectEntityMap() : base("") { }

        public override IEnumerable<IMap> GetMaps() {
            return new[] {
                MapProperty(x => x.ID).AsAutonumber().ToSameName(),
                MapProperty(x => x.Title).ToSameName(),
                MapProperty(x => x.Description).ToSameName(),
                MapProperty(x => x.StatusID).To("Status"),
                MapProperty(x => x.InputOn).ToSameName(),
                MapProperty(x => x.InputByID).ToSameName(),
                MapProperty(x => x.IsActive).ToSameName(),
                MapProperty(x => x.NotMappedProp).NotMapped()
            };
        }
    }
}
