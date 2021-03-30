using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.Repository.Interfaces;
using TowerSoft.Repository.PostgreSql;
using TowerSoft.RepositoryTests.Interfaces;

namespace TowerSoft.RepositoryTests.PostgreSql {
    public class UnitOfWork : IUnitOfWork, IRepositoryUnitOfWork {
        public UnitOfWork() {
            string line = System.IO.File.ReadAllLines("appsecrets.txt").Single(x => x.StartsWith("postgresql =="));
            DbAdapter = new PostgreSqlDbAdapter(line.Split(" == ")[1]);
        }

        public IDbAdapter DbAdapter { get; }

        public void BeginTransaction() {
            DbAdapter.BeginTransaction();
        }

        public void CommitTransaction() {
            DbAdapter.CommitTransaction();
        }

        public void RollbackTransaction() {
            DbAdapter.RollbackTransaction();
        }

        public void Dispose() {
            DbAdapter.Dispose();
        }

        /// <summary>
        /// Stores repositories that have been initialized
        /// </summary>
        private readonly Dictionary<Type, object> _repos = new Dictionary<Type, object>();

        public TRepo GetRepo<TRepo>() where TRepo : IDbRepository {
            Type type = typeof(TRepo);

            if (!_repos.ContainsKey(type)) _repos[type] = Activator.CreateInstance(type, this);
            return (TRepo)_repos[type];
        }
    }
}
