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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.ExtensibleStorage;
//using MaterialSkin;

namespace Qex
{
    public partial class frmEditar : System.Windows.Forms.Form
    {
        public Quantity _q;
        public Document _doc;

        public frmEditar(Document doc, Quantity q)
        {
            InitializeComponent();
            
            _q = q;
            _doc = doc;
            //Rellenar valores
            this.txtName.Text = q.name;
            this.txtCost.Text = q.cost.ToString();
            this.txtDescripcion.Text = q.descripcion;
            this.txtCount.Text = q.count.ToString() + " ud";
            this.txtLength.Text = q.length.ToString() + " ml";
            this.txtArea.Text = q.area.ToString() + " m2";
            this.txtVolume.Text = q.volume.ToString() + " m3";
            this.pic1.BackgroundImage = q.image;
            fillComboParam();

            if (q.matriz == "1000")
            {
                rdbCant.Checked = true;
                txtCount.BackColor = System.Drawing.Color.LightGreen;
            }
            if (q.matriz == "0100")
            {
                rdbLargo.Checked = true;
                txtLength.BackColor = System.Drawing.Color.LightGreen;
            }
            if (q.matriz == "0010")
            {
                rdbArea.Checked = true;
                txtArea.BackColor = System.Drawing.Color.LightGreen;
            }
            if (q.matriz == "0001")
            {
                rdbVolumen.Checked = true;
                txtVolume.BackColor = System.Drawing.Color.LightGreen;
            }
            if (q.matriz == "1111")
            {
                rdbParameter.Checked = true;
                txtParameter.BackColor = System.Drawing.Color.LightGreen;
            }
            //Validar selecciones
            if (q.length == 0)
                this.rdbLargo.Enabled = false;
            if (q.area == 0)
                this.rdbArea.Enabled = false;
            if (q.volume == 0)
                this.rdbVolumen.Enabled = false;
        }

        private void fillComboParam()
        {
            Dictionary<int, string> comboSource = new Dictionary<int, string>();

            foreach (Qparameter param in _q.lstParameters)
            {
                comboSource.Add(param.id, param.nombre);
            }
            
            if (_q.lstParameters.Count > 0)
            {
                this.cmbParametros.DataSource = new BindingSource(comboSource, null);
                this.cmbParametros.DisplayMember = "Value";
                this.cmbParametros.ValueMember = "Key";
                // ¿El ID de parametro guardado existe?
                if (_q.lstParameters.Exists(x => x.id == _q.paramId))
                {
                    Qparameter param = _q.lstParameters.FirstOrDefault(x => x.id == _q.paramId);
                    this.cmbParametros.SelectedValue = param.id;
                    this.txtParameter.Text = param.valor + " " + param.unidad;
                }
                // No existe el ID guardado
                else
                {
                    this.cmbParametros.SelectedValue = _q.lstParameters[0].id;
                    Qparameter param = _q.lstParameters[0];
                    this.txtParameter.Text = param.valor + " " + param.unidad;
                }
            }
            else
            {
                this.cmbParametros.Enabled = false;
                this.rdbParameter.Enabled = false;
            }
        }

        public Element getElement(int typeId, Document doc)
        {
            ElementId id = new ElementId(typeId);
            Element elem = doc.GetElement(id);
            return elem;
        }
        
