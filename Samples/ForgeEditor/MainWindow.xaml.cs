using Igneel;
using Igneel.Graphics;
using Igneel.Physics;
using Igneel.Presenters;
using Igneel.Rendering;
using Igneel.Rendering.Presenters;
using Igneel.States;
using Igneel.Windows;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Igneel.Effects;

namespace ForgeEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainShell
    {

        private float _fps;
        private float _baseTime;

        MainWindowViewModel viewModel;
        private Igneel.Rendering.Effect untranformed;
        private Sprite.IShaderInput input;      

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new MainWindowViewModel(this);
            viewModel.SceneCreated += viewModel_SceneCreated;
            viewModel.ContentLoaded += viewModel_ContentLoaded;
            viewModel.HitTestPerformed += viewModel_HitTestPerformed;
            this.Resources["ViewModel"] = viewModel;

            this.WindowState = System.Windows.WindowState.Maximized;
            Canvas.SnapsToDevicePixels = true;

            Loaded += MainWindow_Loaded;
            DataContext = viewModel;            

        }

        void viewModel_HitTestPerformed(object sender, Igneel.Techniques.HitTestResult e)
        {
            SceneTreeView.SelectedItem = e.Frame;            
        }

        void viewModel_ContentLoaded(object sender, Igneel.SceneManagement.Scene e)
        {
            SceneTreeView.ViewModel.Scene = e;
        }

        void viewModel_SceneCreated(object sender, Igneel.SceneManagement.Scene e)
        {
            SceneTreeView.ViewModel.Scene = e;
        }       

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Bootstraper.Init(this, Canvas);

            viewModel.Init();                

            Canvas.GetNativeCanvas().MouseDown += NativeCanvas_MouseDown;
            Canvas.GetNativeCanvas().MouseMove += MainWindow_MouseMove;
            Engine.Presenter.Rendering += Presenter_Rendering;

            //idTecnique = new IdTechnique();
            //IdTechnique.Register();
          
        }

        public Igneel.Windows.Wpf.Canvas3D.Win32Canvas Canvas3D { get { return Canvas.GetNativeCanvas(); } }

        void MainWindow_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                viewModel.HitMove(e.Location);
            }
        }

        private void NativeCanvas_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                var point = e.Location;
                var location = new Vector2((float)point.X, (float)point.Y);
                viewModel.HitTest(location);
            }
        }


        void Presenter_Rendering()
        {
            if (_fps == -1)
            {
                _fps = 0;
                _baseTime = Engine.Time.Time;
            }
            else
            {
                float time = Engine.Time.Time;
                if ((time - _baseTime) > 1.0f)
                {
                    Dispatcher.Invoke(delegate()
                    {
                        FPS.Text = _fps.ToString();
                    });

                    _fps = 0;
                    _baseTime = time;
                }
                _fps++;
            }

            //var hiTest = viewModel.HitTest.Technique.Texture;
            //RenderTexture(Engine.Graphics, hiTest.Texture);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Engine.Presenter.Rendering -= Presenter_Rendering;
            viewModel.StopGameLoop();
            base.OnClosing(e);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        #region IMainShell Members


        public async void ShowProgressDialog()
        {
           
        }

        public async void HideProgressDialog()
        {
          
        }

        #endregion
      

        private void RenderTexture(GraphicDevice device, Texture2D texture, int x = 0, int y = 0, int width = 256, int height = 256)
        {
            if (untranformed == null)
            {
                untranformed = Igneel.Rendering.Effect.GetEffect<RenderQuadEffect>(GraphicDeviceFactory.Device);
                input = untranformed.Map<Igneel.Rendering.Sprite.IShaderInput>();
            }

            var sprite = Service.Require<Sprite>();
            device.PS.SetResource(0, texture);
            device.PS.SetSampler(0, SamplerState.Linear);

            sprite.Begin();
            sprite.SetTrasform(input, new Igneel.Rectangle(x, y, width, height), Igneel.Matrix.Identity);
            sprite.DrawQuad(untranformed);
            sprite.End();

            device.PS.SetResource(0, null);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
