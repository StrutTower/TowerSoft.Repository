using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.Repository.MySql;

namespace Testing {
    public class UnitOfWorkFactory {
        public string ConnectionString { get; }

        public IUnitOfWork UnitOfWork { get; set; }

        public UnitOfWorkFactory() {
            ConnectionString = "server=towerserver;uid=unittests;password=$1letmein;database=unittests";
            UnitOfWork = new MySqlUnitOfWork(ConnectionString);
        }
    }
}
