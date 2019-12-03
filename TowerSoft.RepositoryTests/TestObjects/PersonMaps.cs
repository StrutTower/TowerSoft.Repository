using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class PersonMaps : EntityMap<Person> {
        public PersonMaps() : base("person", AutoMappingOption.UseDefaultPropertyMaps) { }

        public override IEnumerable<IMap> GetMaps() {
            return new[] {
                MapProperty(x => x.ID).AsAutonumber().ToSameName(),
                MapProperty(x => x.FirstName).ToSameName(),
                MapProperty(x => x.LastName).To("Last_Name"),
                MapProperty(x => x.NotMappedProperty).NotMapped()
            };
        }
    }
}
