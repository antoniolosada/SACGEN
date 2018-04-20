<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAgrupacionCursos
#Region "Upgrade Support "
	Private Shared m_vb6FormDefInstance As frmAgrupacionCursos
	Private Shared m_InitializingDefInstance As Boolean
	Public Shared Property DefInstance() As frmAgrupacionCursos
		Get
			If m_vb6FormDefInstance Is Nothing OrElse m_vb6FormDefInstance.IsDisposed Then
				m_InitializingDefInstance = True
				m_vb6FormDefInstance = CreateInstance()
				m_InitializingDefInstance = False
			End If
			Return m_vb6FormDefInstance
		End Get
		Set(ByVal Value As frmAgrupacionCursos)
			m_vb6FormDefInstance = Value
		End Set
	End Property
#End Region
#Region "Windows Form Designer generated code "
	Public Shared Function CreateInstance() As frmAgrupacionCursos
		Dim theInstance As frmAgrupacionCursos = New frmAgrupacionCursos()
		theInstance.Form_Load()
		Return theInstance
	End Function
	Private visualControls() As String = New String() {"components", "ToolTipMain", "chkElimCursos", "chkCargaServ", "Frame", "pbCursos", "Entidades", "tbNumAgrupaciones", "Timer1", "cmdGenerarGrupos", "tbNumGrupos", "cmdPropagarGrupos", "cmdCopiarCursos", "tbTimeFin", "tbTime", "tbProceso", "tbHuecos", "tbValor", "Text2", "Text1", "cmdSim", "cmdAgrupar", "cmdCorregirCursos", "cmdGenerarListasPalabras", "Command1", "tbActual", "cmdAgruparCursos", "pbTotal", "Label7", "Label6", "Label5", "Label4", "Label3", "Label2", "Label1", "listBoxComboBoxHelper1"}
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTipMain As System.Windows.Forms.ToolTip
	Public WithEvents chkElimCursos As System.Windows.Forms.CheckBox
	Public WithEvents chkCargaServ As System.Windows.Forms.CheckBox
	Public WithEvents Frame As System.Windows.Forms.GroupBox
	Public WithEvents pbCursos As System.Windows.Forms.ProgressBar
	Public WithEvents Entidades As System.Windows.Forms.ComboBox
	Public WithEvents tbNumAgrupaciones As System.Windows.Forms.TextBox
	Public WithEvents Timer1 As System.Windows.Forms.Timer
	Public WithEvents cmdGenerarGrupos As System.Windows.Forms.Button
	Public WithEvents tbNumGrupos As System.Windows.Forms.TextBox
	Public WithEvents cmdPropagarGrupos As System.Windows.Forms.Button
	Public WithEvents cmdCopiarCursos As System.Windows.Forms.Button
	Public WithEvents tbTimeFin As System.Windows.Forms.TextBox
	Public WithEvents tbTime As System.Windows.Forms.TextBox
	Public WithEvents tbProceso As System.Windows.Forms.TextBox
	Public WithEvents tbHuecos As System.Windows.Forms.TextBox
	Public WithEvents tbValor As System.Windows.Forms.TextBox
	Public WithEvents Text2 As System.Windows.Forms.TextBox
	Public WithEvents Text1 As System.Windows.Forms.TextBox
	Public WithEvents cmdSim As System.Windows.Forms.Button
	Public WithEvents cmdAgrupar As System.Windows.Forms.Button
	Public WithEvents cmdCorregirCursos As System.Windows.Forms.Button
	Public WithEvents cmdGenerarListasPalabras As System.Windows.Forms.Button
	Public WithEvents Command1 As System.Windows.Forms.Button
	Public WithEvents tbActual As System.Windows.Forms.TextBox
	Public WithEvents cmdAgruparCursos As System.Windows.Forms.Button
	Public WithEvents pbTotal As System.Windows.Forms.ProgressBar
	Public WithEvents Label7 As System.Windows.Forms.Label
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Private listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAgrupacionCursos))
        Me.ToolTipMain = New System.Windows.Forms.ToolTip(Me.components)
        Me.Frame = New System.Windows.Forms.GroupBox()
        Me.chkElimCursos = New System.Windows.Forms.CheckBox()
        Me.chkCargaServ = New System.Windows.Forms.CheckBox()
        Me.pbCursos = New System.Windows.Forms.ProgressBar()
        Me.Entidades = New System.Windows.Forms.ComboBox()
        Me.tbNumAgrupaciones = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.cmdGenerarGrupos = New System.Windows.Forms.Button()
        Me.tbNumGrupos = New System.Windows.Forms.TextBox()
        Me.cmdPropagarGrupos = New System.Windows.Forms.Button()
        Me.cmdCopiarCursos = New System.Windows.Forms.Button()
        Me.tbTimeFin = New System.Windows.Forms.TextBox()
        Me.tbTime = New System.Windows.Forms.TextBox()
        Me.tbProceso = New System.Windows.Forms.TextBox()
        Me.tbHuecos = New System.Windows.Forms.TextBox()
        Me.tbValor = New System.Windows.Forms.TextBox()
        Me.Text2 = New System.Windows.Forms.TextBox()
        Me.Text1 = New System.Windows.Forms.TextBox()
        Me.cmdSim = New System.Windows.Forms.Button()
        Me.cmdAgrupar = New System.Windows.Forms.Button()
        Me.cmdCorregirCursos = New System.Windows.Forms.Button()
        Me.cmdGenerarListasPalabras = New System.Windows.Forms.Button()
        Me.Command1 = New System.Windows.Forms.Button()
        Me.tbActual = New System.Windows.Forms.TextBox()
        Me.cmdAgruparCursos = New System.Windows.Forms.Button()
        Me.pbTotal = New System.Windows.Forms.ProgressBar()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
        Me.LeerCursosWS = New System.Windows.Forms.Button()
        Me.PropGruposWS = New System.Windows.Forms.Button()
        Me.LocalizarGrupoErroneos = New System.Windows.Forms.Button()
        Me.IdentificarTipoGrupo = New System.Windows.Forms.Button()
        Me.Frame.SuspendLayout()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Frame
        '
        Me.Frame.BackColor = System.Drawing.SystemColors.Control
        Me.Frame.Controls.Add(Me.chkElimCursos)
        Me.Frame.Controls.Add(Me.chkCargaServ)
        Me.Frame.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame.Location = New System.Drawing.Point(18, 112)
        Me.Frame.Name = "Frame"
        Me.Frame.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame.Size = New System.Drawing.Size(201, 65)
        Me.Frame.TabIndex = 28
        Me.Frame.TabStop = False
        '
        'chkElimCursos
        '
        Me.chkElimCursos.BackColor = System.Drawing.SystemColors.Control
        Me.chkElimCursos.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkElimCursos.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkElimCursos.Location = New System.Drawing.Point(8, 16)
        Me.chkElimCursos.Name = "chkElimCursos"
        Me.chkElimCursos.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkElimCursos.Size = New System.Drawing.Size(189, 21)
        Me.chkElimCursos.TabIndex = 30
        Me.chkElimCursos.Text = "Eliminar Entidades con grupo RCP"
        Me.chkElimCursos.UseVisualStyleBackColor = False
        '
        'chkCargaServ
        '
        Me.chkCargaServ.BackColor = System.Drawing.SystemColors.Control
        Me.chkCargaServ.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCargaServ.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCargaServ.Location = New System.Drawing.Point(8, 36)
        Me.chkCargaServ.Name = "chkCargaServ"
        Me.chkCargaServ.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCargaServ.Size = New System.Drawing.Size(177, 21)
        Me.chkCargaServ.TabIndex = 29
        Me.chkCargaServ.Text = "Cargar cursos do servidor"
        Me.chkCargaServ.UseVisualStyleBackColor = False
        '
        'pbCursos
        '
        Me.pbCursos.Location = New System.Drawing.Point(276, 108)
        Me.pbCursos.Name = "pbCursos"
        Me.pbCursos.Size = New System.Drawing.Size(459, 15)
        Me.pbCursos.TabIndex = 27
        '
        'Entidades
        '
        Me.Entidades.BackColor = System.Drawing.SystemColors.Window
        Me.Entidades.Cursor = System.Windows.Forms.Cursors.Default
        Me.Entidades.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Entidades.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Entidades.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.Entidades, New Integer() {0, 0})
        Me.Entidades.Items.AddRange(New Object() {"CURSOS", "ORGANISMOS"})
        Me.Entidades.Location = New System.Drawing.Point(590, 10)
        Me.Entidades.Name = "Entidades"
        Me.Entidades.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Entidades.Size = New System.Drawing.Size(145, 26)
        Me.Entidades.TabIndex = 26
        '
        'tbNumAgrupaciones
        '
        Me.tbNumAgrupaciones.AcceptsReturn = True
        Me.tbNumAgrupaciones.BackColor = System.Drawing.SystemColors.Window
        Me.tbNumAgrupaciones.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tbNumAgrupaciones.Enabled = False
        Me.tbNumAgrupaciones.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbNumAgrupaciones.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tbNumAgrupaciones.Location = New System.Drawing.Point(460, 10)
        Me.tbNumAgrupaciones.MaxLength = 0
        Me.tbNumAgrupaciones.Name = "tbNumAgrupaciones"
        Me.tbNumAgrupaciones.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tbNumAgrupaciones.Size = New System.Drawing.Size(125, 26)
        Me.tbNumAgrupaciones.TabIndex = 24
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 20000
        '
        'cmdGenerarGrupos
        '
        Me.cmdGenerarGrupos.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGenerarGrupos.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGenerarGrupos.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGenerarGrupos.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGenerarGrupos.Location = New System.Drawing.Point(306, 140)
        Me.cmdGenerarGrupos.Name = "cmdGenerarGrupos"
        Me.cmdGenerarGrupos.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGenerarGrupos.Size = New System.Drawing.Size(301, 47)
        Me.cmdGenerarGrupos.TabIndex = 23
        Me.cmdGenerarGrupos.Text = "Xerar Agrupacións"
        Me.cmdGenerarGrupos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdGenerarGrupos.UseVisualStyleBackColor = False
        '
        'tbNumGrupos
        '
        Me.tbNumGrupos.AcceptsReturn = True
        Me.tbNumGrupos.BackColor = System.Drawing.SystemColors.Window
        Me.tbNumGrupos.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tbNumGrupos.Enabled = False
        Me.tbNumGrupos.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbNumGrupos.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tbNumGrupos.Location = New System.Drawing.Point(276, 12)
        Me.tbNumGrupos.MaxLength = 0
        Me.tbNumGrupos.Name = "tbNumGrupos"
        Me.tbNumGrupos.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tbNumGrupos.Size = New System.Drawing.Size(59, 26)
        Me.tbNumGrupos.TabIndex = 16
        '
        'cmdPropagarGrupos
        '
        Me.cmdPropagarGrupos.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPropagarGrupos.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPropagarGrupos.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPropagarGrupos.Location = New System.Drawing.Point(355, 531)
        Me.cmdPropagarGrupos.Name = "cmdPropagarGrupos"
        Me.cmdPropagarGrupos.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPropagarGrupos.Size = New System.Drawing.Size(285, 33)
        Me.cmdPropagarGrupos.TabIndex = 15
        Me.cmdPropagarGrupos.Text = "Propagar Grupos a RCP_GRP_TMP"
        Me.cmdPropagarGrupos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPropagarGrupos.UseVisualStyleBackColor = False
        '
        'cmdCopiarCursos
        '
        Me.cmdCopiarCursos.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCopiarCursos.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCopiarCursos.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCopiarCursos.Location = New System.Drawing.Point(64, 531)
        Me.cmdCopiarCursos.Name = "cmdCopiarCursos"
        Me.cmdCopiarCursos.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCopiarCursos.Size = New System.Drawing.Size(285, 33)
        Me.cmdCopiarCursos.TabIndex = 14
        Me.cmdCopiarCursos.Text = "Leer Cursos de RCP_GRP_TMP"
        Me.cmdCopiarCursos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCopiarCursos.UseVisualStyleBackColor = False
        '
        'tbTimeFin
        '
        Me.tbTimeFin.AcceptsReturn = True
        Me.tbTimeFin.BackColor = System.Drawing.SystemColors.Window
        Me.tbTimeFin.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tbTimeFin.Enabled = False
        Me.tbTimeFin.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbTimeFin.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tbTimeFin.Location = New System.Drawing.Point(82, 42)
        Me.tbTimeFin.MaxLength = 0
        Me.tbTimeFin.Name = "tbTimeFin"
        Me.tbTimeFin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tbTimeFin.Size = New System.Drawing.Size(51, 26)
        Me.tbTimeFin.TabIndex = 13
        Me.tbTimeFin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbTime
        '
        Me.tbTime.AcceptsReturn = True
        Me.tbTime.BackColor = System.Drawing.SystemColors.Window
        Me.tbTime.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tbTime.Enabled = False
        Me.tbTime.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbTime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tbTime.Location = New System.Drawing.Point(82, 14)
        Me.tbTime.MaxLength = 0
        Me.tbTime.Name = "tbTime"
        Me.tbTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tbTime.Size = New System.Drawing.Size(91, 26)
        Me.tbTime.TabIndex = 12
        Me.tbTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbProceso
        '
        Me.tbProceso.AcceptsReturn = True
        Me.tbProceso.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.tbProceso.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tbProceso.Enabled = False
        Me.tbProceso.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbProceso.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tbProceso.Location = New System.Drawing.Point(276, 38)
        Me.tbProceso.MaxLength = 0
        Me.tbProceso.Name = "tbProceso"
        Me.tbProceso.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tbProceso.Size = New System.Drawing.Size(459, 26)
        Me.tbProceso.TabIndex = 11
        '
        'tbHuecos
        '
        Me.tbHuecos.AcceptsReturn = True
        Me.tbHuecos.BackColor = System.Drawing.SystemColors.Window
        Me.tbHuecos.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tbHuecos.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tbHuecos.Location = New System.Drawing.Point(398, 432)
        Me.tbHuecos.MaxLength = 0
        Me.tbHuecos.Name = "tbHuecos"
        Me.tbHuecos.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tbHuecos.Size = New System.Drawing.Size(125, 20)
        Me.tbHuecos.TabIndex = 10
        Me.tbHuecos.Text = "Text2"
        '
        'tbValor
        '
        Me.tbValor.AcceptsReturn = True
        Me.tbValor.BackColor = System.Drawing.SystemColors.Window
        Me.tbValor.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tbValor.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tbValor.Location = New System.Drawing.Point(398, 410)
        Me.tbValor.MaxLength = 0
        Me.tbValor.Name = "tbValor"
        Me.tbValor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tbValor.Size = New System.Drawing.Size(125, 20)
        Me.tbValor.TabIndex = 9
        Me.tbValor.Text = "Text1"
        '
        'Text2
        '
        Me.Text2.AcceptsReturn = True
        Me.Text2.BackColor = System.Drawing.SystemColors.Window
        Me.Text2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text2.Location = New System.Drawing.Point(270, 434)
        Me.Text2.MaxLength = 0
        Me.Text2.Name = "Text2"
        Me.Text2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text2.Size = New System.Drawing.Size(125, 20)
        Me.Text2.TabIndex = 8
        Me.Text2.Text = "Text2"
        '
        'Text1
        '
        Me.Text1.AcceptsReturn = True
        Me.Text1.BackColor = System.Drawing.SystemColors.Window
        Me.Text1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text1.Location = New System.Drawing.Point(270, 412)
        Me.Text1.MaxLength = 0
        Me.Text1.Name = "Text1"
        Me.Text1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text1.Size = New System.Drawing.Size(125, 20)
        Me.Text1.TabIndex = 7
        Me.Text1.Text = "Text1"
        '
        'cmdSim
        '
        Me.cmdSim.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSim.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSim.Location = New System.Drawing.Point(156, 412)
        Me.cmdSim.Name = "cmdSim"
        Me.cmdSim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSim.Size = New System.Drawing.Size(113, 41)
        Me.cmdSim.TabIndex = 6
        Me.cmdSim.Text = "Similares"
        Me.cmdSim.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSim.UseVisualStyleBackColor = False
        '
        'cmdAgrupar
        '
        Me.cmdAgrupar.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAgrupar.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAgrupar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAgrupar.Location = New System.Drawing.Point(214, 298)
        Me.cmdAgrupar.Name = "cmdAgrupar"
        Me.cmdAgrupar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAgrupar.Size = New System.Drawing.Size(285, 39)
        Me.cmdAgrupar.TabIndex = 5
        Me.cmdAgrupar.Text = "Generar Grupos Cursos"
        Me.cmdAgrupar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgrupar.UseVisualStyleBackColor = False
        '
        'cmdCorregirCursos
        '
        Me.cmdCorregirCursos.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCorregirCursos.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCorregirCursos.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCorregirCursos.Location = New System.Drawing.Point(214, 260)
        Me.cmdCorregirCursos.Name = "cmdCorregirCursos"
        Me.cmdCorregirCursos.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCorregirCursos.Size = New System.Drawing.Size(285, 39)
        Me.cmdCorregirCursos.TabIndex = 4
        Me.cmdCorregirCursos.Text = "Corregir cursos"
        Me.cmdCorregirCursos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCorregirCursos.UseVisualStyleBackColor = False
        '
        'cmdGenerarListasPalabras
        '
        Me.cmdGenerarListasPalabras.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGenerarListasPalabras.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGenerarListasPalabras.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGenerarListasPalabras.Location = New System.Drawing.Point(304, 464)
        Me.cmdGenerarListasPalabras.Name = "cmdGenerarListasPalabras"
        Me.cmdGenerarListasPalabras.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGenerarListasPalabras.Size = New System.Drawing.Size(147, 35)
        Me.cmdGenerarListasPalabras.TabIndex = 3
        Me.cmdGenerarListasPalabras.Text = "Listas de palabras"
        Me.cmdGenerarListasPalabras.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdGenerarListasPalabras.UseVisualStyleBackColor = False
        '
        'Command1
        '
        Me.Command1.BackColor = System.Drawing.SystemColors.Control
        Me.Command1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Command1.Location = New System.Drawing.Point(164, 464)
        Me.Command1.Name = "Command1"
        Me.Command1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Command1.Size = New System.Drawing.Size(123, 43)
        Me.Command1.TabIndex = 2
        Me.Command1.Text = "PruebaVelocidad"
        Me.Command1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Command1.UseVisualStyleBackColor = False
        '
        'tbActual
        '
        Me.tbActual.AcceptsReturn = True
        Me.tbActual.BackColor = System.Drawing.SystemColors.Window
        Me.tbActual.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tbActual.Enabled = False
        Me.tbActual.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbActual.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tbActual.Location = New System.Drawing.Point(276, 66)
        Me.tbActual.MaxLength = 0
        Me.tbActual.Name = "tbActual"
        Me.tbActual.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tbActual.Size = New System.Drawing.Size(459, 26)
        Me.tbActual.TabIndex = 1
        '
        'cmdAgruparCursos
        '
        Me.cmdAgruparCursos.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAgruparCursos.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAgruparCursos.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAgruparCursos.Location = New System.Drawing.Point(464, 462)
        Me.cmdAgruparCursos.Name = "cmdAgruparCursos"
        Me.cmdAgruparCursos.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAgruparCursos.Size = New System.Drawing.Size(103, 33)
        Me.cmdAgruparCursos.TabIndex = 0
        Me.cmdAgruparCursos.Text = "Generar diccionario"
        Me.cmdAgruparCursos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgruparCursos.UseVisualStyleBackColor = False
        Me.cmdAgruparCursos.Visible = False
        '
        'pbTotal
        '
        Me.pbTotal.Location = New System.Drawing.Point(276, 92)
        Me.pbTotal.Maximum = 5
        Me.pbTotal.Name = "pbTotal"
        Me.pbTotal.Size = New System.Drawing.Size(459, 15)
        Me.pbTotal.TabIndex = 31
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(341, 8)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(122, 27)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "Nº de grupos"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(191, 65)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(81, 27)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "R.Actual"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(191, 37)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(81, 27)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "Proceso"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(189, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(81, 27)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Rexistro"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(10, 44)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(71, 27)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Tempo"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(134, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(39, 27)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "min"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(10, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(83, 27)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Comezo"
        '
        'LeerCursosWS
        '
        Me.LeerCursosWS.Location = New System.Drawing.Point(214, 214)
        Me.LeerCursosWS.Name = "LeerCursosWS"
        Me.LeerCursosWS.Size = New System.Drawing.Size(285, 40)
        Me.LeerCursosWS.TabIndex = 32
        Me.LeerCursosWS.Text = "Llamada WS"
        Me.LeerCursosWS.UseVisualStyleBackColor = True
        '
        'PropGruposWS
        '
        Me.PropGruposWS.Location = New System.Drawing.Point(214, 334)
        Me.PropGruposWS.Name = "PropGruposWS"
        Me.PropGruposWS.Size = New System.Drawing.Size(285, 40)
        Me.PropGruposWS.TabIndex = 33
        Me.PropGruposWS.Text = "PropagarGruposWS"
        Me.PropGruposWS.UseVisualStyleBackColor = True
        '
        'LocalizarGrupoErroneos
        '
        Me.LocalizarGrupoErroneos.BackColor = System.Drawing.SystemColors.Control
        Me.LocalizarGrupoErroneos.Cursor = System.Windows.Forms.Cursors.Default
        Me.LocalizarGrupoErroneos.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LocalizarGrupoErroneos.Location = New System.Drawing.Point(214, 584)
        Me.LocalizarGrupoErroneos.Name = "LocalizarGrupoErroneos"
        Me.LocalizarGrupoErroneos.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LocalizarGrupoErroneos.Size = New System.Drawing.Size(285, 39)
        Me.LocalizarGrupoErroneos.TabIndex = 34
        Me.LocalizarGrupoErroneos.Text = "Localizar Errores de Grupos"
        Me.LocalizarGrupoErroneos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.LocalizarGrupoErroneos.UseVisualStyleBackColor = False
        '
        'IdentificarTipoGrupo
        '
        Me.IdentificarTipoGrupo.BackColor = System.Drawing.SystemColors.Control
        Me.IdentificarTipoGrupo.Cursor = System.Windows.Forms.Cursors.Default
        Me.IdentificarTipoGrupo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.IdentificarTipoGrupo.Location = New System.Drawing.Point(214, 642)
        Me.IdentificarTipoGrupo.Name = "IdentificarTipoGrupo"
        Me.IdentificarTipoGrupo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.IdentificarTipoGrupo.Size = New System.Drawing.Size(285, 39)
        Me.IdentificarTipoGrupo.TabIndex = 35
        Me.IdentificarTipoGrupo.Text = "Identificar Tipos"
        Me.IdentificarTipoGrupo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.IdentificarTipoGrupo.UseVisualStyleBackColor = False
        '
        'frmAgrupacionCursos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(746, 710)
        Me.Controls.Add(Me.IdentificarTipoGrupo)
        Me.Controls.Add(Me.LocalizarGrupoErroneos)
        Me.Controls.Add(Me.PropGruposWS)
        Me.Controls.Add(Me.LeerCursosWS)
        Me.Controls.Add(Me.Frame)
        Me.Controls.Add(Me.pbCursos)
        Me.Controls.Add(Me.Entidades)
        Me.Controls.Add(Me.tbNumAgrupaciones)
        Me.Controls.Add(Me.cmdGenerarGrupos)
        Me.Controls.Add(Me.tbNumGrupos)
        Me.Controls.Add(Me.cmdPropagarGrupos)
        Me.Controls.Add(Me.cmdCopiarCursos)
        Me.Controls.Add(Me.tbTimeFin)
        Me.Controls.Add(Me.tbTime)
        Me.Controls.Add(Me.tbProceso)
        Me.Controls.Add(Me.tbHuecos)
        Me.Controls.Add(Me.tbValor)
        Me.Controls.Add(Me.Text2)
        Me.Controls.Add(Me.Text1)
        Me.Controls.Add(Me.cmdSim)
        Me.Controls.Add(Me.cmdAgrupar)
        Me.Controls.Add(Me.cmdCorregirCursos)
        Me.Controls.Add(Me.cmdGenerarListasPalabras)
        Me.Controls.Add(Me.Command1)
        Me.Controls.Add(Me.tbActual)
        Me.Controls.Add(Me.cmdAgruparCursos)
        Me.Controls.Add(Me.pbTotal)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 23)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAgrupacionCursos"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "(S.A.C.) Sistema Intelixente de  Agrupación  de Cadeas v1.0"
        Me.Frame.ResumeLayout(False)
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub ReLoadForm(ByVal addEvents As Boolean)
        Form_Load()
    End Sub
    Friend WithEvents LeerCursosWS As System.Windows.Forms.Button
    Friend WithEvents PropGruposWS As System.Windows.Forms.Button
    Public WithEvents LocalizarGrupoErroneos As System.Windows.Forms.Button
    Public WithEvents IdentificarTipoGrupo As System.Windows.Forms.Button
#End Region
End Class