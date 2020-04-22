using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    class BlackBeacon :Beacon
    {
        public BlackBeacon(int x,int y) : base(x, y)
        {
            this.Image = Properties.Resources.beacon1;


        }

    }
}
