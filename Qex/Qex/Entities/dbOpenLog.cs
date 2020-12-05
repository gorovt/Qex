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
using SQLite.Net.Attributes;
#endregion

namespace Qex
{
    /// <summary>
    /// Esta clase representa un registro de Log cada vez que se ejecuta Qex
    /// </summary>
    public class dbOpenLog
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ProjectName { get; set; }
        public int ElementCount { get; set; }

        public dbOpenLog()
        {
            Date = DateTime.Now;
            ProjectName = string.Empty;
            ElementCount = 0;
        }

        public dbOpenLog(DateTime date, string projectName, int elementCount)
        {
            this.Date = date;
            this.ProjectName = projectName;
            this.ElementCount = elementCount;
        }
    }
}
