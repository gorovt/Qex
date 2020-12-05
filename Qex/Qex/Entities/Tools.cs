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

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Analysis;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ClosedXML.Excel;

namespace Qex
{
    /// <summary> Esta clase representa una colección de utilidades del Proyecto </summary>
    public abstract class Tools
    {
        public static List<string> _lstWarnings;
        public static int _elemCount = 0;
        // Needed variables
        public static Document _doc;

        #region Collectors
        public static List<Element> CollectElementByType(Document doc, ElementId typeId)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> elements = collector.WhereElementIsNotElementType().ToList();
            List<Element> lstInstance = (from elem in elements
                                         where elem.GetTypeId() == typeId
                                         select elem).ToList();
            return lstInstance;
        }
        public static List<Element> CollectElements(Document doc, Autodesk.Revit.ApplicationServices.Application app)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> elements = collector.WhereElementIsNotElementType().ToList();

            Options op = app.Create.NewGeometryOptions();
            op.ComputeReferences = true;

            List<Element> lstInstance = (from elem in elements
                                         where elem.Category != null
                                         //&& elem.Category.CategoryType == CategoryType.Model
                                         && elem.get_Geometry(op) != null
                                         && elem.GetTypeId().IntegerValue != -1
                                         && elem.CreatedPhaseId.IntegerValue != -1
                                         //&& elem.Location != null
                                         && elem.Category.Id != (new ElementId(BuiltInCategory.OST_Cameras))
                                         && elem.Category.Id != (new ElementId(BuiltInCategory.OST_StackedWalls))
                                         select elem).ToList();

            // Assemblies
            if (QexOpciones.QuantityByAssembly)
            {
                List<Element> assemblies = collector.OfClass(typeof(AssemblyInstance)).ToList();
                lstInstance.AddRange(assemblies);
                // Quitar Elements del Assembly
                foreach (var montaje in assemblies)
                {
                    foreach (var elem in GetElementsFromAssembly(doc, montaje))
                    {
                        Element encontrado = lstInstance.FirstOrDefault(x => x.Id == elem.Id);
                        lstInstance.Remove(encontrado);
                    }
                }
            }

