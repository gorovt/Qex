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
using Autodesk.Revit.ApplicationServices;

namespace Qex
{
    public partial class frmOpciones : System.Windows.Forms.Form
    {
        public bool _wall = true;
        public bool _floor = true;
        public bool _roof = true;
        public bool _ceiling = true;
        public bool _foundation = true;
        public bool _rebarMarca = false;
        public bool _pad = false;
        public bool _phases = false;
        public bool _levels = false;
        public bool _grupos = false;
        public bool _montajes = false;

        public Autodesk.Revit.ApplicationServices.Application _appGral;
        public Document _doc;
        public ExternalCommandData _commandData;

        public frmOpciones()
        {
            InitializeComponent();
            _appGral = RevitQex._App;
            _doc = Tools._doc;
            _commandData = RevitQex._command;

            //Rellenar ComboBox
            cmbMuros.Items.Add("Por Tipo");
            cmbMuros.Items.Add("Por Materiales");
            if (QexOpciones.wallsMaterials)
                cmbMuros.SelectedItem = "Por Materiales";
            if (!QexOpciones.wallsMaterials)
                cmbMuros.SelectedItem = "Por Tipo";

            cmbSuelos.Items.Add("Por Tipo");
            cmbSuelos.Items.Add("Por Materiales");
            if (QexOpciones.floorMaterials)
                cmbSuelos.SelectedItem = "Por Materiales";
            if (!QexOpciones.floorMaterials)
                cmbSuelos.SelectedItem = "Por Tipo";

            cmbCubiertas.Items.Add("Por Tipo");
            cmbCubiertas.Items.Add("Por Materiales");
            if (QexOpciones.roofMaterials)
                cmbCubiertas.SelectedItem = "Por Materiales";
            if (!QexOpciones.roofMaterials)
                cmbCubiertas.SelectedItem = "Por Tipo";

            cmbTechos.Items.Add("Por Tipo");
            cmbTechos.Items.Add("Por Materiales");
            if (QexOpciones.ceilingMaterials)
                cmbTechos.SelectedItem = "Por Materiales";
            if (!QexOpciones.ceilingMaterials)
                cmbTechos.SelectedItem = "Por Tipo";

            cmbFoundations.Items.Add("Por Tipo");
            cmbFoundations.Items.Add("Por Materiales");
            if (QexOpciones.strFoundationsFloorMaterials)
                cmbFoundations.SelectedItem = "Por Materiales";
            if (!QexOpciones.strFoundationsFloorMaterials)
                cmbFoundations.SelectedItem = "Por Tipo";

            cmbRebar.Items.Add("Por Tipo");
            cmbRebar.Items.Add("Por Marca de Anfitrion");
            if (QexOpciones.rebarMarca)
                cmbRebar.SelectedItem = "Por Marca de Anfitrion";
            if (!QexOpciones.rebarMarca)
                cmbRebar.SelectedItem = "Por Tipo";

            cmbPads.Items.Add("Por Tipo");
            cmbPads.Items.Add("Por Materiales");
            if (QexOpciones.padsMaterials)
                cmbPads.SelectedItem = "Por Materiales";
            if (!QexOpciones.padsMaterials)
                cmbPads.SelectedItem = "Por Tipo";
            if (QexOpciones.QuantityByPhase)
                this.chkByPhase.Checked = true;
            if (QexOpciones.QuantityByLevel)
                this.chkByLevel.Checked = true;
            if (QexOpciones.QuantityByGroup)
                this.chkGrupos.Checked = true;
            if (QexOpciones.QuantityByAssembly)
                this.chkMontajes.Checked = true;
        }

        private void cmbMuros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbMuros.SelectedItem.ToString() == "Por Tipo")
                _wall = false;
            else
                _wall = true;
        }
        private void cmbSuelos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbSuelos.SelectedItem.ToString() == "Por Tipo")
                _floor = false;
            else
                _floor = true;
        }
        private void cmbCubiertas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbCubiertas.SelectedItem.ToString() == "Por Tipo")
                _roof = false;
            else
                _roof = true;
        }
        private void cmbTechos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbTechos.SelectedItem.ToString() == "Por Tipo")
                _ceiling = false;
            else
                _ceiling = true;
        }
        private void cmbFoundations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbFoundations.SelectedItem.ToString() == "Por Tipo")
                _foundation = false;
            else
                _foundation = true;
        }
        private void cmbRebar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbRebar.SelectedItem.ToString() == "Por Tipo")
                _rebarMarca = false;
            else
                _rebarMarca = true;
        }
        private void cmbPads_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbPads.SelectedItem.ToString() == "Por Tipo")
                _pad = false;
            else
                _pad = true;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            QexOpciones.wallsMaterials = _wall;
            QexOpciones.floorMaterials = _floor;
            QexOpciones.roofMaterials = _roof;
            QexOpciones.ceilingMaterials = _ceiling;
            QexOpciones.strFoundationsFloorMaterials = _foundation;
            QexOpciones.rebarMarca = _rebarMarca;
            QexOpciones.padsMaterials = _pad;
            QexOpciones.QuantityByPhase = _phases;
            QexOpciones.QuantityByLevel = _levels;
            QexOpciones.QuantityByGroup = _grupos;
            QexOpciones.QuantityByAssembly = _montajes;

            Tools.SetOptions(Tools.StringOptions());
            QexSchema.CrearEntidadQexOptions(Tools._doc, Tools.StringOptions());
            RevitQex.lstElements.Clear();
            this.DialogResult = DialogResult.OK;
            this.Close();
            
        }

        private void chkByPhase_CheckedChanged(object sender, EventArgs e)
        {
            _phases = this.chkByPhase.Checked;
        }

        private void chkByLevel_CheckedChanged(object sender, EventArgs e)
        {
            _levels = this.chkByLevel.Checked;
        }

        private void chkGrupos_CheckedChanged(object sender, EventArgs e)
        {
            _grupos = this.chkGrupos.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _montajes = this.chkMontajes.Checked;
        }

        private void btnAyudaMontajes_Click(object sender, EventArgs e)
        {
            TaskDialog.Show(RevitQex.QexName, "Cuando se consideran los Montajes, los elementos que forman el" +
                " Montaje NO se incluyen en el Cómputo general.");
        }

        private void frmOpciones_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.DialogResult = DialogResult.Cancel;
        }
    }
}
