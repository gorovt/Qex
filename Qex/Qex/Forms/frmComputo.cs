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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;

namespace Qex
{
    public partial class frmComputo : System.Windows.Forms.Form
    {
        public static List<Quantity> lstQuantity = new List<Quantity>();
        public static Autodesk.Revit.DB.Document _doc;
        public static bool _verDescripcion;
        // Make sure you have the correct using clause to see DllImport:
        // using System.Runtime.InteropServices;
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        public frmComputo(Autodesk.Revit.DB.Document doc, List<Quantity> lstaQuantity, bool verDescripcion)
        {
            InitializeComponent();
            _doc = doc;
            lstQuantity = lstaQuantity;
            _verDescripcion = verDescripcion;

            List<Quantity> lst = new List<Quantity>();
            lst = QexSchema.ReadComputos(_doc);
            foreach (Quantity qu in lst)
            {
                if (lstQuantity.Exists(x => x.qId == qu.qId))
                {
                    Quantity q = lstQuantity.Find(x => x.qId == qu.qId);
                    q.grupo = qu.grupo;
                    q.libro = qu.libro;
                }
            }
            rellenarTreeMultiple(lstQuantity, _verDescripcion);
            foreach (TreeNode node0 in this.treeViewComputo.Nodes)
            {
                node0.Expand();
            }
            if (_verDescripcion)
            {
                this.btnVerTipos.Enabled = true;
                this.btnVerDescripcion.Enabled = false;
            }
            if (!_verDescripcion)
            {
                this.btnVerTipos.Enabled = false;
                this.btnVerDescripcion.Enabled = true;
            }

            treeViewComputo.AllowDrop = true;
            // Add event handlers for the required drag events.
            treeViewComputo.ItemDrag += new ItemDragEventHandler(treeViewComputo_ItemDrag);
            treeViewComputo.DragEnter += new DragEventHandler(treeViewComputo_DragEnter);
            treeViewComputo.DragOver += new DragEventHandler(treeViewComputo_DragOver);
            treeViewComputo.DragDrop += new DragEventHandler(treeViewComputo_DragDrop);
        }

