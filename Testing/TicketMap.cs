using System.Collections.Generic;
using TowerSoft.Repository;
using TowerSoft.Repository.Proxies;

namespace Testing {
    public class TicketMap : LazyLoadEntityMap<Ticket> {
        public TicketMap() : base("ticket", AutoMappingOption.UseDefaultPropertyMaps) { }

        public override IEnumerable<IMap> GetMaps() {
            return new[] {
                MapPropery(x => x.ID).AsAutonumber().ToSameName()
            };
        }

        public override IEnumerable<LazyLoadMap> GetLazyLoadMaps() {
            return new[] {
                MapLazyLoadProperty(x => x.Status_Object).LoadFrom(typeof(StatusRepository), "GetByID").WithParameter(x => x.StatusID)
            };
        }
    }
}
