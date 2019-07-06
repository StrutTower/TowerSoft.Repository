using System;
using System.Linq;
using TowerSoft.Repository;

namespace Testing {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");

            using (IUnitOfWork uow = new UnitOfWorkFactory().UnitOfWork) {

                PersonRepository personRepo = new PersonRepository(uow);
                personRepo.GetCount();

                TicketRepository repo = new TicketRepository(uow);
                var test = repo.GetAll();

                Ticket test2 = test.First();
                var test3 = test2.Status_Object;

                var test4 = repo.GetByID(1);

                var test5 = repo.Test();

                Console.WriteLine();
            }
        }
    }
}
