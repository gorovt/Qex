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

using System.Text.RegularExpressions;
using System.IO;

namespace Qex
{
    public partial class frmMain : System.Windows.Forms.Form
    {
        public frmMain(List<Quantity> lstQuantity)
        {
            InitializeComponent();

            this.treeViewQuantities.ImageList = imageList1;
            Tools.fillTreeView(lstQuantity, this.treeViewQuantities);
            treeViewQuantities.Nodes[0].ImageIndex = 0;
            treeViewQuantities.Nodes[0].SelectedImageIndex = 0;
            dgvElements.MultiSelect = false;
            this.btnToolTipos.Enabled = false;
            this.btnToolDescripcion.Enabled = true;
            this.Text = "Qex";
            this.txtName.Text = RevitQex.QexName;
            this.txtVersion.Text = "Versión " + RevitQex.QexVersion.ToString();
            Tools.UpdatePointsLabel(this.lblPoints);
            this.mnuAsignarRecursos.Enabled = true;
            this.dgvElements.Columns["editar"].CellTemplate = new EditCell();
        }
        #region Treeview y Datagrid
        private void treeViewQuantities_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int level = e.Node.Level;
            this.txtTotalText.Text = "";
            this.txtTotalCost.Text = String.Format("{0:C}", 0);

            if (level == 1) //Folder
            {
                this.dgvElements.DataBindings.Clear();
                string folder = e.Node.Name;

                if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                {
                    this.dgvElements.DataSource = RevitQex.lstElements;
                    this.dgvElements.Columns[category.Name].Visible = true; //Visible
                    this.txtTotalText.Text = e.Node.Text + " >> Costo Total: ";
                    List<Quantity> lstGlobal = RevitQex.lstElements;
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
                if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                {
                    this.dgvElements.DataSource = RevitQex.lstElements.FindAll(x => x.phaseCreated == folder);
                    this.dgvElements.Columns[category.Name].Visible = true; //Visible
                    this.txtTotalText.Text = e.Node.Text + " >> Costo Total: ";
                    List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.phaseCreated == folder);
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
                if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                {
                    int lvlId = (Convert.ToInt32(folder));
                    this.dgvElements.DataSource = RevitQex.lstElements.FindAll(x => x.LevelId == lvlId);
                    this.dgvElements.Columns[category.Name].Visible = true; //Visible
                    this.txtTotalText.Text = e.Node.Text + " >> Costo Total: ";
                    List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.LevelId == lvlId);
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
                if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                {
                    string[] cadena = Regex.Split(folder, " // ");
                    string phase = cadena[0];
                    int lvlId = (Convert.ToInt32(cadena[1]));
                    this.dgvElements.DataSource = RevitQex.lstElements.FindAll(x => x.phaseCreated == phase &&
                    x.LevelId == lvlId);
                    this.dgvElements.Columns[category.Name].Visible = true; //Visible
                    this.txtTotalText.Text = e.Node.Text + " >> Costo Total: ";
                    List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.phaseCreated == phase &&
                    x.LevelId == lvlId);
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
            }
            if (level == 2) //Categories
            {
                this.dgvElements.DataBindings.Clear();
                string _category = e.Node.Text;
                string folder = e.Node.Parent.Name;

                if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                {
                    this.dgvElements.DataSource = RevitQex.lstElements.FindAll(x => x.category == _category);
                    this.dgvElements.Columns[category.Name].Visible = false;
                    this.txtTotalText.Text = e.Node.Text + " >> Costo Total:";
                    List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.category == _category);
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
                if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                {
                    this.dgvElements.DataSource = RevitQex.lstElements.FindAll(x => x.category == _category &&
                    x.phaseCreated == folder);
                    this.dgvElements.Columns[category.Name].Visible = false;
                    this.txtTotalText.Text = e.Node.Text + " >> Costo Total:";
                    List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.category == _category &&
                     x.phaseCreated == folder);
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
                if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                {
                    int lvlId = Convert.ToInt32(folder);
                    this.dgvElements.DataSource = RevitQex.lstElements.FindAll(x => x.category == _category &&
                    x.LevelId == lvlId);
                    this.dgvElements.Columns[category.Name].Visible = false;
                    this.txtTotalText.Text = e.Node.Text + " >> Costo Total:";
                    List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.category == _category &&
                    x.LevelId == lvlId);
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
                if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                {
                    string[] cadena = Regex.Split(folder, " // ");
                    string phase = cadena[0];
                    int lvlId = Convert.ToInt32(cadena[1]);
                    this.dgvElements.DataSource = RevitQex.lstElements.FindAll(x => x.category == _category &&
                    x.phaseCreated == phase && x.LevelId == lvlId);
                    this.dgvElements.Columns[category.Name].Visible = false;
                    this.txtTotalText.Text = e.Node.Text + " >> Costo Total:";
                    List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.category == _category &&
                     x.phaseCreated == phase && x.LevelId == lvlId);
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
            }
        }
        private void dgvElements_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn &&
                e.RowIndex >= 0)
            {
                if (Tools.GetBoolFromDataGridView(this.dgvElements, e.ColumnIndex, e.RowIndex))
                {
                    Tools.SetBooleanCheckToDataGridView(this.dgvElements, e.ColumnIndex, e.RowIndex, false);
                }
                else
                {
                    Tools.SetBooleanCheckToDataGridView(this.dgvElements, e.ColumnIndex, e.RowIndex, true);
                }
            }

                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                Quantity q = (Quantity)dgvElements.Rows[e.RowIndex].DataBoundItem;
                q.length = Math.Round(q.length, 2);
                q.area = Math.Round(q.area, 2);
                q.volume = Math.Round(q.volume, 2);

