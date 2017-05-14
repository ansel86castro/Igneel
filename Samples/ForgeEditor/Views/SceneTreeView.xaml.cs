using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Igneel;

namespace ForgeEditor.Views
{
    /// <summary>
    /// Interaction logic for SceneTreeView.xaml
    /// </summary>
    public partial class SceneTreeView : UserControl, ISceneView
    {
      
        public SceneViewModel ViewModel
        {
            get { return (SceneViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(SceneViewModel), typeof(SceneTreeView), new PropertyMetadata(null, new PropertyChangedCallback(ViewModelChange)));
        private MainWindowViewModel mainViewModel;


        private static void ViewModelChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SceneTreeView control = (SceneTreeView)d;
            SceneViewModel viewModel = (SceneViewModel)e.NewValue;

            if (viewModel.Scene != null)
            {
                control.LoadFrames();
            }
        }

          
        public SceneTreeView()
        {
            InitializeComponent();

            ViewModel = new SceneViewModel(this);
            Loaded += SceneTreeView_Loaded;
        }

        void SceneTreeView_Loaded(object sender, RoutedEventArgs e)
        {
            this.mainViewModel = Service.Get<MainWindowViewModel>();          
        }

        private void LoadFrames()
        {
            foreach (ToggleButton item in ToolBar.Children)
            {
                item.IsChecked = false;
            }
            Frames.IsChecked = true;
            var scene = ViewModel.Scene;

            TreeView.Items.Clear();
            foreach (var frame in scene.Nodes)
            {
                TreeView.Items.Add(CreateTreeViewItem(frame));
            }
        }

        private TreeViewItem CreateTreeViewItem(Igneel.SceneManagement.Frame frame)
        {
            TreeViewItem item = new TreeViewItem()
            {
                Header = frame.Name,
                Tag = frame,                
            };
            foreach (var children in frame.Childrens)
            {
                item.Items.Add(CreateTreeViewItem(children));
            }
            return item;
        }

        private void CollapseView(object sender, RoutedEventArgs e)
        {

        }

        private void OnShowFrames(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void OnShowShowCameras(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void OnShowLights(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void OnShowMeshes(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void OnShowPhysics(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void IsSceneAvailable(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel != null && ViewModel.Scene != null;
        }

        private void OnPingView(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void OnCloseView(object sender, ExecutedRoutedEventArgs e)
        {

        }     

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {            
            TreeViewItem item = (TreeViewItem)e.NewValue;
            if (item == null)
            {
                mainViewModel.TransformationController.SelectedFrame = null;
                return;
            }

            Igneel.SceneManagement.Frame frame = item.Tag as Igneel.SceneManagement.Frame;
            if (frame != null)
            {
                mainViewModel.TransformationController.ShowTransformGlyp(frame);
            }
            else
            {
                mainViewModel.TransformationController.SelectedFrame = null;
            }
        }

        public object SelectedItem
        {
            get { return TreeView.SelectedItem != null ? ((TreeViewItem)TreeView.SelectedItem).Tag : null; }
            set{

                if (value != null)
                {
                    TreeViewItem item = Any(TreeView.Items.Cast<TreeViewItem>(), x => x.Tag == value);
                    if (item != null)
                    {
                        item.IsSelected = true;
                        item.BringIntoView();
                    }
                }
                else
                {
                    TreeViewItem item = Any(TreeView.Items.Cast<TreeViewItem>(), x => x.IsSelected);
                    if(item!=null)
                        item.IsSelected = false;
                }
            }
        }

        private TreeViewItem Any(IEnumerable<TreeViewItem> items, Func<TreeViewItem, bool> predicate)
        {
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    return item;
                }
                else if(item.Items.Count > 0)
                {
                    var finded = Any(item.Items.Cast<TreeViewItem>(), predicate);
                    if (finded!=null)
                    {
                        return finded;
                    }
                }
            }
            return null;
        }

        #region ISceneView Members

        public void OnSceneChanged(Igneel.SceneManagement.Scene scene)
        {
            LoadFrames();
        }

        #endregion
    }
}
