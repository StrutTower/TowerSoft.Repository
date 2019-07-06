using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository.Attributes;

namespace Testing {
    public class Status {
        [Autonumber]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
