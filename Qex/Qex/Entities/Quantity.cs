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

#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;
#endregion

namespace Qex
{
    /// <summary> Esta Clase representa una Quantificación, que es la unidad de Cómputo básica de Qex </summary>
    public class Quantity
    {
        public string qId { get; set; }
        public int typeId { get; set; }
        public string category { get; set; }
        public string name { get; set; }
        public int count { get; set; }
        public double length { get; set; }
        public double area { get; set; }
        public double volume { get; set; }
        public string medicion { get; set; }
        public double cost { get; set; }
        public double totalCost { get; set; }
        public string phaseCreated { get; set; }
        public string matriz { get; set; }
        public string descripcion { get; set; }
        public string grupo { get; set; }
        public string libro { get; set; }
        public bool visible { get; set; }
        public int LevelId { get; set; }
        public Bitmap image { get; set; }
        public List<Qparameter> lstParameters { get; set; }
        public double value { get; set; }
        public string unidad { get; set; }
        public int paramId { get; set; }
        public int grupoId { get; set; }
        public int qOrden { get; set; }
        public int grupoOrden { get; set; }
        public int libroOrden { get; set; }

        #region Constructores
        public Quantity()
        {
            this.qId = string.Empty;
            this.typeId = 0;
            this.category = string.Empty;
            this.name = string.Empty;
            this.count = 1;
            this.area = 0;
            this.length = 0;
            this.volume = 0;
            this.medicion = "";
            this.cost = 0;
            this.totalCost = 0;
            this.phaseCreated = string.Empty;
            this.matriz = "1000";
            this.descripcion = "";
            this.grupo = "";
            this.libro = "";
            this.visible = true;
            this.LevelId = -1;
            this.image = Resource1.NoImage_256x256;
            this.lstParameters = new List<Qparameter>();
            this.value = 0;
            this.unidad = string.Empty;
            this.paramId = -1;
            this.grupoId = -1;
            this.qOrden = 1;
            this.grupoOrden = 1;
            this.libroOrden = 1;
        }

        public Quantity(string qId, int typeId, string category, string name, int count, double length, double area,
            double volume, string medicion, double cost, double totalCost, string phaseCreated, string matriz,
            string descripcion, string grupo, string libro, bool visible, int levelId, 
            List<Qparameter> lstParameters, double value, string unidad, int paramId, int grupoId
            , int qOrden, int grupoOrden, int libroOrden)
        {
            this.qId = qId;
            this.typeId = typeId;
            this.category = category;
            this.name = name;
            this.count = count;
            this.length = length;
            this.area = area;
            this.volume = volume;
            this.medicion = medicion;
            this.cost = cost;
            this.totalCost = totalCost;
            this.phaseCreated = phaseCreated;
            this.matriz = matriz;
            this.descripcion = descripcion;
            this.grupo = grupo;
            this.libro = libro;
            this.visible = visible;
            this.LevelId = levelId;
            this.lstParameters = lstParameters;
            this.value = value;
            this.unidad = unidad;
            this.paramId = paramId;
            this.grupoId = grupoId;
            this.image = Resource1.NoImage_256x256;
            this.qOrden = qOrden;
            this.grupoOrden = grupoOrden;
            this.libroOrden = libroOrden;
        }
        #endregion

        #region Conversiones
        /// <summary> Convierte la Quantificacion en una linea de texto para exportar el archivo QEX </summary>
        public static string ToStringLine(Quantity q, bool verDescripcion)
        {
            string line = "";
            line += q.qId + ";";
            line += q.typeId + ";";
            line += q.category + ";";
            if (verDescripcion)
                line += q.descripcion + ";";
            if (!verDescripcion)
                line += q.name + ";";
            line += q.medicion + ";";
            line += q.totalCost + ";";
            line += q.phaseCreated + ";";
            line += q.grupo + ";";
            line += q.libro + ";";
            if (verDescripcion)
                line += "True;";
            if (!verDescripcion)
                line += "False;";
            line += q.qOrden.ToString() + ";";
            line += q.grupoOrden.ToString() + ";";
            line += q.libroOrden;
            return line;
        }

