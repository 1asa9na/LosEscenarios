using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife {
    internal class Cell {
        static Random rand = new Random();
        public bool state { get; set; }
        public bool nextGen { get; set; }
        public Cell(int freq) { state = rand.Next() % freq == 0; }

        public void Update() { state = nextGen; }
    }
}
