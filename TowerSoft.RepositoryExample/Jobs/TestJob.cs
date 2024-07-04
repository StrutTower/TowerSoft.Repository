using TowerSoft.RepositoryExample.Domain;
using TowerSoft.RepositoryExample.Repository;

namespace TowerSoft.RepositoryExample.Jobs {
    public class TestJob {
        private readonly UnitOfWork uow;

        public TestJob(UnitOfWork uow) {
            this.uow = uow;
        }

        public void StartJob() {
            TransactionRepository transactionRepo = uow.GetRepo<TransactionRepository>();

            var people = uow.GetRepo<PersonRepository>().GetAll();

            Person firstPerson = people.First();

            List<Transaction> transactionsBefore = transactionRepo.GetByPersonID(firstPerson.ID);

            transactionRepo.Add(new Transaction() {
                PersonID = firstPerson.ID,
                DateTime = DateTime.Now,
                Amount = 20
            });

            List<Transaction> transactionsAfter = transactionRepo.GetByPersonID(firstPerson.ID);

            Console.WriteLine();
        }
    }
}
