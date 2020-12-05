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
using rvt=Autodesk.Revit.DB;

namespace Qex
{
    public partial class frmBackDoor : Form
    {
        public rvt.Document _doc;
        public Autodesk.Revit.ApplicationServices.Application _app;

        public frmBackDoor(rvt.Document doc, Autodesk.Revit.ApplicationServices.Application app)
        {
            InitializeComponent();
            _doc = doc;
            _app = app;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn01_Click(object sender, EventArgs e)
        {
            this.txtPanel.Text = string.Empty;
            List<string> categorias = new List<string>();
            List<string> categoriasCount = new List<string>();
            List<rvt.Element> elements = Tools.CollectElements(_doc, _app);
            elements = elements.OrderBy(x => x.Category.Name).ThenBy(x => x.Name).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (rvt.Element elem in elements)
            {
                categoriasCount.Add(elem.Category.Name);
                if (!categorias.Exists(x => x == elem.Category.Name))
                {
                    categorias.Add(elem.Category.Name);
                }
            }
            sb.AppendLine("Categorias: (" + categorias.Count.ToString() + ")");
            foreach (var item in categorias)
            {
                int count = categoriasCount.Count(x => x == item);
                sb.AppendLine(item + " (" + count.ToString() + " elementos)");
            }
            sb.AppendLine("");
            sb.AppendLine("Elementos:");
            foreach (rvt.Element elem in elements)
            {
                sb.AppendLine(elem.Category.Name + " | " + elem.Name);
            }
            this.txtPanel.Text = sb.ToString();
        }

        private void Btn02_Click(object sender, EventArgs e)
        {
            this.txtPanel.Text = string.Empty;
            List<string> categorias = new List<string>();
            List<string> categoriasCount = new List<string>();
            List<rvt.Element> elements = Tools.CollectTopography(_doc, _app);
            elements = elements.OrderBy(x => x.Category.Name).ThenBy(x => x.Name).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (rvt.Element elem in elements)
            {
                categoriasCount.Add(elem.Category.Name);
                if (!categorias.Exists(x => x == elem.Category.Name))
                {
                    categorias.Add(elem.Category.Name);
                }
            }
            sb.AppendLine("Categorias: (" + categorias.Count.ToString() + ")");
            foreach (var item in categorias)
            {
                int count = categoriasCount.Count(x => x == item);
                sb.AppendLine(item + " (" + count.ToString() + " elementos)");
            }
            sb.AppendLine("");
            sb.AppendLine("Elementos:");
            foreach (rvt.Element elem in elements)
            {
                List<Quantity> qs = Tools.CrearQuantityFromTopography(_doc, elem);
                foreach (Quantity q in qs)
                {
                    sb.AppendLine(q.category + " | " + q.name);
                }
            }
            this.txtPanel.Text = sb.ToString();
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            this.txtPanel.Text = string.Empty;
            Tools._lstWarnings = new List<string>();
            List<Quantity> lstQuantity = new List<Quantity>();
            List<rvt.Element> elements = Tools.CollectElements(_doc, _app);
            foreach (rvt.Element elem in elements)
            {
                List<Quantity> qs = Tools.CrearAllQuantities(_doc, elem);
                foreach (Quantity q in qs)
                {
                    if (!lstQuantity.Exists(x => x.qId == q.qId))
                    {
                        lstQuantity.Add(q);
                    }
                    else
                    {
                        Quantity q0 = lstQuantity.Find(x => x.qId == q.qId);
                        q0.count = q0.count + q.count;
                        q0.length = q0.length + q.length;
                        q0.area = q0.area + q.area;
                        q0.volume = q0.volume + q.volume;
                        foreach (Qparameter qparam in q.lstParameters)
                        {
                            // No existe el parametro en la Quantificación existente
                            if (!q0.lstParameters.Exists(x => x.id == qparam.id))
                            {
                                // Agregar el Qparam
                                q0.lstParameters.Add(qparam);
                            }
                            // Si existe el parámetro en la Quantificacion existente
                            else
                            {
                                Qparameter q0Param = q0.lstParameters.FirstOrDefault(x => x.id == qparam.id);
                                q0Param.valor += qparam.valor;
                            }
                        }
                        q0.value = q0.value + q.value;
                        q0.totalCost = q0.totalCost + q.totalCost;
                        q0.medicion = Tools.CalculateMedicion(q0.count, q0.length, q0.area, q0.volume,
                            q0.matriz, q0.value, q0.unidad);
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Quantities:");
            foreach (Quantity q in lstQuantity)
            {
                sb.AppendLine(q.name + " | " + q.image.ToString());
            }
            this.txtPanel.Text = sb.ToString();
        }

        private void Btn4_Click(object sender, EventArgs e)
        {
            this.txtPanel.Text = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("App Path: ");
            sb.AppendLine(App.AppDirectory);
            this.txtPanel.Text = sb.ToString();
        }
    }
}
