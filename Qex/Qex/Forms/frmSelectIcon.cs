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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace Qex
{
    public partial class frmSelectIcon : System.Windows.Forms.Form
    {
        public int _index;

        public frmSelectIcon(int index)
        {
            InitializeComponent();
            this._index = index;
            FillIcons();
        }

        private void FillIcons()
        {
            List<Image> icons = Tools.ListaIconos();
            this.btn01.BackgroundImage = icons[0];
            this.btn02.BackgroundImage = icons[1];
            this.btn03.BackgroundImage = icons[2];
            this.btn04.BackgroundImage = icons[3];
            this.btn05.BackgroundImage = icons[4];
            this.btn06.BackgroundImage = icons[5];
            this.btn07.BackgroundImage = icons[6];
            this.btn08.BackgroundImage = icons[7];
            this.btn09.BackgroundImage = icons[8];
            this.btn10.BackgroundImage = icons[9];
            this.btn11.BackgroundImage = icons[10];
            this.btn12.BackgroundImage = icons[11];
            this.btn13.BackgroundImage = icons[12];
            this.btn14.BackgroundImage = icons[13];
            this.btn15.BackgroundImage = icons[14];
        }

        private void btn01_Click(object sender, EventArgs e)
        {
            this._index = 0;
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            this._index = 1;
        }

        private void btn03_Click(object sender, EventArgs e)
        {
            this._index = 2;
        }

        private void btn04_Click(object sender, EventArgs e)
        {
            this._index = 3;
        }

        private void btn05_Click(object sender, EventArgs e)
        {
            this._index = 4;
        }

        private void btn06_Click(object sender, EventArgs e)
        {
            this._index = 5;
        }

        private void btn07_Click(object sender, EventArgs e)
        {
            this._index = 6;
        }

        private void btn08_Click(object sender, EventArgs e)
        {
            this._index = 7;
        }

        private void btn09_Click(object sender, EventArgs e)
        {
            this._index = 8;
        }

        private void btn10_Click(object sender, EventArgs e)
        {
            this._index = 9;
        }

        private void btn11_Click(object sender, EventArgs e)
        {
            this._index = 10;
        }

        private void btn12_Click(object sender, EventArgs e)
        {
            this._index = 11;
        }

        private void btn13_Click(object sender, EventArgs e)
        {
            this._index = 12;
        }

        private void btn14_Click(object sender, EventArgs e)
        {
            this._index = 13;
        }

        private void btn15_Click(object sender, EventArgs e)
        {
            this._index = 14;
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
