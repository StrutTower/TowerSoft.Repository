using TowerSoft.Repository.Interfaces;
using TowerSoft.Repository.MySql;
using TowerSoft.RepositoryTests.Interfaces;

namespace TowerSoft.RepositoryTests.MySql {
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork {
        public UnitOfWork() {
            string line = File.ReadAllLines("appsecrets.txt").Single(x => x.StartsWith("mysql =="));
            DbAdapter = new MySqlDbAdapter(line.Split(" == ")[1]);
        }

        /// <summary>
        /// Stores repositories that have been initialized
        /// </summary>
        private readonly Dictionary<Type, object> _repos = [];

        public TRepo GetRepo<TRepo>() where TRepo : IDbRepository {
            Type type = typeof(TRepo);

            if (!_repos.ContainsKey(type)) _repos[type] = Activator.CreateInstance(type, this);
            return (TRepo)_repos[type];
        }
    }
}
