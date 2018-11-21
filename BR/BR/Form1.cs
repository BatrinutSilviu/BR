using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace BR
{
    public partial class Form1 : Form
    {
        Graphics graphics;
        Pen pen;
        IList<NewPoint> polygon_coordinates;
        IList<NewPoint> all_coordinates;
        Point initial_point,final_point;
        ConvexHull new_Graphics;
        PointConverter point_Converter;
        const short drawings_Width = 5;
        Point current_Position;
        GraphicsPath path;

        public Form1()
        {
            InitializeComponent();
            GlobalInitialization();
        }

        public void GlobalInitialization()
        {
            polygon_coordinates = new List<NewPoint>();
            all_coordinates = new List<NewPoint>();
            comboBox1.SelectedIndex = 3;
            new_Graphics = new ConvexHull();
            point_Converter = new PointConverter();
            current_Position = new Point();
            path = new GraphicsPath();
        }
        //    GraphicsAndPenInitializing();
        //    float radius = float.Parse(textBox2.Text);
        //    graphics.DrawEllipse(pen, circle_coordinates.X - radius, circle_coordinates.Y - radius, 2 * radius, 2 * radius);

        public void GraphicsAndPenInitializing()
        {
            graphics = pictureBox1.CreateGraphics();
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Black":
                    {
                        pen = new Pen(Color.Black, drawings_Width);
                        break;
                    }
                case "Blue":
                    {
                        pen = new Pen(Color.Blue, drawings_Width);
                        break;
                    }
                case "Red":
                    {
                        pen = new Pen(Color.Red, drawings_Width);
                        break;
                    }
                case "Green":
                    {
                        pen = new Pen(Color.Green, drawings_Width);
                        break;
                    }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Brush brush = (Brush)Brushes.Black;
            GraphicsAndPenInitializing();
            MouseEventArgs me = (MouseEventArgs)e;
            graphics.FillRectangle(brush, me.Location.X, me.Location.Y, 2, 2);
            polygon_coordinates.Add(point_Converter.PointToNewPoint(me.Location));
            all_coordinates.Add(point_Converter.PointToNewPoint(me.Location));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GraphicsAndPenInitializing();
            IList<NewPoint> temporaryList = new_Graphics.MakeHull(polygon_coordinates);
            if (temporaryList.Count == 0 || temporaryList.Count == 1)
            {
                MessageBox.Show("Please select the polygon points on the canvas and then click the " + button2.Text + " button", "Not enough points selected");
                return;
            }
            else
            {
                //    for (short iterator = 0; iterator < temporaryList.Count; iterator++)
                //    {
                //        graphics.FillRectangle(brush, temporaryList[iterator].x, temporaryList[iterator].y, 2, 2);
                //    }
                graphics.DrawPolygon(pen, point_Converter.IListNewPointToPointArray(temporaryList));
            }
            polygon_coordinates.Clear();
            all_coordinates.Add(new NewPoint());
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Text = String.Format("X: {0}; Y: {1}", e.X, e.Y);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GraphicsAndPenInitializing();
            RobotAlgorithm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text)
                || string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("No point selected");
            }
            else
            {
                int[] TextBoxIntValues = new int[4];
                TextBoxIntValues = ParseTextBoxes();

                if(TextBoxIntValues == null)
                {
                    return;
                }

                if (TextBoxIntValues[0] < 0 || TextBoxIntValues[0]  > 977 
                    || TextBoxIntValues[2] < 0 || TextBoxIntValues[2] > 977
                    || TextBoxIntValues[1] < 0 || TextBoxIntValues[1] > 672
                    || TextBoxIntValues[3] < 0 || TextBoxIntValues[3] > 672)
                {
                    MessageBox.Show("Point not in range, Re-enter","Invalid points");
                }
                else
                {
                    initial_point.X = TextBoxIntValues[0];
                    initial_point.Y = TextBoxIntValues[1];

                    final_point.X = TextBoxIntValues[2];
                    final_point.Y = TextBoxIntValues[3];

                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    textBox5.Enabled = false;

                    graphics = pictureBox1.CreateGraphics();
                    Brush brush = (Brush)Brushes.Black;
                    graphics.FillRectangle(brush, initial_point.X, initial_point.Y, drawings_Width, drawings_Width);
                    graphics.FillRectangle(brush, final_point.X, final_point.Y, drawings_Width, drawings_Width);
                }
            }      
        }

        public int [] ParseTextBoxes()
        {
            int[] TextBoxesToIntArray = new int[4];
            short ArrayIndex = 0;

            if (!int.TryParse(textBox2.Text, out TextBoxesToIntArray[ArrayIndex++])
                || !int.TryParse(textBox3.Text, out TextBoxesToIntArray[ArrayIndex++])
                || !int.TryParse(textBox4.Text, out TextBoxesToIntArray[ArrayIndex++])
                || !int.TryParse(textBox5.Text, out TextBoxesToIntArray[ArrayIndex]))
            {
                MessageBox.Show("Re-enter values", "Incorrect format");
                return null;
            }
            else
            {
                return TextBoxesToIntArray;
            }
        }

        public void RobotAlgorithm()
        {
            //graphics.DrawLine(pen,initial_point,final_point);
            StepByStep();
        }

        public static bool IsInPolygon(Point[] poly, Point p)
        {
            Point p1, p2;
            bool inside = false;

            if (poly.Length < 3)
            {
                return inside;
            }

            var oldPoint = new Point(
                poly[poly.Length - 1].X, poly[poly.Length - 1].Y);

            for (int i = 0; i < poly.Length; i++)
            {
                var newPoint = new Point(poly[i].X, poly[i].Y);

                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                if ((newPoint.X < p.X) == (p.X <= oldPoint.X)
                    && (p.Y - (long)p1.Y) * (p2.X - p1.X)
                    < (p2.Y - (long)p1.Y) * (p.X - p1.X))
                {
                    inside = !inside;
                }

                oldPoint = newPoint;
            }

            return inside;
        }


        public void StepByStep()
        {
            IList<NewPoint> temporaryList = new List<NewPoint>();
            foreach(NewPoint point_iterator in all_coordinates)
            {
                if (point_iterator.x != 0)
                {
                    temporaryList.Add(point_iterator);
                }
            }
            IsInPolygon();    

        }


        //functie verificat cai blocate si returnat cale libera optima

    }
}