            return lstInstance;
        }
        public static List<Element> GetElementsFromAssembly(Document doc, Element assembly)
        {
            AssemblyInstance montaje = assembly as AssemblyInstance;
            List<ElementId> lst = montaje.GetMemberIds().ToList();
            List<Element> lstElem = new List<Element>();
            foreach (var item in lst)
            {
                Element elem = doc.GetElement(item);
                lstElem.Add(elem);
            }
            return lstElem;
        }
        public static List<Element> CollectMassElements(Document doc, Autodesk.Revit.ApplicationServices.Application app)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> massFloor = collector.OfClass(typeof(MassLevelData)).ToList();

            List<Element> lstInstance = new List<Element>();
            lstInstance.AddRange(massFloor);

            return lstInstance;
        }
        public static List<Element> CollectTopography(Document doc, Autodesk.Revit.ApplicationServices.Application app)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> lstTopo = collector.OfClass(typeof(TopographySurface)).ToList();

            List<Element> lstInstance = new List<Element>();
            lstInstance.AddRange(lstTopo);

            return lstInstance;
        }
        public static List<Material> GetMaterialsFromElement(Document doc, Element e)
        {
            List<Material> lst = new List<Material>();
            ICollection<ElementId> idsMaterials = e.GetMaterialIds(false);
            foreach (var id in idsMaterials)
            {
                Material mat = doc.GetElement(id) as Material;
                lst.Add(mat);
            }
            return lst;
        }
        public static List<Face> getFaces(Document doc, Element e)
        {
            List<Face> lstFaces = new List<Face>();
            Options opt = new Options();
            opt.ComputeReferences = true;
            GeometryElement geom = e.get_Geometry(opt);
            foreach (GeometryObject geobj in geom)
            {
                Solid geomSolid = geobj as Solid;
                if (geomSolid != null)
                {
                    foreach (Face geoFace in geomSolid.Faces)
                    {
                        lstFaces.Add(geoFace);
                    }
                }
            }
            return lstFaces;
        }
        public static List<Level> GetAllLevels(Document doc)
        {
            List<Level> lst = new List<Level>();
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> lstElem = collector.OfClass(typeof(Level)).ToList();
            foreach (Element elem in lstElem)
            {
                // Convertir el Elemento en Nivel
                Level lvl = elem as Level;
                lst.Add(lvl);
            }
            return lst;
        }
        public static List<ViewPlan> GetAllViewPlans(Document doc)
        {
            List<ViewPlan> lst = new List<ViewPlan>();
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> lstElem = collector.OfClass(typeof(ViewPlan)).ToList();
            foreach (Element elem in lstElem)
            {
                // Convertir el Elemento
                ViewPlan view = elem as ViewPlan;
                if (!view.IsTemplate)
                {
                    lst.Add(view);
                }
            }
            return lst;
        }
        public static List<ViewSection> GetAllViewSections(Document doc)
        {
            List<ViewSection> lst = new List<ViewSection>();
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> lstElem = collector.OfClass(typeof(ViewSection)).ToList();
            foreach (Element elem in lstElem)
            {
                // Convertir el Elemento
                ViewSection view = elem as ViewSection;
                if (!view.IsTemplate)
                {
                    lst.Add(view);
                }
            }
            return lst;
        }
        public static List<ViewSchedule> GetAllViewSchedule(Document doc)
        {
            List<ViewSchedule> lst = new List<ViewSchedule>();
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> lstElem = collector.OfClass(typeof(ViewSchedule)).ToList();
            foreach (Element elem in lstElem)
            {
                // Convertir el Elemento
                ViewSchedule view = elem as ViewSchedule;
                if (!view.IsTemplate)
                {
                    lst.Add(view);
                }
            }
            return lst;
        }
        public static List<CADLinkType> GetAllCadLinks(Document doc)
        {
            List<CADLinkType> lst = new List<CADLinkType>();
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> lstElem = collector.OfClass(typeof(CADLinkType)).ToList();
            foreach (Element elem in lstElem)
            {
                // Convertir el Elemento
                CADLinkType view = elem as CADLinkType;
                if (view.IsExternalFileReference())
                {
                    lst.Add(view);
                }
            }
            return lst;
        }
        public static List<CADLinkType> GetAllCadImports(Document doc)
        {
            List<CADLinkType> lst = new List<CADLinkType>();
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> lstElem = collector.OfClass(typeof(CADLinkType)).ToList();
            foreach (Element elem in lstElem)
            {
                // Convertir el Elemento
                CADLinkType view = elem as CADLinkType;
                if (!view.IsExternalFileReference())
                {
                    lst.Add(view);
                }
            }
            return lst;
        }
        public static List<Group> GetAllGroups(Document doc)
        {
            List<Group> lst = new List<Group>();
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> lstElem = collector.OfClass(typeof(Group)).ToList();
            foreach (Element elem in lstElem)
            {
                // Convertir el Elemento
                Group grupo = elem as Group;
                lst.Add(grupo);
            }
            return lst;
        }
        public static List<AssemblyInstance> GetAllAssemblies(Document doc)
        {
            List<AssemblyInstance> lst = new List<AssemblyInstance>();
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> lstElem = collector.OfClass(typeof(AssemblyInstance)).ToList();
            foreach (Element elem in lstElem)
            {
                // Convertir el Elemento
                AssemblyInstance grupo = elem as AssemblyInstance;
                lst.Add(grupo);
            }
            return lst;
        }
        public static string WriteReportFromList(string title, List<Element> lst)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(title);
            foreach (var item in lst)
            {
                sb.AppendLine(item.Name + "<" + item.Id.IntegerValue + ">");
            }
            sb.AppendLine("Lista Ids:");
            foreach (var item in lst)
            {
                sb.Append(item.Id.IntegerValue + ";");
            }
            return sb.ToString();
        }
        #endregion

        #region Create Quantities
        public static List<Qparameter> GetListParameters(Document doc, Element e)
        {
            List<Qparameter> lst = new List<Qparameter>();
            List<Parameter> lstParam = new List<Parameter>();
            foreach (Parameter param in e.Parameters)
            {
                lstParam.Add(param);
            }
            // El elemento NO es un Material
            if (e.Category.Id != new ElementId(BuiltInCategory.OST_Materials))
            {
                Element elemType = doc.GetElement(e.GetTypeId());
                foreach (Parameter param in elemType.Parameters)
                {
                    lstParam.Add(param);
                }
            }
            foreach (Parameter param in lstParam)
            {
                if (param.Definition.ParameterType == ParameterType.Integer ||
                    param.Definition.ParameterType == ParameterType.Number ||
                    param.Definition.ParameterType == ParameterType.Area || 
                    param.Definition.ParameterType == ParameterType.Length ||
                    param.Definition.ParameterType == ParameterType.Volume)
                {
                    int id = param.Id.IntegerValue;
                    string nombre = param.Definition.Name;
                    double valor = 0;
                    string unidad = "";
                    switch (param.Definition.ParameterType)
                    {
                        case ParameterType.Invalid:
                            break;
                        case ParameterType.Text:
                            break;
                        case ParameterType.Integer:
                            valor = param.AsDouble();
                            unidad = "ud";
                            break;
                        case ParameterType.Number:
                            valor = Math.Round(param.AsDouble(), 2);
                            unidad = "ud";
                            break;
                        case ParameterType.Length:
                            valor = Math.Round(param.AsDouble() * 0.3048, 2);
                            unidad = "ml";
                            break;
                        case ParameterType.Area:
                            valor = Math.Round(param.AsDouble() * 0.3048 * 0.3048, 2);
                            unidad = "m2";
                            break;
                        case ParameterType.Volume:
                            valor = Math.Round(param.AsDouble() * 0.3048 * 0.3048 * 0.3048, 2);
                            unidad = "m3";
                            break;
                    }
                    Qparameter qParam = new Qparameter(id, nombre, valor, unidad);
                    if (!lst.Exists(x => x.id == qParam.id))
                    {
                        lst.Add(qParam);
                    }
                }
            }
            lst = lst.OrderBy(x => x.nombre).ToList();
            return lst;
        }
        public static List<Quantity> CrearQuantityFromElement(Document doc, Element e)
        {
            List<Quantity> lst = new List<Quantity>();
            try
            {
                ElementId levelId = ElementGetLevelId(e);

                string qId = "";
                ElementId grupoId = new ElementId(-1);
                if (QexOpciones.QuantityByPhase)
                {
                    qId += e.CreatedPhaseId.ToString() + "_";
                }
                if (QexOpciones.QuantityByLevel)
                {
                    qId += levelId.ToString() + "_";
                }
                if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                {
                    Element grupoE = doc.GetElement(e.GroupId);
                    ElementId idTipoGrupo = grupoE.GetTypeId();
                    qId += idTipoGrupo.ToString() + "_";
                    grupoId = idTipoGrupo;
                }
                
                qId += e.GetTypeId().ToString();
                int typeId = e.GetTypeId().IntegerValue;
                string category = e.Category.Name;
                if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                {
                    Element grupoE = doc.GetElement(e.GroupId);
                    category = grupoE.Name + " (Grupo)";
                }
                string name = e.get_Parameter(BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM).AsValueString();
                // Cuantificar recuentos
                int count = 1;
                if (e.get_Parameter(BuiltInParameter.REBAR_ELEM_QUANTITY_OF_BARS) != null)
                    count = e.get_Parameter(BuiltInParameter.REBAR_ELEM_QUANTITY_OF_BARS).AsInteger();
                // Cuantificar largos
                double length = 0;
                if (e.get_Parameter(BuiltInParameter.INSTANCE_LENGTH_PARAM) != null)
                    length = Math.Round(e.get_Parameter(BuiltInParameter.INSTANCE_LENGTH_PARAM).AsDouble()
                        * 0.3048, 2);
                if (e.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH) != null)
                    length = Math.Round(e.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble()
                        * 0.3048, 2);
                if (e.get_Parameter(BuiltInParameter.CONTINUOUS_FOOTING_LENGTH) != null)
                    length = Math.Round(e.get_Parameter(BuiltInParameter.CONTINUOUS_FOOTING_LENGTH).AsDouble()
                        * 0.3048, 2);
                if (e.get_Parameter(BuiltInParameter.REBAR_ELEM_TOTAL_LENGTH) != null)
                    length = Math.Round(e.get_Parameter(BuiltInParameter.REBAR_ELEM_TOTAL_LENGTH).AsDouble()
                        * 0.3048, 2);
                if (e.get_Parameter(BuiltInParameter.FAMILY_LINE_LENGTH_PARAM) != null)
                    length = Math.Round(e.get_Parameter(BuiltInParameter.FAMILY_LINE_LENGTH_PARAM).AsDouble()
                        * 0.3048, 2);
                // Cuantificar Areas
                double area = 0;
                if (e.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED) != null)
                    area = Math.Round(e.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble()
                        * 0.3048 * 0.3048, 2);
                // Cuantificar Volumenes
                double volume = 0;
                if (e.get_Parameter(BuiltInParameter.REINFORCEMENT_VOLUME) != null)
                    volume = Math.Round(e.get_Parameter(BuiltInParameter.REINFORCEMENT_VOLUME).AsDouble()
                        * 0.3048 * 0.3048 * 0.3048, 2);
                if (e.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED) != null)
                    volume = Math.Round(e.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble()
                        * 0.3048 * 0.3048 * 0.3048, 2);
                string phaseCreated = e.get_Parameter(BuiltInParameter.PHASE_CREATED).AsValueString();
                //Get Family Type
                ElementId _typeID = e.GetTypeId();
                Element ft = doc.GetElement(_typeID);
                double cost = ft.get_Parameter(BuiltInParameter.ALL_MODEL_COST).AsDouble();
                string descripcion = ft.get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION).AsString();
                string matriz = "1000";
                string grupo = "";
                string libro = "";
                bool visible = true;
                // Lista de Parametros
                List<Qparameter> lstParam = GetListParameters(doc, e);
                double value = 0;
                string unidad = "";
                int paramId = -1;
                int qOrden = 1;
                int grupoOrden = 1;
                int libroOrden = 1;

                //if (QexSchema.SchemaExist("Qex30"))
                //{
                Schema Qschema = QexSchema.GetSchemaByName(QexSchema._schemaQex);
                if (ft.GetEntity(Qschema).IsValid())
                {
                    Entity retrieveEntity = ft.GetEntity(Qschema);
                    matriz = retrieveEntity.Get<string>("QexMatriz");
                    string texto1 = retrieveEntity.Get<string>("QexTexto1");
                    string texto2 = retrieveEntity.Get<string>("QexTexto2");
                    if (texto2 != string.Empty)
                    {
                        List<int> ordenes = Tools.GetOrdenesFromTextOrdenes(texto2);
                        qOrden = ordenes[0];
                        grupoOrden = ordenes[1];
                        libroOrden = ordenes[2];
                    }
                    if (texto1 != string.Empty)
                    {
                        try
                        {
                            paramId = Convert.ToInt32(texto1);
                            Qparameter param = lstParam.FirstOrDefault(x => x.id == paramId);
                            if (param != null)
                            {
                                value = param.valor;
                                unidad = param.unidad;
                            }
                        }
                        catch (Exception)
                        {
                            string mensaje = "Problema con Schema del elemento: <" + e.Id.IntegerValue + "> " + 
                                e.Name;
                            Tools._lstWarnings.Add(mensaje);
                        }
                    }
                }
                //}
                //#endregion
                double totalCost = CalculateTotalCost(count, length, area, volume, cost, matriz, value);
                string medicion = CalculateMedicion(count, length, area, volume, matriz, value, unidad);
                Quantity q = new Quantity(qId, typeId, category, name, count, length, area, volume, medicion,
                        cost, totalCost, phaseCreated, matriz, descripcion, grupo, libro, visible, levelId.IntegerValue, 
                        lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                lst.Add(q);
                #region Qex v2.2 Demoliciones
                if (QexOpciones.QuantityByPhase && e.DemolishedPhaseId.IntegerValue != -1)
                {
                    string qIdDem = "dem_" + e.DemolishedPhaseId.ToString() + "_";
                    if (QexOpciones.QuantityByLevel)
                    {
                        qIdDem += levelId.ToString() + "_";
                    }
                    qIdDem += e.GetTypeId().ToString();
                    string phaseDemolish = e.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED).AsValueString();
                    category = e.Category.Name + " (Demolido)";
                    Quantity qu = new Quantity(qIdDem, typeId, category, name, count, length,
                    area, volume, medicion, cost, totalCost, phaseDemolish, matriz, descripcion, grupo, libro,
                    visible, levelId.IntegerValue, lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                    lst.Add(qu);
                }
                #endregion
            }
            catch (Exception)
            {
                string mensaje = "Problema con el elemento: <" + e.Id.IntegerValue + "> " + e.Name;
                Tools._lstWarnings.Add(mensaje);
            }
            return lst;
        }
        public static List<Quantity> CrearQuantityFromMassElement(Document doc, Element e)
        {
            List<Quantity> lst = new List<Quantity>();
            try
            {
                ElementId levelId = ElementGetLevelId(e);
                ElementId grupoId = new ElementId(-1);
                string qId = "";
                if (QexOpciones.QuantityByPhase)
                {
                    qId += e.CreatedPhaseId.ToString() + "_";
                }
                if (QexOpciones.QuantityByLevel)
                {
                    qId += levelId.ToString() + "_";
                }
                if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                {
                    Element grupoE = doc.GetElement(e.GroupId);
                    ElementId idTipoGrupo = grupoE.GetTypeId();
                    qId += idTipoGrupo.ToString() + "_";
                    grupoId = idTipoGrupo;
                    //qId += e.GroupId.ToString() + "_";
                    //grupoId = e.GroupId;
                }
                qId += e.GetTypeId().ToString();
                //string qId = e.CreatedPhaseId.ToString() + "_" + levelId.ToString() + "_" + e.GetTypeId().ToString();
                int typeId = e.GetTypeId().IntegerValue;
                string category = e.Category.Name;
                if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                {
                    Element grupoE = doc.GetElement(e.GroupId);
                    category = grupoE.Name + " (Grupo)";
                }
                string name = e.get_Parameter(BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM).AsValueString();
                // Cuantificar recuentos
                int count = 1;
                // Cuantificar largos
                double length = 0;
                // Cuantificar Areas
                double area = 0;
                if (e.get_Parameter(BuiltInParameter.MASS_GROSS_SURFACE_AREA) != null)
                    area = Math.Round(e.get_Parameter(BuiltInParameter.MASS_GROSS_SURFACE_AREA).AsDouble()
                        * 0.3048 * 0.3048, 2);
                // Cuantificar Volumenes
                double volume = 0;
                if (e.get_Parameter(BuiltInParameter.MASS_GROSS_VOLUME) != null)
                    volume = Math.Round(e.get_Parameter(BuiltInParameter.MASS_GROSS_VOLUME).AsDouble()
                        * 0.3048 * 0.3048 * 0.3048, 2);
                string phaseCreated = e.get_Parameter(BuiltInParameter.PHASE_CREATED).AsValueString();
                ElementId _typeID = e.GetTypeId();
                Element ft = doc.GetElement(_typeID);
                double cost = ft.get_Parameter(BuiltInParameter.ALL_MODEL_COST).AsDouble();
                string descripcion = ft.get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION).AsString();
                string matriz = "1000";
                string grupo = "";
                string libro = "";
                bool visible = true;
                // Lista de Parametros
                List<Qparameter> lstParam = GetListParameters(doc, e);
                double value = 0;
                string unidad = "";
                int paramId = -1;
                int qOrden = 1;
                int grupoOrden = 1;
                int libroOrden = 1;

                //if (QexSchema.SchemaExist("Qex30"))
                //{
                Schema Qschema = QexSchema.GetSchemaByName(QexSchema._schemaQex);
                if (ft.GetEntity(Qschema).IsValid())
                {
                    Entity retrieveEntity = ft.GetEntity(Qschema);
                    matriz = retrieveEntity.Get<string>("QexMatriz");
                    string texto1 = retrieveEntity.Get<string>("QexTexto1");
                    string texto2 = retrieveEntity.Get<string>("QexTexto2");
                    if (texto2 != string.Empty)
                    {
                        List<int> ordenes = Tools.GetOrdenesFromTextOrdenes(texto2);
                        qOrden = ordenes[0];
                        grupoOrden = ordenes[1];
                        libroOrden = ordenes[2];
                    }
                    if (texto1 != string.Empty)
                    {
                        try
                        {
                            paramId = Convert.ToInt32(texto1);
                            Qparameter param = lstParam.FirstOrDefault(x => x.id == paramId);
                            if (param != null)
                            {
                                value = param.valor;
                                unidad = param.unidad;
                            }
                        }
                        catch (Exception)
                        {
                            string mensaje = "Problema con Schema del elemento: <" + e.Id.IntegerValue + "> " +
                                e.Name;
                            Tools._lstWarnings.Add(mensaje);
                        }
                    }
                }
                //}
                //#endregion
                double totalCost = CalculateTotalCost(count, length, area, volume, cost, matriz, value);
                string medicion = CalculateMedicion(count, length, area, volume, matriz, value, unidad);
                Quantity q = new Quantity(qId, typeId, category, name, count, length, area, volume, medicion,
                        cost, totalCost, phaseCreated, matriz, descripcion, grupo, libro, visible, levelId.IntegerValue, 
                        lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                lst.Add(q);
                #region Qex v2.2 Demoliciones
                if (QexOpciones.QuantityByPhase && e.DemolishedPhaseId.IntegerValue != -1)
                {
                    string qIdDem = "dem_" + e.DemolishedPhaseId.ToString() + "_";
                    if (QexOpciones.QuantityByLevel)
                    {
                        qIdDem += levelId.ToString() + "_";
                    }
                    qIdDem += e.GetTypeId().ToString();
                    category = e.Category.Name + " (Demolido)";
                    string phaseDemolish = e.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED).AsValueString();
                    Quantity qu = new Quantity(qIdDem, typeId, category, name, count, length,
                    area, volume, medicion, cost, totalCost, phaseDemolish, matriz, descripcion, grupo, libro,
                    visible, levelId.IntegerValue, lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                    lst.Add(qu);
                }
                #endregion
            }
            catch (Exception)
            {
                string mensaje = "Problema con el elemento: <" + e.Id.IntegerValue + "> " + e.Name;
                Tools._lstWarnings.Add(mensaje);
            }
            return lst;
        }
        public static List<Quantity> CrearQuantityFromTopography(Document doc, Element e)
        {
            List<Quantity> lst = new List<Quantity>();
            try
            {
                ElementId levelId = ElementGetLevelId(e);
                ElementId grupoId = new ElementId(-1);
                string qId = "";
                if (QexOpciones.QuantityByPhase)
                {
                    qId += e.CreatedPhaseId.ToString() + "_";
                }
                if (QexOpciones.QuantityByLevel)
                {
                    qId += levelId.ToString() + "_";
                }
                if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                {
                    Element grupoE = doc.GetElement(e.GroupId);
                    ElementId idTipoGrupo = grupoE.GetTypeId();
                    qId += idTipoGrupo.ToString() + "_";
                    grupoId = idTipoGrupo;
                    //qId += e.GroupId.ToString() + "_";
                    //grupoId = e.GroupId;
                }
                qId += e.GetTypeId().ToString();
                //string qId = e.CreatedPhaseId.ToString() + "_" + levelId.ToString() + "_" + e.GetTypeId().ToString();
                int typeId = e.Id.IntegerValue;
                string category = e.Category.Name;
                if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                {
                    Element grupoE = doc.GetElement(e.GroupId);
                    category = grupoE.Name + " (Grupo)";
                }
                string name = e.get_Parameter(BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM).AsValueString();
                // Cuantificar recuentos
                int count = 1;
                // Cuantificar largos
                double length = 0;
                // Cuantificar Areas
                double area = 0;
                if (e.get_Parameter(BuiltInParameter.MASS_GROSS_SURFACE_AREA) != null)
                    area = Math.Round(e.get_Parameter(BuiltInParameter.MASS_GROSS_SURFACE_AREA).AsDouble()
                        * 0.3048 * 0.3048, 2);
                // Cuantificar Volumenes
                double volume = 0;
                if (e.get_Parameter(BuiltInParameter.MASS_GROSS_VOLUME) != null)
                    volume = Math.Round(e.get_Parameter(BuiltInParameter.MASS_GROSS_VOLUME).AsDouble()
                        * 0.3048 * 0.3048 * 0.3048, 2);
                string phaseCreated = e.get_Parameter(BuiltInParameter.PHASE_CREATED).AsValueString();
                if (e.Category.Id == new ElementId(BuiltInCategory.OST_MassFloor))
                {
                    qId = e.CreatedPhaseId.ToString() + "_" + e.Id.ToString();
                    name = "Elemento <" + e.Id.ToString() + ">";
                    length = Math.Round(e.get_Parameter(BuiltInParameter.LEVEL_DATA_FLOOR_PERIMETER).AsDouble()
                        * 0.3048, 2);
                    area = Math.Round(e.get_Parameter(BuiltInParameter.LEVEL_DATA_FLOOR_AREA).AsDouble()
                        * 0.3048 * 0.3048, 2);
                    volume = Math.Round(e.get_Parameter(BuiltInParameter.LEVEL_DATA_VOLUME).AsDouble()
                        * 0.3048 * 0.3048 * 0.3048, 2);
                }
                //Get Family Type
                ElementId _typeID = e.Id;
                Element ft = doc.GetElement(_typeID);
                double cost = 0;
                string descripcion = "";
                string matriz = "1000";
                string grupo = "";
                string libro = "";
                bool visible = true;
                // Lista de Parametros
                List<Qparameter> lstParam = GetListParameters(doc, e);
                double value = 0;
                string unidad = "";
                int paramId = -1;
                int qOrden = 1;
                int grupoOrden = 1;
                int libroOrden = 1;

                //if (QexSchema.SchemaExist("Qex30"))
                //{
                Schema Qschema = QexSchema.GetSchemaByName(QexSchema._schemaQex);
                if (ft.GetEntity(Qschema).IsValid())
                {
                    Entity retrieveEntity = ft.GetEntity(Qschema);
                    matriz = retrieveEntity.Get<string>("QexMatriz");
                    string texto1 = retrieveEntity.Get<string>("QexTexto1");
                    string texto2 = retrieveEntity.Get<string>("QexTexto2");
                    if (texto2 != string.Empty)
                    {
                        List<int> ordenes = Tools.GetOrdenesFromTextOrdenes(texto2);
                        qOrden = ordenes[0];
                        grupoOrden = ordenes[1];
                        libroOrden = ordenes[2];
                    }
                    if (texto1 != string.Empty)
                    {
                        try
                        {
                            paramId = Convert.ToInt32(texto1);
                            Qparameter param = lstParam.FirstOrDefault(x => x.id == paramId);
                            if (param != null)
                            {
                                value = param.valor;
                                unidad = param.unidad;
                            }
                        }
                        catch (Exception)
                        {
                            string mensaje = "Problema con Schema del elemento: <" + e.Id.IntegerValue + "> " +
                                e.Name;
                            Tools._lstWarnings.Add(mensaje);
                        }
                    }
                }
                //}
                //#endregion
                double totalCost = CalculateTotalCost(count, length, area, volume, cost, matriz, value);
                string medicion = CalculateMedicion(count, length, area, volume, matriz, value, unidad);
                Quantity q = new Quantity(qId, typeId, category, name, count, length, area, volume, medicion,
                        cost, totalCost, phaseCreated, matriz, descripcion, grupo, libro, visible, levelId.IntegerValue, 
                        lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                lst.Add(q);
                #region Qex v2.2 Demoliciones
                if (QexOpciones.QuantityByPhase && e.DemolishedPhaseId.IntegerValue != -1)
                {
                    string qIdDem = "dem_" + e.DemolishedPhaseId.ToString() + "_";
                    if (QexOpciones.QuantityByLevel)
                    {
                        qIdDem += levelId.ToString() + "_";
                    }
                    qIdDem += e.GetTypeId().ToString();
                    category = e.Category.Name + " (Demolido)";
                    string phaseDemolish = e.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED).AsValueString();
                    Quantity qu = new Quantity(qIdDem, typeId, category, name, count, length,
                    area, volume, medicion, cost, totalCost, phaseDemolish, matriz, descripcion, grupo, libro,
                    visible, levelId.IntegerValue, lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                    lst.Add(qu);
                }
                #endregion
            }
            catch (Exception)
            {
                string mensaje = "Problema con el elemento: <" + e.Id.IntegerValue + "> " + e.Name;
                Tools._lstWarnings.Add(mensaje);
            }
            return lst;
        }
        public static List<Quantity> CrearQuantityFromMaterials(Document doc, Element e)
        {
            List<Quantity> lst = new List<Quantity>();
            try
            {
                foreach (Material mat in GetMaterialsFromElement(doc, e))
                {
                    // Name
                    ElementId levelId = ElementGetLevelId(e);
                    ElementId grupoId = new ElementId(-1);
                    string qId = "";
                    if (QexOpciones.QuantityByPhase)
                    {
                        qId += e.CreatedPhaseId.ToString() + "_";
                    }
                    if (QexOpciones.QuantityByLevel)
                    {
                        qId += levelId.ToString() + "_";
                    }
                    if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                    {
                        Element grupoE = doc.GetElement(e.GroupId);
                        ElementId idTipoGrupo = grupoE.GetTypeId();
                        qId += idTipoGrupo.ToString() + "_";
                        grupoId = idTipoGrupo;
                        //qId += e.GroupId.ToString() + "_";
                        //grupoId = e.GroupId;
                    }
                    string category = e.Category.Name + " (Materiales)";
                    if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                    {
                        Element grupoE = doc.GetElement(e.GroupId);
                        category = grupoE.Name + " (Materiales)" + " (Grupo)";
                    }
                    qId += category + "_" + mat.Id.ToString();
                    int typeId = mat.Id.IntegerValue;
                    string name = mat.Name;
                    // Cuantificar recuentos
                    int count = 1;
                    // Cuantificar largos
                    double length = 0;
                    if (e.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH) != null)
                        length = Math.Round(e.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble()
                            * 0.3048, 2);
                    // Cuantificar Areas
                    double area = 0;
                    area = Math.Round(e.GetMaterialArea(mat.Id, false) * 0.3048 * 0.3048, 2);
                    // Cuantificar Volumenes
                    double volume = 0;
                    volume = Math.Round(e.GetMaterialVolume(mat.Id) * 0.3048 * 0.3048 * 0.3048, 2);
                    string phaseCreated = e.get_Parameter(BuiltInParameter.PHASE_CREATED).AsValueString();
                    //Get Family Type
                    double cost = mat.get_Parameter(BuiltInParameter.ALL_MODEL_COST).AsDouble();
                    string matriz = "0010";
                    string grupo = "";
                    string libro = "";
                    bool visible = true;
                    // Lista de Parametros
                    List<Qparameter> lstParam = GetListParameters(doc, mat);
                    double value = 0;
                    string unidad = "";
                    int paramId = -1;
                    int qOrden = 1;
                    int grupoOrden = 1;
                    int libroOrden = 1;

                    //if (QexSchema.SchemaExist("Qex30"))
                    //{
                    Schema Qschema = QexSchema.GetSchemaByName(QexSchema._schemaQex);
                    if (mat.GetEntity(Qschema).IsValid())
                    {
                        Entity retrieveEntity = mat.GetEntity(Qschema);
                        matriz = retrieveEntity.Get<string>("QexMatriz");
                        string texto1 = retrieveEntity.Get<string>("QexTexto1");
                        string texto2 = retrieveEntity.Get<string>("QexTexto2");
                        if (texto2 != string.Empty)
                        {
                            List<int> ordenes = Tools.GetOrdenesFromTextOrdenes(texto2);
                            qOrden = ordenes[0];
                            grupoOrden = ordenes[1];
                            libroOrden = ordenes[2];
                        }
                        if (texto1 != string.Empty)
                        {
                            try
                            {
                                paramId = Convert.ToInt32(texto1);
                                Qparameter param = lstParam.FirstOrDefault(x => x.id == paramId);
                                if (param != null)
                                {
                                    value = param.valor;
                                    unidad = param.unidad;
                                }
                            }
                            catch (Exception)
                            {
                                string mensaje = "Problema con Schema del elemento: <" + e.Id.IntegerValue + "> " +
                                    e.Name;
                                Tools._lstWarnings.Add(mensaje);
                            }
                        }
                    }
                    //}
                    string descripcion = mat.get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION).AsString();
                    double totalCost = CalculateTotalCost(count, length, area, volume, cost, matriz, value);
                    string medicion = CalculateMedicion(count, length, area, volume, matriz, value, unidad);

                    Quantity q = new Quantity(qId, typeId, category, name, count, length, area, volume,
                        medicion, cost, totalCost, phaseCreated, matriz, descripcion, grupo, libro, visible, 
                        levelId.IntegerValue, lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                    lst.Add(q);

                    #region ver 2.2 Demoliciones
                    if (QexOpciones.QuantityByPhase && e.DemolishedPhaseId.IntegerValue != -1)
                    {
                        string qIdDem = "dem_" + e.DemolishedPhaseId.ToString() + "_";
                        if (QexOpciones.QuantityByLevel)
                        {
                            qIdDem += levelId.ToString() + "_";
                        }
                        category = e.Category.Name + " (Demolido)";
                        qIdDem += category + "_" + mat.Id.ToString();

                        string phaseDemolish = e.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED).AsValueString();
                        Quantity qu = new Quantity(qIdDem, typeId, category, name, count, length, area,
                            volume, medicion, cost, totalCost, phaseDemolish, matriz, descripcion, grupo,
                            libro, visible, levelId.IntegerValue, lstParam, value, unidad, paramId, grupoId.IntegerValue
                            , qOrden, grupoOrden, libroOrden);
                        lst.Add(qu);
                    }
                    #endregion
                }
            }
            catch (Exception)
            {
                string mensaje = "Problema con un Material del elemento: <" + e.Id.IntegerValue + "> " + e.Name;
                Tools._lstWarnings.Add(mensaje);
            }
            return lst;
        }
        public static List<Quantity> CrearQuantityPinturas(Document doc, Element e)
        {
            List<Quantity> lst = new List<Quantity>();
            try
            {
                foreach (Face f in getFaces(doc, e))
                {
                    if (doc.IsPainted(e.Id, f))
                    {
                        ElementId levelId = ElementGetLevelId(e);
                        ElementId grupoId = new ElementId(-1);
                        ElementId id = doc.GetPaintedMaterial(e.Id, f);
                        Material mat = doc.GetElement(id) as Material;
                        // Name
                        string qId = "";
                        if (QexOpciones.QuantityByPhase)
                        {
                            qId += e.CreatedPhaseId.ToString() + "_";
                        }
                        if (QexOpciones.QuantityByLevel)
                        {
                            qId += levelId.ToString() + "_";
                        }
                        if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                        {
                            Element grupoE = doc.GetElement(e.GroupId);
                            ElementId idTipoGrupo = grupoE.GetTypeId();
                            qId += idTipoGrupo.ToString() + "_";
                            grupoId = idTipoGrupo;
                            //qId += e.GroupId.ToString() + "_";
                            //grupoId = e.GroupId;
                        }
                        string category = e.Category.Name + " (Pinturas)";
                        if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                        {
                            Element grupoE = doc.GetElement(e.GroupId);
                            category = grupoE.Name + " (Pinturas)" + " (Grupo)";
                        }
                        qId += category + "_" + mat.Id.ToString();
                        //string qId = e.CreatedPhaseId.ToString() + "_" + levelId.ToString() + "_" +
                        //    mat.Id.ToString();
                        int typeId = mat.Id.IntegerValue;
                        string name = mat.Name;
                        // Cuantificar recuentos
                        int count = 1;
                        // Cuantificar largos
                        double length = 0;
                        // Cuantificar Areas
                        double area = 0;
                        area = Math.Round(f.Area * 0.3048 * 0.3048, 2);
                        // Cuantificar Volumenes
                        double volume = 0;

                        string phaseCreated = e.get_Parameter(BuiltInParameter.PHASE_CREATED).AsValueString();
                        double cost = mat.get_Parameter(BuiltInParameter.ALL_MODEL_COST).AsDouble();
                        string matriz = "0010";
                        string grupo = "";
                        string libro = "";
                        bool visible = true;
                        // Lista de Parametros
                        List<Qparameter> lstParam = GetListParameters(doc, mat);
                        double value = 0;
                        string unidad = "";
                        int paramId = -1;
                        int qOrden = 1;
                        int grupoOrden = 1;
                        int libroOrden = 1;

                        //if (QexSchema.SchemaExist("Qex30"))
                        //{
                        Schema Qschema = QexSchema.GetSchemaByName(QexSchema._schemaQex);
                        if (mat.GetEntity(Qschema).IsValid())
                        {
                            Entity retrieveEntity = mat.GetEntity(Qschema);
                            matriz = retrieveEntity.Get<string>("QexMatriz");
                            string texto1 = retrieveEntity.Get<string>("QexTexto1");
                            string texto2 = retrieveEntity.Get<string>("QexTexto2");
                            if (texto2 != string.Empty)
                            {
                                List<int> ordenes = Tools.GetOrdenesFromTextOrdenes(texto2);
                                qOrden = ordenes[0];
                                grupoOrden = ordenes[1];
                                libroOrden = ordenes[2];
                            }
                            if (texto1 != string.Empty)
                            {
                                try
                                {
                                    paramId = Convert.ToInt32(texto1);
                                    Qparameter param = lstParam.FirstOrDefault(x => x.id == paramId);
                                    if (param != null)
                                    {
                                        value = param.valor;
                                        unidad = param.unidad;
                                    }
                                }
                                catch (Exception)
                                {
                                    string mensaje = "Problema con Schema del elemento: <" + e.Id.IntegerValue + "> " +
                                        e.Name;
                                    Tools._lstWarnings.Add(mensaje);
                                }
                            }
                        }
                        //}
                        string descripcion = mat.get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION).AsString();
                        double totalCost = CalculateTotalCost(count, length, area, volume, cost, matriz, value);
                        string medicion = CalculateMedicion(count, length, area, volume, matriz, value, unidad);
                        Quantity q = new Quantity(qId, typeId, category, name, count, length, area, volume,
                                medicion, cost, totalCost, phaseCreated, matriz, descripcion, grupo, libro,
                                visible, levelId.IntegerValue, lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                        lst.Add(q);

                        #region ver 2.2 Demoliciones
                        if (QexOpciones.QuantityByPhase && e.DemolishedPhaseId.IntegerValue != -1)
                        {
                            string qIdDem = "dem_" + e.DemolishedPhaseId.ToString() + "_";
                            if (QexOpciones.QuantityByLevel)
                            {
                                qIdDem += levelId.ToString() + "_";
                            }
                            category = e.Category.Name + " (Pinturas) (Demolido)";
                            qIdDem += category + "_" + mat.Id.ToString();
                            string phaseDemolish = e.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED).AsValueString();
                            Quantity qu = new Quantity(qIdDem, typeId, category, name, count, length, area,
                                volume, medicion, cost, totalCost, phaseDemolish, matriz, descripcion, grupo,
                                libro, visible, levelId.IntegerValue, lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                            lst.Add(qu);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception)
            {
                string mensaje = "Problema con la pintura del elemento: <" + e.Id.IntegerValue + "> " + e.Name;
                Tools._lstWarnings.Add(mensaje);
            }
            return lst;
        }
        public static List<Quantity> CrearQuantityFromRebar(Document doc, Rebar e)
        {
            List<Quantity> lst = new List<Quantity>();
            try
            {
                ElementId levelId = ElementGetLevelId(e);
                ElementId grupoId = new ElementId(-1);
                string diam = e.get_Parameter(BuiltInParameter.REBAR_BAR_DIAMETER).AsValueString();
                string qId = "";
                if (QexOpciones.QuantityByPhase)
                {
                    qId += e.CreatedPhaseId.ToString() + "_";
                }
                if (QexOpciones.QuantityByLevel)
                {
                    qId += levelId.ToString() + "_";
                }
                if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                {
                    Element grupoE = doc.GetElement(e.GroupId);
                    ElementId idTipoGrupo = grupoE.GetTypeId();
                    qId += idTipoGrupo.ToString() + "_";
                    grupoId = idTipoGrupo;
                }
                qId += e.GetTypeId().ToString() + "_";
                int typeId = e.GetTypeId().IntegerValue;
                string category = e.Category.Name;
                if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                {
                    Element grupoE = doc.GetElement(e.GroupId);
                    category = grupoE.Name + " (Grupo)";
                }
                string name = e.get_Parameter(BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM).AsValueString();

                if (QexOpciones.rebarMarca)
                {
                    if (e.get_Parameter(BuiltInParameter.REBAR_ELEM_HOST_MARK).AsString() != "")
                    {
                        name = e.get_Parameter(BuiltInParameter.REBAR_ELEM_HOST_MARK).AsString() +
                        " >> " + e.get_Parameter(BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM).AsValueString();
                        qId += e.get_Parameter(BuiltInParameter.REBAR_ELEM_HOST_MARK).AsString();
                    }
                    else
                    {
                        name = "Sin Marca >> " + e.get_Parameter(BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM).AsValueString();
                    }
                }
                // Cuantificar recuentos
                int count = 1;
                if (e.get_Parameter(BuiltInParameter.REBAR_ELEM_QUANTITY_OF_BARS) != null)
                    count = e.get_Parameter(BuiltInParameter.REBAR_ELEM_QUANTITY_OF_BARS).AsInteger();
                // Cuantificar largos
                double length = 0;
                if (e.get_Parameter(BuiltInParameter.REBAR_ELEM_TOTAL_LENGTH) != null)
                    length = Math.Round(e.get_Parameter(BuiltInParameter.REBAR_ELEM_TOTAL_LENGTH).AsDouble()
                        * 0.3048, 2);
                // Cuantificar Areas
                double area = 0;
                // Cuantificar Volumenes
                double volume = 0;
                if (e.get_Parameter(BuiltInParameter.REINFORCEMENT_VOLUME) != null)
                    volume = Math.Round(e.get_Parameter(BuiltInParameter.REINFORCEMENT_VOLUME).AsDouble()
                        * 0.3048 * 0.3048 * 0.3048, 2);
                string phaseCreated = e.get_Parameter(BuiltInParameter.PHASE_CREATED).AsValueString();
                //Get Family Type
                ElementId _typeID = e.GetTypeId();
                Element ft = doc.GetElement(_typeID);
                double cost = ft.get_Parameter(BuiltInParameter.ALL_MODEL_COST).AsDouble();
                string descripcion = ft.get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION).AsString();
                string matriz = "0100";
                string grupo = "";
                string libro = "";
                bool visible = true;
                // Lista de Parametros
                List<Qparameter> lstParam = GetListParameters(doc, e);
                double value = 0;
                string unidad = "";
                int paramId = -1;
                int qOrden = 1;
                int grupoOrden = 1;
                int libroOrden = 1;

                //if (QexSchema.SchemaExist("Qex30"))
                //{
                Schema Qschema = QexSchema.GetSchemaByName(QexSchema._schemaQex);
                if (ft.GetEntity(Qschema).IsValid())
                {
                    Entity retrieveEntity = ft.GetEntity(Qschema);
                    matriz = retrieveEntity.Get<string>("QexMatriz");
                    string texto1 = retrieveEntity.Get<string>("QexTexto1");
                    string texto2 = retrieveEntity.Get<string>("QexTexto2");
                    if (texto2 != string.Empty)
                    {
                        List<int> ordenes = Tools.GetOrdenesFromTextOrdenes(texto2);
                        qOrden = ordenes[0];
                        grupoOrden = ordenes[1];
                        libroOrden = ordenes[2];
                    }
                    if (texto1 != string.Empty)
                    {
                        try
                        {
                            paramId = Convert.ToInt32(texto1);
                            Qparameter param = lstParam.FirstOrDefault(x => x.id == paramId);
                            if (param != null)
                            {
                                value = param.valor;
                                unidad = param.unidad;
                            }
                        }
                        catch (Exception)
                        {
                            string mensaje = "Problema con Schema del elemento: <" + e.Id.IntegerValue + "> " +
                                e.Name;
                            Tools._lstWarnings.Add(mensaje);
                        }
                    }
                }
                //}
                double totalCost = CalculateTotalCost(count, length, area, volume, cost, matriz, value);
                string medicion = CalculateMedicion(count, length, area, volume, matriz, value, unidad);

                Quantity q = new Quantity(qId, typeId, category, name, count, length, area, volume, medicion,
                    cost, totalCost, phaseCreated, matriz, descripcion, grupo, libro, visible, levelId.IntegerValue, 
                    lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                lst.Add(q);

                #region Qex v2.2 Demoliciones
                if (QexOpciones.QuantityByPhase && e.DemolishedPhaseId.IntegerValue != -1)
                {
                    string qIdDem = "dem_" + e.DemolishedPhaseId.ToString() + "_";
                    if (QexOpciones.QuantityByLevel)
                    {
                        qIdDem += levelId.ToString() + "_";
                    }
                    qIdDem += e.GetTypeId().ToString() + "_";
                    if (QexOpciones.rebarMarca)
                    {
                        if (e.get_Parameter(BuiltInParameter.REBAR_ELEM_HOST_MARK).AsString() != "")
                        {
                            qIdDem += e.get_Parameter(BuiltInParameter.REBAR_ELEM_HOST_MARK).AsString();
                        }
                        else
                        {
                            // NAda
                        }
                    }
                    category = e.Category.Name + " (Demolido)";
                    string phaseDemolish = e.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED).AsValueString();
                    Quantity qu = new Quantity(qIdDem, typeId, category, name, count, length,
                    area, volume, medicion, cost, totalCost, phaseDemolish, matriz, descripcion, grupo, libro,
                    visible, levelId.IntegerValue, lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                    lst.Add(qu);
                }
                #endregion
            }
            catch (Exception)
            {
                string mensaje = "Problema con el elemento: <" + e.Id.IntegerValue + "> " + e.Name;
                Tools._lstWarnings.Add(mensaje);
            }
            return lst;
        }
        public static List<Quantity> CrearQuantityFromMep(Document doc, Element e)
        {
            List<Quantity> lst = new List<Quantity>();
            try
            {
                ElementId levelId = ElementGetLevelId(e);
                ElementId grupoId = new ElementId(-1);
                int typeId = e.GetTypeId().IntegerValue;
                string category = e.Category.Name;
                if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                {
                    Element grupoE = doc.GetElement(e.GroupId);
                    category = grupoE.Name + " (Grupo)";
                    grupoId = e.GroupId;
                }
                string systemType = "Sin Sistema";
                if (e.get_Parameter(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM) != null)
                    systemType = e.get_Parameter(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM).AsValueString();
                if (e.get_Parameter(BuiltInParameter.RBS_DUCT_SYSTEM_TYPE_PARAM) != null)
                    systemType = e.get_Parameter(BuiltInParameter.RBS_DUCT_SYSTEM_TYPE_PARAM).AsValueString();
                if (e.get_Parameter(BuiltInParameter.RBS_CTC_SERVICE_TYPE) != null)
                {
                    if (e.get_Parameter(BuiltInParameter.RBS_CTC_SERVICE_TYPE).HasValue == true)
                    {
                        systemType = e.get_Parameter(BuiltInParameter.RBS_CTC_SERVICE_TYPE).AsString();
                    }
                }
                string size = e.get_Parameter(BuiltInParameter.RBS_CALCULATED_SIZE).AsString();

                string name = systemType + " >> " + e.get_Parameter(BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM).AsValueString()
                    + " >> " + size;
                string qId = "";
                if (QexOpciones.QuantityByPhase)
                {
                    qId += e.CreatedPhaseId.ToString() + "_";
                }
                if (QexOpciones.QuantityByLevel)
                {
                    qId += levelId.ToString() + "_";
                }
                if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                {
                    Element grupoE = doc.GetElement(e.GroupId);
                    ElementId idTipoGrupo = grupoE.GetTypeId();
                    qId += idTipoGrupo.ToString() + "_";
                    grupoId = idTipoGrupo;
                }
                qId += systemType + "_" + e.GetTypeId().ToString() + "_" + size;
                // Cuantificar recuentos
                int count = 1;
                // Cuantificar largos
                double length = 0;
                if (e.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH) != null)
                    length = Math.Round(e.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble()
                        * 0.3048, 2);
                // Cuantificar Areas
                double area = 0;
                // Cuantificar Volumenes
                double volume = 0;
                if (e.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED) != null)
                    volume = Math.Round(e.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble()
                        * 0.3048 * 0.3048 * 0.3048, 2);
                string phaseCreated = e.get_Parameter(BuiltInParameter.PHASE_CREATED).AsValueString();
                //Get Family Type
                ElementId _typeID = e.GetTypeId();
                Element ft = doc.GetElement(_typeID);
                double cost = ft.get_Parameter(BuiltInParameter.ALL_MODEL_COST).AsDouble();
                string descripcion = ft.get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION).AsString();
                string matriz = "1000";
                string grupo = "";
                string libro = "";
                bool visible = true;
                // Lista de Parametros
                List<Qparameter> lstParam = GetListParameters(doc, e);
                double value = 0;
                string unidad = "";
                int paramId = -1;
                int qOrden = 1;
                int grupoOrden = 1;
                int libroOrden = 1;

                //if (QexSchema.SchemaExist("Qex30"))
                //{
                Schema Qschema = QexSchema.GetSchemaByName(QexSchema._schemaQex);
                if (ft.GetEntity(Qschema).IsValid())
                {
                    Entity retrieveEntity = ft.GetEntity(Qschema);
                    matriz = retrieveEntity.Get<string>("QexMatriz");
                    string texto1 = retrieveEntity.Get<string>("QexTexto1");
                    string texto2 = retrieveEntity.Get<string>("QexTexto2");
                    if (texto2 != string.Empty)
                    {
                        List<int> ordenes = Tools.GetOrdenesFromTextOrdenes(texto2);
                        qOrden = ordenes[0];
                        grupoOrden = ordenes[1];
                        libroOrden = ordenes[2];
                    }
                    if (texto1 != string.Empty)
                    {
                        try
                        {
                            paramId = Convert.ToInt32(texto1);
                            Qparameter param = lstParam.FirstOrDefault(x => x.id == paramId);
                            if (param != null)
                            {
                                value = param.valor;
                                unidad = param.unidad;
                            }
                        }
                        catch (Exception)
                        {
                            string mensaje = "Problema con Schema del elemento: <" + e.Id.IntegerValue + "> " +
                                e.Name;
                            Tools._lstWarnings.Add(mensaje);
                        }
                    }
                }
                //}
                double totalCost = CalculateTotalCost(count, length, area, volume, cost, matriz, value);
                string medicion = CalculateMedicion(count, length, area, volume, matriz, value, unidad);

                Quantity q = new Quantity(qId, typeId, category, name, count, length, area, volume, medicion,
                    cost, totalCost, phaseCreated, matriz, descripcion, grupo, libro, visible, levelId.IntegerValue, 
                    lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                lst.Add(q);

                #region Qex v2.2 Demoliciones
                if (QexOpciones.QuantityByPhase && e.DemolishedPhaseId.IntegerValue != -1)
                {
                    string qIdDem = "dem_" + e.DemolishedPhaseId.ToString() + "_";
                    if (QexOpciones.QuantityByLevel)
                    {
                        qIdDem += levelId.ToString() + "_";
                    }
                    qIdDem += systemType + "_" + e.GetTypeId().ToString() + "_" + size;
                    category = e.Category.Name + " (Demolido)";
                    string phaseDemolish = e.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED).AsValueString();
                    Quantity qu = new Quantity(qIdDem, typeId, category, name, count, length,
                        area, volume, medicion, cost, totalCost, phaseDemolish, matriz, descripcion, grupo,
                        libro, visible, levelId.IntegerValue, lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                    lst.Add(qu);
                }
                #endregion
            }
            catch (Exception)
            {
                string mensaje = "Problema con el elemento: <" + e.Id.IntegerValue + "> " + e.Name;
                Tools._lstWarnings.Add(mensaje);
            }
            return lst;
        }
        public static List<Quantity> CrearQuantityFromAreaReinf(Document doc, AreaReinforcement ar)
        {
            List<Quantity> lst = new List<Quantity>();
            try
            {
                foreach (ElementId id in ar.GetRebarInSystemIds())
                {
                    Element e = doc.GetElement(id);
                    ElementId levelId = ElementGetLevelId(ar);
                    ElementId grupoId = new ElementId(-1);
                    string qId = "";
                    if (QexOpciones.QuantityByPhase)
                    {
                        qId += e.CreatedPhaseId.ToString() + "_";
                    }
                    if (QexOpciones.QuantityByLevel)
                    {
                        qId += levelId.ToString() + "_";
                    }
                    if (QexOpciones.QuantityByGroup && e.GroupId.IntegerValue != -1)
                    {
                        Element grupoE = doc.GetElement(e.GroupId);
                        ElementId idTipoGrupo = grupoE.GetTypeId();
                        qId += idTipoGrupo.ToString() + "_";
                        grupoId = idTipoGrupo;
                    }
                    qId += e.GetTypeId().ToString() + "_";
                    int typeId = e.GetTypeId().IntegerValue;
                    string category = ar.Category.Name;
                    if (QexOpciones.QuantityByGroup && ar.GroupId.IntegerValue != -1)
                    {
                        Element grupoE = doc.GetElement(ar.GroupId);
                        category = grupoE.Name + " (Grupo)";
                    }
                    string name = e.get_Parameter(BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM).AsValueString();
                    if (QexOpciones.rebarMarca)
                    {
                        if (e.get_Parameter(BuiltInParameter.REBAR_ELEM_HOST_MARK).AsString() != "")
                        {
                            name = e.get_Parameter(BuiltInParameter.REBAR_ELEM_HOST_MARK).AsString() +
                            " >> " + e.get_Parameter(BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM).AsValueString();
                            qId += e.get_Parameter(BuiltInParameter.REBAR_ELEM_HOST_MARK).AsString();
                        }
                        else
                        {
                            name = "Sin Marca >> " + e.get_Parameter(BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM).AsValueString();
                        }
                    }
                    // Cuantificar recuentos
                    int count = 1;
                    if (e.get_Parameter(BuiltInParameter.REBAR_ELEM_QUANTITY_OF_BARS) != null)
                        count = e.get_Parameter(BuiltInParameter.REBAR_ELEM_QUANTITY_OF_BARS).AsInteger();
                    // Cuantificar largos
                    double length = 0;
                    if (e.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH) != null)
                        length = Math.Round(e.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble()
                            * 0.3048, 2);
                    if (e.get_Parameter(BuiltInParameter.REBAR_ELEM_TOTAL_LENGTH) != null)
                        length = Math.Round(e.get_Parameter(BuiltInParameter.REBAR_ELEM_TOTAL_LENGTH).AsDouble()
                            * 0.3048, 2);
                    // Cuantificar Areas
                    double area = 0;
                    if (e.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED) != null)
                        area = Math.Round(e.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble()
                            * 0.3048 * 0.3048, 2);
                    // Cuantificar Volumenes
                    double volume = 0;
                    if (e.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED) != null)
                        volume = Math.Round(e.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble()
                            * 0.3048 * 0.3048 * 0.3048, 2);
                    string phaseCreated = ar.get_Parameter(BuiltInParameter.PHASE_CREATED).AsValueString();
                    //Get Family Type
                    ElementId _typeID = e.GetTypeId();
                    Element ft = doc.GetElement(_typeID);
                    double cost = ft.get_Parameter(BuiltInParameter.ALL_MODEL_COST).AsDouble();
                    string descripcion = ft.get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION).AsString();
                    string matriz = "0100";
                    string grupo = "";
                    string libro = "";
                    bool visible = true;
                    // Lista de Parametros
                    List<Qparameter> lstParam = GetListParameters(doc, ar);
                    double value = 0;
                    string unidad = "";
                    int paramId = -1;
                    int qOrden = 1;
                    int grupoOrden = 1;
                    int libroOrden = 1;

                    //if (QexSchema.SchemaExist("Qex30"))
                    //{
                    Schema Qschema = QexSchema.GetSchemaByName(QexSchema._schemaQex);
                    if (ft.GetEntity(Qschema).IsValid())
                    {
                        Entity retrieveEntity = ft.GetEntity(Qschema);
                        matriz = retrieveEntity.Get<string>("QexMatriz");
                        string texto1 = retrieveEntity.Get<string>("QexTexto1");
                        string texto2 = retrieveEntity.Get<string>("QexTexto2");
                        if (texto2 != string.Empty)
                        {
                            List<int> ordenes = Tools.GetOrdenesFromTextOrdenes(texto2);
                            qOrden = ordenes[0];
                            grupoOrden = ordenes[1];
                            libroOrden = ordenes[2];
                        }
                        if (texto1 != string.Empty)
                        {
                            try
                            {
                                paramId = Convert.ToInt32(texto1);
                                Qparameter param = lstParam.FirstOrDefault(x => x.id == paramId);
                                if (param != null)
                                {
                                    value = param.valor;
                                    unidad = param.unidad;
                                }
                            }
                            catch (Exception)
                            {
                                string mensaje = "Problema con Schema del elemento: <" + ar.Id.IntegerValue + "> " +
                                    ar.Name;
                                Tools._lstWarnings.Add(mensaje);
                            }
                        }
                    }
                    //}
                    double totalCost = CalculateTotalCost(count, length, area, volume, cost, matriz, value);
                    string medicion = CalculateMedicion(count, length, area, volume, matriz, value, unidad);

                    Quantity q = new Quantity(qId, typeId, category, name, count, length, area, volume, medicion,
                        cost, totalCost, phaseCreated, matriz, descripcion, grupo, libro, visible, levelId.IntegerValue, 
                        lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                    lst.Add(q);

                    #region Qex v2.2 Demoliciones
                    if (QexOpciones.QuantityByPhase && e.DemolishedPhaseId.IntegerValue != -1)
                    {
                        string qIdDem = "dem_" + e.DemolishedPhaseId.ToString() + "_";
                        if (QexOpciones.QuantityByLevel)
                        {
                            qIdDem += levelId.ToString() + "_";
                        }
                        qIdDem += e.GetTypeId().ToString() + "_";
                        if (QexOpciones.rebarMarca)
                        {
                            if (e.get_Parameter(BuiltInParameter.REBAR_ELEM_HOST_MARK).AsString() != "")
                            {
                                qIdDem += e.get_Parameter(BuiltInParameter.REBAR_ELEM_HOST_MARK).AsString();
                            }
                            else
                            {
                                // NAda
                            }
                        }
                        string phaseDemolish = e.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED).AsValueString();
                        category = ar.Category.Name + " (Demolido)";
                        Quantity qu = new Quantity(qIdDem, typeId, category, name, count, length,
                        area, volume, medicion, cost, totalCost, phaseDemolish, matriz, descripcion, grupo, libro,
                        visible, levelId.IntegerValue, lstParam, value, unidad, paramId, grupoId.IntegerValue, qOrden, grupoOrden, libroOrden);
                        lst.Add(qu);
                    }
                    #endregion
                }
            }
            catch (Exception)
            {
                string mensaje = "Problema con el elemento: <" + ar.Id.IntegerValue + "> " + ar.Name;
                Tools._lstWarnings.Add(mensaje);
            }
            return lst;
        }
        public static List<Quantity> CrearAllQuantities(Document doc, Element e)
        {
            List<Quantity> lst = new List<Quantity>();
            if (e.Category.Id != new ElementId(BuiltInCategory.OST_Assemblies))
            {
                lst.AddRange(CrearQuantityPinturas(doc, e));
            }
            if (e.Category.Id == new ElementId(BuiltInCategory.OST_Walls) && 
                QexOpciones.wallsMaterials == true)
            {
                lst.AddRange(CrearQuantityFromMaterials(doc, e));
            }
            else if (e.Category.Id == new ElementId(BuiltInCategory.OST_Floors) && 
                QexOpciones.floorMaterials == true)
            {
                lst.AddRange(CrearQuantityFromMaterials(doc, e));
            }
            else if (e.Category.Id == new ElementId(BuiltInCategory.OST_Roofs) && 
                QexOpciones.roofMaterials == true)
            {
                lst.AddRange(CrearQuantityFromMaterials(doc, e));
            }
            else if (e.Category.Id == new ElementId(BuiltInCategory.OST_Ceilings) && 
                QexOpciones.ceilingMaterials == true)
            {
                lst.AddRange(CrearQuantityFromMaterials(doc, e));
            }
            else if (e.Category.Id == new ElementId(BuiltInCategory.OST_StructuralFoundation) &&
                QexOpciones.strFoundationsFloorMaterials == true && e as Floor != null)
            {
                lst.AddRange(CrearQuantityFromMaterials(doc, e));
            }
            else if (e.Category.Id == new ElementId(BuiltInCategory.OST_Rebar))
            {
                if (e as Rebar != null)
                {
                    lst.AddRange(CrearQuantityFromRebar(doc, e as Rebar));
                }
                //if (e as AreaReinforcement != null)
                //{
                //    // Nada, Excluir del Cómputo
                //}
            }
            else if (e.Category.Id == new ElementId(BuiltInCategory.OST_AreaRein))
            {
                if (e as AreaReinforcement != null)
                {
                    lst.AddRange(CrearQuantityFromAreaReinf(doc, e as AreaReinforcement));
                }
            }
            else if (e.Category.Id == new ElementId(BuiltInCategory.OST_BuildingPad) &&
                QexOpciones.padsMaterials == true)
            {
                lst.AddRange(CrearQuantityFromMaterials(doc, e));
            }
            else if (e.Category.Id == new ElementId(BuiltInCategory.OST_Mass))
            {
                lst.AddRange(CrearQuantityFromMassElement(doc, e));
            }
            else if (e.Category.Id == new ElementId(BuiltInCategory.OST_Stairs))
            {
                lst.AddRange(CrearQuantityFromMaterials(doc, e));
            }
            else if (e.Category.Id == new ElementId(BuiltInCategory.OST_StairsRuns))
            {
                // Nada, Excluir del Cómputo
            }
            else if (e.Category.Id == new ElementId(BuiltInCategory.OST_StairsLandings))
            {
                // Nada, Excluir del Cómputo
            }
            else if (e.Category.Id == new ElementId(BuiltInCategory.OST_RailingTopRail))
            {
                // Nada, Excluir del Cómputo
            }
            else if (e.Category.Id == new ElementId(BuiltInCategory.OST_PipeCurves) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_PipeAccessory) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_PipeFitting) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_FlexPipeCurves) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_DuctCurves) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_DuctAccessory) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_DuctFitting) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_DuctTerminal) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_FlexDuctCurves) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_Conduit) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_ConduitFitting))
            {
                lst.AddRange(CrearQuantityFromMep(doc, e));
            }
            else
            {
                lst.AddRange(CrearQuantityFromElement(doc, e));
            }
            return lst;
        }
        public static void ActualizarQuantity(string qId, double cost, string descripcion, string matriz, string grupo,
            double value, string unidad, int qOrden, int grupoOrden, int libroOrden)
        {
            Quantity q = RevitQex.lstElements.Find(x => x.qId == qId);
            List<Quantity> lstQ = RevitQex.lstElements.FindAll(x => x.typeId == q.typeId);
            foreach (Quantity qu in lstQ)
            {
                qu.cost = cost;
                qu.descripcion = descripcion;
                double totalCost = Tools.CalculateTotalCost(qu.count, qu.length, qu.area, qu.volume, cost,
                    matriz, value);
                qu.totalCost = totalCost;
                string medicion = Tools.CalculateMedicion(qu.count, qu.length, qu.area, qu.volume, matriz,
                    value, unidad);
                qu.medicion = medicion;
                qu.matriz = matriz;
                qu.grupo = grupo;
                qu.value = value;
                qu.unidad = unidad;
                qu.qOrden = qOrden;
                qu.grupoOrden = grupoOrden;
                qu.libroOrden = libroOrden;
            }
        }
        #endregion

        #region Stats

        public static int getOpenedCount()
        {
            using (var datos = new DataAccess())
            {
                return datos.GetDbOpenLogs().Count;
            };
        }
        public static int getProcessedElements()
        {
            using (var datos = new DataAccess())
            {
                int count = 0;
                foreach (dbOpenLog log in datos.GetDbOpenLogs())
                {
                    count += log.ElementCount;
                }
                return count;
            };
        }

        /// <summary> Obtiene un diccionario para realizar una grafica </summary>
        public static Dictionary<string, int> GetCategoriesCountDictionary()
        {
            Dictionary<string, int> dictio = new Dictionary<string, int>();
            foreach (Quantity q in RevitQex.lstElements)
            {
                if (dictio.ContainsKey(q.category))
                {
                    dictio[q.category] = dictio[q.category] + q.count;
                }
                else
                {
                    dictio.Add(q.category, q.count);
                }
            }
            return dictio;
        }

        /// <summary> Devuelve un Dictionary que contiene 5 Categorías de ARQ </summary>
        public static Dictionary<string, double> GetDictionaryARQ()
        {
            int total = _elemCount;
            Dictionary<string, double> dictio = new Dictionary<string, double>();
            // Agregar Categorias Base
            Category catWalls = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_Walls);
            Category catFloors = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_Floors);
            Category catDoors = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_Doors);
            Category catWindows = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_Windows);
            Category catFurniture = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_Furniture);
            dictio.Add(catWalls.Name, 0);
            dictio.Add(catFloors.Name, 0);
            dictio.Add(catDoors.Name, 0);
            dictio.Add(catWindows.Name, 0);
            dictio.Add(catFurniture.Name, 0);
            // Agregar desde Elementos
            foreach (Element elem in RevitQex.listaElementos)
            {
                if (elem.Category.Id == new ElementId(BuiltInCategory.OST_Walls) ||
                    elem.Category.Id == new ElementId(BuiltInCategory.OST_Floors) ||
                    elem.Category.Id == new ElementId(BuiltInCategory.OST_Doors) ||
                    elem.Category.Id == new ElementId(BuiltInCategory.OST_Windows) ||
                    elem.Category.Id == new ElementId(BuiltInCategory.OST_Furniture))
                {
                    dictio[elem.Category.Name]++;
                }
            }
            // Convertir totales en Coeficientes
            //double wallCoef = Math.Round(dictio[catWalls.Name] / total, 2);
            //double floorCoef = Math.Round(dictio[catFloors.Name] / total, 2);
            //double doorsCoef = Math.Round(dictio[catDoors.Name] / total, 2);
            //double windowsCoef = Math.Round(dictio[catWindows.Name] / total, 2);
            //double furnitureCoef = Math.Round(dictio[catFurniture.Name] / total, 2);
            //dictio[catWalls.Name] = wallCoef;
            //dictio[catFloors.Name] = floorCoef;
            //dictio[catDoors.Name] = doorsCoef;
            //dictio[catWindows.Name] = windowsCoef;
            //dictio[catFurniture.Name] = furnitureCoef;
            return dictio;
        }

        /// <summary> Devuelve un Dictionary que contiene 5 Categorías de STR </summary>
        public static Dictionary<string, double> GetDictionarySTR()
        {
            int total = _elemCount;
            Dictionary<string, double> dictio = new Dictionary<string, double>();
            // Agregar Categorias Base
            Category cat1 = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralFraming);
            Category cat2 = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralColumns);
            Category cat3 = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructConnections);
            Category cat4 = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralFoundation);
            Category cat5 = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_Rebar);
            dictio.Add(cat1.Name, 0);
            dictio.Add(cat2.Name, 0);
            dictio.Add(cat3.Name, 0);
            dictio.Add(cat4.Name, 0);
            dictio.Add(cat5.Name, 0);
            // Agregar desde Elementos
            foreach (Element elem in RevitQex.listaElementos)
            {
                if (elem.Category.Id == new ElementId(BuiltInCategory.OST_StructuralFraming) ||
                    elem.Category.Id == new ElementId(BuiltInCategory.OST_StructuralColumns) ||
                    elem.Category.Id == new ElementId(BuiltInCategory.OST_StructConnections) ||
                    elem.Category.Id == new ElementId(BuiltInCategory.OST_StructuralFoundation) ||
                    elem.Category.Id == new ElementId(BuiltInCategory.OST_Rebar))
                {
                    dictio[elem.Category.Name]++;
                }
            }
            // Convertir totales en Coeficientes
            //double cat1Coef = Math.Round(dictio[cat1.Name] / total, 2);
            //double cat2Coef = Math.Round(dictio[cat2.Name] / total, 2);
            //double cat3Coef = Math.Round(dictio[cat3.Name] / total, 2);
            //double cat4Coef = Math.Round(dictio[cat4.Name] / total, 2);
            //double cat5Coef = Math.Round(dictio[cat5.Name] / total, 2);
            //dictio[cat1.Name] = cat1Coef;
            //dictio[cat2.Name] = cat2Coef;
            //dictio[cat3.Name] = cat3Coef;
            //dictio[cat4.Name] = cat4Coef;
            //dictio[cat5.Name] = cat5Coef;
            return dictio;
        }

        /// <summary> Devuelve un Dictionary que contiene 5 Categorías de MEP </summary>
        public static Dictionary<string, double> GetDictionaryMEP()
        {
            int total = _elemCount;
            Dictionary<string, double> dictio = new Dictionary<string, double>();
            // Agregar Categorias Base
            Category cat1 = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_PipeCurves);
            Category cat2 = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_PipeFitting);
            Category cat3 = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_DuctCurves);
            Category cat4 = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_DuctFitting);
            Category cat5 = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_CableTray);
            dictio.Add(cat1.Name, 0);
            dictio.Add(cat2.Name, 0);
            dictio.Add(cat3.Name, 0);
            dictio.Add(cat4.Name, 0);
            dictio.Add(cat5.Name, 0);
            // Agregar desde Elementos
            foreach (Element elem in RevitQex.listaElementos)
            {
                if (elem.Category.Id == new ElementId(BuiltInCategory.OST_PipeCurves) ||
                    elem.Category.Id == new ElementId(BuiltInCategory.OST_PipeFitting) ||
                    elem.Category.Id == new ElementId(BuiltInCategory.OST_DuctCurves) ||
                    elem.Category.Id == new ElementId(BuiltInCategory.OST_DuctFitting) ||
                    elem.Category.Id == new ElementId(BuiltInCategory.OST_CableTray))
                {
                    dictio[elem.Category.Name]++;
                }
            }
            // Convertir totales en Coeficientes
            //double cat1Coef = Math.Round(dictio[cat1.Name] / total, 2);
            //double cat2Coef = Math.Round(dictio[cat2.Name] / total, 2);
            //double cat3Coef = Math.Round(dictio[cat3.Name] / total, 2);
            //double cat4Coef = Math.Round(dictio[cat4.Name] / total, 2);
            //double cat5Coef = Math.Round(dictio[cat5.Name] / total, 2);
            //dictio[cat1.Name] = cat1Coef;
            //dictio[cat2.Name] = cat2Coef;
            //dictio[cat3.Name] = cat3Coef;
            //dictio[cat4.Name] = cat4Coef;
            //dictio[cat5.Name] = cat5Coef;
            return dictio;
        }

        /// <summary> Devuelve un Dictionary que contiene 5 Categorías Globales </summary>
        public static Dictionary<string, int> GetDictionaryGlobal()
        {
            Dictionary<string, int> dictio = new Dictionary<string, int>();
            // Agregar Categorias Base
            dictio.Add("Niveles", GetAllLevels(_doc).Count);
            dictio.Add("Planos de Planta", GetAllViewPlans(_doc).Count);
            dictio.Add("Secciones", GetAllViewSections(_doc).Count);
            dictio.Add("Vinculos CAD", GetAllCadLinks(_doc).Count);
            dictio.Add("Tablas", GetAllViewSchedule(_doc).Count);
            return dictio;
        }

        public static Chart GetChartBarElementCategories(Dictionary<string, int> dictio, string title)
        {
            Chart chart = new Chart();
            chart.Size = new Size(600, 250);
            chart.Titles.Add(title);
            chart.ChartAreas.Add("Area");
            chart.Legends.Add("Legend1");

            // Valores del diccionario
            string[] xValues = dictio.Select(x => x.Key).ToArray();
            int[] yValues = dictio.Select(x => x.Value).ToArray();

            // Add Series
            Series serie = chart.Series.Add("Categorias");
            serie.Points.DataBindXY(xValues, yValues);

            // Chart Style
            serie.ChartType = SeriesChartType.Bar;
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;
            chart.Series["Categorias"].IsVisibleInLegend = false;
            chart.Series["Categorias"].IsValueShownAsLabel = true;
            chart.Series["Categorias"].SetCustomProperty("DrawingStyle", "Cylinder");
            chart.Series["Categorias"].ToolTip = "#VALX" + " (" + "#VAL" + ")";

            return chart;
        }

        public static Chart GetRadarChart(Dictionary<string, double> dictio, string serieName,
            string title, System.Drawing.Color color)
        {
            string[] xValues = dictio.Select(x => x.Key).ToArray();
            double[] yValues = dictio.Select(x => x.Value).ToArray();

            Chart chart = new Chart();
            chart.Size = new Size(600, 250);
            chart.Titles.Add(title);
            chart.ChartAreas.Add("Area");
            chart.Legends.Add("Legend1");

            // Chart Clear
            chart.Series.Clear();

            // Set Title
            chart.Titles.Add(title);

            // Add Series
            Series serie = chart.Series.Add(serieName);
            serie.Points.DataBindXY(xValues, yValues);

            // Chart Style
            serie.ChartType = SeriesChartType.Radar;
            chart.Series[serieName].IsVisibleInLegend = false;
            chart.Series[serieName].IsValueShownAsLabel = true;
            chart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            chart.Series[0].BorderDashStyle = ChartDashStyle.Solid; // Borde Solido
            chart.Series[0].BorderWidth = 3; // Grosor del borde
            chart.Series[0].SetCustomProperty("AreaDrawingStyle", "Polygon"); // Forma poligonal
            chart.Series[0].Color = color;
            chart.Series[0].BorderColor = System.Drawing.Color.Black;
            chart.Series[0].BorderWidth = 2;
            chart.Series[serieName].ToolTip = "#VALX" + " (" + "#VAL" + ")";

            return chart;
        }

        /// <summary> Dibuja un Grafico de barras horizontales, a partir de un Dictionary </summary>
        public static void FillChartBar(Chart chart, Dictionary<string, int> dictio, string title)
        {
            string[] xValues = dictio.Select(x => x.Key).ToArray();
            int[] yValues = dictio.Select(x => x.Value).ToArray();
            // Chart Clear
            chart.Series.Clear();

            // Chart Title
            chart.Titles.Add(title);

            // Add Series
            Series serie = chart.Series.Add("Categorias");
            serie.Points.DataBindXY(xValues, yValues);

            // Chart Style
            serie.ChartType = SeriesChartType.Bar;
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;
            chart.Series["Categorias"].IsVisibleInLegend = false;
            chart.Series["Categorias"].IsValueShownAsLabel = true;
            chart.Series["Categorias"].SetCustomProperty("DrawingStyle", "Cylinder");
            chart.Series["Categorias"].ToolTip = "#VALX" + " (" + "#VAL" + ")";
        }

        /// <summary> Dibuja un grafico en forma de dona, a partir de un Dictionary </summary>
        public static void FillChartDonut(Chart chart, Dictionary<string,int> dictio, string title)
        {
            string[] xValues = dictio.Select(x => x.Key).ToArray();
            int[] yValues = dictio.Select(x => x.Value).ToArray();

            // Chart Clear
            chart.Series.Clear();

            // Chart Title
            chart.Titles.Add(title);

            // Add Series
            Series serie = chart.Series.Add("Categorias");
            serie.Points.DataBindXY(xValues, yValues);

            // Chart Style
            serie.ChartType = SeriesChartType.Doughnut;
            chart.Series["Categorias"].IsVisibleInLegend = true;
            chart.Series["Categorias"].IsValueShownAsLabel = false;
            chart.Series["Categorias"].ToolTip = "#VALX" + " (" + "#VAL" + ")";
        }

        public static void FillChartRadar(Chart chart, Dictionary<string, int> dictio, string serieName, 
            string title, System.Drawing.Color color)
        {
            string[] xValues = dictio.Select(x => x.Key).ToArray();
            int[] yValues = dictio.Select(x => x.Value).ToArray();

            // Chart Clear
            chart.Series.Clear();

            // Set Title
            chart.Titles.Add(title);

            // Add Series
            Series serie = chart.Series.Add(serieName);
            serie.Points.DataBindXY(xValues, yValues);

            // Chart Style
            serie.ChartType = SeriesChartType.Radar;
            chart.Series[serieName].IsVisibleInLegend = false;
            chart.Series[serieName].IsValueShownAsLabel = true;
            chart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            chart.Series[0].BorderDashStyle = ChartDashStyle.Solid; // Borde Solido
            chart.Series[0].BorderWidth = 3; // Grosor del borde
            chart.Series[0].SetCustomProperty("AreaDrawingStyle", "Polygon"); // Forma poligonal
            chart.Series[0].Color = color;
            chart.Series[0].BorderColor = System.Drawing.Color.Black;
            chart.Series[0].BorderWidth = 2;
            chart.Series[serieName].ToolTip = "#VALX" + " (" + "#VAL" + ")";
        }

        public static void FillChartRadar2(Chart chart, Dictionary<string, double> dictio, string serieName,
            string title, System.Drawing.Color color)
        {
            string[] xValues = dictio.Select(x => x.Key).ToArray();
            double[] yValues = dictio.Select(x => x.Value).ToArray();

            // Chart Clear
            chart.Series.Clear();

            // Set Title
            chart.Titles.Add(title);

            // Add Series
            Series serie = chart.Series.Add(serieName);
            serie.Points.DataBindXY(xValues, yValues);

            // Chart Style
            serie.ChartType = SeriesChartType.Radar;
            chart.Series[serieName].IsVisibleInLegend = false;
            chart.Series[serieName].IsValueShownAsLabel = true;
            chart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            chart.Series[0].BorderDashStyle = ChartDashStyle.Solid; // Borde Solido
            chart.Series[0].BorderWidth = 3; // Grosor del borde
            chart.Series[0].SetCustomProperty("AreaDrawingStyle", "Polygon"); // Forma poligonal
            chart.Series[0].Color = color;
            chart.Series[0].BorderColor = System.Drawing.Color.Black;
            chart.Series[0].BorderWidth = 2;
            chart.Series[serieName].ToolTip = "#VALX" + " (" + "#VAL" + ")";
        }

        public static void InsertPointToDb()
        {
            using (var datos = new DataAccess())
            {
                Point point = new Point();
                datos.InsertPoint(point);
            };
        }

        public static Point GetPointFromDb()
        {
            Point point = null;
            using (var datos = new DataAccess())
            {
                point = datos.GetPoint();
            };
            return point;
        }

        public static void AddPoint(double point)
        {
            using (var datos = new DataAccess())
            {
                Point point0 = datos.GetPoint();
                point0.points = point0.points + point;
                datos.UpdatePoint(point0);
            };
        }

        public static double GetPoints()
        {
            double points = 0;
            using (var datos = new DataAccess())
            {
                Point point0 = datos.GetPoint();
                points = point0.points;
            };
            return points;
        }

        public static void UpdatePointsLabel(System.Windows.Forms.ToolStripLabel label)
        {
            double points = GetPoints();
            string puntos = points.ToString("N1");
            label.Text = puntos + " Puntos";
        }
        #endregion

        #region Calculos
        /// <summary> Calcula el Costo Total antes de crearse la Quantificación, según la Matriz seleccionada. Magnitud * Costo </summary>
        public static double CalculateTotalCost(int count, double length, double area, double volume,
            double cost, string matriz, double value)
        {
            double totalCost = 0;
            if (matriz == "1000")
                totalCost = Math.Round(count * cost, 2);
            if (matriz == "0100")
                totalCost = Math.Round(length * cost, 2);
            if (matriz == "0010")
                totalCost = Math.Round(area * cost, 2);
            if (matriz == "0001")
                totalCost = Math.Round(volume * cost, 2);
            if (matriz == "1111")
                totalCost = Math.Round(value * cost, 2);
            return totalCost;
        }

        /// <summary> Calcula la Medición antes de crearse la Quantificación, según la Matriz seleccionada </summary>
        public static string CalculateMedicion(int count, double length, double area, double
            volume, string matriz, double value, string unidad)
        {
            string medicion = "";
            if (matriz == "1000")
                medicion = count.ToString(count % 1 == 0 ? "F0" : "F2") + " un";// count.ToString() + " ud";
            if (matriz == "0100")
                medicion = length.ToString(length % 1 == 0 ? "F0" : "F2") + " ml";// length.ToString() + " ml";
            if (matriz == "0010")
                medicion = area.ToString(area % 1 == 0 ? "F0" : "F2") + " m2";// area.ToString() + " m2";
            if (matriz == "0001")
                medicion = volume.ToString(volume % 1 == 0 ? "F0" : "F2") + " m3";// volume.ToString() + " m3";
            if (matriz == "1111")
                medicion = value.ToString(volume % 1 == 0 ? "F0" : "F2") + " " + unidad;
            return medicion;
        }

        /// <summary> Obtiene la magnitud preferida según la Matriz seleccionada </summary>
        public static double CalculateMedicionSinUnidad(int count, double length, double area, double volume,
            string matriz, double value)
        {
            double medicion = 0;
            if (matriz == "1000")
                medicion = count;
            if (matriz == "0100")
                medicion = length;
            if (matriz == "0010")
                medicion = area;
            if (matriz == "0001")
                medicion = volume;
            if (matriz == "1111")
                medicion = value;
            return medicion;
        }

        /// <summary> Obtiene la Unidad según la Matriz seleccionada </summary>
        public static string CalculateUnidad(string matriz, string unit)
        {
            string unidad = "";
            if (matriz == "1000")
                unidad = "un";
            if (matriz == "0100")
                unidad = "ml";
            if (matriz == "0010")
                unidad = "m2";
            if (matriz == "0001")
                unidad = "m3";
            if (matriz == "1111")
                unidad = unit;
            return unidad;
        }
        #endregion

        #region Revit
        /// <summary> Modifica el valor del parametro Costo del elemento de Revit </summary>
        public static void SetCostById(int typeId, double cost)
        {
            using (Transaction trans = new Transaction(_doc, "Qex: Actualizar Costo"))
            {
                ElementId eId = new ElementId(typeId);
                Element elem = _doc.GetElement(eId);
                Autodesk.Revit.DB.Parameter param = elem.get_Parameter(BuiltInParameter.ALL_MODEL_COST);
                trans.Start();
                param.Set(cost);
                trans.Commit();
            };
        }

        /// <summary> Modifica el valor del parametro Descripcion del elemento de Revit </summary>
        public static void SetDescriptionById(int typeId, string descripcion)
        {
            ElementId eId = new ElementId(typeId);
            Element elem = _doc.GetElement(eId);
            Autodesk.Revit.DB.Parameter param = elem.get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION);
            using (Transaction trans = new Transaction(_doc, "Qex: Actualizar descripción"))
            {
                trans.Start();
                param.Set(descripcion);
                trans.Commit();
            };
        }

        public static void IsolateElementsView(List<ElementId> lst, Autodesk.Revit.DB.View view)
        {
            view.IsolateElementsTemporary(lst);
        }

        public static void ExportImages(System.Windows.Forms.DataGridView grid)
        {
            List<Quantity> quantities = Tools.GetCheckedQuantitiesFromGrid(grid);
            _lstWarnings.Clear();
            string folderPath = string.Empty;
            string filePath = string.Empty;
            FolderBrowserDialog sfd = new FolderBrowserDialog();
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    folderPath = sfd.SelectedPath;
                    Autodesk.Revit.DB.View actualView = _doc.ActiveView;

                    foreach (Quantity q in quantities)
                    {
                        string title = q.category + " - " + q.name.Replace(":", "-");
                        title = title.Replace("/", "-");
                        title = title.Replace(" >> ", "-");
                        title = title.Replace('"', ' ');
                        title = title.Replace('?', ' ');
                        title = title.Replace('|', '-');
                        title = title.Replace('*', ' ');
                        title = title.Replace("'", "-");

                        filePath = System.IO.Path.Combine(folderPath, title + ".jpg");
                        List<Element> elements = GetElementsOfTypeFromQuantity(q);
                        List<ElementId> ids = new List<ElementId>();
                        foreach (Element elem in elements)
                        {
                            ids.Add(elem.Id);
                        }
                        using (Transaction t = new Transaction(_doc, "Qex: Aislar temporalmente"))
                        {
                            t.Start();
                            Tools.IsolateElementsView(ids, actualView);
                            t.Commit();
                        };
                        ImageExportOptions opt = new ImageExportOptions();
                        opt.ExportRange = ExportRange.CurrentView;
                        opt.ImageResolution = ImageResolution.DPI_300;
                        opt.PixelSize = 2048;
                        opt.FilePath = filePath;
                        using (Transaction t = new Transaction(_doc, "Qex: Exportar Imagen"))
                        {
                            t.Start();
                            try
                            {
                                _doc.ExportImage(opt);
                                t.Commit();
                            }
                            catch (Exception)
                            {
                                _lstWarnings.Add("No se puede exportar: " + q.name);
                                t.RollBack();
                            }
                        };
                        using (Transaction t = new Transaction(_doc, "Qex: Restablecer Vista"))
                        {
                            t.Start();
                            TemporaryViewMode tmp = TemporaryViewMode.TemporaryHideIsolate;
                            actualView.DisableTemporaryViewMode(tmp);
                            t.Commit();
                        };
                        Tools.AddPoint(quantities.Count / 10);
                    }
                    if (_lstWarnings.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in Tools._lstWarnings)
                        {
                            sb.AppendLine(item);
                        }
                        (new frmWarnings("Lista de Errores", "Se muestra una lista con todos las Quantificaciones que "
                        + "no pudieron exportarse", sb.ToString())).ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Imagenes guardadas correctamente", RevitQex.QexName,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error 104: " + ex.Message, RevitQex.QexName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public static void ExportImagen(Document docGral, Quantity q)
        {
            string filePath = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Image|*.jpg";
            string title = q.category + " - " + q.name.Replace(":", "-");
            title = title.Replace("/", "-");
            title = title.Replace(" >> ", "-");
            title = title.Replace('"', ' ');
            title = title.Replace('?', ' ');
            title = title.Replace('|', '-');
            title = title.Replace('*', ' ');
            title = title.Replace("'", "-");

            sfd.FileName = title + ".jpg";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                filePath = sfd.FileName;
                try
                {
                    Autodesk.Revit.DB.View actualView = docGral.ActiveView;
                    ImageExportOptions opt = new ImageExportOptions();
                    List<ElementId> ids = GetElementsIdFromQuantity(q);
                    using (Transaction t = new Transaction(docGral, "Qex: Aislar temporalmente"))
                    {
                        try
                        {
                            t.Start();
                            Tools.IsolateElementsView(ids, actualView);
                            t.Commit();
                        }
                        catch (Exception)
                        {
                            t.RollBack();
                        }
                    };
                    opt.ExportRange = ExportRange.CurrentView;
                    opt.FilePath = filePath;
                    opt.ImageResolution = ImageResolution.DPI_300;
                    opt.PixelSize = 2048;
                    using (Transaction t = new Transaction(docGral, "Exportar Imagen"))
                    {
                        try
                        {
                            t.Start();
                            docGral.ExportImage(opt);
                            t.Commit();
                        }
                        catch (Exception)
                        {
                            t.RollBack();
                        }
                    };
                    TemporaryViewMode tmp = TemporaryViewMode.TemporaryHideIsolate;
                    using (Transaction t = new Transaction(docGral, "Qex: Restablecer Vista"))
                    {
                        try
                        {
                            t.Start();
                            actualView.DisableTemporaryViewMode(tmp);
                            t.Commit();
                        }
                        catch (Exception)
                        {
                            t.RollBack();
                        }
                    };
                    DialogResult pregunta = MessageBox.Show("Imagen guardada correctamente. " +
                        "¿Desea abrir el archivo?", RevitQex.QexName,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (pregunta == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(filePath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, RevitQex.QexName,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public static void IsolateElementsFromGrid(DataGridView grid, ToolStripButton btnRestablecerVista,
            ToolStripButton btnAislarElementos, ToolStripLabel lblPoints)
        {
                Autodesk.Revit.DB.View actualView = Tools._doc.ActiveView;
                Quantity q = grid.SelectedCells[0].OwningRow.DataBoundItem as Quantity;
                List<ElementId> lstId = GetElementsIdFromQuantity(q);
                if (lstId.Count != 0)
                {
                    if (actualView.CanUseTemporaryVisibilityModes())
                    {
                        using (Transaction t = new Transaction(Tools._doc, "Qex: Isolate"))
                        {
                            t.Start();
                            Tools.IsolateElementsView(lstId, actualView);
                            t.Commit();
                        };
                        btnRestablecerVista.Enabled = true;
                        btnAislarElementos.Enabled = false;
                        MessageBox.Show("Elementos aislados correctamente", RevitQex.QexName,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se puede modificar la Vista actual", RevitQex.QexName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                if (lstId.Count == 0)
                {
                    MessageBox.Show("No se puede aislar esta Quantificación", RevitQex.QexName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                Tools.UpdatePointsLabel(lblPoints);
        }

        public static void RestoreActualView(ToolStripButton btnRestablecerVista, ToolStripButton btnAislarElementos)
        {
            Autodesk.Revit.DB.View actualView = _doc.ActiveView;
            using (Transaction t = new Transaction(_doc, "Qex: Restablecer Vista"))
            {
                t.Start();
                try
                {
                    TemporaryViewMode tmp = TemporaryViewMode.TemporaryHideIsolate;
                    actualView.DisableTemporaryViewMode(tmp);
                    t.Commit();
                    btnRestablecerVista.Enabled = false;
                    btnAislarElementos.Enabled = true;
                    MessageBox.Show("Se restableció la Vista actual", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("No se puede restablecer la Vista actual", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    t.RollBack();
                }
            };
        }

        public static void IsolateElementsFromSelectedInGrid(DataGridView grid, ToolStripButton btnRestablecerVista
            , ToolStripButton btnAislarElementos, ToolStripLabel lblPoints)
        {
            List<Quantity> quantities = GetCheckedQuantitiesFromGrid(grid);
            List<ElementId> lstId = new List<ElementId>();
            Autodesk.Revit.DB.View actualView = _doc.ActiveView;
            foreach (Quantity q in quantities)
            {
                List<ElementId> ids = GetElementsIdFromQuantity(q);
                foreach (ElementId id in ids)
                {
                    lstId.Add(id);
                }
            }
            if (lstId.Count != 0)
            {
                if (actualView.CanUseTemporaryVisibilityModes())
                {
                    using (Transaction t = new Transaction(Tools._doc, "Qex: Aislar temporalmente"))
                    {
                        t.Start();
                        Tools.IsolateElementsView(lstId, actualView);
                        t.Commit();
                    };

                    btnRestablecerVista.Enabled = true;
                    btnAislarElementos.Enabled = false;
                    MessageBox.Show("Elementos aislados correctamente", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se puede modificar la Vista actual", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (lstId.Count == 0)
            {
                MessageBox.Show("No se puede aislar esta Quantificación", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Get
        /// <summary> Obtiene la sumatoria de Costo Total de una Lista de Quantificaciones </summary>
        public static double GetGlobalCost(List<Quantity> lst)
        {
            double globalCost = 0;
            foreach (Quantity q in lst)
            {
                globalCost += q.totalCost;
                globalCost = Math.Round(globalCost, 2);
            }
            return globalCost;
        }

        /// <summary> Obtiene el ID del elemento ProjectInfo </summary>
        public static int GetProjectInformationId(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> lst = collector.OfCategory(BuiltInCategory.OST_ProjectInformation).ToList();
            Element projectInfo = lst.First<Element>();
            int id = projectInfo.Id.IntegerValue;
            return id;
        }

        /// <summary> Obtiene el LevelId de un Elemento de Revit </summary>
        public static ElementId ElementGetLevelId(Element e)
        {
            ElementId lvlId = e.LevelId;

            if (e.Category.Id == new ElementId(BuiltInCategory.OST_Stairs) &&
                e.get_Parameter(BuiltInParameter.STAIRS_BASE_LEVEL_PARAM) != null)
            {
                lvlId = e.get_Parameter(BuiltInParameter.STAIRS_BASE_LEVEL_PARAM).AsElementId();
            }
            if (e.Category.Id == new ElementId(BuiltInCategory.OST_Roofs) &&
                e.get_Parameter(BuiltInParameter.ROOF_BASE_LEVEL_PARAM) != null)
            {
                lvlId = e.get_Parameter(BuiltInParameter.ROOF_BASE_LEVEL_PARAM).AsElementId();
            }
            if (e.Category.Id == new ElementId(BuiltInCategory.OST_Roofs) &&
                e.get_Parameter(BuiltInParameter.ROOF_CONSTRAINT_LEVEL_PARAM) != null)
            {
                lvlId = e.get_Parameter(BuiltInParameter.ROOF_CONSTRAINT_LEVEL_PARAM).AsElementId();
            }
            if (e.Category.Id == new ElementId(BuiltInCategory.OST_Rebar))
            {
                try
                {
                    Rebar r = (Rebar)e;
                    ElementId hostId = r.GetHostId();
                    if (hostId != new ElementId(-1))
                    {
                        Document doc = e.Document;
                        Element elem = doc.GetElement(hostId);
                        if (elem.Category.Id == new ElementId(BuiltInCategory.OST_StructuralFraming)) // The Host is Beam?
                        {
                            lvlId = elem.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM).AsElementId();
                        }
                        else
                        {
                            lvlId = elem.LevelId;
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            if (e.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM) != null) // Beams
            {
                lvlId = e.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM).AsElementId();
            }
            //if (e.get_Parameter(BuiltInParameter.RBS_START_LEVEL_PARAM) != null) // Pipes
            //{
            //    lvlId = e.get_Parameter(BuiltInParameter.RBS_START_LEVEL_PARAM).AsElementId();
            //}
            if (e.get_Parameter(BuiltInParameter.SCHEDULE_LEVEL_PARAM) != null) // Family Instance
            {
                if (e.get_Parameter(BuiltInParameter.SCHEDULE_LEVEL_PARAM).AsElementId() !=
                    new ElementId(-1))
                {
                    lvlId = e.get_Parameter(BuiltInParameter.SCHEDULE_LEVEL_PARAM).AsElementId();
                }
            }
            if (e.Category.Id == new ElementId(BuiltInCategory.OST_PipeCurves) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_PipeAccessory) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_PipeFitting) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_DuctCurves) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_DuctAccessory) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_DuctFitting) ||
                e.Category.Id == new ElementId(BuiltInCategory.OST_DuctTerminal))
            {
                lvlId = new ElementId(-1);
            }
            return lvlId;
        }

        /// <summary> Obtiene una Lista de checked TreeNodes, a partir de un TreeView </summary>
        public static List<TreeNode> GetCheckedNodes(TreeView tree)
        {
            List<TreeNode> lista = new List<TreeNode>();
            foreach (TreeNode node0 in tree.Nodes)
            {
                if (node0.Name != "AA")
                {
                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            if (node2.Checked == true)
                            {
                                lista.Add(node2);
                            }
                        }
                        if (node1.Checked)
                            node1.Checked = false;
                    }
                }
                if (node0.Checked)
                    node0.Checked = false;
            }
            return lista;
        }

        /// <summary> A partir del QexTexto2 del Schema Qex se extraen los ordenes: qOrden, grupoOrden y libroOrden </summary>
        public static List<int> GetOrdenesFromTextOrdenes(string ordenes)
        {
            List<int> lst = new List<int>();
            string[] values = System.Text.RegularExpressions.Regex.Split(ordenes, ";");
            //qid;typeId;matriz;grupo;libro
            
            int qOrden = Convert.ToInt32(values[0]);
            int grupoOrden = Convert.ToInt32(values[1]);
            int libroOrden = Convert.ToInt32(values[2]);

            lst.Add(qOrden);
            lst.Add(grupoOrden);
            lst.Add(libroOrden);

            return lst;
        }

        /// <summary> Get the Element Type from the Quantity </summary>
        public static Element GetElementTypeFromQuantity(Quantity quan)
        {
            ElementId id = new ElementId(quan.typeId);
            return _doc.GetElement(id);
        }

        /// <summary> Obtiene la imagen de Vista Previa de la Quantificación </summary>
        public static Bitmap GetImagePreviewFormQuantity(Quantity quan, Size size)
        {
            Bitmap pic = Resource1.NoImage_256x256;
            try
            {
                Element elem = GetElementTypeFromQuantity(quan);
                if (elem.Category.Id == new ElementId(BuiltInCategory.OST_Materials))
                {
                    Material mat = elem as Material;
                    System.Drawing.Color color = System.Drawing.Color.FromArgb(mat.Color.Red, mat.Color.Green,
                        mat.Color.Blue);
                    var myBitmap = new Bitmap(size.Width, size.Height);
                    // Set each pixel in myBitmap to black.
                    for (int Xcount = 0; Xcount < myBitmap.Width; Xcount++)
                    {
                        for (int Ycount = 0; Ycount < myBitmap.Height; Ycount++)
                        {
                            myBitmap.SetPixel(Xcount, Ycount, color);
                        }
                    }
                    pic = myBitmap;
                }
                else
                {
                    ElementType type = (ElementType)elem;
                    if (type.GetPreviewImage(size) != null)
                    {
                        pic = type.GetPreviewImage(size);
                    }
                }
            }
            catch (Exception)
            {

            }
            return pic;
        }

        /// <summary> Obtiene todos los Ejemplares del Tipo de elemento </summary>
        public static List<Element> GetElementsOfTypeFromQuantity(Quantity quan)
        {
            List<Element> lstElements = new List<Element>();
            ElementId typeId = new ElementId(quan.typeId);
            Element elem = _doc.GetElement(typeId);
            if (elem.Category.Id != new ElementId(BuiltInCategory.OST_Materials))
            {
                FilteredElementCollector collector = new FilteredElementCollector(_doc);
                List<Element> elements = collector.WhereElementIsNotElementType().ToList();

                Options op = _doc.Application.Create.NewGeometryOptions();
                op.ComputeReferences = true;


                List<Element> lstInstance = (from elemento in elements
                                             where elemento.GetTypeId() == typeId
                                             && elemento.get_Geometry(op) != null
                                             select elemento).ToList();

                foreach (Element e in lstInstance)
                {
                    lstElements.Add(e);
                }

                // For MEP elements
                if (elem.Category.Id == new ElementId(BuiltInCategory.OST_PipeCurves) ||
                elem.Category.Id == new ElementId(BuiltInCategory.OST_PipeAccessory) ||
                elem.Category.Id == new ElementId(BuiltInCategory.OST_PipeFitting) ||
                elem.Category.Id == new ElementId(BuiltInCategory.OST_FlexPipeCurves) ||
                elem.Category.Id == new ElementId(BuiltInCategory.OST_DuctCurves) ||
                elem.Category.Id == new ElementId(BuiltInCategory.OST_DuctAccessory) ||
                elem.Category.Id == new ElementId(BuiltInCategory.OST_DuctFitting) ||
                elem.Category.Id == new ElementId(BuiltInCategory.OST_DuctTerminal) ||
                elem.Category.Id == new ElementId(BuiltInCategory.OST_FlexDuctCurves) ||
                elem.Category.Id == new ElementId(BuiltInCategory.OST_Conduit) ||
                elem.Category.Id == new ElementId(BuiltInCategory.OST_ConduitFitting))
                {
                    lstElements.Clear();
                    // Get the size and System of Quantity
                    string[] values = System.Text.RegularExpressions.Regex.Split(quan.name, " >> ");
                    string elemSystem = values[0];
                    string elemSize = values[2];

                    foreach (Element e in lstInstance)
                    {
                        // Get the System and Size of Element
                        string systemType = "No System";
                        if (e.get_Parameter(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM) != null)
                            systemType = e.get_Parameter(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM).AsValueString();
                        if (e.get_Parameter(BuiltInParameter.RBS_DUCT_SYSTEM_TYPE_PARAM) != null)
                            systemType = e.get_Parameter(BuiltInParameter.RBS_DUCT_SYSTEM_TYPE_PARAM).AsValueString();
                        if (e.get_Parameter(BuiltInParameter.RBS_CTC_SERVICE_TYPE) != null)
                        {
                            if (e.get_Parameter(BuiltInParameter.RBS_CTC_SERVICE_TYPE).HasValue == true)
                            {
                                systemType = e.get_Parameter(BuiltInParameter.RBS_CTC_SERVICE_TYPE).AsString();
                            }
                        }
                        string size = e.get_Parameter(BuiltInParameter.RBS_CALCULATED_SIZE).AsString();
                        if (systemType == elemSystem && size == elemSize)
                        {
                            lstElements.Add(e);
                        }
                    }
                }
            }
            if (elem.Category.Id == new ElementId(BuiltInCategory.OST_Materials))
            {
                string category = quan.GetRealCategory();
                FilteredElementCollector collector = new FilteredElementCollector(_doc);
                List<Element> elements = collector.WhereElementIsNotElementType().ToList();
                List<Element> lstInstance = (from elemento in elements
                                             where elemento.Category != null
                                             select elemento).ToList();

                foreach (Element e in lstInstance)
                {
                    foreach (ElementId id in e.GetMaterialIds(false))
                    {
                        if (id == elem.Id && e.Category.Name == category && !quan.category.Contains("Paints"))
                        {
                            if (!lstElements.Exists(x => x.Id == e.Id))
                            {
                                lstElements.Add(e);
                            }
                        }
                        if (id == elem.Id && quan.category.Contains("Paints"))
                        {

                        }
                    }
                    foreach (ElementId id in e.GetMaterialIds(true))
                    {
                        if (id == elem.Id && e.Category.Name == category)
                        {
                            if (!lstElements.Exists(x => x.Id == e.Id))
                            {
                                lstElements.Add(e);
                            }
                        }
                        if (id == elem.Id && quan.category.Contains("Paints"))
                        {

                        }
                    }
                }
            }
            return lstElements;
        }

        /// <summary> Obtiene la Lista de QexRecurso de la Quantificación </summary>
        public static List<QexRecurso> GetRecursosFromQuantity(Quantity quan)
        {
            Element elem = _doc.GetElement(new ElementId(quan.typeId));
            return DalRecursos.ReadRecursosFromElement(elem);
        }

        /// <summary> Obtiene una Lista de ElementId de la Quantificación </summary>
        public static List<ElementId> GetElementsIdFromQuantity(Quantity quan)
        {
            Element type = GetElementTypeFromQuantity(quan);
            List<ElementId> lstId = new List<ElementId>();
            List<Element> elements = GetElementsOfTypeFromQuantity(quan);

            // Variables
            string fase = quan.phaseCreated;
            bool demolish = false;
            ElementId lvlId = new ElementId(-1);
            if (quan.LevelId != -1)
            {
                Element lvl = _doc.GetElement(new ElementId(quan.LevelId));
                lvlId = lvl.Id;
            }
            if (quan.category.Contains("(Demolido)"))
            {
                demolish = true;
            }

            foreach (Element elem in elements)
            {
                if (!demolish)
                {
                    if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                    {
                        lstId.Add(elem.Id);
                    }
                    if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                    {
                        if (elem.get_Parameter(BuiltInParameter.PHASE_CREATED).AsValueString() == fase)
                        {
                            lstId.Add(elem.Id);
                        }
                    }
                    if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                    {
                        if (Tools.ElementGetLevelId(elem) == lvlId)
                        {
                            lstId.Add(elem.Id);
                        }
                    }
                    if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                    {
                        if (elem.get_Parameter(BuiltInParameter.PHASE_CREATED).AsValueString() == fase &&
                        Tools.ElementGetLevelId(elem) == lvlId)
                        {
                            lstId.Add(elem.Id);
                        }
                    }
                }
                if (demolish)
                {
                    if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                    {
                        if (elem.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED).AsValueString() == fase)
                        {
                            lstId.Add(elem.Id);
                        }
                    }
                    if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                    {
                        if (elem.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED).AsValueString() == fase &&
                        Tools.ElementGetLevelId(elem) == lvlId)
                        {
                            lstId.Add(elem.Id);
                        }
                    }
                }
            }
            return lstId;
        }

        /// <summary> Obtiene una lista de Elementos de la Quantificación </summary>
        public List<Element> GetStringElementsFromQuantity(Quantity quan)
        {
            List<Element> elements = new List<Element>();
            List<ElementId> ids = GetElementsIdFromQuantity(quan);
            foreach (ElementId id in ids)
            {
                Element elem = _doc.GetElement(id);
                elements.Add(elem);
            }
            return elements;
        }

        /// <summary> Obtiene una lista de Ids, separados por ";" </summary>
        public string GetStringElementsIdsFromQuantity(Quantity quan)
        {
            StringBuilder sb = new StringBuilder();
            List<ElementId> ids = GetElementsIdFromQuantity(quan);
            foreach (ElementId id in ids)
            {
                sb.Append(id.IntegerValue.ToString() + ";");
            }
            return sb.ToString();
        }
        #endregion

        #region Verificaciones
        /// <summary> Verifica si el Schema QexGrupoMateriales existe. Si no existe lo crea </summary>
        public static void VerificarSchemaGrupos()
        {
            try
            {
                if (!DalRecursos.SchemaExist("QexGrupoMateriales"))
                {
                    DalRecursos.CreateSchemaGrupos();
                    DalRecursos.CrearEntidadGrupos(new List<QexGrupoMaterial>());
                }
                else
                {
                    Element info = _doc.GetElement(new ElementId(GetProjectInformationId(_doc)));
                    Schema schema = QexSchema.GetSchemaByName("QexGrupoMateriales");
                    if (!info.GetEntity(schema).IsValid())
                    {
                        DalRecursos.CrearEntidadGrupos(new List<QexGrupoMaterial>());
                    }
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show(RevitQex.QexName, "Error: " + ex.Message);
            }
        }

        /// <summary> Verifica si el Schema QexMateriales existe. Si no existe lo crea </summary>
        public static void VerificarSchemaMateriales()
        {
            try
            {
                if (!DalRecursos.SchemaExist("QexMateriales"))
                {
                    DalRecursos.CreateSchemaMateriales();
                    DalRecursos.CrearEntidadMateriales(new List<QexMaterial>());
                }
                else
                {
                    Element info = _doc.GetElement(new ElementId(GetProjectInformationId(_doc)));
                    Schema schema = QexSchema.GetSchemaByName("QexMateriales");
                    if (!info.GetEntity(schema).IsValid())
                    {
                        DalRecursos.CrearEntidadMateriales(new List<QexMaterial>());
                    }
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show(RevitQex.QexName, "Error: " + ex.Message);
            }
            
        }

        /// <summary> Verifica si el Schema QexRecursos existe en el documento. Si no existe lo crea </summary>
        public static void VerificarSchemaRecursos()
        {
            if (!DalRecursos.SchemaExist("QexRecursos"))
            {
                try
                {
                    DalRecursos.CreateSchemaRecursos();
                }
                catch (Exception)
                {
                    TaskDialog.Show(RevitQex.QexName, "No se puede crear el Schema");
                }
            }
        }

        /// <summary> Verifica si el Schema Qex existe. Si no existe lo crea </summary>
        public static void VerificarSchemaQex()
        {
            try
            {
                if (!QexSchema.SchemaExist(QexSchema._schemaQex))
                {
                    QexSchema.CreateSchemaQex(_doc);
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show(RevitQex.QexName, "Error: " + ex.Message);
            }
        }

        /// <summary> Verifica si el Schema QexComputos existe. Si no existe lo crea </summary>
        public static void VerificarSchemaComputos()
        {
            if (!QexSchema.SchemaExist(QexSchema._schemaComputo))
            {
                QexSchema.CreateSchemaComputos(_doc);
            }
        }

        /// <summary> Verifica si el Schema QexOptions existe. Si no existe lo crea y lo asigna con valores por defecto </summary>
        public static void VerificarSchemaQexOptions(Document doc)
        {
            if (!QexSchema.EntityQexOptionsExist(doc))
            {
                QexSchema.CreateSchemaQexOptions(doc);
                QexSchema.CrearEntidadQexOptions(doc, Tools.StringOptions());
            }
        }

        /// <summary> Borra todos los archivos temporales de imágenes de la carpeta Temp </summary>
        public static void CleanTempFiles()
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.IO.Path.Combine(App.AppDirectory, "Temp"));
            foreach (System.IO.FileInfo file in di.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (Exception ex)
                {
                    Log.Insert("ERROR, no se puede borrar: " + file.FullName);
                    Log.Insert("Causa: " + ex.Message);
                }
            }
        }
        #endregion

        #region Materiales
        /// <summary>
        /// Crea una Lista inicial de Grupos de Materiales
        /// </summary>
        /// <returns></returns>
        public static void CrearListaInicialGrupos()
        {
            List<string> lstNombres = new List<string>();
            List<QexGrupoMaterial> lst = new List<QexGrupoMaterial>();
            lstNombres.Add("Aceros e Hierros");
            lstNombres.Add("Adhesivos");
            lstNombres.Add("Amoblamientos de Cocina");
            lstNombres.Add("Aridos");
            lstNombres.Add("Bloques");
            lstNombres.Add("Ladrillos");

            foreach (string nombre in lstNombres)
            {
                if (!DalRecursos.ReadGrupos().Exists(x => x.Nombre == nombre))
                {
                    QexGrupoMaterial grupo = new QexGrupoMaterial();
                    grupo.Nombre = nombre;
                    lst.Add(grupo);
                }
            }
            DalRecursos.CrearEntidadGrupos(lst);
        }

        /// <summary>
        /// Crea una Lista inicial de Materiales
        /// </summary>
        /// <returns></returns>
        public static void CrearListaInicialMateriales()
        {
            List<QexMaterial> lst = new List<QexMaterial>();
            QexGrupoMaterial ladrillos = DalRecursos.ReadGrupos().FirstOrDefault(x => x.Nombre == "Ladrillos");
            QexGrupoMaterial aridos = DalRecursos.ReadGrupos().FirstOrDefault(x => x.Nombre == "Aridos");
            lst.Add(new QexMaterial(Guid.NewGuid().ToString(), "Ladrillo común", "ud", ladrillos.id, 0, 0));
            lst.Add(new QexMaterial(Guid.NewGuid().ToString(), "Ladrillo cerámico", "ud", ladrillos.id, 0, 0));
            lst.Add(new QexMaterial(Guid.NewGuid().ToString(), "Arena fina", "m3", aridos.id, 0, 1));
            lst.Add(new QexMaterial(Guid.NewGuid().ToString(), "Arena Gruesa", "m3", aridos.id, 0, 1));
            DalRecursos.CrearEntidadMateriales(lst);
        }

        public static List<Image> ListaIconos()
        {
            List<Image> lst = new List<Image>();
            lst.Add(Resource1.box);
            lst.Add(Resource1.area);
            lst.Add(Resource1.bolsa);
            lst.Add(Resource1.botella);
            lst.Add(Resource1.cajas);
            lst.Add(Resource1.camion);
            lst.Add(Resource1.carro);
            lst.Add(Resource1.casco);
            lst.Add(Resource1.herramienta);
            lst.Add(Resource1.kg);
            lst.Add(Resource1.ladrillos);
            lst.Add(Resource1.liquido);
            lst.Add(Resource1.litros);
            lst.Add(Resource1.regla);
            lst.Add(Resource1.tiempo);

            return lst;
        }

        public static ImageList GetImageListIcons()
        {
            ImageList lista = new ImageList();
            foreach (Image image in ListaIconos())
            {
                lista.Images.Add(image);
            }
            return lista;
        }

        public static List<string> GetSheetsFromExcel(string path)
        {
            List<string> lst = new List<string>();
            var workbook = new XLWorkbook(path);
            for (int i = 1; i < workbook.Worksheets.Count + 1; i++)
            {
                string name = workbook.Worksheet(i).Name;
                lst.Add(name);
            }
            return lst;
        }

        public static List<RecursoImportadoExcel> ImportRecursosFromExcel(string path
            , string sheetName, int GroupColumn, int NameColumn, int UnitColumn, int CostColumn
            , int FromRow, int ToRow, int IndexCol)
        {
            List<RecursoImportadoExcel> lst = new List<RecursoImportadoExcel>();
            var book = new XLWorkbook(path);
            var ws1 = book.Worksheet(sheetName);
            int rowCount = ws1.Rows().Count();
            int final = ToRow;
            if (rowCount < ToRow)
            {
                final = rowCount;
            }
            for (int i = FromRow; i < final + 1; i++)
            {
                string grupo = ws1.Row(i).Cell(GroupColumn).Value.ToString();
                string nombre = ws1.Row(i).Cell(NameColumn).Value.ToString();
                string unit = ws1.Row(i).Cell(UnitColumn).Value.ToString();
                double Cost = 0;
                int index = 0;
                try
                {
                    Cost = Math.Round(Convert.ToDouble(ws1.Row(i).Cell(CostColumn).Value), 3);
                }
                catch (Exception)
                {

                }
                double comCost = Cost;
                // Icono
                if (IndexCol > 0)
                {
                    try
                    {
                        index = Convert.ToInt32(ws1.Row(i).Cell(IndexCol).Value.ToString());
                        if (index > Tools.ListaIconos().Count)
                        {
                            index = 0;
                        }
                    }
                    catch (Exception)
                    {
                        
                    }
                }
                RecursoImportadoExcel rec = new RecursoImportadoExcel();
                rec.Group = grupo;
                rec.Name = nombre;
                rec.Unit = unit;
                rec.Cost = Cost;
                rec.Index = index;
                lst.Add(rec);
            }
            return lst;
        }

        public static void ImportAsignacionesFromExcel(string path)
        {
            int countRow = 0;
            int count = 0;
            StringBuilder errores = new StringBuilder();
            List<RecursoImportadoExcel> lst = new List<RecursoImportadoExcel>();
            var book = new XLWorkbook(path);
            var ws1 = book.Worksheet(1);
            foreach (IXLRow row in ws1.Rows())
            {
                // Leer datos
                if (row.Cell(1).Value != null && countRow > 0)
                {
                    string quantificacion = row.Cell(1).Value.ToString();
                    string descripcion = row.Cell(2).Value.ToString();
                    string unidad = row.Cell(3).Value.ToString();
                    string recurso = row.Cell(4).Value.ToString();
                    double consumo = 0;
                    try
                    {
                        consumo = Convert.ToDouble(row.Cell(5).Value.ToString());
                    }
                    catch (Exception)
                    {
                        // Nada
                    }
                    string unidad_ = row.Cell(6).Value.ToString();
                    int index = 0;
                    try
                    {
                        index = Convert.ToInt32(row.Cell(7).Value.ToString());
                    }
                    catch (Exception)
                    {
                        // Nada
                    }
                    // ¿Existe la Quantificación con ese nombre?
                    if (RevitQex.lstElements.Exists(x => x.name == quantificacion) || 
                        RevitQex.lstElements.Exists(x => x.descripcion == descripcion))
                    {
                        Quantity q = RevitQex.lstElements.FirstOrDefault(x => x.name == quantificacion);
                        if (q == null)
                        {
                            q = RevitQex.lstElements.FirstOrDefault(x => x.descripcion == quantificacion);
                        }
                        // ¿La Quantificacion tiene la misma unidad?
                        string qUnit = q.GetUnidad();
                        if (qUnit == unidad)
                        {
                            // ¿Existe el Material del recurso en el Proyecto?
                            List<QexMaterial> materiales = DalRecursos.ReadMateriales();
                            if (!materiales.Exists(x => x.nombre == recurso))
                            {
                                // No existe el material, crearlo
                                // ¿Existe el grupo Qex Importado?
                                List<QexGrupoMaterial> grupos = DalRecursos.ReadGrupos();
                                if (!grupos.Exists(x => x.Nombre == "Qex Importado"))
                                {
                                    QexGrupoMaterial grupo0 = new QexGrupoMaterial();
                                    grupo0.Nombre = "Qex Importado";
                                    if (grupo0.Insert())
                                    {

                                    }
                                    else
                                    {
                                        errores.AppendLine("El Grupo Qex Importado no se pudo crear");
                                    }
                                }
                                QexGrupoMaterial grupo = grupos.FirstOrDefault(x => x.Nombre == "Qex Importado");
                                QexMaterial mat0 = new QexMaterial();
                                mat0.nombre = recurso;
                                mat0.unidad = unidad_;
                                mat0.precio = 0;
                                mat0.grupoId = grupo.id;
                                mat0.index = index;
                                if (mat0.Insert())
                                {

                                }
                                else
                                {
                                    errores.AppendLine("El material " + mat0.nombre + " no se pudo insertar en la " +
                                        " Quantificación " + q.name);
                                }
                            }
                            QexMaterial mat = materiales.FirstOrDefault(x => x.nombre == recurso);
                            
                            // ¿Exiiste el recurso en la Quantificacion?
                            List<QexRecurso> recursos = GetRecursosFromQuantity(q);
                            if (recursos.Exists(x => x.matId == mat.id))
                            {
                                // Existe, sólo actualizar
                                QexRecurso rec0 = recursos.FirstOrDefault(x => x.matId == mat.id);
                                Element elem0 = GetElementTypeFromQuantity(q);
                                rec0.Consumo = consumo;
                                if (rec0.Update(elem0))
                                {
                                    count++;
                                }
                                else
                                {
                                    errores.AppendLine("El recurso " + mat.nombre + " de la Quantificación " +
                                        q.name + " no se pudo importar");
                                }
                            }
                            else
                            {
                                // No existe, asignar
                                QexRecurso rec = new QexRecurso();
                                rec.matId = mat.id;
                                rec.Consumo = consumo;
                                Element elem = GetElementTypeFromQuantity(q);
                                if (rec.Insert(elem))
                                {
                                    count++;
                                }
                                else
                                {
                                    errores.AppendLine("El recurso " + mat.nombre + " de la Quantificación " + q.name +
                                        " no se pudo importar");
                                }
                            }
                        }
                        if (qUnit != unidad)
                        {
                            errores.AppendLine(q.name + " no tiene la misma unidad. No se importaron sus recursos");
                        }
                    }
                }
                countRow++;
            }
            if (count > 0)
            {
                MessageBox.Show("Se importaron " + count + " recursos", RevitQex.QexName,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (errores.Length > 0)
                {
                    (new frmWarnings("Errores encontrados", "Lista de errores encontrados durante la importación",
                        errores.ToString())).ShowDialog();
                }
            }
        }

        public static List<ItemRecurso> GetItemRecursosFromQuantities(List<Quantity> quantities)
        {
            List<ItemRecurso> items = new List<ItemRecurso>();
            foreach (Quantity q in quantities)
            {
                foreach (QexRecurso rec in GetRecursosFromQuantity(q))
                {
                    ItemRecurso item = rec.ToItemRecurso();
                    if (items.Exists(x => x.nombre == item.nombre))
                    {
                        ItemRecurso item0 = items.FirstOrDefault(x => x.nombre == item.nombre);
                        item0.consumo = Math.Round(item0.consumo + (q.GetMedicion() * rec.Consumo), 2);
                        item0.costoTotal = Math.Round(item0.consumo * item0.precioUnit, 2);
                    }
                    else
                    {
                        item.consumo = Math.Round(q.GetMedicion() * rec.Consumo, 2);
                        item.costoTotal = Math.Round(item.consumo * item.precioUnit, 2);
                        items.Add(item);
                    }
                }
            }
            items = items.OrderBy(x => x.nombre).ToList();
            return items;
        }
        #endregion

        #region Stats
        /// <summary> Inserta un registro de Log en la Base de Datos Sqlite de Qex </summary>
        public static void InsertDbOpenLog()
        {
            //Insert Open Log
            using (var datos = new DataAccess())
            {
                dbOpenLog log = new dbOpenLog(DateTime.Now, _doc.Title, 0);
                datos.InsertDbOpenLog(log);
            };
        }
        #endregion

        #region Opciones
        /// <summary> Convierte las Opciones seleccionadas en una cadena de Texto </summary>
        public static string StringOptions()
        {
            string text = QexOpciones.wallsMaterials.ToString() + ";" + QexOpciones.floorMaterials.ToString() + ";" +
                QexOpciones.ceilingMaterials.ToString() + ";" + QexOpciones.roofMaterials.ToString() + ";" +
                QexOpciones.strFoundationsFloorMaterials.ToString() + ";" +
                QexOpciones.padsMaterials.ToString() + ";" + QexOpciones.rebarMarca.ToString() + ";" +
                QexOpciones.QuantityByPhase.ToString() + ";" + QexOpciones.QuantityByLevel.ToString() +
                ";" + QexOpciones.QuantityByGroup.ToString() + ";" + QexOpciones.QuantityByAssembly;
            return text;
        }

        /// <summary> A partir de la cadena de Texto de las opciones, se establecen las opciones en Qex </summary>
        public static void SetOptions(string text)
        {
            string[] values = text.Split(';');
            QexOpciones.wallsMaterials = Convert.ToBoolean(values[0]);
            QexOpciones.floorMaterials = Convert.ToBoolean(values[1]);
            QexOpciones.ceilingMaterials = Convert.ToBoolean(values[2]);
            QexOpciones.roofMaterials = Convert.ToBoolean(values[3]);
            QexOpciones.strFoundationsFloorMaterials = Convert.ToBoolean(values[4]);
            QexOpciones.padsMaterials = Convert.ToBoolean(values[5]);
            QexOpciones.rebarMarca = Convert.ToBoolean(values[6]);
            QexOpciones.QuantityByPhase = Convert.ToBoolean(values[7]);
            QexOpciones.QuantityByLevel = Convert.ToBoolean(values[8]);
            try
            {
                QexOpciones.QuantityByGroup = Convert.ToBoolean(values[9]);
                QexOpciones.QuantityByAssembly = Convert.ToBoolean(values[10]);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Winforms
        /// <summary> Rellena un Combo con los Grupos del Proyecto </summary>
        public static void FillComboGruposMaterial(System.Windows.Forms.ComboBox combo, QexGrupoMaterial selectedGrupo)
        {
            Dictionary<string, string> comboSource = new Dictionary<string, string>();
            List<QexGrupoMaterial> grupos = DalRecursos.ReadGrupos();
            grupos = grupos.OrderBy(x => x.Nombre).ToList();
            foreach (QexGrupoMaterial grupo in grupos)
            {
                comboSource.Add(grupo.id, grupo.Nombre);
            }
            if (grupos.Count >0)
            {
                combo.DataSource = new BindingSource(comboSource, null);
                combo.ValueMember = "Key";
                combo.DisplayMember = "Value";
                
                combo.SelectedValue = selectedGrupo.id;
            }
        }

        public static bool GetBoolFromDataGridView(System.Windows.Forms.DataGridView grid, int column, int row)
        {
            return Convert.ToBoolean(grid.Rows[row].Cells[column].Value);
        }

        public static void SetBooleanCheckToDataGridView(System.Windows.Forms.DataGridView grid
            , int column, int row, bool value)
        {
            grid.Rows[row].Cells[column].Value = value;
        }

        public static List<Quantity> GetCheckedQuantitiesFromGrid(System.Windows.Forms.DataGridView grid)
        {
            List<Quantity> quantities = new List<Quantity>();
            int indexCheck = grid.Columns["check"].Index;
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                if (GetBoolFromDataGridView(grid, indexCheck, i))
                {
                    Quantity q = (Quantity)grid.Rows[i].DataBoundItem;
                    quantities.Add(q);
                }
            }
            return quantities;
        }

        public static void GridUncheckAllRows(System.Windows.Forms.DataGridView grid)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                int colCheck = grid.Columns["check"].Index;
                Tools.SetBooleanCheckToDataGridView(grid, colCheck, row.Index, false);
            }
        }

        public static void fillTreeView(List<Quantity> lstQuantity, TreeView tree)
        {
            List<Tview> lstTviews = new List<Tview>();
            //LEVEL 0 Title
            Tview tv0 = new Tview(_doc.Title, "");
            lstTviews.Add(tv0);
            //LEVEL 1 Phases
            List<Tview> lstLevel1 = new List<Tview>();

            foreach (var item in lstQuantity)
            {
                int lvlId = -1;
                string levelName = "Sin Nivel";
                if (item.LevelId != -1)
                {
                    Element lvl = _doc.GetElement(new ElementId(item.LevelId));
                    levelName = lvl.Name;
                    lvlId = lvl.Id.IntegerValue;
                }
                if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                {
                    if (!lstLevel1.Exists(x => x.name == "Categorias"))
                    {
                        Tview node = new Tview();
                        node.name = "Categorias";
                        node.text = "Categorias";
                        node.parent = _doc.Title;
                        lstLevel1.Add(node);
                    }
                }
                if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                {
                    if (!lstLevel1.Exists(x => x.name == item.phaseCreated))
                    {
                        Tview node = new Tview();
                        node.name = item.phaseCreated;
                        node.text = item.phaseCreated;
                        node.parent = _doc.Title;
                        lstLevel1.Add(node);
                    }
                }
                if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                {
                    if (!lstLevel1.Exists(x => x.name == lvlId.ToString()))
                    {
                        Tview node = new Tview();
                        node.name = lvlId.ToString();
                        node.text = levelName;
                        node.parent = _doc.Title;
                        lstLevel1.Add(node);
                    }
                }
                if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                {
                    if (!lstLevel1.Exists(x => x.name == item.phaseCreated + " // " + lvlId.ToString()))
                    {
                        Tview node = new Tview();
                        node.name = item.phaseCreated + " // " + lvlId.ToString();
                        node.text = item.phaseCreated + " // " + levelName;
                        node.parent = _doc.Title;
                        lstLevel1.Add(node);
                    }
                }
            }
            lstLevel1 = lstLevel1.OrderBy(x => x.text).ToList();
            foreach (var item in lstLevel1)
            {
                lstTviews.Add(item);
            }
            //LEVEL 2 Categories
            foreach (var node1 in lstLevel1)
            {
                List<Tview> lstLevel2 = new List<Tview>();
                foreach (var item in lstQuantity)
                {
                    if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                    {
                        if (!lstLevel2.Exists(x => x.name == item.category))
                        {
                            Tview node = new Tview();
                            node.name = item.category;
                            node.text = item.category;
                            node.parent = "Categorias";
                            lstLevel2.Add(node);
                        }
                    }
                    if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                    {
                        if (item.phaseCreated == node1.name)
                        {
                            if (!lstLevel2.Exists(x => x.name == item.category))
                            {
                                Tview node = new Tview();
                                node.name = item.category;
                                node.text = item.category;
                                node.parent = item.phaseCreated;
                                lstLevel2.Add(node);
                            }
                        }
                    }
                    if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                    {
                        if (item.LevelId.ToString() == node1.name)
                        {
                            if (!lstLevel2.Exists(x => x.name == item.category))
                            {
                                Tview node = new Tview();
                                node.name = item.category;
                                node.text = item.category;
                                node.parent = item.LevelId.ToString();
                                lstLevel2.Add(node);
                            }
                        }
                    }
                    if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                    {
                        string groupName = item.phaseCreated + " // " + item.LevelId.ToString();

                        if (groupName == node1.name)
                        {
                            if (!lstLevel2.Exists(x => x.name == item.category))
                            {
                                Tview node = new Tview();
                                node.name = item.category;
                                node.text = item.category;
                                node.parent = groupName;
                                lstLevel2.Add(node);
                            }
                        }
                    }
                }
                lstLevel2 = lstLevel2.OrderBy(x => x.name).ToList();
                foreach (var item in lstLevel2)
                {
                    lstTviews.Add(item);
                }
            }
            List<Tview> parentTviews = new List<Tview>();

            foreach (var item in lstTviews)
            {
                if (item.parent == "")
                {
                    TreeNode node0 = new TreeNode();
                    node0.Name = _doc.Title;
                    node0.Text = _doc.Title;
                    node0.ImageIndex = 0;
                    node0.SelectedImageIndex = 0;
                    parentTviews.Add(item);
                    tree.Nodes.Add(node0);
                }
                else
                {

                }
            }
            foreach (var item in parentTviews)
            {
                readChild(tree, lstTviews, item);
            }
        }

        public static void readChild(TreeView tree, List<Tview> lstTviews, Tview tv)
        {
            foreach (Tview t in tv.GetChilds(lstTviews))
            {
                TreeNode node = new TreeNode();
                node.Name = t.name;
                node.Text = t.text;
                node.ImageIndex = 2;
                node.SelectedImageIndex = 2;
                if (t.text.Contains("(Materiales)"))
                {
                    node.ImageIndex = 3;
                    node.SelectedImageIndex = 3;
                }
                if (t.text.Contains("(Pinturas)"))
                {
                    node.ImageIndex = 3;
                    node.SelectedImageIndex = 3;
                }
                if (t.text.Contains("(Demolido)"))
                {
                    node.ImageIndex = 4;
                    node.SelectedImageIndex = 4;
                }
                if (t.text.Contains("(Grupo)"))
                {
                    node.ImageIndex = 5;
                    node.SelectedImageIndex = 5;
                }
                if (t.text.Contains("Montajes"))
                {
                    node.ImageIndex = 6;
                    node.SelectedImageIndex = 6;
                }
                TreeNode parentNode = new TreeNode();
                parentNode = tree.Nodes.Find(t.parent, true).First<TreeNode>();
                parentNode.ImageIndex = 1;
                parentNode.SelectedImageIndex = 1;

                parentNode.Nodes.Add(node);
                tree.Nodes[0].Expand();

                readChild(tree, lstTviews, t);
            }
        }

        #endregion

        #region Excel
        public static void ExportQuantitiesToExcel(string fase, string faseText, List<Quantity> quantities, bool tipos)
        {
            List<string> lstInforme = Informe.CrearListaParaExcel(quantities, fase, tipos);
            ExportToExcel(lstInforme, faseText);
        }

        public static void ExportQuantitiesToWord(string fase, string faseText, List<Quantity> quantities, bool tipos)
        {
            Log.Clean();
            Log.Start("*** Log iniciado ***");
            Log.Insert("Creando lista para Word");
            List<string> lstInforme = Informe.CrearListaParaWord(quantities, fase, tipos);
            Log.Insert("Exportando a Word");
            ExportToWord(lstInforme, faseText);
            Log.End("*** Log finalizado ***");
        }

        public static void ExportToExcel(List<string> lstInforme, string fase)
        {
            //Exporting to Excel
            string folderPath = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Documento Excel|*.xlsx";
            string title = _doc.Title;
            sfd.FileName = title + " - " + fase + ".xlsx";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderPath = sfd.FileName;
                try
                {
                    Informe.CrearExcelImages(title, lstInforme, fase, folderPath);
                    DialogResult result2 = MessageBox.Show("El documento se ha creado correctamente. " +
                        "¿Desea abrirlo?",
                        RevitQex.QexName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    switch (result2)
                    {
                        case DialogResult.Yes:
                            System.Diagnostics.Process.Start(folderPath);
                            break;
                        case DialogResult.No:
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("No se puede crear el documento. Verifica que el documento no se encuentra abierto");
                    sb.AppendLine(ex.Message);
                    MessageBox.Show(sb.ToString(), RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public static void ExportToWord(List<string> lstInforme, string folder)
        {
            //Exporting to Word
            string folderPath = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Documento Word|*.docx";
            string title = _doc.Title;
            sfd.FileName = title + " - " + folder + ".docx";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderPath = sfd.FileName;
                try
                {
                    Informe.CrearWord(title, lstInforme, folder, folderPath);
                    DialogResult result2 = MessageBox.Show("El documento se ha creado correctamente. " +
                        "¿Desea abrirlo?",
                        RevitQex.QexName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    switch (result2)
                    {
                        case DialogResult.Yes:
                            System.Diagnostics.Process.Start(folderPath);
                            break;
                        case DialogResult.No:
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("No se puede crear el documento. Verifique que el archivo no esté abierto");
                    sb.AppendLine(ex.Message);
                    MessageBox.Show(sb.ToString(), RevitQex.QexName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        #endregion

        public static int TotalNodes(TreeNode node0)
        {
            int total = 0;
            foreach (TreeNode node1 in node0.Nodes)
            {
                total++;
                foreach (TreeNode node2 in node1.Nodes)
                {
                    total++;
                }
            }
            return total;
        }

        public static string GetCleanedName(string name)
        {
            name = name.Replace(":", "-");
            name = name.Replace("/", "-");
            name = name.Replace(" >> ", "-");
            name = name.Replace('"', ' ');
            name = name.Replace('?', ' ');
            name = name.Replace('|', '-');
            name = name.Replace('*', ' ');
            name = name.Replace("'", "-");
            return name;
        }
    }
}
