using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Igneel.Design.Forms
{
    public partial class TypeProviderForm : Form
    {
        List<Type> types;
        List<Assembly> assemblys;
        Type baseType;

        public List<Type> Types
        {
            get { return types; }           
        }        

        public List<Assembly> Assemblys
        {
            get { return assemblys; }           
        }        

        public Type BaseType
        {
            get { return baseType; }           
        }

          public Type SelectedType
        {
            get
            {
                if (clbTypes.SelectedIndex >= 0)
                {
                    return (Type)clbTypes.Items[clbTypes.SelectedIndex];
                }
                return null;
            }
        }
       

        public TypeProviderForm(Type baseType)
        {
            this.baseType = baseType;

            InitializeComponent();

            types = new List<Type>();
            assemblys = new List<Assembly>() { baseType.Assembly };

            Initialize();
        }

        public void Initialize()
        {
            types.Clear();

            foreach (var assembly in assemblys)
            {
                AddTypes(assembly);
            }
            FillAssemblyListBox();
            FillTechniquesListBox();
        }

        private void AddTypes(Assembly assembly)
        {
            Type[] _types = assembly.GetExportedTypes();            
            for (int i = 0; i < _types.Length; i++)
            {
                Type t = _types[i];
                if (!types.Contains(t) && !t.IsInterface && !t.IsAbstract)
                {                    
                    if (baseType.IsInterface && t.GetInterface(baseType.FullName) != null ||
                       (!baseType.IsInterface && t.IsSubclassOf(baseType)))
                             types.Add(t);
                }
            }
        }

        private void FillAssemblyListBox()
        {
            lbAssemblys.Items.Clear();
            foreach (var ass in assemblys)
            {
                lbAssemblys.Items.Add(ass);
            }
        }

        private void FillTechniquesListBox()
        {
            clbTypes.Items.Clear();
            foreach (var type in types)
            {
                clbTypes.Items.Add(type);
            }
        }

        public void AddAssembly(Assembly assembly)
        {
            if (!assemblys.Contains(assembly))
                assemblys.Add(assembly);

            AddTypes(assembly);
        }

        public IEnumerable<Type> GetSelectedTechniques()
        {            
            return clbTypes.SelectedItems.Cast<Type>();            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(this);
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Hide();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Hide();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                Assembly assembly = Assembly.LoadFile(openFileDialog1.FileName);
                AddAssembly(assembly);
                FillAssemblyListBox();
                FillTechniquesListBox();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Unable to Load the Types. \r\nException Message : " + ee.Message);
            }
        }
    }
}
