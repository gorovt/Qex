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
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Qex
{
    /// <summary> Esta Clase representa un Recurso consumido </summary>
    public class QexRecurso
    {
        public string id { get; set; }
        public string matId { get; set; }
        public double Consumo { get; set; }

        #region Constructor
        public QexRecurso()
        {
            this.id = Guid.NewGuid().ToString();
            this.matId = string.Empty;
            this.Consumo = 1;
        }

        public QexRecurso(string id, string matId, double consumo)
        {
            this.id = id;
            this.matId = matId;
            this.Consumo = consumo;
        }
        #endregion

        #region Conversiones
        /// <summary> Se obtiene un TreeNode </summary>
        public System.Windows.Forms.TreeNode ToNodeText()
        {
            System.Windows.Forms.TreeNode node = new System.Windows.Forms.TreeNode();
            node.Name = this.id;
            QexMaterial mat = GetMaterial();
            node.Text = mat.nombre + " (Consumo: " + this.Consumo.ToString() + " " + mat.unidad + ")";
            return node;
        }

        /// <summary> Convierte el Recurso en una linea separada por ; </summary>
        public string ToLine()
        {
            string line = this.id + ";" + this.matId + ";" + this.Consumo.ToString();
            return line;
        }

        /// <summary> Devuelve un ListViewItem básico </summary>
        public System.Windows.Forms.ListViewItem ToItem()
        {
            System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem();
            item.Name = this.id;
            item.Text = GetMaterial().nombre;
            item.ImageIndex = GetMaterial().index;
            return item;
        }

        /// <summary> Convierte un Consumo de Recurso en ItemRecurso </summary>
        public ItemRecurso ToItemRecurso()
        {
            QexMaterial mat = GetMaterial();
            ItemRecurso rec = new ItemRecurso();
            rec.nombre = mat.nombre;
            rec.consumo = this.Consumo;
            rec.unidad = mat.unidad;
            rec.precioUnit = mat.precio;
            rec.costoTotal = Math.Round(this.Consumo * mat.precio, 2);
            rec.index = mat.index;
            return rec;
        }
        #endregion

        #region Base de Datos
        /// <summary> Obtiene el Material del Recurso </summary>
        public QexMaterial GetMaterial()
        {
            return DalRecursos.ReadMateriales().FirstOrDefault(x => x.id == this.matId);
        }

        /// <summary> Devuelve True si se puede insertar el Recurso en el Elemento </summary>
        public bool Insert(Element elem)
        {
            bool exito = false;
            try
            {
                if (DalRecursos.InsertRecurso(this, elem))
                {
                    exito = true;
                }
                else
                {
                    exito = false;
                }
            }
            catch (Exception)
            {
                exito = false;
            }
            return exito;
        }

        /// <summary> Devuelve True si se puede actualizar el Recurso en el Elemento </summary>
        public bool Update(Element elem)
        {
            bool exito = false;
            try
            {
                List<QexRecurso> recursos = DalRecursos.ReadRecursosFromElement(elem);
                QexRecurso recurso = recursos.FirstOrDefault(x => x.id == this.id);
                recurso.matId = this.matId;
                recurso.Consumo = this.Consumo;
                if (DalRecursos.UpdateRecursos(elem, recursos))
                {
                    exito = true;
                }
            }
            catch (Exception)
            {
                exito = false;
            }
            return exito;
        }

        /// <summary> Devuelve True si se puede eliminar el Recurso en el Elemento </summary>
        public bool Delete(Element elem)
        {
            bool exito = false;
            try
            {
                List<QexRecurso> recursos = DalRecursos.ReadRecursosFromElement(elem);
                int index = recursos.FindIndex(x => x.id == this.id);
                recursos.RemoveAt(index);
                if (DalRecursos.UpdateRecursos(elem, recursos))
                {
                    exito = true;
                }
            }
            catch (Exception)
            {
                exito = false;
            }
            return exito;
        }
#endregion
    }

    public class ItemRecurso
    {
        public string nombre { get; set; }
        public double consumo { get; set; }
        public string unidad { get; set; }
        public double precioUnit { get; set; }
        public double costoTotal { get; set; }
        public int index { get; set; }

        public ItemRecurso()
        {
            this.nombre = string.Empty;
            this.consumo = 0;
            this.unidad = string.Empty;
            this.precioUnit = 0;
            this.costoTotal = 0;
            this.index = 0;
        }

        public ItemRecurso(string nombre, double consumo, string unidad, double precioUnit, double costoTotal, int index)
        {
            this.nombre = nombre;
            this.consumo = consumo;
            this.unidad = unidad;
            this.precioUnit = precioUnit;
            this.costoTotal = costoTotal;
            this.index = index;
        }
    }
}
