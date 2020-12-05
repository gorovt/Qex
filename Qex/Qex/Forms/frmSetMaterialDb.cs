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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qex
{
    public partial class frmSetMaterialDb : Form
    {
        public frmSetMaterialDb()
        {
            InitializeComponent();
            fillTree(this.treeViewMateriales);
            this.treeViewMateriales.ImageList = imageList1;
            this.lstvMateriales.SmallImageList = Tools.GetImageListIcons();
            this.lstvMateriales.LargeImageList = Tools.GetImageListIcons();
            if (this.treeViewMateriales.Nodes.Count > 0)
                this.treeViewMateriales.SelectedNode = this.treeViewMateriales.Nodes[0];
        }

        private void fillTree(TreeView tree)
        {
            tree.Nodes.Clear();
            List<QexGrupoMaterial> grupos = DalRecursos.ReadGrupos();
            grupos = grupos.OrderBy(x => x.Nombre).ToList();
            foreach (QexGrupoMaterial grupo in grupos)
            {
                TreeNode node1 = grupo.ToNode();
                node1.ImageIndex = 0;
                node1.SelectedImageIndex = 1;
                tree.Nodes.Add(node1);
            }
        }

        private void fillListView(string grupoId)
        {
            QexGrupoMaterial grupo = DalRecursos.GetGrupoMaterialById(grupoId);
            foreach (QexMaterial mat in grupo.GetChilds())
            {
                ListViewItem itm = mat.ToItem();
                this.lstvMateriales.Items.Add(itm);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuImportarMateriales_Click(object sender, EventArgs e)
        {
            
            (new frmMaterialesImportarExcel()).ShowDialog();
            fillTree(this.treeViewMateriales);
            this.treeViewMateriales.ImageList = imageList1;
            if (this.treeViewMateriales.Nodes.Count > 0)
                this.treeViewMateriales.SelectedNode = this.treeViewMateriales.Nodes[0];
        }

        #region ListView Materiales

        private void lstvMateriales_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (this.lstvMateriales.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    this.mnuRecursos.Show(Cursor.Position);
                }
            }
        }

        private void btnCrearMateriales_Click(object sender, EventArgs e)
        {
            Tools.CrearListaInicialGrupos();
            Tools.CrearListaInicialMateriales();
            this.treeViewMateriales.Nodes.Clear();
            fillTree(this.treeViewMateriales);
        }

        private void btnEditarMaterial_Click(object sender, EventArgs e)
        {
            if (this.lstvMateriales.SelectedItems.Count > 0)
            {
                ListViewItem item = this.lstvMateriales.SelectedItems[0];
                string id = item.Name;
                QexMaterial material = DalRecursos.GetMaterialById(id);
                string grupoId = material.grupoId;
                (new frmEditarMaterial(material)).ShowDialog();
                this.txtBuscar.Text = "";
                this.treeViewMateriales.Nodes.Clear();
                fillTree(this.treeViewMateriales);
                this.treeViewMateriales.SelectedNode = this.treeViewMateriales.Nodes[grupoId];
            }
            else
            {
                MessageBox.Show("Debe seleccionar un Material", "Revit",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEliminarMaterial_Click(object sender, EventArgs e)
        {
            if (this.lstvMateriales.SelectedItems.Count > 0)
            {
                DialogResult result = MessageBox.Show("¿Desea eliminar este Material?", "Revit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    ListViewItem item = this.lstvMateriales.SelectedItems[0];
                    string id = item.Name;
                    QexMaterial mat = DalRecursos.GetMaterialById(id);
                    if (mat.IsUsed())
                    {
                        MessageBox.Show("El Material está siendo utilizado por una Quantificación", RevitQex.QexName,
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string grupoId = mat.grupoId;
                        if (mat.Delete())
                        {
                            this.treeViewMateriales.Nodes.Clear();
                            fillTree(this.treeViewMateriales);
                            this.lblStatus.Text = DateTime.Now.ToShortTimeString() +
                                "El Material se ha eliminado correctamente";
                            //MessageBox.Show("El Material se ha eliminado correctamente", "Revit",
                            //    MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No se puede eliminar el Material.", "Revit",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        this.txtBuscar.Text = "";
                        this.treeViewMateriales.Nodes.Clear();
                        fillTree(this.treeViewMateriales);
                        this.treeViewMateriales.SelectedNode = this.treeViewMateriales.Nodes[grupoId];
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un Material", "Revit",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (this.txtBuscar.Text == "")
            {
                this.treeViewMateriales.Nodes.Clear();
                fillTree(this.treeViewMateriales);
                this.treeViewMateriales.SelectedNode = this.treeViewMateriales.Nodes[0];
            }
            else
            {
                string buscar = txtBuscar.Text;
                this.treeViewMateriales.Nodes.Clear();
                this.lstvMateriales.Items.Clear();
                TreeNode node0 = new TreeNode();
                node0.Name = "00";
                node0.Text = "Resultados";
                node0.ImageIndex = 0;
                node0.SelectedImageIndex = 0;
                node0.ExpandAll();
                foreach (QexMaterial mat in DalRecursos.ReadMateriales())
                {
                    if (mat.nombre.ToLower().Contains(buscar))
                    {
                        ListViewItem itm = mat.ToItem();
                        this.lstvMateriales.Items.Add(itm);
                    }
                }
                this.treeViewMateriales.Nodes.Add(node0);
            }
        }
        #endregion

        #region TreeView
        private void treeViewMateriales_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.lstvMateriales.Items.Clear();
            string id = e.Node.Name;
            fillListView(id);
            if (DalRecursos.ReadMateriales().FindAll(x => x.grupoId == id).Count != 0)
            {
                this.lstvMateriales.Items[0].Selected = true;
                //this.lstvMateriales.Select();
            }
        }

        private void treeViewMateriales_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //Verificar que se esté seleccionando un Nodo
                if (e.Node != null)
                {
                    this.mnuGrupos.Show(Cursor.Position);
                }
            }
        }

        private void btnCrearGrupo_Click(object sender, EventArgs e)
        {
            (new frmEditarGrupo()).ShowDialog();
            this.treeViewMateriales.Nodes.Clear();
            fillTree(this.treeViewMateriales);
            if (this.treeViewMateriales.Nodes.Count > 0)
                this.treeViewMateriales.SelectedNode = this.treeViewMateriales.Nodes[0];
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (this.treeViewMateriales.Nodes.Count > 0)
            {
                string id = this.treeViewMateriales.SelectedNode.Name;
                frmEditarMaterial matForm = new frmEditarMaterial(id);
                matForm.ShowDialog();
                this.txtBuscar.Text = "";
                this.treeViewMateriales.Nodes.Clear();
                fillTree(this.treeViewMateriales);
                this.treeViewMateriales.SelectedNode = this.treeViewMateriales.Nodes[id];
            }
            else
            {
                MessageBox.Show("Debe seleccionar un Grupo de Materiales", "Revit",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (this.treeViewMateriales.SelectedNode != null)
            {
                string id = this.treeViewMateriales.SelectedNode.Name;
                QexGrupoMaterial grupo = DalRecursos.GetGrupoMaterialById(id);
                (new frmEditarGrupo(grupo)).ShowDialog();
                this.treeViewMateriales.Nodes.Clear();
                fillTree(this.treeViewMateriales);
                this.treeViewMateriales.SelectedNode = this.treeViewMateriales.Nodes[id];
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            StringBuilder db = new StringBuilder();
            DialogResult result = MessageBox.Show("¿Desea eliminar este Grupo?", "Revit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string id = this.treeViewMateriales.SelectedNode.Name;
                    QexGrupoMaterial grupo = DalRecursos.GetGrupoMaterialById(id);
                    int delete = grupo.Delete();
                    if (delete == 0)
                    {
                        MessageBox.Show("No se puede eliminar el Grupo", RevitQex.QexName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    if (delete == 1)
                    {
                        this.treeViewMateriales.Nodes.Clear();
                        this.lstvMateriales.Items.Clear();
                        fillTree(this.treeViewMateriales);
                        lblStatus.Text = DateTime.Now.ToShortTimeString() + ": El Grupo se ha eliminado correctamente";
                    }

                    if (delete == 2)
                    {
                        MessageBox.Show("No se puede eliminar el Grupo, hay Recursos usados por Quantificaciones"
                            , RevitQex.QexName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("No se puede eliminar el Grupo", "Revit",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        #endregion

        private void lstvMateriales_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            string id = e.Item.Name;
            QexMaterial mat = DalRecursos.GetMaterialById(id);
            QexGrupoMaterial grupo = DalRecursos.GetGrupoMaterialById(mat.grupoId);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Unidad: " + mat.unidad);
            sb.AppendLine("Precio: " + String.Format("{0:c}", mat.precio));
            sb.AppendLine("Grupo: " + grupo.Nombre);
            ToolTip tip = new ToolTip();
            tip.ShowAlways = true;
            tip.SetToolTip(this.lstvMateriales, sb.ToString());
            tip.ToolTipTitle = mat.nombre;
            tip.ToolTipIcon = ToolTipIcon.Info;
        }

        private void mnuExportarMaterialesExcel_Click(object sender, EventArgs e)
        {
            //Exporting to Excel
            string folderPath = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Documento Excel|*.xlsx";
            string title = Tools._doc.Title;
            sfd.FileName = title + ".xlsx";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderPath = sfd.FileName;
                try
                {
                    Informe.ExportarRecursosExcel(title, DalRecursos.ReadMateriales(), folderPath);
                    MessageBox.Show("Documento creado correctamente", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnEsUsado_Click(object sender, EventArgs e)
        {
            ListViewItem item = this.lstvMateriales.SelectedItems[0];
            string id = item.Name;
            QexMaterial mat = DalRecursos.GetMaterialById(id);
            if(mat.IsUsed())
            {
                MessageBox.Show("El Recurso es utilizado por una Quantificación", RevitQex.QexName,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("El Recurso NO está siendo utilizado", RevitQex.QexName,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
