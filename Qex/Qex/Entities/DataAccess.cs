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
using SQLite.Net;
using SQLite.Net.Platform.Win32;
using System.IO;
using System.Reflection;
#endregion

namespace Qex
{
    /// <summary> Esta Clase se conecta con la Base de Datos SqLite de Qex </summary>
    public class DataAccess : IDisposable
    {
        private SQLiteConnection connection;

        public DataAccess()
        {
            string exePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "qexdb.db3");
            connection = new SQLiteConnection(new SQLitePlatformWin32(), exePath);
            //Create Tables
            connection.CreateTable<dbOpenLog>();
            connection.CreateTable<Point>();
        }
        
        #region dbOpenLog
        public void InsertDbOpenLog(dbOpenLog log)
        {
            connection.Insert(log);
        }
        public void UpdateDbOpenLog(dbOpenLog log)
        {
            connection.Update(log);
        }
        public void DeleteDbOpenLog(dbOpenLog log)
        {
            connection.Delete(log);
        }
        public dbOpenLog GetDbOpenLogById(int id)
        {
            return connection.Table<dbOpenLog>().FirstOrDefault(xx => xx.Id == id);
        }
        public List<dbOpenLog> GetDbOpenLogs()
        {
            return connection.Table<dbOpenLog>().ToList();
        }
        #endregion

        #region Points
        public void InsertPoint(Point point)
        {
            connection.Insert(point);
        }
        public void UpdatePoint(Point point)
        {
            connection.Update(point);
        }
        public Point GetPoint()
        {
            Point point = null;
            List<Point> points = connection.Table<Point>().ToList();
            if (points.Count > 0)
            {
                point = points.First();
            }
            return point;
        }
#endregion
        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
