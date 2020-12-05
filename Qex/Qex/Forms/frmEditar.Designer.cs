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
    partial class frmEditar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditar));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtParameter = new System.Windows.Forms.Label();
            this.rdbParameter = new System.Windows.Forms.RadioButton();
            this.txtVolume = new System.Windows.Forms.Label();
            this.txtArea = new System.Windows.Forms.Label();
            this.txtLength = new System.Windows.Forms.Label();
            this.txtCount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.rdbCant = new System.Windows.Forms.RadioButton();
            this.rdbLargo = new System.Windows.Forms.RadioButton();
            this.rdbArea = new System.Windows.Forms.RadioButton();
            this.rdbVolumen = new System.Windows.Forms.RadioButton();
            this.cmbParametros = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCost = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.pic1 = new System.Windows.Forms.PictureBox();
            this.mnuImage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOK = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnRecursos = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).BeginInit();
            this.mnuImage.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.tableLayoutPanel1.Controls.Add(this.txtParameter, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.rdbParameter, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtVolume, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtArea, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtLength, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtCount, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rdbCant, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rdbLargo, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.rdbArea, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.rdbVolumen, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbParametros, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(602, 85);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // txtParameter
            // 
            this.txtParameter.AutoSize = true;
            this.txtParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtParameter.Location = new System.Drawing.Point(412, 57);
            this.txtParameter.Name = "txtParameter";
            this.txtParameter.Size = new System.Drawing.Size(186, 27);
            this.txtParameter.TabIndex = 23;
            this.txtParameter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rdbParameter
            // 
            this.rdbParameter.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdbParameter.Location = new System.Drawing.Point(412, 32);
            this.rdbParameter.Name = "rdbParameter";
            this.rdbParameter.Size = new System.Drawing.Size(186, 21);
            this.rdbParameter.TabIndex = 22;
            this.rdbParameter.TabStop = true;
            this.rdbParameter.UseVisualStyleBackColor = true;
            this.rdbParameter.CheckedChanged += new System.EventHandler(this.rdbParameter_CheckedChanged);
            // 
            // txtVolume
            // 
            this.txtVolume.AutoSize = true;
            this.txtVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVolume.Location = new System.Drawing.Point(310, 57);
            this.txtVolume.Name = "txtVolume";
            this.txtVolume.Size = new System.Drawing.Size(95, 27);
            this.txtVolume.TabIndex = 16;
            this.txtVolume.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtArea
            // 
            this.txtArea.AutoSize = true;
            this.txtArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtArea.Location = new System.Drawing.Point(208, 57);
            this.txtArea.Name = "txtArea";
            this.txtArea.Size = new System.Drawing.Size(95, 27);
            this.txtArea.TabIndex = 16;
            this.txtArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtLength
            // 
            this.txtLength.AutoSize = true;
            this.txtLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLength.Location = new System.Drawing.Point(106, 57);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(95, 27);
            this.txtLength.TabIndex = 16;
            this.txtLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCount
            // 
            this.txtCount.AutoSize = true;
            this.txtCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCount.Location = new System.Drawing.Point(4, 57);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(95, 27);
            this.txtCount.TabIndex = 14;
            this.txtCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(310, 1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 27);
            this.label7.TabIndex = 14;
            this.label7.Text = "Vol. [m3]";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(208, 1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 27);
            this.label6.TabIndex = 14;
            this.label6.Text = "Area [m2]";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(106, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 27);
            this.label5.TabIndex = 14;
            this.label5.Text = "Largo [ml]";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(4, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 27);
            this.label4.TabIndex = 14;
            this.label4.Text = "Recuento";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rdbCant
            // 
            this.rdbCant.AutoSize = true;
            this.rdbCant.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbCant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdbCant.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbCant.Location = new System.Drawing.Point(4, 32);
            this.rdbCant.Name = "rdbCant";
            this.rdbCant.Size = new System.Drawing.Size(95, 21);
            this.rdbCant.TabIndex = 17;
            this.rdbCant.TabStop = true;
            this.rdbCant.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbCant.UseVisualStyleBackColor = true;
            this.rdbCant.CheckedChanged += new System.EventHandler(this.rdbCant_CheckedChanged);
            // 
            // rdbLargo
            // 
            this.rdbLargo.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbLargo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdbLargo.Location = new System.Drawing.Point(106, 32);
            this.rdbLargo.Name = "rdbLargo";
            this.rdbLargo.Size = new System.Drawing.Size(95, 21);
            this.rdbLargo.TabIndex = 18;
            this.rdbLargo.TabStop = true;
            this.rdbLargo.UseVisualStyleBackColor = true;
            this.rdbLargo.CheckedChanged += new System.EventHandler(this.rdbLargo_CheckedChanged);
            // 
            // rdbArea
            // 
            this.rdbArea.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdbArea.Location = new System.Drawing.Point(208, 32);
            this.rdbArea.Name = "rdbArea";
            this.rdbArea.Size = new System.Drawing.Size(95, 21);
            this.rdbArea.TabIndex = 19;
            this.rdbArea.TabStop = true;
            this.rdbArea.UseVisualStyleBackColor = true;
            this.rdbArea.CheckedChanged += new System.EventHandler(this.rdbArea_CheckedChanged);
            // 
            // rdbVolumen
            // 
            this.rdbVolumen.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbVolumen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdbVolumen.Location = new System.Drawing.Point(310, 32);
            this.rdbVolumen.Name = "rdbVolumen";
            this.rdbVolumen.Size = new System.Drawing.Size(95, 21);
            this.rdbVolumen.TabIndex = 20;
            this.rdbVolumen.TabStop = true;
            this.rdbVolumen.UseVisualStyleBackColor = true;
            this.rdbVolumen.CheckedChanged += new System.EventHandler(this.rdbVolumen_CheckedChanged);
            // 
            // cmbParametros
            // 
            this.cmbParametros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbParametros.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParametros.Enabled = false;
            this.cmbParametros.FormattingEnabled = true;
            this.cmbParametros.Location = new System.Drawing.Point(412, 4);
            this.cmbParametros.Name = "cmbParametros";
            this.cmbParametros.Size = new System.Drawing.Size(186, 26);
            this.cmbParametros.TabIndex = 21;
            this.cmbParametros.SelectedIndexChanged += new System.EventHandler(this.cmbParametros_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(9, 9);
            this.panel1.Margin = new System.Windows.Forms.Padding(9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(606, 89);
            this.panel1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(114, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 18);
            this.label1.TabIndex = 10;
            this.label1.Text = "Familia y Tipo:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(219, 12);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(388, 24);
            this.txtName.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(165, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 18);
            this.label3.TabIndex = 14;
            this.label3.Text = "Costo:";
            // 
            // txtCost
            // 
            this.txtCost.Location = new System.Drawing.Point(219, 70);
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(98, 24);
            this.txtCost.TabIndex = 1;
            this.txtCost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCost_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(127, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 18);
            this.label2.TabIndex = 13;
            this.label2.Text = "Descripción:";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescripcion.Location = new System.Drawing.Point(219, 41);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(388, 24);
            this.txtDescripcion.TabIndex = 0;
            // 
            // pic1
            // 
            this.pic1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pic1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pic1.ContextMenuStrip = this.mnuImage;
            this.pic1.Location = new System.Drawing.Point(9, 6);
            this.pic1.Margin = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this.pic1.Name = "pic1";
            this.pic1.Size = new System.Drawing.Size(90, 90);
            this.pic1.TabIndex = 15;
            this.pic1.TabStop = false;
            // 
            // mnuImage
            // 
            this.mnuImage.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.mnuImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSaveImage});
            this.mnuImage.Name = "mnuImage";
            this.mnuImage.Size = new System.Drawing.Size(180, 28);
            // 
            // mnuSaveImage
            // 
            this.mnuSaveImage.Image = global::Qex.Resource1.disk_black;
            this.mnuSaveImage.Name = "mnuSaveImage";
            this.mnuSaveImage.Size = new System.Drawing.Size(179, 24);
            this.mnuSaveImage.Text = "Guardar imagen";
            this.mnuSaveImage.Click += new System.EventHandler(this.mnuSaveImage_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(387, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(114, 29);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "GUARDAR";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(507, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 29);
            this.button1.TabIndex = 7;
            this.button1.Text = "CERRAR";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.Controls.Add(this.button1, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnOK, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnRecursos, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 299);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(624, 35);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // btnRecursos
            // 
            this.btnRecursos.BackColor = System.Drawing.Color.LightSalmon;
            this.btnRecursos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRecursos.FlatAppearance.BorderColor = System.Drawing.Color.IndianRed;
            this.btnRecursos.FlatAppearance.BorderSize = 2;
            this.btnRecursos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecursos.Image = global::Qex.Resource1.inbox_image24;
            this.btnRecursos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRecursos.Location = new System.Drawing.Point(3, 3);
            this.btnRecursos.Name = "btnRecursos";
            this.btnRecursos.Size = new System.Drawing.Size(144, 29);
            this.btnRecursos.TabIndex = 8;
            this.btnRecursos.Text = "Consumos";
            this.btnRecursos.UseVisualStyleBackColor = false;
            this.btnRecursos.Click += new System.EventHandler(this.btnRecursos_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(5, 184);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(624, 107);
            this.tableLayoutPanel4.TabIndex = 13;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel3, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel4, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 5;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(634, 339);
            this.tableLayoutPanel5.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(5, 2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(624, 30);
            this.label8.TabIndex = 0;
            this.label8.Text = "   Parámetros de Tipo";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(5, 149);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(624, 30);
            this.label9.TabIndex = 1;
            this.label9.Text = "   Seleccionar magnitud preferida (Los valores son sumatorias)";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.pic1);
            this.panel2.Controls.Add(this.txtName);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtCost);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtDescripcion);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(5, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(624, 107);
            this.panel2.TabIndex = 14;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.statusStrip1.Location = new System.Drawing.Point(0, 339);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(634, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // frmEditar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(634, 361);
            this.Controls.Add(this.tableLayoutPanel5);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 400);
            this.Name = "frmEditar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Editar Quantificación";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).EndInit();
            this.mnuImage.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCost;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label txtCount;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label txtVolume;
        private System.Windows.Forms.Label txtArea;
        private System.Windows.Forms.Label txtLength;
        private System.Windows.Forms.RadioButton rdbCant;
        private System.Windows.Forms.RadioButton rdbLargo;
        private System.Windows.Forms.RadioButton rdbArea;
        private System.Windows.Forms.RadioButton rdbVolumen;
        private System.Windows.Forms.PictureBox pic1;
        private System.Windows.Forms.ContextMenuStrip mnuImage;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveImage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ComboBox cmbParametros;
        private System.Windows.Forms.Label txtParameter;
        private System.Windows.Forms.RadioButton rdbParameter;
        private System.Windows.Forms.Button btnRecursos;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}