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

using System.Windows.Forms;
using Novacode;
using ClosedXML.Excel;
using System.Data;
using Autodesk.Revit.DB;
using System.Text.RegularExpressions;
#endregion

namespace Qex
{
    /// <summary> Esta Clase abstracta reune Métodos para crear Informes en Qex </summary>
    public abstract class Informe
    {
        /// <summary> Crea una Lista de Strings con las Quantificaciones para crear un archivo de Word. Las Categorias tienen un prefijo ### para poder darles formato luego </summary>
        public static List<string> CrearListaParaWord(List<Quantity> lstQuantity, string folder, bool tipos)
        {
            List<string> lstInforme = new List<string>();
            List<string> lstCategories = new List<string>();

            // Fill Folder List
            foreach (Quantity q in lstQuantity)
            {
                if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                {
                    if (!lstCategories.Exists(x => x == q.category))
                    {
                        lstCategories.Add(q.category);
                    }
                }
                if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                {
                    if (q.phaseCreated == folder && !lstCategories.Exists(x => x == q.category))
                    {
                        lstCategories.Add(q.category);
                    }
                }
                if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                {
                    ElementId lvlId = new ElementId(Convert.ToInt32(folder));
                    if (q.LevelId == lvlId.IntegerValue && !lstCategories.Exists(x => x == q.category))
                    {
                        lstCategories.Add(q.category);
                    }
                }
                if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                {
                    string[] cadena = Regex.Split(folder, " // ");
                    string phase = cadena[0];
                    ElementId lvlId = new ElementId(Convert.ToInt32(cadena[1]));
                    if (q.phaseCreated == phase && q.LevelId == lvlId.IntegerValue && 
                        !lstCategories.Exists(x => x == q.category))
                    {
                        lstCategories.Add(q.category);
                    }
                }
            }
            foreach (string s in lstCategories)
            {
                //Agregar el nombre de la Categoría
                lstInforme.Add("###" + s);
                //Agregar nombres de Quantities
                if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                {
                    foreach (Quantity q in lstQuantity.FindAll(x => x.category == s))
                    {
                        if (tipos)
                        {
                            lstInforme.Add(q.name + "\t" + q.medicion + "\t" +
                                String.Format("{0:C}", q.totalCost) + "\t" + q.qId);
                        }
                        else
                        {
                            lstInforme.Add(q.descripcion + "\t" + q.medicion + "\t" +
                                String.Format("{0:C}", q.totalCost) + "\t" + q.qId);
                        }
                    }
                }
                if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                {
                    foreach (Quantity q in lstQuantity.FindAll(x => x.phaseCreated == folder && x.category == s))
                    {
                        if (tipos)
                        {
                            lstInforme.Add(q.name + "\t" + q.medicion + "\t" +
                                String.Format("{0:C}", q.totalCost) + "\t" + q.qId);
                        }
                        else
                        {
                            lstInforme.Add(q.descripcion + "\t" + q.medicion + "\t" +
                                String.Format("{0:C}", q.totalCost) + "\t" + q.qId);
                        }
                    }
                }
                if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                {
                    ElementId lvlId = new ElementId(Convert.ToInt32(folder));
                    foreach (Quantity q in lstQuantity.FindAll(x => x.LevelId == lvlId.IntegerValue && x.category == s))
                    {
                        if (tipos)
                        {
                            lstInforme.Add(q.name + "\t" + q.medicion + "\t" +
                                String.Format("{0:C}", q.totalCost) + "\t" + q.qId);
                        }
                        else
                        {
                            lstInforme.Add(q.descripcion + "\t" + q.medicion + "\t" +
                                String.Format("{0:C}", q.totalCost) + "\t" + q.qId);
                        }
                    }
                }
                if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                {
                    string[] cadena = Regex.Split(folder, " // ");
                    string phase = cadena[0];
                    ElementId lvlId = new ElementId(Convert.ToInt32(cadena[1]));
                    foreach (Quantity q in lstQuantity.FindAll(x => x.phaseCreated == phase && 
                    x.LevelId == lvlId.IntegerValue && x.category == s))
                    {
                        if (tipos)
                        {
                            lstInforme.Add(q.name + "\t" + q.medicion + "\t" +
                                String.Format("{0:C}", q.totalCost) + "\t" + q.qId);
                        }
                        else
                        {
                            lstInforme.Add(q.descripcion + "\t" + q.medicion + "\t" +
                                String.Format("{0:C}", q.totalCost) + "\t" + q.qId);
                        }
                    }
                }
            }
            return lstInforme;
        }

