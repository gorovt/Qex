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
    partial class frmOpciones
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOpciones));
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbPads = new System.Windows.Forms.ComboBox();
            this.cmbFoundations = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTechos = new System.Windows.Forms.ComboBox();
            this.cmbCubiertas = new System.Windows.Forms.ComboBox();
            this.cmbSuelos = new System.Windows.Forms.ComboBox();
            this.cmbMuros = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAyudaMontajes = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.chkMontajes = new System.Windows.Forms.CheckBox();
            this.cmbRebar = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkByPhase = new System.Windows.Forms.CheckBox();
            this.chkByLevel = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkGrupos = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.cmbPads);
            this.groupBox5.Controls.Add(this.cmbFoundations);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.cmbTechos);
            this.groupBox5.Controls.Add(this.cmbCubiertas);
            this.groupBox5.Controls.Add(this.cmbSuelos);
            this.groupBox5.Controls.Add(this.cmbMuros);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(345, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(336, 275);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Categorías con Capas de Materiales";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 189);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 15);
            this.label7.TabIndex = 11;
            this.label7.Text = "Plataformas:";
            // 
            // cmbPads
            // 
            this.cmbPads.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPads.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPads.FormattingEnabled = true;
            this.cmbPads.Location = new System.Drawing.Point(91, 186);
            this.cmbPads.Name = "cmbPads";
            this.cmbPads.Size = new System.Drawing.Size(237, 23);
            this.cmbPads.TabIndex = 10;
            this.cmbPads.SelectedIndexChanged += new System.EventHandler(this.cmbPads_SelectedIndexChanged);
            // 
            // cmbFoundations
            // 
            this.cmbFoundations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFoundations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFoundations.FormattingEnabled = true;
            this.cmbFoundations.Location = new System.Drawing.Point(145, 155);
            this.cmbFoundations.Name = "cmbFoundations";
            this.cmbFoundations.Size = new System.Drawing.Size(183, 23);
            this.cmbFoundations.TabIndex = 9;
            this.cmbFoundations.SelectedIndexChanged += new System.EventHandler(this.cmbFoundations_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "Losas de cimentación:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Techos:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Cubiertas:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Suelos:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Muros:";
            // 
            // cmbTechos
            // 
            this.cmbTechos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTechos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTechos.FormattingEnabled = true;
            this.cmbTechos.Location = new System.Drawing.Point(91, 122);
            this.cmbTechos.Name = "cmbTechos";
            this.cmbTechos.Size = new System.Drawing.Size(237, 23);
            this.cmbTechos.TabIndex = 3;
            this.cmbTechos.SelectedIndexChanged += new System.EventHandler(this.cmbTechos_SelectedIndexChanged);
            // 
            // cmbCubiertas
            // 
            this.cmbCubiertas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCubiertas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCubiertas.FormattingEnabled = true;
            this.cmbCubiertas.Location = new System.Drawing.Point(91, 91);
            this.cmbCubiertas.Name = "cmbCubiertas";
            this.cmbCubiertas.Size = new System.Drawing.Size(237, 23);
            this.cmbCubiertas.TabIndex = 2;
            this.cmbCubiertas.SelectedIndexChanged += new System.EventHandler(this.cmbCubiertas_SelectedIndexChanged);
            // 
            // cmbSuelos
            // 
            this.cmbSuelos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSuelos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSuelos.FormattingEnabled = true;
            this.cmbSuelos.Location = new System.Drawing.Point(91, 60);
            this.cmbSuelos.Name = "cmbSuelos";
            this.cmbSuelos.Size = new System.Drawing.Size(237, 23);
            this.cmbSuelos.TabIndex = 1;
            this.cmbSuelos.SelectedIndexChanged += new System.EventHandler(this.cmbSuelos_SelectedIndexChanged);
            // 
            // cmbMuros
            // 
            this.cmbMuros.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbMuros.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMuros.FormattingEnabled = true;
            this.cmbMuros.Location = new System.Drawing.Point(91, 29);
            this.cmbMuros.Name = "cmbMuros";
            this.cmbMuros.Size = new System.Drawing.Size(237, 23);
            this.cmbMuros.TabIndex = 0;
            this.cmbMuros.SelectedIndexChanged += new System.EventHandler(this.cmbMuros_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(581, 284);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 24);
            this.button2.TabIndex = 4;
            this.button2.Text = "GUARDAR";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAyudaMontajes);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.chkMontajes);
            this.groupBox1.Controls.Add(this.cmbRebar);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 171);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(336, 107);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Opciones especiales";
            // 
            // btnAyudaMontajes
            // 
            this.btnAyudaMontajes.Location = new System.Drawing.Point(248, 58);
            this.btnAyudaMontajes.Name = "btnAyudaMontajes";
            this.btnAyudaMontajes.Size = new System.Drawing.Size(23, 23);
            this.btnAyudaMontajes.TabIndex = 4;
            this.btnAyudaMontajes.Text = "?";
            this.btnAyudaMontajes.UseVisualStyleBackColor = true;
            this.btnAyudaMontajes.Click += new System.EventHandler(this.btnAyudaMontajes_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(41, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 15);
            this.label8.TabIndex = 3;
            this.label8.Text = "Montajes:";
            // 
            // chkMontajes
            // 
            this.chkMontajes.AutoSize = true;
            this.chkMontajes.Location = new System.Drawing.Point(108, 61);
            this.chkMontajes.Name = "chkMontajes";
            this.chkMontajes.Size = new System.Drawing.Size(134, 19);
            this.chkMontajes.TabIndex = 2;
            this.chkMontajes.Text = "Computar Montajes";
            this.chkMontajes.UseVisualStyleBackColor = true;
            this.chkMontajes.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // cmbRebar
            // 
            this.cmbRebar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbRebar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRebar.FormattingEnabled = true;
            this.cmbRebar.Location = new System.Drawing.Point(108, 29);
            this.cmbRebar.Name = "cmbRebar";
            this.cmbRebar.Size = new System.Drawing.Size(220, 23);
            this.cmbRebar.TabIndex = 1;
            this.cmbRebar.SelectedIndexChanged += new System.EventHandler(this.cmbRebar_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Armadura EST:";
            // 
            // chkByPhase
            // 
            this.chkByPhase.AutoSize = true;
            this.chkByPhase.Location = new System.Drawing.Point(31, 57);
            this.chkByPhase.Name = "chkByPhase";
            this.chkByPhase.Size = new System.Drawing.Size(189, 19);
            this.chkByPhase.TabIndex = 7;
            this.chkByPhase.Text = "Agrupar por Fase de Creación";
            this.chkByPhase.UseVisualStyleBackColor = true;
            this.chkByPhase.CheckedChanged += new System.EventHandler(this.chkByPhase_CheckedChanged);
            // 
            // chkByLevel
            // 
            this.chkByLevel.AutoSize = true;
            this.chkByLevel.Location = new System.Drawing.Point(31, 30);
            this.chkByLevel.Name = "chkByLevel";
            this.chkByLevel.Size = new System.Drawing.Size(133, 19);
            this.chkByLevel.TabIndex = 8;
            this.chkByLevel.Text = "Agrupar por Niveles";
            this.chkByLevel.UseVisualStyleBackColor = true;
            this.chkByLevel.CheckedChanged += new System.EventHandler(this.chkByLevel_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkGrupos);
            this.groupBox2.Controls.Add(this.chkByPhase);
            this.groupBox2.Controls.Add(this.chkByLevel);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(336, 162);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Opciones de agrupación";
            // 
            // chkGrupos
            // 
            this.chkGrupos.AutoSize = true;
            this.chkGrupos.Location = new System.Drawing.Point(31, 86);
            this.chkGrupos.Name = "chkGrupos";
            this.chkGrupos.Size = new System.Drawing.Size(195, 19);
            this.chkGrupos.TabIndex = 2;
            this.chkGrupos.Text = "Agrupar por Grupos de Modelo";
            this.chkGrupos.UseVisualStyleBackColor = true;
            this.chkGrupos.CheckedChanged += new System.EventHandler(this.chkGrupos_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox5, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(684, 311);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(342, 281);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // frmOpciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 311);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOpciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Opciones Generales";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmOpciones_FormClosed);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTechos;
        private System.Windows.Forms.ComboBox cmbCubiertas;
        private System.Windows.Forms.ComboBox cmbSuelos;
        private System.Windows.Forms.ComboBox cmbMuros;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbRebar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbFoundations;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbPads;
        private System.Windows.Forms.CheckBox chkByPhase;
        private System.Windows.Forms.CheckBox chkByLevel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox chkGrupos;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkMontajes;
        private System.Windows.Forms.Button btnAyudaMontajes;
    }
}