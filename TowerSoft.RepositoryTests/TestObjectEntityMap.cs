using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests {
    public class TestObjectEntityMap : EntityMap<TestObject> {
        public TestObjectEntityMap() : base("") { }

        public override IEnumerable<IMap> GetMaps() {
            return new[] {
                MapProperty(x => x.ID).AsAutonumber().ToSameName(),
                MapProperty(x => x.Title).ToSameName(),
                MapProperty(x => x.Description).To("Description"),
                MapProperty(x => x.StatusID).ToSameName(),
                MapProperty(x => x.InputOn).ToSameName(),
                MapProperty(x => x.InputByID).ToSameName(),
                MapProperty(x => x.IsActive).ToSameName()
            };
        }
    }
}
