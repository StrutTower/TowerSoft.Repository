using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.Repository.PostgreSql;
using TowerSoft.Repository.Interfaces;

namespace TowerSoft.RepositoryTests.PostgreSql {
    public class UnitOfWork : IRepositoryUnitOfWork {
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

        public TRepo GetRepo<TRepo>() {
            Type type = typeof(TRepo);

            if (!IsAssignableFromGeneric(type, typeof(DbRepository<>))) {
                throw new Exception($"The type {type.Name} does not extend TowerSoft.Repository.Repository<T> and cannot be loaded by this method.");
            }

            if (!_repos.ContainsKey(type)) _repos[type] = Activator.CreateInstance(type, this);
            return (TRepo)_repos[type];
        }

        private static bool IsAssignableFromGeneric(Type extendType, Type baseType) {
            while (!baseType.IsAssignableFrom(extendType)) {
                if (extendType.Equals(typeof(object))) {
                    return false;
                }
                if (extendType.IsGenericType && !extendType.IsGenericTypeDefinition) {
                    extendType = extendType.GetGenericTypeDefinition();
                } else {
                    extendType = extendType.BaseType;
                }
            }
            return true;
        }
    }
}
