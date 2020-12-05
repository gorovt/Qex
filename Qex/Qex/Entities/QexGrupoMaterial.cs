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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
#endregion

namespace Qex
{
    /// <summary>
    /// Esta Clase representa un Grupo de Material
    /// </summary>
    public class QexGrupoMaterial
    {
        public string id { get; set; }
        public string Nombre { get; set; }

        #region Constructores
        public QexGrupoMaterial()
        {
            this.id = Guid.NewGuid().ToString();
            this.Nombre = string.Empty;
        }

        public QexGrupoMaterial(string id, string nombre)
        {
            this.id = id;
            this.Nombre = nombre;
        }
        #endregion

        #region Conversiones
        /// <summary> Convierte el Grupo en una linea separada por ; </summary>
        public string ToLine()
        {
            string line = this.id + ";" + this.Nombre;
            return line;
        }

        /// <summary> Obtiene un TreeNode básico </summary>
        public System.Windows.Forms.TreeNode ToNode()
        {
            System.Windows.Forms.TreeNode node = new System.Windows.Forms.TreeNode();
            node.Name = this.id;
            node.Text = this.Nombre;
            return node;
        }
        #endregion

        #region Base de Datos
        /// <summary> Inserta el Grupo en el elemento ProjectInfo </summary>
        public bool Insert()
        {
            bool exito = false;
            try
            {
                if (DalRecursos.InsertGrupo(this))
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

        /// <summary> Actualiza el Grupo en el elemento ProjectInfo </summary>
        public bool Update()
        {
            bool exito = false;
            try
            {
                List<QexGrupoMaterial> grupos = DalRecursos.ReadGrupos();
                QexGrupoMaterial grupo = grupos.FirstOrDefault(x => x.id == this.id);
                grupo.Nombre = this.Nombre;
                DalRecursos.UpdateGrupos(grupos);
                exito = true;
            }
            catch (Exception)
            {
                exito = false;
            }
            return exito;
        }

        /// <summary> Elimina el Grupo y todos sus Materiales del ProjectInfo. 
        /// (0 no se puede) (1 eliminado todo) (2 hay Materiales usados) (</summary>
        public int Delete()
        {
            int used = 0;
            int exito = 0;
            try
            {
                List<QexGrupoMaterial> grupos = DalRecursos.ReadGrupos();
                List<QexMaterial> materiales = DalRecursos.ReadMateriales();

                // Quitar los Materiales del Grupo
                List<QexMaterial> hijos = GetChilds();
                foreach (QexMaterial item in hijos)
                {
                    if (!item.IsUsed())
                    {
                        int indexHijo = materiales.FindIndex(x => x.id == item.id);
                        materiales.RemoveAt(indexHijo);
                    }
                    else
                    {
                        used++;
                    }
                }

                // Quitar el grupo de la Lista
                if (used == 0)
                {
                    int indexGrupo = grupos.FindIndex(x => x.id == this.id);
                    grupos.RemoveAt(indexGrupo);

                    // Actualizar todo
                    if (DalRecursos.UpdateGrupos(grupos) && DalRecursos.UpdateMateriales(materiales))
                    {
                        exito = 1;
                    }
                }
                else
                {
                    exito = 2;
                }
            }
            catch (Exception)
            {
                exito = 0;
            }
            return exito;
        }

        /// <summary> Obtiene todos los Materiales del Grupo </summary>
        public List<QexMaterial> GetChilds()
        {
            return DalRecursos.ReadMateriales().FindAll(x => x.grupoId == this.id);
        }
#endregion
    }
}
