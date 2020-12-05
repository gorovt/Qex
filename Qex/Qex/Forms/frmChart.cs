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
using System.Windows.Forms.DataVisualization.Charting;

namespace Qex
{
    public partial class frmChart : Form
    {
        public frmChart(Chart chart, string title)
        {
            InitializeComponent();
            this.Text = title;
            this.chart1.Series.Clear();
            this.chart1.ChartAreas.Clear();
            this.chart1.Series.Add(chart.Series[0]);
            this.chart1.ChartAreas.Add(chart.ChartAreas[0]);
            this.chart1.Titles.Add(chart.Titles[0]);
            //this.chart1.Series[0].IsValueShownAsLabel = false;
            this.chart1.Series[0].ToolTip = "#VALX" + " (" + "#VAL" + ")";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Imagen|*.jpg";
            sfd.FileName = this.Text + ".jpg";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    filePath = sfd.FileName;
                    this.chart1.SaveImage(filePath, ChartImageFormat.Jpeg);
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
    }
}
