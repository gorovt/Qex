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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qex
{
    public partial class frmMultipleEdit : System.Windows.Forms.Form
    {
        public Document _doc;
        public List<Quantity> _quantities;

        public frmMultipleEdit(Document doc, List<Quantity> quantities)
        {
            InitializeComponent();
            _doc = doc;
            _quantities = quantities;
        }

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

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
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
                string matriz = "";
                if (this.rdbCant.Checked)
                    matriz = "1000";
                if (this.rdbLargo.Checked)
                    matriz = "0100";
                if (this.rdbArea.Checked)
                    matriz = "0010";
                if (this.rdbVolumen.Checked)
                    matriz = "0001";

                //2) Modificar los objetos Quantity y 4) Actualizar lista de Quantities
                foreach (Quantity q in _quantities)
                {
                    if (q.area != 0 || q.volume != 0)
                    {

                    }
                    Tools.ActualizarQuantity(q.qId, q.cost, q.descripcion, matriz, q.grupo, q.value, q.unidad
                        , q.qOrden, q.grupoOrden, q.libroOrden);
                }

                //3) Actualizar los elementos de Revit (Parametros y schema)
                
                foreach (Quantity _q in _quantities)
                {
                    if (QexSchema.SchemaExist(QexSchema._schemaQex))
                    {
                        Schema Qschema = QexSchema.GetSchemaByName(QexSchema._schemaQex);
                        QexSchema.CreateFields(_doc, Qschema, _q.typeId, _q.cost, matriz, _q.visible,
                            _q.paramId.ToString(), "");
                    }
                    if (!QexSchema.SchemaExist(QexSchema._schemaQex))
                    {
                        if (QexSchema.CreateSchemaQex(_doc))
                        {
                            Schema qschema = QexSchema.GetSchemaByName(QexSchema._schemaQex);
                            QexSchema.CreateFields(_doc, qschema, _q.typeId, _q.cost, matriz, _q.visible,
                                _q.paramId.ToString(), "");
                        }
                        else
                        {
                            MessageBox.Show("No se puede crear el Schema", RevitQex.QexName,
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                Tools.AddPoint(_quantities.Count * 0.10);
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
    }
}
