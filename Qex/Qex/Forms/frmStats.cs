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
using System.Windows.Forms.DataVisualization.Charting;

namespace Qex
{
    public partial class frmStats : System.Windows.Forms.Form
    {
        public static int _elemCount;
        public static int _openedCount;
        public static int _processedElements;
        public static double _puntos;

        public frmStats()
        {
            InitializeComponent();
            _elemCount = Tools._elemCount;
            _openedCount = Tools.getOpenedCount();
            _processedElements = Tools.getProcessedElements();
            _puntos = Tools.GetPoints();

            //fillGoals();
            fillProgressBar();
            //fillCharts();
            fillDataGridActualStats();
            fillDataGridGlobalStats();
            fillDataGridLogros();
            fillDataGridGraficos();
        }

        private void fillDataGridGlobalStats()
        {
            Autodesk.Revit.DB.Document doc = Tools._doc;
            List<ItemStat> lst = new List<ItemStat>();
            // Usos de Qex
            ItemStat item1 = new ItemStat("Usos de Qex", _openedCount.ToString() +
                " usos", Resource1.Qex_32x32);
            lst.Add(item1);
            // Elementos procesados
            ItemStat item2 = new ItemStat("Elementos procesados histórico", _processedElements.ToString() +
                " Elementos", Resource1.Ribbon_Shaded_32);
            lst.Add(item2);

            this.dgvGlobalStats.DataBindings.Clear();
            this.dgvGlobalStats.DataSource = lst;
        }

        private void fillDataGridLogros()
        {
            List<ItemStat> lst = new List<ItemStat>();
            // Logros del usuario
            ItemStat item1 = new ItemStat("1,000 Puntos", "Bloqueado", Resource1.crown);
            ItemStat item2 = new ItemStat("2,500 Puntos", "Bloqueado", Resource1.crown);
            ItemStat item3 = new ItemStat("10,000 Puntos", "Bloqueado", Resource1.crown);
            
            if (_processedElements > 249999)
            {
                item1.value = "Usuario Avanzado";
            }
            if (_processedElements > 499999)
            {
                item2.value = "Usuario Experto";
            }
            if (_processedElements > 999999)
            {
                item3.value = "Usuario Jedi";
            }

            lst.Add(item1);
            lst.Add(item2);
            lst.Add(item3);

            this.dgvLogros.DataBindings.Clear();
            this.dgvLogros.DataSource = lst;
        }

        private void fillDataGridGraficos()
        {
            List<ItemStat> lst = new List<ItemStat>();
            // Graficas
            ItemStat item1 = new ItemStat("Gráfico de Elementos por Categoría", "Ver gráfico", Resource1.chart);
            ItemStat item2 = new ItemStat("Gráfico de Elementos de Arquitectura", "Ver gráfico", Resource1.chart_pie);
            ItemStat item3 = new ItemStat("Gráfico de elementos de Estructura", "Ver gráfico", Resource1.chart_pie);
            ItemStat item4 = new ItemStat("Gráfico de elementos de MEP", "Ver gráfico", Resource1.chart_pie);

            lst.Add(item1);
            lst.Add(item2);
            lst.Add(item3);
            lst.Add(item4);

            this.dgvGraficos.DataBindings.Clear();
            this.dgvGraficos.DataSource = lst;
        }

