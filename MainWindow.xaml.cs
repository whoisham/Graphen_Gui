using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Graphen_gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Matrix matrixen;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void addMatrixButtons(int[,] mat)
        {
            List<List<Button>> lsts = new List<List<Button>>();
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                lsts.Add(new List<Button>());

                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    Button butt = new Button
                    {
                        Width = 30,
                        Height = 30
                    };
                    butt.Content = mat[i, j];
                    int[] index = new int[2];
                    index[0] = i;
                    index[1] = j;
                    butt.Tag = index;
                    butt.Click += new RoutedEventHandler(button_Click);
                    lsts[i].Add(butt);
                }
            }
            Adjazenz.ItemsSource = lsts;
        }

        private void addWegMatrixFelder(int[,] mat)
        {
            List<List<TextBlock>> lsts = new List<List<TextBlock>>();
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                lsts.Add(new List<TextBlock>());

                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    TextBlock txt = new TextBlock
                    {
                        Width = 30,
                        Height = 30,
                        Background = Brushes.Gray,
                        TextAlignment = TextAlignment.Center,
                        Margin = new Thickness(1, 1, 1, 1)

                    };
                    txt.Text = mat[i, j].ToString();
                    lsts[i].Add(txt);
                }
            }
            Weg.ItemsSource = lsts;
        }

        private void addDistanzMatrixFelder(int[,] mat)
        {
            List<List<TextBlock>> lsts = new List<List<TextBlock>>();
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                lsts.Add(new List<TextBlock>());

                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    TextBlock txt = new TextBlock
                    {
                        Width = 30,
                        Height = 30,
                        Background = Brushes.Gray,
                        TextAlignment = TextAlignment.Center,
                        Margin = new Thickness(1, 1, 1, 1)
                    };
                    txt.Text = mat[i, j].ToString();
                    lsts[i].Add(txt);
                }
            }
            Distanz.ItemsSource = lsts;
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int[] tg = button.Tag as int[];

            double x1 = 0.0;
            double x2 = 0.0;
            double y1 = 0.0;
            double y2 = 0.0; 

            foreach (UIElement uielem in ZeichenFlaeche.Children)
            {
                var item = uielem as Ellipse;
                if (item != null)
                {
                    if ((int)item.Tag == tg[0])
                    {
                        x1 = Canvas.GetLeft(item)+15;
                        y1 = Canvas.GetTop(item)+15;
                    }
                    if ((int)item.Tag == tg[1])
                    {
                        x2 = Canvas.GetLeft(item)+15;
                        y2 = Canvas.GetTop(item)+15;
                    }
                }
            }
            if (button.Content.ToString() == "0")
            {
                button.Content = 1;
                Line myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                myLine.X1 = x1;
                myLine.X2 = x2;
                myLine.Y1 = y1;
                myLine.Y2 = y2;
                myLine.Tag = "" + tg[0] + tg[1];
                myLine.HorizontalAlignment = HorizontalAlignment.Left;
                myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = 4;
                Canvas.SetZIndex(myLine, 0);

                matrixen.Adjazenzmatrix[tg[0], tg[1]] = 1;
                matrixen.Adjazenzmatrix[tg[1], tg[0]] = 1;
                ZeichenFlaeche.Children.Add(myLine);
            }
            else
            {
                Line remove = null;
                var ob = ZeichenFlaeche.Children;
                foreach (UIElement element in ob)
                {
                    var item = element as Line;
                    if (item != null)
                    {
                        if (item.Tag.Equals("" + tg[0] + tg[1]))
                        {
                            remove = item;
                        }
                    }
                }

                ZeichenFlaeche.Children.Remove(remove);
                button.Content = 0;
                matrixen.Adjazenzmatrix[tg[0], tg[1]] = 0;
                matrixen.Adjazenzmatrix[tg[1], tg[0]] = 0;
                
            }
            updateView();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowDialog wd = new WindowDialog();
            this.Hide();
            wd.ShowDialog();
            matrixen = new Matrix((int)wd.slider.Value);
            this.Show();
            updateView();
            zeichneGraphen();
        }
        private void updateData()
        {
        
            matrixen.Wegmatrixzeichnung(matrixen.Adjazenzmatrix);
            matrixen.Distanzmatrixzeichnung(matrixen.Adjazenzmatrix);
            
        }
        private void updateView()
        {
            addMatrixButtons(matrixen.Adjazenzmatrix);
            updateData();
            addWegMatrixFelder(matrixen.Wegmatrix);
            addDistanzMatrixFelder(matrixen.Distanzmatrix);
            textDurchmesser.Text = "Durchmesser: " + matrixen.DurchmesserBerechnung();
            textRadius.Text = "Radius: " + matrixen.RadiusBerechnung();
            textKomponente.Text = "Anzahl Komponente: " + KomponenteToText();
            
        }
        private String KomponenteToText()
        {
            String komp = matrixen.KomponentenAnzahl().Count+"\n";

            for (int i = 0; i < matrixen.KomponentenAnzahl().Count; i++)
            {
                ArrayList komponente = matrixen.KomponentenAnzahl();
                ArrayList unterKomponente = (ArrayList) komponente[i];

                for (int x = 0; x < unterKomponente.Count; x++)
                {
                    int d =(int) unterKomponente[x] + 1;
                    komp += d + " ";
                }
                komp += "\n";
            }
            return komp;
        }



        private void zeichneGraphen()
        {
            ZeichenFlaeche.Children.Clear();
            Random rnd = new Random();

            for (int i = 0; i < matrixen.Adjazenzmatrix.GetLength(1); i++)
            {
                Ellipse myEllipse = new Ellipse();
                myEllipse.Tag=i;
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                // Describes the brush's color using RGB values. 
                // Each value has a range of 0-255.
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
                myEllipse.Fill = mySolidColorBrush;
                myEllipse.StrokeThickness = 2;
                myEllipse.Stroke = Brushes.Black;

                // Set the width and height of the Ellipse.
                myEllipse.Width = 30;
                myEllipse.Height = 30;
                myEllipse.MouseDown += new MouseButtonEventHandler(xMouseDown);
                myEllipse.MouseMove += new MouseEventHandler(xMouseMove);
                myEllipse.MouseUp += new MouseButtonEventHandler(xMouseUp);
                Canvas.SetZIndex(myEllipse, 1);
                Canvas.SetLeft(myEllipse, rnd.Next(1, 20) * 20);
                Canvas.SetTop(myEllipse, rnd.Next(1, 20) * 20);
                ZeichenFlaeche.Children.Add(myEllipse);
            }

        }
        bool moving;
        Ellipse pressedEllipse;

        private void xMouseDown(object sender, MouseButtonEventArgs e)
        {
            moving = true;
            pressedEllipse = sender as Ellipse;
            pressedEllipse.CaptureMouse();
        }

        private void xMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                moving = false;
            }
            if (moving == true)
            {
                double x = e.GetPosition(ZeichenFlaeche).X - 15;
                double y = e.GetPosition(ZeichenFlaeche).Y - 15;
                
                if(x < 0 )
                x = 0; 
                if(y < 0 )
                y = 0;
                if (x >= ZeichenFlaeche.ActualWidth)
                    x = ZeichenFlaeche.ActualWidth-30;
                if (y >= ZeichenFlaeche.Height)
                    y = ZeichenFlaeche.Height-30;


                string eltag = pressedEllipse.Tag.ToString();
                var ob = ZeichenFlaeche.Children;

                foreach (UIElement element in ob)
                {
                    var item = element as Line;
                    if (item != null)
                    {
                        if(item.Tag.ToString().StartsWith(eltag))
                        {
                            item.X1 = x+15;
                            item.Y1 = y+15;
                        }
                        if(item.Tag.ToString().EndsWith(eltag))
                        {
                            item.X2 = x+15;
                            item.Y2 = y+15;
                        }
                    }
                }



                Canvas.SetLeft(pressedEllipse, x);
                Canvas.SetTop(pressedEllipse,y);
            }
        }
        private void xMouseUp(object sender, MouseButtonEventArgs e)
        {
            moving = false;
            pressedEllipse.ReleaseMouseCapture();
        }

    }


}
