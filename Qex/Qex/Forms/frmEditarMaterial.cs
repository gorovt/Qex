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
    public partial class frmEditarMaterial : Form
    {
        public QexMaterial _material;
        public QexGrupoMaterial _grupo;
        public bool _nuevo = true;
        public int _index = 0;

        // Nuevo Material
        public frmEditarMaterial(string grupoId) 
        {
            InitializeComponent();
            _grupo = DalRecursos.GetGrupoMaterialById(grupoId);
            List<Image> imagenes = Tools.ListaIconos();
            this.pictureBox1.BackgroundImage = imagenes[0];
            Tools.FillComboGruposMaterial(this.cmbGrupo, _grupo);
            this.lblTitulo.Text = "Nuevo Recurso";
        }

        // Editar Material
        public frmEditarMaterial(QexMaterial material) 
        {
            InitializeComponent();
            _nuevo = false;
            _material = material;
            this.txtNombre.Text = material.nombre;
            this.txtUnidad.Text = material.unidad;
            this.txtPrecio.Text = material.precio.ToString();
            _grupo = DalRecursos.GetGrupoMaterialById(material.grupoId);
            List<Image> imagenes = Tools.ListaIconos();
            this.pictureBox1.BackgroundImage = imagenes[material.index];
            _index = material.index;
            Tools.FillComboGruposMaterial(this.cmbGrupo, _grupo);
            this.lblTitulo.Text = "Editar Recurso";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validaciones
            if (this.txtNombre.Text == "")
            {
                MessageBox.Show("Debe escribir un Nombre para el recurso", "Revit", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                this.txtNombre.Focus();
                return;
            }
            // Todo Ok, procedemos
            string nombre = this.txtNombre.Text;
            string unidad = this.txtUnidad.Text;
            double precio = 0;
            int indexIcono = _index;
            if (this.txtPrecio.Text != string.Empty)
            {
                try
                {
                    precio = Convert.ToDouble(this.txtPrecio.Text);
                }
                catch (Exception)
                {

                }
            }
            string id = this.cmbGrupo.SelectedValue.ToString();
            QexGrupoMaterial grupo = DalRecursos.GetGrupoMaterialById(id);
            _grupo = grupo;
            // Nuevo Material
            if (_nuevo)
            {
                
                QexMaterial material = new QexMaterial(Guid.NewGuid().ToString(), nombre, unidad, _grupo.id, precio, indexIcono);
                if (material.Insert())
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se puede editar el Material", "Revit",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // Editar Material
            if (!_nuevo)
            {
                _material.nombre = nombre;
                _material.unidad = unidad;
                _material.precio = precio;
                _material.grupoId = _grupo.id;
                _material.index = indexIcono;
                if (_material.Update())
                {

                }
                else
                {
                    MessageBox.Show("No se puede guardar el Material", "Revit",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                this.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmSelectIcon icon = new frmSelectIcon(_index);
            icon.ShowDialog();
            _index = icon._index;
            List<Image> imagenes = Tools.ListaIconos();
            this.pictureBox1.BackgroundImage = imagenes[_index];
        }

        private void cmbGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = this.cmbGrupo.SelectedValue.ToString();
            QexGrupoMaterial grupo = DalRecursos.GetGrupoMaterialById(id);
            _grupo = grupo;
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                this.txtUnidad.Focus();
            }
        }

        private void txtUnidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                this.txtPrecio.Focus();
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                this.btnGuardar.Focus();
            }
        }
    }
}
