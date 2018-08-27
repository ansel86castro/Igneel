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
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MNISTDigitRecognizer"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MNISTDigitRecognizer;assembly=MNISTDigitRecognizer"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class CurveGraph : Control
    {
        static CurveGraph()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CurveGraph), new FrameworkPropertyMetadata(typeof(CurveGraph)));
        }



        public string XaxisLabel
        {
            get { return (string)GetValue(XaxisLabelProperty); }
            set { SetValue(XaxisLabelProperty, value); }
        }



        public string YaxisLabel
        {
            get { return (string)GetValue(YaxisLabelProperty); }
            set { SetValue(YaxisLabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YaxisLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YaxisLabelProperty =
            DependencyProperty.Register("YaxisLabel", typeof(string), typeof(CurveGraph), new PropertyMetadata("x"));



        // Using a DependencyProperty as the backing store for XaxisLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XaxisLabelProperty =
            DependencyProperty.Register("XaxisLabel", typeof(string), typeof(CurveGraph), new PropertyMetadata("y"));

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            
        }


    }
}
