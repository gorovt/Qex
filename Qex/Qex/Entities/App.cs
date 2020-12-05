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

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Events;
using System.IO;

namespace Qex
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class App : Autodesk.Revit.UI.IExternalApplication
    {
        static Autodesk.Revit.DB.AddInId m_appId = new Autodesk.Revit.DB.AddInId(new Guid("e28b6fcd-47dd-4779-94c4-c4f6bf130d0c"));
        //get the absolute path of this assembly
        public static string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        public static string AppDirectory = System.IO.Path.GetDirectoryName(ExecutingAssemblyPath);

        public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
        {
            AddMenu(application);
            return Autodesk.Revit.UI.Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Autodesk.Revit.UI.Result.Succeeded;
        }

        private void AddMenu(UIControlledApplication app)
        {
            Autodesk.Revit.UI.RibbonPanel QexPanel = app.CreateRibbonPanel("Qex 3.4");
            //IMPORTANT NOTE: las imagenes de los botones deben tener su propiedad "Build Action" como Resource.

            //Add button to the Qex Panel
            PushButton button3 = QexPanel.AddItem(new PushButtonData("Qex1", "Iniciar Qex",
                ExecutingAssemblyPath, "Qex.RevitQex")) as PushButton;

            button3.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Qex;component/Resources/Qex-32x32.png"));
            button3.ToolTip = "Crea una Quantificación completa del modelo actual";
            button3.LongDescription = "Qex evita la creación de numerosas tablas, " +
                "y permite exportar los cómputos a Excel";

            string path;
            path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.ChmFile,
                "http://www.universobim.com/foro/forumdisplay.php?fid=3"); // hard coding for simplicity. 

            button3.SetContextualHelp(contextHelp);
        }
    }
}

