using System.Collections;
using System.Linq;
using System.Windows.Forms;

namespace AracListesi
{
    partial class FrmArac
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.propertyGrid1 = new ExPropertyGrid();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(128, 384);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Tamam";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OkClicked);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(244, 384);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 30);
            this.button2.TabIndex = 2;
            this.button2.Text = "İptal";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.CancelClicked);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(12, 12);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid1.Size = new System.Drawing.Size(342, 366);
            this.propertyGrid1.TabIndex = 0;
            // 
            // FrmArac
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 426);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.propertyGrid1);
            this.Name = "FrmArac";
            this.Text = "FrmArac";
            this.Load += new System.EventHandler(this.FrmPerson_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ExPropertyGrid propertyGrid1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

public class ExPropertyGrid : PropertyGrid
{
    protected override bool ProcessTabKey(bool forward)
    {
        var grid = this.Controls[2];
        var field = grid.GetType().GetField("allGridEntries",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);
        var entries = (field.GetValue(grid) as IEnumerable).Cast<GridItem>().ToList();
        var index = entries.IndexOf(this.SelectedGridItem);

        if (forward && index < entries.Count - 1)
        {
            var next = entries[index + 1];
            next.Select();
            return true;
        }
        return base.ProcessTabKey(forward);
    }
}