        /// <summary> Crea una Lista de Strings con las Quantificaciones para crear un archivo de Excel. Las Categorias tienen un prefijo ### para poder darles formato luego </summary>
        public static List<string> CrearListaParaExcel(List<Quantity> lstQuantity, string folder, bool tipos)
        {
            List<string> lstInforme = new List<string>();
            List<string> lstCategories = new List<string>();

            foreach (Quantity q in lstQuantity)
            {
                if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                {
                    if (!lstCategories.Exists(x => x == q.category))
                    {
                        lstCategories.Add(q.category);
                    }
                }
                if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                {
                    if (q.phaseCreated == folder && !lstCategories.Exists(x => x == q.category))
                    {
                        lstCategories.Add(q.category);
                    }
                }
                if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                {
                    ElementId lvlId = new ElementId(Convert.ToInt32(folder));
                    if (q.LevelId == lvlId.IntegerValue && !lstCategories.Exists(x => x == q.category))
                    {
                        lstCategories.Add(q.category);
                    }
                }
                if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                {
                    string[] cadena = Regex.Split(folder, " // ");
                    string phase = cadena[0];
                    ElementId lvlId = new ElementId(Convert.ToInt32(cadena[1]));
                    if (q.phaseCreated == phase && q.LevelId == lvlId.IntegerValue && !lstCategories.Exists(x => x == q.category))
                    {
                        lstCategories.Add(q.category);
                    }
                }
            }
            foreach (string s in lstCategories)
            {
                //Agregar el nombre de la Categoría
                lstInforme.Add("###" + s);
                //Agregar nombres de Quantities
                if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                {
                    foreach (Quantity q in lstQuantity.FindAll(x => x.category == s))
                    {
                        double medicion = Tools.CalculateMedicionSinUnidad(q.count, q.length, q.area, q.volume, 
                            q.matriz, q.value);
                        string unidad = Tools.CalculateUnidad(q.matriz, q.unidad);
                        if (tipos)
                        {
                            lstInforme.Add(q.name + "\t" + medicion + "\t" + unidad + "\t" + q.totalCost.ToString() + "\t" + q.qId);
                        }
                        else
                        {
                            lstInforme.Add(q.descripcion + "\t" + medicion + "\t" + unidad + "\t" + q.totalCost.ToString() + "\t" + q.qId);
                        }
                    }
                }
                if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                {
                    foreach (Quantity q in lstQuantity.FindAll(x => x.phaseCreated == folder && x.category == s))
                    {
                        double medicion = Tools.CalculateMedicionSinUnidad(q.count, q.length, q.area, q.volume, 
                            q.matriz, q.value);
                        string unidad = Tools.CalculateUnidad(q.matriz, q.unidad);
                        if (tipos)
                        {
                            lstInforme.Add(q.name + "\t" + medicion + "\t" + unidad + "\t" + q.totalCost.ToString() + "\t" + q.qId);
                        }
                        else
                        {
                            lstInforme.Add(q.descripcion + "\t" + medicion + "\t" + unidad + "\t" + q.totalCost.ToString() + "\t" + q.qId);
                        }
                    }
                }
                if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                {
                    ElementId lvlId = new ElementId(Convert.ToInt32(folder));
                    foreach (Quantity q in lstQuantity.FindAll(x => x.LevelId == lvlId.IntegerValue && x.category == s))
                    {
                        double medicion = Tools.CalculateMedicionSinUnidad(q.count, q.length, q.area, q.volume, 
                            q.matriz, q.value);
                        string unidad = Tools.CalculateUnidad(q.matriz, q.unidad);
                        if (tipos)
                        {
                            lstInforme.Add(q.name + "\t" + medicion + "\t" + unidad + "\t" + q.totalCost.ToString() + "\t" + q.qId);
                        }
                        else
                        {
                            lstInforme.Add(q.descripcion + "\t" + medicion + "\t" + unidad + "\t" + q.totalCost.ToString() + "\t" + q.qId);
                        }
                    }
                }
                if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                {
                    string[] cadena = Regex.Split(folder, " // ");
                    string phase = cadena[0];
                    ElementId lvlId = new ElementId(Convert.ToInt32(cadena[1]));
                    foreach (Quantity q in lstQuantity.FindAll(x => x.phaseCreated == phase &&
                     x.LevelId == lvlId.IntegerValue && x.category == s))
                    {
                        double medicion = Tools.CalculateMedicionSinUnidad(q.count, q.length, q.area, q.volume, 
                            q.matriz, q.value);
                        string unidad = Tools.CalculateUnidad(q.matriz, q.unidad);
                        if (tipos)
                        {
                            lstInforme.Add(q.name + "\t" + medicion + "\t" + unidad + "\t" + q.totalCost.ToString() + "\t" + q.qId);
                        }
                        else
                        {
                            lstInforme.Add(q.descripcion + "\t" + medicion + "\t" + unidad + "\t" + q.totalCost.ToString() + "\t" + q.qId);
                        }
                    }
                }
            }
            return lstInforme;
        }

