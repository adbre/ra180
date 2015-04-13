namespace Ra180.Client.WinForms
{
    partial class Dart380Control
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.synchronizationContextTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // synchronizationContextTimer
            // 
            this.synchronizationContextTimer.Enabled = true;
            this.synchronizationContextTimer.Interval = 500;
            this.synchronizationContextTimer.Tick += new System.EventHandler(this.synchronizationContextTimer_Tick);
            // 
            // Dart380Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.DoubleBuffered = true;
            this.Name = "Dart380Control";
            this.Size = new System.Drawing.Size(672, 476);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer synchronizationContextTimer;
    }
}
