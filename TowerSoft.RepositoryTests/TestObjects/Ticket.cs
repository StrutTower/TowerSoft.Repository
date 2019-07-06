using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository.Attributes;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class Ticket {
        [Autonumber]
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int StatusID { get; set; }

        public DateTime InputOn { get; set; }

        public int InputByID { get; set; }

        public bool IsActive { get; set; }


        public virtual Lazy<Status> Status_Object { get; set; }
    }
}
