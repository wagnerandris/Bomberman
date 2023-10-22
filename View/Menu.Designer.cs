namespace View
{
    partial class Menu
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
            start_button = new Button();
            prev_map = new Button();
            map_picture = new PictureBox();
            next_map = new Button();
            ((System.ComponentModel.ISupportInitialize)map_picture).BeginInit();
            SuspendLayout();
            // 
            // start_button
            // 
            start_button.Location = new Point(128, 64);
            start_button.Name = "start_button";
            start_button.Size = new Size(256, 64);
            start_button.TabIndex = 0;
            start_button.Text = "Start Game";
            start_button.UseVisualStyleBackColor = true;
            start_button.Click += start_button_Click;
            // 
            // prev_map
            // 
            prev_map.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            prev_map.Location = new Point(96, 288);
            prev_map.Margin = new Padding(0);
            prev_map.Name = "prev_map";
            prev_map.Size = new Size(32, 64);
            prev_map.TabIndex = 2;
            prev_map.UseVisualStyleBackColor = true;
            prev_map.Click += prev_map_Click;
            // 
            // map_picture
            // 
            map_picture.BorderStyle = BorderStyle.FixedSingle;
            map_picture.Location = new Point(128, 192);
            map_picture.Name = "map_picture";
            map_picture.Size = new Size(256, 256);
            map_picture.SizeMode = PictureBoxSizeMode.StretchImage;
            map_picture.TabIndex = 4;
            map_picture.TabStop = false;
            // 
            // next_map
            // 
            next_map.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            next_map.Location = new Point(384, 288);
            next_map.Margin = new Padding(0);
            next_map.Name = "next_map";
            next_map.Size = new Size(32, 64);
            next_map.TabIndex = 5;
            next_map.UseVisualStyleBackColor = true;
            next_map.Click += next_map_Click;
            // 
            // Menu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(next_map);
            Controls.Add(map_picture);
            Controls.Add(prev_map);
            Controls.Add(start_button);
            Name = "Menu";
            Size = new Size(512, 512);
            ((System.ComponentModel.ISupportInitialize)map_picture).EndInit();
            ResumeLayout(false);
        }


        #endregion

        private Button start_button;
        private Button prev_map;
        private PictureBox map_picture;
        private Button next_map;
    }
}
