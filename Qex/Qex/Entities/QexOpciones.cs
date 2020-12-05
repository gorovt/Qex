//*******************************************************************************
//*                                                                             *
//*    Copyright (c) 2016-2020 Luciano Gorosito <lucianogorosito@hotmail.com>   *
//*                                                                             *
//*    This file is part of Qex                                                 *
//*                                                                             *
//*    Qex is free software: you can redistribute it and/or modify              *
//*    it under the terms of the GNU General Public License as published by     *
//*    the Free Software Foundation, either version 3 of the License, or        *
//*    (at your option) any later version.                                      *
//*                                                                             *
//*    Qex is distributed in the hope that it will be useful,                   *
//*    but WITHOUT ANY WARRANTY; without even the implied warranty of           *
//*    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the            *
//*    GNU General Public License for more details.                             *
//*                                                                             *
//*    You should have received a copy of the GNU General Public License        *
//*    along with this program.  If not, see <https://www.gnu.org/licenses/>.   *
//*                                                                             *
//*******************************************************************************

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
    /// Esta Clase abstracta reune las opciones disponibles en Qex
    /// </summary>
    public abstract class QexOpciones
    {
        public static bool wallsMaterials = true;
        public static bool floorMaterials = true;
        public static bool ceilingMaterials = true;
        public static bool roofMaterials = true;
        public static bool strFoundationsFloorMaterials = true;
        public static bool padsMaterials = false;
        public static bool rebarMarca = false;

        public static bool QuantityByPhase = false;
        public static bool QuantityByLevel = false;
        public static bool QuantityByGroup = false;
        public static bool QuantityByAssembly = false;
    }
}
