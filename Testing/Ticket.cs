using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository.Attributes;

namespace Testing {
    public class Ticket {
        [Autonumber]
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int StatusID { get; set; }

        public DateTime InputOn { get; set; }

        public int InputByID { get; set; }

        public bool IsActive { get; set; }

        [LazyLoad(typeof(StatusRepository), "GetByID", "StatusID")]
        public virtual Status Status_Object { get; set; }
    }
}