        /// <summary> Crea un documento de Word a partir de la Lista Informe </summary>
        public static void CrearWord(string title_, List<string> lstInforme, string folderText, string fileName)
        {
            // A formatting object for our title:
            var titleFormat = new Formatting();
            titleFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            titleFormat.Size = 18D;
            titleFormat.Bold = true;
            titleFormat.Position = 12;
            // A formatting object for our Category:
            var categoryFormat = new Formatting();
            categoryFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            categoryFormat.Size = 12D;
            categoryFormat.Bold = true;
            categoryFormat.Position = 12;
            // A formatting object for our normal paragraph text:
            var paraFormat = new Formatting();
            paraFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            paraFormat.Size = 10D;
            // Create the document in memory:
            var doc = DocX.Create(fileName);
            //Insert Title
            doc.InsertParagraph(title_, false, titleFormat);
            doc.InsertParagraph(folderText, false, titleFormat);
            
            // Create Table
            Table t = doc.AddTable(lstInforme.Count + 1, 4);
            int row = 1;
            //t.Design = TableDesign.ColorfulShadingAccent2;
            t.SetColumnWidth(0, 1000);
            t.SetColumnWidth(1, 5000);
            t.SetColumnWidth(2, 1500);
            t.SetColumnWidth(3, 1500);

            t.Rows[0].Cells[0].Paragraphs[0].InsertText("Imagen");
            t.Rows[0].Cells[1].Paragraphs[0].InsertText("Descripción");
            t.Rows[0].Cells[2].Paragraphs[0].InsertText("Cómputo");
            t.Rows[0].Cells[3].Paragraphs[0].InsertText("Costo total");

            foreach (string s in lstInforme)
            {
                if (s.Contains("###"))
                {
                    string ss = s.Remove(0, 3);
                    t.Rows[row].Cells[1].Paragraphs[0].InsertText(ss, false, categoryFormat);
                    row++;
                }
                else
                {
                    string[] array = s.Split('\t');
                    for (int i = 0; i < array.Count() - 1; i++)
                    {
                        t.Rows[row].Cells[i+1].Paragraphs[0].InsertText(array[i], false, paraFormat);
                    }
                    Quantity q = RevitQex.lstElements.Find(x => x.qId == array[3]);
                    string name0 = Tools.GetCleanedName(q.name);

                    try
                    {
                        q.image.Save(System.IO.Path.Combine(App.AppDirectory, "Temp", q.category + " - " + name0 + ".jpg"),
                                    System.Drawing.Imaging.ImageFormat.Jpeg);
                        Novacode.Image img = doc.AddImage(System.IO.Path.Combine(App.AppDirectory, "Temp",
                            q.category + " - " + name0 + ".jpg"));
                        Picture pic = img.CreatePicture(32, 32);

                        t.Rows[row].Cells[0].Paragraphs[0].InsertPicture(pic, 0);
                        //q.image.Save(System.IO.Path.Combine(App.AppDirectory, "Temp", q.category + " - " + title + ".jpg"), 
                        //    System.Drawing.Imaging.ImageFormat.Jpeg);
                        //var image = System.Drawing.Image.FromFile(System.IO.Path.Combine(App.AppDirectory, "Temp", 
                        //    q.category + " - " + title + ".jpg"));
                        //using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        //{
                        //    image.Save(ms, image.RawFormat);
                        //    ms.Seek(0, System.IO.SeekOrigin.Begin);
                        //    Novacode.Image img = doc.AddImage(ms);
                        //    Picture pic = img.CreatePicture(32, 32);

                        //    t.Rows[row].Cells[0].Paragraphs[0].InsertPicture(pic, 0);
                        //}
                    }
                    catch (Exception)
                    {
                        t.Rows[row].Cells[0].Paragraphs[0].InsertText("Error");
                    }
                    row++;
                }
            }
            doc.InsertParagraph().InsertTableBeforeSelf(t);

            doc.Save();
        }

