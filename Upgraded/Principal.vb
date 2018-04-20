Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.DB.DAO
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Diagnostics
Imports System.Globalization
Imports System.Text
Imports System.Windows.Forms


Partial Friend Class frmAgrupacionCursos
    Inherits System.Windows.Forms.Form
    'Informacion .INI
    '[SAC]
    'RUTA_BD=D:\DOCUMENTOS\PROY\SAC\SAC.MDB
    'RUTA_BD_RCP = RCPReal_InterLink
    'OPCS_BD = ODBC;UID=rcpop;PWD=xunta2006

    Dim m_sLog As String
    'Control de fila y columna para recuperar información de la base de datos RCP
    Dim iCol As Integer
    Dim ifila As Integer
    Dim iCont As Integer = 0

    Const MAX_COD_GRP = 10000000
    Const EST_NORMAL = 1
    Const EST_BUSCANDO_ERROR = 2
    Const LEER_NUM_CURSOS = 10
    Const TAG_ERROR As Integer = -1
    Const TAG_OK As Integer = 1
    Const PORCENTAJE_PALABRA_SIMILAR As Double = 0.6
    'Const RUTA_BD = "D:\DOCUMENTOS\PROY\SAC\SAC.MDB"
    'Const RUTA_BD = "RCPPru_InterLink"
    'Const OPCS_BD = "ODBC;UID=alg;PWD=xunta006"

    'Const C_RUTA_BD = "C:\DOCUMENTOS\PROY\SAC\SAC.MDB"
    'Const C_RUTA_BD_RCP = "RCPReal"
    'Const C_OPCS_BD = "ODBC;UID=rcpop;PWD=xunta2006"

    Const C_RUTA_BD As String = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=C:\Users\antonio\Dropbox\TONI\PROY\SACGen\SAC.MDB;"
    Const C_RUTA_BD_RCP As String = "RCPReal"
    Const C_OPCS_BD As String = "ODBC;UID=rcpop;PWD=xunta2006"

    Const LIMITE_REINTENTOS As Integer = 5

    Dim sCad As New FixedLengthString(255)
    Dim RUTA_BD As String = ""
    Dim RUTA_BD_RCP As String = ""
    Dim OPCS_BD As String = ""

    'UPGRADE_NOTE: (2041) The following line was commented. More Information: http://www.vbtonet.com/ewis/ewi2041.aspx
    'Private Declare Function GetPrivateProfileString Lib "kernel32"  Alias "GetPrivateProfileStringA"(ByVal lpApplicationName As String, ByVal lpKeyName As Integer, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer

    Dim db As DAODatabaseHelper

    Dim lNumCads As Integer
    Public Sub New()
        MyBase.New()
        If m_vb6FormDefInstance Is Nothing Then
            If m_InitializingDefInstance Then
                m_vb6FormDefInstance = Me
            Else
                Try
                    'For the start-up form, the first instance created is the default instance.
                    If System.Reflection.Assembly.GetExecutingAssembly.EntryPoint.DeclaringType Is Me.GetType Then
                        m_vb6FormDefInstance = Me
                    End If

                Catch
                End Try
            End If
        End If
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        ReLoadForm(False)
    End Sub



    Private Sub cmdAgrupar_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgrupar.Click
        Dim iCont As Double
        Dim rsPal, rsBuscar, rsPalCurso, rsPorcentaje As DAORecordSetHelper
        Dim iContPalCurso As Integer
        Dim sPalabra As String = ""
        Dim sCad1 As New StringBuilder()
        Dim sCad As New StringBuilder()
        Dim iCodPalabra, iContPalabras, iPosicion As Integer
        Dim sBuscar As String = ""
        Dim iLon As Integer
        Dim lGrupo, lGrupoTmp As Integer
        Dim dPorcentaje As Double
        Dim sSQL As String = ""
        Dim iPalSimil As Integer
        Dim lCodCursoMaestro As Integer


        tbProceso.Text = "Inicializando datos"
        tbProceso.Refresh()


        'UPGRADE_WARNING: (2065) DAO.Database method DBEngine.OpenDatabase has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2065.aspx
        db = DBEngineHelper.Instance(Artinsoft.VB6.DB.AdoFactoryManager.GetFactory()).OpenDatabase(RUTA_BD)
        Dim TempCommand As DbCommand = db.Connection.CreateCommand()
        TempCommand.CommandText = "DELETE FROM palabras_similares"
        TempCommand.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
        TempCommand.ExecuteNonQuery()
        Dim TempCommand_2 As DbCommand = db.Connection.CreateCommand()
        TempCommand_2.CommandText = "DELETE FROM grupos_cursos"
        TempCommand_2.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
        TempCommand_2.ExecuteNonQuery()


        'Primero tenemos que localizar cual será el número del siguiente grupo
        'Si estamos reagrupando nuevos cursos, RCP nos ha dejado en la tabla rcp_grp_tmp tres elementos de cada uno de los grupos
        'El primero grupo generado debe ser el siguiente al máximo grupo de rcp_grp_tmp
        Dim rs As DAORecordSetHelper = db.OpenRecordset("SELECT MAX(cod_id_grupo) AS max_codigo FROM cursos")
        If rs.EOF Then
            lGrupo = 1
        ElseIf rs("max_codigo") < MAX_COD_GRP Then
            lGrupo = CInt(rs("max_codigo") + 1)
        Else
            lGrupo = 1
        End If
        rs.Close()

        Dim lGrupoInicial As Integer = lGrupo

        Dim iContCursos As Integer = 0
        'Marcamos los cursos que entran en juego. Son todos los de denom_curso_corregida distinta
        Dim TempCommand_3 As DbCommand = db.Connection.CreateCommand()
        TempCommand_3.CommandText = "UPDATE cursos SET control = 0"
        TempCommand_3.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
        TempCommand_3.ExecuteNonQuery()
        'De cada denominación distinta, primero escogemos el menor de los cursos con gr
        Dim TempCommand_4 As DbCommand = db.Connection.CreateCommand()
        TempCommand_4.CommandText = "UPDATE cursos SET control = 1 WHERE cod_id_curso IN (SELECT MIN(cod_id_curso) AS cod_curso FROM cursos WHERE cod_id_grupo > 0 GROUP BY denom_curso_corregida)"
        TempCommand_4.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
        TempCommand_4.ExecuteNonQuery()
        'Del resto, marcamos uno de cada denominación corregida distinta, siempre que no coincida con
        'la denom_corregida de uno marcado como control
        Dim TempCommand_5 As DbCommand = db.Connection.CreateCommand()
        TempCommand_5.CommandText = "UPDATE cursos SET control = 1 WHERE cod_id_curso IN (SELECT MIN(cod_id_curso) AS cod_curso FROM cursos GROUP BY denom_curso_corregida)" & _
                                    " AND NOT denom_curso_corregida IN (SELECT DISTINCT(denom_curso_corregida) FROM cursos WHERE control = 1)"
        TempCommand_5.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
        TempCommand_5.ExecuteNonQuery()

        tbProceso.Text = "Xerando grupos"
        tbProceso.Refresh()


        rs = db.OpenRecordset("SELECT denom_curso_corregida, cod_id_curso AS cod_curso, palabras  FROM cursos WHERE control = 1 ORDER BY palabras DESC, denom_curso_corregida")
        If Not rs.EOF Then
            rs.MoveLast()
            pbCursos.Maximum = rs.RecordCount
            rs.MoveFirst()
            pbCursos.Value = 0
        End If
        Dim dPrecision As Double
        While Not rs.EOF
            pbCursos.Value += 1
            Application.DoEvents()
            Inc(iContCursos)
            tbActual.Text = "(" & iContCursos & ") " & rs("denom_curso_corregida")

            rsPal = db.OpenRecordset("SELECT p.cod_id_palabra,palabra, posicion, contador FROM palabras p, palabras_cursos pc " & _
                    "WHERE p.cod_id_palabra = pc.cod_id_palabra AND cod_id_curso = " & rs("cod_curso") & " ORDER BY posicion")
            'Localizamos todos los cursos que tengan palabras que comiencen por las mismas dos letras
            '1. Ajustamos las palabras parecidas o que podemos considerar iguales
            '2. Comprobamos los cursos que tienen estas palabras
            '3. Comprobamos los cursos que pueden ser similares al que estamos comprobando
            '4. Asignamos el grupo a los cursos similares
            If Not rsPal.EOF Then
                rsPal.MoveLast()
                iLon = rsPal.RecordCount
                rsPal.MoveFirst()
            Else
                iLon = 0
            End If
            While Not rsPal.EOF
                sCad1 = New StringBuilder("")
                sCad = New StringBuilder("")
                sPalabra = rsPal("palabra")
                iCodPalabra = rsPal("cod_id_palabra")
                iPosicion = rsPal("posicion")
                'db.Execute "INSERT INTO palabras_similares_origen (" & iCodPalabra & ",'" & sPalabra & "')"
                iContPalabras = 1
                'Comprobamos si esta palabra ya ha sido comprobada
                rsBuscar = db.OpenRecordset("SELECT COUNT(*) FROM palabras_similares WHERE cod_id_palabra_origen = " & iCodPalabra)
                If rsBuscar(0) = 0 Then
                    rsBuscar.Close()
                    'rsBuscar = db.OpenRecordset("SELECT cod_id_palabra, palabra FROM palabras p WHERE (palabra LIKE '" & Strings.Left(sPalabra, 3) & "%' OR palabra LIKE '_" & Strings.Mid(sPalabra, 2, 3) & "%')")
                    Dim iG(4, 2) As Single, iGd(4, 2) As Single
                    Dim i1 As String, i2 As String, i3 As String, i4 As String
                    ExtraerCaracteristicas(sPalabra, iG, iGd)
                    i1 = iG(0, 0).ToString
                    i1 = " ABS(Grupo1 -" + i1 + ")<=2 "
                    i2 = iG(1, 0).ToString
                    i2 = " ABS(Grupo2 -" + i2 + ")<=2 "
                    i3 = iG(2, 0).ToString
                    i3 = " ABS(Grupo3 -" + i3 + ")<=2 "
                    i4 = iG(3, 0).ToString
                    i4 = " ABS(Grupo4 -" + i4 + ")<=2 "

                    'i1 = iG(0, 0).ToString
                    'i2 = iG(1, 0).ToString
                    'i3 = iG(2, 0).ToString
                    'i4 = iG(3, 0).ToString
                    'i1 = " ABS(Grupo1 -" + i1 + ")"
                    'i2 = " ABS(Grupo2 -" + i2 + ")"
                    'i3 = " ABS(Grupo3 -" + i3 + ")"
                    'i4 = " ABS(Grupo4 -" + i4 + ")"

                    sSQL = "SELECT cod_id_palabra, palabra FROM palabras p WHERE " + i1 + " AND " + i2 + " AND " + i3 + " AND " + i4
                    'sSQL = "SELECT cod_id_palabra, palabra FROM palabras p WHERE (" + i1 + "+" + i2 + "+" + i3 + "+" + i4 + ") <= longitud/4+1"
                    rsBuscar = db.OpenRecordset(sSQL)
                    While Not rsBuscar.EOF

                        ' Por cada palabra comprobamos si la consideramos similar
                        'tbProceso.Text = "(" & iContCursos & ")Comparando " & sPalabra & "," & rsBuscar.Fields("palabra")
                        Application.DoEvents()
                        If sPalabra = rsBuscar("palabra") Then
                            sCad.Append(rsBuscar("palabra") & Environment.NewLine)
                            'La propia palabra siempre entra con código 0
                            Dim TempCommand_6 As DbCommand = db.Connection.CreateCommand()
                            TempCommand_6.CommandText = "INSERT INTO palabras_similares VALUES (0," & iCodPalabra & "," & rsBuscar("cod_id_palabra") & ", 1)"
                            TempCommand_6.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                            TempCommand_6.ExecuteNonQuery()
                        Else
                            dPorcentaje = Comparar(sPalabra, rsBuscar("palabra"))
                            If dPorcentaje > PORCENTAJE_PALABRA_SIMILAR Then
                                sCad1.Append(rsBuscar("palabra") & Environment.NewLine)
                                'Si la consideramos similar la introducimos dentro del grupo de palabras similares
                                Dim TempCommand_7 As DbCommand = db.Connection.CreateCommand()
                                TempCommand_7.CommandText = "INSERT INTO palabras_similares VALUES (" & iContPalabras & "," & CStr(iCodPalabra) & "," & rsBuscar("cod_id_palabra") & ",'" & CStr(dPorcentaje) & "')"
                                TempCommand_7.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                                TempCommand_7.ExecuteNonQuery()
                                Inc(iContPalabras)
                            End If
                        End If
                        rsBuscar.MoveNext()
                    End While
                    'MsgBox sCad & "------------------" & vbCrLf & sCad1
                End If
                rsBuscar.Close()
                sCad = New StringBuilder("")

                'NO SE USA: Localizamos los cursos que contienen la palabra o sus similares en el mismo lugar o cerca
                'sBuscar = sBuscar & " AND pc.posicion = " & iPosicion & " AND pc.cod_id_palabra IN (SELECT cod_id_palabra FROM palabras_similares ps WHERE ps.cod_id_palabra_origen = " & iCodPalabra & ")"
                'Set rsBuscar = db.OpenRecordset("SELECT c.cod_id_curso FROM cursos c, palabras_cursos pc WHERE c.cod_id_curso = pc.cod_id_curso " & _
                ''            " AND pc.posicion > " & iPosicion - 1 & "AND pc.posicion < " & iPosicion + 1 & " AND pc.cod_id_palabra IN (SELECT cod_id_palabra FROM palabras_similares ps WHERE ps.cod_id_palabra_origen = " & iCodPalabra & ")", _
                ''            dbOpenSnapshot)
                'rsBuscar.Close
                rsPal.MoveNext()
            End While
            'Ahora tenemos todas las palabras del curso y todas sus similares grabadas en la base de datos
            'Localizamos los cursos que tienen la palabra del curso o sus similares en la misma posición o en la siguiente o anterior

            'Set rsBuscar = db.OpenRecordset("SELECT pc2.cod_id_curso, COUNT(*) AS contador FROM cursos c, palabras_cursos pc1, palabras_cursos pc2 " & _
            ''    " WHERE pc2.cod_id_curso = c.cod_id_curso AND c.control = 1 AND pc2.cod_id_curso <> pc1.cod_id_curso AND pc1.cod_id_curso = " & _
            ''    rs.Fields("cod_curso") & _
            ''    " AND (pc2.posicion >= pc1.posicion-1 AND pc2.posicion <= pc1.posicion+1) AND pc2.cod_id_palabra IN " & _
            ''    "(SELECT cod_id_palabra FROM palabras_similares WHERE cod_id_palabra_origen = pc1.cod_id_palabra) " & _
            ''    "GROUP BY pc2.cod_id_curso HAVING COUNT(*) >= " & iLon - 1 & " ORDER BY 2 DESC", dbOpenSnapshot)



            If iLon > 12 Then
                iPalSimil = iLon - 3
            ElseIf iLon > 7 Then
                iPalSimil = iLon - 2
            ElseIf iLon > 3 Then
                iPalSimil = iLon - 1
            Else
                iPalSimil = iLon
            End If
            ' Genera nuevos grupos aunque no estén emparejados
            'sSQL = "SELECT pc2.cod_id_curso, SUM(ps.porcentaje) AS suma_porcentaje, COUNT(*) AS contador FROM cursos c, palabras_cursos pc1, palabras_cursos pc2, palabras_similares ps " & _
            ''    " WHERE pc2.cod_id_curso = c.cod_id_curso AND c.control = 1 AND pc2.cod_id_curso <> pc1.cod_id_curso AND pc1.cod_id_curso = " & _
            ''    rs.Fields("cod_curso") & _
            ''    " AND (pc2.posicion >= pc1.posicion-1 AND pc2.posicion <= pc1.posicion+1) AND pc2.cod_id_palabra = ps.cod_id_palabra AND ps.cod_id_palabra_origen = pc1.cod_id_palabra " & _
            ''    "GROUP BY pc2.cod_id_curso HAVING COUNT(*) >= " & iPalSimil & " ORDER BY 2 DESC"

            'Buscamos si el curso maestro está ya en algún grupo
            sSQL = "SELECT COUNT(*) FROM grupos_cursos WHERE cod_id_curso = " & rs("cod_curso")
            rsBuscar = db.OpenRecordset(sSQL)
            iCont = rsBuscar(0)
            rsBuscar.Close()

            'Buscamos si el grupo pertenece a la lista de no agrupables
            sSQL = "SELECT COUNT(*) FROM denom_corregidas_no_agrupables WHERE denom_curso = '" & rs("denom_curso_corregida") & "'"
            rsBuscar = db.OpenRecordset(sSQL)
            iCont += rsBuscar(0)
            rsBuscar.Close()

            'Genera un nuevo grupo solo si no está el curso ya emparejado en algún grupo
            If iCont = 0 Then
                sSQL = "SELECT pc2.cod_id_curso, SUM(ps.porcentaje) AS suma_porcentaje, COUNT(*) AS contador FROM cursos c, palabras_cursos pc1, palabras_cursos pc2, palabras_similares ps " & _
                       " WHERE pc2.cod_id_curso = c.cod_id_curso AND c.control = 1 AND pc2.cod_id_curso <> pc1.cod_id_curso AND pc1.cod_id_curso = " & _
                       rs("cod_curso") & _
                       " AND (pc2.posicion >= pc1.posicion-1 AND pc2.posicion <= pc1.posicion+1) AND pc2.cod_id_palabra = ps.cod_id_palabra AND ps.cod_id_palabra_origen = pc1.cod_id_palabra " & _
                       "GROUP BY pc2.cod_id_curso HAVING (COUNT(*) >= " & CStr(iPalSimil) & ") ORDER BY 2 DESC"

                Debug.WriteLine(sSQL)
                Debug.WriteLine(rs("denom_curso_corregida"))
                rsBuscar = db.OpenRecordset(sSQL)

                lCodCursoMaestro = rs("cod_curso")

                'El curso tiene otros similares
                If Not rsBuscar.EOF Then
                    Inc(lGrupo)
                    tbNumAgrupaciones.Text = CStr(lGrupo) & "," & CStr(lGrupo - lGrupoInicial)
                    'Insertamos el propio curso con un emparejamiento de todas las palabras y lo identificamos como curso maestro
                    Dim TempCommand_8 As DbCommand = db.Connection.CreateCommand()
                    TempCommand_8.CommandText = "INSERT INTO grupos_cursos VALUES (" & lGrupo & "," & rs("cod_curso") & "," & CStr(iLon) & "," & CStr(iLon) & "," & CStr(iLon) & ",1,1," & CStr(lCodCursoMaestro) & ")"
                    TempCommand_8.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                    TempCommand_8.ExecuteNonQuery()
                    While Not rsBuscar.EOF
                        'Localizamos el número de palabras del curso agrupado
                        rsPalCurso = db.OpenRecordset("SELECT COUNT(*) FROM palabras_cursos pc " & _
                                     "WHERE cod_id_curso = " & rsBuscar("cod_id_curso"))
                        iContPalCurso = rsPalCurso(0)
                        rsPalCurso.Close()

                        'Comprobamos si el curso ya está en otro grupo y comprobamos la precisión del agrupamiento
                        'Set rsPalCurso = db.OpenRecordset("SELECT cod_id_curso, MAX(porcentaje) AS precision FROM grupos_cursos gc WHERE cod_id_curso = " & rsBuscar.Fields("cod_id_curso") & " GROUP BY cod_id_curso", dbOpenSnapshot)
                        rsPalCurso = db.OpenRecordset("SELECT cod_id_curso, porcentaje, equiv, curso_maestro1 FROM grupos_cursos WHERE cod_id_curso = " & rsBuscar("cod_id_curso"))

                        If Not rsPalCurso.EOF Then
                            'Solo Cambiamos de grupo el elemento en el grupo si su precisión de agrupamiento es mayor
                            'y el número de palabras equivalentes con su maestro es mayor o igual y no es un maestro
                            dPrecision = rsBuscar("suma_porcentaje") / iContPalCurso
                            If rsPalCurso("curso_maestro1") = 0 And rsPalCurso("porcentaje") < dPrecision And rsBuscar("contador") >= rsPalCurso("equiv") Then
                                Dim TempCommand_9 As DbCommand = db.Connection.CreateCommand()
                                TempCommand_9.CommandText = "UPDATE grupos_cursos SET cod_id_grupo = " & lGrupo & ", equiv = " & rsBuscar("contador") & ", num_palabras = " & CStr(iContPalCurso) & ",num_palabras_maestro = " & CStr(iLon) & ", porcentaje = '" & CStr(dPrecision) & "',curso_maestro1 = 0, cod_id_maestro = " & CStr(lCodCursoMaestro) & " WHERE cod_id_curso = " & rsPalCurso("cod_id_curso")
                                TempCommand_9.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                                TempCommand_9.ExecuteNonQuery()
                            End If
                        Else
                            ' El elemento no está en otro grupo
                            Dim TempCommand_10 As DbCommand = db.Connection.CreateCommand()
                            TempCommand_10.CommandText = "INSERT INTO grupos_cursos VALUES (" & lGrupo & "," & rsBuscar("cod_id_curso") & "," & rsBuscar("contador") & "," & CStr(iContPalCurso) & "," & CStr(iLon) & ",'" & CStr(rsBuscar("suma_porcentaje") / iContPalCurso) & "',0," & CStr(lCodCursoMaestro) & ")"
                            TempCommand_10.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                            TempCommand_10.ExecuteNonQuery()
                        End If

                        rsPalCurso.Close()

                        rsBuscar.MoveNext()
                    End While
                Else
                    'El curso actual no tiene ningún curso similar
                    'Insertamos el propio curso con un emparejamiento de todas las palabras
                    'db.Execute "INSERT INTO grupos_cursos VALUES (" & lGrupo & "," & rs.Fields("cod_curso") & "," & iLon & "," & iLon & ",1,1)"

                    'Si el grupo no tiene a nadie similar no lo insertamos para que intente agruparlo en otros grupos
                    'Al final los elementos con control = 1 que no esten en ningún grupo hay que crearles grupos contiguos de un elemento
                End If
                rsBuscar.Close()
            End If

            rs.MoveNext()
        End While
        rs.Close()

        tbProceso.Text = "Recalculando reagrupacións en grupos existentes"
        tbProceso.Refresh()

        'Ahora tenemos que ver los grupos que contienen elementos ya agrupados para decidir si realmente todo el grupo
        'pertenece a un grupo ya existente

        '1. Primero comprobamos si el curso maestro de algún grupo es un curso con grupo existente en rcp
        rs = db.OpenRecordset("SELECT c.cod_id_curso, c.cod_id_grupo AS grupo_rcp, gc.cod_id_grupo AS grupo_nuevo, gc.num_palabras FROM grupos_cursos gc, cursos c WHERE " & _
             "gc.cod_id_curso = c.cod_id_curso AND c.cod_id_grupo > 0 and gc.curso_maestro1 = 1")
        While Not rs.EOF
            'Puede ser posible que los distintos componentes de un mismo grupo hayan sido agrupados en grupos distintos
            'Si el padre tiene mas de una palabra asumimos que la agrupación es correcta y dos dos grupos se unifican, si los hubiera
            tbActual.Text = "Proc. Reg.: Grupo Novo " & rs("grupo_nuevo") & " ->RCP: " & rs("grupo_rcp") & " Curso: " & rs("cod_id_curso")
            tbActual.Refresh()
            'Modificamos el grupo por el ya existente en RCP
            If CompruebaSiAsigGrupoRCP(rs("grupo_rcp"), rs("cod_id_curso"), rs("num_palabras")) Then
                Dim TempCommand_11 As DbCommand = db.Connection.CreateCommand()
                TempCommand_11.CommandText = "UPDATE grupos_cursos SET cod_id_grupo = " & rs("grupo_rcp") & " WHERE cod_id_grupo = " & rs("grupo_nuevo")
                TempCommand_11.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                TempCommand_11.ExecuteNonQuery()
            End If
            rs.MoveNext()
        End While
        rs.Close()

        '2. Ahora comprobamos si algún grupo tiene como alguno de sus componentes alguna de las denominaciones ya agrupadas en rcp
        '   Cambiaremos el código del grupo, si el parecido de esa denominación con el padre supera cierto grado de equivalencia

        rs = db.OpenRecordset("SELECT c.cod_id_curso, (num_palabras_maestro-equiv) AS parecido,porcentaje,num_palabras,num_palabras_maestro," & _
             "c.cod_id_grupo AS grupo_rcp, gc.cod_id_grupo AS grupo_nuevo FROM grupos_cursos gc, cursos c WHERE " & _
             "gc.cod_id_curso = c.cod_id_curso AND c.cod_id_grupo <> 0 and gc.cod_id_grupo >= " & CStr(lGrupoInicial) & " ORDER BY 2,3 DESC")
        'tenemos todos los componentes ya agrupados en rcp de cada grupo,debemos decidir cual es el que agrupa mejor con su maestro
        'para ello ordenmos por parecido y ante el mismo numero de palabras iguales ordenmos por precision
        While Not rs.EOF
            If lGrupoTmp <> rs("grupo_nuevo") Then
                tbActual.Text = "Proc. Reg.: Grupo Novo " & rs("grupo_nuevo") & "  -> Grupo RCP: " & rs("grupo_rcp")
                tbActual.Refresh()
                'Estamos procesando el primer elemento del grupo
                'Cambiamos el grupo de todos los elementos al presente en rcp
                If CompruebaSiAsigGrupoRCP(rs("grupo_nuevo"), rs("cod_id_curso"), rs("num_palabras")) Then
                    Dim TempCommand_12 As DbCommand = db.Connection.CreateCommand()
                    TempCommand_12.CommandText = "UPDATE grupos_cursos SET cod_id_grupo = " & rs("grupo_rcp") & " WHERE cod_id_grupo = " & rs("grupo_nuevo")
                    TempCommand_12.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                    TempCommand_12.ExecuteNonQuery()
                End If
                lGrupoTmp = rs("grupo_nuevo")
            End If
            rs.MoveNext()
        End While
        rs.Close()


        tbProceso.Text = "Pechando xeración de grupos"
        tbProceso.Refresh()

        'Al final los elementos con control = 1 que no esten en ningún grupo hay que crearles grupos contiguos de un elemento
        rs = db.OpenRecordset("SELECT c.cod_id_curso, COUNT(*) AS num_palabras FROM cursos AS c, palabras_cursos pc WHERE pc.cod_id_curso = c.cod_id_curso AND control=1 and (SELECT COUNT(*) FROM grupos_cursos gc WHERE gc.cod_id_curso = c.cod_id_curso) = 0 GROUP BY c.cod_id_curso")
        While Not rs.EOF
            Inc(lGrupo)
            tbNumAgrupaciones.Text = CStr(lGrupo) & "," & CStr(lGrupo - lGrupoInicial)
            tbNumAgrupaciones.Refresh()
            'Insertamos el propio curso con un emparejamiento de todas las palabras
            iLon = rs("num_palabras")
            Dim TempCommand_13 As DbCommand = db.Connection.CreateCommand()
            TempCommand_13.CommandText = "INSERT INTO grupos_cursos VALUES (" & lGrupo & "," & rs("cod_id_curso") & "," & CStr(iLon) & "," & CStr(iLon) & "," & CStr(iLon) & ",1,1," & rs("cod_id_curso") & ")"
            TempCommand_13.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
            TempCommand_13.ExecuteNonQuery()
            rs.MoveNext()
        End While
        rs.Close()

        'Los cursos de control que tienen vacía la denom_curso_corregida no salen en la consulta anterior
        rs = db.OpenRecordset("SELECT c.cod_id_curso FROM cursos AS c WHERE denom_curso_corregida = '' AND control=1")
        While Not rs.EOF
            Inc(lGrupo)
            tbNumAgrupaciones.Text = CStr(lGrupo) & "," & CStr(lGrupo - lGrupoInicial)
            tbNumAgrupaciones.Refresh()
            'Insertamos el propio curso con un emparejamiento de todas las palabras
            iLon = 0
            Dim TempCommand_14 As DbCommand = db.Connection.CreateCommand()
            TempCommand_14.CommandText = "INSERT INTO grupos_cursos VALUES (" & lGrupo & "," & rs("cod_id_curso") & "," & CStr(iLon) & "," & CStr(iLon) & "," & CStr(iLon) & ",1,1," & rs("cod_id_curso") & ")"
            TempCommand_14.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
            TempCommand_14.ExecuteNonQuery()
            rs.MoveNext()
        End While
        rs.Close()

        'Si queda algún curso sin agrupar le damos grupos consecutivos
        Dim lCodCurso As Integer
        rs = db.OpenRecordset("SELECT c.cod_id_curso, gc.cod_id_grupo, c.denom_curso_corregida FROM cursos c, grupos_cursos gc WHERE c.control = 1 AND gc.cod_id_curso = c.cod_id_curso ORDER BY c.cod_id_curso")
        While Not rs.EOF
            If lCodCurso = rs("cod_id_curso") Then
                MessageBox.Show("Curso repetido: " & lCodCurso, Application.ProductName)
            End If
            lCodCurso = rs("cod_id_curso")
            tbActual.Text = rs("denom_curso_corregida")
            tbActual.Refresh()
            Dim TempCommand_15 As DbCommand = db.Connection.CreateCommand()
            TempCommand_15.CommandText = "UPDATE cursos SET cod_id_grupo = " & rs("cod_id_grupo") & " WHERE denom_curso_corregida = '" & rs("denom_curso_corregida") & "'"
            TempCommand_15.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
            TempCommand_15.ExecuteNonQuery()
            rs.MoveNext()
            tbNumAgrupaciones.Text = CStr(lGrupo) & "," & CStr(lGrupo - lGrupoInicial)
            tbNumAgrupaciones.Refresh()
        End While
        rs.Close()
        Artinsoft.VB6.DB.TransactionManager.DeEnlist(db.Connection)
        db.Close()

        tbNumAgrupaciones.Text = CStr(lGrupo)
        tbProceso.Text = "Operación Finalizada"

    End Sub


    Function Similares(ByRef sCad1 As String, ByRef sCad2 As String, ByVal iPosCad1 As Integer, ByVal iPosCad2 As Integer, ByVal dValor As Double, ByRef iHuecos As Integer) As Double
        Dim result As Double = 0
        Dim sCar1, sCar2 As String
        Dim iHuecosTotal As Integer
        Dim dValorMayor As Double

        Dim iLon1 As Integer = sCad1.Length
        Dim iLon2 As Integer = sCad2.Length

        Dim dValor1 As Double = 0
        Dim dValor2 As Double = 0
        Dim dValor3 As Double = 0

        Dim iHuecos1 As Integer = 0
        Dim iHuecos2 As Integer = 0
        Dim iHuecos3 As Integer = 0

        Do While iPosCad1 <= iLon1 And iPosCad2 <= iLon2

            sCar1 = Strings.Mid(sCad1, iPosCad1, 1)
            sCar2 = Strings.Mid(sCad2, iPosCad2, 1)

            If sCar1 = sCar2 Then
                Inc(dValor)
                Inc(iPosCad1)
                Inc(iPosCad2)
            Else
                Exit Do
            End If
        Loop

        'Si no acumulamos dos aciertos eliminamos este camino
        'If dValor < 1 Then
        '    Inc iHuecos
        '    Similares = 0
        '    Exit Function
        'End If

        'Si no se termina ninguna cadena y dejan de ser iguales

        If iPosCad1 <= iLon1 And iPosCad2 <= iLon2 Then
            'Si el caracter no es el mismo tenemos tres opciones

            '1. Ignorar el de cad1
            If iPosCad1 < sCad1.Length Then
                dValor1 = Similares(sCad1, sCad2, iPosCad1 + 1, iPosCad2, 0, iHuecos1)
            End If

            '2. Ignorar el de cad2
            If iPosCad2 < sCad2.Length Then
                dValor2 = Similares(sCad1, sCad2, iPosCad1, iPosCad2 + 1, 0, iHuecos2)
            End If

            '3. Ignoramos los dos
            If iPosCad1 < sCad1.Length And iPosCad2 < sCad2.Length Then
                dValor3 = Similares(sCad1, sCad2, iPosCad1 + 1, iPosCad2 + 1, 0, iHuecos3)
            End If

            Inc(iHuecos)


            If dValor1 > dValor2 Then
                dValorMayor = dValor1
                iHuecosTotal = iHuecos1
            Else
                dValorMayor = dValor2
                iHuecosTotal = iHuecos2
            End If

            If dValorMayor < dValor3 Then
                dValorMayor = dValor3
                iHuecosTotal = iHuecos3
            End If
        End If

        result = dValor + dValorMayor
        iHuecos += iHuecosTotal
        Return result
    End Function

    Private Sub cmdAgruparCursos_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgruparCursos.Click
        Dim sCad As String = ""
        'UPGRADE_WARNING: (2065) DAO.Database method DBEngine.OpenDatabase has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2065.aspx
        db = DBEngineHelper.Instance(Artinsoft.VB6.DB.AdoFactoryManager.GetFactory()).OpenDatabase(RUTA_BD)
        Dim TempCommand As DbCommand = db.Connection.CreateCommand()
        TempCommand.CommandText = "DELETE FROM diccionario"
        TempCommand.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
        TempCommand.ExecuteNonQuery()
        Dim iFile As Integer = FileSystem.FreeFile()
        FileSystem.FileOpen(iFile, "d:\documentos\proy\sac\dic\gallego.txt", OpenMode.Input)
        While Not FileSystem.EOF(iFile)
            sCad = FileSystem.LineInput(iFile)
            sCad = ProcesarCadena(sCad)
            If sCad.Trim() <> "" Then
                Dim TempCommand_2 As DbCommand = db.Connection.CreateCommand()
                TempCommand_2.CommandText = "INSERT INTO diccionario VALUES (1,'" & sCad & "')"
                TempCommand_2.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                TempCommand_2.ExecuteNonQuery()
            End If
            tbProceso.Text = sCad
            tbProceso.Refresh()
            Application.DoEvents()
        End While
        FileSystem.FileClose(iFile)

        FileSystem.FileOpen(iFile, "d:\documentos\proy\sac\dic\espanol.txt", OpenMode.Input)
        While Not FileSystem.EOF(iFile)
            sCad = FileSystem.LineInput(iFile)
            sCad = ProcesarCadena(sCad)
            If sCad.Trim() <> "" Then
                Dim TempCommand_3 As DbCommand = db.Connection.CreateCommand()
                TempCommand_3.CommandText = "INSERT INTO diccionario VALUES (2,'" & sCad & "')"
                TempCommand_3.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                TempCommand_3.ExecuteNonQuery()
            End If
            tbProceso.Text = sCad
            tbProceso.Refresh()
            Application.DoEvents()
        End While
        FileSystem.FileClose(iFile)

        Artinsoft.VB6.DB.TransactionManager.DeEnlist(db.Connection)
        db.Close()

    End Sub

    Function ProcesarCadena(ByRef sCad As String) As String
        Dim result As New StringBuilder()
        Dim bAlfabetico As Boolean


        sCad = sCad.Trim().ToUpper()
        Dim i As Integer = (sCad.IndexOf("/"c) + 1)
        If i > 0 Then
            sCad = Strings.Mid(sCad, 1, i - 1)
        End If

        For Each sCar As Char In sCad
            Select Case sCar
                Case "Á", "À", "Ä"
                    sCar = "A"
                Case "É", "È", "Ë"
                    sCar = "E"
                Case "Í", "Ì", "Ï"
                    sCar = "I"
                Case "Ó", "Ò", "Ö"
                    sCar = "O"
                Case "Ú", "Û", "Ü"
                    sCar = "U"
                Case """", "'", "`"
                    sCar = ""
                Case Else
            End Select
            If (sCar >= "a" And sCar <= "z") Or (sCar >= "A" And sCar <= "Z") Or sCar = "Ñ"c Then
                bAlfabetico = True
            Else
                If bAlfabetico Then
                    If sCar >= "1" And sCar <= "9" Then
                        sCar = " " & sCar
                    Else
                        sCar = " "
                    End If
                Else
                    sCar = " "
                End If
                bAlfabetico = False
            End If
            result.Append(sCar)
        Next sCar
        Return result.ToString()
    End Function

    Function CorregirCadena(ByRef sCad As String) As String
        Dim result As New StringBuilder()
        Dim sCar As String = ""
        Dim bAlfabetico As Boolean

        'Eliminamos grupos de espacios y todos los caracteres no alfabéticos o numéricos
        Dim bEspacio As Boolean = False
        For i As Integer = 1 To sCad.Length
            sCar = Strings.Mid(sCad, i, 1).ToUpper()
            'Eliminamos los espacios contiguos
            If sCar = " " Then
                If bEspacio Then
                    sCar = ""
                Else
                    bEspacio = True
                End If
            Else
                bEspacio = False
            End If

            Select Case sCar
                Case "Á", "À", "Ä"
                    sCar = "A"
                Case "É", "È", "Ë"
                    sCar = "E"
                Case "Í", "Ì", "Ï"
                    sCar = "I"
                Case "Ó", "Ò", "Ö"
                    sCar = "O"
                Case "Ú", "Û", "Ü"
                    sCar = "U"
                Case "(", ")", "-", ".", ",", ";", ":"
                    sCar = " "
                    bEspacio = True
            End Select
            If Not ((sCar >= "0" And sCar <= "9") Or (sCar >= "a" And sCar <= "z") Or (sCar >= "A" And sCar <= "Z") Or sCar = "Ñ" Or sCar = " ") Then
                sCar = ""
            End If
            result.Append(sCar)
        Next
        Return result.ToString().Trim()
    End Function

    Private Sub cmdCopiarCursos_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCopiarCursos.Click
        Dim rs As DAORecordSetHelper
        Dim dbRCP As DAODatabaseHelper

        tbProceso.Text = "Lendo datos de RCP_GRP_TMP"
        tbProceso.Refresh()
        Dim lNumGrupos As Integer = 0
        'UPGRADE_WARNING: (2065) DAO.Database method DBEngine.OpenDatabase has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2065.aspx
        'db = DBEngineHelper.Instance(Artinsoft.VB6.DB.AdoFactoryManager.GetFactory()).OpenDatabase(RUTA_BD)
        db = DBEngineHelper.Instance(Artinsoft.VB6.DB.AdoFactoryManager.GetFactory()).OpenDatabase(RUTA_BD)


        If RUTA_BD_RCP <> "" And chkCargaServ.CheckState = CheckState.Checked Then
            tbProceso.Text = "Lendo datos de RCP_GRP_TMP no servidor"
            'UPGRADE_WARNING: (2065) DAO.Database method DBEngine.OpenDatabase has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2065.aspx
            dbRCP = DBEngineHelper.Instance(Artinsoft.VB6.DB.AdoFactoryManager.GetFactory()).OpenDatabase(RUTA_BD_RCP)

            rs = dbRCP.OpenRecordset("SELECT * FROM rcp_grp_tmp")
            If rs.EOF Then
                If MessageBox.Show("Non hai cursos para tratar, ¿quere sair?", Application.ProductName, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
                    rs.Close()
                    Artinsoft.VB6.DB.TransactionManager.DeEnlist(dbRCP.Connection)
                    dbRCP.Close()
                    Environment.Exit(0)
                End If
            Else
                'Copiamos la informacion a local
                Dim TempCommand As DbCommand = db.Connection.CreateCommand()
                TempCommand.CommandText = "DELETE FROM rcp_grp_tmp"
                TempCommand.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                TempCommand.ExecuteNonQuery()
                If Not rs.EOF Then
                    rs.MoveLast()
                    pbCursos.Maximum = rs.RecordCount
                    rs.MoveFirst()
                    pbCursos.Value = 0
                End If

                On Error Resume Next
                While Not rs.EOF
                    tbActual.Text = rs("descripcion")
                    tbActual.Refresh()
                    Information.Err().Clear()
                    'sCad = "INSERT INTO rcp_grp_tmp VALUES ('"
                    'If Err.Number > 0 Then MsgBox Err.Description & " 1"
                    'sCad = sCad & Trim$(rs.Fields("id_proceso"))
                    'If Err.Number > 0 Then MsgBox Err.Description & " 2"
                    'sCad = sCad & "'," & rs.Fields("id_fila") & ",'"
                    'If Err.Number > 0 Then MsgBox Err.Description & "3"
                    'sCad = sCad & EliminaCarProhibidos(Trim$(rs.Fields("descripcion")))
                    'If Err.Number > 0 Then MsgBox Err.Description & "4"
                    'sCad = sCad & "','" & Trim$(rs.Fields("descripcion_mod")) & "',"
                    'If Err.Number > 0 Then MsgBox Err.Description & "5"
                    'sCad = sCad & NumeroSinNulos(rs.Fields("codigo"))
                    'If Err.Number > 0 Then MsgBox Err.Description & "6"
                    'sCad = sCad & ",'" & Trim$(rs.Fields("subtipo")) & "','"
                    'If Err.Number > 0 Then MsgBox Err.Description & "7"
                    'sCad = sCad & Trim$(rs.Fields("contenido")) & "','" & rs.Fields("es_padre") & "','"
                    'If Err.Number > 0 Then MsgBox Err.Description & "8"
                    'sCad = sCad & rs.Fields("porcent_simil") & "',"
                    'If Err.Number > 0 Then MsgBox Err.Description & "9"
                    'sCad = sCad & rs.Fields("num_palab_tot") & ","
                    'If Err.Number > 0 Then MsgBox Err.Description & "10"
                    'sCad = sCad & rs.Fields("num_palab_sim")
                    'If Err.Number > 0 Then MsgBox Err.Description & "11"
                    'sCad = sCad & "," & rs.Fields("num_palab_maes")
                    'If Err.Number > 0 Then MsgBox Err.Description & "12"
                    'sCad = sCad & "," & rs.Fields("codigo_maestro") & ")"
                    'If Err.Number > 0 Then MsgBox Err.Description & "13"

                    'db.Execute sCad
                    Dim TempCommand_2 As DbCommand = db.Connection.CreateCommand()
                    TempCommand_2.CommandText = "INSERT INTO rcp_grp_tmp VALUES ('" & _
                                                rs("id_proceso").Trim() & "'," & rs("id_fila") & ",'" & _
                                                EliminaCarProhibidos(rs("descripcion").Trim()) & "','" & rs("descripcion_mod").Trim() & "'," & CStr(NumeroSinNulos(rs("codigo"))) & ",'" & rs("subtipo").Trim() & "','" & _
                                                rs("contenido").Trim() & "','" & rs("es_padre") & "','" & _
                                                rs("porcent_simil") & "'," & rs("num_palab_tot") & "," & _
                                                rs("num_palab_sim") & "," & rs("num_palab_maes") & "," & rs("codigo_maestro") & ")"
                    TempCommand_2.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                    TempCommand_2.ExecuteNonQuery()
                    rs.MoveNext()
                    pbCursos.Value += 1
                    Application.DoEvents()
                End While
                On Error GoTo 0
            End If

            Artinsoft.VB6.DB.TransactionManager.DeEnlist(dbRCP.Connection)
            dbRCP.Close()
        End If

        rs = db.OpenRecordset("SELECT * FROM rcp_grp_tmp")
        If rs.EOF Then
            If MessageBox.Show("Non hai cursos para tratar, ¿quere procesar os cursos xa importados?", Application.ProductName, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.No Then
                Dim TempCommand_3 As DbCommand = db.Connection.CreateCommand()
                TempCommand_3.CommandText = "DELETE FROM cursos"
                TempCommand_3.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                TempCommand_3.ExecuteNonQuery()
            Else
                Dim TempCommand_4 As DbCommand = db.Connection.CreateCommand()
                TempCommand_4.CommandText = "UPDATE cursos SET control = 0, palabras = 0"
                TempCommand_4.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                TempCommand_4.ExecuteNonQuery()
            End If
        Else
            Dim TempCommand_5 As DbCommand = db.Connection.CreateCommand()
            TempCommand_5.CommandText = "DELETE FROM cursos"
            TempCommand_5.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
            TempCommand_5.ExecuteNonQuery()
        End If
        If Not rs.EOF Then
            rs.MoveLast()
            pbCursos.Maximum = rs.RecordCount
            rs.MoveFirst()
            pbCursos.Value = 0
        End If
        While Not rs.EOF
            If rs("id_proceso").Trim().ToUpper() = Entidades.Text.ToUpper Then
                tbActual.Text = rs("descripcion")
                tbNumGrupos.Text = rs("id_fila")

                tbNumGrupos.Refresh()
                tbActual.Refresh()
                Inc(lNumCads)
                Dim TempCommand_6 As DbCommand = db.Connection.CreateCommand()
                TempCommand_6.CommandText = "INSERT INTO cursos VALUES (" & rs("id_fila") & ",'" & EliminaCarProhibidos(rs("descripcion")) & "', '',0,0," & CStr(NumeroSinNulos(rs("codigo"))) & ",'" & rs("contenido").Trim() & "')"
                TempCommand_6.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                TempCommand_6.ExecuteNonQuery()
            End If
            rs.MoveNext()
            pbCursos.Value += 1
            Application.DoEvents()
        End While
        rs.Close()
        Artinsoft.VB6.DB.TransactionManager.DeEnlist(db.Connection)
        db.Close()
        tbProceso.Text = "Operación Finalizada"
    End Sub

    Private Sub cmdCorregirCursos_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCorregirCursos.Click
        Dim rsPal As DAORecordSetHelper
        Dim sCad, sCad_Corregida As String
        Dim i As Integer
        Dim aCad(100) As String
        Dim iCont, iContPalabras, iContIgnorar As Integer
        Dim sPalabra As String = ""
        Dim lCodPalabra As Integer
        Dim iPalCorregidas As Integer

        tbProceso.Text = "Inicializando datos"
        tbProceso.Refresh()
        db = DBEngineHelper.Instance(Artinsoft.VB6.DB.AdoFactoryManager.GetFactory()).OpenDatabase(RUTA_BD)
        'Eliminamos los grupos de los cursos nuevos para comenzar un nuevo agrupamiento
        EjecutarQuery("UPDATE cursos SET cod_id_grupo = 0 WHERE contenido = 'NUEVO'", db)
        'Eliminamos la lista de palabras a ignorar
        'UPGRADE_WARNING: (2065) DAO.Database method DBEngine.OpenDatabase has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2065.aspx
        Dim TempCommand As DbCommand = db.Connection.CreateCommand()
        TempCommand.CommandText = "UPDATE cursos SET denom_curso_corregida = ''"
        TempCommand.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
        TempCommand.ExecuteNonQuery()
        Dim TempCommand_2 As DbCommand = db.Connection.CreateCommand()
        TempCommand_2.CommandText = "DELETE FROM palabras"
        TempCommand_2.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
        TempCommand_2.ExecuteNonQuery()
        Dim TempCommand_3 As DbCommand = db.Connection.CreateCommand()
        TempCommand_3.CommandText = "DELETE FROM palabras_cursos"
        TempCommand_3.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
        TempCommand_3.ExecuteNonQuery()

        tbProceso.Text = "Descompoñendo cursos en palabras"
        tbProceso.Refresh()

        Dim lContPalabra As Integer = 0
        Dim rs As DAORecordSetHelper = db.OpenRecordset("SELECT DISTINCT cod_id_curso, denom_curso, denom_curso_corregida FROM cursos ORDER BY denom_curso")
        If Not rs.EOF Then
            rs.MoveLast()
            pbCursos.Maximum = rs.RecordCount
            rs.MoveFirst()
            pbCursos.Value = 0
        End If
        While Not rs.EOF
            pbCursos.Value += 1
            Application.DoEvents()
            If rs("denom_curso_corregida") = "" Then
                sCad = rs("denom_curso")
                'Eliminamos las comillas, los espacios contiguos y separamos palabras de caracteres no alfabéticos
                sCad = CorregirCadena(sCad)
                sCad = ConversionUnidades(sCad)
                iCont = DividirCampo(sCad, aCad, " ")
                i = 0
                iPalCorregidas = 0
                sCad_Corregida = ""
                While i < iCont
                    sPalabra = aCad(i)
                    'Comprobamos si hay que ignorar la palabra
                    rsPal = db.OpenRecordset("SELECT COUNT(*) FROM ignorar WHERE palabra = '" & sPalabra & "' AND (posicion = 0 OR posicion = " & CStr(i + 1) & ")")
                    iContIgnorar = rsPal(0)
                    rsPal.Close()
                    rsPal = db.OpenRecordset("SELECT COUNT(*) FROM palabras_no_discriminantes WHERE palabra = '" & sPalabra & "' AND (posicion = 0 OR posicion = " & CStr(i + 1) & ")")
                    iContIgnorar += rsPal(0)
                    If iContIgnorar = 0 Then
                        rsPal.Close()
                        If Not EsNumeroRomano(sPalabra) Then
                            If sCad_Corregida = "" Then
                                sCad_Corregida = sPalabra.Trim()
                            Else
                                sCad_Corregida = sCad_Corregida & " " & sPalabra.Trim()
                            End If
                            'Localizamos características de la palabra
                            Dim iG(4, 2) As Single, iGd(4, 2) As Single
                            ExtraerCaracteristicas(sPalabra, iG, iGd)
                            rsPal = db.OpenRecordset("SELECT cod_id_palabra FROM palabras WHERE palabra = '" & sPalabra & "'")

                            If rsPal.EOF Then
                                Inc(lContPalabra)
                                Dim TempCommand_4 As DbCommand = db.Connection.CreateCommand()
                                'TempCommand_4.CommandText = "INSERT INTO palabras VALUES (" & lContPalabra & ", '" & sPalabra & "',1,0)"
                                Dim sSQL As String, j As Integer
                                sSQL = "INSERT INTO palabras VALUES (" & lContPalabra & ", '" & sPalabra & "',1,0"
                                For j = 0 To 3
                                    sSQL = sSQL + "," + (iG(j, 0) + iG(j, 1)).ToString
                                Next
                                For j = 0 To 3
                                    sSQL = sSQL + "," + (iGd(j, 0) + iGd(j, 1)).ToString
                                Next
                                sSQL = sSQL + "," + sPalabra.Length.ToString + ")"
                                TempCommand_4.CommandText = sSQL
                                TempCommand_4.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                                TempCommand_4.ExecuteNonQuery()
                                lCodPalabra = lContPalabra
                            Else
                                lCodPalabra = rsPal("cod_id_palabra")
                                Dim TempCommand_5 As DbCommand = db.Connection.CreateCommand()
                                TempCommand_5.CommandText = "UPDATE palabras SET contador = contador + 1 WHERE cod_id_palabra = " & lCodPalabra
                                TempCommand_5.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                                TempCommand_5.ExecuteNonQuery()
                            End If
                            Dim TempCommand_6 As DbCommand = db.Connection.CreateCommand()
                            TempCommand_6.CommandText = "INSERT into palabras_cursos VALUES (" & lCodPalabra & "," & rs("cod_id_curso") & "," & CStr(iPalCorregidas) & ")"
                            TempCommand_6.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                            TempCommand_6.ExecuteNonQuery()
                            Inc(iPalCorregidas)
                            rsPal.Close()
                        End If
                    Else
                        rsPal.Close()
                    End If
                    Inc(i)
                End While
                iContPalabras = DividirCampo(sCad_Corregida, aCad, " ")
                Dim TempCommand_7 As DbCommand = db.Connection.CreateCommand()
                TempCommand_7.CommandText = "UPDATE cursos SET palabras = " & iContPalabras & " ,denom_curso_corregida = '" & sCad_Corregida & "' WHERE denom_curso = '" & rs("denom_curso") & "' AND cod_id_curso = " & rs("cod_id_curso")
                TempCommand_7.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                TempCommand_7.ExecuteNonQuery()
                tbActual.Text = sCad_Corregida
                tbActual.Refresh()
                Application.DoEvents()
            End If
            rs.MoveNext()
        End While
        rs.Close()
        Artinsoft.VB6.DB.TransactionManager.DeEnlist(db.Connection)
        db.Close()
        tbProceso.Text = "Operación Finalizada"
        Exit Sub

    End Sub
    Function ConversionUnidades(ByVal sCad As String) As String
        Dim aCad() As String
        Dim sNum As String, sLetras As String = ""
        Dim iNum As Integer

        aCad = sCad.Split(" ")
        For i As Integer = 0 To aCad.Length - 1
            If aCad(i).Trim() = "" Then
                sNum = Numeros(aCad(i))
                sLetras = Letras(aCad(i))

                If sNum = aCad(i) Then
                    sLetras = ""
                    'Toda la palabra es un número, recuperamos la unidad de la siguiente palabra no vacía
                    For j As Integer = i + 1 To aCad.Length - 1
                        If aCad(j).Trim() <> "" Then
                            sLetras = aCad(j)
                            Exit For
                        End If
                    Next
                End If

                If sNum <> "" And sLetras <> "" Then
                    iNum = Val(sNum)
                    Select Case sLetras
                        Case "CC"
                            sLetras = "ML"
                        Case "KG"
                            sLetras = "GR"
                            iNum *= 1000
                        Case "G"
                            sLetras = "GR"
                        Case "L"
                            iNum *= 1000
                        Case "U", "UN", "COMP"
                            sLetras = "UNIDADES"
                    End Select
                End If
            End If
        Next

        Return iNum.ToString() & " " & sLetras
    End Function
    Function Numeros(ByVal sCad As String) As String
        Dim sNum As String = ""
        For i As Integer = 0 To sCad.Length - 1
            If IsNumeric(sCad(i)) Then
                sNum &= sCad(i)
            Else
                Exit For
            End If
        Next
        Return sNum
    End Function
    Function Letras(ByVal sCad As String) As String
        Dim sLetras As String = ""
        For i As Integer = sCad.Length - 1 To 0 Step -1
            If sCad(i) >= "A" And sCad(i) <= "Z" Then
                sLetras = sCad(i) & sLetras
            Else
                Exit For
            End If
        Next
        Return sLetras
    End Function
    Function EsNumeroRomano(ByRef sCad As String) As Boolean
        Dim result As Boolean = False

        result = True
        For Each sCar As Char In sCad
            If sCar <> "V" And sCar <> "X" And sCad <> "L" And sCar <> "I" Then
                Return False
            End If
        Next sCar
        Return result
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (cmdFinalizarGrupos_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdFinalizarGrupos_Click()
    'Dim rsCurso As DAORecordSetHelper
    'Para todos los cursos que se encuentren en varios grupos, debemos colocarlo en el que más se le parezca
    'Dim rs As DAORecordSetHelper = db.OpenRecordset("SELECT cod_id_curso, COUNT(*) FROM grupos_cursos gc WHERE curso_maestro = 0 GROUP BY cod_id_curso HAVING COUNT(*) > 1")
    'While Not rs.EOF
    'rsCurso = db.OpenRecordset("SELECT * FROM grupos_cursos WHERE")
    '
    'rs.MoveNext()
    'End While
    'rs.Close()
    'End Sub

    Private Sub cmdGenerarGrupos_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdGenerarGrupos.Click
        tbTime.Text = DateTimeHelper.Time.ToString("HH:mm:ss")
        Timer1.Enabled = True

        Entidades.Enabled = False
        cmdGenerarGrupos.Enabled = False
        pbTotal.Value = 1
        'cmdCopiarCursos_Click(cmdCopiarCursos, New EventArgs())
        If chkCargaServ.Checked = CheckState.Checked Then
            LeerCursosWS_Click(cmdCopiarCursos, New EventArgs())
        End If
        pbTotal.Value += 1
        cmdCorregirCursos_Click(cmdCorregirCursos, New EventArgs())
        pbTotal.Value += 1
        cmdAgrupar_Click(cmdAgrupar, New EventArgs())
        pbTotal.Value += 1
        PropagarGruposWS_Click(cmdPropagarGrupos, New EventArgs())
        pbTotal.Value += 1
        cmdGenerarGrupos.Enabled = True
        Entidades.Enabled = True

        If Information.IsDate(tbTime.Text) Then
            tbTimeFin.Text = CStr(DateAndTime.DateDiff("n", CDate(tbTime.Text), CDate(DateTimeHelper.Time.ToString("HH:mm:ss")), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1))
        End If

        Timer1.Enabled = False
        MessageBox.Show("Operación Finalizada", "Mesaxe", MessageBoxButtons.OK)
    End Sub

    Private Sub cmdGenerarListasPalabras_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdGenerarListasPalabras.Click
        'UPGRADE_WARNING: (2065) DAO.Database method DBEngine.OpenDatabase has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2065.aspx
        db = DBEngineHelper.Instance(Artinsoft.VB6.DB.AdoFactoryManager.GetFactory()).OpenDatabase(RUTA_BD)

        Artinsoft.VB6.DB.TransactionManager.DeEnlist(db.Connection)
        db.Close()
    End Sub

    Public Function DividirCampo(ByVal sCampo As String, ByRef aeMail() As String, ByRef sSeparador As String) As Integer

        Dim result As Integer = 0
        result = 1
        Dim j As Integer = (sCampo.IndexOf(sSeparador) + 1)
        While j > 0
            aeMail(result - 1) = Strings.Mid(sCampo, 1, j - 1).Trim()
            sCampo = Strings.Mid(sCampo, j + sSeparador.Length)
            j = (sCampo.IndexOf(sSeparador) + 1)
            Inc(result)
        End While
        aeMail(result - 1) = sCampo

        Return result
    End Function

    Function Inc(ByRef i As Double) As Object
        i += 1
    End Function

    Private Sub cmdPropagarGrupos_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPropagarGrupos.Click
        Dim sSQL, sSQL2 As String
        Dim lGrupo As Integer
        Dim dbRCP As DAODatabaseHelper

        tbProceso.Text = "Propagando xeración de grupos"
        tbProceso.Refresh()

        'UPGRADE_WARNING: (2065) DAO.Database method DBEngine.OpenDatabase has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2065.aspx
        db = DBEngineHelper.Instance(Artinsoft.VB6.DB.AdoFactoryManager.GetFactory()).OpenDatabase(RUTA_BD)

        'Eliminamos todos los registros con grupo de rcp_grp_tmp
        If chkElimCursos.CheckState = True Then
            Dim TempCommand As DbCommand = db.Connection.CreateCommand()
            TempCommand.CommandText = "DELETE FROM rcp_grp_tmp WHERE codigo > 0"
            TempCommand.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
            TempCommand.ExecuteNonQuery()
        End If

        Dim rs As DAORecordSetHelper = db.OpenRecordset("SELECT c.cod_id_grupo, c.cod_id_curso, c.denom_curso_corregida FROM cursos c WHERE contenido = 'NUEVO' ORDER BY c.cod_id_curso")

        If Not rs.EOF Then
            rs.MoveLast()
            pbCursos.Maximum = rs.RecordCount
            rs.MoveFirst()
            pbCursos.Value = 0
        End If

        While Not rs.EOF

            Dim TempCommand_2 As DbCommand = db.Connection.CreateCommand()
            TempCommand_2.CommandText = "UPDATE rcp_grp_tmp SET codigo = " & _
                                        rs("cod_id_grupo") & ", descripcion_mod = '" & _
                                        rs("denom_curso_corregida") & "' WHERE id_fila = " & _
                                        rs("cod_id_curso")
            TempCommand_2.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
            TempCommand_2.ExecuteNonQuery()

            tbNumGrupos.Text = rs("cod_id_curso")
            Application.DoEvents()
            rs.MoveNext()
            pbCursos.Value += 1
        End While
        rs.Close()

        'Si algún curso queda sin grupo, su denominación no es válida y hay que dejarlo en un grupo el solo
        rs = db.OpenRecordset("SELECT MAX(codigo) AS max_codigo FROM rcp_grp_tmp")
        'UPGRADE_WARNING: (1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
        If Not Convert.IsDBNull(rs("max_codigo")) Then
            lGrupo = CInt(rs(0) + 1)
            rs.Close()
            If lGrupo > 900000 Then
                Interaction.MsgBox("Propagación de grupos no realizada", "ERROR")
                Exit Sub
            End If
            rs = db.OpenRecordset("SELECT id_fila FROM rcp_grp_tmp WHERE codigo IS NULL OR codigo = 0")
            While Not rs.EOF
                Dim TempCommand_3 As DbCommand = db.Connection.CreateCommand()
                TempCommand_3.CommandText = "UPDATE rcp_grp_tmp SET codigo = " & lGrupo & _
                                            " WHERE id_fila = " & rs("id_fila")
                TempCommand_3.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                TempCommand_3.ExecuteNonQuery()
                Inc(lGrupo)
                rs.MoveNext()
            End While
        End If
        rs.Close()

        'Propagamos las características
        rs = db.OpenRecordset("SELECT c.cod_id_grupo, c.cod_id_curso, gc.equiv, gc.num_palabras, gc.num_palabras_maestro, " & _
             "gc.porcentaje, gc.curso_maestro1, gc.cod_id_maestro, c.denom_curso_corregida FROM cursos c, grupos_cursos gc " & _
             "WHERE gc.cod_id_curso = c.cod_id_curso AND control = 1 AND contenido = 'NUEVO' ORDER BY c.cod_id_curso")
        If Not rs.EOF Then
            rs.MoveLast()
            pbCursos.Maximum = rs.RecordCount
            rs.MoveFirst()
            pbCursos.Value = 0
        End If

        While Not rs.EOF
            Dim TempCommand_4 As DbCommand = db.Connection.CreateCommand()
            TempCommand_4.CommandText = "UPDATE rcp_grp_tmp SET subtipo = '-', es_padre = " & _
                                        (IIf(rs("curso_maestro") = 1, "'S'", "'N'")) & ",porcent_simil = '" & rs("porcentaje") & "', num_palab_tot = " & _
                                        rs("num_palabras") & ",num_palab_sim =" & rs("equiv") & _
                                        ",num_palab_maes=" & rs("num_palabras_maestro") & _
                                        ",codigo_maestro = " & rs("cod_id_maestro") & _
                                        ",descripcion_mod = '" & rs("denom_curso_corregida") & "'" & _
                                        " WHERE descripcion_mod = '" & rs("denom_curso_corregida") & "'"
            TempCommand_4.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
            TempCommand_4.ExecuteNonQuery()

            tbNumGrupos.Text = rs("cod_id_curso")
            pbCursos.Value += 1
            tbNumGrupos.Refresh()
            rs.MoveNext()
        End While
        rs.Close()

        tbProceso.Text = "Estableciendo maestros en todos los grupos"
        Application.DoEvents()
        'Antes de propagar nos aseguramos que todos los grupos tienen un padre
        rs = db.OpenRecordset("SELECT MAX(id_fila) AS cod_fila, codigo FROM rcp_grp_tmp g1 WHERE " & _
             " (SELECT count(*) FROM rcp_grp_tmp g2 WHERE g2.codigo = g1.codigo AND es_padre = 'S') = 0 GROUP BY codigo")
        While Not rs.EOF
            Dim TempCommand_5 As DbCommand = db.Connection.CreateCommand()
            TempCommand_5.CommandText = "UPDATE rcp_grp_tmp SET es_padre = 'S' WHERE codigo = " & _
                                        rs("codigo") & " AND id_fila = " & rs("cod_fila")
            TempCommand_5.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
            TempCommand_5.ExecuteNonQuery()
            rs.MoveNext()
        End While
        rs.Close()

        'propagamos los cambios a la copia principal
        Dim iErrores As Integer
        If RUTA_BD_RCP <> "" And chkCargaServ.CheckState = CheckState.Checked Then
            'UPGRADE_WARNING: (2065) DAO.Database method DBEngine.OpenDatabase has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2065.aspx
            dbRCP = DBEngineHelper.Instance(Artinsoft.VB6.DB.AdoFactoryManager.GetFactory()).OpenDatabase(RUTA_BD_RCP)

            tbProceso.Text = "Propagando rexistros pais"
            Application.DoEvents()
            'Primero propagamos los padres de cada grupo, si coinciden en registros ya presentes en RCP
            rs = db.OpenRecordset("SELECT * FROM rcp_grp_tmp WHERE contenido <> 'NUEVO' AND es_padre = 'S'")

            If Not rs.EOF Then
                rs.MoveLast()
                pbCursos.Maximum = rs.RecordCount
                rs.MoveFirst()
                pbCursos.Value = 0
            End If

            While Not rs.EOF
                sSQL = "UPDATE rcp_grp_tmp SET es_padre = '" & rs("es_padre") & "' WHERE id_fila = " & rs("id_fila")
                Debug.WriteLine(sSQL)
                iErrores = 0
                Do While iErrores <= LIMITE_REINTENTOS
                    If iErrores = LIMITE_REINTENTOS Then
                        If Interaction.MsgBox("Se han producido 5 errores accediendo a la base de datos, ¿Reintentar?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "AVISO") = System.Windows.Forms.DialogResult.Yes Then
                            iErrores = 0
                        Else
                            Exit Sub
                        End If
                    End If

                    On Error Resume Next
                    Dim TempCommand_6 As DbCommand = dbRCP.Connection.CreateCommand()
                    TempCommand_6.CommandText = sSQL2
                    TempCommand_6.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(dbRCP.Connection)
                    TempCommand_6.ExecuteNonQuery()
                    On Error GoTo 0
                    tbActual.Text = "Procesando " & rs("descripcion")
                    Application.DoEvents()

                    If Information.Err().Number = 0 Then
                        iErrores = 0
                        Exit Do
                    Else
                        iErrores += 1
                    End If
                Loop
                rs.MoveNext()
                pbCursos.Value += 1
                Application.DoEvents()
            End While
            rs.Close()

            tbProceso.Text = "Propagando características"
            Application.DoEvents()
            'Ahora propagamos las características a los resgistros nuevos
            rs = db.OpenRecordset("SELECT * FROM rcp_grp_tmp WHERE contenido = 'NUEVO'")

            If Not rs.EOF Then
                rs.MoveLast()
                pbCursos.Maximum = rs.RecordCount
                rs.MoveFirst()
                pbCursos.Value = 0
            End If

            While Not rs.EOF
                sSQL = "UPDATE rcp_grp_tmp SET descripcion_mod = '" & _
                       rs("descripcion_mod") & "',codigo = " & rs("codigo") & ",es_padre = '" & rs("es_padre") & _
                       "',porcent_simil = '" & rs("porcent_simil") & _
                       "',num_palab_tot = " & rs("num_palab_tot") & _
                       ",num_palab_sim= " & rs("num_palab_sim") & _
                       ",num_palab_maes = " & rs("num_palab_maes") & _
                       ",codigo_maestro = " & rs("codigo_maestro") & _
                       " WHERE id_fila = " & rs("id_fila")
                Debug.WriteLine(sSQL)

                iErrores = 0
                Do While iErrores <= LIMITE_REINTENTOS
                    If iErrores = LIMITE_REINTENTOS Then
                        If Interaction.MsgBox("Se han producido 5 errores accediendo a la base de datos, ¿Reintentar?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "AVISO") = System.Windows.Forms.DialogResult.Yes Then
                            iErrores = 0
                        Else
                            Exit Sub
                        End If
                    End If

                    On Error Resume Next
                    Dim TempCommand_7 As DbCommand = dbRCP.Connection.CreateCommand()
                    TempCommand_7.CommandText = sSQL
                    TempCommand_7.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(dbRCP.Connection)
                    TempCommand_7.ExecuteNonQuery()
                    On Error GoTo 0
                    tbActual.Text = "Procesando " & rs("descripcion")
                    Application.DoEvents()

                    If Information.Err().Number = 0 Then
                        iErrores = 0
                        Exit Do
                    Else
                        iErrores += 1
                    End If
                Loop

                rs.MoveNext()
                pbCursos.Value += 1
                Application.DoEvents()
            End While
            rs.Close()
            Artinsoft.VB6.DB.TransactionManager.DeEnlist(dbRCP.Connection)
            dbRCP.Close()
        End If

        tbProceso.Text = "Operación Finalizada"
    End Sub

    Private Sub cmdSim_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSim.Click
        Dim iHuecos As Integer
        tbHuecos.Text = CStr(Comparar(Text1.Text, Text2.Text))
        tbValor.Text = CStr(Comparar2(Text1.Text, Text2.Text))

    End Sub

    'UPGRADE_WARNING: (2080) Form_Load event was upgraded to Form_Load method and has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2080.aspx
    Private Sub Form_Load()
        Dim mml_FRASE0096 As String = ""

        Entidades.SelectedIndex = 0

        Dim iBDLon As Integer = SAV_VB_NETSupport.SafeNative.kernel32.GetPrivateProfileString("RUTA_BD", "SAC", C_RUTA_BD, sCad.Value, 255, "RCP.ini")
        If iBDLon = 0 Then
            Interaction.MsgBox("Parámetros RUTA_BD no encontrado", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, mml_FRASE0096)
            Environment.Exit(0)
        End If
        Dim i As Integer = (sCad.Value.IndexOf(Strings.Chr(0).ToString()) + 1)
        If i > 0 Then
            RUTA_BD = Strings.Mid(sCad.Value, 1, i - 1)
        End If
        iBDLon = SAV_VB_NETSupport.SafeNative.kernel32.GetPrivateProfileString("RUTA_BD_RCP", "SAC", C_RUTA_BD_RCP, sCad.Value, 255, "RCP.ini")
        If iBDLon = 0 Then
            Interaction.MsgBox("Parámetros RUTA_BD_RCP no encontrado", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, mml_FRASE0096)
            Environment.Exit(0)
        End If
        i = (sCad.Value.IndexOf(Strings.Chr(0).ToString()) + 1)
        If i > 0 Then
            RUTA_BD_RCP = Strings.Mid(sCad.Value, 1, i - 1)
        End If
        iBDLon = SAV_VB_NETSupport.SafeNative.kernel32.GetPrivateProfileString("OPCS_BD", "SAC", C_OPCS_BD, sCad.Value, 255, "RCP.ini")
        If iBDLon = 0 Then
            Interaction.MsgBox("Parámetros OPCS_BD no encontrado", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, mml_FRASE0096)
            Environment.Exit(0)
        End If
        i = (sCad.Value.IndexOf(Strings.Chr(0).ToString()) + 1)
        If i > 0 Then
            OPCS_BD = Strings.Mid(sCad.Value, 1, i - 1)
        End If

        Dim sPar() As String = Interaction.Command().Split("|"c)
        On Error Resume Next
        If sPar(0) <> "" Then
            RUTA_BD = sPar(0)
        End If
        If sPar(1) <> "" Then
            RUTA_BD_RCP = sPar(1)
        End If
        If sPar(2) <> "" Then
            OPCS_BD = sPar(2)
        End If
        If RUTA_BD = "" Or RUTA_BD_RCP = "" Or OPCS_BD = "" Then
            MessageBox.Show("Parámetros erróneos. Formato: <BD Local>|<DSN RCP>|ODBC;UID=<usr_rcp>;PWD=<pwd_rcp>", Application.ProductName)
            Environment.Exit(0)
        End If

    End Sub

    Private Sub Label1_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Label1.DoubleClick
        MessageBox.Show(Interaction.Command(), Application.ProductName)
    End Sub

    Private Sub Timer1_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Timer1.Tick
        If Information.IsDate(tbTime.Text) Then
            tbTimeFin.Text = CStr(DateAndTime.DateDiff("n", CDate(tbTime.Text), CDate(DateTimeHelper.Time.ToString("HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1))
        End If

    End Sub

    Function Comparar3(ByVal sCad1 As String, ByVal sCad2 As String) As Double
        Dim CompararX As Double
        Dim iPos As Integer
        Dim sCar As String = ""
        Dim dPuntos As Double

        Dim iLonCad1 As Integer = sCad1.Length
        Dim iLonCad2 As Integer = sCad2.Length
        Dim iDist As Integer = 0
        Dim iPosAnt As Integer = 1
        For i As Integer = 1 To iLonCad1
            If i <= iLonCad2 Then
                sCar = Strings.Mid(sCad1, i, 1)
                iPos = Strings.InStr(iPosAnt, sCad2, sCar)

                If iPos = i Then
                    'La letra está en la misma posición
                    dPuntos += 1
                    'Reseteamos el control de distancia
                    iDist = 0
                    'Comenzamos a comprobar a partir de la siguiente posición
                    iPosAnt = i + 1
                ElseIf iPos = i + iDist Then
                    'La letra está en la misma posición más el desplazamiento actual
                    dPuntos += 0.9
                    'Comenzamos a comprobar a partir de la siguiente posición más el desplazamiento
                    iPosAnt = i + iDist + 1
                ElseIf iPos <> 0 Then
                    'Cuanto más alejada está menos puntua
                    dPuntos += 1 / (Math.Abs(iPos - i) + 1)

                    'Establecemos esta distancia como fija y desplazamos la comprobación
                    If Math.Abs(iPos - i) <= 2 Then
                        iDist = (iPos - i)
                        'Comenzamos a comprobar a partir de la siguiente posición más el desplazamiento
                        iPosAnt = i + iDist + 1
                    Else
                        iPosAnt = i + 1
                    End If
                    'Si no hemos localizado la letra reseteamos el lugar de comienzo de búsqueda
                ElseIf iPosAnt > i Then
                    i -= 1
                    iPosAnt = i
                    iDist = 0
                End If
            End If

        Next

        If iLonCad2 > iLonCad1 Then
            CompararX = dPuntos / iLonCad1
            CompararX -= Math.Abs(iLonCad2 - iLonCad1) / iLonCad2
        Else
            CompararX = dPuntos / iLonCad2
            CompararX -= Math.Abs(iLonCad2 - iLonCad1) / iLonCad1
        End If
    End Function

    'Función original
    Function Comparar2(ByVal sCad1 As String, ByVal sCad2 As String) As Double
        Dim result As Double = 0
        Dim iPos As Integer
        Dim sCar As String = ""
        Dim dPuntos As Double
        Dim iDesplazamiento As Integer = 0
        Dim ip As Integer = 0

        Dim iLonCad1 As Integer = sCad1.Length
        Dim iLonCad2 As Integer = sCad2.Length

        ip = 1
        For i As Integer = 1 To iLonCad1
            If ip <= iLonCad2 Then
                sCar = Strings.Mid(sCad1, i, 1)
                iPos = Strings.InStr(ip, sCad2, sCar)
                If iPos > 0 Then
                    If iPos = i Then
                        'La letra está en la misma posición
                        dPuntos += 1
                        Inc(ip)
                    ElseIf Math.Abs(iPos - i) = iDesplazamiento Then
                        'La letra se encuentra en la misma posición pero desplazada
                        dPuntos += 0.9
                        Inc(ip)
                    Else
                        'la letra no está en la misma posición, puede deberse a un desplazamiento o a un error
                        iDesplazamiento = Math.Abs(iPos - i)
                        'Cuanto más alejada está menos puntua
                        dPuntos += 1 / (iDesplazamiento + 1)
                    End If
                Else
                    iDesplazamiento = 1
                    'Si no está la letra no suma pero no resta
                    'dPuntos -= 1 / iLonCad1 
                End If
            End If

        Next

        result = dPuntos / iLonCad1
        If iLonCad2 > iLonCad1 Then
            result -= (iLonCad2 - iLonCad1) / iLonCad2 / 4
        End If
        Return result
    End Function
    Function Comparar1(ByRef sCad1 As String, ByRef sCad2 As String) As Boolean
        Dim iHuecos As Integer

        Dim dValor As Double = Similares(sCad1, sCad2, 1, 1, 0, iHuecos)

        Return iHuecos * 2 < dValor
    End Function

    Function Comparar(ByVal sCad1 As String, ByVal sCad2 As String) As Double
        Dim dValor As Double

        Return Comparar2(sCad1, sCad2)

        'dValor = Comparar2(sCad2, sCad1)
        'If dValor > Comparar Then Comparar = dValor
    End Function

    Function EliminaCarProhibidos(ByRef sCad As String) As String
        Dim result As New StringBuilder()

        result = New StringBuilder("")
        For Each sCar As Char In sCad
            If Not (sCar = "'" Or sCar = """" Or Strings.Asc(sCar) = 10 Or Strings.Asc(sCar) = 13) Then
                result.Append(sCar)
            End If
        Next sCar
        Return result.ToString()
    End Function

    Function NumeroSinNulos(ByVal sCad As Object) As Integer
        On Error GoTo error_Renamed
        Dim dbNumericTemp As Double
        If IsNumeric(sCad) Then
            Return CInt(Val(sCad.ToString))
        Else
            Return 0
        End If
