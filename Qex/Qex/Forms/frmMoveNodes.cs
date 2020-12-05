/*   Qex License
*******************************************************************************
*                                                                             *
*    Copyright (c) 2016-2020 Luciano Gorosito <lucianogorosito@hotmail.com>   *
*                                                                             *
*    This file is part of Qex                                                 *
*                                                                             *
*    Qex is free software: you can redistribute it and/or modify              *
*    it under the terms of the GNU General Public License as published by     *
*    the Free Software Foundation, either version 3 of the License, or        *
*    (at your option) any later version.                                      *
*                                                                             *
*    Qex is distributed in the hope that it will be useful,                   *
*    but WITHOUT ANY WARRANTY; without even the implied warranty of           *
*    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the            *
*    GNU General Public License for more details.                             *
*                                                                             *
*    You should have received a copy of the GNU General Public License        *
*    along with this program.  If not, see <https://www.gnu.org/licenses/>.   *
*                                                                             *
*******************************************************************************
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qex
{
    public partial class frmMoveNodes : Form
    {
        public static TreeNode _selectedNode;

        public frmMoveNodes(TreeView tree)
        {
            InitializeComponent();
            fillTree(tree);
            this.btnOK.Enabled = false;
            this.treeViewGrupos.ImageList = imageList1;
        }

        private void fillTree(TreeView tree)
        {
            foreach (TreeNode node0 in tree.Nodes)
            {
                if (node0.Name != "AA")
                {
                    TreeNode node_0 = new TreeNode();
                    node_0.Name = node0.Name;
                    node_0.Text = node0.Text;
                    node_0.ImageIndex = 0;
                    node_0.SelectedImageIndex = 0;
                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        TreeNode node_1 = new TreeNode();
                        node_1.Name = node1.Name;
                        node_1.Text = node1.Text;
                        node_1.ImageIndex = 1;
                        node_1.SelectedImageIndex = 1;
                        node_0.Nodes.Add(node_1);
                    }
                    node_0.ExpandAll();
                    this.treeViewGrupos.Nodes.Add(node_0);
                }
            }
        }

        private void treeViewGrupos_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                this.btnOK.Enabled = false;
            }
            if (e.Node.Level == 1)
            {
                this.btnOK.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _selectedNode = null;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _selectedNode = this.treeViewGrupos.SelectedNode;
            this.Close();
        }
    }
}
