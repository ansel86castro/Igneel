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

namespace MNISTDigitRecognizer
{
    /// <summary>
    /// Interaction logic for LearningCurveGraph.xaml
    /// </summary>
    public partial class LearningCurveGraph : UserControl
    {
        int maxX;
        int maxY;

        public LearningCurveGraph()
        {
            InitializeComponent();            
            
        }

        public void AddPoint(int example, double error)
        {
            var x = (double) example / maxX;
            x *= ActualWidth;

            var y = error / maxY;
            y *= ActualHeight;

            polyline.Points.Add(new Point(x, y));
        }   

    }
}
