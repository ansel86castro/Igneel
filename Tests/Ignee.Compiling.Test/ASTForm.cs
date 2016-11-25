using Igneel.Compiling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ignee.Compiling.Test
{
    public partial class ASTForm : Form
    {       

        public ASTForm(HLSLCompiler compiler)
        {
            InitializeComponent();

            CreateTree(compiler);
        }

        private void CreateTree(HLSLCompiler compiler)
        {
            foreach (var p in compiler.Programs)
            {
                foreach (var item in p.Declarations)
                {
                    tvAST.Nodes.Add(CreateNode(item));
                }
            }
           
            tvAST.Refresh();
        }

        public TreeNode CreateNode(ASTNode node)
        {
            TreeNode tnode = new TreeNode(node.ToString()) { Tag = node }; ;
            foreach (var item in node.GetNodes())
            {
                if(item!=null)
                    tnode.Nodes.Add(CreateNode(item));
            }
            return tnode;
        }

        private void tvAST_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = e.Node.Tag as ASTNode;
            if (node == null) return;
            tbNodeString.Text = node.ToString();            

        }


    }
}
