﻿namespace DesktopInteropTest
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
            hwndHostControl1 = new Cappuccino.DesktopInterop.Controls.WinForms.HwndHostControl();
            SuspendLayout();
            // 
            // hwndHostControl1
            // 
            hwndHostControl1.Dock = DockStyle.Fill;
            hwndHostControl1.Location = new Point(0, 0);
            hwndHostControl1.Name = "hwndHostControl1";
            hwndHostControl1.Size = new Size(800, 450);
            hwndHostControl1.TabIndex = 0;
            hwndHostControl1.Text = "hwndHostControl1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(hwndHostControl1);
            Name = "Form1";
            Text = "Cappuccino.DesktopInterop test";
            ResumeLayout(false);
        }

        #endregion

        private Cappuccino.DesktopInterop.Controls.WinForms.HwndHostControl hwndHostControl1;
    }
}
