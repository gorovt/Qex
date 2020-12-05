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

using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Structure;
using System.Net;
using System.Windows.Forms;

namespace Qex
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class RevitQex : IExternalCommand
    {
        public static string QexName = "Quantificaciones Express";
        public static double QexVersion = 3.4;

        public static List<Quantity> lstElements;
        public static string docGuid;
        public static List<Element> listaElementos;
        public static List<Material> listaMaterials;
        public static Autodesk.Revit.ApplicationServices.Application _App;
        public static ExternalCommandData _command;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application appGral = uiapp.Application;
            Document doc = uidoc.Document;
            _App = appGral;
            _command = commandData;
            Tools._doc = doc;

            Log.Start("Qex iniciado");

            #region Qex v2.3 Variables Necesarias
            //Variables necesarias
            DalRecursos._doc = doc;
            DalRecursos._projInfoId = Tools.GetProjectInformationId(doc);
            QexSchema._projInfoId = Tools.GetProjectInformationId(doc);
            
            
            #endregion
            //Document GUID
            if (doc.Title != "")
            {
                docGuid = doc.Title;//.Remove(doc.Title.Length -4, 4);
            }
            else
            {
                docGuid = "New Project";
            }
            Tools.InsertDbOpenLog();
            // Verificar Points
            if (null == Tools.GetPointFromDb())
            {
                Tools.InsertPointToDb();
            }

            // Verificar Schema y Entity
            Tools.VerificarSchemaQex();
            Tools.VerificarSchemaGrupos();
            Tools.VerificarSchemaMateriales();
            Tools.VerificarSchemaRecursos();
            Tools.VerificarSchemaComputos();

            //Opciones
            Tools.VerificarSchemaQexOptions(Tools._doc);

            Tools.SetOptions(QexSchema.ReadQexOptions(Tools._doc));

            (new frmSplash("Iniciando Qex, por favor espere...")).ShowDialog();

            frmMain mainForm = new frmMain(lstElements);
            mainForm.ShowDialog();
            return Result.Succeeded;
        }
    }
}
