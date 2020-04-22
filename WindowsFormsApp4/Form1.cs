using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Http;
using System.Windows.Input;
using WindowsFormsApp4.MyClasses;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private static int fixPixel = 13;
        Bitmap gra;
        int a = 0;
        int x, y, x1, y1,x2,y2,x3,y3;
        int i = 0, p = 0,q=0;
        string floor;
      //  int[] X = new int[100];
      //  int[] Y = new int[100];
      //  int[] X1 = new int[100];
      //  int[] Y1 = new int[100];
        Color[] colors = { Color.AliceBlue, Color.AntiqueWhite, Color.Aqua, Color.Aquamarine, Color.Azure, Color.Beige, Color.Bisque, Color.BlanchedAlmond, Color.Brown };
        Dictionary<string, Color> firefighterAndColor = new Dictionary<string, Color>();
        Graphics drawPan = null;

     //   Boolean p1 = false, p2 = false, p3 = false, p4 = false;

        private picturebox pic = new picturebox();

        private static readonly HttpClient client = new HttpClient();
        private DataSourceManager dataSourceManager;
        private void updateLocation()
        {
            Dictionary<string, Point> firefighters = dataSourceManager.firefighterLocation(floor);
            foreach(string firefighter in firefighters.Keys)
            {
                Console.WriteLine(firefighter);
                Point location = firefighters[firefighter];
                Brush brush = new SolidBrush(firefighterAndColor[firefighter]);
                drawPan.FillEllipse(brush, location.X - 10, location.Y + 20, 40, 40);
            }
        }
       /* private void redraw()
        {
            if (i == 0)
            {
                for(int z = 0; z <p; z++)
                    pictureBox1.Controls.Add(new BlackBeacon(X[z] - 17, Y[z] - 18));
            }
            else if (i == 1)
            {
                for (int z = 0; z < q; z++)
                    pictureBox1.Controls.Add(new BlackBeacon(X1[z] - 17, Y1[z] - 18));
            }
        }*/
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Controls.Clear();

            if (comboBox1.SelectedIndex == 0)
            {
                i = 0;
                gra = Properties.Resources.F4591;
                pictureBox1.Image = gra;
                Console.WriteLine("F4591");
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                i = 1;
                gra = Properties.Resources.F4;
                pictureBox1.Image = gra;
                Console.WriteLine("F4");
            }
            drawPan = Graphics.FromImage(gra);
            floor = comboBox1.SelectedItem.ToString();
            updatePictureBox(floor);
        }

        private void updatePictureBox(string floor)
        {
            int i = 0;
            Dictionary<string, Dictionary<string, Point>> locationOfBeacons = dataSourceManager.getLocationOfBeacons();
            var locationOfBeaconKeys = locationOfBeacons.Keys;
            //remove all BlackBeacon

            Console.WriteLine(pictureBox1.Controls.Count);
            while (pictureBox1.Controls.Count != 0)
            {
                foreach (Control control in pictureBox1.Controls)
                {
                    Console.WriteLine(i);
                    pictureBox1.Controls.Remove(control);
                    control.Dispose();
                    Console.WriteLine("rm blackbeacon");
                    i++;
                }
                Console.WriteLine(pictureBox1.Controls.Count);
            }
            Dictionary<string, Point> beacons = locationOfBeacons[floor];
            foreach (string mac in beacons.Keys)
            {
                Point point = beacons[mac];
                pictureBox1.Controls.Add(new BlackBeacon(point.X, point.Y));
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int fixed_X = e.X - fixPixel;
            int fixed_Y = e.Y - fixPixel;
            AddBeaconForm addBeaconForm = new AddBeaconForm(dataSourceManager);
            DialogResult result = addBeaconForm.ShowDialog();
            string returnValue = addBeaconForm.getBeaconMac;
            string floor = comboBox1.GetItemText(comboBox1.SelectedItem);
            if (returnValue != "" && returnValue != null && result != DialogResult.Cancel)
            {
                if (dataSourceManager.addBeacon(floor, addBeaconForm.getBeaconMac, new Point(fixed_X, fixed_Y)))
                {
                    pictureBox1.Controls.Add(new BlackBeacon(fixed_X, fixed_Y));
                }
                else
                {
                    MessageBox.Show("已有相同項目", "訊息");
                }
            }
            else
            {
                Console.WriteLine("cancel");
            }
        }

        

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
         //   p3 = checkBox3.Checked;
            textBox5.Visible = checkBox3.Checked;
            textBox6.Visible = checkBox3.Checked;
        }

        private async void button1_Click_1Async(object sender, EventArgs e)
        {
            var values = new Dictionary<string, string> { { "data", "Result" } };
            var content = new FormUrlEncodedContent(values);
            var DeleteDataBase = await client.PostAsync("http://163.18.53.144/F459/php/C%23_ServerBackend/C%23_DelResult.php", content);
            var responseString = await DeleteDataBase.Content.ReadAsStringAsync(); //回傳結果

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (pic.CanIdentify(e.X,e.Y))
            {
                pictureBox1.Cursor = Cursors.Hand; 
            }
            else
            {
                pictureBox1.Cursor = Cursors.Default;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
          //  p4 = checkBox4.Checked;
            textBox7.Visible = checkBox4.Checked;
            textBox8.Visible = checkBox4.Checked;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            dataSourceManager = new DataSourceManager();
            //  Console.WriteLine("Form1_load");
            //  int i = dataSourceManager.getFirefighters().Length;
            //  int j = i;
            //  Task.WaitAll(DataSourceManager.getFirefighterTask);
            //  foreach (string firefighter in dataSourceManager.getFirefighters())
            //  {
            //      Console.WriteLine("initialize firefighterAndColor  " + firefighter);
            //      firefighterAndColor.Add(firefighter, colors[i - j]);
            //      j--;
            //  }
            //
            //  Console.WriteLine("numberofColor: " + firefighterAndColor.Keys.Count);
            //  foreach (string tmp in firefighterAndColor.Keys)
            //  {
            //      Console.WriteLine("Color: " + tmp);
            //  }

             dataSourceManager.getFirefighterMac();
             dataSourceManager.getNewBeaconList();
        }


        public Form1()
        {
            InitializeComponent();
          //  dataSourceManager = new DataSourceManager();
          //  int i = dataSourceManager.getFirefighters().Length;
          //  int j = i;
          //  Task.WaitAll(DataSourceManager.getFirefighterTask);
          //  foreach (string firefighter in dataSourceManager.getFirefighters())
          //  {
          //      Console.WriteLine("initialize firefighterAndColor  " + firefighter);
          //      firefighterAndColor.Add(firefighter, colors[i - j]);
          //      j--;
          //  }
          //  
          //  Console.WriteLine("numberofColor: " + firefighterAndColor.Keys.Count);
          //  foreach(string tmp in firefighterAndColor.Keys)
          //  {
          //      Console.WriteLine("Color: " + tmp);
          //  }
            gra = Properties.Resources.F4591;
            
            textBox1.Text = "100";
            textBox2.Text = "350";
            textBox3.Text = "200";
            textBox4.Text = "350";
            textBox5.Text = "300";
            textBox6.Text = "200";
            textBox7.Text = "200";
            textBox8.Text = "200";
           
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            textBox7.Visible = false;
            textBox8.Visible = false;
            timer1.Enabled = true;
           

            comboBox1.Items.Add("f459");
            comboBox1.Items.Add("F4");
            comboBox1.SelectedItem = comboBox1.Items[0];

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            updateLocation();

            //原本我寫的timer 
            /*
            x = int.Parse(textBox1.Text); y = int.Parse(textBox2.Text);
            x1 = int.Parse(textBox3.Text); y1= int.Parse(textBox4.Text);
            x2 = int.Parse(textBox5.Text); y2 = int.Parse(textBox6.Text);
            x3 = int.Parse(textBox7.Text); y3 = int.Parse(textBox8.Text);
            if (i == 0)
            if (a < 0)
            {   
                Graphics g = Graphics.FromImage(gra);
                Brush bush = new SolidBrush(Color.Red);
                Brush bush2 = new SolidBrush(Color.Green);
                Brush bush3 = new SolidBrush(Color.Yellow );
                Brush bush4 = new SolidBrush(Color.Blue );
                    //畫空心矩形
                Pen pen = new Pen(Brushes.Red);
                Pen pen1 = new Pen(Brushes.Green);
                Pen pen2 = new Pen(Brushes.Yellow);
                Pen pen3 = new Pen(Brushes.Blue);

                    if (p1 == true)
                {
                        //g.DrawRectangle(pen, 0, 0, 80, 140);
                        g.FillEllipse(bush, x, y, 50, 50);
                }
                if (p2 == true)
                {
                        //g.DrawRectangle(pen1, 80, 140, 80, 130);
                        g.FillEllipse(bush2, x1, y1, 50, 50);
                }
                if (p3 == true)
                {
                        //g.DrawRectangle(pen2, 0, 140, 80, 130);
                        g.FillEllipse(bush3, x2, y2, 50, 50);
                }
                if (p4 == true)
                {
                        //g.DrawRectangle(pen3, 0, 270, 80, 130);
                        g.FillEllipse(bush4, x3, y3, 50, 50);
                }


                pictureBox1.Image = gra;

            }
            else
            {
                gra = Properties.Resources.F4591;
                pictureBox1.Image = gra;

            }
            if (i == 1)
            {
                if (a < 0)
                {
                    Graphics g = Graphics.FromImage(gra);
                    Brush bush = new SolidBrush(Color.Red);
                    Brush bush2 = new SolidBrush(Color.Green);
                    Brush bush3 = new SolidBrush(Color.Yellow);
                    Brush bush4 = new SolidBrush(Color.Blue);
                    if (p1 == true)
                    {
                        g.FillEllipse(bush, x, y, 50, 50);
                    }
                    if (p2 == true)
                    {
                        g.FillEllipse(bush2, x1, y1, 50, 50);
                    }
                    if (p3 == true)
                    {
                        g.FillEllipse(bush3, x2, y2, 50, 50);
                    }
                    if (p4 == true)
                    {
                        g.FillEllipse(bush4, x3, y3, 50, 50);
                    }


                    pictureBox1.Image = gra;

                }
                else
                {
                    gra = Properties.Resources.f4;
                    pictureBox1.Image = gra;

                }
            }

            a *= -1;
            */
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
          //  p1 = checkBox1.Checked;
            textBox1.Visible = checkBox1.Checked;
            textBox2.Visible = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
         //   p2 = checkBox2.Checked;
            textBox3.Visible = checkBox2.Checked;
            textBox4.Visible = checkBox2.Checked;
        }
        //下面沒用誤點
      
        private void comboBox1_MouseDown(object sender, MouseEventArgs e)
        {
       
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            /*
                Stream myStream;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";

                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        pictureBox1.Image.Save(saveFileDialog1.FileName);

                    }
                }
                */
        }
        private void pictureBox1_Mouse_Click(object sender, MouseEventArgs e)
        {  

        }
    

    }
}
