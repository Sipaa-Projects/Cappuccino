namespace Cappuccino.Main
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            bootSpinnerControl1 = new BootSpinnerControl();
            SuspendLayout();
            // 
            // bootSpinnerControl1
            // 
            bootSpinnerControl1.AutoSize = true;
            bootSpinnerControl1.Font = null;
            bootSpinnerControl1.Location = new Point(357, 226);
            bootSpinnerControl1.Name = "bootSpinnerControl1";
            bootSpinnerControl1.Size = new Size(55, 50);
            bootSpinnerControl1.TabIndex = 0;
            bootSpinnerControl1.Text = null;
            bootSpinnerControl1.Theme = BootSpinnerTheme.Windows11;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 22F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(776, 501);
            Controls.Add(bootSpinnerControl1);
            Font = new Font("Segoe UI Variable Text Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private BootSpinnerControl bootSpinnerControl1;
    }
}
