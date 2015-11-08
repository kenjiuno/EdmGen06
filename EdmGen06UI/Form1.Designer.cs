namespace EdmGen06UI {
    partial class Form1 {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EFModelGen = new System.Windows.Forms.Button();
            this.ver = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.targetSchema = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.modelName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.providerName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.connectionString = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.EFCodeFirstGen = new System.Windows.Forms.Button();
            this.generator = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.output_cs = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.input_edmx = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pwd = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tb = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.EFModelGen);
            this.groupBox1.Controls.Add(this.ver);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.targetSchema);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.modelName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.providerName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.connectionString);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(759, 159);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "EFModelGen (Generate .edmx file from PostgreSQL database)";
            // 
            // EFModelGen
            // 
            this.EFModelGen.Location = new System.Drawing.Point(26, 119);
            this.EFModelGen.Name = "EFModelGen";
            this.EFModelGen.Size = new System.Drawing.Size(75, 23);
            this.EFModelGen.TabIndex = 10;
            this.EFModelGen.Text = "Run";
            this.EFModelGen.UseVisualStyleBackColor = true;
            this.EFModelGen.Click += new System.EventHandler(this.EFModelGen_Click);
            // 
            // ver
            // 
            this.ver.FormattingEnabled = true;
            this.ver.Items.AddRange(new object[] {
            "3.0",
            "1.0"});
            this.ver.Location = new System.Drawing.Point(676, 80);
            this.ver.Name = "ver";
            this.ver.Size = new System.Drawing.Size(77, 23);
            this.ver.TabIndex = 9;
            this.ver.Text = "3.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(673, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "ver";
            // 
            // targetSchema
            // 
            this.targetSchema.FormattingEnabled = true;
            this.targetSchema.Location = new System.Drawing.Point(461, 80);
            this.targetSchema.Name = "targetSchema";
            this.targetSchema.Size = new System.Drawing.Size(209, 23);
            this.targetSchema.TabIndex = 7;
            this.targetSchema.Text = "public";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(458, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "targetSchema";
            // 
            // modelName
            // 
            this.modelName.FormattingEnabled = true;
            this.modelName.Location = new System.Drawing.Point(225, 80);
            this.modelName.Name = "modelName";
            this.modelName.Size = new System.Drawing.Size(230, 23);
            this.modelName.TabIndex = 5;
            this.modelName.Text = "npgsql_tests";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "modelName";
            // 
            // providerName
            // 
            this.providerName.FormattingEnabled = true;
            this.providerName.Items.AddRange(new object[] {
            "Npgsql",
            "System.Data.SqlClient"});
            this.providerName.Location = new System.Drawing.Point(26, 80);
            this.providerName.Name = "providerName";
            this.providerName.Size = new System.Drawing.Size(189, 23);
            this.providerName.TabIndex = 3;
            this.providerName.Text = "Npgsql";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "providerName";
            // 
            // connectionString
            // 
            this.connectionString.FormattingEnabled = true;
            this.connectionString.Location = new System.Drawing.Point(26, 36);
            this.connectionString.Name = "connectionString";
            this.connectionString.Size = new System.Drawing.Size(727, 23);
            this.connectionString.TabIndex = 1;
            this.connectionString.Text = "Port=5678;Host=127.0.0.1;Username=npgsql_tests;Password=npgsql_tests;Database=npg" +
    "sql_tests";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Npgsql ConnectionString";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.EFCodeFirstGen);
            this.groupBox2.Controls.Add(this.generator);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.output_cs);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.input_edmx);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(12, 221);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(759, 117);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "EFCodeFirstGen (Generate .cs file from .edmx)";
            // 
            // EFCodeFirstGen
            // 
            this.EFCodeFirstGen.Location = new System.Drawing.Point(26, 79);
            this.EFCodeFirstGen.Name = "EFCodeFirstGen";
            this.EFCodeFirstGen.Size = new System.Drawing.Size(75, 23);
            this.EFCodeFirstGen.TabIndex = 6;
            this.EFCodeFirstGen.Text = "Run";
            this.EFCodeFirstGen.UseVisualStyleBackColor = true;
            this.EFCodeFirstGen.Click += new System.EventHandler(this.EFCodeFirstGen_Click);
            // 
            // generator
            // 
            this.generator.FormattingEnabled = true;
            this.generator.Items.AddRange(new object[] {
            "DbContext.EFv6",
            "DbContext.EFv5",
            "ObjectContext"});
            this.generator.Location = new System.Drawing.Point(540, 36);
            this.generator.Name = "generator";
            this.generator.Size = new System.Drawing.Size(213, 23);
            this.generator.TabIndex = 5;
            this.generator.Text = "DbContext.EFv6";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(537, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 15);
            this.label9.TabIndex = 4;
            this.label9.Text = "generator";
            // 
            // output_cs
            // 
            this.output_cs.FormattingEnabled = true;
            this.output_cs.Location = new System.Drawing.Point(283, 36);
            this.output_cs.Name = "output_cs";
            this.output_cs.Size = new System.Drawing.Size(251, 23);
            this.output_cs.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(280, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 15);
            this.label8.TabIndex = 2;
            this.label8.Text = "output.cs";
            // 
            // input_edmx
            // 
            this.input_edmx.FormattingEnabled = true;
            this.input_edmx.Location = new System.Drawing.Point(26, 36);
            this.input_edmx.Name = "input_edmx";
            this.input_edmx.Size = new System.Drawing.Size(251, 23);
            this.input_edmx.TabIndex = 1;
            this.input_edmx.TextChanged += new System.EventHandler(this.input_edmx_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "input.edmx";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 15);
            this.label6.TabIndex = 4;
            this.label6.Text = "Working directory:";
            // 
            // pwd
            // 
            this.pwd.FormattingEnabled = true;
            this.pwd.Location = new System.Drawing.Point(12, 27);
            this.pwd.Name = "pwd";
            this.pwd.Size = new System.Drawing.Size(759, 23);
            this.pwd.TabIndex = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tb);
            this.groupBox3.Location = new System.Drawing.Point(12, 344);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(759, 167);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log";
            // 
            // tb
            // 
            this.tb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb.Location = new System.Drawing.Point(3, 18);
            this.tb.Name = "tb";
            this.tb.Size = new System.Drawing.Size(753, 146);
            this.tb.TabIndex = 0;
            this.tb.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 523);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.pwd);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EdmGen06 for Npgsql";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox connectionString;
        private System.Windows.Forms.ComboBox providerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox modelName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox targetSchema;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ver;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox pwd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox input_edmx;
        private System.Windows.Forms.ComboBox output_cs;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox generator;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button EFModelGen;
        private System.Windows.Forms.Button EFCodeFirstGen;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox tb;
    }
}

