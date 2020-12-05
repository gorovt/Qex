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

using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qex
{
    public partial class frmRecursosMain : System.Windows.Forms.Form
    {
        public static List<Quantity> _lstQuantity;
        public static Autodesk.Revit.DB.Document _doc;
        public static List<QexMaterial> _lstRecursos;
        public static bool _verDescripcion;
        public List<QexRecurso> _clipboardRecursos;

        public frmRecursosMain(Autodesk.Revit.DB.Document doc, List<Quantity> listaQuantity, bool verDescripcion)
        {
            InitializeComponent();
            _doc = doc;
            _lstQuantity = listaQuantity.OrderBy(x => x.libroOrden).ThenBy(x => x.grupoOrden).ThenBy(x => x.qOrden).ToList();
            _verDescripcion = verDescripcion;
            this.lstvRecursos.SmallImageList = Tools.GetImageListIcons();
            this.lstvRecursos.LargeImageList = Tools.GetImageListIcons();

            List<Quantity> lst = new List<Quantity>();
            lst = QexSchema.ReadComputos(_doc);
            foreach (Quantity qu in lst)
            {
                if (_lstQuantity.Exists(x => x.qId == qu.qId))
                {
                    Quantity q = _lstQuantity.Find(x => x.qId == qu.qId);
                    q.grupo = qu.grupo;
                    q.libro = qu.libro;
                }
            }

            rellenarTreeMultiple(_lstQuantity, _verDescripcion);
            treePintarRecursos(this.treeViewComputo);
        }

        private void rellenarTreeMultiple(List<Quantity> lstQ, bool verDescripcion)
        {
            this.treeViewComputo.ImageList = imageList1;
            fillTreeLibros(this.treeViewComputo, lstQ, verDescripcion);
            fillTreeRevit(this.treeViewComputo, lstQ, verDescripcion);
        }

        private void fillTreeLibros(TreeView tree, List<Quantity> lstQ, bool verDescripcion)
        {
            tree.ImageList = imageList1;
            foreach (Quantity q in lstQ)
            {
                //Q tiene Libro asignado? SI. Genial
                if (q.libro != "")
                {
                    //Existe el Libro en el Arbol? NO. Entonces agregarlo. Y agregar el GRUPO. Y agregar Q
                    if (!tree.Nodes.ContainsKey(q.libro))
                    {
                        tree.Nodes.Add(q.libro, q.libro, 0, 0);
                        tree.Nodes[q.libro].NodeFont = new Font(tree.Font, FontStyle.Underline);
                        //tree.Nodes[q.libro].Expand();
                        tree.Nodes[q.libro].Nodes.Add(q.grupo, q.grupo, 1, 1);
                        //tree.Nodes[q.libro].Nodes[q.grupo].ExpandAll();
                        if (verDescripcion)
                        {
                            tree.Nodes[q.libro].Nodes[q.grupo].Nodes.Add(q.qId, q.descripcion
                                + ": " + q.medicion, 2, 2);
                        }
                        if (!verDescripcion)
                        {
                            tree.Nodes[q.libro].Nodes[q.grupo].Nodes.Add(q.qId, q.name
                                + ": " + q.medicion, 2, 2);
                        }
                    }
                    //Existe el Libro en el Arbol? SI.
                    else
                    {
                        //Existe el GRUPO en el Arbol? NO. Entonces agregarlo. Y luego agregar Q
                        if (!tree.Nodes[q.libro].Nodes.ContainsKey(q.grupo))
                        {
                            tree.Nodes[q.libro].Nodes.Add(q.grupo, q.grupo, 1, 1);
                            //tree.Nodes[q.libro].Nodes[q.grupo].Expand();
                            if (verDescripcion)
                            {
                                tree.Nodes[q.libro].Nodes[q.grupo].Nodes.Add(q.qId, q.descripcion
                                    + ": " + q.medicion, 2, 2);
                            }
                            if (!verDescripcion)
                            {
                                tree.Nodes[q.libro].Nodes[q.grupo].Nodes.Add(q.qId, q.name
                                    + ": " + q.medicion, 2, 2);
                            }
                        }
                        //Existe el GRUPO en el Arbol? SI. Entonces agregar Q
                        else
                        {
                            if (verDescripcion)
                            {
                                tree.Nodes[q.libro].Nodes[q.grupo].Nodes.Add(q.qId, q.descripcion
                                    + ": " + q.medicion, 2, 2);
                            }
                            if (!verDescripcion)
                            {
                                tree.Nodes[q.libro].Nodes[q.grupo].Nodes.Add(q.qId, q.name
                                    + ": " + q.medicion, 2, 2);
                            }
                        }
                    }
                }
            }

            //No hay nada en el Arbol?
            if (tree.Nodes.Count == 0)
            {
                tree.Nodes.Add("Quantificación", "Quantificación", 0, 0);
                tree.Nodes["Quantificación"].Expand();
                tree.Nodes["Quantificación"].NodeFont = new Font(treeViewComputo.Font, FontStyle.Underline);
                tree.Nodes["Quantificación"].Nodes.Add("Grupo 1", "Grupo 1", 1, 1);
                tree.Nodes["Quantificación"].Nodes["Grupo 1"].Expand();
                tree.Nodes["Quantificación"].Nodes.Add("Grupo 2", "Grupo 2", 1, 1);
                tree.Nodes["Quantificación"].Nodes["Grupo 2"].Expand();
            }
            foreach (TreeNode node0 in tree.Nodes)
            {
                node0.Expand();
            }
        }
        private void fillBanderas(TreeView tree, Quantity q, string folder)
        {
            //Poner banderas verdes
            if (tree.Nodes["AA"].Nodes[folder].Nodes[q.category].Nodes.Count == 0)
            {
                tree.Nodes["AA"].Nodes[folder].Nodes[q.category].ImageIndex = 6;
                tree.Nodes["AA"].Nodes[folder].Nodes[q.category].SelectedImageIndex = 6;
            }
            if (tree.Nodes["AA"].Nodes[folder].Nodes[q.category].Nodes.Count != 0)
            {
                tree.Nodes["AA"].Nodes[folder].Nodes[q.category].ImageIndex = 7;
                tree.Nodes["AA"].Nodes[folder].Nodes[q.category].SelectedImageIndex = 7;
                #region Qex v2.2
                if (q.category.Contains("(Materiales)"))
                {
                    tree.Nodes["AA"].Nodes[folder].Nodes[q.category].ImageIndex = 8;
                    tree.Nodes["AA"].Nodes[folder].Nodes[q.category].SelectedImageIndex = 8;
                }
                if (q.category.Contains("(Pinturas)"))
                {
                    tree.Nodes["AA"].Nodes[folder].Nodes[q.category].ImageIndex = 8;
                    tree.Nodes["AA"].Nodes[folder].Nodes[q.category].SelectedImageIndex = 8;
                }
                if (q.category.Contains("(Demolido)"))
                {
                    tree.Nodes["AA"].Nodes[folder].Nodes[q.category].ImageIndex = 9;
                    tree.Nodes["AA"].Nodes[folder].Nodes[q.category].SelectedImageIndex = 9;
                }
                #endregion
            }
        }
        private void fillTreeRevit(TreeView tree, List<Quantity> lstQ, bool verDescripcion)
        {
            // Completar Arbol AA
            tree.Nodes.Add("AA", "Modelo Revit", 3, 3);
            tree.Nodes["AA"].Expand();
            tree.Nodes["AA"].NodeFont = new Font(tree.Font, FontStyle.Underline);
            //LEVEL 1 Folder
            List<Tview> lstLevel1 = new List<Tview>();

            foreach (Quantity q in lstQ)
            {
                string folder = "";
                string lvlName = "Sin Nivel";
                string folderText = "";

                if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                {
                    folder = "Categorias";
                    folderText = "Categorias";
                }
                if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                {
                    folder = q.phaseCreated;
                    folderText = q.phaseCreated;
                }
                if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                {
                    folder = q.LevelId.ToString();
                    ElementId lvlId = new ElementId(-1);
                    if (q.LevelId != -1)
                    {
                        Element lvl = _doc.GetElement(new ElementId(q.LevelId));
                        lvlName = lvl.Name;
                    }
                    folderText = lvlName;
                }
                if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                {
                    folder = q.phaseCreated + " // " + q.LevelId.ToString();
                    ElementId lvlId = new ElementId(-1);
                    if (q.LevelId != -1)
                    {
                        Element lvl = _doc.GetElement(new ElementId(q.LevelId));
                        lvlName = lvl.Name;
                    }
                    folderText = q.phaseCreated + " // " + lvlName;
                }

                //Existe la CARPETA en el Arbol AA? NO. Entonce agregarla. Luego agregar la CATEGORIA
                if (!tree.Nodes["AA"].Nodes.ContainsKey(folder))
                {
                    tree.Nodes["AA"].Nodes.Add(folder, folderText, 4, 4);
                    //tree.Nodes["AA"].Nodes[folder].Expand();
                    tree.Nodes["AA"].Nodes[folder].Nodes.Add(q.category, q.category, 6, 6);
                }
                //Existe la CARPETA en el Arbol AA? SI. ¿Y existe la CATEGORIA en la CARPETA? NO, Entonces agregarla
                if (tree.Nodes["AA"].Nodes.ContainsKey(folder))
                {
                    if (!tree.Nodes["AA"].Nodes[folder].Nodes.ContainsKey(q.category))
                    {
                        tree.Nodes["AA"].Nodes[folder].Nodes.Add(q.category, q.category, 7, 7);
                    }
                }
                //Q tiene LIBRO? NO. Entonces agregarlo al Arbol AA
                if (q.libro == "")
                {
                    if (verDescripcion)
                    {
                        tree.Nodes["AA"].Nodes[folder].Nodes[q.category].Nodes.Add(q.qId, q.descripcion +
                        ": " + q.medicion, 2, 2);
                    }
                    if (!verDescripcion)
                    {
                        tree.Nodes["AA"].Nodes[folder].Nodes[q.category].Nodes.Add(q.qId, q.name +
                        ": " + q.medicion, 2, 2);
                    }
                }
                fillBanderas(tree, q, folder);
            }
            tree.SelectedNode = tree.Nodes[0];
        }

        private void mnuBtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void recursosDeProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new frmSetMaterialDb()).ShowDialog();
        }


        private void rellenarTreeMultiple2(List<Quantity> lstQ, bool verDescripcion)
        {
            this.treeViewComputo.ImageList = imageList1;
            #region Completar Libros
            TreeNode node00 = new TreeNode();
            node00.Name = "Computos";
            node00.Text = "Computos";
            node00.ImageIndex = 11;
            node00.SelectedImageIndex = 12;

            foreach (Quantity q in lstQ)
            {
                //Q tiene Libro asignado? SI. Genial
                if (q.libro != "")
                {
                    //Existe el Libro en el Arbol? NO. Entonces agregarlo. Y agregar el GRUPO. Y agregar Q
                    if (!node00.Nodes.ContainsKey(q.libro))
                    {
                        node00.Nodes.Add(q.libro, q.libro, 0, 0);
                        node00.Nodes[q.libro].NodeFont = new Font(treeViewComputo.Font, FontStyle.Underline);
                        node00.Nodes[q.libro].ExpandAll();
                        node00.Nodes[q.libro].Nodes.Add(q.grupo, q.grupo, 1, 1);
                        node00.Nodes[q.libro].Nodes[q.grupo].ExpandAll();
                        if (verDescripcion)
                        {
                            node00.Nodes[q.libro].Nodes[q.grupo].Nodes.Add(q.qId, q.descripcion
                            + ": " + q.medicion, 2, 2);
                        }
                        if (!verDescripcion)
                        {
                            node00.Nodes[q.libro].Nodes[q.grupo].Nodes.Add(q.qId, q.name
                            + ": " + q.medicion, 2, 2);
                        }
                    }
                    //Existe el Libro en el Arbol? SI.
                    else
                    {
                        //Existe el GRUPO en el Arbol? NO. Entonces agregarlo. Y luego agregar Q
                        if (!node00.Nodes[q.libro].Nodes.ContainsKey(q.grupo))
                        {
                            node00.Nodes[q.libro].Nodes.Add(q.grupo, q.grupo, 1, 1);
                            node00.Nodes[q.libro].Nodes[q.grupo].ExpandAll();
                            if (verDescripcion)
                            {
                                node00.Nodes[q.libro].Nodes[q.grupo].Nodes.Add(q.qId, q.descripcion
                                + ": " + q.medicion, 2, 2);
                            }
                            if (!verDescripcion)
                            {
                                node00.Nodes[q.libro].Nodes[q.grupo].Nodes.Add(q.qId, q.name
                                + ": " + q.medicion, 2, 2);
                            }
                        }
                        //Existe el GRUPO en el Arbol? SI. Entonces agregar Q
                        else
                        {
                            if (verDescripcion)
                            {
                                node00.Nodes[q.libro].Nodes[q.grupo].Nodes.Add(q.qId, q.descripcion
                                + ": " + q.medicion, 2, 2);
                            }
                            if (!verDescripcion)
                            {
                                node00.Nodes[q.libro].Nodes[q.grupo].Nodes.Add(q.qId, q.name
                                + ": " + q.medicion, 2, 2);
                            }
                        }
                    }
                }
            }
            //Agregar Libros a Arbol
            this.treeViewComputo.Nodes.Add(node00);
            #endregion

            #region Completar Arbol AA
            this.treeViewComputo.Nodes.Add("AA", "Modelo de Revit", 3, 3);
            this.treeViewComputo.Nodes["AA"].ExpandAll();
            this.treeViewComputo.Nodes["AA"].NodeFont = new Font(treeViewComputo.Font, FontStyle.Underline);

            foreach (Quantity q in lstQ)
            {
                //Existe la FASE en el Arbol AA? NO. Entonce agregarla. Luego agregar la CATEGORIA
                if (!this.treeViewComputo.Nodes["AA"].Nodes.ContainsKey(q.phaseCreated))
                {
                    this.treeViewComputo.Nodes["AA"].Nodes.Add(q.phaseCreated, q.phaseCreated, 4, 4);
                    this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].ExpandAll();
                    this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes.Add(q.category, q.category, 6, 6);
                }
                //Existe la FASE en el Arbol AA? SI. ¿Y existe la CATEGORIA en la FASE? NO, Entonces agregarla
                if (this.treeViewComputo.Nodes["AA"].Nodes.ContainsKey(q.phaseCreated))
                {
                    if (!this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes.ContainsKey(q.category))
                    {
                        this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes.Add(q.category, q.category, 7, 7);
                    }
                }
                //Q tiene LIBRO? NO. Entonces agregarlo al Arbol AA
                if (q.libro == "")
                {
                    if (verDescripcion)
                    {
                        if (Tools.GetRecursosFromQuantity(q).Count == 0)
                            this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes[q.category].Nodes.Add(q.qId, q.descripcion +
                            ": " + q.medicion, 2, 2);
                        else
                            this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes[q.category].Nodes.Add(q.qId, q.descripcion +
                            ": " + q.medicion, 10, 10);
                    }
                    if (!verDescripcion)
                    {
                        if (Tools.GetRecursosFromQuantity(q).Count == 0)
                            this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes[q.category].Nodes.Add(q.qId, q.name +
                            ": " + q.medicion, 2, 2);
                        else
                            this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes[q.category].Nodes.Add(q.qId, q.name +
                            ": " + q.medicion, 10, 10);
                    }
                }
                //Poner banderas verdes
                if (this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes[q.category].Nodes.Count != 0)
                {
                    this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes[q.category].ImageIndex = 7;
                    this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes[q.category].SelectedImageIndex = 7;
                    #region Qex v2.2
                    if (q.category.Contains("(Materiales)"))
                    {
                        this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes[q.category].ImageIndex = 8;
                        this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes[q.category].SelectedImageIndex = 8;
                    }
                    if (q.category.Contains("Pinturas"))
                    {
                        this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes[q.category].ImageIndex = 8;
                        this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes[q.category].SelectedImageIndex = 8;
                    }
                    if (q.category.Contains("(Demoliciones)"))
                    {
                        this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes[q.category].ImageIndex = 9;
                        this.treeViewComputo.Nodes["AA"].Nodes[q.phaseCreated].Nodes[q.category].SelectedImageIndex = 9;
                    }
                    #endregion
                }
            }
            treePintarRecursos(this.treeViewComputo);
            this.treeViewComputo.SelectedNode = this.treeViewComputo.Nodes[0];
            #endregion
        }

        private void importGrupos()
        {
            List<Quantity> lstQimported = new List<Quantity>();
            List<Quantity> lstFinal = new List<Quantity>();
            //string tempPath = QexOpciones.pathComputos;
            //string tempFile = tempPath + RevitQex.docGuid + ".txt";
            //if (File.Exists(tempFile))
            //{
            //    string[] lines = File.ReadAllLines(tempFile);
            //    string[] lines2 = lines.Skip(1).ToArray();
            //    string boolDescripcion = "False";

            //    for (int i = 0; i < lines2.Length; i++)
            //    {
            //        string[] values = Regex.Split(lines2[i], ";");
            //        string qId = values[0];
            //        string grupo = values[7];
            //        string libro = values[8];
            //        boolDescripcion = values[9];
            //        if (lstQuantity.Exists(x => x.qId == qId))
            //        {
            //            Quantity q = lstQuantity.Find(x => x.qId == qId);
            //            q.grupo = grupo;
            //            q.libro = libro;
            //            lstQimported.Add(q);
            //        }
            //    }
            //    this.treeViewComputo.Nodes.Clear();
            //    if (boolDescripcion == "False")
            //        _verDescripcion = false;
            //    if (boolDescripcion == "True")
            //        _verDescripcion = true;
            //    lstFinal = lstQuantity.Except(lstQimported).ToList();
            //    foreach (Quantity q in lstFinal)
            //    {
            //        q.grupo = "";
            //        q.libro = "";
            //    }
            //    lstQimported.AddRange(lstFinal);
            //    rellenarTreeMultiple(lstQimported, _verDescripcion);
            //    foreach (TreeNode node0 in this.treeViewComputo.Nodes)
            //    {
            //        node0.Expand();
            //        foreach (TreeNode node1 in node0.Nodes)
            //        {
            //            node1.Expand();
            //        }
            //    }
            //}
        }

        private void treeViewComputo_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int level = e.Node.Level;
            if (level == 0) //Libro o Modelo Revit
            {
                if (e.Node.Name == "AA") //Modelo Revit
                {
                    List<Quantity> lst = new List<Quantity>();
                    foreach (TreeNode node0 in e.Node.Nodes)
                    {
                        foreach (TreeNode node1 in node0.Nodes)
                        {
                            foreach (TreeNode node2 in node1.Nodes)
                            {
                                string id = node2.Name;
                                Quantity q = _lstQuantity.Find(x => x.qId == id);
                                lst.Add(q);
                            }
                        }
                    }
                    fillListView(lst);
                }
                else //Libros
                {
                    List<Quantity> lst = new List<Quantity>();
                    foreach (TreeNode node1 in e.Node.Nodes)
                    {
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            string id = node2.Name;
                            Quantity q = _lstQuantity.Find(x => x.qId == id);
                            lst.Add(q);
                        }
                    }
                    fillListView(lst);
                }
            }
            if (level == 1) //Grupo o Fase del Modelo Revit
            {
                if (e.Node.Parent.Name == "AA") //FASE del Modelo Revit
                {
                    List<Quantity> lst = new List<Quantity>();
                    foreach (TreeNode node1 in e.Node.Nodes)
                    {
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            string id = node2.Name;
                            Quantity q = _lstQuantity.Find(x => x.qId == id);
                            lst.Add(q);
                        }
                    }
                    fillListView(lst);
                }
                else //Grupo
                {
                    List<Quantity> lst = new List<Quantity>();
                    foreach (TreeNode node2 in e.Node.Nodes)
                    {
                        string id = node2.Name;
                        Quantity q = _lstQuantity.Find(x => x.qId == id);
                        lst.Add(q);
                    }
                    fillListView(lst);
                }
            }
            if (level == 2) //Quantificacion o Categoria del Modelo Revit
            {
                if (e.Node.Parent.Parent.Name == "AA") //Categoria del Modelo Revit
                {
                    List<Quantity> lst = new List<Quantity>();
                    foreach (TreeNode node2 in e.Node.Nodes)
                    {
                        string id = node2.Name;
                        Quantity q = _lstQuantity.Find(x => x.qId == id);
                        lst.Add(q);
                    }
                    fillListView(lst);
                }
                else //Quantificación
                {
                    List<Quantity> lst = new List<Quantity>();
                    string id = e.Node.Name;
                    Quantity q = _lstQuantity.Find(x => x.qId == id);
                    lst.Add(q);
                    fillListView(lst);
                }
            }
            if (level == 3) //Quantificacion del Modelo Revit
            {
                if (e.Node.Parent.Parent.Parent.Name == "AA") //Quantificacion del Modelo Revit
                {
                    string id = e.Node.Name;
                    List<Quantity> lst = new List<Quantity>();
                    Quantity q = _lstQuantity.Find(x => x.qId == id);
                    lst.Add(q);
                    fillListView(lst);
                }
                //    else
                //    {
                //        string id = e.Node.Name;
                //        List<Quantity> lst = new List<Quantity>();
                //        Quantity q = RevitQex.lstElements.Find(x => x.qId == id);
                //        lst.Add(q);
                //        fillListView(lst);
                //    }
            }
        }
        private void fillListView(List<Quantity> lstQuantities)
        {
            double total = 0;
            this.lstvRecursos.Items.Clear();
            List<QexRecurso> lstConsumos = new List<QexRecurso>();
            foreach (Quantity qu in lstQuantities)
            {
                if (Tools.GetRecursosFromQuantity(qu).Count > 0)
                {
                    foreach (QexRecurso rec in Tools.GetRecursosFromQuantity(qu))
                    {
                        QexMaterial mat = rec.GetMaterial();
                        if (!lstConsumos.Exists(x => x.matId == mat.id))
                        {
                            double consumo = Math.Round(qu.GetMedicion() * rec.Consumo, 2);
                            rec.Consumo = consumo;
                            lstConsumos.Add(rec);
                        }
                        else
                        {
                            QexRecurso recurso = lstConsumos.Find(x => x.matId == mat.id);
                            double consumo = Math.Round(recurso.Consumo + (rec.Consumo * qu.GetMedicion()), 2);
                            recurso.Consumo = consumo;
                        }
                    }
                }
            }
            //Ordenar Consumos por materiales
            List<string[]> lstRecursos = new List<string[]>();
            if (lstConsumos.Count > 0)
            {
                foreach (QexRecurso rec in lstConsumos)
                {
                    QexMaterial mat = rec.GetMaterial();
                    string[] values = new string[5];
                    values[0] = mat.nombre;
                    values[1] = rec.Consumo.ToString("N2");
                    values[2] = mat.unidad;
                    values[3] = mat.index.ToString();
                    double subTotal = Math.Round(rec.Consumo * mat.precio, 2);
                    values[4] = subTotal.ToString("C2");
                    lstRecursos.Add(values);

                    // Total
                    total += subTotal;
                }
                lstRecursos = lstRecursos.OrderBy(x => x[0]).ToList();
            }
            if (lstRecursos.Count > 0)
            {
                foreach (string[] s in lstRecursos)
                {
                    ListViewItem itm = new ListViewItem(s);
                    itm.ImageIndex = Convert.ToInt32(s[3]);
                    this.lstvRecursos.Items.Add(itm);
                }
            }
            this.txtCosto.Text = total.ToString("C2");
        }

        private void treeViewComputo_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeViewComputo.SelectedNode = e.Node;
                int level = this.treeViewComputo.SelectedNode.Level;

                if (level == 2 && e.Node.Parent.Parent.Name != "AA")
                {
                    mnuTreeItem.Show(Cursor.Position);
                }
                if (level == 3)
                {
                    mnuTreeItem.Show(Cursor.Position);
                }
            }
        }

        private void mnuItemVerRecursos_Click(object sender, EventArgs e)
        {
            string qId = this.treeViewComputo.SelectedNode.Name;
            Quantity q = _lstQuantity.Find(x => x.qId == qId);
            (new frmRecursosMaterial(Tools._doc, q)).ShowDialog();
            this.lstvRecursos.Items.Clear();
            List<Quantity> lst = new List<Quantity>();
            lst.Add(q);
            fillListView(lst);
            treePintarRecursos(this.treeViewComputo);
        }
        private void copyToClipboard(Quantity q)
        {
            List<QexRecurso> lstRecursos = new List<QexRecurso>();
            foreach (QexRecurso rec in Tools.GetRecursosFromQuantity(q))
            {
                lstRecursos.Add(rec);
            }
            _clipboardRecursos = lstRecursos.ToList();
        }
        private bool pasteFromClipboard(Quantity q)
        {
            Autodesk.Revit.DB.Element elem = Tools._doc.GetElement(new Autodesk.Revit.DB.ElementId(q.typeId));
            if (DalRecursos.CrearEntidadRecursos(_clipboardRecursos, elem))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void mnuCopiarRecursos_Click(object sender, EventArgs e)
        {
            string qId = this.treeViewComputo.SelectedNode.Name;
            Quantity q = _lstQuantity.Find(x => x.qId == qId);
            List<QexRecurso> lstRecursos = new List<QexRecurso>();
            foreach (QexRecurso rec in Tools.GetRecursosFromQuantity(q))
            {
                lstRecursos.Add(rec);
            }
            if (lstRecursos.Count > 0)
            {
                this.mnuPegarRecursos.Enabled = true;
            }
            _clipboardRecursos = lstRecursos.ToList();
        }

        private void mnuPegarRecursos_Click(object sender, EventArgs e)
        {
            string qid = this.treeViewComputo.SelectedNode.Name;
            Quantity q = _lstQuantity.Find(x => x.qId == qid);
            if (pasteFromClipboard(q))
            {
                MessageBox.Show("Los Recursos se pegaron con éxito", RevitQex.QexName,
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                treePintarRecursos(this.treeViewComputo);
                List<Quantity> lst = new List<Quantity>();
                lst.Add(q);
                fillListView(lst);
            }
            else
            {
                MessageBox.Show("No se pueden pegar los Recursos", RevitQex.QexName,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void treePintarRecursos(TreeView tree)
        {
            foreach (TreeNode node0 in tree.Nodes)
            {
                foreach (TreeNode node1 in node0.Nodes)
                {
                    foreach (TreeNode node2 in node1.Nodes)
                    {
                        if (node0.Name != "AA")
                        {
                            string qId = node2.Name;
                            Quantity q = _lstQuantity.Find(X => X.qId == qId);
                            if (Tools.GetRecursosFromQuantity(q).Count > 0)
                            {
                                node2.ImageIndex = 10;
                                node2.SelectedImageIndex = 10;
                            }
                            else
                            {
                                node2.ImageIndex = 13;
                                node2.SelectedImageIndex = 13;
                            }
                        }
                        foreach (TreeNode node3 in node2.Nodes)
                        {
                            string qId = node3.Name;
                            Quantity q = _lstQuantity.Find(X => X.qId == qId);
                            if (Tools.GetRecursosFromQuantity(q).Count > 0)
                            {
                                node3.ImageIndex = 10;
                                node3.SelectedImageIndex = 10;
                            }
                            else
                            {
                                node3.ImageIndex = 13;
                                node3.SelectedImageIndex = 13;
                            }
                        }
                    }
                }
            }
        }

        private void lstvRecursos_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnGestionarRecursos_Click(object sender, EventArgs e)
        {
            (new frmSetMaterialDb()).ShowDialog();
        }

        private void toolBtnExportExcel_Click(object sender, EventArgs e)
        {
            //Exporting to Excel
            string folderPath = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Documento Excel|*.xlsx";
            string title = Tools._doc.Title;
            sfd.FileName = title + ".xlsx";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderPath = sfd.FileName;
                try
                {
                    Informe.CrearExcelRecursos(treeViewComputo, folderPath);
                    DialogResult result2 = MessageBox.Show("Documento creado correctamente. ¿Desea abrir el archivo creado?"
                        , RevitQex.QexName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result2 == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(folderPath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void mnuBtnExportAsignacionesExcel_Click(object sender, EventArgs e)
        {
            //Exporting to Excel
            string folderPath = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Documento Excel|*.xlsx";
            string title = Tools._doc.Title;
            sfd.FileName = title + " recursos asignados.xlsx";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderPath = sfd.FileName;
                try
                {
                    Informe.CrearExcelRecursosAsignados(treeViewComputo, folderPath);
                    DialogResult result2 = MessageBox.Show("Documento creado correctamente. ¿Desea abrir el archivo creado?"
                        , RevitQex.QexName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result2 == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(folderPath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void mnuBtnExportarRecursosDetallados_Click(object sender, EventArgs e)
        {
            //Exporting to Excel
            string folderPath = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Documento Excel|*.xlsx";
            string title = Tools._doc.Title;
            sfd.FileName = title + " recursos detallados.xlsx";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderPath = sfd.FileName;
                try
                {
                    Informe.CrearExcelRecursosDetallados(treeViewComputo, folderPath, _verDescripcion);
                    DialogResult result2 = MessageBox.Show("Documento creado correctamente. ¿Desea abrir el archivo creado?"
                        , RevitQex.QexName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result2 == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(folderPath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void mnuBtnImportarAsignaciones_Click(object sender, EventArgs e)
        {
            DialogResult pregunta = MessageBox.Show("Con este comando se pueden importar Asignaciones de Recursos " +
                "exportados previamente. Si alguna Quantificación tiene el mismo nombre o descripción, entonces se " +
                "agregarán los recursos correspondientes. Si los recursos NO existen en el Proyecto, se crearán " +
                "como nuevos recursos. ¿Desea continuar?", RevitQex.QexName, MessageBoxButtons.YesNo, 
                MessageBoxIcon.Information);
            if (pregunta == DialogResult.Yes)
            {
                //Importing from Excel
                string folderPath = string.Empty;
                OpenFileDialog sfd = new OpenFileDialog();
                sfd.Filter = "Documento Excel|*.xlsx";
                DialogResult result = sfd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    folderPath = sfd.FileName;
                    try
                    {
                        Tools.ImportAsignacionesFromExcel(folderPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, RevitQex.QexName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    TreeNode node = this.treeViewComputo.SelectedNode;
                    treePintarRecursos(this.treeViewComputo);
                    this.treeViewComputo.SelectedNode = null;
                    this.treeViewComputo.SelectedNode = node;
                }
            }
        }

        private void mnuQuitarRecurso_Click(object sender, EventArgs e)
        {
            DialogResult pregunta = MessageBox.Show("¿Desea quitar todos los recursos de esta Quantificación?",
                RevitQex.QexName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (pregunta == DialogResult.Yes)
            {
                int count = 0;
                StringBuilder errores = new StringBuilder();
                string qid = this.treeViewComputo.SelectedNode.Name;
                Quantity q = _lstQuantity.Find(x => x.qId == qid);
                Element elem = Tools.GetElementTypeFromQuantity(q);
                List<QexRecurso> recursos = Tools.GetRecursosFromQuantity(q);
                foreach (QexRecurso rec in recursos)
                {
                    if (rec.Delete(elem))
                    {
                        count++;
                    }
                    else
                    {
                        QexMaterial mat = DalRecursos.GetMaterialById(rec.id);
                        errores.AppendLine(mat.nombre + " no se pudo quitar de la Quantificación");
                    }
                }
                if (errores.Length > 0)
                {
                    (new frmWarnings("Lista de Errores", "Errores encontrados al quitar recursos",
                        errores.ToString())).ShowDialog();
                }
                else
                {
                    MessageBox.Show(count + " recursos eliminados correctamente", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                TreeNode node = this.treeViewComputo.SelectedNode;
                treePintarRecursos(this.treeViewComputo);
                this.treeViewComputo.SelectedNode = this.treeViewComputo.Nodes[0];
                this.treeViewComputo.SelectedNode = node;
            }
        }

        private void lstvRecursos_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {

        }

        private void lstvRecursos_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int colIndex = e.Column;

        }
    }
}
