Imports Rnd = vbCallWebService.RandomNumber

Public Class FrmCallWS
    Inherits System.Windows.Forms.Form

#Region " Código generado por el Diseñador de Windows Forms "

    Public Sub New()
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()

    End Sub

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
    'Puede modificarse utilizando el Diseñador de Windows Forms. 
    'No lo modifique con el editor de código.
    Friend WithEvents panel1 As System.Windows.Forms.Panel
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents txtMin As System.Windows.Forms.TextBox
    Friend WithEvents txtMax As System.Windows.Forms.TextBox
    Friend WithEvents txtCount As System.Windows.Forms.TextBox
    Friend WithEvents lstNums As System.Windows.Forms.ListBox
    Friend WithEvents cmdRnd As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.panel1 = New System.Windows.Forms.Panel
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.label3 = New System.Windows.Forms.Label
        Me.txtMin = New System.Windows.Forms.TextBox
        Me.txtMax = New System.Windows.Forms.TextBox
        Me.txtCount = New System.Windows.Forms.TextBox
        Me.lstNums = New System.Windows.Forms.ListBox
        Me.cmdRnd = New System.Windows.Forms.Button
        Me.panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.label2)
        Me.panel1.Controls.Add(Me.label1)
        Me.panel1.Controls.Add(Me.label3)
        Me.panel1.Controls.Add(Me.txtMin)
        Me.panel1.Controls.Add(Me.txtMax)
        Me.panel1.Controls.Add(Me.txtCount)
        Me.panel1.Location = New System.Drawing.Point(72, 40)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(376, 152)
        Me.panel1.TabIndex = 8
        '
        'label2
        '
        Me.label2.Location = New System.Drawing.Point(48, 64)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(88, 24)
        Me.label2.TabIndex = 1
        Me.label2.Text = "Maximo Numero"
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(48, 24)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(88, 24)
        Me.label1.TabIndex = 0
        Me.label1.Text = "Minimo Numero"
        '
        'label3
        '
        Me.label3.Location = New System.Drawing.Point(48, 104)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(128, 24)
        Me.label3.TabIndex = 1
        Me.label3.Text = "Cantidad de Numeros"
        '
        'txtMin
        '
        Me.txtMin.Location = New System.Drawing.Point(184, 24)
        Me.txtMin.Name = "txtMin"
        Me.txtMin.Size = New System.Drawing.Size(88, 20)
        Me.txtMin.TabIndex = 0
        Me.txtMin.Text = ""
        '
        'txtMax
        '
        Me.txtMax.Location = New System.Drawing.Point(184, 64)
        Me.txtMax.Name = "txtMax"
        Me.txtMax.Size = New System.Drawing.Size(88, 20)
        Me.txtMax.TabIndex = 1
        Me.txtMax.Text = ""
        '
        'txtCount
        '
        Me.txtCount.Location = New System.Drawing.Point(184, 104)
        Me.txtCount.Name = "txtCount"
        Me.txtCount.Size = New System.Drawing.Size(88, 20)
        Me.txtCount.TabIndex = 2
        Me.txtCount.Text = ""
        '
        'lstNums
        '
        Me.lstNums.Location = New System.Drawing.Point(216, 224)
        Me.lstNums.Name = "lstNums"
        Me.lstNums.Size = New System.Drawing.Size(200, 134)
        Me.lstNums.TabIndex = 7
        '
        'cmdRnd
        '
        Me.cmdRnd.Location = New System.Drawing.Point(56, 272)
        Me.cmdRnd.Name = "cmdRnd"
        Me.cmdRnd.Size = New System.Drawing.Size(112, 24)
        Me.cmdRnd.TabIndex = 6
        Me.cmdRnd.Text = "Random"
        '
        'FrmCallWS
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(512, 390)
        Me.Controls.Add(Me.panel1)
        Me.Controls.Add(Me.lstNums)
        Me.Controls.Add(Me.cmdRnd)
        Me.Name = "FrmCallWS"
        Me.Text = "FrmCallWS"
        Me.panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    '//variables globales
    Public numbers As Integer()

    '//funcion para llenar el listBox
    Public Sub LlenarListBox()

        'limpiando el ListBox
        lstNums.Items.Clear()

        '//llenando el ListBox
        For i As Integer = 0 To numbers.Length - 1
            lstNums.Items.Add(numbers(i))
        Next

    End Sub


    Private Sub cmdRnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRnd.Click
        '//varaibles de uso
        Dim num, min, max As Integer

        '//metiendole una excepcion por siaca
        '//haya error
        Try
            num = Convert.ToInt16(txtCount.Text)
            min = Convert.ToInt16(txtMin.Text)
            max = Convert.ToInt16(txtMax.Text)

            '//reservando el tamaño del arreglo
            ReDim numbers(num)

            '//Declarando el WEbSErvice
            Dim wsRnd As New Rnd.Generator

            '//CONSUMIENDO EL WEB SERVICE
            numbers = wsRnd.GenerateRandomDotOrg(min, max, num)

        Catch ex As Exception
            MessageBox.Show("ocurrio un error: " + ex.Message.ToString())
        End Try

        '//ahora llamar a la funcion que llena el listBox
        Me.LlenarListBox()

    End Sub
End Class
