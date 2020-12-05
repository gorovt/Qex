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
    /// <summary> Esta Clase abstracta se conecta con la info guardada en los Schemas de Recursos </summary>
    public abstract class DalRecursos
    {
        // Los Mateeriales se guardan en el ProjectInfo
        // Los Grupos de Materiales se guardan en el ProjectInfo
        // Los Recursos o Consumos se guardan en los Elementos (Tipos de Familia y Materiales)

        //Parametros necesarios
        public static Document _doc;
        public static int _projInfoId;
        public static string _schemaRecursos = "QexRecursos";
        public static string _schemaGrupos = "QexGrupoMateriales";
        public static string _schemaMateriales = "QexMateriales";
        
        #region Schemas
        /// <summary>Devuelve True si se crea el Schema QexRecursos, y False si no puede crear el Schema</summary>
        public static bool CreateSchemaRecursos()
        {
            bool exito = false;
            try
            {
                using (Transaction t = new Transaction(_doc, "Qex: Crear Schema"))
                {
                    t.Start();
                    SchemaBuilder sb = new SchemaBuilder(Guid.NewGuid());
                    sb.SetReadAccessLevel(AccessLevel.Public);
                    sb.SetWriteAccessLevel(AccessLevel.Public);
                    //sb.SetVendorId("UniBIM");
                    sb.SetSchemaName(_schemaRecursos);

                    //Create Fields
                    FieldBuilder recursos = sb.AddSimpleField("recursos", typeof(string));
                    recursos.SetDocumentation("Recurso del elemento");

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

        /// <summary>Devuelve True si se crea el Schema QexGrupoMateriales, y False si no se puede crear el Schema</summary>
        public static bool CreateSchemaGrupos()
        {
            bool exito = false;
            try
            {
                using (Transaction t = new Transaction(_doc, "Qex: Crear Schema"))
                {
                    t.Start();
                    SchemaBuilder sb = new SchemaBuilder(Guid.NewGuid());
                    sb.SetReadAccessLevel(AccessLevel.Public);
                    sb.SetWriteAccessLevel(AccessLevel.Public);
                    //sb.SetVendorId("UniBIM");
                    sb.SetSchemaName(_schemaGrupos);

                    //Create Fields
                    FieldBuilder grupo = sb.AddSimpleField("grupo", typeof(string));
                    grupo.SetDocumentation("Grupo de Materiales");

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

        /// <summary>Devuelve True si se crea el Schema QexMateriales, y False si no se puede crear el Schema</summary>
        public static bool CreateSchemaMateriales()
        {
            bool exito = false;
            try
            {
                using (Transaction t = new Transaction(_doc, "Qex: Crear Schema"))
                {
                    t.Start();
                    SchemaBuilder sb = new SchemaBuilder(Guid.NewGuid());
                    sb.SetReadAccessLevel(AccessLevel.Public);
                    sb.SetWriteAccessLevel(AccessLevel.Public);
                    //sb.SetVendorId("UniBIM");
                    sb.SetSchemaName(_schemaMateriales);

                    //Create Fields
                    FieldBuilder materiales = sb.AddSimpleField("materiales", typeof(string));
                    materiales.SetDocumentation("Materiales del Proyecto");
                    FieldBuilder grupos = sb.AddSimpleField("grupos", typeof(string));
                    grupos.SetDocumentation("Grupos de Materiales");

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

        /// <summary>Devuelve True si el elemento tiene una entidad válida del Schema</summary>
        public static bool EntityExist(Element elem, string schemaName)
        {
            bool exito = false;
            try
            {
                Schema schema = GetSchemaByName(schemaName);
                if (elem.GetEntity(schema).IsValid())
                {
                    exito = true;
                }
            }
            catch (Exception)
            {
                exito = false;
            }
            return exito;
        }

        /// <summary>Obtiene una Lista de Schemas del Documento actual</summary>
        public static List<Schema> GetSchemas()
        {
            return Schema.ListSchemas().ToList();
        }

        /// <summary>Devuelve True si el Schema existe en el Documento actual</summary>
        public static bool SchemaExist(string name)
        {
            bool existe = false;
            foreach (Schema sch in GetSchemas())
            {
                if (sch.SchemaName == name)
                {
                    existe = true;
                }
            }
            return existe;
        }

        /// <summary>Obtiene un Schema por su nombre. Devulve Null si el Schema no existe</summary>
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

        #region Entidades
        /// <summary> Devuelve True si pueden guardar los Recursos en el Elemento </summary>
        public static bool CrearEntidadRecursos(List<QexRecurso> lstRecursos, Element elem)
        {
            bool creado = false;
            try
            {
                using (Transaction t = new Transaction(_doc, "Qex: Crear Recursos"))
                {
                    t.Start();
                    //Get the field from schema
                    Schema schema = GetSchemaByName(_schemaRecursos);
                    Field recField = schema.GetField("recursos");
                    Entity entityRec = new Entity(schema);

                    StringBuilder sb = new StringBuilder();
                    foreach (QexRecurso rec in lstRecursos)
                    {
                        sb.AppendLine(rec.ToLine());
                    }

                    entityRec.Set<string>(recField, sb.ToString());

                    elem.SetEntity(entityRec);
                    t.Commit();
                    creado = true;
                };
            }
            catch (Exception)
            {
                creado = false;
            }
            return creado;
        }

        /// <summary> Devuelve True si pueden guardar los GruposMaterial en el ProjectInfo </summary>
        public static bool CrearEntidadGrupos(List<QexGrupoMaterial> lstGrupos)
        {
            bool exito = false;
            try
            {
                using (Transaction t = new Transaction(_doc, "Qex: Crear Grupos"))
                {
                    t.Start();
                    //Get the field from schema
                    Schema schema = GetSchemaByName(_schemaGrupos);
                    Field recField = schema.GetField("grupo");
                    Entity entityRec = new Entity(schema);

                    StringBuilder sb = new StringBuilder();
                    foreach (QexGrupoMaterial grupo in lstGrupos)
                    {
                        sb.AppendLine(grupo.ToLine());
                    }

                    entityRec.Set<string>(recField, sb.ToString());

                    ElementId id = new ElementId(Tools.GetProjectInformationId(Tools._doc));
                    Element elem = Tools._doc.GetElement(id);
                    elem.SetEntity(entityRec);
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

        /// <summary> Devuelve True si pueden guardar los Materiales en el ProjectInfo </summary>
        public static bool CrearEntidadMateriales(List<QexMaterial> lstMaterial)
        {
            bool exito = false;
            try
            {
                using (Transaction t = new Transaction(_doc, "Qex: Crear Materiales"))
                {
                    t.Start();
                    //Get the field from schema
                    Schema schema = GetSchemaByName(_schemaMateriales);
                    Field recField = schema.GetField("materiales");
                    Entity entityRec = new Entity(schema);

                    StringBuilder sb = new StringBuilder();
                    foreach (QexMaterial mat in lstMaterial)
                    {
                        sb.AppendLine(mat.ToLine());
                    }

                    entityRec.Set<string>(recField, sb.ToString());

                    ElementId id = new ElementId(Tools.GetProjectInformationId(Tools._doc));
                    Element elem = Tools._doc.GetElement(id);
                    elem.SetEntity(entityRec);
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
        #endregion

        #region DataBase
        /// <summary> Obtiene el GrupoMaterial a partir de su Id </summary>
        public static QexGrupoMaterial GetGrupoMaterialById(string id)
        {
            List<QexGrupoMaterial> grupos = ReadGrupos();
            return grupos.FirstOrDefault(x => x.id == id);
        }

        /// <summary> Obtiene el Material a partir de su Id </summary>
        public static QexMaterial GetMaterialById(string id)
        {
            return ReadMateriales().FirstOrDefault(x => x.id == id);
        }

        /// <summary> Obtiene el Recurso a partir del Id y del Elemento </summary>
        public static QexRecurso GetRecursoById(Element elem, string id)
        {
            return ReadRecursosFromElement(elem).FirstOrDefault(x => x.id == id);
        }

        /// <summary> Obtiene el último Grupo insertado </summary>
        public static QexGrupoMaterial GetLastGrupo()
        {
            return ReadGrupos().LastOrDefault();
        }

        /// <summary> Devuelve una Lista de QexGrupoMaterial guardada en el ProjectInfo </summary>
        public static List<QexGrupoMaterial> ReadGrupos()
        {
            List<QexGrupoMaterial> lstGrupos = new List<QexGrupoMaterial>();
            Element elem = _doc.GetElement(new ElementId(_projInfoId));
            Schema schema = GetSchemaByName(_schemaGrupos);
            if (elem.GetEntity(schema).IsValid())
            {
                Entity entity = elem.GetEntity(schema);
                string grupos = entity.Get<string>("grupo");
                string[] lines = grupos.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] values = Regex.Split(lines[i], ";");
                    string id = values[0];
                    string nombre = values[1];
                    QexGrupoMaterial gm = new QexGrupoMaterial(id, nombre);
                    lstGrupos.Add(gm);
                }
            }
            return lstGrupos;
        }

        /// <summary> Devuelve una Lista de Materiales guardada en el ProjectInfo </summary>
        public static List<QexMaterial> ReadMateriales()
        {
            List<QexMaterial> lstMateriales = new List<QexMaterial>();
            Element elem = _doc.GetElement(new ElementId(_projInfoId));
            Schema schema = GetSchemaByName(_schemaMateriales);
            if (elem.GetEntity(schema).IsValid())
            {
                Entity entity = elem.GetEntity(schema);
                string material = entity.Get<string>("materiales");
                string[] lines = material.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] values = Regex.Split(lines[i], ";");
                    string id = values[0];
                    string nombre = values[1];
                    string unidad = values[2];
                    string grupo = values[3];
                    int index = 0;
                    double precio = 0;
                    try
                    {
                        precio = Convert.ToDouble(values[4]);
                        index = Convert.ToInt32(values[5]);
                    }
                    catch (Exception)
                    {

                    }
                    QexMaterial mat = new QexMaterial(id, nombre, unidad, grupo, precio, index);
                    lstMateriales.Add(mat);
                }
                lstMateriales = lstMateriales.OrderBy(x => x.nombre).ToList();
            }
            return lstMateriales;
        }

        /// <summary> Obtiene una Lista de Recursos guardados en el Elemento </summary>
        public static List<QexRecurso> ReadRecursosFromElement(Element elem)
        {
            List<QexRecurso> lstRecursos = new List<QexRecurso>();
            Schema schema = DalRecursos.GetSchemaByName(_schemaRecursos);
            if (elem.GetEntity(schema).IsValid())
            {
                Entity entity = elem.GetEntity(schema);
                string recurso = entity.Get<string>("recursos");
                string[] lines = recurso.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] values = Regex.Split(lines[i], ";");
                    string id = values[0];
                    string matId = values[1];
                    double consumo = Convert.ToDouble(values[2]);
                    QexRecurso rec = new QexRecurso(id, matId, consumo);
                    lstRecursos.Add(rec);
                }
            }
            return lstRecursos;
        }

        /// <summary> Devuelve True si puede guardar un QexGrupoMaterial en el ProjectInfo </summary>
        public static bool InsertGrupo(QexGrupoMaterial grupo)
        {
            bool insertado = false;
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (QexGrupoMaterial gr in ReadGrupos())
                {
                    sb.AppendLine(gr.ToLine());
                }
                sb.AppendLine(grupo.ToLine());

                Element elem = _doc.GetElement(new ElementId(_projInfoId));
                Schema schema = GetSchemaByName(_schemaGrupos);
                if (elem.GetEntity(schema).IsValid())
                {
                    Entity entity = elem.GetEntity(schema);
                    using (Transaction t = new Transaction(_doc, "Qex: Create Grupo"))
                    {
                        t.Start();
                        //Get the field from schema
                        Field grField = schema.GetField("grupo");
                        entity.Set<string>(grField, sb.ToString());
                        elem.SetEntity(entity);
                        t.Commit();
                    };
                    insertado = true;
                }
                else
                {
                    insertado = false;
                }
            }
            catch (Exception ex)
            {
                insertado = false;
            }
            return insertado;
        }

        /// <summary> Devuelve True si puede guardar un QexMaterial en el ProjectInfo </summary>
        public static bool InsertMaterial(QexMaterial material)
        {
            bool insertado = false;
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (QexMaterial mat in ReadMateriales())
                {
                    sb.AppendLine(mat.ToLine());
                }
                sb.AppendLine(material.ToLine());

                Element elem = _doc.GetElement(new ElementId(_projInfoId));
                Schema schema = GetSchemaByName(_schemaMateriales);
                if (elem.GetEntity(schema).IsValid())
                {
                    Entity entity = elem.GetEntity(schema);
                    using (Transaction t = new Transaction(_doc, "Qex: Crear Material"))
                    {
                        t.Start();
                        //Get the field from schema
                        Field matField = schema.GetField("materiales");
                        entity.Set<string>(matField, sb.ToString());
                        elem.SetEntity(entity);
                        t.Commit();
                    };
                    insertado = true;
                }
                else
                {
                    insertado = false;
                }
            }
            catch (Exception)
            {
                insertado = false;
            }
            return insertado;
        }

        /// <summary> Devuelve True si se agregó el recurso al Elemento </summary>
        public static bool InsertRecurso(QexRecurso recurso, Element elem)
        {
            bool exito = false;
            try
            {
                List<QexRecurso> recursos = ReadRecursosFromElement(elem);
                recursos.Add(recurso);
                if (CrearEntidadRecursos(recursos, elem))
                {
                    exito = true;
                }
            }
            catch (Exception)
            {
                exito = false;
            }
            return exito;
        }

        /// <summary> Devuelve True si se pueden actualizar los Grupos en el ProjectInfo </summary>
        public static bool UpdateGrupos(List<QexGrupoMaterial> grupos)
        {
            bool actualizado = false;
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (QexGrupoMaterial gr in grupos)
                {
                    sb.AppendLine(gr.ToLine());
                }

                Element elem = _doc.GetElement(new ElementId(_projInfoId));
                Schema schema = GetSchemaByName(_schemaGrupos);
                if (elem.GetEntity(schema).IsValid())
                {
                    Entity entity = elem.GetEntity(schema);
                    using (Transaction t = new Transaction(_doc, "Qex: Crear Grupo"))
                    {
                        t.Start();
                        //Get the field from schema
                        Field grField = schema.GetField("grupo");
                        entity.Set<string>(grField, sb.ToString());
                        elem.SetEntity(entity);
                        t.Commit();
                    };
                    actualizado = true;
                }
                else
                {
                    actualizado = false;
                }
            }
            catch (Exception)
            {
                actualizado = false;
            }
            return actualizado;
        }

        /// <summary> Devuelve True si se pueden actualizar los Materiales en el ProjectInfo </summary>
        public static bool UpdateMateriales(List<QexMaterial> materiales)
        {
            bool actualizado = false;
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (QexMaterial mat in materiales)
                {
                    sb.AppendLine(mat.ToLine());
                }

                Element elem = _doc.GetElement(new ElementId(_projInfoId));
                Schema schema = GetSchemaByName(_schemaMateriales);
                if (elem.GetEntity(schema).IsValid())
                {
                    Entity entity = elem.GetEntity(schema);
                    using (Transaction t = new Transaction(_doc, "Qex: Actualizar Materiales"))
                    {
                        t.Start();
                        //Get the field from schema
                        Field matField = schema.GetField("materiales");
                        entity.Set<string>(matField, sb.ToString());
                        elem.SetEntity(entity);
                        t.Commit();
                    };
                    actualizado = true;
                }
                else
                {
                    actualizado = false;
                }
            }
            catch (Exception)
            {
                actualizado = false;
            }
            return actualizado;
        }

        /// <summary> Devuelve True si se pueden actualizar los Recursos de un Elemento </summary>
        public static bool UpdateRecursos(Element elem, List<QexRecurso> lst)
        {
            bool actualizado = false;
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (QexRecurso rec in lst)
                {
                    sb.AppendLine(rec.ToLine());
                }

                Schema schema = GetSchemaByName(_schemaRecursos);
                if (elem.GetEntity(schema).IsValid())
                {
                    Entity entity = elem.GetEntity(schema);
                    using (Transaction t = new Transaction(_doc, "Qex: Actualizar Recursos"))
                    {
                        t.Start();
                        //Get the field from schema
                        Field recField = schema.GetField("recursos");
                        entity.Set<string>(recField, sb.ToString());
                        elem.SetEntity(entity);
                        t.Commit();
                    };
                    actualizado = true;
                }
                else
                {
                    actualizado = false;
                }
            }
            catch (Exception)
            {
                actualizado = false;
            }
            return actualizado;
        }
        #endregion
    }
}
