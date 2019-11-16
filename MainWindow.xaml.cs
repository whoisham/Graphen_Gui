using System;
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
                        TextAlignment = TextAlignment.Center
                        
                    };
                    txt.Text = mat[i, j].ToString();
                    lsts[i].Add(txt);
                }
            }
            Weg.ItemsSource = lsts;
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int[] tg = button.Tag as int[];
            if (button.Content.ToString() == "0")
            {
                button.Content = 1;

                matrixen.Adjazenzmatrix[tg[0], tg[1]] = 1;
                matrixen.Adjazenzmatrix[tg[1], tg[0]] = 1;
            }
            else
            {
                button.Content = 0;
                matrixen.Adjazenzmatrix[tg[0], tg[1]] = 0;
                matrixen.Adjazenzmatrix[tg[1], tg[0]] = 0;
            }
            updateView();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowDialog wd = new WindowDialog();
            wd.ShowDialog();
            matrixen = new Matrix((int)wd.slider.Value);
            
            updateView();
        }

        private void updateView()
        {
            addMatrixButtons(matrixen.Adjazenzmatrix);
            matrixen.Wegmatrixzeichnung(matrixen.Adjazenzmatrix);
            addWegMatrixFelder(matrixen.Wegmatrix);
        }
    }


}
