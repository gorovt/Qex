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

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qex
{
    public partial class frmRecursosMaterial : System.Windows.Forms.Form
    {
        public Quantity _quan;
        public Element _elem;
        public Document _doc;

        public frmRecursosMaterial(Document doc, Quantity quan)
        {
            InitializeComponent();
            _quan = quan;
            _doc = doc;
            _elem = doc.GetElement(new ElementId(quan.typeId));
            this.lblConsumos.Text = "   Consumo de Recursos: " + quan.name;
            this.picQuantity.BackgroundImage = quan.image;
            this.lsvMateriales.SmallImageList = Tools.GetImageListIcons();
            this.lsvMateriales.LargeImageList = Tools.GetImageListIcons();
            this.lstvRecursos.SmallImageList = Tools.GetImageListIcons();
            this.lstvRecursos.LargeImageList = Tools.GetImageListIcons();
            fillTree(this.trvGrupos);
            fillListView();
        }

        private void fillTree(TreeView tree)
        {
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

        private void fillListViewMateriales(string grupoId)
        {
            QexGrupoMaterial grupo = DalRecursos.GetGrupoMaterialById(grupoId);
            foreach (QexMaterial mat in grupo.GetChilds())
            {
                ListViewItem itm = mat.ToItem();
                this.lsvMateriales.Items.Add(itm);
            }
        }

        private void fillListView()
        {
            List<QexRecurso> recursos = DalRecursos.ReadRecursosFromElement(_elem);
            foreach (QexRecurso rec in recursos)
            {
                QexMaterial mat = rec.GetMaterial();
                string[] values = new string[3];
                values[0] = mat.nombre;
                values[1] = rec.Consumo.ToString();
                values[2] = mat.unidad;
                ListViewItem itm = new ListViewItem(values);
                itm.Name = rec.id;
                itm.ImageIndex = mat.index;
                //itm.StateImageIndex = 1;
                this.lstvRecursos.Items.Add(itm);
            }
        }
        
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (this.lstvRecursos.SelectedItems.Count > 0)
            {
                ListViewItem item = this.lstvRecursos.SelectedItems[0];
                string id = item.Name;
                QexRecurso recurso = DalRecursos.ReadRecursosFromElement(_elem).FirstOrDefault(x => x.id == id);
                (new frmEditarRecurso(_doc, _quan, recurso)).ShowDialog();
                this.lstvRecursos.Items.Clear();
                fillListView();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un Material", "Revit",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (this.lstvRecursos.SelectedItems.Count > 0)
            {
                DialogResult result = MessageBox.Show("¿Desea quitar este Recurso?", "Revit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    ListViewItem item = this.lstvRecursos.SelectedItems[0];
                    string id = item.Name;
                    QexRecurso recurso = DalRecursos.ReadRecursosFromElement(_elem).FirstOrDefault(x => x.id == id);
                    if (recurso.Delete(_elem))
                    {
                        //MessageBox.Show("Se quitó correctamente", "Revit",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.lstvRecursos.Items.Clear();
                        fillListView();
                    }
                    else
                    {
                        MessageBox.Show("No se puede quitar", "Revit",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un Material", "Revit",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void trvGrupos_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.lsvMateriales.Items.Clear();
            string id = e.Node.Name;
            fillListViewMateriales(id);
            QexGrupoMaterial grupo = DalRecursos.GetGrupoMaterialById(id);
            if (grupo.GetChilds().Count != 0)
            {
                this.lsvMateriales.Items[0].Selected = true;
                //this.lstvMateriales.Select();
                //this.btnEditarMaterial.Enabled = true;
                //this.btnEliminarMaterial.Enabled = true;
            }
            else
            {
                //this.btnEditarMaterial.Enabled = false;
                //this.btnEliminarMaterial.Enabled = false;
            }
        }

        private void lsvMateriales_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (this.lsvMateriales.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    this.mnuMaterial.Show(Cursor.Position);
                }
            }
        }

        private void mnuMatAgregar_Click(object sender, EventArgs e)
        {
            string id = this.lsvMateriales.SelectedItems[0].Name;
            QexMaterial mat = DalRecursos.GetMaterialById(id);
            QexRecurso rec = new QexRecurso(Guid.NewGuid().ToString(), mat.id, 1);
            if (rec.Insert(_elem))
            {
                
            }
            else
            {
                MessageBox.Show("No se pueden agregar los Recursos", RevitQex.QexName,
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.lstvRecursos.Items.Clear();
            fillListView();
        }

        private void lstvRecursos_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (this.lstvRecursos.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    this.mnuRecurso.Show(Cursor.Position);
                }
            }
        }

        private void mnuRecEditar_Click(object sender, EventArgs e)
        {
            string id = this.lstvRecursos.SelectedItems[0].Name;
            QexRecurso recurso = DalRecursos.GetRecursoById(_elem, id);
            (new frmEditarRecurso(_doc, _quan, recurso)).ShowDialog();
            this.lstvRecursos.Items.Clear();
            fillListView();
        }

        private void mnuRecQuitar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Desea quitar este Recurso?", RevitQex.QexName,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                string id = this.lstvRecursos.SelectedItems[0].Name;
                List<QexRecurso> lst = DalRecursos.ReadRecursosFromElement(_elem);
                List<QexRecurso> lstUpdated = new List<QexRecurso>();
                foreach (var item in lst)
                {
                    if (item.id != id)
                    {
                        lstUpdated.Add(item);
                    }
                }
                DalRecursos.CrearEntidadRecursos(lstUpdated, _elem);
                this.lstvRecursos.Items.Clear();
                fillListView();
            }
        }

        private void lsvMateriales_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
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
            tip.SetToolTip(this.lsvMateriales, sb.ToString());
            tip.ToolTipTitle = mat.nombre;
            tip.ToolTipIcon = ToolTipIcon.Info;
        }

        private void btnMover_Click(object sender, EventArgs e)
        {
            
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (this.txtBuscar.Text == "")
            {
                this.trvGrupos.Nodes.Clear();
                fillTree(this.trvGrupos);
                this.trvGrupos.SelectedNode = this.trvGrupos.Nodes[0];
            }
            else
            {
                string buscar = txtBuscar.Text;
                this.trvGrupos.Nodes.Clear();
                this.lsvMateriales.Items.Clear();
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
                        this.lsvMateriales.Items.Add(itm);
                    }
                }
                this.trvGrupos.Nodes.Add(node0);
            }
        }

        private void btnGestionarRecursos_Click(object sender, EventArgs e)
        {
            (new frmSetMaterialDb()).ShowDialog();
            fillTree(this.trvGrupos);
        }
    }
}