        #region Treeview Propiedades
        private void fillTreeLibros(TreeView tree, List<Quantity> lstQ, bool verDescripcion)
        {
            tree.ImageList = imageList1;
            lstQ = lstQ.OrderBy(x => x.libroOrden).ThenBy(x => x.grupoOrden).ThenBy(x => x.qOrden).ToList();
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
        }
        private void fillBanderas(TreeView tree, Quantity q, string folder)
        {
            //Poner banderas verdes
            if (tree.Nodes["AA"].Nodes[folder].Nodes[q.category].Nodes.Count == 0)
            {
                tree.Nodes["AA"].Nodes[folder].Nodes[q.category].ImageIndex = 6;
                tree.Nodes["AA"].Nodes[folder].Nodes[q.category].SelectedImageIndex = 6;
            }
            // Poner iconos de materiales y demoliciones
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
                if (q.category.Contains("(Grupo)"))
                {
                    tree.Nodes["AA"].Nodes[folder].Nodes[q.category].ImageIndex = 10;
                    tree.Nodes["AA"].Nodes[folder].Nodes[q.category].SelectedImageIndex = 10;
                }
                if (q.category.Contains("Montajes"))
                {
                    tree.Nodes["AA"].Nodes[folder].Nodes[q.category].ImageIndex = 11;
                    tree.Nodes["AA"].Nodes[folder].Nodes[q.category].SelectedImageIndex = 11;
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
        private void rellenarTreeMultiple(List<Quantity> lstQ, bool verDescripcion)
        {
            this.treeViewComputo.ImageList = imageList1;
            fillTreeLibros(this.treeViewComputo, lstQ, verDescripcion);
            fillTreeRevit(this.treeViewComputo, lstQ, verDescripcion);
        }
        private void treeViewComputo_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeViewComputo.SelectedNode = e.Node;
                int level = this.treeViewComputo.SelectedNode.Level;
                if (level == 0)
                {
                    mnuTreeProyecto.Show(Cursor.Position);
                }
                if (level == 1)
                {
                    mnuTreeGrupo.Show(Cursor.Position);
                }
                if (level == 2)
                {
                    mnuTreeItem.Show(Cursor.Position);
                }
                if (level == 3)
                {
                    mnuTreeItem.Show(Cursor.Position);
                }
            }
        }
        private void treeViewComputo_ItemDrag(object sender, ItemDragEventArgs e)
        {
            string root = "";
            TreeNode node = e.Item as TreeNode;
            while (node.Parent != null)
            {
                node = node.Parent;
                root = node.Name;
            }
            // Move the dragged node when the left mouse button is used.
            if (e.Button == MouseButtons.Left)
            {
                if ((e.Item as TreeNode).Level == 2 && root != "AA")
                {
                    DoDragDrop(e.Item, DragDropEffects.Move);
                    Tools.AddPoint(0.5);
                }
                if ((e.Item as TreeNode).Level == 3)
                {
                    DoDragDrop(e.Item, DragDropEffects.Move);
                    Tools.AddPoint(0.5);
                }
            }
            // Copy the dragged node when the right mouse button is used.
            //else if (e.Button == MouseButtons.Right)
            //{
            //    DoDragDrop(e.Item, DragDropEffects.Copy);
            //}
        }
        private void treeViewComputo_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }
        private void treeViewComputo_DragOver(object sender, DragEventArgs e)
        {
            // Set a constant to define the autoscroll region
            const Single scrollRegion = 20;

            // See where the cursor is
            System.Drawing.Point pt = treeViewComputo.PointToClient(Cursor.Position);

            // See if we need to scroll up or down
            if ((pt.Y + scrollRegion) > treeViewComputo.Height)
            {
                // Call the API to scroll down
                SendMessage(treeViewComputo.Handle, (int)277, (int)1, 0);
            }
            else if (pt.Y < (treeViewComputo.Top + scrollRegion))
            {
                // Call thje API to scroll up
                SendMessage(treeViewComputo.Handle, (int)277, (int)0, 0);
            }

            // Retrieve the client coordinates of the mouse position.
            System.Drawing.Point targetPoint = treeViewComputo.PointToClient(new System.Drawing.Point(e.X, e.Y));

            // Select the node at the mouse position.
            treeViewComputo.SelectedNode = treeViewComputo.GetNodeAt(targetPoint);
        }
        private void treeViewComputo_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            System.Drawing.Point targetPoint = treeViewComputo.PointToClient(new System.Drawing.Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = treeViewComputo.GetNodeAt(targetPoint);
            TreeNode parentDraggedNode = ((TreeNode)e.Data.GetData(typeof(TreeNode))).Parent;

            if (targetNode.Level == 1)
            {
                if (targetNode.Parent.Name != "AA")
                {
                    // Retrieve the node that was dragged.
                    TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

                    // Confirm that the node at the drop location is not 
                    // the dragged node or a descendant of the dragged node.
                    if (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode))
                    {
                        // If it is a move operation, remove the node from its current 
                        // location and add it to the node at the drop location.
                        if (e.Effect == DragDropEffects.Move)
                        {
                            draggedNode.Remove();
                            targetNode.Nodes.Add(draggedNode);
                            this.treeViewComputo.SelectedNode = draggedNode;
                            //Update Quantity Group
                            Quantity q = lstQuantity.Find(x => x.qId == draggedNode.Name);
                            q.grupo = targetNode.Name;
                            q.libro = targetNode.Parent.Name;
                            Tools.AddPoint(0.5);
                        }

                        targetNode.Expand();
                    }
                    if (parentDraggedNode.Nodes.Count == 0 && parentDraggedNode.Parent.Parent != null)
                    {
                        if (parentDraggedNode.Parent.Parent.Name == "AA")
                        {
                            parentDraggedNode.ImageIndex = 6;
                            parentDraggedNode.SelectedImageIndex = 6;
                            treeViewComputo.Refresh();
                            treeViewComputo.Update();
                        }
                    }
                }
            }
        }
        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            // Check the parent node of the second node.
            if (node2.Parent == null) return false;
            if (node2.Parent.Equals(node1)) return true;

            // If the parent node is not null or equal to the first node, 
            // call the ContainsNode method recursively using the parent of 
            // the second node.
            return ContainsNode(node1, node2.Parent);
        }
        private void treeViewComputo_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int level = e.Node.Level;
            txtTexto.Text = "";
            txtCosto.Text = String.Format("{0:C}", 0);

            if (level == 0 && e.Node.Name != "AA")
            {
                txtTexto.Text = e.Node.Text + " >> Costo Total:";
                double totalCost = 0;

                foreach (TreeNode node1 in e.Node.Nodes)
                {
                    foreach (TreeNode node2 in node1.Nodes)
                    {
                        Quantity q = lstQuantity.Find(x => x.qId == node2.Name);
                        totalCost += q.totalCost;
                    }
                }
                txtCosto.Text = String.Format("{0:C}", totalCost);
            }
            if (level == 1 && e.Node.Parent.Name != "AA")
            {
                txtTexto.Text = e.Node.Text + " >> Costo Total:";
                double totalCost = 0;
                foreach (TreeNode node1 in e.Node.Nodes)
                {
                    Quantity q = lstQuantity.Find(x => x.qId == node1.Name);
                    totalCost += q.totalCost;
                }
                txtCosto.Text = String.Format("{0:C}", totalCost);
            }
            if (level == 2 && e.Node.Parent.Parent.Name != "AA")
            {
                txtTexto.Text = e.Node.Text + " >> Costo Total:";
                double totalCost = 0;
                Quantity q = lstQuantity.Find(x => x.qId == e.Node.Name);
                totalCost += q.totalCost;
                txtCosto.Text = String.Format("{0:C}", totalCost);
            }
        }
        private void treeViewComputo_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                if (e.Node.Name != "AA" && e.Label != null)
                {
                    foreach (Quantity q in lstQuantity.FindAll(x => x.libro == e.Node.Name))
                    {
                        q.libro = e.Label;
                    }
                    if (this.treeViewComputo.Nodes.Contains(this.treeViewComputo.Nodes[e.Label]))
                    {
                        MessageBox.Show("Este nombre ya existe. Escriba un nombre único", RevitQex.QexName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    e.Node.Name = e.Label;
                    this.treeViewComputo.SelectedNode = e.Node;
                }
            }
            if (e.Node.Level == 1 && e.Label != null)
            {
                foreach (Quantity q in lstQuantity.FindAll(x => x.grupo == e.Node.Name && x.libro == e.Node.Parent.Name))
                {
                    q.grupo = e.Label;
                }
                if (this.treeViewComputo.Nodes[e.Node.Parent.Name].Nodes.Contains(this.treeViewComputo.Nodes[e.Node.Parent.Name].Nodes[e.Label]))
                {
                    MessageBox.Show("Este nombre ya existe. Escriba un nombre único", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                e.Node.Name = e.Label;
                this.treeViewComputo.SelectedNode = e.Node;
            }
            this.treeViewComputo.LabelEdit = false;
        }
        private void banderasVerdes(TreeView tree)
        {
            foreach (TreeNode node1 in tree.Nodes["AA"].Nodes)
            {
                foreach (TreeNode node2 in node1.Nodes)
                {
                    if (node2.Nodes.Count == 0)
                    {
                        node2.ImageIndex = 6;
                        node2.SelectedImageIndex = 6;
                    }
                }
            }
        }
#endregion
        #region Menu Proyecto
        private void mnuTreeProyectoRenombrar_Click(object sender, EventArgs e)
        {
            int level = this.treeViewComputo.SelectedNode.Level;
            if (level == 0 && this.treeViewComputo.SelectedNode.Name == "AA")
            {
                MessageBox.Show("No se puede renombrar este Grupo", RevitQex.QexName,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.treeViewComputo.LabelEdit = true;
                this.treeViewComputo.SelectedNode.BeginEdit();
            }
        }
        private void mnuTreeProyectoEliminarLibro_Click(object sender, EventArgs e)
        {
            TreeNode nodeSelected = this.treeViewComputo.SelectedNode;
            if (nodeSelected.Name != "AA")
            {
                if (nodeSelected.Nodes.Count == 0)
                {
                    nodeSelected.Remove();
                }
                else
                {
                    MessageBox.Show("El Libro debe estar vacío", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("No se puede borrar este Grupo", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void mnuTreeProyectoAgregarGrupo_Click(object sender, EventArgs e)
        {
            crearGrupo();
            Tools.AddPoint(0.1);
        }
        #endregion

        #region Menu Grupo
        private void mnuTreeGrupoRenombrar_Click(object sender, EventArgs e)
        {
            int level = this.treeViewComputo.SelectedNode.Level;
            if (this.treeViewComputo.SelectedNode.Parent.Name != "AA")
            {
                this.treeViewComputo.LabelEdit = true;
                this.treeViewComputo.SelectedNode.BeginEdit();
            }
            else
            {
                MessageBox.Show("No se puede renombrar este Grupo", RevitQex.QexName,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void mnuTreeGrupoBorrar_Click(object sender, EventArgs e)
        {
            TreeNode nodeSelected = this.treeViewComputo.SelectedNode;
            if (nodeSelected.Parent.Name != "AA")
            {
                if (nodeSelected.Nodes.Count == 0)
                {
                    nodeSelected.Remove();
                }
                else
                {
                    MessageBox.Show("El Grupo debe estar vacío", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("No se puede borrar este Grupo", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void mnuTreeGrupoMoverUP_Click(object sender, EventArgs e)
        {
            MoveUp(this.treeViewComputo.SelectedNode);
        }
        private void mnuTreeGrupoMoveDown_Click(object sender, EventArgs e)
        {
            MoveDown(this.treeViewComputo.SelectedNode);
        }
        public static void MoveUp(TreeNode node)
        {
            TreeNode parent = node.Parent;
            TreeView view = node.TreeView;
            if (parent != null)
            {
                int index = parent.Nodes.IndexOf(node);
                if (index > 0)
                {
                    parent.Nodes.RemoveAt(index);
                    parent.Nodes.Insert(index - 1, node);
                    view.SelectedNode = parent.Nodes[index - 1];
                }
            }
            else if (node.TreeView.Nodes.Contains(node)) //root node
            {
                int index = view.Nodes.IndexOf(node);
                if (index > 0)
                {
                    view.Nodes.RemoveAt(index);
                    view.Nodes.Insert(index - 1, node);
                    view.SelectedNode = view.Nodes[index - 1];
                }
            }
            Tools.AddPoint(0.1);
        }
        public static void MoveDown(TreeNode node)
        {
            TreeNode parent = node.Parent;
            TreeView view = node.TreeView;
            if (parent != null)
            {
                int index = parent.Nodes.IndexOf(node);
                if (index < parent.Nodes.Count - 1)
                {
                    parent.Nodes.RemoveAt(index);
                    parent.Nodes.Insert(index + 1, node);
                    view.SelectedNode = parent.Nodes[index + 1];
                }
            }
            else if (view != null && view.Nodes.Contains(node)) //root node
            {
                int index = view.Nodes.IndexOf(node);
                if (index < view.Nodes.Count - 1)
                {
                    view.Nodes.RemoveAt(index);
                    view.Nodes.Insert(index + 1, node);
                    view.SelectedNode = view.Nodes[index + 1];
                }
            }
            Tools.AddPoint(0.1);
        }
        #endregion

        #region Menu Items
        private void mnuTreeItemEliminar_Click(object sender, EventArgs e)
        {
            // Validaciones
            TreeNode selectedNode = this.treeViewComputo.SelectedNode;
            // ¿Es una Quantificación?
            if (lstQuantity.Find(x => x.qId == selectedNode.Name) != null)
            {
                Quantity q = lstQuantity.Find(x => x.qId == this.treeViewComputo.SelectedNode.Name);
                // ¿Tiene un Grupo?
                if (q.grupo != "")
                {
                    deleteItem(this.treeViewComputo, this.treeViewComputo.SelectedNode);
                }
                else
                {
                    MessageBox.Show("No se puede borrar este Item", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("No se puede borrar este Grupo", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void deleteItem(TreeView tree, TreeNode selectedNode)
        {
            Quantity q = lstQuantity.Find(x => x.qId == selectedNode.Name);
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
                if (q.LevelId != -1)
                {
                    Element lvl = _doc.GetElement(new ElementId(q.LevelId));
                    lvlName = lvl.Name;
                }
                folderText = q.phaseCreated + " // " + lvlName;
            }

            selectedNode.Remove();
            if (_verDescripcion)
            {
                tree.Nodes["AA"].Nodes[folder].Nodes[q.category].Nodes.Add(q.qId, q.descripcion +
                ": " + q.medicion, 2, 2);
            }
            if (!_verDescripcion)
            {
                tree.Nodes["AA"].Nodes[folder].Nodes[q.category].Nodes.Add(q.qId, q.name +
                ": " + q.medicion, 2, 2);
            }
            tree.Nodes["AA"].Nodes[folder].Nodes[q.category].ExpandAll();
            tree.SelectedNode = tree.Nodes["AA"].Nodes[folder].
                Nodes[q.category];//.Nodes[q.qId];
            q.grupo = "";
            q.libro = "";
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
                if (q.category.Contains("(Grupo)"))
                {
                    tree.Nodes["AA"].Nodes[folder].Nodes[q.category].ImageIndex = 10;
                    tree.Nodes["AA"].Nodes[folder].Nodes[q.category].SelectedImageIndex = 10;
                }
                if (q.category.Contains("Montajes"))
                {
                    tree.Nodes["AA"].Nodes[folder].Nodes[q.category].ImageIndex = 11;
                    tree.Nodes["AA"].Nodes[folder].Nodes[q.category].SelectedImageIndex = 11;
                }
                #endregion
                tree.Refresh();
                tree.Update();
            }
        }
        private void mnuTreeItemMoveUp_Click(object sender, EventArgs e)
        {
            MoveUp(this.treeViewComputo.SelectedNode);
        }
        private void mnuTreeItemMoveDown_Click(object sender, EventArgs e)
        {
            MoveDown(this.treeViewComputo.SelectedNode);
        }
        #endregion

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            saveChanges();
        }
        private void saveChanges()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                int libroOrden = 1;
                foreach (TreeNode node0 in this.treeViewComputo.Nodes)
                {
                    if (node0.Name != "AA")
                    {
                        int grupoOrden = 1;
                        foreach (TreeNode node1 in node0.Nodes)
                        {
                            int qOrden = 1;
                            foreach (TreeNode node2 in node1.Nodes)
                            {
                                Quantity q = RevitQex.lstElements.Find(x => x.qId == node2.Name);
                                q.qOrden = qOrden;
                                q.grupoOrden = grupoOrden;
                                q.libroOrden = libroOrden;
                                string ordenes = q.ToOrdenLine();
                                Schema schema = QexSchema.GetSchemaByName(QexSchema._schemaQex);
                                QexSchema.CreateFields(_doc, schema, q.typeId, q.cost, q.matriz, q.visible,
                                    q.paramId.ToString(), ordenes);
                                string computo = q.ToStringComputo();
                                sb.AppendLine(computo);
                                qOrden++;
                            }
                            grupoOrden++;
                        }
                        libroOrden++;
                    }

                }
            }
            catch (Exception ex)
            {
                StringBuilder sbe = new StringBuilder();
                sbe.AppendLine("Error:");
                sbe.AppendLine(ex.Message);
                MessageBox.Show(sb.ToString(), RevitQex.QexName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                QexSchema.CrearEntidadComputos(_doc, sb.ToString());
                MessageBox.Show("Libros guardados correctamente", RevitQex.QexName,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                StringBuilder sbe = new StringBuilder();
                sbe.AppendLine("Error:");
                sbe.AppendLine(ex.Message);
                MessageBox.Show(sbe.ToString(), RevitQex.QexName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }
        private void exportGrupos(StringBuilder sb)
        {
            //Exporting Qex
            string folderPath = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Quantificación (*.qex)|*.qex";
            string title = "Nombre del Proyecto";// docGral.Title.Remove(docGral.Title.Count() - 4, 4);
            sfd.FileName = title + ".qex";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderPath = sfd.FileName;
                try
                {
                    File.WriteAllText(folderPath, sb.ToString());
                    DialogResult result2 = MessageBox.Show("Libros guardados correctamente. ¿Desea abrir el archivo creado?"
                        , RevitQex.QexName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result2 == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(folderPath);
                    }
                    Tools.AddPoint(0.5);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, RevitQex.QexName, 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
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
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCrearGrupo_Click(object sender, EventArgs e)
        {
            crearGrupo();
        }
        private void btnUp_Click(object sender, EventArgs e)
        {
            MoveUp(this.treeViewComputo.SelectedNode);
        }
        private void btnDown_Click(object sender, EventArgs e)
        {
            MoveDown(this.treeViewComputo.SelectedNode);
        }

        private void btnVerTipos_Click(object sender, EventArgs e)
        {
            btnVerDescripcion.Enabled = true;
            btnVerTipos.Enabled = false;
            _verDescripcion = false;
            this.treeViewComputo.Nodes.Clear();
            rellenarTreeMultiple(lstQuantity, _verDescripcion);
            foreach (TreeNode node0 in this.treeViewComputo.Nodes)
            {
                node0.Expand();
                foreach (TreeNode node1 in node0.Nodes)
                {
                    node1.Expand();
                }
            }
        }

        private void btnVerDescripcion_Click(object sender, EventArgs e)
        {
            btnVerDescripcion.Enabled = false;
            btnVerTipos.Enabled = true;
            _verDescripcion = true;
            this.treeViewComputo.Nodes.Clear();
            rellenarTreeMultiple(lstQuantity, _verDescripcion);
            foreach (TreeNode node0 in this.treeViewComputo.Nodes)
            {
                node0.Expand();
                foreach (TreeNode node1 in node0.Nodes)
                {
                    node1.Expand();
                }
            }
        }

        private void btnExportarWord_Click(object sender, EventArgs e)
        {
            //Exporting to Word
            string folderPath = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Documento de Word|*.docx";
            string title = Tools._doc.Title;
            sfd.FileName = title + ".docx";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderPath = sfd.FileName;
                try
                {
                    Informe.CrearWordComputoImages(this.treeViewComputo, folderPath, _verDescripcion);
                    DialogResult result2 = MessageBox.Show("Documento creado correctamente. ¿Desea abrir el archivo creado?"
                        , RevitQex.QexName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result2 == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(folderPath);
                    }
                    Tools.AddPoint(0.5);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, RevitQex.QexName, 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                Tools.CleanTempFiles();
            }
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
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
                    Informe.CrearExcelComputoImages(treeViewComputo, folderPath, _verDescripcion);
                    DialogResult result2 = MessageBox.Show("Documento creado correctamente. ¿Desea abrir el archivo creado?"
                        , RevitQex.QexName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result2 == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(folderPath);
                    }
                    Tools.AddPoint(0.5);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, RevitQex.QexName, 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                Tools.CleanTempFiles();
            }
        }

        private void btnCrearLibro_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            string name = "New Book " + rnd.Next(1, 101);
            TreeNode node = new TreeNode();
            node.Name = name;
            node.Text = name;
            node.ImageIndex = 0;
            node.SelectedImageIndex = 0;
            this.treeViewComputo.Nodes.Insert(0, node);
            this.treeViewComputo.SelectedNode = this.treeViewComputo.Nodes[node.Name];
            Tools.AddPoint(1);
        }

        private void crearGrupo()
        {
            int level = this.treeViewComputo.SelectedNode.Level;

            if (level == 0)
            {
                if (this.treeViewComputo.SelectedNode.Name != "AA")
                {
                    TreeNode nodeSelected = this.treeViewComputo.SelectedNode;
                    Random rnd = new Random();
                    string name = "Nuevo Grupo " + rnd.Next(1, 101);
                    nodeSelected.Nodes.Add(name, name, 1, 1);
                    nodeSelected.Nodes[name].Parent.Expand();
                    this.treeViewComputo.SelectedNode = nodeSelected.Nodes[name];
                    Tools.AddPoint(0.1);
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un Libro", RevitQex.QexName, 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un Libro", RevitQex.QexName, 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExportarGR_Click(object sender, EventArgs e)
        {
            string folderPath = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Quantification (*.qex)|*.qex";
            string title = Tools._doc.Title;
            sfd.FileName = title + ".qex";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderPath = sfd.FileName;

                StringBuilder sb = new StringBuilder();
                string titles = "qId;typeId;Category;Name;Medicion;TotalCost;FaseCreación;Grupo;Libro;verDescripcion";
                sb.AppendLine(titles);
                foreach (TreeNode node0 in this.treeViewComputo.Nodes)
                {
                    if (node0.Name != "AA")
                    {
                        foreach (TreeNode node1 in node0.Nodes)
                        {
                            foreach (TreeNode node2 in node1.Nodes)
                            {
                                Quantity q = lstQuantity.Find(x => x.qId == node2.Name);
                                string line = Quantity.ToStringLine(q, _verDescripcion);
                                sb.AppendLine(line);
                            }
                        }
                    }
                }
                try
                {
                    File.WriteAllText(folderPath, sb.ToString());
                    MessageBox.Show("Quantificación exportada correctamente", RevitQex.QexName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    StringBuilder sx = new StringBuilder();
                    sx.AppendLine("Error: ");
                    sx.AppendLine(ex.Message);
                    MessageBox.Show(sx.ToString(), RevitQex.QexName, 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            //System.Diagnostics.Process.Start(QexOpciones.pathQex + "QexGR.exe");
        }

        private void treeViewComputo_AfterCheck(object sender, TreeViewEventArgs e)
        {
            int level = e.Node.Level;
            if (level == 0)
            {
                foreach (TreeNode node in e.Node.Nodes)
                {
                    if (e.Node.Checked == true)
                    {
                        node.Checked = true;
                        foreach (TreeNode node1 in node.Nodes)
                        {
                            node1.Checked = true;
                        }
                    }
                    else
                    {
                        node.Checked = false;
                        foreach (TreeNode node1 in node.Nodes)
                        {
                            node1.Checked = false;
                        }
                    }
                }
            }

            if (level == 1)
            {
                foreach (TreeNode node in e.Node.Nodes)
                {
                    if (e.Node.Checked == true)
                    {
                        node.Checked = true;
                        foreach (TreeNode node1 in node.Nodes)
                        {
                            node1.Checked = true;
                        }
                    }
                    else
                    {
                        node.Checked = false;
                        foreach (TreeNode node1 in node.Nodes)
                        {
                            node1.Checked = false;
                        }
                    }
                }
            }

            if (level == 2)
            {
                foreach (TreeNode node in e.Node.Nodes)
                {
                    if (e.Node.Checked == true)
                    {
                        node.Checked = true;
                    }
                    else
                    {
                        node.Checked = false;
                    }
                }
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            List<TreeNode> selectedNodes = new List<TreeNode>();

            foreach (TreeNode node0 in this.treeViewComputo.Nodes)
            {
                if (node0.Name == "AA")
                {
                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            foreach (TreeNode node3 in node2.Nodes)
                            {
                                if (node3.Checked == true)
                                {
                                    selectedNodes.Add(node3);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (TreeNode node1 in node0.Nodes)
                    {
                        foreach (TreeNode node2 in node1.Nodes)
                        {
                            if (node2.Checked == true)
                            {
                                selectedNodes.Add(node2);
                            }
                        }
                    }
                }
            }
            (new frmMoveNodes(this.treeViewComputo)).ShowDialog();
            if (frmMoveNodes._selectedNode != null)
            {
                foreach (TreeNode node0 in this.treeViewComputo.Nodes)
                {
                    if (node0.Name != "AA")
                    {
                        foreach (TreeNode node1 in node0.Nodes)
                        {
                            if (node1.Name == frmMoveNodes._selectedNode.Name &&
                                node1.Parent.Name == frmMoveNodes._selectedNode.Parent.Name)
                            {
                                MoveNodes(node1, selectedNodes);
                                foreach (TreeNode node in this.treeViewComputo.Nodes)
                                {
                                    clearChecked(node);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void clearChecked(TreeNode node)
        {
            node.Checked = false;
            foreach (TreeNode node1 in node.Nodes)
            {
                clearChecked(node1);
            }
        }

        private void MoveNodes(TreeNode targetNode, List<TreeNode> lstNodes)
        {
            TreeNode parentDraggedNode = targetNode.Parent;

            foreach (TreeNode draggedNode in lstNodes)
            {
                draggedNode.Checked = false;
                draggedNode.Remove();
                targetNode.Nodes.Add(draggedNode);

                //Update Quantity Group
                Quantity q = lstQuantity.Find(x => x.qId == draggedNode.Name);
                if (q != null)
                {
                    q.grupo = targetNode.Name;
                    q.libro = targetNode.Parent.Name;
                }
            }
            banderasVerdes(this.treeViewComputo);
            this.treeViewComputo.SelectedNode = targetNode;
            targetNode.Expand();
            Tools.AddPoint(lstNodes.Count / 10);
        }

        private void toolBtnSave_Click(object sender, EventArgs e)
        {
            saveChanges();
        }

        private void btnAutoCreate_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Los Libros y Grupos se crearán automáticamente, según " +
                "la agrupación elegida. ¿Desea continuar?", RevitQex.QexName, MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);
            switch (result)
            {
                case DialogResult.Yes:
                    {
                        // Recrear la Lista de Quantities
                        foreach (Quantity q in lstQuantity)
                        {
                            q.grupo = q.category;
                            int lvlId = -1;
                            string levelName = "No Level";
                            if (q.LevelId != -1)
                            {
                                Element lvl = _doc.GetElement(new ElementId(q.LevelId));
                                levelName = lvl.Name;
                                lvlId = lvl.Id.IntegerValue;
                            }
                            if (!QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // No Phase no Levels
                            {
                                q.libro = "Categories";
                            }
                            if (QexOpciones.QuantityByPhase && !QexOpciones.QuantityByLevel) // By Phase
                            {
                                q.libro = q.phaseCreated;
                            }
                            if (!QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Level
                            {
                                q.libro = levelName;
                            }
                            if (QexOpciones.QuantityByPhase && QexOpciones.QuantityByLevel) // By Phase and Level
                            {
                                q.libro = q.phaseCreated + " // " + levelName;
                            }
                        }
                        // Redibujar el Treeview
                        this.treeViewComputo.Nodes.Clear();
                        rellenarTreeMultiple(lstQuantity, _verDescripcion);
                        foreach (TreeNode node0 in this.treeViewComputo.Nodes)
                        {
                            node0.Expand();
                        }
                        if (_verDescripcion)
                        {
                            this.btnVerTipos.Enabled = true;
                            this.btnVerDescripcion.Enabled = false;
                        }
                        if (!_verDescripcion)
                        {
                            this.btnVerTipos.Enabled = false;
                            this.btnVerDescripcion.Enabled = true;
                        }
                        Tools.AddPoint(2);
                        break;
                    }
                case DialogResult.No:
                    {

                        break;
                    }
            }
        }

        private void btnEliminarSeleccionados_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Todas las Quantificaciones marcadas se eliminarán. " +
                "¿Desea continuar?", RevitQex.QexName, MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);
            switch (result)
            {
                case DialogResult.Yes:
                    {
                        List<TreeNode> lista = Tools.GetCheckedNodes(this.treeViewComputo);
                        foreach (TreeNode node in lista)
                        {
                            deleteItem(this.treeViewComputo, node);
                        }
                        this.treeViewComputo.SelectedNode = this.treeViewComputo.Nodes["AA"];
                        break;
                    }
                case DialogResult.No:
                    {

                        break;
                    }
            }
        }
    }
}
