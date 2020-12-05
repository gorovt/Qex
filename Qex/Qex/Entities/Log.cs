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
using System.IO;

namespace Qex
{
    public abstract class Log
    {
        public static string path = Path.Combine(App.AppDirectory, "log.txt");

        public static void Insert(string text)
        {
            string log = DateTime.Now.ToString() + " --> " + text;
            try
            {
                File.AppendAllLines(path, new List<string>() { log });
            }
            catch (Exception)
            {
                // 
            }
        }

        public static void Start(string text)
        {
            string log = DateTime.Now.ToString() + " --> " + text;
            try
            {
                File.AppendAllLines(path, new List<string>() { "" });
                File.AppendAllLines(path, new List<string>() { log });
            }
            catch (Exception)
            {
                // 
            }
        }

        public static void End(string text)
        {
            string log = DateTime.Now.ToString() + " --> " + text;
            try
            {
                File.AppendAllLines(path, new List<string>() { log });
                File.AppendAllLines(path, new List<string>() { "" });
            }
            catch (Exception)
            {
                // 
            }
        }
        public static void Clean()
        {
            try
            {
                File.WriteAllText(path, "");
            }
            catch (Exception)
            {
                //
            }
        }

        public static void Show()
        {
            try
            {
                System.Diagnostics.Process.Start(path);
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
