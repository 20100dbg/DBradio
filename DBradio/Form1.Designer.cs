
namespace DBradio
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.b_import = new System.Windows.Forms.Button();
            this.b_createTable = new System.Windows.Forms.Button();
            this.tb_log = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tb_sql = new System.Windows.Forms.TextBox();
            this.b_execSQL = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // b_import
            // 
            this.b_import.Location = new System.Drawing.Point(419, 415);
            this.b_import.Name = "b_import";
            this.b_import.Size = new System.Drawing.Size(75, 23);
            this.b_import.TabIndex = 0;
            this.b_import.Text = "Import";
            this.b_import.UseVisualStyleBackColor = true;
            this.b_import.Click += new System.EventHandler(this.b_import_Click);
            // 
            // b_createTable
            // 
            this.b_createTable.Location = new System.Drawing.Point(419, 374);
            this.b_createTable.Name = "b_createTable";
            this.b_createTable.Size = new System.Drawing.Size(75, 23);
            this.b_createTable.TabIndex = 1;
            this.b_createTable.Text = "Creer tables";
            this.b_createTable.UseVisualStyleBackColor = true;
            this.b_createTable.Click += new System.EventHandler(this.b_createTable_Click);
            // 
            // tb_log
            // 
            this.tb_log.Location = new System.Drawing.Point(29, 346);
            this.tb_log.Multiline = true;
            this.tb_log.Name = "tb_log";
            this.tb_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_log.Size = new System.Drawing.Size(360, 92);
            this.tb_log.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(29, 87);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(542, 253);
            this.dataGridView1.TabIndex = 3;
            // 
            // tb_sql
            // 
            this.tb_sql.Location = new System.Drawing.Point(29, 24);
            this.tb_sql.Multiline = true;
            this.tb_sql.Name = "tb_sql";
            this.tb_sql.Size = new System.Drawing.Size(542, 57);
            this.tb_sql.TabIndex = 4;
            // 
            // b_execSQL
            // 
            this.b_execSQL.Location = new System.Drawing.Point(597, 24);
            this.b_execSQL.Name = "b_execSQL";
            this.b_execSQL.Size = new System.Drawing.Size(75, 23);
            this.b_execSQL.TabIndex = 5;
            this.b_execSQL.Text = "Executer";
            this.b_execSQL.UseVisualStyleBackColor = true;
            this.b_execSQL.Click += new System.EventHandler(this.b_execSQL_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.b_execSQL);
            this.Controls.Add(this.tb_sql);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.tb_log);
            this.Controls.Add(this.b_createTable);
            this.Controls.Add(this.b_import);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button b_import;
        private System.Windows.Forms.Button b_createTable;
        private System.Windows.Forms.TextBox tb_log;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox tb_sql;
        private System.Windows.Forms.Button b_execSQL;
    }
}

