
namespace GCController
{
    partial class Form2
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
            Sgry.Azuki.FontInfo fontInfo1 = new Sgry.Azuki.FontInfo();
            this.azukiControl1 = new Sgry.Azuki.WinForms.AzukiControl();
            this.SuspendLayout();
            // 
            // azukiControl1
            // 
            this.azukiControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.azukiControl1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.azukiControl1.DrawingOption = ((Sgry.Azuki.DrawingOption)(((((Sgry.Azuki.DrawingOption.DrawsFullWidthSpace | Sgry.Azuki.DrawingOption.HighlightCurrentLine) 
            | Sgry.Azuki.DrawingOption.ShowsLineNumber) 
            | Sgry.Azuki.DrawingOption.ShowsDirtBar) 
            | Sgry.Azuki.DrawingOption.HighlightsMatchedBracket)));
            this.azukiControl1.DrawsEolCode = false;
            this.azukiControl1.DrawsTab = false;
            this.azukiControl1.FirstVisibleLine = 0;
            this.azukiControl1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            fontInfo1.Name = "Consolas";
            fontInfo1.Size = 9;
            fontInfo1.Style = System.Drawing.FontStyle.Regular;
            this.azukiControl1.FontInfo = fontInfo1;
            this.azukiControl1.ForeColor = System.Drawing.Color.Black;
            this.azukiControl1.Location = new System.Drawing.Point(12, 12);
            this.azukiControl1.Name = "azukiControl1";
            this.azukiControl1.ScrollPos = new System.Drawing.Point(0, 0);
            this.azukiControl1.Size = new System.Drawing.Size(558, 426);
            this.azukiControl1.TabIndex = 0;
            this.azukiControl1.Text = "azukiControl1";
            this.azukiControl1.ViewWidth = 4136;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.azukiControl1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);

        }

        #endregion

        private Sgry.Azuki.WinForms.AzukiControl azukiControl1;
    }
}