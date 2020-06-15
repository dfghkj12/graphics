using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImTools;

namespace Проект
{
    public partial class Form1 : Form
    {
        class Point
        {
            public double X;
            public double Y;
            public double Z;
            public double H;

            static public Point Parse(string str)
            {
                Point point = new Point();

                string[] st = str.Split(' ');

                point.X = Double.Parse(st[0]);
                point.Y = Double.Parse(st[1]);
                point.Z = Double.Parse(st[2]);
                point.H = Double.Parse(st[3]);

                return point;
            }
        }

        class Line
        {
            public int begin;
            public int end;

            static public Line Parse(string str)
            {
                Line line = new Line();

                string[] st = str.Split(' ');

                line.begin = Convert.ToInt32(st[0]);
                line.end = Convert.ToInt32(st[1]);

                return line;
            }
        }
        class Ploskost
        {
             public int a;
             public int b;
             public int c;
            static public Ploskost Parse(string str)
            {
                Ploskost line = new Ploskost();
                string[] a = str.Split(' ');
                line.a = int.Parse(a[0]);
                line.b = int.Parse(a[1]);
                line.c = int.Parse(a[2]);
                return line;
            }

 
        } 

        List<Point> tochki = new List<Point>();
        List<Line> otrzki = new List<Line>();
        List<Ploskost> ploskost = new List<Ploskost>();

        Graphics g;
        private int i;
        private double newY;
        private double newZ;
    
        private double newX;

        void ReadFile()
        {
            StreamReader sr = new StreamReader("C://Users//valus//source//repos//WindowsFormsApp9//123.txt");

            var str = sr.ReadLine();
            while (true)
            {

                if (sr.EndOfStream) break;
                if (str == "*") break;
                tochki.Add(Point.Parse(str));
                str = sr.ReadLine();
            }

            while (true)
            {

                if (sr.EndOfStream) break;
                if (str == "*") break;
                str = sr.ReadLine();
                otrzki.Add(Line.Parse(str));

            }
            while (true)
            {

                if (sr.EndOfStream) break;
                str = sr.ReadLine();
                ploskost.Add(Ploskost.Parse(str));
            }
        }


        void Vpisat()
        {
            double maxX = tochki[0].X;
            double minX = tochki[0].X;
            double maxY = tochki[0].Y;
            double minY = tochki[0].Y;

            for (int i = 0; i < tochki.Count; i++)
            {
                if (tochki[i].X > maxX) maxX = tochki[i].X;
                if (tochki[i].Y > maxY) maxY = tochki[i].Y;

                if (tochki[i].X < minX) minX = tochki[i].X;
                if (tochki[i].Y < minY) minY = tochki[i].Y;
            }

            double k;

            if (pictureBox1.Height / (maxY - minY) > pictureBox1.Width / (maxX - minX))
                k = pictureBox1.Width / (maxX - minX);
            else k = pictureBox1.Height / (maxY - minY);


            for (int i = 0; i < tochki.Count; i++)
            {
                tochki[i].X -= minX;
                tochki[i].Y -= minY;

                tochki[i].X *= k;
                tochki[i].Y *= k;
                tochki[i].Z *= k;
            }
        }                                                                                                           
       

