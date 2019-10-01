namespace ImageLocalizer
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
            this.selectFolderButton = new System.Windows.Forms.Button();
            this.selectedFolderList = new System.Windows.Forms.ListBox();
            this.constructDBButton = new System.Windows.Forms.Button();
            this.evaluatePerformanceButton = new System.Windows.Forms.Button();
            this.localizeButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.trainedPlaceList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.deleteClickedFolderButton = new System.Windows.Forms.Button();
            this.LocalizeVideoButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // selectFolderButton
            // 
            this.selectFolderButton.Location = new System.Drawing.Point(428, 27);
            this.selectFolderButton.Name = "selectFolderButton";
            this.selectFolderButton.Size = new System.Drawing.Size(156, 40);
            this.selectFolderButton.TabIndex = 0;
            this.selectFolderButton.Text = "Select Folder";
            this.selectFolderButton.UseVisualStyleBackColor = true;
            this.selectFolderButton.Click += new System.EventHandler(this.selectFolderButton_Click);
            // 
            // selectedFolderList
            // 
            this.selectedFolderList.FormattingEnabled = true;
            this.selectedFolderList.Location = new System.Drawing.Point(12, 26);
            this.selectedFolderList.Name = "selectedFolderList";
            this.selectedFolderList.Size = new System.Drawing.Size(410, 95);
            this.selectedFolderList.TabIndex = 1;
            // 
            // constructDBButton
            // 
            this.constructDBButton.Enabled = false;
            this.constructDBButton.Location = new System.Drawing.Point(428, 117);
            this.constructDBButton.Name = "constructDBButton";
            this.constructDBButton.Size = new System.Drawing.Size(156, 40);
            this.constructDBButton.TabIndex = 2;
            this.constructDBButton.Text = "Construct DB";
            this.constructDBButton.UseVisualStyleBackColor = true;
            this.constructDBButton.Click += new System.EventHandler(this.constructDBButton_Click);
            // 
            // evaluatePerformanceButton
            // 
            this.evaluatePerformanceButton.Enabled = false;
            this.evaluatePerformanceButton.Location = new System.Drawing.Point(428, 162);
            this.evaluatePerformanceButton.Name = "evaluatePerformanceButton";
            this.evaluatePerformanceButton.Size = new System.Drawing.Size(156, 40);
            this.evaluatePerformanceButton.TabIndex = 3;
            this.evaluatePerformanceButton.Text = "Performance Evaluation";
            this.evaluatePerformanceButton.UseVisualStyleBackColor = true;
            this.evaluatePerformanceButton.Click += new System.EventHandler(this.EvaluatePerformanceButton_Click);
            // 
            // localizeButton
            // 
            this.localizeButton.Enabled = false;
            this.localizeButton.Location = new System.Drawing.Point(428, 207);
            this.localizeButton.Name = "localizeButton";
            this.localizeButton.Size = new System.Drawing.Size(156, 40);
            this.localizeButton.TabIndex = 4;
            this.localizeButton.Text = "Localize Image";
            this.localizeButton.UseVisualStyleBackColor = true;
            this.localizeButton.Click += new System.EventHandler(this.localizeButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 309);
            this.progressBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(572, 23);
            this.progressBar.TabIndex = 5;
            // 
            // trainedPlaceList
            // 
            this.trainedPlaceList.FormattingEnabled = true;
            this.trainedPlaceList.Location = new System.Drawing.Point(12, 146);
            this.trainedPlaceList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.trainedPlaceList.Name = "trainedPlaceList";
            this.trainedPlaceList.Size = new System.Drawing.Size(410, 147);
            this.trainedPlaceList.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 127);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Trained Places";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Selected Folders";
            // 
            // deleteClickedFolderButton
            // 
            this.deleteClickedFolderButton.Location = new System.Drawing.Point(428, 72);
            this.deleteClickedFolderButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.deleteClickedFolderButton.Name = "deleteClickedFolderButton";
            this.deleteClickedFolderButton.Size = new System.Drawing.Size(156, 40);
            this.deleteClickedFolderButton.TabIndex = 9;
            this.deleteClickedFolderButton.Text = "Delete Clicked Folder";
            this.deleteClickedFolderButton.UseVisualStyleBackColor = true;
            this.deleteClickedFolderButton.Click += new System.EventHandler(this.deleteClickedFolderButton_Click);
            // 
            // LocalizeVideoButton
            // 
            this.LocalizeVideoButton.Location = new System.Drawing.Point(428, 253);
            this.LocalizeVideoButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LocalizeVideoButton.Name = "LocalizeVideoButton";
            this.LocalizeVideoButton.Size = new System.Drawing.Size(156, 39);
            this.LocalizeVideoButton.TabIndex = 10;
            this.LocalizeVideoButton.Text = "Localize Video";
            this.LocalizeVideoButton.UseVisualStyleBackColor = true;
            this.LocalizeVideoButton.Click += new System.EventHandler(this.LocalizeVideoButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 339);
            this.Controls.Add(this.LocalizeVideoButton);
            this.Controls.Add(this.deleteClickedFolderButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trainedPlaceList);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.localizeButton);
            this.Controls.Add(this.evaluatePerformanceButton);
            this.Controls.Add(this.constructDBButton);
            this.Controls.Add(this.selectedFolderList);
            this.Controls.Add(this.selectFolderButton);
            this.Name = "Form1";
            this.Text = "Localizer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button selectFolderButton;
        private System.Windows.Forms.ListBox selectedFolderList;
        private System.Windows.Forms.Button constructDBButton;
        private System.Windows.Forms.Button evaluatePerformanceButton;
        private System.Windows.Forms.Button localizeButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ListBox trainedPlaceList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button deleteClickedFolderButton;
        private System.Windows.Forms.Button LocalizeVideoButton;
    }
}

