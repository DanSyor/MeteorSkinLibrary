namespace MeteorSkinLibrary
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skinsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addForSelectedCharacterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetLibraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CharacterList = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SkinOriginText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SkinNameText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SlotNumberText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.selected_csp_name = new System.Windows.Forms.Label();
            this.remove_selected_csp = new System.Windows.Forms.Button();
            this.csps_ListView = new System.Windows.Forms.ListView();
            this.SkinListBox = new System.Windows.Forms.ListBox();
            this.skinboxlabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textConsole = new System.Windows.Forms.RichTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cleanWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.skinsToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(874, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.cleanWorkspaceToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.menu_software_exit);
            // 
            // skinsToolStripMenuItem
            // 
            this.skinsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addForSelectedCharacterToolStripMenuItem});
            this.skinsToolStripMenuItem.Name = "skinsToolStripMenuItem";
            this.skinsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.skinsToolStripMenuItem.Text = "Skins";
            // 
            // addForSelectedCharacterToolStripMenuItem
            // 
            this.addForSelectedCharacterToolStripMenuItem.Name = "addForSelectedCharacterToolStripMenuItem";
            this.addForSelectedCharacterToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.addForSelectedCharacterToolStripMenuItem.Text = "Add for selected Character";
            this.addForSelectedCharacterToolStripMenuItem.Click += new System.EventHandler(this.skin_add);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationToolStripMenuItem,
            this.resetLibraryToolStripMenuItem,
            this.resetWorkspaceToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.configurationToolStripMenuItem.Text = "Configuration";
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.menu_config);
            // 
            // resetLibraryToolStripMenuItem
            // 
            this.resetLibraryToolStripMenuItem.Name = "resetLibraryToolStripMenuItem";
            this.resetLibraryToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.resetLibraryToolStripMenuItem.Text = "Reset Library";
            this.resetLibraryToolStripMenuItem.Click += new System.EventHandler(this.menu_reset_library);
            // 
            // CharacterList
            // 
            this.CharacterList.FormattingEnabled = true;
            this.CharacterList.Location = new System.Drawing.Point(0, 53);
            this.CharacterList.Name = "CharacterList";
            this.CharacterList.Size = new System.Drawing.Size(120, 550);
            this.CharacterList.TabIndex = 1;
            this.CharacterList.SelectedIndexChanged += new System.EventHandler(this.character_selected);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(396, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(466, 372);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(458, 346);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Skin Info Editor";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.SkinOriginText);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.SkinNameText);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.SlotNumberText);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(446, 334);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Skin Information";
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(315, 168);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(125, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Delete Skin";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.skin_delete);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(161, 168);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(125, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Clean All Files";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 168);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Save Information";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.save_skin_info);
            // 
            // SkinOriginText
            // 
            this.SkinOriginText.Location = new System.Drawing.Point(87, 108);
            this.SkinOriginText.Name = "SkinOriginText";
            this.SkinOriginText.ReadOnly = true;
            this.SkinOriginText.Size = new System.Drawing.Size(188, 20);
            this.SkinOriginText.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Origin";
            // 
            // SkinNameText
            // 
            this.SkinNameText.Location = new System.Drawing.Point(87, 69);
            this.SkinNameText.Name = "SkinNameText";
            this.SkinNameText.Size = new System.Drawing.Size(188, 20);
            this.SkinNameText.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // SlotNumberText
            // 
            this.SlotNumberText.Location = new System.Drawing.Point(87, 33);
            this.SlotNumberText.Name = "SlotNumberText";
            this.SlotNumberText.ReadOnly = true;
            this.SlotNumberText.Size = new System.Drawing.Size(188, 20);
            this.SlotNumberText.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Slot Number";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(458, 346);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "File Manager";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(446, 538);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Information";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBox7);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Location = new System.Drawing.Point(7, 20);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(433, 60);
            this.groupBox5.TabIndex = 16;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Model";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(182, 20);
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(188, 20);
            this.textBox7.TabIndex = 8;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 27);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 13);
            this.label12.TabIndex = 8;
            this.label12.Text = "Model Status";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.button5);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.textBox5);
            this.groupBox4.Controls.Add(this.textBox6);
            this.groupBox4.Controls.Add(this.button4);
            this.groupBox4.Location = new System.Drawing.Point(9, 253);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(431, 71);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Import";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Model Folder (cXX)";
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(350, 38);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 13;
            this.button5.Text = "Move";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.batch_copy_csp);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 43);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(84, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "CSP Folder (chr)";
            // 
            // textBox5
            // 
            this.textBox5.AllowDrop = true;
            this.textBox5.Location = new System.Drawing.Point(132, 16);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(188, 20);
            this.textBox5.TabIndex = 8;
            this.textBox5.DragDrop += new System.Windows.Forms.DragEventHandler(this.model_DragDrop);
            this.textBox5.DragEnter += new System.Windows.Forms.DragEventHandler(this.model_DragEnter);
            // 
            // textBox6
            // 
            this.textBox6.AllowDrop = true;
            this.textBox6.Location = new System.Drawing.Point(132, 40);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(188, 20);
            this.textBox6.TabIndex = 11;
            this.textBox6.DragDrop += new System.Windows.Forms.DragEventHandler(this.csp_DragDrop2);
            this.textBox6.DragEnter += new System.Windows.Forms.DragEventHandler(this.csp_DragEnter2);
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(350, 14);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 12;
            this.button4.Text = "Move";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.TextChanged += new System.EventHandler(this.batch_move_model);
            this.button4.Click += new System.EventHandler(this.batch_move_model);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.selected_csp_name);
            this.groupBox3.Controls.Add(this.remove_selected_csp);
            this.groupBox3.Controls.Add(this.csps_ListView);
            this.groupBox3.Location = new System.Drawing.Point(9, 86);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(431, 161);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "CSP";
            // 
            // selected_csp_name
            // 
            this.selected_csp_name.AutoSize = true;
            this.selected_csp_name.Location = new System.Drawing.Point(6, 137);
            this.selected_csp_name.Name = "selected_csp_name";
            this.selected_csp_name.Size = new System.Drawing.Size(88, 13);
            this.selected_csp_name.TabIndex = 14;
            this.selected_csp_name.Text = "No selected CSP";
            // 
            // remove_selected_csp
            // 
            this.remove_selected_csp.Enabled = false;
            this.remove_selected_csp.Location = new System.Drawing.Point(273, 132);
            this.remove_selected_csp.Name = "remove_selected_csp";
            this.remove_selected_csp.Size = new System.Drawing.Size(152, 23);
            this.remove_selected_csp.TabIndex = 14;
            this.remove_selected_csp.Text = "Remove Selected CSP";
            this.remove_selected_csp.UseVisualStyleBackColor = true;
            this.remove_selected_csp.Click += new System.EventHandler(this.remove_selected_csp_Click);
            // 
            // csps_ListView
            // 
            this.csps_ListView.Location = new System.Drawing.Point(7, 19);
            this.csps_ListView.MultiSelect = false;
            this.csps_ListView.Name = "csps_ListView";
            this.csps_ListView.Size = new System.Drawing.Size(418, 109);
            this.csps_ListView.TabIndex = 0;
            this.csps_ListView.UseCompatibleStateImageBehavior = false;
            this.csps_ListView.SelectedIndexChanged += new System.EventHandler(this.csp_selected);
            // 
            // SkinListBox
            // 
            this.SkinListBox.FormattingEnabled = true;
            this.SkinListBox.Location = new System.Drawing.Point(126, 53);
            this.SkinListBox.Name = "SkinListBox";
            this.SkinListBox.Size = new System.Drawing.Size(264, 550);
            this.SkinListBox.TabIndex = 0;
            this.SkinListBox.SelectedIndexChanged += new System.EventHandler(this.skin_selected);
            // 
            // skinboxlabel
            // 
            this.skinboxlabel.AutoSize = true;
            this.skinboxlabel.Location = new System.Drawing.Point(243, 37);
            this.skinboxlabel.Name = "skinboxlabel";
            this.skinboxlabel.Size = new System.Drawing.Size(33, 13);
            this.skinboxlabel.TabIndex = 1;
            this.skinboxlabel.Text = "Skins";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Characters";
            // 
            // textConsole
            // 
            this.textConsole.Location = new System.Drawing.Point(400, 422);
            this.textConsole.Name = "textConsole";
            this.textConsole.Size = new System.Drawing.Size(458, 179);
            this.textConsole.TabIndex = 4;
            this.textConsole.Text = "";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(403, 406);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Console";
            // 
            // cleanWorkspaceToolStripMenuItem
            // 
            this.cleanWorkspaceToolStripMenuItem.Name = "cleanWorkspaceToolStripMenuItem";
            this.cleanWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.cleanWorkspaceToolStripMenuItem.Text = "Clean Workspace";
            // 
            // resetWorkspaceToolStripMenuItem
            // 
            this.resetWorkspaceToolStripMenuItem.Name = "resetWorkspaceToolStripMenuItem";
            this.resetWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.resetWorkspaceToolStripMenuItem.Text = "Reset Workspace";
            this.resetWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.resetWorkspaceToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 613);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textConsole);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.skinboxlabel);
            this.Controls.Add(this.CharacterList);
            this.Controls.Add(this.SkinListBox);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Mowjoh\'s Meteor Skin Library Alpha";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ListBox CharacterList;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox SkinListBox;
        private System.Windows.Forms.Label skinboxlabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox SkinOriginText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SkinNameText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SlotNumberText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetLibraryToolStripMenuItem;
        private System.Windows.Forms.RichTextBox textConsole;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolStripMenuItem skinsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addForSelectedCharacterToolStripMenuItem;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView csps_ListView;
        private System.Windows.Forms.Button remove_selected_csp;
        private System.Windows.Forms.Label selected_csp_name;
        private System.Windows.Forms.ToolStripMenuItem cleanWorkspaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetWorkspaceToolStripMenuItem;
    }
}

