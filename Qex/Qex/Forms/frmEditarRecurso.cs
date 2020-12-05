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
    public partial class frmEditarRecurso : System.Windows.Forms.Form
    {
        public Quantity _quan;
        public QexRecurso _recurso;
        public Element _elem;
        public Document _doc;

        public frmEditarRecurso(Document doc, Quantity quan, QexRecurso recurso)
        {
            InitializeComponent();
            _quan = quan;
            _recurso = recurso;
            _doc = doc;
            _elem = _doc.GetElement(new ElementId(quan.typeId));
            QexMaterial mat = recurso.GetMaterial();
            this.txtName.Text = mat.nombre;
            this.txtConsumo.Text = recurso.Consumo.ToString();
            this.txtUnidad.Text = mat.unidad;
            this.pictureBox1.BackgroundImage = Tools.ListaIconos()[mat.index];
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            double consumo = 0;
            try
            {
                consumo = Convert.ToDouble(this.txtConsumo.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Debe ingresar un número válido", "Revit",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _recurso.Consumo = consumo;
            if (_recurso.Update(_elem))
            {
                this.Close();
                //MessageBox.Show("Se guardó correctamente", "Revit", 
                //    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se puede guardar el consumo", "Revit",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void txtConsumo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                this.btnGuardar.Focus();
            }
        }
    }
}
