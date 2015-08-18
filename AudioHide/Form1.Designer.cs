namespace AudioHide
{
    partial class AudioHide
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.InputURL = new System.Windows.Forms.TextBox();
            this.KeyURL = new System.Windows.Forms.TextBox();
            this.Decode = new System.Windows.Forms.Button();
            this.Encode = new System.Windows.Forms.Button();
            this.EmbededData = new System.Windows.Forms.TextBox();
            this.Method = new System.Windows.Forms.ComboBox();
            this.OutURL = new System.Windows.Forms.TextBox();
            this.Noise = new System.Windows.Forms.ComboBox();
            this.OutData = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // InputURL
            // 
            this.InputURL.Location = new System.Drawing.Point(29, 31);
            this.InputURL.Name = "InputURL";
            this.InputURL.Size = new System.Drawing.Size(391, 21);
            this.InputURL.TabIndex = 0;
            this.InputURL.Text = "C:\\Users\\BIT\\Documents\\programming\\AudioHide\\AudioHide\\TestCase\\Audio\\record01.wa" +
    "v";
            this.InputURL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // KeyURL
            // 
            this.KeyURL.Location = new System.Drawing.Point(29, 67);
            this.KeyURL.Name = "KeyURL";
            this.KeyURL.Size = new System.Drawing.Size(391, 21);
            this.KeyURL.TabIndex = 1;
            this.KeyURL.Text = "C:\\Users\\BIT\\Documents\\programming\\AudioHide\\AudioHide\\TestCase\\Audio\\record01.mp" +
    "3";
            // 
            // Decode
            // 
            this.Decode.Location = new System.Drawing.Point(541, 102);
            this.Decode.Name = "Decode";
            this.Decode.Size = new System.Drawing.Size(86, 94);
            this.Decode.TabIndex = 2;
            this.Decode.Text = "Decode";
            this.Decode.UseVisualStyleBackColor = true;
            this.Decode.Click += new System.EventHandler(this.Decode_Click);
            // 
            // Encode
            // 
            this.Encode.Location = new System.Drawing.Point(441, 102);
            this.Encode.Name = "Encode";
            this.Encode.Size = new System.Drawing.Size(86, 94);
            this.Encode.TabIndex = 3;
            this.Encode.Text = "Encode";
            this.Encode.UseVisualStyleBackColor = true;
            this.Encode.Click += new System.EventHandler(this.Ecnode_Click);
            // 
            // EmbededData
            // 
            this.EmbededData.Location = new System.Drawing.Point(29, 137);
            this.EmbededData.Name = "EmbededData";
            this.EmbededData.Size = new System.Drawing.Size(391, 21);
            this.EmbededData.TabIndex = 4;
            this.EmbededData.Text = "salyu";
            // 
            // Method
            // 
            this.Method.FormattingEnabled = true;
            this.Method.Items.AddRange(new object[] {
            "1. LSB",
            "2. MCLT"});
            this.Method.Location = new System.Drawing.Point(441, 32);
            this.Method.Name = "Method";
            this.Method.Size = new System.Drawing.Size(186, 20);
            this.Method.TabIndex = 6;
            this.Method.Text = "1. LSB";
            // 
            // OutURL
            // 
            this.OutURL.Location = new System.Drawing.Point(29, 102);
            this.OutURL.Name = "OutURL";
            this.OutURL.Size = new System.Drawing.Size(391, 21);
            this.OutURL.TabIndex = 7;
            this.OutURL.Text = "C:\\Users\\BIT\\Documents\\programming\\AudioHide\\AudioHide\\TestCase\\Out\\Out01.wav";
            // 
            // Noise
            // 
            this.Noise.FormattingEnabled = true;
            this.Noise.Items.AddRange(new object[] {
            "1. None",
            "2. Gauss",
            "3. Random"});
            this.Noise.Location = new System.Drawing.Point(441, 68);
            this.Noise.Name = "Noise";
            this.Noise.Size = new System.Drawing.Size(186, 20);
            this.Noise.TabIndex = 10;
            this.Noise.Text = "1. None";
            // 
            // OutData
            // 
            this.OutData.Location = new System.Drawing.Point(29, 173);
            this.OutData.Name = "OutData";
            this.OutData.Size = new System.Drawing.Size(391, 21);
            this.OutData.TabIndex = 11;
            // 
            // AudioHide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 213);
            this.Controls.Add(this.OutData);
            this.Controls.Add(this.Noise);
            this.Controls.Add(this.OutURL);
            this.Controls.Add(this.Method);
            this.Controls.Add(this.EmbededData);
            this.Controls.Add(this.Encode);
            this.Controls.Add(this.Decode);
            this.Controls.Add(this.KeyURL);
            this.Controls.Add(this.InputURL);
            this.Name = "AudioHide";
            this.Text = "AudioHide";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public static string storage = "C:\\Users\\BIT\\Documents\\programming\\AudioHide\\AudioHide\\TestCase\\";
        public static string _instorage = "\\TestCase\\";
        public static string _outstorage = "\\TestCase\\";
        public static string _keystorage = "\\TestCase\\";
        private System.Windows.Forms.TextBox InputURL;
        private System.Windows.Forms.TextBox KeyURL;
        private System.Windows.Forms.Button Decode;
        private System.Windows.Forms.Button Encode;
        private System.Windows.Forms.TextBox EmbededData;
        private System.Windows.Forms.ComboBox Method;
        private System.Windows.Forms.TextBox OutURL;
        private System.Windows.Forms.ComboBox Noise;
        private System.Windows.Forms.TextBox OutData;
    }
}

