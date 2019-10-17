using System;
using System.Collections.Generic;
using System.Linq;
using TestingLib.Domain;
using TestingLib.Repository;

namespace Testing {
    class Program {
        static void Main(string[] args) {
            using (UnitOfWork uow = UnitOfWork.CreateNew()) {
                uow.BeginTransaction();

                try {
                    PersonRepository personRepo = new PersonRepository(uow);

                    List<Person> people = personRepo.GetAll();

                    long test = personRepo.GetCount();

                    Person test1 = personRepo.GetByID(1);

                    uow.CommitTransaction();
                } catch {
                    uow.RollbackTransaction();
                    throw;
                }
                Console.WriteLine();
            }
        }
    }
}
