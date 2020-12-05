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

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System.Text.RegularExpressions;
#endregion

namespace Qex
{
    /// <summary>
    /// Esta Clase abstracta reune los metodos para gestionar Schemas en Revit
    /// </summary>
    public abstract class QexSchema
    {
        public static int _projInfoId;
        public static string _schemaQex = "Qex30";
        public static string _schemaComputo = "QexComputos30";
        public static string _schemaOpciones = "QexOptions30";


        #region Gestion de Schema Qex30
        /// <summary> Devuelve True si se puede crear el Schema llamado Qex </summary>
        public static bool CreateSchemaQex(Document doc)
        {
            bool exito = false;
            try
            {
                using (Transaction t = new Transaction(doc, "Qex: Crear Schema"))
                {
                    t.Start();
                    SchemaBuilder sb = new SchemaBuilder(Guid.NewGuid());
                    sb.SetReadAccessLevel(AccessLevel.Public);
                    sb.SetWriteAccessLevel(AccessLevel.Public);
                    sb.SetSchemaName(_schemaQex);

                    //Create Fields
                    FieldBuilder fbCost = sb.AddSimpleField("Cost", typeof(double));
                    fbCost.SetDocumentation("Costo Unitario");
                    fbCost.SetUnitType(UnitType.UT_Currency);
                    FieldBuilder fbMatriz = sb.AddSimpleField("QexMatriz", typeof(string));
                    fbMatriz.SetDocumentation("Matriz de la Quantificacion");
                    FieldBuilder fbVisible = sb.AddSimpleField("QexVisible", typeof(bool));
                    fbVisible.SetDocumentation("Visibilidad de la Quantificacion");
                    FieldBuilder fbTexto1 = sb.AddSimpleField("QexTexto1", typeof(string)); // Se utiliza este campo
                    // para guardar el ID del parametro seleccionado
                    fbTexto1.SetDocumentation("Texto1");
                    // Se utiliza este campo para guardar el orden de Quantificaciones Grupos y Libros, separados por ";"
                    FieldBuilder fbTexto2 = sb.AddSimpleField("QexTexto2", typeof(string));
                    fbTexto2.SetDocumentation("Texto2");
                    Schema schema = sb.Finish();
                    t.Commit();
                    exito = true;
                };
            }
            catch (Exception)
            {
                exito = false;
            }
            return exito;
        }

        /// <summary> Obtiene una Lista de Schemas presentes en el Documento </summary>
        public static List<Schema> GetSchemas()
        {
            IList<Schema> schemas = Schema.ListSchemas();
            return schemas.ToList();
        }

        /// <summary> Devuelve True si existe un Schema con el nombre dado </summary>
        public static bool SchemaExist(string name)
        {
            bool exito = false;
            IList<Schema> schemas = Schema.ListSchemas();

            foreach (Schema sch in schemas)
            {
                if (sch.SchemaName == name)
                {
                    exito = true;
                }
            }
            return exito;
        }

        /// <summary> Obtiene el Schema según un nombre dado. Devuelve NULL si el Schema no existe </summary>
        public static Schema GetSchemaByName(string name)
        {
            Schema sc = null;
            List<Schema> lstSchemas = GetSchemas();

            foreach (Schema schema in lstSchemas)
            {
                if (schema.SchemaName == name)
                {
                    sc = schema;
                }
            }
            return sc;
        }
        #endregion

        #region Leer y escribir Matrices
        /// <summary> Devuelve True si puede crear los campos del Schema dado y asignar el Schema a un Elemento </summary>
        public static bool CreateFields(Document doc, Schema schema, int elemId, double cost, string matriz,
            bool visible, string texto1, string texto2)
        {
            bool exito = false;
            try
            {
                using (Transaction t = new Transaction(doc, "Qex: Crear Matriz"))
                {
                    t.Start();
                    //Get the field from schema
                    Field sbCost = schema.GetField("Cost");
                    Field sbMatriz = schema.GetField("QexMatriz");
                    Field sbVisible = schema.GetField("QexVisible");
                    Field sbTexto1 = schema.GetField("QexTexto1");
                    Field sbTexto2 = schema.GetField("QexTexto2");
                    Entity entityCost = new Entity(schema);
                    entityCost.Set<double>(sbCost, cost, DisplayUnitType.DUT_CURRENCY);
                    entityCost.Set<string>(sbMatriz, matriz);
                    entityCost.Set<bool>(sbVisible, visible);
                    entityCost.Set<string>(sbTexto1, texto1);
                    entityCost.Set<string>(sbTexto2, texto2);
                    Element e = doc.GetElement(new ElementId(elemId));
                    e.SetEntity(entityCost);
                    t.Commit();
                    exito = true;
                };
            }
            catch (Exception)
            {
                exito = false;
            }
            return exito;
        }

        /// <summary> Obtiene la Matriz del Schema asociado al Elemento. Si el elemento es un Material devuelve 0010 </summary>
        public static string GetMatrizFromSchema(Element ft)
        {
            string matriz = "1000";
            if (ft.Category.Id == new ElementId(BuiltInCategory.OST_Materials))
            {
                matriz = "0010";
            }
            
            if (QexSchema.SchemaExist(_schemaQex))
            {
                Schema Qschema = QexSchema.GetSchemaByName(_schemaQex);
                if (ft.GetEntity(Qschema).IsValid())
                {
                    Entity retrieveEntity = ft.GetEntity(Qschema);
                    matriz = retrieveEntity.Get<string>("QexMatriz");
                }
            }
            return matriz;
        }
        #endregion

