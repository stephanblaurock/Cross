namespace CrossUtilsTest {
	partial class Form1 {
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent() {
			this._Ausgabe = new DevExpress.XtraEditors.MemoEdit();
			this._ButtGetUser = new DevExpress.XtraEditors.SimpleButton();
			this._ButtGetStempelzeiten = new DevExpress.XtraEditors.SimpleButton();
			((System.ComponentModel.ISupportInitialize)(this._Ausgabe.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// _Ausgabe
			// 
			this._Ausgabe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._Ausgabe.Location = new System.Drawing.Point(315, 12);
			this._Ausgabe.Name = "_Ausgabe";
			this._Ausgabe.Size = new System.Drawing.Size(1187, 1016);
			this._Ausgabe.TabIndex = 0;
			// 
			// _ButtGetUser
			// 
			this._ButtGetUser.Location = new System.Drawing.Point(12, 13);
			this._ButtGetUser.Name = "_ButtGetUser";
			this._ButtGetUser.Size = new System.Drawing.Size(297, 36);
			this._ButtGetUser.TabIndex = 1;
			this._ButtGetUser.Text = "GetUser";
			this._ButtGetUser.Click += new System.EventHandler(this._ButtGetUser_Click);
			// 
			// _ButtGetStempelzeiten
			// 
			this._ButtGetStempelzeiten.Location = new System.Drawing.Point(12, 55);
			this._ButtGetStempelzeiten.Name = "_ButtGetStempelzeiten";
			this._ButtGetStempelzeiten.Size = new System.Drawing.Size(297, 36);
			this._ButtGetStempelzeiten.TabIndex = 2;
			this._ButtGetStempelzeiten.Text = "GetStempelzeiten";
			this._ButtGetStempelzeiten.Click += new System.EventHandler(this._ButtGetStempelzeiten_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1514, 1040);
			this.Controls.Add(this._ButtGetStempelzeiten);
			this.Controls.Add(this._ButtGetUser);
			this.Controls.Add(this._Ausgabe);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this._Ausgabe.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.MemoEdit _Ausgabe;
		private DevExpress.XtraEditors.SimpleButton _ButtGetUser;
		private DevExpress.XtraEditors.SimpleButton _ButtGetStempelzeiten;
	}
}

