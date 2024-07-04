using Dapper;
using Microsoft.Extensions.Configuration;
using TowerSoft.Repository;
using TowerSoft.Repository.Interfaces;
using TowerSoft.Repository.SQLite;
using TowerSoft.RepositoryExample.Domain;

namespace TowerSoft.RepositoryExample.Repository {
    public class UnitOfWork : UnitOfWorkBase {
        public UnitOfWork(IConfiguration config) {
            DbAdapter = new SQLiteDbAdapter(config.GetConnectionString("default"));
            //              ^ Change the DbAdapter based on the type of database you are connecting to


            // Setup methods
            CreateDatabase();
            SeedDatabase();
        }


        // Stores the repositories that have already been initialized
        private readonly Dictionary<Type, object> repos = new Dictionary<Type, object>();

        public TRepo GetRepo<TRepo>() where TRepo : IDbRepository {
            Type type = typeof(TRepo);

            if (!repos.ContainsKey(type)) repos[type] = Activator.CreateInstance(type, this);
            return (TRepo)repos[type];
        }


        // Setup methods - This shouldn't be used normally
        private void CreateDatabase() {
            // Create the test tables
            DbAdapter.DbConnection.Execute("CREATE TABLE IF NOT EXISTS Person (" +
                "ID INTEGER PRIMARY KEY AUTOINCREMENT," +
                "DisplayName TEXT NOT NULL," +
                "CreatedOn TEXT," +
                "Birthdate TEXT," +
                "IsActive INTEGER NOT NULL);");
            DbAdapter.DbConnection.Execute("DELETE FROM Person");
            DbAdapter.DbConnection.Execute("CREATE TABLE IF NOT EXISTS TransactionEntry (" +
                "ID INTEGER PRIMARY KEY AUTOINCREMENT," +
                "PersonID INTEGER NOT NULL," +
                "DateTime TEXT," +
                "Amount INTEGER NOT NULL);");
            DbAdapter.DbConnection.Execute("DELETE FROM TransactionEntry");
        }

        private void SeedDatabase() {
            PersonRepository personRepo = GetRepo<PersonRepository>();
            TransactionRepository transactionRepo = GetRepo<TransactionRepository>();

            List<Transaction> transactions = [];

            Person person1 = new() {
                DisplayName = "John Smith",
                CreatedOn = DateTime.Now.AddDays(-50),
                DateOfBirth = new DateTime(1970, 5, 23),
                IsActive = true,
            };
            personRepo.Add(person1);

            transactions.Add(new() {
                PersonID = person1.ID,
                DateTime = DateTime.Now.AddDays(-3),
                Amount = 9.99
            });
            transactions.Add(new() {
                PersonID = person1.ID,
                DateTime = DateTime.Now.AddDays(-2),
                Amount = 69.99
            });


            Person person2 = new() {
                DisplayName = "Jane Smith",
                CreatedOn = DateTime.Now.AddDays(-51),
                DateOfBirth = new DateTime(1982, 8, 2),
                IsActive = true,
            };
            personRepo.Add(person2);

            transactions.Add(new() {
                PersonID = person2.ID,
                DateTime = DateTime.Now.AddDays(-4),
                Amount = 9.99
            });
            transactions.Add(new() {
                PersonID = person2.ID,
                DateTime = DateTime.Now.AddDays(-3),
                Amount = 69.99
            });
            transactions.Add(new() {
                PersonID = person2.ID,
                DateTime = DateTime.Now.AddDays(-1),
                Amount = 17.99
            });


            Person person3 = new() {
                DisplayName = "Sarah Nguyen",
                CreatedOn = DateTime.Now.AddDays(-54),
                DateOfBirth = new DateTime(1981, 1, 21),
                IsActive = true,
            };
            personRepo.Add(person3);

            transactions.Add(new() {
                PersonID = person3.ID,
                DateTime = DateTime.Now.AddDays(-4),
                Amount = 8.99
            });
            transactions.Add(new() {
                PersonID = person3.ID,
                DateTime = DateTime.Now.AddDays(-3),
                Amount = 49.99
            });
            transactions.Add(new() {
                PersonID = person3.ID,
                DateTime = DateTime.Now.AddDays(-1),
                Amount = 12.99
            });

            transactionRepo.Add(transactions);
        }
    }
}
