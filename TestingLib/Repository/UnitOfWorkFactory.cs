using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.Repository.MySql;

namespace TestingLib.Repository {
    public class UnitOfWorkFactory {
        public UnitOfWork UnitOfWork { get; set; }

        public UnitOfWorkFactory() {
            string connectionString = "server=towerserver;uid=unittests;password=$1letmein;database=unittests";
            UnitOfWork = new UnitOfWork(connectionString);
        }
    }
}
