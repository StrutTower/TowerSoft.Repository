using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;

namespace Testing {
    public class TicketMapBasic : EntityMap<Ticket> {
        public TicketMapBasic() : base("ticket", AutoMappingOption.UseDefaultPropertyMaps) { }

        public override IEnumerable<IMap> GetMaps() {
            return new[] {
                MapPropery(x => x.ID).AsAutonumber().ToSameName()
            };
        }
    }
}
