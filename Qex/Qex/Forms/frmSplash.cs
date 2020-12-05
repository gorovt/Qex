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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qex
{
    public partial class frmSplash : System.Windows.Forms.Form
    {

        public frmSplash(string title)
        {
            InitializeComponent();
            this.label1.Text = title;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Tools._lstWarnings = new List<string>();
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();

            List<Element> lstElem = Tools.CollectElements(Tools._doc, RevitQex._App);
            RevitQex.listaElementos = lstElem;
            int total = lstElem.Count;
            Tools._elemCount = total;
            using (var datos = new DataAccess())
            {
                dbOpenLog log = datos.GetDbOpenLogs()[datos.GetDbOpenLogs().Count - 1];
                log.ElementCount = total;
                datos.UpdateDbOpenLog(log);
            };
            int actual = 0;
            List<Quantity> lstQuantity = new List<Quantity>();

            foreach (Element elem in lstElem)
            {
                actual += 1;
                this.lblStatus.Text = "Procesando " + actual.ToString() + " elementos";
                foreach (Quantity q in Tools.CrearAllQuantities(Tools._doc, elem))
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
                int progress = 100 * actual / total;
                this.backgroundWorker1.ReportProgress(progress);
            }
            RevitQex.lstElements = lstQuantity.OrderBy(x => x.category).ThenBy(x => x.name).ToList();
            // Set the images
            foreach (Quantity q in RevitQex.lstElements)
            {
                q.image = Tools.GetImagePreviewFormQuantity(q, new Size(256,256));
            }
            // Get the elapsed time as a TimeSpan value.
            //TimeSpan ts = stopWatch.Elapsed;
            // Format and display the TimeSpan value.
            //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //    ts.Hours, ts.Minutes, ts.Seconds,
            //    ts.Milliseconds / 10);
            //MessageBox.Show("Tiempo total: " + elapsedTime);
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Tools._lstWarnings.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in Tools._lstWarnings)
                {
                    sb.AppendLine(item);
                }
                (new frmWarnings("Lista de Errores", "Se muestra una lista con todos los elementos de Revit que "
                    + "no pudieron computarse", sb.ToString())).ShowDialog();
            }
            Tools.AddPoint(RevitQex.listaElementos.Count / 100);
            this.Close();
            this.Dispose();
        }
        private void frmSplash_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage;
        }

        private void mainProcess()
        {
            Tools._lstWarnings = new List<string>();

            List<Element> lstElem = Tools.CollectElements(Tools._doc, RevitQex._App);
            RevitQex.listaElementos = lstElem;
            int total = lstElem.Count;
            Tools._elemCount = total;
            using (var datos = new DataAccess())
            {
                dbOpenLog log = datos.GetDbOpenLogs()[datos.GetDbOpenLogs().Count - 1];
                log.ElementCount = total;
                datos.UpdateDbOpenLog(log);
            };
            int actual = 0;
            List<Quantity> lstQuantity = new List<Quantity>();

            foreach (Element elem in lstElem)
            {
                actual += 1;
                this.lblStatus.Text = "Procesando " + actual.ToString() + " elementos";
                foreach (Quantity q in Tools.CrearAllQuantities(Tools._doc, elem))
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
                int progress = 100 * actual / total;
            }
            RevitQex.lstElements = lstQuantity.OrderBy(x => x.category).ThenBy(x => x.name).ToList();
            // Set the images
            foreach (Quantity q in RevitQex.lstElements)
            {
                q.image = Tools.GetImagePreviewFormQuantity(q, new Size(256, 256));
            }
            this.Close();
        }
    }
}
