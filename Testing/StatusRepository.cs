using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.Repository.MySql;

namespace Testing {
   public  class StatusRepository : Repository<Status> {
        public StatusRepository() 
            : base("server=towerserver;uid=unittests;password=$1letmein;database=unittests", new MySqlDbAdapter())
            { }

        public StatusRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Status GetByID (int id) {
            return GetSingleEntity(new WhereCondition("ID", id));
        }

        public Status GetByIDTest(int id) {
            return GetSingleEntity(WhereEqual(x => x.ID, id));
        }
    }
}
