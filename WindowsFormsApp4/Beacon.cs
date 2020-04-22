using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace WindowsFormsApp4
{
     abstract class Beacon : PictureBox
    {

     
        public Beacon (int x,int y)
        {
            this.BackColor = Color.White;
            this.Location = new Point(x, y);
            this.Size = new Size(33,35);


        }
    }
}
