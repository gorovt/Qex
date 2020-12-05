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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qex
{
    public class MaterialImportado
    {
        public string id { get; set; }
        public string nombre { get; set; }

        public MaterialImportado()
        {
            id = string.Empty;
            nombre = string.Empty;
        }
    }
    public class RecursoImportadoExcel
    {
        public string Group { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public string Unit { get; set; }
        public int Index { get; set; }

        public RecursoImportadoExcel()
        {
            Group = string.Empty;
            Name = string.Empty;
            Unit = string.Empty;
            Cost = 0;
            Index = 0;
        }
    }
}
