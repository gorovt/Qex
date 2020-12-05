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

namespace Qex
{
    partial class frmRecursosMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRecursosMain));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.treeViewComputo = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lstvRecursos = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.txtElemento = new System.Windows.Forms.Label();
            this.txtCosto = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnGestionarRecursos = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRecursos = new System.Windows.Forms.ToolStripMenuItem();
            this.toolBtnExportExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBtnExportarRecursosDetallados = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAsignaciones = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBtnExportAsignacionesExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBtnImportarAsignaciones = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.mnuTreeItem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuItemVerRecursos = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCopiarRecursos = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPegarRecursos = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuQuitarRecurso = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.mnuTreeItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 539);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 43);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel4);
            this.splitContainer1.Size = new System.Drawing.Size(778, 493);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.SplitterWidth = 17;
            this.splitContainer1.TabIndex = 9;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.treeViewComputo, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(300, 493);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightSalmon;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(290, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "   Navegador de Cómputos";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // treeViewComputo
            // 
            this.treeViewComputo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewComputo.HideSelection = false;
            this.treeViewComputo.Location = new System.Drawing.Point(5, 53);
            this.treeViewComputo.Name = "treeViewComputo";
            this.treeViewComputo.Size = new System.Drawing.Size(290, 435);
            this.treeViewComputo.TabIndex = 0;
            this.treeViewComputo.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewComputo_AfterSelect);
            this.treeViewComputo.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewComputo_NodeMouseClick);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.lstvRecursos, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(461, 493);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // lstvRecursos
            // 
            this.lstvRecursos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lstvRecursos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvRecursos.FullRowSelect = true;
            this.lstvRecursos.HideSelection = false;
            this.lstvRecursos.Location = new System.Drawing.Point(5, 53);
            this.lstvRecursos.MultiSelect = false;
            this.lstvRecursos.Name = "lstvRecursos";
            this.lstvRecursos.Size = new System.Drawing.Size(451, 393);
            this.lstvRecursos.TabIndex = 1;
            this.lstvRecursos.UseCompatibleStateImageBehavior = false;
            this.lstvRecursos.View = System.Windows.Forms.View.Details;
            this.lstvRecursos.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstvRecursos_ColumnClick);
            this.lstvRecursos.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.lstvRecursos_ItemMouseHover);
            this.lstvRecursos.SelectedIndexChanged += new System.EventHandler(this.lstvRecursos_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Nombre";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Cantidad";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader2.Width = 75;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Unidad";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Código";
            this.columnHeader4.Width = 0;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Costo";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader5.Width = 95;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.LightSalmon;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(451, 40);
            this.label2.TabIndex = 2;
            this.label2.Text = "   Lista de Recursos";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel5.Controls.Add(this.txtElemento, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.txtCosto, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(5, 454);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(451, 34);
            this.tableLayoutPanel5.TabIndex = 3;
            // 
            // txtElemento
            // 
            this.txtElemento.AutoSize = true;
            this.txtElemento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtElemento.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtElemento.Location = new System.Drawing.Point(6, 0);
            this.txtElemento.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.txtElemento.Name = "txtElemento";
            this.txtElemento.Size = new System.Drawing.Size(261, 34);
            this.txtElemento.TabIndex = 0;
            this.txtElemento.Text = "Costo Total";
            this.txtElemento.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCosto
            // 
            this.txtCosto.AutoSize = true;
            this.txtCosto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCosto.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCosto.Location = new System.Drawing.Point(273, 0);
            this.txtCosto.Name = "txtCosto";
            this.txtCosto.Size = new System.Drawing.Size(175, 34);
            this.txtCosto.TabIndex = 1;
            this.txtCosto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label4, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(784, 40);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.LightSalmon;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(45, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(736, 40);
            this.label4.TabIndex = 2;
            this.label4.Text = "Recursos de Cómputos";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip1.Size = new System.Drawing.Size(42, 40);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.AutoToolTip = false;
            this.toolStripDropDownButton1.BackColor = System.Drawing.Color.Transparent;
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnGestionarRecursos,
            this.mnuRecursos,
            this.mnuAsignaciones});
            this.toolStripDropDownButton1.Image = global::Qex.Resource1.menu2_32;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(37, 37);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // btnGestionarRecursos
            // 
            this.btnGestionarRecursos.Image = global::Qex.Resource1.database_gear;
            this.btnGestionarRecursos.Name = "btnGestionarRecursos";
            this.btnGestionarRecursos.Size = new System.Drawing.Size(182, 30);
            this.btnGestionarRecursos.Text = "Gestionar Recursos";
            this.btnGestionarRecursos.Click += new System.EventHandler(this.btnGestionarRecursos_Click);
            // 
            // mnuRecursos
            // 
            this.mnuRecursos.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnExportExcel,
            this.mnuBtnExportarRecursosDetallados});
            this.mnuRecursos.Image = global::Qex.Resource1.excel_exports;
            this.mnuRecursos.Name = "mnuRecursos";
            this.mnuRecursos.Size = new System.Drawing.Size(182, 30);
            this.mnuRecursos.Text = "Recursos";
            // 
            // toolBtnExportExcel
            // 
            this.toolBtnExportExcel.Image = global::Qex.Resource1.excel_2013;
            this.toolBtnExportExcel.Name = "toolBtnExportExcel";
            this.toolBtnExportExcel.Size = new System.Drawing.Size(232, 30);
            this.toolBtnExportExcel.Text = "Exportar Recursos";
            this.toolBtnExportExcel.Click += new System.EventHandler(this.toolBtnExportExcel_Click);
            // 
            // mnuBtnExportarRecursosDetallados
            // 
            this.mnuBtnExportarRecursosDetallados.Image = global::Qex.Resource1.excel_2013;
            this.mnuBtnExportarRecursosDetallados.Name = "mnuBtnExportarRecursosDetallados";
            this.mnuBtnExportarRecursosDetallados.Size = new System.Drawing.Size(232, 30);
            this.mnuBtnExportarRecursosDetallados.Text = "Exportar Recursos detallados";
            this.mnuBtnExportarRecursosDetallados.Click += new System.EventHandler(this.mnuBtnExportarRecursosDetallados_Click);
            // 
            // mnuAsignaciones
            // 
            this.mnuAsignaciones.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBtnExportAsignacionesExcel,
            this.mnuBtnImportarAsignaciones});
            this.mnuAsignaciones.Image = global::Qex.Resource1.excel_exports;
            this.mnuAsignaciones.Name = "mnuAsignaciones";
            this.mnuAsignaciones.Size = new System.Drawing.Size(182, 30);
            this.mnuAsignaciones.Text = "Asignaciones";
            // 
            // mnuBtnExportAsignacionesExcel
            // 
            this.mnuBtnExportAsignacionesExcel.Image = global::Qex.Resource1.excel_2013;
            this.mnuBtnExportAsignacionesExcel.Name = "mnuBtnExportAsignacionesExcel";
            this.mnuBtnExportAsignacionesExcel.Size = new System.Drawing.Size(201, 30);
            this.mnuBtnExportAsignacionesExcel.Text = "Exportar asignaciones";
            this.mnuBtnExportAsignacionesExcel.Click += new System.EventHandler(this.mnuBtnExportAsignacionesExcel_Click);
            // 
            // mnuBtnImportarAsignaciones
            // 
            this.mnuBtnImportarAsignaciones.Image = global::Qex.Resource1.excel_2013;
            this.mnuBtnImportarAsignaciones.Name = "mnuBtnImportarAsignaciones";
            this.mnuBtnImportarAsignaciones.Size = new System.Drawing.Size(201, 30);
            this.mnuBtnImportarAsignaciones.Text = "Importar Asignaciones";
            this.mnuBtnImportarAsignaciones.Click += new System.EventHandler(this.mnuBtnImportarAsignaciones_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "book-brown.png");
            this.imageList1.Images.SetKeyName(1, "envelope.png");
            this.imageList1.Images.SetKeyName(2, "notebook.png");
            this.imageList1.Images.SetKeyName(3, "building.png");
            this.imageList1.Images.SetKeyName(4, "calendar.png");
            this.imageList1.Images.SetKeyName(5, "building-low.png");
            this.imageList1.Images.SetKeyName(6, "flag-green.png");
            this.imageList1.Images.SetKeyName(7, "navtools-orthographic.ico");
            this.imageList1.Images.SetKeyName(8, "Ribbon_OverrideItemTransparency_16.ico");
            this.imageList1.Images.SetKeyName(9, "hard-hat.png");
            this.imageList1.Images.SetKeyName(10, "box.png");
            this.imageList1.Images.SetKeyName(11, "folder-horizontal.png");
            this.imageList1.Images.SetKeyName(12, "folder-horizontal-open.png");
            this.imageList1.Images.SetKeyName(13, "box-grey.png");
            // 
            // mnuTreeItem
            // 
            this.mnuTreeItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuItemVerRecursos,
            this.toolStripSeparator1,
            this.mnuCopiarRecursos,
            this.mnuPegarRecursos,
            this.toolStripSeparator2,
            this.mnuQuitarRecurso});
            this.mnuTreeItem.Name = "mnuTreeItem";
            this.mnuTreeItem.Size = new System.Drawing.Size(160, 104);
            // 
            // mnuItemVerRecursos
            // 
            this.mnuItemVerRecursos.Image = global::Qex.Resource1.box__pencil1;
            this.mnuItemVerRecursos.Name = "mnuItemVerRecursos";
            this.mnuItemVerRecursos.Size = new System.Drawing.Size(159, 22);
            this.mnuItemVerRecursos.Text = "Ver Recursos";
            this.mnuItemVerRecursos.Click += new System.EventHandler(this.mnuItemVerRecursos_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(156, 6);
            // 
            // mnuCopiarRecursos
            // 
            this.mnuCopiarRecursos.Image = global::Qex.Resource1.page_copy;
            this.mnuCopiarRecursos.Name = "mnuCopiarRecursos";
            this.mnuCopiarRecursos.Size = new System.Drawing.Size(159, 22);
            this.mnuCopiarRecursos.Text = "Copiar Recursos";
            this.mnuCopiarRecursos.Click += new System.EventHandler(this.mnuCopiarRecursos_Click);
            // 
            // mnuPegarRecursos
            // 
            this.mnuPegarRecursos.Enabled = false;
            this.mnuPegarRecursos.Image = global::Qex.Resource1.paste_plain;
            this.mnuPegarRecursos.Name = "mnuPegarRecursos";
            this.mnuPegarRecursos.Size = new System.Drawing.Size(159, 22);
            this.mnuPegarRecursos.Text = "Pegar Recursos";
            this.mnuPegarRecursos.Click += new System.EventHandler(this.mnuPegarRecursos_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(156, 6);
            // 
            // mnuQuitarRecurso
            // 
            this.mnuQuitarRecurso.Image = global::Qex.Resource1.delete16;
            this.mnuQuitarRecurso.Name = "mnuQuitarRecurso";
            this.mnuQuitarRecurso.Size = new System.Drawing.Size(159, 22);
            this.mnuQuitarRecurso.Text = "Quitar Recursos";
            this.mnuQuitarRecurso.Click += new System.EventHandler(this.mnuQuitarRecurso_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // frmRecursosMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimizeBox = false;
            this.Name = "frmRecursosMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Asignar Recursos";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.mnuTreeItem.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem btnGestionarRecursos;
        private System.Windows.Forms.TreeView treeViewComputo;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView lstvRecursos;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ContextMenuStrip mnuTreeItem;
        private System.Windows.Forms.ToolStripMenuItem mnuItemVerRecursos;
        private System.Windows.Forms.ToolStripMenuItem mnuCopiarRecursos;
        private System.Windows.Forms.ToolStripMenuItem mnuPegarRecursos;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem mnuRecursos;
        private System.Windows.Forms.ToolStripMenuItem toolBtnExportExcel;
        private System.Windows.Forms.ToolStripMenuItem mnuAsignaciones;
        private System.Windows.Forms.ToolStripMenuItem mnuBtnExportAsignacionesExcel;
        private System.Windows.Forms.ToolStripMenuItem mnuBtnExportarRecursosDetallados;
        private System.Windows.Forms.ToolStripMenuItem mnuBtnImportarAsignaciones;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuQuitarRecurso;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label txtElemento;
        private System.Windows.Forms.Label txtCosto;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}