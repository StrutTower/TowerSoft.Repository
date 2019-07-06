using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.Repository.Proxies;

namespace Testing {
    public class TicketRepository : LazyLoadRepository<Ticket> {
        public TicketRepository(IUnitOfWork uow) : base(uow/*, new TicketMap()*/) { }

        public Ticket GetByID(int id) {
            return GetSingleEntity(WhereEqual(x => x.ID, id));
        }

        public List<Ticket> Test() {
            QueryBuilder query = GetQueryBuilder();

            return GetEntities(query);
        }
    }
}