        private void fillDataGridActualStats()
        {
            Autodesk.Revit.DB.Document doc = Tools._doc;
            List<ItemStat> lst = new List<ItemStat>();
            // Elementos procesados
            ItemStat item0 = new ItemStat("Elementos procesados", _elemCount.ToString() +
                " Unidades", Resource1.Ribbon_Shaded_32);
            lst.Add(item0);
            // Planos de Planta
            ItemStat item1 = new ItemStat("Planos de Planta totales", Tools.GetAllViewPlans(doc).Count.ToString() +
                " Unidades", Resource1.ViewPlan_32);
            lst.Add(item1);
            // Secciones
            ItemStat item2 = new ItemStat("Secciones totales", Tools.GetAllViewSections(doc).Count.ToString() +
                " Unidades", Resource1.Section_32);
            lst.Add(item2);
            // CAD Links
            ItemStat item3 = new ItemStat("Vinculos CAD totales", Tools.GetAllCadLinks(doc).Count.ToString() +
                " Unidades", Resource1.CadLink_32);
            lst.Add(item3);
            // CAD Imports
            ItemStat item4 = new ItemStat("Importaciones CAD totales", Tools.GetAllCadImports(doc).Count.ToString() +
                " Unidades", Resource1.CadImport_32);
            lst.Add(item4);
            // Tablas
            ItemStat item5 = new ItemStat("Tablas de Planificación totales", Tools.GetAllViewSchedule(doc).Count.ToString() +
                " Unidades", Resource1.Tabla_32);
            lst.Add(item5);
            // Niveles
            ItemStat item6 = new ItemStat("Niveles totales", Tools.GetAllLevels(doc).Count.ToString() +
                " Unidades", Resource1.Nivel_32);
            lst.Add(item6);
            // Grupos de Modelo
            ItemStat item7 = new ItemStat("Grupos de Modelo totales", Tools.GetAllGroups(doc).Count.ToString() +
                " Unidades", Resource1.GrupoModelo_32);
            lst.Add(item7);
            // Montajes
            ItemStat item8 = new ItemStat("Montajes totales", Tools.GetAllAssemblies(doc).Count.ToString() +
                " Unidades", Resource1.Montajes_32);
            lst.Add(item8);

            this.dgvActualStats.DataBindings.Clear();
            this.dgvActualStats.DataSource = lst;
        }

        private void fillProgressBar()
        {
            double current = 0;
            string level = "";
            if (_puntos < 1000)
            {
                current = (100 * _puntos / 1000);
                level = _puntos.ToString("N1") + " / 1,000 para desbloquear logro";
            }
            if (_puntos > 999 && _puntos < 2500)
            {
                current = (100 * _puntos / 2500);
                level = _puntos.ToString("N1") + " / 2,500 para desbloquear logro";
            }
            if (_puntos > 2499 && _puntos < 10000)
            {
                current = (100 * _puntos / 10000);
                level = _puntos.ToString("N1") + " / 10,000 para desbloquear logro";
            }
            if (_puntos > 9999)
            {
                current = 100;
                level = "Todos los Logros desbloqueados";
            }
            this.lblLevel.Text = level;
            progressBar2.Value = Convert.ToInt32(current);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvActualStats_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string titulo = "";
            string mensaje;
            List<Element> lst = new List<Element>();
            Document doc = Tools._doc;
            switch (e.RowIndex)
            {
                case 0:
                    // Elementos procesados
                    titulo = "Lista de Elementos procesados";
                    lst = Tools.CollectElements(Tools._doc, RevitQex._App);
                    break;
                case 1:
                    // Planos de Planta
                    titulo = "Lista de Planos de Planta:";
                    foreach (var item in Tools.GetAllViewPlans(doc))
                    {
                        lst.Add(item as Element);
                    }
                    break;
            }
            mensaje = Tools.WriteReportFromList(titulo, lst);
            //(new frmWarnings(titulo, "Detalle de elementos", mensaje)).ShowDialog();
        }

        private void dgvGraficos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0)
            {
                string title = "Elementos por Categorías";
                Dictionary<string, int> dictio = Tools.GetCategoriesCountDictionary();
                Chart chart0 = Tools.GetChartBarElementCategories(dictio, title);
                (new frmChart(chart0, title)).ShowDialog();
            }
            if (e.RowIndex == 1)
            {
                string title = "Elementos de Arquitectura";
                Dictionary<string, double> dictioARQ = Tools.GetDictionaryARQ();
                Chart chart0 = Tools.GetRadarChart(dictioARQ, "ARQ", title, System.Drawing.Color.CadetBlue);
                (new frmChart(chart0, title)).ShowDialog();
            }
            if (e.RowIndex == 2)
            {
                string title = "Elementos de Estructuras";
                Dictionary<string, double> dictioSTR = Tools.GetDictionarySTR();
                Chart chart0 = Tools.GetRadarChart(dictioSTR, "STR", title, System.Drawing.Color.IndianRed);
                (new frmChart(chart0, title)).ShowDialog();
            }
            if (e.RowIndex == 3)
            {
                string title = "Elementos de Instalaciones MEP";
                Dictionary<string, double> dictioMEP = Tools.GetDictionaryMEP();
                Chart chart0 = Tools.GetRadarChart(dictioMEP, "MEP", title, System.Drawing.Color.CadetBlue);
                (new frmChart(chart0, title)).ShowDialog();
            }
        }
    }
}
