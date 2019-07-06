using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;

namespace Testing {
    public class PersonMap : EntityMap<Person> {
        public PersonMap() : base("Person", AutoMappingOption.UseDefaultPropertyMaps) { }

        public override IEnumerable<IMap> GetMaps() {
            
            return new[] {
                MapPropery(x => x.ID).AsAutonumber().ToSameName(),
                MapPropery(x => x.FirstName).To("FirstName")
            };
        }
    }
}