        #region Checked changes
        private void rdbCant_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCant.Checked)
                txtCount.BackColor = System.Drawing.Color.LightGreen;
            if (!rdbCant.Checked)
                txtCount.BackColor = System.Drawing.Color.White;
        }
        private void rdbLargo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbLargo.Checked)
                txtLength.BackColor = System.Drawing.Color.LightGreen;
            if (!rdbLargo.Checked)
                txtLength.BackColor = System.Drawing.Color.White;
        }
        private void rdbArea_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbArea.Checked)
                txtArea.BackColor = System.Drawing.Color.LightGreen;
            if (!rdbArea.Checked)
                txtArea.BackColor = System.Drawing.Color.White;
        }
        private void rdbVolumen_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbVolumen.Checked)
                txtVolume.BackColor = System.Drawing.Color.LightGreen;
            if (!rdbVolumen.Checked)
                txtVolume.BackColor = System.Drawing.Color.White;
        }
        private void rdbParameter_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbParameter.Checked)
            {
                txtParameter.BackColor = System.Drawing.Color.LightGreen;
                this.cmbParametros.Enabled = true;
            }
            if (!rdbParameter.Checked)
            {
                txtParameter.BackColor = System.Drawing.Color.White;
                this.cmbParametros.Enabled = false;
            }
        }
        #endregion
        private void btnOk_Click(object sender, EventArgs e)
        {
            /* EDITAR QUANTITY
         * 1) Recolectar info del formulario
         * 2) Modificar el objeto Quantity
         * 3) Actualizar el Elemento de Revit (parametros y schema)
         * 4) Actualizar la lista de Quantities
         */
            try
            {
                //1) Recolectar info del formulario
                string descripcion = this.txtDescripcion.Text.ToString();
                double cost = Convert.ToDouble(this.txtCost.Text.ToString());
                string matriz = "";
                bool visible = _q.visible;
                if (this.rdbCant.Checked)
                    matriz = "1000";
                if (this.rdbLargo.Checked)
                    matriz = "0100";
                if (this.rdbArea.Checked)
                    matriz = "0010";
                if (this.rdbVolumen.Checked)
                    matriz = "0001";
                if (this.rdbParameter.Checked)
                {
                    matriz = "1111";
                    int paramId = ((KeyValuePair<int, string>)this.cmbParametros.SelectedItem).Key;
                    Qparameter param = _q.lstParameters.FirstOrDefault(x => x.id == paramId);
                    _q.value = param.valor;
                    _q.unidad = param.unidad;
                    _q.paramId = param.id;
                }

                //2) Modificar el objeto Quantity y 4) Actualizar lista de Quantities
                actualizarQuantity(_q.qId, cost, descripcion, matriz, _q.grupo, _q.value, _q.unidad);

                //3) Actualizar el elemento de Revit (Parametros y schema)
                Tools.SetCostById(_q.typeId, cost);
                Tools.SetDescriptionById(_q.typeId, descripcion);
                if (QexSchema.SchemaExist(QexSchema._schemaQex))
                {
                    Schema Qschema = QexSchema.GetSchemaByName(QexSchema._schemaQex);
                    QexSchema.CreateFields(_doc, Qschema, _q.typeId, cost, matriz, visible, 
                        _q.paramId.ToString(), "");
                }
                if (!QexSchema.SchemaExist(QexSchema._schemaQex))
                {
                    if (QexSchema.CreateSchemaQex(_doc))
                    {
                        Schema qschema = QexSchema.GetSchemaByName(QexSchema._schemaQex);
                        QexSchema.CreateFields(_doc, qschema, _q.typeId, cost, matriz, visible,
                            _q.paramId.ToString(), "");
                    }
                    else
                    {
                        MessageBox.Show("No se puede crear el Schema", RevitQex.QexName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                Tools.AddPoint(0.1);
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Error:");
                sb.AppendLine(ex.Message);
                MessageBox.Show(sb.ToString(), RevitQex.QexName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.Close();
        }

        private void actualizarQuantity(string qId, double cost, string descripcion, string matriz, string grupo,
            double value, string unidad)
        {
            Quantity q = RevitQex.lstElements.Find(x => x.qId == qId);
            List<Quantity> lstQ = RevitQex.lstElements.FindAll(x => x.typeId == q.typeId);
            foreach (Quantity qu in lstQ)
            {
                qu.cost = cost;
                qu.descripcion = descripcion;
                double totalCost = Tools.CalculateTotalCost(qu.count, qu.length, qu.area, qu.volume, cost, 
                    matriz, value);
                qu.totalCost = totalCost;
                string medicion = Tools.CalculateMedicion(qu.count, qu.length, qu.area, qu.volume, matriz,
                    value, unidad);
                qu.medicion = medicion;
                qu.matriz = matriz;
                qu.grupo = grupo;
                qu.value = value;
                qu.unidad = unidad;
            }
        }

        private void txtCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void pic1_Click(object sender, EventArgs e)
        {
            (new frmImagePreview(_q.image)).ShowDialog();
        }

        private void mnuSaveImage_Click(object sender, EventArgs e)
        {
            //Exporting to Excel
            string folderPath = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Archivos PNG|*.png";
            string title = Tools._doc.Title;
            string name = _q.name.Replace(':', '-');
            sfd.FileName = name + ".png";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderPath = sfd.FileName;
                try
                {
                    this.pic1.BackgroundImage.Save(folderPath, System.Drawing.Imaging.ImageFormat.Png);
                    MessageBox.Show("Imagen guardada correctamente", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void cmbParametros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_q.lstParameters.Count > 0)
            {
                int paramId = ((KeyValuePair<int, string>)this.cmbParametros.SelectedItem).Key;
                Qparameter param = _q.lstParameters.FirstOrDefault(x => x.id == paramId);
                this.txtParameter.Text = param.valor + " " + param.unidad;
            }
        }

        private void btnRecursos_Click(object sender, EventArgs e)
        {
            (new frmRecursosMaterial(_doc, _q)).ShowDialog();
        }
    }
}
