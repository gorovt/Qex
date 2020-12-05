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
    public partial class frmEditarGrupo : Form
    {
        public QexGrupoMaterial _grupo;
        public bool _nuevo = true;

        // Nuevo Grupo
        public frmEditarGrupo()
        {
            InitializeComponent();
            _nuevo = true;
            this.lblTitulo.Text = "Nuevo Grupo";
            this.btnGuardar.Text = "Crear";
        }

        // Editar Grupo
        public frmEditarGrupo(QexGrupoMaterial grupo)
        {
            InitializeComponent();
            _nuevo = false;
            this.lblTitulo.Text = "Editar Grupo";
            this.btnGuardar.Text = "Guardar";
            this.txtNombre.Text = grupo.Nombre;
            _grupo = grupo;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validaciones
            if (this.txtNombre.Text == "")
            {
                MessageBox.Show("Debe ingresar un nombre de Grupo", RevitQex.QexName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtNombre.Focus();
                return;
            }
            string nombre = this.txtNombre.Text;
            // NUevo Grupo
            if (_nuevo)
            {
                QexGrupoMaterial grupo = new QexGrupoMaterial();
                grupo.Nombre = nombre;
                if (grupo.Insert())
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se puede crear el Grupo", RevitQex.QexName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            // Editar Grupo
            if (!_nuevo)
            {
                _grupo.Nombre = nombre;
                if (_grupo.Update())
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se puede guardar el Grupo", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                this.btnGuardar.Focus();
            }
        }
    }
}
