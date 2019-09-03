using System;
using System.Linq;
using TowerSoft.Repository;

namespace Testing {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");

            using (IUnitOfWork uow = new UnitOfWorkFactory().UnitOfWork) {

                PersonRepository personRepo = new PersonRepository(uow);
                var test = personRepo.GetCount();
                var test1 = personRepo.GetByID(1);

                Console.WriteLine();
            }
        }
    }
}