        /// <summary> Convierte la Quanfiticación a una línea de texto para guardar en un Schema </summary>
        public string ToStringComputo()
        {
            //qid;typeId;matriz;grupo;libro
            string computo = "";
            computo += this.qId + ";";
            computo += this.typeId.ToString() + ";";
            computo += this.matriz + ";";
            computo += this.grupo + ";";
            computo += this.libro + ";";
            computo += this.qOrden.ToString() + ";";
            computo += this.grupoOrden.ToString() + ";";
            computo += this.libroOrden.ToString();
            return computo;
        }

        /// <summary> Obtiene una linea de texto con los Ordenes, para guardar en el Schema Qex </summary>
        public string ToOrdenLine()
        {
            string line = this.qOrden.ToString() + ";" + this.grupoOrden.ToString() + ";" + this.libroOrden.ToString();
            return line;
        }
#endregion

        #region Get
        /// <summary> Obtiene el nombre de la Categoria, quitando los parentesis </summary>
        public string GetRealCategory()
        {
            string[] lines = this.category.Split('(');
            int count = lines[0].Length;
            string realCat = lines[0].Remove(count - 1, 1);
            return realCat;
        }

        public List<ItemRecurso> GetItemRecursos()
        {
            List<ItemRecurso> items = new List<ItemRecurso>();
            foreach (QexRecurso rec in Tools.GetRecursosFromQuantity(this))
            {
                ItemRecurso item = rec.ToItemRecurso();
                items.Add(item);
            }
            items = items.OrderBy(x => x.nombre).ToList();
            return items;
        }

        /// <summary> Obtiene el número de la magnitud preferida </summary>
        public double GetMedicion()
        {
            double _medicion = 0;
            if (this.matriz == "1000")
                _medicion = this.count;
            if (this.matriz == "0100")
                _medicion = this.length;
            if (this.matriz == "0010")
                _medicion = this.area;
            if (this.matriz == "0001")
                _medicion = this.volume;
            if (this.matriz == "1111")
                _medicion = this.value;
            return _medicion;
        }

        /// <summary> Obtiene la unidad </summary>
        public string GetUnidad()
        {
            string _medicion = "";
            if (this.matriz == "1000")
                _medicion = "un";
            if (this.matriz == "0100")
                _medicion = "ml";
            if (this.matriz == "0010")
                _medicion = "m2";
            if (this.matriz == "0001")
                _medicion = "m3";
            if (this.matriz == "1111")
                _medicion = this.unidad;
            return _medicion;
        }

        
        #endregion

        #region Calculos
        /// <summary> Obtiene el Costo Total de la Quantificación. Magnitud x CostoUnit </summary>
        private double CalcularCostoTotal()
        {
            double totalCost = 0;
            if (this.matriz == "1000")
                totalCost = Math.Round(this.count * this.cost, 2);
            if (this.matriz == "0100")
                totalCost = Math.Round(this.length * this.cost, 2);
            if (this.matriz == "0010")
                totalCost = Math.Round(this.area * this.cost, 2);
            if (this.matriz == "0001")
                totalCost = Math.Round(this.volume * this.cost, 2);
            if (this.matriz == "1111")
                totalCost = Math.Round(this.value * this.cost, 2);
            return totalCost;
        }

        /// <summary> Obtiene la Unidad de la Quantificación </summary>
        private string CalcularMedición()
        {
            string medicion = "";
            if (this.matriz == "1000")
                medicion = this.count.ToString(this.count % 1 == 0 ? "F0" : "F2") + " ud";
            if (this.matriz == "0100")
                medicion = this.length.ToString(this.length % 1 == 0 ? "F0" : "F2") + " ml";
            if (this.matriz == "0010")
                medicion = this.area.ToString(this.area % 1 == 0 ? "F0" : "F2") + " m2";
            if (this.matriz == "0001")
                medicion = this.volume.ToString(this.volume % 1 == 0 ? "F0" : "F2") + " m3";
            if (this.matriz == "1111")
                medicion = this.value.ToString(this.value % 1 == 0 ? "F0" : "F2") + " " + this.unidad;
            return medicion;
        }
#endregion
    }
}
