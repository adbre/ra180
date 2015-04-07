namespace Ra180.Client.WinForms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dart380Control1 = new global::Ra180.Client.WinForms.Dart380Control();
            this.SuspendLayout();
            // 
            // dart380Control1
            // 
            this.dart380Control1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("dart380Control1.BackgroundImage")));
            this.dart380Control1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.dart380Control1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dart380Control1.Location = new System.Drawing.Point(0, 0);
            this.dart380Control1.Name = "dart380Control1";
            this.dart380Control1.Size = new System.Drawing.Size(1096, 863);
            this.dart380Control1.TabIndex = 0;
            // 
            // Dart380Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1096, 863);
            this.Controls.Add(this.dart380Control1);
            this.DoubleBuffered = true;
            this.Name = "Dart380Form";
            this.Text = "Ra180/Dart380";
            this.ResumeLayout(false);

        }

        #endregion

        private Dart380Control dart380Control1;
    }
}