                (new frmEditar(Tools._doc, q)).ShowDialog();
                Tools.UpdatePointsLabel(this.lblPoints);
                int level = this.treeViewQuantities.SelectedNode.Level;
                if (level == 1) //Folder
                {
                    string folder = this.treeViewQuantities.SelectedNode.Name;
                    this.txtTotalText.Text = this.treeViewQuantities.SelectedNode.Text + " >> Costo Total:";

                    if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                    {
                        List<Quantity> lstGlobal = RevitQex.lstElements;
                        this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                    }
                    if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                    {
                        List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.phaseCreated == folder);
                        this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                    }
                    if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                    {
                        int lvlId = (Convert.ToInt32(folder));
                        List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.LevelId == lvlId);
                        this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                    }
                    if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                    {
                        string[] cadena = Regex.Split(folder, " // ");
                        string phase = cadena[0];
                        List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.phaseCreated == phase);
                        this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                    }
                }
                if (level == 2) //Categories
                {
                    string _category = this.treeViewQuantities.SelectedNode.Name;
                    string folder = this.treeViewQuantities.SelectedNode.Parent.Name;
                    this.txtTotalText.Text = "Costo: " + _category;

                    if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                    {
                        List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.category == _category);
                        this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                    }
                    if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                    {
                        List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.category == _category &&
                     x.phaseCreated == folder);
                        this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                    }
                    if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                    {
                        int lvlId = (Convert.ToInt32(folder));
                        List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.category == _category &&
                     x.LevelId == lvlId);
                        this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                    }
                    if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                    {
                        string[] cadena = Regex.Split(folder, " // ");
                        string phase = cadena[0];
                        int lvlId = (Convert.ToInt32(cadena[1]));
                        List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.category == _category &&
                     x.phaseCreated == phase && x.LevelId == lvlId);
                        this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                    }
                }
                //this.Text = RevitQex.QexName + " " + RevitQex.QexVersion;
                this.dgvElements.Update();
                this.dgvElements.Refresh();
            }
        }
        private void treeViewQuantities_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            int level = e.Node.Level;

            if (level == 0)
            {
                e.Cancel = true;
            }
        }
        public class EditCell : DataGridViewButtonCell
        {
            Image del = Resource1.edit_28;
            protected override void Paint(Graphics graphics, System.Drawing.Rectangle clipBounds, 
                System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, 
                object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, 
                DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, 
                    cellStyle, advancedBorderStyle, paintParts);
                graphics.DrawImage(del, cellBounds);
            }
        }
        public class DeleteColumn : DataGridViewButtonColumn
        {
            public DeleteColumn()
            {
                this.CellTemplate = new EditCell();
                this.Width = 20;
                //set other options here 
            }
        }
        #endregion
        #region Menu desplegable
        private void toolConfeccionarComputo_Click(object sender, EventArgs e)
        {
            bool verDescripcion = false;
            if (!this.btnToolDescripcion.Enabled)
            {
                verDescripcion = true;
            }
            (new frmComputo(Tools._doc, RevitQex.lstElements, verDescripcion)).ShowDialog();
            Tools.UpdatePointsLabel(this.lblPoints);
        }
        private void mnuAsignarRecursos_Click(object sender, EventArgs e)
        {
            bool verDescripcion = false;
            if (!this.btnToolDescripcion.Enabled)
            {
                verDescripcion = true;
            }
            (new frmRecursosMain(Tools._doc, RevitQex.lstElements, verDescripcion)).ShowDialog();
            Tools.UpdatePointsLabel(this.lblPoints);
            
        }
        private void btnExportWord_Click(object sender, EventArgs e)
        {
            List<string> lstInforme = new List<string>();
            string folder = "";
            string folderName = "";
            try
            {
                if (this.treeViewQuantities.SelectedNode.Level == 1)
                {
                    folder = this.treeViewQuantities.SelectedNode.Name;
                    folderName = this.treeViewQuantities.SelectedNode.Text;
                    lstInforme = Informe.CrearListaParaWord(RevitQex.lstElements, folder, !this.btnToolTipos.Enabled);
                    Tools.ExportToWord(lstInforme, folderName);
                }
                if (this.treeViewQuantities.SelectedNode.Level == 2)
                {
                    folder = this.treeViewQuantities.SelectedNode.Parent.Name;
                    folderName = this.treeViewQuantities.SelectedNode.Parent.Text;
                    lstInforme = Informe.CrearListaParaWord(RevitQex.lstElements, folder, !this.btnToolTipos.Enabled);
                    Tools.ExportToWord(lstInforme, folderName);
                }
                Tools.UpdatePointsLabel(this.lblPoints);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("No se puede crear el documento. Debe seleccionar un Grupo");
                sb.AppendLine(ex.Message);
                MessageBox.Show(sb.ToString(), RevitQex.QexName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            List<string> lstInforme = new List<string>();
            string folder = "";
            string folderName = "";
            try
            {
                if (this.treeViewQuantities.SelectedNode.Level == 1)
                {
                    folder = this.treeViewQuantities.SelectedNode.Name;

                    lstInforme = Informe.CrearListaParaExcel(RevitQex.lstElements, folder, !this.btnToolTipos.Enabled);
                    Tools.ExportToExcel(lstInforme, folder);
                }
                if (this.treeViewQuantities.SelectedNode.Level == 2)
                {
                    folder = this.treeViewQuantities.SelectedNode.Parent.Name;
                    lstInforme = Informe.CrearListaParaExcel(RevitQex.lstElements, folder, !this.btnToolTipos.Enabled);
                    Tools.ExportToExcel(lstInforme, folder);
                }
                Tools.UpdatePointsLabel(this.lblPoints);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("No se puede crear el documento. Debe seleccionar un Grupo");
                sb.AppendLine(ex.Message);
                MessageBox.Show(sb.ToString(), RevitQex.QexName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void mnuBtnStats_Click(object sender, EventArgs e)
        {
            (new frmStats()).ShowDialog();
            Tools.UpdatePointsLabel(this.lblPoints);
        }
        private void btnOpciones_Click(object sender, EventArgs e)
        {
            frmOpciones opciones = new frmOpciones();
            opciones.ShowDialog();
            if (opciones.DialogResult == DialogResult.OK)
            {
                this.treeViewQuantities.Nodes.Clear();
                //dgvElements.MultiSelect = false;
                dgvElements.DataBindings.Clear();
                dgvElements.DataSource = new List<Quantity>();
                (new frmSplash("Actualizando Quantificaciones:")).ShowDialog();
                Tools.fillTreeView(RevitQex.lstElements, this.treeViewQuantities);
                treeViewQuantities.Nodes[0].ImageIndex = 0;
                treeViewQuantities.Nodes[0].SelectedImageIndex = 0;
                Tools.UpdatePointsLabel(this.lblPoints);
            }
            

            //this.dgvElements.Update();
            //this.dgvElements.Refresh();
        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #region Toolbar
        private void btnToolTipos_Click(object sender, EventArgs e)
        {
            this.btnToolTipos.Enabled = false;
            this.btnToolDescripcion.Enabled = true;
            this.dgvElements.Columns[name.Name].Visible = true;
            this.dgvElements.Columns[descripcion.Name].Visible = false;
        }
        private void btnToolDescripcion_Click(object sender, EventArgs e)
        {
            this.btnToolTipos.Enabled = true;
            this.btnToolDescripcion.Enabled = false;
            this.dgvElements.Columns[name.Name].Visible = false;
            this.dgvElements.Columns[descripcion.Name].Visible = true;
        }
        private void btnAislarElementos_Click(object sender, EventArgs e)
        {
            if (this.dgvElements.SelectedCells.Count > 0)
            {
                Tools.IsolateElementsFromGrid(this.dgvElements, this.btnRestablecerVista, this.btnAislarElementos
                    , this.lblPoints);
            }
            else
            {
                MessageBox.Show("Debe seleccionar una Quantificación", RevitQex.QexName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void manualDeUsoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.universobim.com/qex_manual");
        }
        private void btnRestablecerVista_Click(object sender, EventArgs e)
        {
            Tools.RestoreActualView(this.btnRestablecerVista, this.btnAislarElementos);
        }
        private void btnGuardarImagen_Click(object sender, EventArgs e)
        {
            if (this.dgvElements.SelectedCells.Count > 0)
            {
                Quantity q = this.dgvElements.SelectedCells[0].OwningRow.DataBoundItem as Quantity;
                Tools.ExportImagen(Tools._doc, q);
                Tools.UpdatePointsLabel(this.lblPoints);
            }
            else
            {
                MessageBox.Show("Debe seleccionar una Quantificación", RevitQex.QexName,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void mnuAcerca_Click(object sender, EventArgs e)
        {
            (new frmAbout()).ShowDialog();
        }
        #endregion
        #region Metodos varios
        
        
        
        #endregion

        private void btnActivateLicense_Click(object sender, EventArgs e)
        {
                
        }

        private void btnBuyLicense_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.universobim.com/producto/qex-para-revit/");
        }

        private void forumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.universobim.com/foro/forumdisplay.php?fid=3");
        }

        private void btnShowColumns_Click(object sender, EventArgs e)
        {
            this.dgvElements.Columns["count"].Visible = true;
            this.dgvElements.Columns["length"].Visible = true;
            this.dgvElements.Columns["area"].Visible = true;
            this.dgvElements.Columns["volume"].Visible = true;
            this.btnShowColumns.Enabled = false;
            this.btnHideColumns.Enabled = true;
        }

        private void btnHideColumns_Click(object sender, EventArgs e)
        {
            this.dgvElements.Columns["count"].Visible = false;
            this.dgvElements.Columns["length"].Visible = false;
            this.dgvElements.Columns["area"].Visible = false;
            this.dgvElements.Columns["volume"].Visible = false;
            this.btnShowColumns.Enabled = true;
            this.btnHideColumns.Enabled = false;
        }

        private void toolsBtnVerPerfil_Click(object sender, EventArgs e)
        {
        }

        private void statusButton_Click(object sender, EventArgs e)
        {
            //(new frmProfile()).ShowDialog();
            //Tools.UpdatePointsLabel(this.lblPoints);
        }

        private void toolBtnSeleccionarMagnitud_Click(object sender, EventArgs e)
        {
            List<Quantity> quantities = Tools.GetCheckedQuantitiesFromGrid(this.dgvElements);
            frmMultipleEdit multiple = new frmMultipleEdit(Tools._doc, quantities);
            multiple.ShowDialog();
            Tools.UpdatePointsLabel(this.lblPoints);
            Tools.GridUncheckAllRows(this.dgvElements);
            int level = this.treeViewQuantities.SelectedNode.Level;
            if (level == 1) //Folder
            {
                string folder = this.treeViewQuantities.SelectedNode.Name;
                this.txtTotalText.Text = this.treeViewQuantities.SelectedNode.Text + " >> Costo Total:";

                if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                {
                    List<Quantity> lstGlobal = RevitQex.lstElements;
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
                if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                {
                    List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.phaseCreated == folder);
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
                if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                {
                    int lvlId = (Convert.ToInt32(folder));
                    List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.LevelId == lvlId);
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
                if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                {
                    string[] cadena = Regex.Split(folder, " // ");
                    string phase = cadena[0];
                    List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.phaseCreated == phase);
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
            }
            if (level == 2) //Categories
            {
                string _category = this.treeViewQuantities.SelectedNode.Name;
                string folder = this.treeViewQuantities.SelectedNode.Parent.Name;
                this.txtTotalText.Text = "Costo: " + _category;

                if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                {
                    List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.category == _category);
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
                if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                {
                    List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.category == _category &&
                 x.phaseCreated == folder);
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
                if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                {
                    int lvlId = (Convert.ToInt32(folder));
                    List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.category == _category &&
                 x.LevelId == lvlId);
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
                if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                {
                    string[] cadena = Regex.Split(folder, " // ");
                    string phase = cadena[0];
                    int lvlId = (Convert.ToInt32(cadena[1]));
                    List<Quantity> lstGlobal = RevitQex.lstElements.FindAll(x => x.category == _category &&
                 x.phaseCreated == phase && x.LevelId == lvlId);
                    this.txtTotalCost.Text = String.Format("{0:C}", Tools.GetGlobalCost(lstGlobal));
                }
            }
            //this.Text = RevitQex.QexName + " " + RevitQex.QexVersion;
            Tools.UpdatePointsLabel(this.lblPoints);
            this.dgvElements.Update();
            this.dgvElements.Refresh();
        }

        private void toolBtnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dgvElements.Rows)
            {
                int colCheck = this.dgvElements.Columns["check"].Index;
                Tools.SetBooleanCheckToDataGridView(this.dgvElements, colCheck, row.Index, true);
            }
        }

        private void toolBtnSelectNone_Click(object sender, EventArgs e)
        {
            Tools.GridUncheckAllRows(this.dgvElements);
        }

        private void toolBtnAislarSeleccionados_Click(object sender, EventArgs e)
        {
            Tools.IsolateElementsFromSelectedInGrid(this.dgvElements, this.btnRestablecerVista,
                this.btnAislarElementos, this.lblPoints);
            Tools.GridUncheckAllRows(this.dgvElements);
            Tools.UpdatePointsLabel(this.lblPoints);
        }

        private void toolBtnGuardarImagenes_Click(object sender, EventArgs e)
        {
            Tools.ExportImages(this.dgvElements);
            Tools.GridUncheckAllRows(this.dgvElements);
            Tools.UpdatePointsLabel(this.lblPoints);
        }

        private void treeViewQuantities_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.treeViewQuantities.SelectedNode = e.Node;
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.Level == 1)
                {
                    this.mnuGrupos.Show(Cursor.Position);
                }
            }
        }

        private void mnuExportExcel_Click(object sender, EventArgs e)
        {
            string fase = this.treeViewQuantities.SelectedNode.Name;
            string faseText = this.treeViewQuantities.SelectedNode.Text;
            Tools.ExportQuantitiesToExcel(fase, faseText, RevitQex.lstElements, !this.btnToolTipos.Enabled);
            Tools.CleanTempFiles();
        }

        private void mnuGruposExportWord_Click(object sender, EventArgs e)
        {
            string fase = this.treeViewQuantities.SelectedNode.Name;
            string faseText = this.treeViewQuantities.SelectedNode.Text;
            Tools.ExportQuantitiesToWord(fase, faseText, RevitQex.lstElements, !this.btnToolTipos.Enabled);
            Tools.CleanTempFiles();
        }

        private void mnuVideosYouTube_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/playlist?list=PL-SHZ8hdVMWiLwmL6cBgAxBhxA7NbXaGR");
        }

        private void mnuBtnVerPerfil_Click(object sender, EventArgs e)
        {
            Tools.UpdatePointsLabel(this.lblPoints);
        }

        private void lblWeb_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.universobim.com");
        }

        private void ToolVerLog_Click(object sender, EventArgs e)
        {
            Log.Show();
        }

        private void btnChangeLog_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Path.Combine(App.AppDirectory, "Qex_cambios.txt"));
        }
    }
}
