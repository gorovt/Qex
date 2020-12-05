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
#endregion

namespace Qex
{
    /// <summary>
    /// Esta Clase Representa un Material
    /// </summary>
    public class QexMaterial
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string unidad { get; set; }
        public string grupoId { get; set; }
        public double precio { get; set; }
        public int index { get; set; }

        #region Constructores
        public QexMaterial()
        {
            this.id = Guid.NewGuid().ToString();
            this.nombre = string.Empty;
            this.unidad = string.Empty;
            this.grupoId = string.Empty;
            this.precio = 0;
            this.index = 0;
        }

        public QexMaterial(string id, string nombre, string unidad, string grupoId, double precio, int index)
        {
            this.id = id;
            this.nombre = nombre;
            this.unidad = unidad;
            this.grupoId = grupoId;
            this.precio = precio;
            this.index = index;
        }
        #endregion

        #region Conversiones
        /// <summary> Devuelve un TreeNode básico </summary>
        public System.Windows.Forms.TreeNode ToNode()
        {
            System.Windows.Forms.TreeNode node = new System.Windows.Forms.TreeNode();
            node.Name = this.id;
            node.Text = this.nombre;
            return node;
        }

        /// <summary> Convierte el Material en una linea separada por ; </summary>
        public string ToLine()
        {
            string line = this.id + ";" + this.nombre + ";" + this.unidad + ";" + this.grupoId + ";" + 
                this.precio.ToString() + ";" + this.index;
            return line;
        }

        /// <summary> Devuelve un ListViewItem básico </summary>
        public System.Windows.Forms.ListViewItem ToItem()
        {
            System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem();
            item.Name = this.id;
            item.Text = this.nombre;
            item.ImageIndex = this.index;
            return item;
        }

        
        #endregion

        #region Base de Datos
        /// <summary> Devuelve True si puede insertar el Material en el ProjectInfo </summary>
        public bool Insert()
        {
            bool exito = false;
            try
            {
                if (DalRecursos.InsertMaterial(this))
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

        /// <summary> Devuelve True si se puede actualizar el Material en el ProjectInfo </summary>
        public bool Update()
        {
            bool exito = false;
            try
            {
                List<QexMaterial> materiales = DalRecursos.ReadMateriales();
                QexMaterial material = materiales.FirstOrDefault(x => x.id == this.id);
                material.nombre = this.nombre;
                material.unidad = this.unidad;
                material.grupoId = this.grupoId;
                material.precio = this.precio;
                material.index = this.index;
                if (DalRecursos.UpdateMateriales(materiales))
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

        /// <summary> Devuelve True si se puede eliminar el Material del ProjectInfo FALTAN LOS RECURSOS!!</summary>
        public bool Delete()
        {
            bool deleted = false;
            try
            {
                List<QexMaterial> materiales = DalRecursos.ReadMateriales();
                int index = materiales.FindIndex(x => x.id == this.id);
                materiales.RemoveAt(index);
                if (DalRecursos.UpdateMateriales(materiales))
                {
                    deleted = true;
                }
            }
            catch (Exception)
            {
                deleted = false;
            }
            return deleted;
        }
#endregion

        /// <summary> Devuelve True si el Material está siendo usado por algún Recurso </summary>
        public bool IsUsed()
        {
            bool used = false;
            List<QexRecurso> recursos = new List<QexRecurso>();
            foreach (Quantity q in RevitQex.lstElements)
            {
                recursos.AddRange(Tools.GetRecursosFromQuantity(q));
            }
            if (recursos.Exists(x => x.matId == this.id))
            {
                used = true;
            }
            return used;
        }
    }
}