        #region Leer y escribir Computos
        /// <summary> Crea el Schema QexComputos </summary>
        public static void CreateSchemaComputos(Document doc)
        {
            using (Transaction t = new Transaction(doc, "Qex: Crear Schema"))
            {
                t.Start();
                SchemaBuilder sb = new SchemaBuilder(Guid.NewGuid());
                sb.SetReadAccessLevel(AccessLevel.Public);
                sb.SetWriteAccessLevel(AccessLevel.Public);
                //sb.SetVendorId("UniBIM");
                sb.SetSchemaName(_schemaComputo);

                //Create Fields
                FieldBuilder computos = sb.AddSimpleField("computos", typeof(string));
                computos.SetDocumentation("Computo del modelo");

                Schema schema = sb.Finish();
                t.Commit();
            };
        }

        /// <summary> Devuelve True si el Schema QexComputos existe en el elemento ProjectInfo </summary>
        public static bool EntityComputosExist(Document doc)
        {
            bool existe = false;
            Element elem = doc.GetElement(new ElementId(_projInfoId));
            Schema schema = GetSchemaByName(_schemaComputo);
            if (elem.GetEntity(schema).IsValid())
            {
                existe = true;
            }
            else
            {
                existe = false;
            }
            return existe;
        }

        /// <summary> Guarda el String de computo en el Schema QexComputo del elemento ProjectInfo </summary>
        public static void CrearEntidadComputos(Document doc, string stringComputo)
        {
            using (Transaction t = new Transaction(doc, "Qex: Crear Computo"))
            {
                t.Start();
                //Get the field from schema
                Schema schema = GetSchemaByName(_schemaComputo);
                Field computos = schema.GetField("computos");
                Entity entityMat = new Entity(schema);
                entityMat.Set<string>(computos, stringComputo);
                Element e = doc.GetElement(new ElementId(_projInfoId));
                e.SetEntity(entityMat);
                t.Commit();
            };
        }

        /// <summary> Obtiene la Lista de Quantificaciones guardada en el Schema QexComputos del elemento ProjectInfo </summary>
        public static List<Quantity> ReadComputos(Document doc)
        {
            List<Quantity> lstQuantities = new List<Quantity>();
            Element elem = doc.GetElement(new ElementId(_projInfoId));
            Schema schema = GetSchemaByName(_schemaComputo);
            if (EntityComputosExist(Tools._doc))
            {
                Entity entity = elem.GetEntity(schema);
                string computos = entity.Get<string>("computos");
                string[] lines = computos.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; i++)
                {
                    //qid;typeId;matriz;grupo;libro
                    string[] values = Regex.Split(lines[i], ";");
                    string qid = values[0];
                    int typeId = Convert.ToInt32(values[1]);
                    string matriz = values[2];
                    string grupo = values[3];
                    string libro = values[4];
                    Quantity q = new Quantity();
                    q.qId = qid;
                    q.typeId = typeId;
                    q.matriz = matriz;
                    q.grupo = grupo;
                    q.libro = libro;
                    lstQuantities.Add(q);
                }
            }
            return lstQuantities;
        }
        #endregion

        #region QexOpciones
        /// <summary> Crea el Schema QexOptions en el Documento actual </summary>
        public static void CreateSchemaQexOptions(Document doc)
        {
            using (Transaction t = new Transaction(doc, "Qex: Crear Schema"))
            {
                t.Start();
                SchemaBuilder sb = new SchemaBuilder(Guid.NewGuid());
                sb.SetReadAccessLevel(AccessLevel.Public);
                sb.SetWriteAccessLevel(AccessLevel.Public);
                //sb.SetVendorId("UniBIM");
                sb.SetSchemaName(_schemaOpciones);

                //Create Fields
                FieldBuilder qexOptions = sb.AddSimpleField("qexOptions", typeof(string));
                qexOptions.SetDocumentation("Qex Options");

                Schema schema = sb.Finish();
                t.Commit();
            };
        }

        /// <summary> Devuelve True si el Schema QexOptions existe en el elemento ProjectInfo </summary>
        public static bool EntityQexOptionsExist(Document doc)
        {
            bool exist = false;
            Element elem = doc.GetElement(new ElementId(_projInfoId));
            if (SchemaExist(_schemaOpciones))
            {
                Schema schema = GetSchemaByName(_schemaOpciones);
                if (elem.GetEntity(schema).IsValid())
                {
                    exist = true;
                }
                else
                {
                    exist = false;
                }
            }
            else
            {
                exist = false;
            }
            return exist;
        }

        /// <summary> Guarda el String de Opciones en el Schema QexOptions del elemento ProjectInfo </summary>
        public static void CrearEntidadQexOptions(Document doc, string stringQexOptions)
        {
            using (Transaction t = new Transaction(doc, "Qex: Crear Opciones"))
            {
                t.Start();
                //Get the field from schema
                Schema schema = GetSchemaByName(_schemaOpciones);
                Field qexOptions = schema.GetField("qexOptions");
                Entity entityMat = new Entity(schema);
                entityMat.Set<string>(qexOptions, stringQexOptions);
                Element e = doc.GetElement(new ElementId(_projInfoId));
                e.SetEntity(entityMat);
                t.Commit();
            };
        }

        /// <summary> Obtiene un String con las opciones guardadas en el Schema QexOptions del elemento ProjectInfo </summary>
        public static string ReadQexOptions(Document doc)
        {
            Element elem = doc.GetElement(new ElementId(_projInfoId));
            Schema schema = GetSchemaByName(_schemaOpciones);
            Entity entity = elem.GetEntity(schema);
            string qexOptions = entity.Get<string>("qexOptions");
            return qexOptions;
        }
        #endregion
    }
}
