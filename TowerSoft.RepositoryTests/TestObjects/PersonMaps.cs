using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class PersonMaps : EntityMap<Person> {
        public PersonMaps() : base("person", AutoMappingOption.UseDefaultPropertyMaps) { }

        public override IEnumerable<IMap> GetMaps() {
            return new[] {
                MapPropery(x => x.ID).AsAutonumber().ToSameName(),
                MapPropery(x => x.FirstName).ToSameName(),
                MapPropery(x => x.LastName).To("Last_Name"),
                MapPropery(x => x.NotMappedProperty).NotMapped()
            };
        }
    }
}