        /// <summary> Crea un documento de Wpord a partir del treeView de la Pantalla Confeccionar Computos </summary>
        public static void CrearWordComputo(TreeView tree, string fileName, bool verDescripcion)
        {
            // A formatting object for our title:
            var titleFormat = new Formatting();
            titleFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            titleFormat.Size = 18D;
            titleFormat.Bold = true;
            titleFormat.Position = 12;
            // A formatting object for our Category:
            var categoryFormat = new Formatting();
            categoryFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            categoryFormat.Size = 12D;
            categoryFormat.Bold = true;
            categoryFormat.Position = 12;
            // A formatting object for our normal paragraph text:
            var paraFormat = new Formatting();
            paraFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            paraFormat.Size = 10D;
            // Create the document in memory:
            var doc = DocX.Create(fileName);
            //Insert Title
            string title = Tools._doc.Title;
            doc.InsertParagraph(title, false, titleFormat);

            foreach (TreeNode node0 in tree.Nodes)
            {
                if (node0.Name != "AA")
                {
                    doc.InsertParagraph(node0.Text, false, titleFormat);

                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        doc.InsertParagraph(node1.Text, false, categoryFormat);
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            Quantity q = RevitQex.lstElements.Find(x => x.qId == node2.Name);
                            string ss = "";
                            double medicion = Tools.CalculateMedicionSinUnidad(q.count, q.length, q.area, 
                                q.volume, q.matriz, q.value);
                            string unidad = Tools.CalculateUnidad(q.matriz, q.unidad);
                            if (verDescripcion)
                            {
                                ss = q.descripcion + "\t" + medicion + "\t" + unidad + "\t" + q.totalCost.ToString();
                                doc.InsertParagraph(ss, false, paraFormat);
                            }
                            if (!verDescripcion)
                            {
                                ss = q.name + "\t" + medicion + "\t" + unidad + "\t" + q.totalCost.ToString();
                                doc.InsertParagraph(ss, false, paraFormat);
                            }
                        }
                    }
                }
            }
            doc.Save();
        }

        /// <summary> Crea un documento de Wpord a partir del treeView de la Pantalla Confeccionar Computos </summary>
        public static void CrearWordComputoImages(TreeView tree, string fileName, bool verDescripcion)
        {
            // A formatting object for our title:
            var titleFormat = new Formatting();
            titleFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            titleFormat.Size = 18D;
            titleFormat.Bold = true;
            titleFormat.Position = 12;
            // A formatting object for our Category:
            var categoryFormat = new Formatting();
            categoryFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            categoryFormat.Size = 12D;
            categoryFormat.Bold = true;
            categoryFormat.Position = 12;
            // A formatting object for our normal paragraph text:
            var paraFormat = new Formatting();
            paraFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            paraFormat.Size = 10D;
            // Create the document in memory:
            var doc = DocX.Create(fileName);
            //Insert Title
            string title = Tools._doc.Title;
            doc.InsertParagraph(title, false, titleFormat);

            foreach (TreeNode node0 in tree.Nodes)
            {
                if (node0.Name != "AA")
                {
                    // Create Table
                    int branches = Tools.TotalNodes(node0);
                    Table t = doc.AddTable(branches + 1, 4);
                    int row = 1;
                    t.SetColumnWidth(0, 1000);
                    t.SetColumnWidth(1, 5000);
                    t.SetColumnWidth(2, 1500);
                    t.SetColumnWidth(3, 1500);

                    t.Rows[0].Cells[0].Paragraphs[0].InsertText("Imagen");
                    t.Rows[0].Cells[1].Paragraphs[0].InsertText("Descripción");
                    t.Rows[0].Cells[2].Paragraphs[0].InsertText("Cómputo");
                    t.Rows[0].Cells[3].Paragraphs[0].InsertText("Costo total");

                    doc.InsertParagraph(node0.Text, false, titleFormat);

                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        t.Rows[row].Cells[1].Paragraphs[0].InsertText(node1.Text, false, categoryFormat);
                        row++;
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            Quantity q = RevitQex.lstElements.Find(x => x.qId == node2.Name);
                            string ss = "";
                            double medicion = Tools.CalculateMedicionSinUnidad(q.count, q.length, q.area,
                                q.volume, q.matriz, q.value);
                            string unidad = Tools.CalculateUnidad(q.matriz, q.unidad);
                            if (verDescripcion)
                            {
                                t.Rows[row].Cells[1].Paragraphs[0].InsertText(q.descripcion, false, paraFormat);
                            }
                            if (!verDescripcion)
                            {
                                t.Rows[row].Cells[1].Paragraphs[0].InsertText(q.name, false, paraFormat);
                            }
                            t.Rows[row].Cells[2].Paragraphs[0].InsertText(medicion.ToString() + " " + unidad, false, paraFormat);
                            t.Rows[row].Cells[3].Paragraphs[0].InsertText(q.totalCost.ToString(), false, paraFormat);

                            // Images
                            string name0 = Tools.GetCleanedName(q.name);

                            try
                            {
                                q.image.Save(System.IO.Path.Combine(App.AppDirectory, "Temp", q.category + " - " + name0 + ".jpg"),
                                    System.Drawing.Imaging.ImageFormat.Jpeg);
                                Novacode.Image img = doc.AddImage(System.IO.Path.Combine(App.AppDirectory, "Temp",
                                    q.category + " - " + name0 + ".jpg"));
                                Picture pic = img.CreatePicture(32, 32);

                                t.Rows[row].Cells[0].Paragraphs[0].InsertPicture(pic, 0);

                                //using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                                //{
                                //    image.Save(ms, image.RawFormat);
                                //    ms.Seek(0, System.IO.SeekOrigin.Begin);
                                //    Novacode.Image img = doc.AddImage(ms);
                                //    Picture pic = img.CreatePicture(32, 32);

                                //    t.Rows[row].Cells[0].Paragraphs[0].InsertPicture(pic, 0);
                                //}
                            }
                            catch (Exception)
                            {
                                t.Rows[row].Cells[0].Paragraphs[0].InsertText("Error");
                            }

                            row++;
                        }
                    }
                    doc.InsertParagraph().InsertTableBeforeSelf(t);
                }
            }
            
            doc.Save();
        }

        /// <summary> Crea un documento de Excel a partir de la Lista Informe </summary>
        public static void CrearExcel(string title, List<string> lstInforme, string faseText, string fileName)
        {
            //Creating DataTable
            DataTable dt = new DataTable();
            //Adding the Columns
            dt.Columns.Add("Family and Type");
            dt.Columns.Add("Quantification");
            dt.Columns.Add("Unit");
            dt.Columns.Add("Total Cost");
            foreach (string s in lstInforme)
                dt.Rows.Add();
            
            //Exporting to Excel
            XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(dt, faseText);
            var ws = wb.Worksheet(1);

            int row = 2;
            foreach (string s in lstInforme)
            {
                if (s.Contains("###"))
                {
                    string ss = s.Remove(0, 3);
                    ws.Cell(row, 1).Value = ss;
                    ws.Cell(row, 1).Style.Font.Bold = true;
                    row += 1;
                }
                else
                {
                    string[] array = s.Split('\t');
                    for (int i = 0; i < array.Count(); i++)
                    {
                        ws.Cell(row, i + 1).Value = array[i];
                    }
                    row += 1;
                }
            }
            wb.SaveAs(fileName);
        }

        /// <summary> Crea un documento de Excel a partir de la Lista Informe </summary>
        public static void CrearExcelImages(string title_, List<string> lstInforme, string faseText, string fileName)
        {
            //Creating DataTable
            DataTable dt = new DataTable();
            //Adding the Columns
            dt.Columns.Add("Imagen");
            dt.Columns.Add("Family and Type");
            dt.Columns.Add("Quantification");
            dt.Columns.Add("Unit");
            dt.Columns.Add("Total Cost");
            foreach (string s in lstInforme)
                dt.Rows.Add();

            //Exporting to Excel
            XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(dt, faseText);
            var ws = wb.Worksheet(1);

            ws.Column(2).Width = 60;
            ws.Column(1).Width = 6;

            int row = 2;
            foreach (string s in lstInforme)
            {
                if (s.Contains("###"))
                {
                    string ss = s.Remove(0, 3);
                    ws.Cell(row, 2).Value = ss;
                    ws.Cell(row, 2).Style.Font.Bold = true;
                    ws.Cell(row, 2).Style.Font.FontSize = 14;
                    ws.Row(row).Height = 30;
                    row += 1;
                }
                else
                {
                    string[] array = s.Split('\t');
                    for (int i = 0; i < array.Count() -1; i++)
                    {
                        ws.Cell(row, i + 2).Value = array[i];
                    }
                    ws.Row(row).Height = 30;
                    // Get the Quantity and get the image
                    
                    try
                    {
                        Quantity q = RevitQex.lstElements.Find(x => x.qId == array[4]);
                        string title = q.name.Replace(":", "-");
                        title = title.Replace("/", "-");
                        title = title.Replace(" >> ", "-");
                        title = title.Replace('"', ' ');
                        title = title.Replace('?', ' ');
                        title = title.Replace('|', '-');
                        title = title.Replace('*', ' ');
                        title = title.Replace("'", "-");
                        q.image.Save(System.IO.Path.Combine(App.AppDirectory, "Temp", title + ".jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                        var image = ws.AddPicture(System.IO.Path.Combine(App.AppDirectory, "Temp", title + ".jpg")).MoveTo(ws.Cell(row, 1));
                        image.Height = 46;
                        image.Width = 46;
                    }
                    catch (Exception)
                    {
                        ws.Cell(row, 1).Value = "Error";
                    }

                    // Formatting Cells
                    ws.Cell(row, 2).Style.Alignment.WrapText = true;
                    ws.Cell(row, 3).Style.NumberFormat.Format = "0.00";
                    ws.Cell(row, 5).Style.NumberFormat.Format = "0.00";
                    row += 1;
                }
            }
            wb.SaveAs(fileName);
        }

        /// <summary> Crea un Documento de Excel a partir del TreeView de la pantalla Confeccionar Computos </summary>
        public static void CrearExcelComputo(TreeView tree, string fileName, bool verDescripcion)
        {
            //Exporting to Excel
            XLWorkbook wb = new XLWorkbook();
            int page = 1;

            foreach (TreeNode node0 in tree.Nodes)
            {
                if (node0.Name != "AA")
                {
                    //Creating DataTable
                    DataTable dt = new DataTable();
                    //Adding the Columns
                    if (verDescripcion)
                    {
                        dt.Columns.Add("Descripción");
                    }
                    if (!verDescripcion)
                    {
                        dt.Columns.Add("Familia y Tipo");
                    }

                    dt.Columns.Add("Quantificación");
                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Costo Total");

                    //dt.Rows.Add();
                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        dt.Rows.Add();
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            dt.Rows.Add();
                        }
                    }
                    wb.Worksheets.Add(dt, node0.Name);

                    var ws = wb.Worksheet(page);
                    page += 1;
                    int row = 2;



                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        ws.Cell(row, 1).Value = node1.Text;
                        ws.Cell(row, 1).Style.Font.Bold = true;
                        row += 1;
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            Quantity q = RevitQex.lstElements.Find(x => x.qId == node2.Name);
                            double medicion = Tools.CalculateMedicionSinUnidad(q.count, q.length, q.area, 
                                q.volume, q.matriz, q.value);
                            string unidad = Tools.CalculateUnidad(q.matriz, q.unidad);
                            if (verDescripcion)
                            {
                                ws.Cell(row, 1).Value = q.descripcion;
                                ws.Cell(row, 2).Value = medicion;
                                ws.Cell(row, 3).Value = unidad;
                                ws.Cell(row, 4).Value = q.totalCost.ToString();
                                row += 1;
                            }
                            if (!verDescripcion)
                            {
                                ws.Cell(row, 1).Value = q.name;
                                ws.Cell(row, 2).Value = medicion;
                                ws.Cell(row, 3).Value = unidad;
                                ws.Cell(row, 4).Value = q.totalCost.ToString();
                                row += 1;
                            }
                        }
                    }
                }
            }
            wb.SaveAs(fileName);
        }

        /// <summary> Crea un Documento de Excel a partir del TreeView de la pantalla Confeccionar Computos </summary>
        public static void CrearExcelComputoImages(TreeView tree, string fileName, bool verDescripcion)
        {
            //Exporting to Excel
            XLWorkbook wb = new XLWorkbook();
            int page = 1;

            foreach (TreeNode node0 in tree.Nodes)
            {
                if (node0.Name != "AA")
                {
                    //Creating DataTable
                    DataTable dt = new DataTable();
                    //Adding the Columns
                    dt.Columns.Add("Imagen");
                    if (verDescripcion)
                    {
                        dt.Columns.Add("Descripción");
                    }
                    if (!verDescripcion)
                    {
                        dt.Columns.Add("Familia y Tipo");
                    }

                    dt.Columns.Add("Quantificación");
                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Costo Total");

                    //dt.Rows.Add();
                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        dt.Rows.Add();
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            dt.Rows.Add();
                        }
                    }
                    wb.Worksheets.Add(dt, node0.Name);

                    var ws = wb.Worksheet(page);
                    page += 1;
                    int row = 2;

                    ws.Column(2).Width = 60;
                    ws.Column(1).Width = 6;

                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        ws.Cell(row, 2).Value = node1.Text;
                        ws.Cell(row, 2).Style.Font.Bold = true;
                        ws.Cell(row, 2).Style.Font.FontSize = 14;
                        ws.Row(row).Height = 30;
                        row += 1;
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            Quantity q = RevitQex.lstElements.Find(x => x.qId == node2.Name);
                            double medicion = Tools.CalculateMedicionSinUnidad(q.count, q.length, q.area,
                                q.volume, q.matriz, q.value);
                            string unidad = Tools.CalculateUnidad(q.matriz, q.unidad);
                            ws.Row(row).Height = 30;
                            if (verDescripcion)
                            {
                                ws.Cell(row, 2).Value = q.descripcion;
                                ws.Cell(row, 3).Value = medicion;
                                ws.Cell(row, 4).Value = unidad;
                                ws.Cell(row, 5).Value = q.totalCost.ToString();
                                // Add image
                                string title = q.name.Replace(":", "-");
                                title = title.Replace("/", "-");
                                title = title.Replace(" >> ", "-");
                                title = title.Replace('"', ' ');
                                title = title.Replace('?', ' ');
                                title = title.Replace('|', '-');
                                title = title.Replace('*', ' ');
                                title = title.Replace("'", "-");
                                q.image.Save(System.IO.Path.Combine(App.AppDirectory, "Temp", title + ".jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                var image = ws.AddPicture(System.IO.Path.Combine(App.AppDirectory, "Temp", title + ".jpg")).MoveTo(ws.Cell(row, 1));
                                image.Height = 46;
                                image.Width = 46;
                            }
                            if (!verDescripcion)
                            {
                                ws.Cell(row, 2).Value = q.name;
                                ws.Cell(row, 3).Value = medicion;
                                ws.Cell(row, 4).Value = unidad;
                                ws.Cell(row, 5).Value = q.totalCost.ToString();
                                // Add image
                                string title = q.name.Replace(":", "-");
                                title = title.Replace("/", "-");
                                title = title.Replace(" >> ", "-");
                                title = title.Replace('"', ' ');
                                title = title.Replace('?', ' ');
                                title = title.Replace('|', '-');
                                title = title.Replace('*', ' ');
                                title = title.Replace("'", "-");
                                q.image.Save(System.IO.Path.Combine(App.AppDirectory, "Temp", title + ".jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                var image = ws.AddPicture(System.IO.Path.Combine(App.AppDirectory, "Temp", title + ".jpg")).MoveTo(ws.Cell(row, 1));
                                image.Height = 46;
                                image.Width = 46;
                            }
                            // Formatting Cells
                            ws.Cell(row, 2).Style.Alignment.WrapText = true;
                            ws.Cell(row, 3).Style.NumberFormat.Format = "0.00";
                            ws.Cell(row, 5).Style.NumberFormat.Format = "0.00";
                            row += 1;

                        }
                    }
                }
            }
            wb.SaveAs(fileName);
        }

        /// <summary> Crea un Documento de Excel a partir del TreeView de la pantalla Asignar Recursos </summary>
        public static void CrearExcelRecursos(TreeView tree, string fileName)
        {
            //Exporting to Excel
            XLWorkbook wb = new XLWorkbook();
            int page = 1;

            foreach (TreeNode node0 in tree.Nodes)
            {
                if (node0.Name != "AA")
                {
                    //Creating DataTable
                    DataTable dt = new DataTable();
                    //Adding the Columns
                    dt.Columns.Add("Recurso");
                    dt.Columns.Add("Consumo");
                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Costo Unitario");
                    dt.Columns.Add("Costo Total");

                    //dt.Rows.Add();
                    List<Quantity> selectedQuantities = new List<Quantity>();
                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        //dt.Rows.Add();
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            Quantity q = RevitQex.lstElements.Find(x => x.qId == node2.Name);
                            selectedQuantities.Add(q);
                        }
                    }
                    // Get Recursos
                    List<ItemRecurso> items = Tools.GetItemRecursosFromQuantities(selectedQuantities);
                    foreach (ItemRecurso item in items)
                    {
                        dt.Rows.Add();
                    }

                    wb.Worksheets.Add(dt, node0.Name);

                    var ws = wb.Worksheet(page);
                    page += 1;
                    int row = 2;

                    foreach (ItemRecurso item in items)
                    {
                        ws.Cell(row, 1).Value = item.nombre;
                        ws.Cell(row, 2).Value = item.consumo;
                        ws.Cell(row, 3).Value = item.unidad;
                        ws.Cell(row, 4).Value = item.precioUnit;
                        ws.Cell(row, 5).Value = item.costoTotal;

                        
                        row += 1;
                    }
                    ws.Cell(row, 1).Style.Font.Bold = true;
                    ws.Cell(row, 1).Value = "TOTAL GENERAL";
                    ws.Cell(row, 5).Style.Font.Bold = true;
                    ws.Cell(row, 5).Value = Math.Round(items.Sum(x => x.costoTotal), 2);
                }
            }
            wb.SaveAs(fileName);
        }

        /// <summary> Crea un Documento de Excel a partir del TreeView de la pantalla Asignar Recursos, detallados por Grupos </summary>
        public static void CrearExcelRecursosDetallados(TreeView tree, string fileName, bool verDescripcion)
        {
            //Exporting to Excel
            XLWorkbook wb = new XLWorkbook();
            int page = 1;

            foreach (TreeNode node0 in tree.Nodes)
            {
                if (node0.Name != "AA")
                {
                    //Creating DataTable
                    DataTable dt = new DataTable();
                    //Adding the Columns
                    dt.Columns.Add("Recurso");
                    dt.Columns.Add("Consumo");
                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Costo Unitario");
                    dt.Columns.Add("Costo Total");

                    //dt.Rows.Add();
                    List<Quantity> selectedQuantities = new List<Quantity>();
                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        //dt.Rows.Add();
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            Quantity q = RevitQex.lstElements.Find(x => x.qId == node2.Name);
                            selectedQuantities.Add(q);
                        }
                    }
                    // Get Recursos
                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        //dt.Rows.Add();
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            Quantity q = RevitQex.lstElements.Find(x => x.qId == node2.Name);
                            dt.Rows.Add();
                            List<ItemRecurso> items = q.GetItemRecursos();
                            foreach (ItemRecurso item in items)
                            {
                                dt.Rows.Add();
                            }
                            dt.Rows.Add();
                            dt.Rows.Add();
                        }
                    }
                    dt.Rows.Add();

                    wb.Worksheets.Add(dt, node0.Name);

                    var ws = wb.Worksheet(page);
                    page += 1;
                    int row = 2;

                    double totalGeneral = 0;
                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        //dt.Rows.Add();
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            Quantity q = RevitQex.lstElements.Find(x => x.qId == node2.Name);
                            if (verDescripcion)
                            {
                                ws.Cell(row, 1).Value = q.descripcion;
                            }
                            else
                            {
                                ws.Cell(row, 1).Value = q.name;
                            }
                            List<ItemRecurso> items = q.GetItemRecursos();
                            ws.Cell(row, 1).Style.Font.Bold = true;
                            double total = 0;
                            row += 1;
                            foreach (ItemRecurso item in items)
                            {
                                ws.Cell(row, 1).Value = item.nombre;
                                double consumo = Math.Round(q.GetMedicion() * item.consumo, 2);
                                ws.Cell(row, 2).Value = consumo;
                                ws.Cell(row, 3).Value = item.unidad;
                                ws.Cell(row, 4).Value = item.precioUnit;
                                ws.Cell(row, 5).Value = Math.Round(consumo * item.precioUnit, 2);
                                total += Math.Round(consumo * item.precioUnit, 2);
                                row += 1;
                            }
                            ws.Cell(row, 5).Style.Font.Bold = true;
                            ws.Cell(row, 5).Value = total;
                            totalGeneral += total;
                            row += 1;
                            row += 1;
                        }
                    }
                    ws.Cell(row, 1).Style.Font.Bold = true;
                    ws.Cell(row, 1).Value = "TOTAL GENERAL";
                    ws.Cell(row, 5).Style.Font.Bold = true;
                    ws.Cell(row, 5).Value = totalGeneral;
                }
            }
            wb.SaveAs(fileName);
        }

        /// <summary> Crea un Documento de Excel conteniendo los consumos de recursos de Quantificaciones </summary>
        public static void CrearExcelRecursosAsignados(TreeView tree, string fileName)
        {
            //Exporting to Excel
            XLWorkbook wb = new XLWorkbook();
            int page = 1;

            foreach (TreeNode node0 in tree.Nodes)
            {
                if (node0.Name != "AA")
                {
                    //Creating DataTable
                    DataTable dt = new DataTable();
                    //Adding the Columns
                    dt.Columns.Add("Quantificacion");
                    dt.Columns.Add("Descripcion");
                    dt.Columns.Add("Unidad");
                    dt.Columns.Add("Recurso");
                    dt.Columns.Add("Consumo");
                    dt.Columns.Add("Unidad_");
                    dt.Columns.Add("Icono");

                    //dt.Rows.Add();
                    List<Quantity> selectedQuantities = new List<Quantity>();
                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        //dt.Rows.Add();
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            Quantity q = RevitQex.lstElements.Find(x => x.qId == node2.Name);
                            selectedQuantities.Add(q);
                        }
                    }
                    // Get Recursos
                    foreach (Quantity q in selectedQuantities)
                    {
                        List<QexRecurso> recursos = Tools.GetRecursosFromQuantity(q);
                        foreach (QexRecurso item in recursos)
                        {
                            dt.Rows.Add();
                        }
                    }

                    wb.Worksheets.Add(dt, node0.Name);

                    var ws = wb.Worksheet(page);
                    page += 1;
                    int row = 2;

                    foreach (Quantity q in selectedQuantities)
                    {
                        List<QexRecurso> recursos = Tools.GetRecursosFromQuantity(q);
                        foreach (QexRecurso rec in recursos)
                        {
                            QexMaterial mat = rec.GetMaterial();
                            ws.Cell(row, 1).Value = q.name;
                            ws.Cell(row, 2).Value = q.descripcion;
                            ws.Cell(row, 3).Value = q.GetUnidad();
                            ws.Cell(row, 4).Value = mat.nombre;
                            ws.Cell(row, 5).Value = rec.Consumo;
                            ws.Cell(row, 6).Value = mat.unidad;
                            ws.Cell(row, 7).Value = mat.index;
                            row += 1;
                        }
                    }
                    //ws.Cell(row, 1).Style.Font.Bold = true;
                    //ws.Cell(row, 1).Value = "TOTAL GENERAL";
                    //ws.Cell(row, 5).Style.Font.Bold = true;
                    //ws.Cell(row, 5).Value = Math.Round(items.Sum(x => x.costoTotal), 2);
                }
            }
            wb.SaveAs(fileName);
        }

        /// <summary> Crea un documento de Excel a partir de la Lista Informe </summary>
        public static void ExportarRecursosExcel(string title, List<QexMaterial> lstMateriales, string fileName)
        {
            //Creating DataTable
            DataTable dt = new DataTable();
            //Adding the Columns
            dt.Columns.Add("Grupo");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Precio");
            dt.Columns.Add("Unidad");
            dt.Columns.Add("Icono");
            foreach (var s in lstMateriales)
                dt.Rows.Add();

            //Exporting to Excel
            XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(dt, "Recursos");
            var ws = wb.Worksheet(1);

            int row = 2;
            foreach (var mat in lstMateriales)
            {
                string[] array = new string[5];
                array[0] = DalRecursos.GetGrupoMaterialById(mat.grupoId).Nombre;
                array[1] = mat.nombre;
                array[2] = mat.precio.ToString();
                array[3] = mat.unidad;
                array[4] = mat.index.ToString();
                for (int i = 0; i < array.Count(); i++)
                {
                    ws.Cell(row, i + 1).Value = array[i];
                }
                row += 1;
            }
            wb.SaveAs(fileName);
        }
    }
}
