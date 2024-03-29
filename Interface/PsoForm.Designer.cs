﻿namespace Interface
{
    partial class PsoForm
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
            this.resultPictureBox = new System.Windows.Forms.PictureBox();
            this.startButton = new System.Windows.Forms.Button();
            this.particlePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.initialPictureBox = new System.Windows.Forms.PictureBox();
            this.particleSelectLabel = new System.Windows.Forms.Label();
            this.clustersNumeric = new System.Windows.Forms.NumericUpDown();
            this.clustersNumericLabel = new System.Windows.Forms.Label();
            this.initialPictureBoxLabel = new System.Windows.Forms.Label();
            this.resultPictureBoxLabel = new System.Windows.Forms.Label();
            this.particleNumeric = new System.Windows.Forms.NumericUpDown();
            this.iterationsNumeric = new System.Windows.Forms.NumericUpDown();
            this.iterationsNumericLabel = new System.Windows.Forms.Label();
            this.openFileButton = new System.Windows.Forms.Button();
            this.observableCheckBox = new System.Windows.Forms.CheckBox();
            this.seedLabel = new System.Windows.Forms.Label();
            this.socialLabel = new System.Windows.Forms.Label();
            this.seedComboBox = new System.Windows.Forms.ComboBox();
            this.socialComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.resultPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clustersNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.particleNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iterationsNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // resultPictureBox
            // 
            this.resultPictureBox.Location = new System.Drawing.Point(880, 241);
            this.resultPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.resultPictureBox.Name = "resultPictureBox";
            this.resultPictureBox.Size = new System.Drawing.Size(402, 285);
            this.resultPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.resultPictureBox.TabIndex = 0;
            this.resultPictureBox.TabStop = false;
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(286, 583);
            this.startButton.Margin = new System.Windows.Forms.Padding(4);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(100, 33);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // particlePanel
            // 
            this.particlePanel.Location = new System.Drawing.Point(422, 12);
            this.particlePanel.Name = "particlePanel";
            this.particlePanel.Size = new System.Drawing.Size(452, 620);
            this.particlePanel.TabIndex = 2;
            // 
            // initialPictureBox
            // 
            this.initialPictureBox.Location = new System.Drawing.Point(15, 241);
            this.initialPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.initialPictureBox.Name = "initialPictureBox";
            this.initialPictureBox.Size = new System.Drawing.Size(402, 285);
            this.initialPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.initialPictureBox.TabIndex = 5;
            this.initialPictureBox.TabStop = false;
            // 
            // particleSelectLabel
            // 
            this.particleSelectLabel.AutoSize = true;
            this.particleSelectLabel.Location = new System.Drawing.Point(164, 130);
            this.particleSelectLabel.Name = "particleSelectLabel";
            this.particleSelectLabel.Size = new System.Drawing.Size(59, 16);
            this.particleSelectLabel.TabIndex = 6;
            this.particleSelectLabel.Text = "Particles";
            // 
            // clustersNumeric
            // 
            this.clustersNumeric.Location = new System.Drawing.Point(8, 149);
            this.clustersNumeric.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.clustersNumeric.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.clustersNumeric.Name = "clustersNumeric";
            this.clustersNumeric.Size = new System.Drawing.Size(115, 22);
            this.clustersNumeric.TabIndex = 7;
            this.clustersNumeric.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.clustersNumeric.ValueChanged += new System.EventHandler(this.clustersNumeric_ValueChanged);
            // 
            // clustersNumericLabel
            // 
            this.clustersNumericLabel.AutoSize = true;
            this.clustersNumericLabel.Location = new System.Drawing.Point(29, 130);
            this.clustersNumericLabel.Name = "clustersNumericLabel";
            this.clustersNumericLabel.Size = new System.Drawing.Size(55, 16);
            this.clustersNumericLabel.TabIndex = 8;
            this.clustersNumericLabel.Text = "Clusters";
            // 
            // initialPictureBoxLabel
            // 
            this.initialPictureBoxLabel.AutoSize = true;
            this.initialPictureBoxLabel.Location = new System.Drawing.Point(151, 220);
            this.initialPictureBoxLabel.Name = "initialPictureBoxLabel";
            this.initialPictureBoxLabel.Size = new System.Drawing.Size(96, 16);
            this.initialPictureBoxLabel.TabIndex = 9;
            this.initialPictureBoxLabel.Text = "Original picture";
            // 
            // resultPictureBoxLabel
            // 
            this.resultPictureBoxLabel.AutoSize = true;
            this.resultPictureBoxLabel.Location = new System.Drawing.Point(1064, 220);
            this.resultPictureBoxLabel.Name = "resultPictureBoxLabel";
            this.resultPictureBoxLabel.Size = new System.Drawing.Size(45, 16);
            this.resultPictureBoxLabel.TabIndex = 10;
            this.resultPictureBoxLabel.Text = "Result";
            // 
            // particleNumeric
            // 
            this.particleNumeric.Location = new System.Drawing.Point(140, 149);
            this.particleNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.particleNumeric.Name = "particleNumeric";
            this.particleNumeric.Size = new System.Drawing.Size(115, 22);
            this.particleNumeric.TabIndex = 11;
            this.particleNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.particleNumeric.ValueChanged += new System.EventHandler(this.particleNumeric_ValueChanged);
            // 
            // iterationsNumeric
            // 
            this.iterationsNumeric.Location = new System.Drawing.Point(281, 149);
            this.iterationsNumeric.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.iterationsNumeric.Name = "iterationsNumeric";
            this.iterationsNumeric.Size = new System.Drawing.Size(115, 22);
            this.iterationsNumeric.TabIndex = 12;
            this.iterationsNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.iterationsNumeric.ValueChanged += new System.EventHandler(this.iterationsNumeric_ValueChanged);
            // 
            // iterationsNumericLabel
            // 
            this.iterationsNumericLabel.AutoSize = true;
            this.iterationsNumericLabel.Location = new System.Drawing.Point(306, 130);
            this.iterationsNumericLabel.Name = "iterationsNumericLabel";
            this.iterationsNumericLabel.Size = new System.Drawing.Size(61, 16);
            this.iterationsNumericLabel.TabIndex = 13;
            this.iterationsNumericLabel.Text = "Iterations";
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(37, 583);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(91, 33);
            this.openFileButton.TabIndex = 14;
            this.openFileButton.Text = "Open File";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // observableCheckBox
            // 
            this.observableCheckBox.AutoSize = true;
            this.observableCheckBox.Location = new System.Drawing.Point(286, 556);
            this.observableCheckBox.Name = "observableCheckBox";
            this.observableCheckBox.Size = new System.Drawing.Size(81, 20);
            this.observableCheckBox.TabIndex = 15;
            this.observableCheckBox.Text = "Observe";
            this.observableCheckBox.UseVisualStyleBackColor = true;
            // 
            // seedLabel
            // 
            this.seedLabel.AutoSize = true;
            this.seedLabel.Location = new System.Drawing.Point(12, 43);
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size(109, 16);
            this.seedLabel.TabIndex = 17;
            this.seedLabel.Text = "Seeding strategy";
            // 
            // socialLabel
            // 
            this.socialLabel.AutoSize = true;
            this.socialLabel.Location = new System.Drawing.Point(151, 43);
            this.socialLabel.Name = "socialLabel";
            this.socialLabel.Size = new System.Drawing.Size(96, 16);
            this.socialLabel.TabIndex = 19;
            this.socialLabel.Text = "Social strategy";
            // 
            // seedComboBox
            // 
            this.seedComboBox.DisplayMember = "Domain";
            this.seedComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.seedComboBox.FormattingEnabled = true;
            this.seedComboBox.Items.AddRange(new object[] {
            "Domain",
            "Dataset",
            "K-Means"});
            this.seedComboBox.Location = new System.Drawing.Point(8, 62);
            this.seedComboBox.Name = "seedComboBox";
            this.seedComboBox.Size = new System.Drawing.Size(115, 24);
            this.seedComboBox.TabIndex = 20;
            // 
            // socialComboBox
            // 
            this.socialComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.socialComboBox.FormattingEnabled = true;
            this.socialComboBox.Items.AddRange(new object[] {
            "gbest",
            "lbest - Social",
            "lbest - Geographic"});
            this.socialComboBox.Location = new System.Drawing.Point(140, 62);
            this.socialComboBox.Name = "socialComboBox";
            this.socialComboBox.Size = new System.Drawing.Size(115, 24);
            this.socialComboBox.TabIndex = 21;
            this.socialComboBox.SelectedIndexChanged += new System.EventHandler(this.socialComboBox_SelectedIndexChanged);
            // 
            // PsoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1296, 644);
            this.Controls.Add(this.socialComboBox);
            this.Controls.Add(this.seedComboBox);
            this.Controls.Add(this.socialLabel);
            this.Controls.Add(this.seedLabel);
            this.Controls.Add(this.observableCheckBox);
            this.Controls.Add(this.openFileButton);
            this.Controls.Add(this.iterationsNumericLabel);
            this.Controls.Add(this.iterationsNumeric);
            this.Controls.Add(this.particleNumeric);
            this.Controls.Add(this.resultPictureBoxLabel);
            this.Controls.Add(this.initialPictureBoxLabel);
            this.Controls.Add(this.clustersNumericLabel);
            this.Controls.Add(this.clustersNumeric);
            this.Controls.Add(this.particleSelectLabel);
            this.Controls.Add(this.initialPictureBox);
            this.Controls.Add(this.particlePanel);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.resultPictureBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PsoForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.resultPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clustersNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.particleNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iterationsNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox resultPictureBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.FlowLayoutPanel particlePanel;
        private System.Windows.Forms.PictureBox initialPictureBox;
        private System.Windows.Forms.Label particleSelectLabel;
        private System.Windows.Forms.NumericUpDown clustersNumeric;
        private System.Windows.Forms.Label clustersNumericLabel;
        private System.Windows.Forms.Label initialPictureBoxLabel;
        private System.Windows.Forms.Label resultPictureBoxLabel;
        private System.Windows.Forms.NumericUpDown particleNumeric;
        private System.Windows.Forms.NumericUpDown iterationsNumeric;
        private System.Windows.Forms.Label iterationsNumericLabel;
        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.CheckBox observableCheckBox;
        private System.Windows.Forms.Label seedLabel;
        private System.Windows.Forms.Label socialLabel;
        private System.Windows.Forms.ComboBox seedComboBox;
        private System.Windows.Forms.ComboBox socialComboBox;
    }
}

