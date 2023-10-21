namespace View
{
    partial class GameForm
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
            tile_map = new TableLayoutPanel();
            menu = new Menu();
            status_strip = new StatusStrip();
            time_label = new ToolStripStatusLabel();
            destroyed_label = new ToolStripStatusLabel();
            status_strip.SuspendLayout();
            SuspendLayout();
            // 
            // tile_map
            // 
            tile_map.AutoSize = true;
            tile_map.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tile_map.BackColor = Color.Transparent;
            tile_map.ColumnCount = 1;
            tile_map.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tile_map.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tile_map.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tile_map.Location = new Point(0, 0);
            tile_map.Margin = new Padding(0);
            tile_map.Name = "tile_map";
            tile_map.RowCount = 1;
            tile_map.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tile_map.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tile_map.Size = new Size(0, 0);
            tile_map.TabIndex = 0;
            tile_map.Visible = false;
            // 
            // menu
            // 
            menu.BackColor = Color.Transparent;
            menu.Location = new Point(0, 0);
            menu.Margin = new Padding(0);
            menu.Name = "menu";
            menu.Size = new Size(512, 512);
            menu.TabIndex = 1;
            // 
            // status_strip
            // 
            status_strip.Items.AddRange(new ToolStripItem[] { time_label, destroyed_label });
            status_strip.Location = new Point(0, 512);
            status_strip.Name = "status_strip";
            status_strip.Size = new Size(512, 22);
            status_strip.TabIndex = 2;
            status_strip.Text = "statusStrip1";
            // 
            // time_label
            // 
            time_label.Name = "time_label";
            time_label.Size = new Size(39, 17);
            time_label.Text = "Time: ";
            // 
            // destroyed_label
            // 
            destroyed_label.Name = "destroyed_label";
            destroyed_label.Size = new Size(112, 17);
            destroyed_label.Text = "Enemies destroyed: ";
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.stars_background;
            ClientSize = new Size(512, 534);
            Controls.Add(status_strip);
            Controls.Add(menu);
            Controls.Add(tile_map);
            KeyPreview = true;
            MaximumSize = new Size(528, 573);
            MinimumSize = new Size(528, 573);
            Name = "GameForm";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Bomberman";
            status_strip.ResumeLayout(false);
            status_strip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tile_map;
        private Menu menu;
        private StatusStrip status_strip;
        private ToolStripStatusLabel time_label;
        private ToolStripStatusLabel destroyed_label;
    }
}