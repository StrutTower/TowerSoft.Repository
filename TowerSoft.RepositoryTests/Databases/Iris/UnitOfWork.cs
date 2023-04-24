//using System;
//using System.Collections.Generic;
//using System.Linq;
//using TowerSoft.Repository;
//using TowerSoft.Repository.Iris;
//using TowerSoft.Repository.Interfaces;
//using TowerSoft.RepositoryTests.Interfaces;

//namespace TowerSoft.RepositoryTests.Databases.Iris {
//    public class UnitOfWork : IRepositoryUnitOfWork, IUnitOfWork {
//        public UnitOfWork() {
//            string line = System.IO.File.ReadAllLines("appsecrets.txt").Single(x => x.StartsWith("iris =="));
//            DbAdapter = new IrisDbAdapter(line.Split(" == ")[1]);
//        }

//        public IDbAdapter DbAdapter { get; }
//        public void BeginTransaction() => DbAdapter.BeginTransaction();
//        public void CommitTransaction() => DbAdapter.CommitTransaction();
//        public void RollbackTransaction() => DbAdapter.RollbackTransaction();
//        public void Dispose() => DbAdapter.Dispose();

//        /// <summary>
//        /// Stores repositories that have been initialized
//        /// </summary>
//        private readonly Dictionary<Type, object> _repos = new Dictionary<Type, object>();

//        public TRepo GetRepo<TRepo>() where TRepo : IDbRepository {
//            Type type = typeof(TRepo);

//            if (!_repos.ContainsKey(type)) _repos[type] = Activator.CreateInstance(type, this);
//            return (TRepo)_repos[type];
//        }
//    }
//}
