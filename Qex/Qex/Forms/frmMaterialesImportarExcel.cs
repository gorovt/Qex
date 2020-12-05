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
    public partial class frmMaterialesImportarExcel : Form
    {
        public static List<RecursoImportadoExcel> _lst;
        public static string _status = "";
        public static string _category = "";
        public int _index = 0;

        public frmMaterialesImportarExcel()
        {
            InitializeComponent();
            fillComboColumns();
            fillComboRanges();
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "Archivo Excel (*.xlsx)|*.xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string path = sfd.FileName;
                    this.txtPath.Text = path;
                    fillComboSheets();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: \n" + ex.Message, RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }
        private void fillComboSheets()
        {
            cmbSheets.Items.Clear();
            foreach (string s in Tools.GetSheetsFromExcel(this.txtPath.Text))
            {
                cmbSheets.Items.Add(s);
            }
            cmbSheets.SelectedIndex = 0;
        }
        private void fillComboColumns()
        {
            List<string> aToZ = Enumerable.Range('A', 26)
                              .Select(x => ((char)x).ToString())
                              .ToList();
            foreach (string s in aToZ)
            {
                cmbGroup.Items.Add(s);
                cmbName.Items.Add(s);
                cmbUnit.Items.Add(s);
                cmbCost.Items.Add(s);
                cmbIcono.Items.Add(s);
            }
            cmbGroup.SelectedIndex = 0;
            cmbName.SelectedIndex = 1;
            cmbCost.SelectedIndex = 2;
            cmbUnit.SelectedIndex = 3;
            cmbIcono.SelectedIndex = 4;
        }
        private void fillComboRanges()
        {
            List<string> numbers = Enumerable.Range(1, 10000)
                                    .Select(x => x.ToString())
                                    .ToList();
            foreach (string s in numbers)
            {
                cmbFrom.Items.Add(s);
                //cmbTo.Items.Add(s);
            }
            cmbFrom.SelectedIndex = 1;
            //cmbTo.SelectedIndex = 9999;
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (this.txtPath.Text == "")
            {
                MessageBox.Show("Selecciona un archivo de Excel", RevitQex.QexName,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnOpen.Focus();
                return;
            }
            if (this.cmbSheets.SelectedItem == null)
            {
                MessageBox.Show("Selecciona una Hoja de cálculo", RevitQex.QexName,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.cmbSheets.Focus();
                return;
            }
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                int groupCol = cmbGroup.SelectedIndex + 1;
                int nameCol = cmbName.SelectedIndex + 1;
                int costCol = cmbCost.SelectedIndex + 1;
                int unitCol = cmbUnit.SelectedIndex + 1;
                int indexCol = cmbIcono.SelectedIndex + 1;
                int from = cmbFrom.SelectedIndex + 1;
                int to = 19999 + 1;

                // Iconos
                if (!this.chkIcono.Checked)
                {
                    indexCol = 0;
                }
                List<RecursoImportadoExcel> lst = Tools.ImportRecursosFromExcel(this.txtPath.Text,
                    this.cmbSheets.SelectedItem.ToString(), groupCol, nameCol, unitCol, costCol, from, to, indexCol);
                this.dgvPreview.DataBindings.Clear();
                this.dgvPreview.DataSource = lst;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: \n" + ex.Message, RevitQex.QexName,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (this.txtPath.Text == "")
            {
                MessageBox.Show("Selecciona un archivo de Excel", RevitQex.QexName,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnOpen.Focus();
                return;
            }
            if (this.cmbSheets.SelectedItem == null)
            {
                MessageBox.Show("Selecciona una Hoja de cálculo", RevitQex.QexName,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.cmbSheets.Focus();
                return;
            }
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                int groupCol = cmbGroup.SelectedIndex + 1;
                int nameCol = cmbName.SelectedIndex + 1;
                int unitCol = cmbUnit.SelectedIndex + 1;
                int costCol = cmbCost.SelectedIndex + 1;
                int indexCol = cmbIcono.SelectedIndex + 1;
                int from = cmbFrom.SelectedIndex + 1;
                int to = 19999 + 1;

                // Iconos
                if (!this.chkIcono.Checked)
                {
                    indexCol = 0;
                }

                _lst = Tools.ImportRecursosFromExcel(this.txtPath.Text,
                    this.cmbSheets.SelectedItem.ToString(), groupCol, nameCol, unitCol, costCol, from, to, indexCol);

                this.groupBox1.Enabled = false;
                this.groupBox2.Enabled = false;
                this.btnImport.Enabled = false;
                Procesar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: \n" + ex.Message, RevitQex.QexName,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Procesar()
        {
            int progress = 1;
            int count = 1;

            foreach (RecursoImportadoExcel rec in _lst)
            {
                //Grupos
                QexGrupoMaterial grupo0;
                List<QexGrupoMaterial> grupos = DalRecursos.ReadGrupos();
                if (!grupos.Exists(xx => xx.Nombre == rec.Group))
                {
                    QexGrupoMaterial grupo = new QexGrupoMaterial();
                    grupo.Nombre = rec.Group;
                    grupo.Insert();
                    grupo0 = DalRecursos.GetLastGrupo();
                }
                else
                {
                    grupo0 = grupos.FirstOrDefault(xx => xx.Nombre == rec.Group);
                }
                //Recursos
                QexMaterial rec0 = new QexMaterial();
                rec0.grupoId = grupo0.id;
                rec0.nombre = rec.Name;
                rec0.unidad = rec.Unit;
                rec0.precio = rec.Cost;
                rec0.index = rec.Index;


                List<QexMaterial> materiales = DalRecursos.ReadMateriales();
                if (!materiales.Exists(xx => xx.nombre == rec.Name))
                {
                    rec0.Insert();
                }
                else
                {
                    QexMaterial rec00 = materiales.FirstOrDefault(x => x.nombre == rec.Name);
                    rec0.id = rec00.id;
                    //rec0.index = rec00.index;
                    rec0.Update();
                }
                progress = 100 * count / _lst.Count;
                _status = "Procesando " + count + "/" + _lst.Count + " elementos";
                count++;

            }
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;
            int total = _lst.Count;
            MessageBox.Show(total.ToString() + " Recursos importados correctamente", RevitQex.QexName,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;
            int total = _lst.Count;
            MessageBox.Show(total.ToString() + " Recursos importados correctamente", RevitQex.QexName,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            (new frmTutorial("Importar Recursos",
                "Las columnas de la Tabla de Excel deben ser como la siguiente imagen de ejemplo",
                Resource1.ImportMaterialesExcel2)).ShowDialog();
        }

        private void chkIcono_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkIcono.Checked)
            {
                this.cmbIcono.Enabled = true;
            }
            else
            {
                this.cmbIcono.Enabled = false;
            }
        }
    }
}
