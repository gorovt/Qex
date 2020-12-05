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
#endregion

namespace Qex
{
    /// <summary>
    /// Esta Clase representa un TreeNode en Qex
    /// </summary>
    public class Tview
    {
        public string name { get; set; }
        public string text { get; set; }
        public string parent { get; set; }
        public List<Quantity> lstQuantity { get; set; }

        #region Constructores
        public Tview(string name, string parent)
        {
            this.name = name;
            this.parent = parent;
        }

        public Tview(string name, string text, string parent)
        {
            this.name = name;
            this.text = text;
            this.parent = parent;
        }

        public Tview()
        {
            lstQuantity = new List<Quantity>();
        }
        #endregion

        #region Get
        /// <summary>
        /// Obtiene una Lista de los Tviews hijos
        /// </summary>
        /// <param name="lstTviews">Lista de Tviews completa</param>
        /// <returns></returns>
        public List<Tview> GetChilds(List<Tview> lstTviews)
        {
            List<Tview> lst = lstTviews.FindAll(x => x.parent == name);
            return lst;
        }
#endregion
    }
}
