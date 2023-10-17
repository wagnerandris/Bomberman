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
            menu.Name = "menu";
            menu.Size = new Size(512, 512);
            menu.TabIndex = 1;
            // 
            // Bomberman
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.stars_background;
            ClientSize = new Size(512, 512);
            Controls.Add(menu);
            Controls.Add(tile_map);
            MaximumSize = new Size(528, 551);
            MinimumSize = new Size(528, 551);
            Name = "Bomberman";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Bomberman";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tile_map;
        private Menu menu;
    }
}