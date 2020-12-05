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
    /// Esta Clase representa un Parametro de un elemento de Revit
    /// </summary>
    public class Qparameter
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public double valor { get; set; }
        public string unidad { get; set; }

        #region Constructores
        public Qparameter()
        {
            this.id = -1;
            this.nombre = string.Empty;
            this.valor = 0;
            this.unidad = string.Empty;
        }

        public Qparameter(int id, string nombre, double valor, string unidad)
        {
            this.id = id;
            this.nombre = nombre;
            this.valor = valor;
            this.unidad = unidad;
        }
#endregion
    }
}