error_Renamed:
        MessageBox.Show(Information.Err().Description & " - " & sCad.Value, Application.ProductName)
        Return 0
    End Function

    Function CompruebaSiAsigGrupoRCP(ByRef lCodGrupoRCP As Integer, ByRef lCodNuevoMaestro As Integer, ByRef iNumPalabrasMaestro As Integer) As Boolean
        Dim result As Boolean = False
        Dim rsBuscar As DAORecordSetHelper
        Dim lCodViejoMaestro As Integer
        Dim sNuevoMaestro, sViejoMaestro As String
        Dim iNumPalabrasViejoMaestro As Integer


        'Si hay más de una palabra, el encaje es mucho más preciso y se considera válido
        If iNumPalabrasMaestro = 1 Then
            'Antes comprobamos el grupo ya ha sido recalificado a un grupo RCP
            rsBuscar = db.OpenRecordset("SELECT MIN(cod_id_maestro) AS min_cod_id_maestro FROM grupos_cursos WHERE cod_id_grupo = " & lCodGrupoRCP)
            If Not rsBuscar.EOF Then
                'UPGRADE_WARNING: (1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                If Not Convert.IsDBNull(rsBuscar("min_cod_id_maestro")) Then
                    lCodViejoMaestro = rsBuscar("min_cod_id_maestro")
                    'Ya hay un grupo con el mismo código RCP => dos cursos del mismo grupo han sido seleccionados como maestros en <> grupos
                    'Comprobamos si el maestro del curso que pretendemos agrupar se parece al maestro ya agrupado
                    rsBuscar.Close()

                    rsBuscar = db.OpenRecordset("SELECT denom_curso_corregida FROM cursos WHERE cod_id_curso = " & lCodNuevoMaestro)
                    If Not rsBuscar.EOF Then
                        sNuevoMaestro = rsBuscar("denom_curso_corregida")
                    End If
                    rsBuscar.Close()
                    rsBuscar = db.OpenRecordset("SELECT denom_curso_corregida,palabras FROM cursos WHERE cod_id_curso = " & lCodViejoMaestro)
                    If Not rsBuscar.EOF Then
                        sViejoMaestro = rsBuscar("denom_curso_corregida")
                        iNumPalabrasViejoMaestro = rsBuscar("palabras")
                    End If
                    rsBuscar.Close()

                    'Si el nuevo maestro tiene una palabra y el viejo más de una, consideramos el encaje correcto, ya
                    ' que la palabra del nuevo debiera ser una de las del viejo
                    If iNumPalabrasViejoMaestro = 1 Then
                        If Comparar(sViejoMaestro, sNuevoMaestro) > PORCENTAJE_PALABRA_SIMILAR Then
                            result = True
                        End If
                    Else
                        result = True
                    End If
                Else
                    result = True
                End If
            Else
                result = True
            End If
        Else
            result = True
        End If
        Return result
    End Function

    Private Sub LeerCursosWS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LeerCursosWS.Click
        RecuperarCursosWS()
    End Sub



    Private Sub RecuperarCursosWS()
        Dim sCursos As String
        Dim id As Long
        Dim iNumCursos As Integer = LEER_NUM_CURSOS
        Dim iEstado As Integer = EST_NORMAL
        Dim sErrores As String = "id_fila = "
        Dim lIdFilaCursos As Long
        Dim valor(11, 10) As String
        Dim rs As DAORecordSetHelper

        tbProceso.Text = "Lendo datos de RCP_GRP_TMP"
        tbProceso.Refresh()
        Dim lNumGrupos As Integer = 0
        db = DBEngineHelper.Instance(Artinsoft.VB6.DB.AdoFactoryManager.GetFactory()).OpenDatabase(RUTA_BD)
        If chkCargaServ.Checked Then

            Dim TempCommand As DbCommand = db.Connection.CreateCommand()
            TempCommand.CommandText = "DELETE FROM rcp_grp_tmp"
            TempCommand.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
            TempCommand.ExecuteNonQuery()
            TempCommand.Dispose()

            id = 0
            pbCursos.Maximum = RecuperarNumElementos(id, Entidades.Text)
            pbCursos.Value = 0
            Do
                Try
                    sCursos = RecuperarCursos(id, iNumCursos, Entidades.Text)

                    id = 0
                    iCont = 0
                    ifila = -1
                    iCol = 0
                    RecuperarInformación(sCursos, iCont, id, db)
                Catch ex As Exception
                    'MessageBox.Show(ex.Message)
                    If iEstado = EST_NORMAL Then
                        iNumCursos = 1
                        iEstado = EST_BUSCANDO_ERROR
                    ElseIf iEstado = EST_BUSCANDO_ERROR Then
                        'Localizamos el elemento que da el error
                        lIdFilaCursos = RecuperarFilaCurso(id, Entidades.Text)
                        id = lIdFilaCursos
                        'Hemos localizado un cursos con errores, lo almacenamos y pasamos al siguiente en modo normal
                        If sErrores <> "id_fila = " Then sErrores = sErrores + ","
                        sErrores = sErrores & lIdFilaCursos
                        m_sLog = "Cursos non recuperados do servidor: " & sErrores & vbCrLf
                        iEstado = EST_NORMAL
                        iNumCursos = LEER_NUM_CURSOS
                    End If

                End Try
            Loop While id > 0

        End If

        'Transferimos la información a las tablas de trabajo
        rs = db.OpenRecordset("SELECT * FROM rcp_grp_tmp")
        If rs.EOF Then
            If MessageBox.Show("Non hai cursos para tratar, ¿quere procesar os cursos xa importados?", Application.ProductName, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.No Then
                Dim TempCommand_3 As DbCommand = db.Connection.CreateCommand()
                TempCommand_3.CommandText = "DELETE FROM cursos"
                TempCommand_3.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                TempCommand_3.ExecuteNonQuery()
            Else
                Dim TempCommand_4 As DbCommand = db.Connection.CreateCommand()
                TempCommand_4.CommandText = "UPDATE cursos SET control = 0, palabras = 0"
                TempCommand_4.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                TempCommand_4.ExecuteNonQuery()
            End If
        Else
            Dim TempCommand_5 As DbCommand = db.Connection.CreateCommand()
            TempCommand_5.CommandText = "DELETE FROM cursos"
            TempCommand_5.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
            TempCommand_5.ExecuteNonQuery()
        End If
        If Not rs.EOF Then
            rs.MoveLast()
            pbCursos.Maximum = rs.RecordCount
            rs.MoveFirst()
            pbCursos.Value = 0
        End If
        While Not rs.EOF
            If rs("id_proceso").Trim().ToUpper() = Entidades.Text.ToUpper Then
                tbActual.Text = rs("descripcion")
                tbNumGrupos.Text = rs("id_fila")

                tbNumGrupos.Refresh()
                tbActual.Refresh()
                Inc(lNumCads)
                Dim TempCommand_6 As DbCommand = db.Connection.CreateCommand()
                TempCommand_6.CommandText = "INSERT INTO cursos VALUES (" & rs("id_fila") & ",'" & EliminaCarProhibidos(rs("descripcion")) & "', '',0,0," & CStr(NumeroSinNulos(rs("codigo"))) & ",'" & rs("contenido").Trim() & "')"
                TempCommand_6.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                TempCommand_6.ExecuteNonQuery()
            End If
            rs.MoveNext()
            pbCursos.Value += 1
            Application.DoEvents()
        End While
        rs.Close()
        Artinsoft.VB6.DB.TransactionManager.DeEnlist(db.Connection)
        db.Close()
        tbProceso.Text = "Operación Finalizada"

    End Sub
    Sub RecuperarInformación(ByVal sCursos As String, ByRef iCont As Integer, ByRef id As Long, ByVal db As DAODatabaseHelper)
        Dim Tag As String = ""
        Dim bCierre As Boolean
        Dim iRes As Integer
        Dim sInsert As String
        Dim aCol(8) As String
        Dim sCampos As String = "id_proceso,id_fila,descripcion,descripcion_mod,codigo,subtipo,contenido,es_padre,porcent_simil,num_palab_tot,num_palab_sim,num_palab_maes,codigo_maestro"

        Dim TempCommand As DbCommand = db.Connection.CreateCommand()

        While iCont < sCursos.Length
            Tag = ""
            iRes = LeerTag(sCursos, iCont, Tag, bCierre)
            If iRes = TAG_ERROR Then
                'No hay un tag, hay información que debemos recuperar
                RecuperarInformacion(sCursos, iCont)
            Else
                If Tag = "fila" Then
                    If bCierre Then
                        'Hemos cerrado una fila y la recuperamos a nuestra base de datos
                        Inc(pbCursos.Value)

                        iCol = 0
                        sInsert = "INSERT INTO rcp_grp_tmp (id_proceso,id_fila,descripcion,codigo,contenido,es_padre,codigo_maestro) VALUES ('" + Entidades.Text.Trim().ToUpper + "'," +
                            aCol(0) + ",""" + CorregirDenominacion(aCol(1)) + """," + aCol(2) + ",'" + aCol(3) + "','" + aCol(4) + "'," + aCol(5) + ")"
                        TempCommand.CommandText = sInsert
                        TempCommand.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                        TempCommand.ExecuteNonQuery()
                    ElseIf ifila > 0 Then
                        Inc(ifila)
                    Else
                        ifila = 1
                    End If
                ElseIf Tag = "columna" And Not bCierre And ifila >= 1 Then
                    aCol(iCol) = RecuperarInformacion(sCursos, iCont)
                    If iCol = 0 Then
                        id = aCol(iCol)
                        tbNumAgrupaciones.Text = id
                        tbNumAgrupaciones.Refresh()
                    End If
                    Inc(iCol)
                End If
            End If
        End While

    End Sub

    Function RecuperarInformacion(ByVal sCad As String, ByRef iCont As Integer)
        Dim c As Char

        RecuperarInformacion = ""
        Do
            c = sCad.Substring(iCont, 1)
            If c <> "<" Then
                RecuperarInformacion = RecuperarInformacion + c
                Inc(iCont)
            End If
        Loop While c <> "<"
    End Function

    Function LeerTag(ByVal sCad As String, ByRef iCont As Integer, ByRef Tag As String, ByRef bCierre As Boolean) As Integer
        Dim c As Char
        Dim FinTag As Boolean
        Dim bSalir As Boolean = False
        Dim iContTag As Integer

        iContTag = iCont
        bCierre = False
        Tag = ""
        While Not bSalir
            c = sCad.Substring(iCont, 1)
            If c <> "<" And c <> "/" And c <> ">" Then
                Tag = Tag + c
            End If
            If iCont - iContTag = 0 Then
                If c <> "<" Then
                    LeerTag = TAG_ERROR
                    Exit Function
                End If
            ElseIf iCont - iContTag = 1 Then
                If c = "/" Then
                    bCierre = True
                End If
            Else
                If c = ">" Then
                    bSalir = True
                End If
            End If
            Inc(iCont)
        End While
        LeerTag = TAG_OK
    End Function

    Function RecuperarCursos(ByVal id_fila_ini As Long, ByVal iNumCursos As Integer, ByVal sTipo As String) As String
        Dim WSc As New SAC.WS.SafefpWSClient

        'Recuperamos los cursos en grupos de 10 elementos
        'RecuperarCursos = WSc.procesarQuery("FC", "KoJR6p64bPxtbAV", "SEL_GRP_TMP", "<datos><campo><tipo>int</tipo><valor>10</valor></campo><campo><valor>cursos</valor></campo><campo><tipo>int</tipo><valor>0</valor></campo></datos>")
        RecuperarCursos = WSc.procesarQuery("FC", "KoJR6p64bPxtbAV", "SEL_GRP_TMP", "<datos><campo><tipo>int</tipo><valor>" + iNumCursos.ToString + "</valor></campo><campo><valor>" + sTipo.ToLower + "</valor></campo><campo><tipo>int</tipo><valor>" + id_fila_ini.ToString + "</valor></campo></datos>")
    End Function
    Function RecuperarFilaCurso(ByVal id_fila_ini As Long, ByVal sTipo As String) As Long
        Dim WSc As New SAC.WS.SafefpWSClient
        Dim sCad As String
        Dim valor(2, 2) As String
        Dim iFila As Integer = 0, iCol As Integer = 0

        'Recuperamos los cursos en grupos de 10 elementos
        'RecuperarCursos = WSc.procesarQuery("FC", "KoJR6p64bPxtbAV", "SEL_GRP_TMP", "<datos><campo><tipo>int</tipo><valor>10</valor></campo><campo><valor>cursos</valor></campo><campo><tipo>int</tipo><valor>0</valor></campo></datos>")
        sCad = WSc.procesarQuery("FC", "KoJR6p64bPxtbAV", "SEL_GRP_TMP_MIN", "<datos><campo><valor>" + sTipo.ToLower + "</valor></campo><campo><tipo>int</tipo><valor>" + id_fila_ini.ToString + "</valor></campo></datos>")
        RecuperarValores(sCad, valor, iFila, iCol)

        RecuperarFilaCurso = valor(1, 0)

    End Function
    Function RecuperarNumElementos(ByVal id_fila_ini As Long, ByVal sTipo As String) As Long
        Dim WSc As New SAC.WS.SafefpWSClient
        Dim sCad As String
        Dim valor(2, 2) As String
        Dim iFila As Integer = 0, iCol As Integer = 0

        'Recuperamos los cursos en grupos de 10 elementos
        'RecuperarCursos = WSc.procesarQuery("FC", "KoJR6p64bPxtbAV", "SEL_GRP_TMP", "<datos><campo><tipo>int</tipo><valor>10</valor></campo><campo><valor>cursos</valor></campo><campo><tipo>int</tipo><valor>0</valor></campo></datos>")
        sCad = WSc.procesarQuery("FC", "KoJR6p64bPxtbAV", "SEL_GRP_TMP_COUNT", "<datos><campo><valor>" + sTipo.ToLower + "</valor></campo><campo><tipo>int</tipo><valor>" + id_fila_ini.ToString + "</valor></campo></datos>")
        RecuperarValores(sCad, valor, iFila, iCol)

        RecuperarNumElementos = valor(1, 0)

    End Function
    Sub ActualizarRCPPadre(ByVal id_fila_ini As Long, ByVal sPadre As String)
        Dim WSc As New SAC.WS.SafefpWSClient
        Dim sCad As String

        'Recuperamos los cursos en grupos de 10 elementos
        'RecuperarCursos = WSc.procesarQuery("FC", "KoJR6p64bPxtbAV", "SEL_GRP_TMP", "<datos><campo><tipo>int</tipo><valor>10</valor></campo><campo><valor>cursos</valor></campo><campo><tipo>int</tipo><valor>0</valor></campo></datos>")
        sCad = WSc.procesarQuery("FC", "KoJR6p64bPxtbAV", "SEL_GRP_UPD_PADRE", "<datos><campo><valor>" + sPadre + "</valor></campo><campo><tipo>int</tipo><valor>" + id_fila_ini.ToString + "</valor></campo></datos>")

        If sCad.IndexOf("1") = 0 Then
            MessageBox.Show("Error actualización curso_id: " + id_fila_ini)
        End If

    End Sub
    Sub ActualizarRCP_GRP_TMP(ByVal id_fila_ini As Long, ByVal DescMod As String, ByVal Codigo As Long, ByVal EsPadre As String, ByVal PorSimil As Integer, ByVal NumPalTot As Integer, ByVal NumPalMaes As Integer, ByVal CodMaestro As Long)
        Dim WSc As New SAC.WS.SafefpWSClient
        Dim sCad As String
        Dim iNumPalSim As Integer = 0

        'Recuperamos los cursos en grupos de 10 elementos
        'RecuperarCursos = WSc.procesarQuery("FC", "KoJR6p64bPxtbAV", "SEL_GRP_TMP", "<datos><campo><tipo>int</tipo><valor>10</valor></campo><campo><valor>cursos</valor></campo><campo><tipo>int</tipo><valor>0</valor></campo></datos>")
        sCad = WSc.procesarQuery("FC", "KoJR6p64bPxtbAV", "SEL_GRP_TMP_UPD", "<datos><campo><valor>" + DescMod + "</valor></campo><campo><tipo>int</tipo><valor>" + Codigo.ToString() + "</valor></campo><campo><valor>" + EsPadre + "</valor></campo><campo><tipo>int</tipo><valor>" + PorSimil.ToString() + "</valor></campo><campo><tipo>int</tipo><valor>" + NumPalTot.ToString() + "</valor></campo><campo><tipo>int</tipo><valor>" + iNumPalSim.ToString() + "</valor></campo><campo><tipo>int</tipo><valor>" + NumPalMaes.ToString() + "</valor></campo><campo><tipo>int</tipo><valor>" + CodMaestro.ToString() + "</valor></campo><campo><tipo>int</tipo><valor>" + id_fila_ini.ToString + "</valor></campo></datos>")

        If sCad.IndexOf("1") = 0 Then
            MessageBox.Show("Error actualización curso_id: " + id_fila_ini)
        End If
    End Sub

    Function CorregirDenominacion(ByVal sCad As String) As String
        Dim c As Char
        Dim i As Integer

        sCad = sCad.Trim
        CorregirDenominacion = ""
        For i = 0 To sCad.Length - 1
            c = sCad.Substring(i, 1)
            If c = """" Then c = "'"
            CorregirDenominacion = CorregirDenominacion + c
        Next
    End Function

    Function ComprobarTag(ByVal sTagLeido As String, ByVal sTag As String, ByVal bCierreLeido As Boolean, ByVal bCierre As Boolean, Optional ByVal bCheck As Boolean = False) As Boolean
        If sTagLeido = sTag And bCierreLeido = bCierre Then
            ComprobarTag = True
        Else
            ComprobarTag = False
            If bCheck Then
                Throw New WSResponseException
            End If
        End If
    End Function

    Function RecuperarValores(ByVal sCad As String, ByRef val(,) As String, ByRef iFila As Integer, ByRef iCol As Integer) As Boolean
        Dim Tag As String
        Dim iCont As Integer
        Dim bCierre As Boolean
        Dim sInfo As String

        RecuperarValores = True

        Tag = ""
        LeerTag(sCad, iCont, Tag, bCierre)
        If ComprobarTag(Tag, "safefp", bCierre, False) Then
            LeerTag(sCad, iCont, Tag, bCierre)
            If ComprobarTag(Tag, "resultado", bCierre, False) Then
                LeerTag(sCad, iCont, Tag, bCierre)
                If ComprobarTag(Tag, "cabecera", bCierre, False) Then
                    'Recuperamos las columnas hasta encontrar el TAG de cierre de la cabecera
                    LeerTag(sCad, iCont, Tag, bCierre)
                    While Not ComprobarTag(Tag, "cabecera", bCierre, True)
                        If ComprobarTag(Tag, "columna", bCierre, False) Then
                            sInfo = RecuperarInformacion(sCad, iCont)
                            val(0, iCol) = sInfo
                            Inc(iCol)
                            LeerTag(sCad, iCont, Tag, bCierre)
                            ComprobarTag(Tag, "columna", bCierre, True)
                        End If
                        LeerTag(sCad, iCont, Tag, bCierre)
                    End While
                    'Recuperamos la información
                    iCol = 0
                    iFila = 0
                    LeerTag(sCad, iCont, Tag, bCierre)
                    ComprobarTag(Tag, "lista", bCierre, False)
                    LeerTag(sCad, iCont, Tag, bCierre)
                    While Not ComprobarTag(Tag, "lista", bCierre, True)
                        ComprobarTag(Tag, "fila", bCierre, False)
                        Inc(iFila)
                        iCol = 0
                        LeerTag(sCad, iCont, Tag, bCierre)
                        While Not ComprobarTag(Tag, "fila", bCierre, True)
                            If ComprobarTag(Tag, "columna", bCierre, False) Then
                                sInfo = RecuperarInformacion(sCad, iCont)
                                val(iFila, iCol) = sInfo
                                Inc(iCol)
                                LeerTag(sCad, iCont, Tag, bCierre)
                                ComprobarTag(Tag, "columna", bCierre, True)
                            End If
                            LeerTag(sCad, iCont, Tag, bCierre)
                        End While
                        LeerTag(sCad, iCont, Tag, bCierre)
                    End While
                End If
                LeerTag(sCad, iCont, Tag, bCierre)
                ComprobarTag(Tag, "resultado", bCierre, True)
            End If
            LeerTag(sCad, iCont, Tag, bCierre)
            ComprobarTag(Tag, "safefp", bCierre, True)
        End If

    End Function

    Private Sub PropagarGruposWS()
        Dim sSQL, sSQL2 As String
        Dim lGrupo As Integer
        Dim dbRCP As DAODatabaseHelper

        tbProceso.Text = "Propagando xeración de grupos"
        tbProceso.Refresh()

        'UPGRADE_WARNING: (2065) DAO.Database method DBEngine.OpenDatabase has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2065.aspx
        db = DBEngineHelper.Instance(Artinsoft.VB6.DB.AdoFactoryManager.GetFactory()).OpenDatabase(RUTA_BD)

        'Eliminamos todos los registros con grupo de rcp_grp_tmp
        If chkElimCursos.CheckState = CheckState.Checked Then
            Dim TempCommand As DbCommand = db.Connection.CreateCommand()
            TempCommand.CommandText = "DELETE FROM rcp_grp_tmp WHERE codigo > 0"
            TempCommand.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
            TempCommand.ExecuteNonQuery()
        End If

        Dim rs As DAORecordSetHelper = db.OpenRecordset("SELECT c.cod_id_grupo, c.cod_id_curso, c.denom_curso_corregida FROM cursos c WHERE contenido = 'NUEVO' ORDER BY c.cod_id_curso")

        If Not rs.EOF Then
            rs.MoveLast()
            pbCursos.Maximum = rs.RecordCount
            rs.MoveFirst()
            pbCursos.Value = 0
        End If

        While Not rs.EOF

            Dim TempCommand_2 As DbCommand = db.Connection.CreateCommand()
            TempCommand_2.CommandText = "UPDATE rcp_grp_tmp SET codigo = " & _
                                        rs("cod_id_grupo") & ", descripcion_mod = '" & _
                                        rs("denom_curso_corregida") & "' WHERE id_fila = " & _
                                        rs("cod_id_curso")
            TempCommand_2.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
            TempCommand_2.ExecuteNonQuery()

            tbNumGrupos.Text = rs("cod_id_curso")
            Application.DoEvents()
            rs.MoveNext()
            pbCursos.Value += 1
        End While
        rs.Close()

        'Si algún curso queda sin grupo, su denominación no es válida y hay que dejarlo en un grupo el solo
        rs = db.OpenRecordset("SELECT MAX(codigo) AS max_codigo FROM rcp_grp_tmp")
        'UPGRADE_WARNING: (1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
        If Not Convert.IsDBNull(rs("max_codigo")) Then
            lGrupo = CInt(rs(0) + 1)
            rs.Close()
            If lGrupo > 900000 Then
                Interaction.MsgBox("Propagación de grupos no realizada", "ERROR")
                Exit Sub
            End If
            rs = db.OpenRecordset("SELECT id_fila FROM rcp_grp_tmp WHERE codigo IS NULL OR codigo = 0")
            While Not rs.EOF
                Dim TempCommand_3 As DbCommand = db.Connection.CreateCommand()
                TempCommand_3.CommandText = "UPDATE rcp_grp_tmp SET codigo = " & lGrupo & _
                                            " WHERE id_fila = " & rs("id_fila")
                TempCommand_3.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
                TempCommand_3.ExecuteNonQuery()
                Inc(lGrupo)
                rs.MoveNext()
            End While
        End If
        rs.Close()

        'Propagamos las características
        rs = db.OpenRecordset("SELECT c.cod_id_grupo, c.cod_id_curso, gc.equiv, gc.num_palabras, gc.num_palabras_maestro, " & _
             "gc.porcentaje, gc.curso_maestro1, gc.cod_id_maestro, c.denom_curso_corregida FROM cursos c, grupos_cursos gc " & _
             "WHERE gc.cod_id_curso = c.cod_id_curso AND control = 1 AND contenido = 'NUEVO' ORDER BY c.cod_id_curso")
        If Not rs.EOF Then
            rs.MoveLast()
            pbCursos.Maximum = rs.RecordCount
            rs.MoveFirst()
            pbCursos.Value = 0
        End If

        While Not rs.EOF
            Dim TempCommand_4 As DbCommand = db.Connection.CreateCommand()
            TempCommand_4.CommandText = "UPDATE rcp_grp_tmp SET subtipo = '-', es_padre = " & _
                                        (IIf(rs("curso_maestro1") = 1, "'S'", "'N'")) & ",porcent_simil = '" & rs("porcentaje") & "', num_palab_tot = " & _
                                        rs("num_palabras") & ",num_palab_sim =" & rs("equiv") & _
                                        ",num_palab_maes=" & rs("num_palabras_maestro") & _
                                        ",codigo_maestro = " & rs("cod_id_maestro") & _
                                        ",descripcion_mod = '" & rs("denom_curso_corregida") & "'" & _
                                        " WHERE descripcion_mod = '" & rs("denom_curso_corregida") & "'"
            TempCommand_4.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
            TempCommand_4.ExecuteNonQuery()

            tbNumGrupos.Text = rs("cod_id_curso")
            pbCursos.Value += 1
            tbNumGrupos.Refresh()
            rs.MoveNext()
        End While
        rs.Close()

        tbProceso.Text = "Estableciendo maestros en todos los grupos"
        Application.DoEvents()
        'Antes de propagar nos aseguramos que todos los grupos tienen un padre
        rs = db.OpenRecordset("SELECT MAX(id_fila) AS cod_fila, codigo FROM rcp_grp_tmp g1 WHERE " & _
             " (SELECT count(*) FROM rcp_grp_tmp g2 WHERE g2.codigo = g1.codigo AND es_padre = 'S') = 0 GROUP BY codigo")
        While Not rs.EOF
            Dim TempCommand_5 As DbCommand = db.Connection.CreateCommand()
            TempCommand_5.CommandText = "UPDATE rcp_grp_tmp SET es_padre = 'S' WHERE codigo = " & _
                                        rs("codigo") & " AND id_fila = " & rs("cod_fila")
            TempCommand_5.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
            TempCommand_5.ExecuteNonQuery()
            rs.MoveNext()
        End While
        rs.Close()

        'propagamos los cambios a la copia principal
        Dim iErrores As Integer
        If chkCargaServ.CheckState = CheckState.Checked Then
            'UPGRADE_WARNING: (2065) DAO.Database method DBEngine.OpenDatabase has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2065.aspx

            tbProceso.Text = "Propagando rexistros pais"
            Application.DoEvents()
            'Primero propagamos los padres de cada grupo, si coinciden en registros ya presentes en RCP
            rs = db.OpenRecordset("SELECT * FROM rcp_grp_tmp WHERE contenido <> 'NUEVO' AND es_padre = 'S'")

            If Not rs.EOF Then
                rs.MoveLast()
                pbCursos.Maximum = rs.RecordCount
                rs.MoveFirst()
                pbCursos.Value = 0
            End If

            While Not rs.EOF
                sSQL = "UPDATE rcp_grp_tmp SET es_padre = '" & rs("es_padre") & "' WHERE id_fila = " & rs("id_fila")
                Debug.WriteLine(sSQL)
                iErrores = 0
                Do While iErrores <= LIMITE_REINTENTOS
                    If iErrores = LIMITE_REINTENTOS Then
                        If Interaction.MsgBox("Se han producido 5 errores accediendo a la base de datos, ¿Reintentar?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "AVISO") = System.Windows.Forms.DialogResult.Yes Then
                            iErrores = 0
                        Else
                            Exit Sub
                        End If
                    End If

                    ActualizarRCPPadre(rs("id_fila"), rs("es_padre"))
                    tbActual.Text = "Procesando " & rs("descripcion")
                    Application.DoEvents()

                    If Information.Err().Number = 0 Then
                        iErrores = 0
                        Exit Do
                    Else
                        iErrores += 1
                    End If
                Loop
                rs.MoveNext()
                pbCursos.Value += 1
                Application.DoEvents()
            End While
            rs.Close()

            tbProceso.Text = "Propagando características"
            Application.DoEvents()
            'Ahora propagamos las características a los resgistros nuevos
            rs = db.OpenRecordset("SELECT * FROM rcp_grp_tmp WHERE contenido = 'NUEVO'")

            If Not rs.EOF Then
                rs.MoveLast()
                pbCursos.Maximum = rs.RecordCount
                rs.MoveFirst()
                pbCursos.Value = 0
            End If

            While Not rs.EOF
                sSQL = "UPDATE rcp_grp_tmp SET descripcion_mod = '" & _
                       rs("descripcion_mod") & "',codigo = " & rs("codigo") & ",es_padre = '" & rs("es_padre") & _
                       "',porcent_simil = '" & rs("porcent_simil") & _
                       "',num_palab_tot = " & rs("num_palab_tot") & _
                       ",num_palab_sim= " & rs("num_palab_sim") & _
                       ",num_palab_maes = " & rs("num_palab_maes") & _
                       ",codigo_maestro = " & rs("codigo_maestro") & _
                       " WHERE id_fila = " & rs("id_fila")
                Debug.WriteLine(sSQL)

                iErrores = 0
                Do While iErrores <= LIMITE_REINTENTOS
                    If iErrores = LIMITE_REINTENTOS Then
                        If Interaction.MsgBox("Se han producido 5 errores accediendo a la base de datos, ¿Reintentar?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "AVISO") = System.Windows.Forms.DialogResult.Yes Then
                            iErrores = 0
                        Else
                            Exit Sub
                        End If
                    End If

                    ActualizarRCP_GRP_TMP(rs("id_fila"), rs("descripcion_mod"), rs("codigo"), rs("es_padre"), rs("porcent_simil"), rs("num_palab_tot"), rs("num_palab_maes"), rs("codigo_maestro"))
                    tbActual.Text = "Procesando " & rs("descripcion")
                    Application.DoEvents()

                    If Information.Err().Number = 0 Then
                        iErrores = 0
                        Exit Do
                    Else
                        iErrores += 1
                    End If
                Loop

                rs.MoveNext()
                pbCursos.Value += 1
                Application.DoEvents()
            End While
            rs.Close()
        End If

        tbProceso.Text = "Operación Finalizada." + vbCrLf + m_sLog
    End Sub

    Private Sub PropagarGruposWS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropGruposWS.Click
        PropagarGruposWS()
    End Sub

    Private Sub EjecutarQuery(ByVal sQuery As String, ByRef db As DAODatabaseHelper)
        Dim TempCommand As DbCommand = db.Connection.CreateCommand()
        TempCommand.CommandText = sQuery
        TempCommand.Transaction = Artinsoft.VB6.DB.TransactionManager.GetTransaction(db.Connection)
        TempCommand.ExecuteNonQuery()
    End Sub

    Sub ExtraerCaracteristicas(ByVal sPalabra As String, ByRef iG(,) As Single, ByRef iGd(,) As Single)
        Dim aPal() As String = {"ADLGYF", "EICBVZK", "ONTPQJW", "SRUMHÑX"}
        Dim iLon As Integer
        Dim i As Integer, j As Integer
        Dim c As Char
        Dim iMitad As Integer
        Dim iMitadValor As Integer
        'Comprobamos el número de letras de cada grupo y la suma de su órden
        'Tiene más valor cuanta menor probabilidad de aparición en el idioma y cuanto más cerca está su posición del inicio de la palabra
        iLon = sPalabra.Length()
        iMitadValor = iLon / 2
        For j = 0 To 3
            iMitad = 0
            iG(j, 0) = 0
            iG(j, 1) = 0
            iGd(j, 0) = 0
            iGd(j, 1) = 0
        Next
        For i = 0 To iLon - 1
            If i = iMitadValor Then iMitad = 1
            c = sPalabra.Substring(i, 1)
            For j = 0 To 3
                If aPal(j).IndexOf(c) > -1 Then
                    iG(j, iMitad) = iG(j, iMitad) + 1
                    iGd(j, iMitad) = iGd(j, iMitad) + iLon - i
                End If
            Next
        Next
    End Sub

    Private Sub chkElimCursos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkElimCursos.CheckedChanged

    End Sub

    Private Sub PruebaVelocidad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Command1.Click
        Dim sCad As String
        Dim aCad(30) As String
        Dim i As Integer, iCont As Integer

        Command1.Enabled = False
        tbTime.Text = DateTimeHelper.Time.ToString("HH:mm:ss")

        For i = 0 To 10000
            sCad = CorregirCadena("CURSO DE PREVENCIÓN DE RISCOS LABORAIS NA MODALIDADE DE TELEFORMACIÓN CON ESPECIALIDADE EN HIXIENE INDUSTRIAL")
            iCont = DividirCampo(sCad, aCad, " ")
            tbTimeFin.Text = i
            tbTimeFin.Refresh()
        Next
        Command1.Enabled = True

    End Sub

    Private Sub LocalizarGrupoErroneos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LocalizarGrupoErroneos.Click
        '-. Recuperamos todos los códigos de grupo de la base de datos
        '-. Entramos en un bucle en la que recuperamos todos los elementos del grupo uno por uno
        '-. Reducimos cada elementos hasta optenes la descripción modificada mínima normalizada
        '-. Recuperamos todas las palabras distintas del grupo
        '-. Comprobamos la frecuencia de aparición de cada palabra en el grupo y el número de elementos que la contienen
        '-. Pasamos las palabras por la red autoasociativa
        '-. En las palabras que aparezcan muy pocas vez o solo una vez, buscamos la distancia con el resto y en caso de que sea menor que la configurada, las equiparamos
        '-. Realizamos un tabla para cada elemento con cada palabra y el número de cursos que la contienen. Se da una puntuación a cada elementos que será mayor cuantos más cursos
        '   contengan sus mismas palabras
        '-. Los elementos con menos de la puntuación indicada se marcarán como no pertenecientes al grupo

    End Sub

    Private Sub IdentificarTipoGrupo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IdentificarTipoGrupo.Click
        '-. Recuperamos todas las denominaciones distintas de la base de datos
        '-. Recuperamos todos los grupos de la base de datos
        '-. Recuperamos sus tipos
        '-. Recuperamos los coñecementos asociados
        '-. Simplificamos los cursos y descomponemos los cursos en palabras
        '-. Agrupamos las palabras con una red neuronal autoasociativa
        '-. Identificamos cada tipo
        '-. Localizamos los cursos que tienen asociado este tipo
        '-. Localizamos todas sus palabras
        '-. Por cada palabra contabilizamos el número de veces que aparece
        '-. Construimos una función de pertenencia a un conjunto difuso basándose en una puntuación por aparición de palabras más representativas
        '-. Por cada uno de los cursos a clasificar automáticamente
        '-.     Los descomponemos en palabras
        '-.     Aplicamos la función de pertenencia a los conjuntos difusos de tipos
        '-.     Finalmente, si el grado de pertenencia a alguno de los conjuntos supera el umbral, asignamos el tipo especificado
        '-. Para cada uno de los coñecementos realizamos la misma operación



    End Sub
End Class

Public Class WSResponseException
    Inherits Exception

    Public Sub New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal inner As Exception)
        MyBase.New(message, inner)
    End Sub
End Class
