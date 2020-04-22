namespace WindowsFormsApp4
{
    partial class AddBeaconForm
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
            this.beaconListCB = new System.Windows.Forms.ComboBox();
            this.confirmBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // beaconListCB
            // 
            this.beaconListCB.FormattingEnabled = true;
            this.beaconListCB.Location = new System.Drawing.Point(12, 27);
            this.beaconListCB.Name = "beaconListCB";
            this.beaconListCB.Size = new System.Drawing.Size(271, 23);
            this.beaconListCB.TabIndex = 0;
            // 
            // confirmBtn
            // 
            this.confirmBtn.Location = new System.Drawing.Point(207, 68);
            this.confirmBtn.Name = "confirmBtn";
            this.confirmBtn.Size = new System.Drawing.Size(75, 23);
            this.confirmBtn.TabIndex = 1;
            this.confirmBtn.Text = "確認";
            this.confirmBtn.UseVisualStyleBackColor = true;
            this.confirmBtn.Click += new System.EventHandler(this.confirmBtn_Click);
            // 
            // AddBeaconForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 106);
            this.Controls.Add(this.confirmBtn);
            this.Controls.Add(this.beaconListCB);
            this.Name = "AddBeaconForm";
            this.Text = "AddBeaconForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox beaconListCB;
        private System.Windows.Forms.Button confirmBtn;
    }
}