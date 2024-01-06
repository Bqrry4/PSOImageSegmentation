namespace Interface
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
            this.button1 = new System.Windows.Forms.Button();
            this.particlePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.particleSelect = new System.Windows.Forms.ComboBox();
            this.initialPictureBox = new System.Windows.Forms.PictureBox();
            this.particleSelectLabel = new System.Windows.Forms.Label();
            this.clustersNumeric = new System.Windows.Forms.NumericUpDown();
            this.clustersNumericLabel = new System.Windows.Forms.Label();
            this.initialPictureBoxLabel = new System.Windows.Forms.Label();
            this.resultPictureBoxLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.resultPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clustersNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // resultPictureBox
            // 
            this.resultPictureBox.Location = new System.Drawing.Point(881, 169);
            this.resultPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.resultPictureBox.Name = "resultPictureBox";
            this.resultPictureBox.Size = new System.Drawing.Size(402, 285);
            this.resultPictureBox.TabIndex = 0;
            this.resultPictureBox.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(145, 567);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // particlePanel
            // 
            this.particlePanel.Location = new System.Drawing.Point(422, 12);
            this.particlePanel.Name = "particlePanel";
            this.particlePanel.Size = new System.Drawing.Size(452, 620);
            this.particlePanel.TabIndex = 2;
            // 
            // particleSelect
            // 
            this.particleSelect.FormattingEnabled = true;
            this.particleSelect.Location = new System.Drawing.Point(23, 98);
            this.particleSelect.Name = "particleSelect";
            this.particleSelect.Size = new System.Drawing.Size(123, 24);
            this.particleSelect.TabIndex = 4;
            // 
            // initialPictureBox
            // 
            this.initialPictureBox.Location = new System.Drawing.Point(13, 169);
            this.initialPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.initialPictureBox.Name = "initialPictureBox";
            this.initialPictureBox.Size = new System.Drawing.Size(402, 285);
            this.initialPictureBox.TabIndex = 5;
            this.initialPictureBox.TabStop = false;
            // 
            // particleSelectLabel
            // 
            this.particleSelectLabel.AutoSize = true;
            this.particleSelectLabel.Location = new System.Drawing.Point(23, 79);
            this.particleSelectLabel.Name = "particleSelectLabel";
            this.particleSelectLabel.Size = new System.Drawing.Size(123, 16);
            this.particleSelectLabel.TabIndex = 6;
            this.particleSelectLabel.Text = "Number of particles";
            // 
            // clustersNumeric
            // 
            this.clustersNumeric.Location = new System.Drawing.Point(257, 100);
            this.clustersNumeric.Name = "clustersNumeric";
            this.clustersNumeric.Size = new System.Drawing.Size(115, 22);
            this.clustersNumeric.TabIndex = 7;
            // 
            // clustersNumericLabel
            // 
            this.clustersNumericLabel.AutoSize = true;
            this.clustersNumericLabel.Location = new System.Drawing.Point(254, 79);
            this.clustersNumericLabel.Name = "clustersNumericLabel";
            this.clustersNumericLabel.Size = new System.Drawing.Size(118, 16);
            this.clustersNumericLabel.TabIndex = 8;
            this.clustersNumericLabel.Text = "Number of clusters";
            // 
            // initialPictureBoxLabel
            // 
            this.initialPictureBoxLabel.AutoSize = true;
            this.initialPictureBoxLabel.Location = new System.Drawing.Point(149, 149);
            this.initialPictureBoxLabel.Name = "initialPictureBoxLabel";
            this.initialPictureBoxLabel.Size = new System.Drawing.Size(96, 16);
            this.initialPictureBoxLabel.TabIndex = 9;
            this.initialPictureBoxLabel.Text = "Original picture";
            // 
            // resultPictureBoxLabel
            // 
            this.resultPictureBoxLabel.AutoSize = true;
            this.resultPictureBoxLabel.Location = new System.Drawing.Point(1036, 148);
            this.resultPictureBoxLabel.Name = "resultPictureBoxLabel";
            this.resultPictureBoxLabel.Size = new System.Drawing.Size(45, 16);
            this.resultPictureBoxLabel.TabIndex = 10;
            this.resultPictureBoxLabel.Text = "Result";
            // 
            // PsoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1296, 644);
            this.Controls.Add(this.resultPictureBoxLabel);
            this.Controls.Add(this.initialPictureBoxLabel);
            this.Controls.Add(this.clustersNumericLabel);
            this.Controls.Add(this.clustersNumeric);
            this.Controls.Add(this.particleSelectLabel);
            this.Controls.Add(this.initialPictureBox);
            this.Controls.Add(this.particleSelect);
            this.Controls.Add(this.particlePanel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.resultPictureBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PsoForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.resultPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clustersNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox resultPictureBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FlowLayoutPanel particlePanel;
        private System.Windows.Forms.ComboBox particleSelect;
        private System.Windows.Forms.PictureBox initialPictureBox;
        private System.Windows.Forms.Label particleSelectLabel;
        private System.Windows.Forms.NumericUpDown clustersNumeric;
        private System.Windows.Forms.Label clustersNumericLabel;
        private System.Windows.Forms.Label initialPictureBoxLabel;
        private System.Windows.Forms.Label resultPictureBoxLabel;
    }
}

