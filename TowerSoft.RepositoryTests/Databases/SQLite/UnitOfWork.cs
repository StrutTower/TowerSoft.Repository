using System;
using System.Collections.Generic;
using TowerSoft.Repository;
using TowerSoft.Repository.Interfaces;
using TowerSoft.Repository.SQLite;
using TowerSoft.RepositoryTests.Interfaces;

namespace TowerSoft.RepositoryTests.SQLite {
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork {
        public UnitOfWork(string path) {
            DbAdapter = new SQLiteDbAdapter($"Data Source={path};Version=3;");
        }

        /// <summary>
        /// Stores repositories that have been initialized
        /// </summary>
        private readonly Dictionary<Type, object> _repos = new();

        public TRepo GetRepo<TRepo>() where TRepo : IDbRepository {
            Type type = typeof(TRepo);

            if (!_repos.ContainsKey(type)) _repos[type] = Activator.CreateInstance(type, this);
            return (TRepo)_repos[type];
        }
    }
}
