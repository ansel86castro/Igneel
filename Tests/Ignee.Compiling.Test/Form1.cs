using Igneel.Compiling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ignee.Compiling.Test
{
    public partial class Form1 : Form
    {
        HLSLCompiler _compiler;
        public Form1()
        {

            InitializeComponent();
            int a = 5 + 
# if !DEBUG
                5
#else
                6
#endif
                ;

            var r = Regex.Replace("ansel<castro+5(N)[S]\\.DOTA", @"\w+", x =>
                {
                    if (x.Value == "N")
                        return "SAMPLER";
                    return x.Value;
                });        
           
        }

        private void compileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSource.Text))
                return;

            _compiler = new HLSLCompiler();
            _compiler.Sources = new string[] { tbSource.Text };
            _compiler.IncludePath = new string[] { @"E:\Projects\Igneel\ShaderCompiler\Shaders.D3D10\Shaders.D3D10" };
            _compiler.Compile();

            if (_compiler.Errors.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var item in _compiler.Errors)
                {
                    sb.AppendLine(item);
                }
                MessageBox.Show(sb.ToString(), "Errors");
            }
            else
            {
                tbCompiled.Text = _compiler.GenerateCode();
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_compiler == null)return;

            ASTForm form = new ASTForm(_compiler);
            form.Show(this);
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog d = new OpenFileDialog())
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    tbSource.Text = File.ReadAllText(d.FileName);
                }
            }
        }
    }
}