        void Narisovat()
        {
            g.Clear(Color.White);
            if (checkBox1.Checked)
            {
                for (int i = 0; i < otrzki.Count; i++)
                {
                    double Ax = tochki[ploskost[i].b - 1].X - tochki[ploskost[i].a - 1].X;
                    double Ay = tochki[ploskost[i].b - 1].Y - tochki[ploskost[i].a - 1].Y;
                    double Az = tochki[ploskost[i].b - 1].Z - tochki[ploskost[i].a - 1].Z;
                    double Bx = tochki[ploskost[i].c - 1].X - tochki[ploskost[i].a - 1].X;
                    double By = tochki[ploskost[i].c - 1].Y - tochki[ploskost[i].a - 1].Y;
                    double Bz = tochki[ploskost[i].c - 1].Z - tochki[ploskost[i].a - 1].Z;
                    double Normalx = Ay * Bz - By * Az;
                    double Normaly = -(Ax * Bz - Bx * Az);
                    double Normalz = Ax * By - Bx * Ay;
                    double prois = 0;
                    for (int j = 0; j < tochki.Count; j++)
                    {
                        double tx = tochki[j].X - tochki[ploskost[i].a - 1].X;
                        double ty = tochki[j].Y - tochki[ploskost[i].a - 1].Y;
                        double tz = tochki[j].Z - tochki[ploskost[i].a - 1].Z;
                        prois = Normalx * tx + Normaly * ty + Normalz * tz;
                        if (prois > 1 || prois < -1) break;
                    }
                    if (prois > 0)
                    {
                        Normalx *= -1;
                        Normaly = -Normaly;
                        Normalz = -Normalz;
                    }
                    if (Normalz > 0)
                    {
                        for (int k = 0; k < otrzki.Count; k++)
                        {
                            if ((ploskost[i].a == otrzki[k].begin & ploskost[i].b == otrzki[k].end) ||
                                (ploskost[i].a == otrzki[k].end & ploskost[i].b == otrzki[k].begin))
                            {
                                g.DrawLine(new Pen(Color.Red, 2),
                                      (float)tochki[otrzki[k].begin - 1].X, (float)tochki[otrzki[k].begin - 1].Y,
                                      (float)tochki[otrzki[k].end - 1].X, (float)tochki[otrzki[k].end - 1].Y);
                            }
                            if ((ploskost[i].a == otrzki[k].begin & ploskost[i].c == otrzki[k].end) ||
                                (ploskost[i].a == otrzki[k].end & ploskost[i].c == otrzki[k].begin))
                            {
                                g.DrawLine(new Pen(Color.Orange, 2),
                                      (float)tochki[otrzki[k].begin - 1].X, (float)tochki[otrzki[k].begin - 1].Y,
                                      (float)tochki[otrzki[k].end - 1].X, (float)tochki[otrzki[k].end - 1].Y);
                            }
                            if ((ploskost[i].c == otrzki[k].begin & ploskost[i].b == otrzki[k].end) ||
                                (ploskost[i].c == otrzki[k].end & ploskost[i].b == otrzki[k].begin))
                            {
                                g.DrawLine(new Pen(Color.Blue, 2),
                                      (float)tochki[otrzki[k].begin - 1].X, (float)tochki[otrzki[k].begin - 1].Y,
                                      (float)tochki[otrzki[k].end - 1].X, (float)tochki[otrzki[k].end - 1].Y);
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < otrzki.Count; i++)
                {
                    g.DrawLine(new Pen(Color.Black, 2),
                        (float)tochki[otrzki[i].begin - 1].X, (float)tochki[otrzki[i].begin - 1].Y,
                        (float)tochki[otrzki[i].end - 1].X, (float)tochki[otrzki[i].end - 1].Y);
                }

                
            }
            pictureBox.Invalidate();
        }
        
    

    void mashtab(double x, double y, double z)
    {
        for (int i = 0; i < tochki.Count; i++)
            tochki[i].X = tochki[i].X * x;
        tochki[i].Y = tochki[i].Y * y;
        tochki[i].Z = tochki[i].Z * z;

    }
    void perenos(double x, double y, double z)
    {
        for (int i = 0; i < tochki.Count; i++)
            tochki[i].X = tochki[i].X + x;
        tochki[i].Y = tochki[i].Y + y;
        tochki[i].Z = tochki[i].Z + z;
    }
    void povorot(double A, double B, double C)
    {
        for (int i = 0; i < tochki.Count; i++)
            newY = (tochki[i].Y * Math.Cos(A * Math.PI / 180)) - tochki[i].Z * Math.Sin(A * Math.PI / 180);
        newZ = tochki[i].Y * Math.Cos(A * Math.PI / 180) + tochki[i].Z * Math.Sin(A * Math.PI / 180);
        tochki[i].Y = newY;
        tochki[i].Z = newZ;

        newX = (tochki[i].X * Math.Cos(B * Math.PI / 180)) + tochki[i].Z * Math.Sin(B * Math.PI / 180);
        newZ = tochki[i].Z * Math.Cos(B * Math.PI / 180) - tochki[i].X * Math.Sin(B * Math.PI / 180);

        tochki[i].X = newX;
        tochki[i].Z = newZ;


        newX = (tochki[i].X * Math.Cos(B * Math.PI / 180)) + tochki[i].Y * Math.Sin(B * Math.PI / 180);
        newY = tochki[i].Y * Math.Cos(B * Math.PI / 180) + tochki[i].X * Math.Sin(B * Math.PI / 180);

        tochki[i].X = newX;
        tochki[i].Y = newY;
    }
    void sdvig(double xy, double xz, double yx, double yz, double zx, double zy)
    {
        for (int i = 0; i < tochki.Count; i++)
            tochki[i].X = tochki[i].X + xy * tochki[i].Y;
        tochki[i].X = tochki[i].X + xz * tochki[i].Z;

        tochki[i].Y = tochki[i].X * yx + tochki[i].Y;
        tochki[i].Y = tochki[i].Y + yz * tochki[i].Z;

        tochki[i].Z = tochki[i].X * zx + tochki[i].Z;
        tochki[i].Z = tochki[i].Z + zy * tochki[i].Z;


    }
    void OPP(double fx, double fy, double fz)
    {
        for (int i = 0; i < tochki.Count; i++)
            tochki[i].H = tochki[i].H + tochki[i].X * 1 / fx;
        tochki[i].H = tochki[i].H + tochki[i].Y * 1 / fy;
        tochki[i].H = tochki[i].H + tochki[i].X * 1 / fz;
    }

    public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);

            ReadFile();

            Vpisat();

            Narisovat();
        }

    private void button1_Click(object sender, EventArgs e)
    {
        mashtab(double.Parse(textBox2.Text), double.Parse(textBox3.Text), double.Parse(textBox4.Text));
        Narisovat();
        perenos(double.Parse(textBox1.Text), double.Parse(textBox5.Text), double.Parse(textBox6.Text));
        povorot(double.Parse(textBox7.Text), double.Parse(textBox8.Text), double.Parse(textBox9.Text));
        sdvig(double.Parse(textBox10.Text), double.Parse(textBox11.Text), double.Parse(textBox12.Text),
            double.Parse(textBox13.Text), double.Parse(textBox14.Text), double.Parse(textBox15.Text));
        OPP(double.Parse(textBox16.Text), double.Parse(textBox17.Text), double.Parse(textBox18.Text));

    }



    

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

    internal struct NewStruct
    {
        public object Item1;
        public object Item2;

        public NewStruct(object item1, object item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public override bool Equals(object obj)
        {
            return obj is NewStruct other &&
                   EqualityComparer<object>.Default.Equals(Item1, other.Item1) &&
                   EqualityComparer<object>.Default.Equals(Item2, other.Item2);
        }

        public override int GetHashCode()
        {
            int hashCode = -1030903623;
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Item1);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Item2);
            return hashCode;
        }

        public void Deconstruct(out object item1, out object item2)
        {
            item1 = Item1;
            item2 = Item2;
        }

        public static implicit operator (object, object)(NewStruct value)
        {
            return (value.Item1, value.Item2);
        }

        public static implicit operator NewStruct((object, object) value)
        {
            return new NewStruct(value.Item1, value.Item2);
        }
    }
}
}


