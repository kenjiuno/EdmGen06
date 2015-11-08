﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EdmGen06UI {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            pwd.Items.Add(Environment.CurrentDirectory);
            pwd.Text = Environment.CurrentDirectory;
        }

        private void EFModelGen_Click(object sender, EventArgs e) {
            var p = new EdmGen06.EdmGenModelGen();
            String typeProviderServices = typeof(Npgsql.NpgsqlServices).ToString();
            p.ModelGen2(connectionString.Text, providerName.Text, typeProviderServices, modelName.Text, targetSchema.Text, Version.Parse(ver.Text));

            MessageBox.Show(this, "Done.\n\n" + modelName.Text + ".edmx generated", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            input_edmx.Text = modelName.Text + ".edmx";
            output_cs.Text = modelName.Text + ".cs";
        }

        private void EFCodeFirstGen_Click(object sender, EventArgs e) {
            var p = new EdmGen06.EdmGenClassGen();
            p.CodeFirstGen(input_edmx.Text, output_cs.Text, generator.Text);

            MessageBox.Show(this, "Done.\n\n" + output_cs.Text + " generated", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void input_edmx_TextChanged(object sender, EventArgs e) {

        }
    }
}
