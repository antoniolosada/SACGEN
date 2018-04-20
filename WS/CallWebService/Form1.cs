using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

//imporntant el NAmeSpace del WEb sErvice
using rnd = CallWebService.RandomNumber;

namespace CallWebService
{
	/// <summary>
	/// Descripción breve de Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtMin;
		private System.Windows.Forms.TextBox txtMax;
		private System.Windows.Forms.TextBox txtCount;
		private System.Windows.Forms.Button cmdRnd;
		private System.Windows.Forms.ListBox lstNums;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		/// <summary>
		/// Variable del diseñador requerida.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Necesario para admitir el Diseñador de Windows Forms
			//
			InitializeComponent();

			//
			// TODO: agregar código de constructor después de llamar a InitializeComponent
			//
		}

		/// <summary>
		/// Limpiar los recursos que se estén utilizando.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Código generado por el Diseñador de Windows Forms
		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtMin = new System.Windows.Forms.TextBox();
			this.txtMax = new System.Windows.Forms.TextBox();
			this.txtCount = new System.Windows.Forms.TextBox();
			this.cmdRnd = new System.Windows.Forms.Button();
			this.lstNums = new System.Windows.Forms.ListBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtMin
			// 
			this.txtMin.Location = new System.Drawing.Point(184, 24);
			this.txtMin.Name = "txtMin";
			this.txtMin.Size = new System.Drawing.Size(88, 20);
			this.txtMin.TabIndex = 0;
			this.txtMin.Text = "";
			// 
			// txtMax
			// 
			this.txtMax.Location = new System.Drawing.Point(184, 64);
			this.txtMax.Name = "txtMax";
			this.txtMax.Size = new System.Drawing.Size(88, 20);
			this.txtMax.TabIndex = 1;
			this.txtMax.Text = "";
			// 
			// txtCount
			// 
			this.txtCount.Location = new System.Drawing.Point(184, 104);
			this.txtCount.Name = "txtCount";
			this.txtCount.Size = new System.Drawing.Size(88, 20);
			this.txtCount.TabIndex = 2;
			this.txtCount.Text = "";
			// 
			// cmdRnd
			// 
			this.cmdRnd.Location = new System.Drawing.Point(64, 248);
			this.cmdRnd.Name = "cmdRnd";
			this.cmdRnd.Size = new System.Drawing.Size(112, 24);
			this.cmdRnd.TabIndex = 3;
			this.cmdRnd.Text = "Random";
			this.cmdRnd.Click += new System.EventHandler(this.cmdRnd_Click);
			// 
			// lstNums
			// 
			this.lstNums.Location = new System.Drawing.Point(224, 200);
			this.lstNums.Name = "lstNums";
			this.lstNums.Size = new System.Drawing.Size(200, 134);
			this.lstNums.TabIndex = 4;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.txtMin);
			this.panel1.Controls.Add(this.txtMax);
			this.panel1.Controls.Add(this.txtCount);
			this.panel1.Location = new System.Drawing.Point(80, 16);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(376, 152);
			this.panel1.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(48, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 24);
			this.label2.TabIndex = 1;
			this.label2.Text = "Maximo Numero";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(48, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Minimo Numero";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(48, 104);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 24);
			this.label3.TabIndex = 1;
			this.label3.Text = "Cantidad de Numeros";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(528, 358);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lstNums);
			this.Controls.Add(this.cmdRnd);
			this.Name = "Form1";
			this.Text = "Call Web Service Random -- Sergio Tarrillo -- sergiomanc@yahoo.es";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Punto de entrada principal de la aplicación.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		//variables globales
		public int[] numbers;

		private void cmdRnd_Click(object sender, System.EventArgs e) {
			
			//varaibles de uso
			int num, min, max;

			//metiendole una excepcion por siaca
			//haya error
			try {
				num = Convert.ToInt16(txtCount.Text);
				min = Convert.ToInt16(txtMin.Text);
				max = Convert.ToInt16(txtMax.Text);

				//reservando el tamaño del arreglo
				numbers = new int[num];
		
				//Declarando el WEbSErvice
				rnd.Generator wsRnd = new rnd.Generator();

				//CONSUMIENDO EL WEB SERVICE
				numbers = wsRnd.GenerateRandomDotOrg(min,max,numbers.Length);

			} catch (Exception ex) {
				MessageBox.Show("ocurrio un error: " + ex.Message.ToString());
			}
		
	
			//ahora llamar a la funcion que llena el listBox
			this.LlenarListBox();
		}

		//funcion para llenar el listBox
		public void LlenarListBox() {
			//limpiando la lista
			lstNums.Items.Clear();

			//llenando el ListBox
			for ( int i=0 ; i< numbers.Length; i++){
				lstNums.Items.Add(numbers[i]);
			}
		}

		private void Form1_Load(object sender, System.EventArgs e) {
		
		}
	}
}
