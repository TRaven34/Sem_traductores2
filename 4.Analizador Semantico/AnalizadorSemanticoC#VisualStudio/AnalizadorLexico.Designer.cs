namespace Analizador_Lexico__Traductor_
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.traducirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.limpiarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TBCode = new System.Windows.Forms.TextBox();
            this.DGWarnings = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.DGVariables = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.DGSintaxis = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.LBCode = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.DGErrores = new System.Windows.Forms.DataGridView();
            this.panel8 = new System.Windows.Forms.Panel();
            this.Menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGWarnings)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVariables)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGSintaxis)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGErrores)).BeginInit();
            this.SuspendLayout();
            // 
            // Menu
            // 
            this.Menu.BackColor = System.Drawing.Color.SteelBlue;
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.traducirToolStripMenuItem,
            this.limpiarToolStripMenuItem});
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(1335, 24);
            this.Menu.TabIndex = 2;
            this.Menu.Text = "Menu";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrirToolStripMenuItem1,
            this.guardarToolStripMenuItem});
            this.archivoToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // abrirToolStripMenuItem1
            // 
            this.abrirToolStripMenuItem1.Name = "abrirToolStripMenuItem1";
            this.abrirToolStripMenuItem1.Size = new System.Drawing.Size(116, 22);
            this.abrirToolStripMenuItem1.Text = "Abrir";
            this.abrirToolStripMenuItem1.Click += new System.EventHandler(this.abrirToolStripMenuItem1_Click);
            // 
            // guardarToolStripMenuItem
            // 
            this.guardarToolStripMenuItem.Name = "guardarToolStripMenuItem";
            this.guardarToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.guardarToolStripMenuItem.Text = "Guardar";
            this.guardarToolStripMenuItem.Click += new System.EventHandler(this.guardarToolStripMenuItem_Click);
            // 
            // traducirToolStripMenuItem
            // 
            this.traducirToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.traducirToolStripMenuItem.Name = "traducirToolStripMenuItem";
            this.traducirToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.traducirToolStripMenuItem.Text = "Compilar";
            this.traducirToolStripMenuItem.Click += new System.EventHandler(this.traducirToolStripMenuItem_Click);
            // 
            // limpiarToolStripMenuItem
            // 
            this.limpiarToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.limpiarToolStripMenuItem.Name = "limpiarToolStripMenuItem";
            this.limpiarToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.limpiarToolStripMenuItem.Text = "Limpiar";
            this.limpiarToolStripMenuItem.Click += new System.EventHandler(this.limpiarToolStripMenuItem_Click);
            // 
            // TBCode
            // 
            this.TBCode.Location = new System.Drawing.Point(304, 78);
            this.TBCode.Multiline = true;
            this.TBCode.Name = "TBCode";
            this.TBCode.Size = new System.Drawing.Size(726, 304);
            this.TBCode.TabIndex = 3;
            // 
            // DGWarnings
            // 
            this.DGWarnings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DGWarnings.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(38)))), ((int)(((byte)(45)))));
            this.DGWarnings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGWarnings.Location = new System.Drawing.Point(13, 14);
            this.DGWarnings.Name = "DGWarnings";
            this.DGWarnings.RowTemplate.Height = 25;
            this.DGWarnings.Size = new System.Drawing.Size(621, 155);
            this.DGWarnings.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(37)))), ((int)(((byte)(45)))));
            this.panel1.Controls.Add(this.panel11);
            this.panel1.Controls.Add(this.panel10);
            this.panel1.Controls.Add(this.panel9);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.TBCode);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.DGErrores);
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1335, 642);
            this.panel1.TabIndex = 8;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.SteelBlue;
            this.panel11.Controls.Add(this.label4);
            this.panel11.Location = new System.Drawing.Point(193, 403);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(257, 37);
            this.panel11.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Bahnschrift", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(80, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 29);
            this.label4.TabIndex = 0;
            this.label4.Text = "Errores:";
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.SteelBlue;
            this.panel10.Controls.Add(this.label3);
            this.panel10.Location = new System.Drawing.Point(862, 403);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(257, 37);
            this.panel10.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Bahnschrift", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(53, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 29);
            this.label3.TabIndex = 0;
            this.label3.Text = "Advertencias:";
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.SteelBlue;
            this.panel9.Controls.Add(this.DGWarnings);
            this.panel9.Location = new System.Drawing.Point(681, 450);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(644, 182);
            this.panel9.TabIndex = 10;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.SteelBlue;
            this.panel7.Controls.Add(this.label2);
            this.panel7.Location = new System.Drawing.Point(1058, 68);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(257, 37);
            this.panel7.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Bahnschrift", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(77, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "Variables:";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.SteelBlue;
            this.panel6.Controls.Add(this.label1);
            this.panel6.Location = new System.Drawing.Point(19, 67);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(254, 37);
            this.panel6.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bahnschrift", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(42, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Analisis Lexico:";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.SteelBlue;
            this.panel5.Controls.Add(this.DGVariables);
            this.panel5.Location = new System.Drawing.Point(1047, 110);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(278, 283);
            this.panel5.TabIndex = 11;
            // 
            // DGVariables
            // 
            this.DGVariables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DGVariables.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(37)))), ((int)(((byte)(45)))));
            this.DGVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVariables.Location = new System.Drawing.Point(11, 12);
            this.DGVariables.Name = "DGVariables";
            this.DGVariables.RowTemplate.Height = 25;
            this.DGVariables.Size = new System.Drawing.Size(257, 260);
            this.DGVariables.TabIndex = 8;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.SteelBlue;
            this.panel4.Controls.Add(this.DGSintaxis);
            this.panel4.Location = new System.Drawing.Point(10, 110);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(278, 285);
            this.panel4.TabIndex = 10;
            // 
            // DGSintaxis
            // 
            this.DGSintaxis.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DGSintaxis.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(37)))), ((int)(((byte)(45)))));
            this.DGSintaxis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGSintaxis.Location = new System.Drawing.Point(9, 12);
            this.DGSintaxis.Name = "DGSintaxis";
            this.DGSintaxis.RowTemplate.Height = 25;
            this.DGSintaxis.Size = new System.Drawing.Size(254, 259);
            this.DGSintaxis.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.SteelBlue;
            this.panel3.Controls.Add(this.LBCode);
            this.panel3.Location = new System.Drawing.Point(591, 27);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(188, 37);
            this.panel3.TabIndex = 9;
            // 
            // LBCode
            // 
            this.LBCode.AutoSize = true;
            this.LBCode.Font = new System.Drawing.Font("Bahnschrift", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LBCode.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LBCode.Location = new System.Drawing.Point(52, 3);
            this.LBCode.Name = "LBCode";
            this.LBCode.Size = new System.Drawing.Size(91, 29);
            this.LBCode.TabIndex = 0;
            this.LBCode.Text = "Codigo:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SteelBlue;
            this.panel2.Location = new System.Drawing.Point(294, 68);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(745, 327);
            this.panel2.TabIndex = 8;
            // 
            // DGErrores
            // 
            this.DGErrores.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DGErrores.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(38)))), ((int)(((byte)(45)))));
            this.DGErrores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGErrores.Location = new System.Drawing.Point(19, 463);
            this.DGErrores.Name = "DGErrores";
            this.DGErrores.RowTemplate.Height = 25;
            this.DGErrores.Size = new System.Drawing.Size(633, 155);
            this.DGErrores.TabIndex = 5;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.SteelBlue;
            this.panel8.Location = new System.Drawing.Point(10, 449);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(654, 182);
            this.panel8.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1335, 642);
            this.Controls.Add(this.Menu);
            this.Controls.Add(this.panel1);
            this.MainMenuStrip = this.Menu;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Analizador Lexico";
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGWarnings)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVariables)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGSintaxis)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGErrores)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MenuStrip Menu;
        private ToolStripMenuItem archivoToolStripMenuItem;
        private ToolStripMenuItem abrirToolStripMenuItem1;
        private ToolStripMenuItem traducirToolStripMenuItem;
        private ToolStripMenuItem limpiarToolStripMenuItem;
        private TextBox TBCode;
        private ToolStripMenuItem guardarToolStripMenuItem;
        private DataGridView DGWarnings;
        private Panel panel1;
        private Panel panel3;
        private Label LBCode;
        private Panel panel2;
        private Panel panel4;
        private DataGridView DGSintaxis;
        private Panel panel5;
        private DataGridView DGVariables;
        private Panel panel6;
        private Label label1;
        private Panel panel7;
        private Label label2;
        private Panel panel11;
        private Label label4;
        private Panel panel10;
        private Label label3;
        private Panel panel9;
        private DataGridView DGErrores;
        private Panel panel8;
    }
}