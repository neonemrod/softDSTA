'28-04-2020 INICIO PARA CONFIGURACIÓN DE MEDICIÓN DESDE ARCHIVO
'Carlos Sátiva #280420
'Versión 2.0 correcciones de orden de código y estructura de medición
'Esta versión solo funciona con los cilindros 5 y 6 conectados - no sirve en general debe modificarse - #050520DA #CS 
'Versión 2.1 - para entrega de equipo (Fecha de entrega 160620) #130620DA #WCS

Imports System 'carlos: librería logfile
Imports System.IO
Imports System.IO.Ports
Imports System.Threading
Imports System.Windows.Forms.DataVisualization.Charting
Imports Microsoft.VisualBasic.FileIO

Public Class Form_Principal
    Public puerto As String
    Public com As SerialPort
    Public EstadoConnectPuerto As Boolean
    Dim inData As String = ""
    Dim inDataTemp As String = ""
    Dim ThreadingLectura As New Thread(AddressOf hiloLectura)
    Dim ThreadingModoArchivo As New Thread(AddressOf hiloLecturaArchivo)
    Dim ArrayIn() As String
    Dim tempString As String
    Dim SleepLectura As Integer = 2000
    Dim SleppLectura2 As Integer = 1728000
    Dim ArrayConfig As String = String.Empty
    Dim ArrayConfigCompara As String = String.Empty
    Dim VariableFuncionamiento As Boolean = False
    Dim VariableFuncionamientoModoFile As Boolean = False
    Dim ArrayControlTableGraf(24) As Integer
    Dim ArrayNames() As String = {"T1-A1", "T2-A1", "T3-A1", "T4-A1", "T1-A2", "T2-A2", "T3-A2", "T4-A2", "T1-A3", "T2-A3", "T3-A3", "T4-A3", "C - 1", "C - 2", "C - 3", "C - 4", "C - 5", "C - 6", "C - 7", "C - 8", "C - 9", "C - 10", "C - 11", "C - 12"}
    'Dim ArrayNames() As String = {"T1 - A1", "T2 - A1", "T3 - A1", "T4 - A1", "T1 - A2", "T2 - A2", "T3 - A2", "T4 - A2", "T1 - A3", "T2 - A3", "T3 - A3", "T4 - A3",
    '    "C - 1", "C - 2", "C - 3", "C - 4", "C - 5", "C - 6", "C - 7", "C - 8", "C - 9", "C - 10", "C - 11", "C - 12"}
    Dim ArrayListCsv As New ArrayList

    Dim control = New Boolean() {False, False, False, False, False, False, False, False, False, False, False, False}
    Dim control2 = New Boolean() {False, False, False, False, False, False, False, False, False, False, False, False}

    Public ArchivoProyecto As String = ""
    Dim ruta As String = ""
    Dim ruta2 As String = ""
    Dim escritor As StreamWriter
    Dim escritor2 As StreamWriter
    Dim Counter As Double = 1

    Dim Flag As Boolean = True
    Dim multiploMinuto As Integer = 1 'Dejar valor Paramétrico
    Dim grafModuloMin = 30 'Dejar valor Paramétrico
    Dim minutoTemp As Integer = 0
    Dim moduloMinuto As Integer = 0
    Dim minuto As Integer = 1
    Dim flagRead As Boolean = True
    Dim flagRecepcion As Boolean = True
    Dim limiteContadorLora As Integer = 25 'Dejar valor Paramétrico
    Dim recepcionContador As Integer = 0
    Dim limiteTimeout As Integer = 3
    Dim CadenaTabla = New ArrayList
    Dim arregloMedidas As New ArrayList
    Dim rango_minimo As Integer = 5 '#130720DA #CS
    Dim rango_maximo As Integer = 110
    'MODO ARCHIVO
    Dim lineaArchivo As Integer = 0
    Dim flagMedidaInvalida As Boolean = True
    Dim flagMedidaAnterior As Boolean = False
    Dim cil5ant As Double = 0.0
    Dim cil6ant As Double = 0.0


    Dim contadorLoRa As Integer = 0
    Dim rstCounter As Integer = 0
    Dim CounterTimer As Integer = 0
    Dim contadorTimeOut As Integer = 0 '#030520DA #WCS
    Dim contadorMedidaInvalida As Integer = 0 '#030520DA #WCS
    Dim medidasComparacionAnterior(24) As Double
    Dim contadorComparacion() As Integer = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    Dim limContadorComparacion As Integer = 8 '#110620DA #WCS "T - 1"'#130620DA #WCS se aumenta de 4 a 12 por problemas con cilindros ‘#190720DA #CS se sube de 60 a 200 para prueba de estabiilidad #CS271020
    Dim flagArchivo As Boolean = False '#110620DA #WCS
    'ARREGLOS DE CALIBRACIÓN
    Dim mArray() As Double = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, Convert.ToDouble(My.Settings.m_1), Convert.ToDouble(My.Settings.m_2), Convert.ToDouble(My.Settings.m_3), Convert.ToDouble(My.Settings.m_4), Convert.ToDouble(My.Settings.m_5), Convert.ToDouble(My.Settings.m_6), Convert.ToDouble(My.Settings.m_7), Convert.ToDouble(My.Settings.m_8), Convert.ToDouble(My.Settings.m_9), Convert.ToDouble(My.Settings.m_10), Convert.ToDouble(My.Settings.m_11), Convert.ToDouble(My.Settings.m_12)}
    Dim bArray() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToDouble(My.Settings.b_1), Convert.ToDouble(My.Settings.b_2), Convert.ToDouble(My.Settings.b_3), Convert.ToDouble(My.Settings.b_4), Convert.ToDouble(My.Settings.b_5), Convert.ToDouble(My.Settings.b_6), Convert.ToDouble(My.Settings.b_7), Convert.ToDouble(My.Settings.b_8), Convert.ToDouble(My.Settings.b_9), Convert.ToDouble(My.Settings.b_10), Convert.ToDouble(My.Settings.b_11), Convert.ToDouble(My.Settings.b_12)}
    'Dim arregloMedidasAnterior As New ArrayList

    Dim Arraypromedio As New ArrayList

    'Private dtSource As DataTable
    'Dim dtClient As New DataTable("TestTable")

    Public LogFile As String = "\" + "LogFile" + DateTime.Now.ToString("dd-MM-yyyy") + "-" + DateTime.Now.ToString("hhmmss") + ".log"
    Private Function SysLog(TextLogP As String)
        Using outputFile As New StreamWriter(Application.StartupPath & Convert.ToString(LogFile), True)
            outputFile.WriteLine(DateTime.Now.ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("hh:mm:ss") + " - " + TextLogP)
        End Using
        Return False
    End Function



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Timer1.Interval = 10000
        'permite la llamade de subprocesos
        SysLog("I: Inicio de programa")
        CheckForIllegalCrossThreadCalls = False

        CheckBoxModo.Checked = False

        'consulta puertos com activos
        Try
            GetSerialPortNames()
        Catch ex As Exception
            MsgBox("Error, no hay puertos activos: " & ex.Message, MsgBoxStyle.Critical, "DSTAi")
            EstadoConnectPuerto = False
        End Try


        Chart1.ChartAreas("ChartArea1").AxisX.Title = "Time"
        Chart1.ChartAreas("ChartArea1").AxisY.Title = "Temperature ºC"

        DataGridView1.Columns.Add("Time", "Time")

        For i = 0 To 24
            Arraypromedio.Add(New ArrayList)
            arregloMedidas.Add(New ArrayList)
        Next


        Timer1.Interval = 10000

        If My.Settings.T_1 <> "None" Then
            label_T1.Text = label_T1.Text + " (" + My.Settings.T_1(0) + My.Settings.T_1(1) + My.Settings.T_1(2) + ")"
            DataGridView1.Columns.Add("T1 - A1 (" + My.Settings.T_1.ToString + ")", "T1 - A1 (" + My.Settings.T_1.ToString + ")")
            'dtClient.Columns.Add("T - 1 (" + My.Settings.T_1.ToString + ")")
            ArrayControlTableGraf(0) = 1
        Else
            ArrayControlTableGraf(0) = 0
            checkT1.Image = My.Resources.ina
            checkT1.Enabled = False
        End If

        If My.Settings.T_2 <> "None" Then
            label_T2.Text = label_T2.Text + " (" + My.Settings.T_2(0) + My.Settings.T_2(1) + My.Settings.T_2(2) + ")"
            DataGridView1.Columns.Add("T2 - A1 (" + My.Settings.T_2.ToString + ")", "T2 - A1 (" + My.Settings.T_2.ToString + ")")
            'dtClient.Columns.Add("T - 2 (" + My.Settings.T_2.ToString + ")")
            ArrayControlTableGraf(1) = 1
        Else
            ArrayControlTableGraf(1) = 0
            checkT2.Image = My.Resources.ina
            checkT2.Enabled = False
        End If

        If My.Settings.T_3 <> "None" Then
            label_T3.Text = label_T3.Text + " (" + My.Settings.T_3(0) + My.Settings.T_3(1) + My.Settings.T_3(2) + ")"
            DataGridView1.Columns.Add("T3 - A1 (" + My.Settings.T_3.ToString + ")", "T3 - A1 (" + My.Settings.T_3.ToString + ")")
            'dtClient.Columns.Add("T - 3 (" + My.Settings.T_3.ToString + ")")
            ArrayControlTableGraf(2) = 1
        Else
            ArrayControlTableGraf(2) = 0
            checkT3.Image = My.Resources.ina
            checkT3.Enabled = False
        End If

        If My.Settings.T_4 <> "None" Then
            label_T4.Text = label_T4.Text + " (" + My.Settings.T_4(0) + My.Settings.T_4(1) + My.Settings.T_4(2) + ")"
            DataGridView1.Columns.Add("T4 - A1 (" + My.Settings.T_4.ToString + ")", "T4 - A1 (" + My.Settings.T_4.ToString + ")")
            'dtClient.Columns.Add("T - 4 (" + My.Settings.T_4.ToString + ")")
            ArrayControlTableGraf(3) = 1
        Else
            ArrayControlTableGraf(3) = 0
            checkT4.Image = My.Resources.ina
            checkT4.Enabled = False
        End If

        If My.Settings.T_5 <> "None" Then
            label_T5.Text = label_T5.Text + " (" + My.Settings.T_5(0) + My.Settings.T_5(1) + My.Settings.T_5(2) + ")"
            DataGridView1.Columns.Add("T1 - A2 (" + My.Settings.T_5.ToString + ")", "T1 - A2 (" + My.Settings.T_5.ToString + ")")
            'dtClient.Columns.Add("T - 5 (" + My.Settings.T_5.ToString + ")")
            ArrayControlTableGraf(4) = 1
        Else
            ArrayControlTableGraf(4) = 0
            checkT5.Image = My.Resources.ina
            checkT5.Enabled = False
        End If

        If My.Settings.T_6 <> "None" Then
            label_T6.Text = label_T6.Text + " (" + My.Settings.T_6(0) + My.Settings.T_6(1) + My.Settings.T_6(2) + ")"
            DataGridView1.Columns.Add("T2 - A2 (" + My.Settings.T_6.ToString + ")", "T2 - A2 (" + My.Settings.T_6.ToString + ")")
            'dtClient.Columns.Add("T - 6 (" + My.Settings.T_6.ToString + ")")
            ArrayControlTableGraf(5) = 1
        Else
            ArrayControlTableGraf(5) = 0
            checkT6.Image = My.Resources.ina
            checkT6.Enabled = False
        End If

        If My.Settings.T_7 <> "None" Then
            label_T7.Text = label_T7.Text + " (" + My.Settings.T_7(0) + My.Settings.T_7(1) + My.Settings.T_7(2) + ")"
            DataGridView1.Columns.Add("T3 - A2 (" + My.Settings.T_7.ToString + ")", "T3 - A2 (" + My.Settings.T_7.ToString + ")")
            'dtClient.Columns.Add("T - 7 (" + My.Settings.T_7.ToString + ")")
            ArrayControlTableGraf(6) = 1
        Else
            ArrayControlTableGraf(6) = 0
            checkT7.Image = My.Resources.ina
            checkT7.Enabled = False
        End If

        If My.Settings.T_8 <> "None" Then
            label_T8.Text = label_T8.Text + " (" + My.Settings.T_8(0) + My.Settings.T_8(1) + My.Settings.T_8(2) + ")"
            DataGridView1.Columns.Add("T4 - A2 (" + My.Settings.T_8.ToString + ")", "T4 - A2 (" + My.Settings.T_8.ToString + ")")
            'dtClient.Columns.Add("T - 8 (" + My.Settings.T_8.ToString + ")")
            ArrayControlTableGraf(7) = 1
        Else
            ArrayControlTableGraf(7) = 0
            checkT8.Image = My.Resources.ina
            checkT8.Enabled = False
        End If

        If My.Settings.T_9 <> "None" Then
            label_T9.Text = label_T9.Text + " (" + My.Settings.T_9(0) + My.Settings.T_9(1) + My.Settings.T_9(2) + ")"
            DataGridView1.Columns.Add("T1 - A3 (" + My.Settings.T_9.ToString + ")", "T1 - A3 (" + My.Settings.T_9.ToString + ")")
            'dtClient.Columns.Add("T - 9 (" + My.Settings.T_9.ToString + ")")
            ArrayControlTableGraf(8) = 1
        Else
            ArrayControlTableGraf(8) = 0
            checkT9.Image = My.Resources.ina
            checkT9.Enabled = False
        End If

        If My.Settings.T_10 <> "None" Then
            label_T10.Text = label_T10.Text + " (" + My.Settings.T_10(0) + My.Settings.T_10(1) + My.Settings.T_10(2) + ")"
            DataGridView1.Columns.Add("T2 - A3 (" + My.Settings.T_10.ToString + ")", "T2 - A3 (" + My.Settings.T_10.ToString + ")")
            'dtClient.Columns.Add("T - 10 (" + My.Settings.T_10.ToString + ")")
            ArrayControlTableGraf(9) = 1
        Else
            ArrayControlTableGraf(9) = 0
            checkT10.Image = My.Resources.ina
            checkT10.Enabled = False
        End If

        If My.Settings.T_11 <> "None" Then
            label_T11.Text = label_T11.Text + " (" + My.Settings.T_11(0) + My.Settings.T_11(1) + My.Settings.T_11(2) + ")"
            DataGridView1.Columns.Add("T3 - A3 (" + My.Settings.T_11.ToString + ")", "T3 - A3 (" + My.Settings.T_11.ToString + ")")
            'dtClient.Columns.Add("T - 11 (" + My.Settings.T_11.ToString + ")")
            ArrayControlTableGraf(10) = 1
        Else
            ArrayControlTableGraf(10) = 0
            checkT11.Image = My.Resources.ina
            checkT11.Enabled = False
        End If

        If My.Settings.T_12 <> "None" Then
            label_T12.Text = label_T12.Text + " (" + My.Settings.T_12(0) + My.Settings.T_12(1) + My.Settings.T_12(2) + ")"
            DataGridView1.Columns.Add("T4 - A3 (" + My.Settings.T_12.ToString + ")", "T4 - A3 (" + My.Settings.T_12.ToString + ")")
            'dtClient.Columns.Add("T - 12 (" + My.Settings.T_12.ToString + ")")
            ArrayControlTableGraf(11) = 1
        Else
            ArrayControlTableGraf(11) = 0
            checkT12.Image = My.Resources.ina
            checkT12.Enabled = False
        End If


        If My.Settings.C_1 <> "None" Then
            DataGridView1.Columns.Add("C - 1" + My.Settings.C_1.ToString, "C - 1 (" + My.Settings.C_1.ToString + ")")
            'dtClient.Columns.Add("C - 1" + My.Settings.C_1.ToString)
            ArrayControlTableGraf(12) = 1
        Else
            ArrayControlTableGraf(12) = 0
            checkC1.Image = My.Resources.ina
            checkC1.Enabled = False
        End If

        If My.Settings.C_2 <> "None" Then
            DataGridView1.Columns.Add("C - 2" + My.Settings.C_1.ToString, "C - 2 (" + My.Settings.C_1.ToString + ")")
            'dtClient.Columns.Add("C - 2" + My.Settings.C_1.ToString)
            ArrayControlTableGraf(13) = 1
        Else
            ArrayControlTableGraf(13) = 0
            checkC2.Image = My.Resources.ina
            checkC2.Enabled = False
        End If

        If My.Settings.C_3 <> "None" Then
            DataGridView1.Columns.Add("C - 3" + My.Settings.C_1.ToString, "C - 3 (" + My.Settings.C_1.ToString + ")")
            'dtClient.Columns.Add("C - 3" + My.Settings.C_1.ToString)
            ArrayControlTableGraf(14) = 1
        Else
            ArrayControlTableGraf(14) = 0
            checkC3.Image = My.Resources.ina
            checkC3.Enabled = False
        End If

        If My.Settings.C_4 <> "None" Then
            DataGridView1.Columns.Add("C - 4" + My.Settings.C_1.ToString, "C - 4 (" + My.Settings.C_1.ToString + ")")
            'dtClient.Columns.Add("C - 4" + My.Settings.C_1.ToString)
            ArrayControlTableGraf(15) = 1
        Else
            ArrayControlTableGraf(15) = 0
            checkC4.Image = My.Resources.ina
            checkC4.Enabled = False
        End If

        If My.Settings.C_5 <> "None" Then
            DataGridView1.Columns.Add("C - 5" + My.Settings.C_1.ToString, "C - 5 (" + My.Settings.C_1.ToString + ")")
            'dtClient.Columns.Add("C - 5" + My.Settings.C_1.ToString)
            ArrayControlTableGraf(16) = 1
        Else
            ArrayControlTableGraf(16) = 0
            checkC5.Image = My.Resources.ina
            checkC5.Enabled = False
        End If

        If My.Settings.C_6 <> "None" Then
            DataGridView1.Columns.Add("C - 6" + My.Settings.C_1.ToString, "C - 6 (" + My.Settings.C_1.ToString + ")")
            'dtClient.Columns.Add("C - 6" + My.Settings.C_1.ToString)
            ArrayControlTableGraf(17) = 1
        Else
            ArrayControlTableGraf(17) = 0
            checkC6.Image = My.Resources.ina
            checkC6.Enabled = False
        End If

        If My.Settings.C_7 <> "None" Then
            DataGridView1.Columns.Add("C - 7" + My.Settings.C_1.ToString, "C - 7 (" + My.Settings.C_1.ToString + ")")
            'dtClient.Columns.Add("C - 7" + My.Settings.C_1.ToString)
            ArrayControlTableGraf(18) = 1
        Else
            ArrayControlTableGraf(18) = 0
            checkC7.Image = My.Resources.ina
            checkC7.Enabled = False
        End If

        If My.Settings.C_8 <> "None" Then
            DataGridView1.Columns.Add("C - 8" + My.Settings.C_1.ToString, "C - 8 (" + My.Settings.C_1.ToString + ")")
            'dtClient.Columns.Add("C - 8" + My.Settings.C_1.ToString)
            ArrayControlTableGraf(19) = 1
        Else
            ArrayControlTableGraf(19) = 0
            checkC8.Image = My.Resources.ina
            checkC8.Enabled = False
        End If

        If My.Settings.C_9 <> "None" Then
            DataGridView1.Columns.Add("C - 9" + My.Settings.C_1.ToString, "C - 9 (" + My.Settings.C_1.ToString + ")")
            'dtClient.Columns.Add("C - 9" + My.Settings.C_1.ToString)
            ArrayControlTableGraf(20) = 1
        Else
            ArrayControlTableGraf(20) = 0
            checkC9.Image = My.Resources.ina
            checkC9.Enabled = False
        End If

        If My.Settings.C_10 <> "None" Then
            DataGridView1.Columns.Add("C - 10" + My.Settings.C_1.ToString, "C - 10 (" + My.Settings.C_1.ToString + ")")
            'dtClient.Columns.Add("C - 10" + My.Settings.C_1.ToString)
            ArrayControlTableGraf(21) = 1
        Else
            ArrayControlTableGraf(21) = 0
            checkC10.Image = My.Resources.ina
            checkC10.Enabled = False
        End If

        If My.Settings.C_11 <> "None" Then
            DataGridView1.Columns.Add("C - 11" + My.Settings.C_1.ToString, "C - 11 (" + My.Settings.C_1.ToString + ")")
            'dtClient.Columns.Add("C - 11" + My.Settings.C_1.ToString)
            ArrayControlTableGraf(22) = 1
        Else
            ArrayControlTableGraf(22) = 0
            checkC11.Image = My.Resources.ina
            checkC11.Enabled = False
        End If

        If My.Settings.C_12 <> "None" Then
            DataGridView1.Columns.Add("C - 12" + My.Settings.C_1.ToString, "C - 12 (" + My.Settings.C_1.ToString + ")")
            'dtClient.Columns.Add("C - 12" + My.Settings.C_1.ToString)
            ArrayControlTableGraf(23) = 1
        Else
            ArrayControlTableGraf(23) = 0
            checkC12.Image = My.Resources.ina
            checkC12.Enabled = False
        End If

        For i = 0 To (ArrayNames.Count - 1)
            Chart1.Series(ArrayNames(i)).Enabled = False
        Next

        Chart1.ChartAreas(0).AxisX.Interval = 10
        Chart1.ChartAreas(0).AxisY.Interval = 5
        Chart1.ChartAreas(0).AxisX.Minimum = 0
        Chart1.ChartAreas(0).AxisX.LabelStyle.Angle = 90
        'Chart1.ChartAreas(0).AxisY.Interval = 1
        Chart1.ChartAreas(0).AxisY.Minimum = 0
        'Chart1.ChartAreas(0).AxisX.ScaleView.Size = 10
        'Chart1.ChartAreas(0).AxisX.ScrollBar.Size = 20
        'Chart1.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
        Chart1.Series(0).IsVisibleInLegend = False
        'Chart1.ChartAreas(0).AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
        'Chart1.ChartAreas(0).AxisX.ScrollBar.IsPositionedInside = True
        'Chart1.ChartAreas(0).AxisX.ScrollBar.BackColor = Color.Azure
        'Chart1.ChartAreas(0).AxisX.ScrollBar.ButtonColor = Color.FromArgb(2, 40, 105)

        Try '#150620DA - Carga de calibración para las termocuplas
            Using MyReader As New Microsoft.VisualBasic.
                            FileIO.TextFieldParser(
                              "C:\Users\Public\calibracionTermos.csv")
                MyReader.TextFieldType = FileIO.FieldType.Delimited
                MyReader.SetDelimiters(";")
                Dim currentRow As String()
                Dim icsv As Integer = 0
                Dim posi As Integer = 0
                While Not MyReader.EndOfData
                    Try
                        currentRow = MyReader.ReadFields()
                        Dim currentField As String

                        For Each currentField In currentRow
                            'SysLog("I: cadena leida : " + currentField)
                            If icsv Mod 2 = 0 Then
                                mArray(posi) = Convert.ToDouble(currentField)
                            Else
                                bArray(posi) = Convert.ToDouble(currentField)
                                posi += 1
                            End If
                            icsv += 1
                        Next
                    Catch ex As Microsoft.VisualBasic.
                                    FileIO.MalformedLineException
                        SysLog("Line " & ex.Message &
                            "Valor de calibración inválido")
                        MsgBox("Line " & ex.Message &
                            "Valor de calibración inválido")
                    End Try
                End While
            End Using
            Dim mVal As Double = 0
            Dim cadenaM As String = ""
            Dim bVal As Double = 0
            Dim cadenaB As String = ""
            For Each mVal In mArray
                cadenaM += mVal.ToString + " ; "
            Next
            For Each bVal In bArray
                cadenaB += bVal.ToString + " ; "
            Next
            SysLog("I: arreglos m de calibración : " + cadenaM)
            SysLog("I: arreglos b de calibración : " + cadenaB)
        Catch ex As Exception
            MsgBox("Error: no hay archivo de calibración para termocuplas")
        End Try


    End Sub

    '*********************************************************************
    '_______________________________METODOS_______________________________

    '*********************************************************************
    'Despliega los nombres de los COM`s disponibles para seleccionarlo
    Sub GetSerialPortNames()
        Dim sp = My.Computer.Ports.SerialPortNames

        If sp.Count > 0 Then
            puerto = sp.ElementAt(0)
        Else
            MsgBox("No se encuentra activo ningun puerto COM, por favor verifique conexiones e intente de nuevo.",
                       MsgBoxStyle.Critical, "Cuidado!!!")
        End If

        'Si la variable puerto no esta vacia inicia proceso de coneccion
        If puerto IsNot Nothing Then
            com = New SerialPort(puerto)

            Try
                With com
                    If .IsOpen Then
                        .Close()
                    End If

                    .BaudRate = 9600
                    .DataBits = 8
                    .Parity = IO.Ports.Parity.None
                    .StopBits = IO.Ports.StopBits.One
                    .ReadTimeout = 10000 '#WCS #300420DA #CS Modificado porque se va a excepcion timeout al leer puerto seial en modo archivo 
                    .Open()
                End With

                MsgBox("Conexion Exitosa con el puerto: " + puerto.ToString, MsgBoxStyle.Information, "Soft-Laser")
                ToolStripStatusLabel1.Text = "COM - Port = " + puerto.ToString
                EstadoConnectPuerto = True

            Catch ex As Exception
                MsgBox("Error al abrir el puerto serial: " & ex.Message, MsgBoxStyle.Critical, "Soft-Laser")
                EstadoConnectPuerto = False

                Me.Close()
            End Try
        Else
            MessageBox.Show("Primero seleccione el puerto al cual desea conectarse.", "Error!!!", MessageBoxButtons.OK)
            EstadoConnectPuerto = False
        End If
    End Sub

    Private Sub ButtonConectarBarcode_Click(sender As Object, e As EventArgs) Handles ButtonConectarBarcode.Click
        rstCounter = 0

        If ArchivoProyecto.Equals(String.Empty) Or ArchivoProyecto.Equals("") Then
            Dim name As New Form_NameFile
            name.Show()
        Else
            ruta = "C:\Users\Public\" + ArchivoProyecto + ".txt"
            ruta2 = "C:\Users\Public\" + ArchivoProyecto + "_RAW.txt"
            SysLog("I: Archivo de proyecto : " + " C:\Users\Public\" + ArchivoProyecto + ".txt")

            'escritor = File.AppendText(ruta)

            If CheckBoxModo.Checked Then
                SysLog("I: Programa configurado en MODO SEGUIMIENTO DE ARCHIVO") '#010520DA #CS #WCS - Log
                ButtonConectarBarcode.Enabled = False
                'Variable que almacena la ruta del archivo seleccionado
                Dim myFileDialog As New OpenFileDialog()

                Try
                    'Filtro para solo archivos .dat
                    With myFileDialog
                        .Filter = "Excel Files |*.csv"
                        .Title = "Open File"
                        .ShowDialog()
                    End With

                    If myFileDialog.FileName.ToString <> "" Then
                        SysLog("I: Archivo para seguimiento : " + myFileDialog.FileName) '#010520DA #CS #WCS - Log
                        Dim tfp As New TextFieldParser(myFileDialog.FileName.ToString, System.Text.Encoding.ASCII)
                        tfp.SetDelimiters(";")

                        Do While Not tfp.EndOfData
                            'dtClient.Rows.Add(tfp.ReadFields)
                            ArrayListCsv.Add(tfp.ReadFields) '#290420 #RCS arreglo para cargar archivo a seguir
                        Loop
                        SysLog("I: Líneas de archivo - " + ArrayListCsv.Count.ToString) '#010520DA #CS #WCS - Log
                    End If

                    ThreadingModoArchivo.Start()
                Catch ex As Exception
                    MsgBox("Wrong file format, try again. ", MsgBoxStyle.Critical, "Cemex")

                End Try
            Else
                Try
                    If com.IsOpen Then
                        ButtonConectarBarcode.Enabled = False
                        com.Write("RST%")

                        VariableFuncionamiento = False
                        Try
                            SysLog("I: Iniciando hilo de lectura")
                            ThreadingLectura.Start()
                        Catch ex As Exception

                        End Try

                    Else
                        MessageBox.Show("Primero seleccione el puerto al cual desea conectarse.", "Error!!!", MessageBoxButtons.OK)
                        EstadoConnectPuerto = False
                    End If
                Catch ex As Exception
                    MessageBox.Show("Primero seleccione el puerto al cual desea conectarse.", "Error!!!", MessageBoxButtons.OK)
                    EstadoConnectPuerto = False
                End Try

            End If
        End If


    End Sub

    Private Sub hiloLectura()
        'Declaración de variables
        Dim flagTimer As Boolean = False
        Dim flagArray() As Boolean = {True, True}
        Dim flagGrafModulo As Boolean = False
        Dim cadena As String = ""
        Dim datosCadena As String = ""
        flagMedidaAnterior = False
        'Inicialización de variables
        recepcionContador = 0
        flagRecepcion = True
        flagRead = True
        contadorTimeOut = 0
        limContadorComparacion = 8 '#110620DA #WCS a valor 200 '#130620DA #WCS a valor 20 ‘#060920WCS #271020WCS a 8

        ToolStripStatusLabel2.Text = "COM - State = " + "Connected"
        SysLog("I: " + "COM - State = " + "Connected")
        For i = 0 To (ArrayNames.Count - 1)
            Chart1.Series(ArrayNames(i)).Enabled = False
        Next

        While True
            'Variables para ciclos de lectura

            Dim strCompara As String = ""
            Dim medidaValida As Boolean = False
            Dim CadenaTablaInterface = New ArrayList
            Dim flagCalibracion As Boolean = False
            recepcionContador = 0
            cadena = recepcionCadena()
            Flag = True 'Andres: Siempre debe iniciar este Flag en true, en caso de que VariableFuncionamiento pase a False
            contadorLoRa = 0 'Andres: Se inicia el contador en 0 para incrementarlo mientras se realiza medidas

            If cadena.Contains("CONFIG") Then 'Revisa que el ESP32 esté pidiendo configuración para enviar datos
                VariableFuncionamiento = arregloConfiguracion() 'Crea el arreglo de configuración y el de comparacion
                While VariableFuncionamiento
                    flagRecepcion = True
                    esperaDatosInalambricos() 'espera que haya datos inalámbricos disponibles en el ESP32 de control
                    'CONDICIONES DE MEDICIÓN TIMER
                    flagTimer = condicionTemporizador()
                    'CONDICIÓN DE LECTURA : Temporizador multiplo minuto y que haya nuevo dato
                    If flagTimer And flagRead Or flagMedidaInvalida Then
                        minutoTemp = minuto 'variable para no medir dos veces en el mismo minuto
                        SysLog("I: Secuencia de medición minuto : " + minuto.ToString)
                        com.DiscardInBuffer() 'Se borra el buffer para evitar errores en inicio de medición
                        If flagCalibracion Then
                            configurarCalibracion() 'funcion de configuración de calibración
                            flagCalibracion = False
                        End If
                        datosCadena = ObtenerCadena() 'Se obtiene una nueva cadena desde el ESP32
                        strCompara = ControladorTemperaturaGrafico(datosCadena) 'Se gráfica el estado de los controladores de temperatura
                        'Condición para continuar análisis de información en ArrayIn
                        If ArrayIn.ElementAt(0).Equals("*") And ArrayIn.ElementAt(ArrayIn.Count - 1).Contains("%") And ArrayConfigCompara.Equals(strCompara) Then

                            CadenaTabla.Clear() 'cadena para escribir al archivo o a la tabla
                            CadenaTablaInterface.Clear()
                            CadenaTabla.Add(Now.ToString("yyyy-MM-dd HH:mm:ss"))

                            medidaValida = ObtenerMedidas(datosCadena)
                            If medidaValida Then
                                SysLog("I: Medida validada")
                                If minuto Mod grafModuloMin = 0 Then 'Establece la condición para graficar un nuevo dato
                                    flagGrafModulo = True
                                Else
                                    flagGrafModulo = False
                                End If
                                Try '#260720DA #CS excepción adiconal para problema en el form data visualitation. 
                                    If flagGrafModulo Then 'Se cumple la condición para graficar un nuevo dato
                                        'Procedimiento para graficar
                                        For i = 0 To 23
                                            If ArrayControlTableGraf(i) = 1 Then 'Solo llena las posiciones de la tabla configuradas para medir
                                                Try
                                                    Dim promedioVariable As Double = promedio(arregloMedidas.Item(i)) 'Cambiar el 1 por variable para recorrer el arreglo
                                                    Chart1.Series(ArrayNames(i)).Points.AddY(promedioVariable)
                                                    arregloMedidas.Item(i).clear
                                                Catch ex As Exception
                                                    SysLog("E: No hay dato para medir modo normal - plot")
                                                End Try
                                            End If
                                        Next

                                    End If
                                Catch ex As Exception
                                    SysLog("E: excepcion para - plot")
                                End Try



                                If flagGrafModulo Then
                                    Try
                                        Chart1.ChartAreas(0).AxisX.CustomLabels.Add(Counter - 1, Counter + 1, Now.ToString("HH:mm:ss"))
                                        Counter += 1
                                    Catch ex As Exception
                                        SysLog("E: No hay dato para medir modo normal time - plot")
                                    End Try

                                End If
                                'Fin sección graficar punto

                                Try
                                    CadenaTablaInterface = CadenaTabla
                                    DataGridView1.Rows.Add(CadenaTablaInterface.ToArray)
                                Catch ex As Exception
                                    SysLog("E: No hay dato para medir modo normal time - datagrid")
                                End Try


                                Try
                                    escritor = File.AppendText(ruta)
                                    escritor2 = File.AppendText(ruta2)
                                Catch ex As Exception
                                    SysLog("E: no se pudo agregar cadena al escritor")
                                End Try


                                Dim cadenaArchivoTemp As String = ""
                                For g = 0 To (CadenaTabla.Count - 1)
                                    cadenaArchivoTemp += CadenaTabla(g).ToString
                                    cadenaArchivoTemp += ";"
                                Next

                                Try
                                    escritor.WriteLine(cadenaArchivoTemp)
                                    escritor.Flush()
                                    escritor.Close()

                                    escritor2.WriteLine(cadenaArchivoTemp)
                                    escritor2.Flush()
                                    escritor2.Close()
                                Catch ex As Exception
                                    SysLog("E: no se pudo agregar cadena al archivo")
                                End Try


                                Try
                                    DataGridView1.FirstDisplayedScrollingRowIndex = DataGridView1.Rows.Count - 1
                                Catch ex As Exception
                                    SysLog("E: No hay dato para medir modo normal time - datagrid2")
                                End Try

                                flagMedidaInvalida = False
                                Thread.Sleep(SleepLectura)
                                SysLog("I: Medida consolidada minuto modo normal : " + minuto.ToString)
                            Else
                                SysLog("E: Medida invalida o fuera de rango")
                                flagMedidaInvalida = True '#010520DA #CS #wCS - bandera para retomar ciclo de medición en el mismo minuto
                                Thread.Sleep(1000)

                                Try '#190720DA #CS
                                    escritor2 = File.AppendText(ruta2)
                                Catch ex As Exception
                                    SysLog("E: Cadena a archivo escritor raw excepcion")
                                End Try

                                Try '#190720DA #CS
                                    Dim cadenaArchivoTemp As String = "" '#190720DA #CS
                                    For g = 0 To (CadenaTabla.Count - 1)
                                        cadenaArchivoTemp += CadenaTabla(g).ToString
                                        cadenaArchivoTemp += ";"
                                    Next
                                    escritor2.WriteLine(cadenaArchivoTemp)
                                    escritor2.Flush()
                                    escritor2.Close()
                                    SysLog("E: Cadena a archivo raw")
                                Catch ex As Exception
                                    SysLog("E: Cadena a archivo raw excepcion")
                                End Try

                            End If
                            'Sección graficar punto


                        ElseIf inData.Contains("ERROR%") Then
                            com.Write("RST%")
                            SysLog("E: ERROR recibido desde ESP32")
                            ToolStripStatusLabel5.Text = "Reiniciando controlador Modo ERROR..." '#140520WCS
                            Thread.Sleep(3000) '#140520WCS
                            VariableFuncionamiento = False
                        Else
                            com.Write("RST%")
                            SysLog("I: RST enviado a ESP32 #1") '#140520WCS
                            ToolStripStatusLabel5.Text = "Reiniciando controlador Modo normal..." '#140520WCS
                            Thread.Sleep(3000) '#140520WCS
                            VariableFuncionamiento = False
                        End If
                        'Andres: Contador para resetear el TTGO y asegurar la comunicación LoRa



                        'Andres: Fin de contador
                    End If 'Temporizador de medición
                    If contadorLoRa >= limiteContadorLora Then
                        com.Write("RST%")
                        VariableFuncionamiento = False
                        Flag = True
                        ToolStripStatusLabel5.Text = "Reconfirmando comunicación inalámbrica, espere unos segundos... c = " + rstCounter.ToString
                        SysLog("I: reiniciando ESP32 por no tener nuevos datos desde conexión inalámbrica desde hace " + limiteContadorLora.ToString + " intentos")
                        rstCounter += 1
                        Thread.Sleep(3000)
                    End If

                End While

            Else
                SysLog("I: Envío de RST a ESP 32 #2")
                com.Write("RST%")
                ToolStripStatusLabel5.Text = "Reiniciando controlador Modo normal..." '#140520WCS
                Thread.Sleep(3000) '#140520WCS
                VariableFuncionamiento = False
            End If
        End While
    End Sub

    Private Sub hiloLecturaArchivo() '#280420 #CS
        Dim flagTimer As Boolean = False
        Dim flagArray() As Boolean = {True, True}
        Dim flagGrafModulo As Boolean = False
        Dim cadena As String = ""
        Dim datosCadena As String = ""
        Dim lineaDatosArchivo As Boolean = True
        Dim funcionamientoArchivo As Boolean = True
        flagArchivo = True
        limContadorComparacion = 8 '#110620DA #WCS a valor 200 '#130620DA #WCS a valor 20 a 10 ‘#060920WCS #271020WCS a 8
        contadorTimeOut = 0 '#030520DA #WCS
        flagMedidaAnterior = False

        'Inicialización de variables
        recepcionContador = 0
        flagRecepcion = True
        flagRead = True

        ToolStripStatusLabel2.Text = "COM - State = " + "Connected"
        SysLog("I: " + "COM - State = " + "Connected")
        For i = 0 To (ArrayNames.Count - 1)
            Chart1.Series(ArrayNames(i)).Enabled = False
        Next

        While lineaDatosArchivo
            Dim strCompara As String = ""
            Dim medidaValida As Boolean = False
            Dim CadenaTablaInterface = New ArrayList
            Dim flagCalibracion As Boolean = False
            recepcionContador = 0
            cadena = recepcionCadena()
            Flag = True 'Andres: Siempre debe iniciar este Flag en true, en caso de que VariableFuncionamiento pase a False
            contadorLoRa = 0 'Andres: Se inicia el contador en 0 para incrementarlo mientras se realiza medidas

            If cadena.Contains("CONFIG") Then 'Revisa que el ESP32 esté pidiendo configuración para enviar datos
                funcionamientoArchivo = True '#010520DA #CS #wCS -se metió función al While para cambiar la cadena de configuración
                While funcionamientoArchivo
                    funcionamientoArchivo = arregloConfiguracion() '#010520DA #CS #wCS -se metió función al While para cambiar la cadena de configuración
                    flagRecepcion = True
                    esperaDatosInalambricos() 'espera que haya datos inalámbricos disponibles en el ESP32 de control
                    'Obtener cadena
                    'INICIA CONDICIONES DE TEMPORIZADOR #010520DA #WCS
                    'SysLog("I: Bandera flagTimer : " + flagTimer.ToString + " - bandera Flag Read : " + lineaArchivo.ToString)
                    flagTimer = condicionTemporizador()
                    'CONDICIÓN DE LECTURA : Temporizador multiplo minuto y que haya nuevo dato
                    If flagTimer And flagRead Or flagMedidaInvalida Then 'Revisar para el  Not medidaValida
                        minutoTemp = minuto 'variable para no medir dos veces en el mismo minuto
                        SysLog("I: Secuencia de medición minuto : " + minuto.ToString + " - línea de archivo : " + lineaArchivo.ToString)
                        com.DiscardInBuffer() 'Se borra el buffer para evitar errores en inicio de medición
                        If flagCalibracion Then
                            configurarCalibracion() 'funcion de configuración de calibración
                            flagCalibracion = False
                        End If
                        datosCadena = ObtenerCadena() 'Se obtiene una nueva cadena desde el ESP32
                        strCompara = ControladorTemperaturaGrafico(datosCadena) 'Se gráfica el estado de los controladores de temperatura
                        'Condición para continuar análisis de información en ArrayIn

                        If ArrayIn.ElementAt(0).Equals("*") And ArrayIn.ElementAt(ArrayIn.Count - 1).Contains("%") And ArrayConfigCompara.Equals(strCompara) Then
                            SysLog("I: Ingreso a condicional, Cadena Comparación : " + strCompara) '#010520DA #WCS temporal por debug se puede borrar
                            CadenaTabla.Clear() 'cadena para escribir al archivo o a la tabla
                            CadenaTablaInterface.Clear()
                            CadenaTabla.Add(Now.ToString("yyyy-MM-dd HH:mm:ss"))

                            medidaValida = ObtenerMedidas(datosCadena)
                            If medidaValida Then
                                SysLog("I: Medida validada")
                                If minuto Mod grafModuloMin = 0 Then 'Establece la condición para graficar un nuevo dato
                                    flagGrafModulo = True
                                Else
                                    flagGrafModulo = False
                                End If

                                Try '#260720DA #CS excepción adiconal para problema en el form data visualitation. 
                                    If flagGrafModulo Then 'Se cumple la condición para graficar un nuevo dato
                                        'Procedimiento para graficar
                                        For i = 0 To 23
                                            If ArrayControlTableGraf(i) = 1 Then 'Solo llena las posiciones de la tabla configuradas para medir
                                                Try
                                                    Dim promedioVariable As Double = promedio(arregloMedidas.Item(i)) 'Cambiar el 1 por variable para recorrer el arreglo
                                                    Chart1.Series(ArrayNames(i)).Points.AddY(promedioVariable)
                                                    arregloMedidas.Item(i).clear
                                                Catch ex As Exception
                                                    SysLog("E: No hay dato para medir modo archivo - plot")
                                                End Try
                                            End If
                                        Next

                                    End If
                                Catch ex As Exception
                                    SysLog("E: excepcion para archivo - plot")
                                End Try

                                If flagGrafModulo Then
                                    Try
                                        Chart1.ChartAreas(0).AxisX.CustomLabels.Add(Counter - 1, Counter + 1, Now.ToString("HH:mm:ss"))
                                        Counter += 1
                                    Catch ex As Exception
                                        SysLog("E: no había medida para graficar - plot")
                                    End Try

                                End If
                                'Fin sección graficar punto


                                CadenaTablaInterface = CadenaTabla
                                DataGridView1.Rows.Add(CadenaTablaInterface.ToArray)

                                Try
                                    escritor = File.AppendText(ruta)
                                    escritor2 = File.AppendText(ruta2)
                                Catch ex As Exception

                                End Try


                                Dim cadenaArchivoTemp As String = ""
                                For g = 0 To (CadenaTabla.Count - 1)
                                    cadenaArchivoTemp += CadenaTabla(g).ToString
                                    cadenaArchivoTemp += ";"
                                Next

                                Try
                                    escritor.WriteLine(cadenaArchivoTemp)
                                    escritor.Flush()
                                    escritor.Close()

                                    escritor2.WriteLine(cadenaArchivoTemp)
                                    escritor2.Flush()
                                    escritor2.Close()
                                Catch ex As Exception

                                End Try

                                DataGridView1.FirstDisplayedScrollingRowIndex = DataGridView1.Rows.Count - 1

                                SysLog("I: Medida consolidada minuto modo archivo : " + minuto.ToString) '#140520WCS

                                Thread.Sleep(SleepLectura)
                                flagMedidaInvalida = False
                                If lineaArchivo = (ArrayListCsv.Count - 1) Then 'Revisa si quedan lineas que leer en el archivo 
                                    lineaDatosArchivo = False
                                    funcionamientoArchivo = False
                                    SysLog("I: Se ha llegado a la última línea de archivo : " + lineaArchivo.ToString) '#010520DA #CS #wCS
                                    ToolStripStatusLabel5.Text = "Archivo para seguimiento finalizado - ensayo finalizado" '#010520DA #CS #wCS
                                    'VariableFuncionamientoModoFile = False
                                End If
                                lineaArchivo += 1
                            Else
                                SysLog("E: Medida invalida o fuera de rango")
                                flagMedidaInvalida = True '#010520DA #CS #wCS - bandera para retomar ciclo de medición en el mismo minuto

                                Try '#190720DA #CS
                                    escritor2 = File.AppendText(ruta2)
                                Catch ex As Exception
                                End Try

                                Try '#190720DA #CS
                                    Dim cadenaArchivoTemp As String = "" '#190720DA #CS
                                    For g = 0 To (CadenaTabla.Count - 1)
                                        cadenaArchivoTemp += CadenaTabla(g).ToString
                                        cadenaArchivoTemp += ";"
                                    Next
                                    escritor2.WriteLine(cadenaArchivoTemp)
                                    escritor2.Flush()
                                    escritor2.Close()
                                    SysLog("E: Cadena a archivo raw")
                                Catch ex As Exception
                                End Try

                                Thread.Sleep(1000)
                            End If
                            'Sección graficar punto

                        ElseIf inData.Contains("ERROR%") Then
                            com.Write("RST%")
                            SysLog("E: ERROR recibido desde ESP32")
                            ToolStripStatusLabel5.Text = "Reiniciando controlador Modo normal..." '#140520WCS
                            Thread.Sleep(3000) '#140520WCS
                            VariableFuncionamiento = False
                        Else
                            SysLog("E: La cadena recibida no cumple para ser leída, Cadena de comparación : " + strCompara) '#010520DA #WCS temporal por debug se puede borrar
                            SysLog("E: Arreglo de comparación : " + ArrayConfigCompara) '#010520DA #WCS temporal por debug se puede borrar
                            com.Write("RST%")
                            SysLog("E: Modo Archivo - RST enviado a ESP32 #1") '#140520WCS
                            ToolStripStatusLabel5.Text = "Reiniciando controlador Modo archivo..." '#140520WCS
                            Thread.Sleep(3000)
                            funcionamientoArchivo = False
                        End If
                        'Andres: Contador para resetear el TTGO y asegurar la comunicación LoRa



                        'Andres: Fin de contador
                    End If 'Temporizador de medición



                End While
            End If

        End While
        'VERIFICACIÓN DE CONDICIONES
        '-ARCHIVO CONSISTENCIA
        '-ASIGNACIÓN DE ARCHIVO A TERMO (SOLO UNA POR ENSAYO)
        '-DEFINIR UN TIEMPO FINAL PARA EL ENSAYO - STOP FINAL U OTRO MÉTODO PARA TERMINAR
        'LECTURA DE CADENA SERIAL Y COMPROBACIÓN
        'TEMPORIZADOR DE ACTIVACIÓN
        'GENERACIÓN DE CADENA PARA LECTURA
        '-SE CREA UNA CADENA DE LECTURA TOTAL
        '-SE CONCATENA CON EL ARCHIVO
        '-SE ENVIA A LOS PROCEDIMIENTOS DE COMPROBACIÓN
        'PROCESO NORMAL DE TEMPORIZADORES PARA GRAFICAR

    End Sub


    Private Sub ButtonButtonCancelPort_Click(sender As Object, e As EventArgs) Handles ButtonCancelPort.Click
        'Dim config As New Form1
        'config.Show()
        Dim result As DialogResult = MessageBox.Show("Do you confirm stop?",
                              "Title",
                              MessageBoxButtons.YesNo)

        If result = DialogResult.Yes Then
            SysLog("I: Confirmación de stop program ")
            Try
                com.Write("RST%")
            Catch ex As Exception

            End Try

            Try
                ThreadingLectura.Abort()

            Catch ex As Exception

            End Try

            Try
                ThreadingModoArchivo.Abort()
            Catch ex As Exception

            End Try

        Else
            SysLog("I: Negación de stop program")
        End If

    End Sub

    Private Sub Form_Port_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.Closing
        Try
            com.Write("RST%")
        Catch ex As Exception

        End Try

        Try
            ThreadingLectura.Abort()

        Catch ex As Exception

        End Try

        Try
            ThreadingModoArchivo.Abort()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ButtonConfig_Click(sender As Object, e As EventArgs) Handles ButtonConfig.Click
        Dim config As New Form_Settings
        config.Show()
    End Sub

    Private Sub COMConnectionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles COMConnectionToolStripMenuItem.Click
        Dim Com_port As New Form_Port
        Com_port.Show()
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingsToolStripMenuItem.Click
        Dim config As New Form_Settings
        config.Show()
    End Sub

    Private Sub CalibrationSettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CalibrationSettingsToolStripMenuItem.Click
        Dim config As New Form_Calibration
        config.Show()
    End Sub

    Private Sub ButtonCalibra_Click(sender As Object, e As EventArgs) Handles ButtonCalibra.Click
        Dim config As New Form_Calibration
        config.Show()
    End Sub

    Function ConversorArrayConf(ArrayInConf As String) As String

        Dim ArrayManage() As String
        ArrayManage = ArrayInConf.Split(",")

        For i = 1 To (ArrayManage.Count - 1)
            If (ArrayManage(i).Contains("T")) Then
                ArrayManage(i) = ArrayManage(i).Replace("T - ", String.Empty)

            ElseIf (ArrayManage(i).Equals("None")) Then
                ArrayManage(i) = "0"
            End If
        Next

        ArrayInConf = String.Empty

        For i = 0 To (ArrayManage.Count - 1)
            ArrayInConf += ArrayManage(i)
            If i <> (ArrayManage.Count - 1) Then
                ArrayInConf += ","
            End If
        Next

        Return ArrayInConf

    End Function

    Private Sub ButtonExaminar_Click(sender As Object, e As EventArgs) Handles ButtonExaminar.Click
        If DataGridView1.RowCount > 0 Then
            Dim save As New SaveFileDialog

            save.Filter = "Excel Document (*.xlsx)|*.xlsx"
            save.FileName = "Registro" & " " & Now.Day & "-" & Now.Month & "-" & Now.Year & ".xlsx"

            If save.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim xlApp As Microsoft.Office.Interop.Excel.Application
                Dim xlWorkBook As Microsoft.Office.Interop.Excel.Workbook
                Dim xlWorkSheet As Microsoft.Office.Interop.Excel.Worksheet
                Dim misValue As Object = System.Reflection.Missing.Value
                Dim i As Integer
                Dim j As Integer

                xlApp = New Microsoft.Office.Interop.Excel.Application
                xlWorkBook = xlApp.Workbooks.Add(misValue)
                Try
                    xlWorkSheet = xlWorkBook.Sheets("Hoja1")
                Catch ex As Exception
                    xlWorkSheet = xlWorkBook.Sheets("sheet1")
                End Try


                For i = 0 To DataGridView1.RowCount - 2
                    For j = 0 To DataGridView1.ColumnCount - 1
                        For k As Integer = 1 To DataGridView1.Columns.Count
                            xlWorkSheet.Cells(1, k) = DataGridView1.Columns(k - 1).HeaderText
                            xlWorkSheet.Cells(i + 2, j + 1) = DataGridView1(j, i).Value.ToString()
                        Next
                    Next
                Next

                xlWorkSheet.SaveAs(save.FileName)
                xlWorkBook.Close()
                xlApp.Quit()

                releaseObject(xlApp)
                releaseObject(xlWorkBook)
                releaseObject(xlWorkSheet)

                MsgBox("Successfully saved" & vbCrLf & "File are saved at : " & save.FileName, MsgBoxStyle.Information, "Information")
            End If
        Else
            MsgBox("No se encontraron datos para exportar", MsgBoxStyle.Critical, "Verificar")
            Exit Sub
        End If
    End Sub

    Private Sub ButtonReset_Click(sender As Object, e As EventArgs) Handles ButtonReset.Click
        Try
            com.Write("RST%")
        Catch ex As Exception

        End Try

        Try
            ThreadingLectura.Abort()

        Catch ex As Exception

        End Try

        Try
            ThreadingModoArchivo.Abort()
        Catch ex As Exception

        End Try

        Try
            Application.Restart()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub checkT1_Click(sender As Object, e As EventArgs) Handles checkT1.Click
        If Not control(0) Then
            control(0) = True
            checkT1.Image = My.Resources.check
            Chart1.Series(ArrayNames(0)).Enabled = True
            'Chart1.Series("t1").Enabled = True
        Else
            control(0) = False
            checkT1.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(0)).Enabled = False
            'Chart1.Series("t1").Enabled = False
        End If
    End Sub

    Private Sub checkT2_Click(sender As Object, e As EventArgs) Handles checkT2.Click
        If Not control(1) Then
            control(1) = True
            checkT2.Image = My.Resources.check
            Chart1.Series(ArrayNames(1)).Enabled = True
        Else
            control(1) = False
            checkT2.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(1)).Enabled = False
        End If
    End Sub

    Private Sub checkT3_Click(sender As Object, e As EventArgs) Handles checkT3.Click
        If Not control(2) Then
            control(2) = True
            checkT3.Image = My.Resources.check
            Chart1.Series(ArrayNames(2)).Enabled = True
        Else
            control(2) = False
            checkT3.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(2)).Enabled = False
        End If
    End Sub

    Private Sub checkT4_Click(sender As Object, e As EventArgs) Handles checkT4.Click
        If Not control(3) Then
            control(3) = True
            checkT4.Image = My.Resources.check
            Chart1.Series(ArrayNames(3)).Enabled = True
        Else
            control(3) = False
            checkT4.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(3)).Enabled = False
        End If
    End Sub

    Private Sub checkT5_Click(sender As Object, e As EventArgs) Handles checkT5.Click
        If Not control(4) Then
            control(4) = True
            checkT5.Image = My.Resources.check
            Chart1.Series(ArrayNames(4)).Enabled = True
        Else
            control(4) = False
            checkT5.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(4)).Enabled = False
        End If
    End Sub

    Private Sub checkT6_Click(sender As Object, e As EventArgs) Handles checkT6.Click
        If Not control(5) Then
            control(5) = True
            checkT6.Image = My.Resources.check
            Chart1.Series(ArrayNames(5)).Enabled = True
        Else
            control(5) = False
            checkT6.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(5)).Enabled = False
        End If
    End Sub

    Private Sub checkT7_Click(sender As Object, e As EventArgs) Handles checkT7.Click
        If Not control(6) Then
            control(6) = True
            checkT7.Image = My.Resources.check
            Chart1.Series(ArrayNames(6)).Enabled = True
        Else
            control(6) = False
            checkT7.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(6)).Enabled = False
        End If
    End Sub

    Private Sub checkT8_Click(sender As Object, e As EventArgs) Handles checkT8.Click
        If Not control(7) Then
            control(7) = True
            checkT8.Image = My.Resources.check
            Chart1.Series(ArrayNames(7)).Enabled = True
        Else
            control(7) = False
            checkT8.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(7)).Enabled = False
        End If
    End Sub

    Private Sub checkT9_Click(sender As Object, e As EventArgs) Handles checkT9.Click
        If Not control(8) Then
            control(8) = True
            checkT9.Image = My.Resources.check
            Chart1.Series(ArrayNames(8)).Enabled = True
        Else
            control(8) = False
            checkT9.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(8)).Enabled = False
        End If
    End Sub

    Private Sub checkT10_Click(sender As Object, e As EventArgs) Handles checkT10.Click
        If Not control(9) Then
            control(9) = True
            checkT10.Image = My.Resources.check
            Chart1.Series(ArrayNames(9)).Enabled = True
        Else
            control(9) = False
            checkT10.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(9)).Enabled = False
        End If
    End Sub

    Private Sub checkT11_Click(sender As Object, e As EventArgs) Handles checkT11.Click
        If Not control(10) Then
            control(10) = True
            checkT11.Image = My.Resources.check
            Chart1.Series(ArrayNames(10)).Enabled = True
        Else
            control(10) = False
            checkT11.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(10)).Enabled = False
        End If
    End Sub

    Private Sub checkT12_Click(sender As Object, e As EventArgs) Handles checkT12.Click
        If Not control(11) Then
            control(11) = True
            checkT12.Image = My.Resources.check
            Chart1.Series(ArrayNames(11)).Enabled = True
        Else
            control(11) = False
            checkT12.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(11)).Enabled = False
        End If
    End Sub

    Private Sub checkC1_Click(sender As Object, e As EventArgs) Handles checkC1.Click
        If Not control2(0) Then
            control2(0) = True
            checkC1.Image = My.Resources.check
            Chart1.Series(ArrayNames(12)).Enabled = True
        Else
            control2(0) = False
            checkC1.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(12)).Enabled = False
        End If
    End Sub

    Private Sub checkC2_Click(sender As Object, e As EventArgs) Handles checkC2.Click
        If Not control2(1) Then
            control2(1) = True
            checkC2.Image = My.Resources.check
            Chart1.Series(ArrayNames(13)).Enabled = True
        Else
            control2(1) = False
            checkC2.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(13)).Enabled = False
        End If
    End Sub

    Private Sub checkC3_Click(sender As Object, e As EventArgs) Handles checkC3.Click
        If Not control2(2) Then
            control2(2) = True
            checkC3.Image = My.Resources.check
            Chart1.Series(ArrayNames(14)).Enabled = True
        Else
            control2(2) = False
            checkC3.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(14)).Enabled = False
        End If
    End Sub

    Private Sub checkC4_Click(sender As Object, e As EventArgs) Handles checkC4.Click
        If Not control2(3) Then
            control2(3) = True
            checkC4.Image = My.Resources.check
            Chart1.Series(ArrayNames(15)).Enabled = True
        Else
            control2(3) = False
            checkC4.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(15)).Enabled = False
        End If
    End Sub

    Private Sub checkC5_Click(sender As Object, e As EventArgs) Handles checkC5.Click
        If Not control2(4) Then
            control2(4) = True
            checkC5.Image = My.Resources.check
            Chart1.Series(ArrayNames(16)).Enabled = True
        Else
            control2(4) = False
            checkC5.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(16)).Enabled = False
        End If
    End Sub

    Private Sub checkC6_Click(sender As Object, e As EventArgs) Handles checkC6.Click
        If Not control2(5) Then
            control2(5) = True
            checkC6.Image = My.Resources.check
            Chart1.Series(ArrayNames(17)).Enabled = True
        Else
            control2(5) = False
            checkC6.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(17)).Enabled = False
        End If
    End Sub

    Private Sub checkC7_Click(sender As Object, e As EventArgs) Handles checkC7.Click
        If Not control2(6) Then
            control2(6) = True
            checkC7.Image = My.Resources.check
            Chart1.Series(ArrayNames(18)).Enabled = True
        Else
            control2(6) = False
            checkC7.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(18)).Enabled = False
        End If
    End Sub

    Private Sub checkC8_Click(sender As Object, e As EventArgs) Handles checkC8.Click
        If Not control2(7) Then
            control2(7) = True
            checkC8.Image = My.Resources.check
            Chart1.Series(ArrayNames(19)).Enabled = True
        Else
            control2(7) = False
            checkC8.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(19)).Enabled = False
        End If
    End Sub

    Private Sub checkC9_Click(sender As Object, e As EventArgs) Handles checkC9.Click
        If Not control2(8) Then
            control2(8) = True
            checkC9.Image = My.Resources.check
            Chart1.Series(ArrayNames(20)).Enabled = True
        Else
            control2(8) = False
            checkC9.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(20)).Enabled = False
        End If
    End Sub

    Private Sub checkC10_Click(sender As Object, e As EventArgs) Handles checkC10.Click
        If Not control2(9) Then
            control2(9) = True
            checkC10.Image = My.Resources.check
            Chart1.Series(ArrayNames(21)).Enabled = True
        Else
            control2(9) = False
            checkC10.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(21)).Enabled = False
        End If
    End Sub

    Private Sub checkC11_Click(sender As Object, e As EventArgs) Handles checkC11.Click
        If Not control2(10) Then
            control2(10) = True
            checkC11.Image = My.Resources.check
            Chart1.Series(ArrayNames(22)).Enabled = True
        Else
            control2(10) = False
            checkC11.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(22)).Enabled = False
        End If
    End Sub

    Private Sub checkC12_Click(sender As Object, e As EventArgs) Handles checkC12.Click
        If Not control2(11) Then
            control2(11) = True
            checkC12.Image = My.Resources.check
            Chart1.Series(ArrayNames(23)).Enabled = True
        Else
            control2(11) = False
            checkC12.Image = My.Resources.noCheck
            Chart1.Series(ArrayNames(23)).Enabled = False
        End If
    End Sub



    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub



    Function promedio(entra As ArrayList)
        Dim sumaRetorno As Double = 0
        Dim promedioRetorno As Double = 0
        Dim errorIn As Integer = 0
        For i = 0 To (entra.Count - 1)
            If entra.Item(i) < 110 And entra.Item(i) >= 0 Then 'Andres: condicion para evitar datos fuera de rango
                sumaRetorno += entra.Item(i)
            Else
                errorIn += 1
                SysLog("E: Se recibe un dato fuera de rango en " + i.ToString)
            End If
        Next
        promedioRetorno = sumaRetorno / (entra.Count - errorIn)
        Return promedioRetorno
    End Function

    Function esperaDatosInalambricos()
        If Flag Then 'If de espera inicial. Luego de un RESET, el ESP32 necesita al menos 20 segundos para recibir los datos desde lora, para una sola araña
            Dim SleepLora As Integer = 50000
            ToolStripStatusLabel5.Text = "Recibiendo datos inalambricos, espere " + (SleepLora / 1000).ToString + " segundos"
            SysLog("I: Iniciando espera 20s para recibir datos de LoRa")
            Thread.Sleep(SleepLora)
            Flag = False
        End If
    End Function

    Function configurarCalibracion()
        'CADENAS DE CALIBRACIÓN PARA ESP32
        Dim ArrayConfigAnt As String = String.Empty
        ArrayConfigAnt = ArrayConfig
        ToolStripStatusLabel5.Text = "Configurando calibración para los sensores espere " + (5).ToString + " segundos"
        SysLog("I: Configurando calibración para los sensores, termocuplas y cilindros")
        ArrayConfig = ""

        For i = 0 To 11 'Cadena 2000 - pendiente de implementación
            If i = 0 Then
                ArrayConfig += "$,"
            End If
            ArrayConfig += (mArray(i) + 2000).ToString.Replace(",", ".")
            ArrayConfig += ","
            If i = 11 Then
                ArrayConfig += "%"
            End If
        Next
        Try
            SysLog("I: Configurando calibración para los sensores, termocuplas m - cadena mT 2000: " + ArrayConfig)
            com.Write(ArrayConfig)
            SysLog("I: Arreglo de configuración 2000 : " + ArrayConfig)
            ArrayConfig = ""
            'Lina y Andres: Flag para slepp conexion lora 
            Thread.Sleep(3000)
            inData = com.ReadLine
            SysLog("I: Cadena recibida 2000 : " + inData)
            ArrayConfig = ""
        Catch ex As Exception
            SysLog("E: Timeout en configuración 2000")
        End Try

        For i = 0 To 11 'Cadena 3000 - pendiente de implementación
            If i = 0 Then
                ArrayConfig += "$,"
            End If
            ArrayConfig += (bArray(i) + 3000).ToString.Replace(",", ".")
            ArrayConfig += ","
            If i = 11 Then
                ArrayConfig += "%"
            End If
        Next
        Try
            SysLog("I: Configurando calibración para los sensores, termocuplas b - cadena bT 3000: " + ArrayConfig)
            com.Write(ArrayConfig)
            SysLog("I: Arreglo de configuración 3000 : " + ArrayConfig)
            ArrayConfig = ""
            'Lina y Andres: Flag para slepp conexion lora 
            Thread.Sleep(3000)
            inData = com.ReadLine
            SysLog("I: Cadena recibida 3000 : " + inData)
            ArrayConfig = ""
        Catch ex As Exception
            SysLog("E: Timeout en configuración 3000")
        End Try


        For i = 12 To 23 'Cadena 4000 - cilindros, datos en formulario
            If i = 12 Then
                ArrayConfig += "$,"
            End If
            ArrayConfig += (mArray(i) + 4000).ToString.Replace(",", ".")
            ArrayConfig += ","
            If i = 23 Then
                ArrayConfig += "%"
            End If
        Next
        Try
            SysLog("I: Configurando calibración para los sensores, cilindros m - cadena mC 4000: " + ArrayConfig)
            com.Write(ArrayConfig)
            SysLog("I: Arreglo de configuración 4000 : " + ArrayConfig)
            ArrayConfig = ""
            'Lina y Andres: Flag para slepp conexion lora 
            Thread.Sleep(3000)
            inData = com.ReadLine
            SysLog("I: Cadena recibida 4000 : " + inData)
            ArrayConfig = ""
        Catch ex As Exception
            SysLog("E: Timeout en configuración 4000")
        End Try

        For i = 12 To 23  'Cadena 5000 - cilindros, datos en formulario
            If i = 12 Then
                ArrayConfig += "$,"
            End If
            ArrayConfig += (bArray(i) + 5000).ToString.Replace(",", ".")
            ArrayConfig += ","
            If i = 23 Then
                ArrayConfig += "%"
            End If
        Next
        Try
            SysLog("I: Configurando calibración para los sensores, cilindros b - cadena bC 5000: " + ArrayConfig)
            com.Write(ArrayConfig)
            SysLog("I: Arreglo de configuración 4000 : " + ArrayConfig)
            ArrayConfig = ""
            'Lina y Andres: Flag para slepp conexion lora 
            Thread.Sleep(3000)
            inData = com.ReadLine
            SysLog("I: Cadena recibida 4000 : " + inData)
            ArrayConfig = ""
        Catch ex As Exception
            SysLog("E: Timeout en configuración 4000")
        End Try

        ArrayConfig = ArrayConfigAnt

    End Function

    Function condicionTemporizador()
        Dim flagTimer As Boolean = False
        minuto = DateTime.Now.ToString("mm")
        moduloMinuto = minuto Mod multiploMinuto
        'moduloMinuto = 0
        If moduloMinuto = 0 Then 'Condición para evaluar que se encuentra en minuto para medición y que no haya medido previamente en el minuto presente
            flagTimer = True
            If minutoTemp <> minuto Then 'Condición para cambio de minuto 
                flagRead = True
            End If
            'SysLog("I: if moduloMinuto : " + moduloMinuto.ToString)
        Else
            'SysLog("I: else, moduloMinuto : " + moduloMinuto.ToString)
            flagTimer = False
            flagRead = True
        End If
        'SysLog("I: Evaluación de timers Estado FT : " + flagTimer.ToString + " FR : " + flagRead.ToString)
        Return flagTimer
    End Function

    Function ObtenerCadena()
        While flagRecepcion 'Verifica si hay nueva información
            Try

                com.Write(ArrayConfig)
                SysLog("I: Arreglo de configuración : " + ArrayConfig)
                'Lina y Andres: Flag para slepp conexion lora 
                Thread.Sleep(3000)
                inData = com.ReadLine
                If inData = inDataTemp Then
                    'No ha leido un nuevo datos - se mantiene condicional para leer
                    flagRead = True
                    SysLog("I: Se repite cadena")
                    contadorLoRa += 1
                    flagRecepcion = True
                    If contadorLoRa >= limiteContadorLora Then
                        flagRecepcion = False
                    End If
                Else
                    'Valida la cadena con la función
                    'Si la cadena es validada (True) ejecuta las siguiente lineas
                    'En otro caso vuelva a pedir una nueva cadena
                    flagRead = False
                    flagRecepcion = False
                    SysLog("I: obtenerCadena(): nueva cadena recibida : " + inData)
                    contadorLoRa = 0
                End If
                inDataTemp = inData
            Catch ex As Exception
                inData = String.Empty
                'MsgBox("Timeout error, check your connections and try again.", MsgBoxStyle.Critical, "Cemex")
                ToolStripStatusLabel5.Text = "Timeout de datos número " + recepcionContador.ToString
                recepcionContador += 1
                SysLog("E: Timeout de datos #1 contador = " + recepcionContador.ToString)
                flagRecepcion = True
                If recepcionContador >= limiteTimeout Then
                    flagRecepcion = False
                    SysLog("E: Sistema ha llegado al limite de timeout")
                End If
            End Try
        End While

        Return inData

    End Function

    Function ControladorTemperaturaGrafico(datos As String)
        Dim StrCompara As String = String.Empty
        Dim StringLeds As String = String.Empty

        Try 'Procedimiento para evaluación de estado del controlador de temperatura
            ArrayIn = Split(datos, ",")

            For p = 37 To 48
                StrCompara += ArrayIn(p)
                If p <> 48 Then
                    StrCompara += ","
                Else
                    StrCompara += "&"
                End If
            Next

            StrCompara = StrCompara.Replace(".0", String.Empty)
            StrCompara = StrCompara.Replace("0,", ",")
            StrCompara = StrCompara.Replace("0&", String.Empty)

            For i = 25 To 36
                StringLeds += ArrayIn(i)
                If i <> 36 Then
                    StringLeds += ","
                End If
            Next

            Dim ArrayManage() As String
            ArrayManage = StringLeds.Split(",")

            For i = 0 To (ArrayManage.Count - 1)
                If ArrayManage(i).Equals("1") Then
                    Select Case i
                        Case 0
                            led_C1.Image = My.Resources.green
                        Case 1
                            led_C2.Image = My.Resources.green
                        Case 2
                            led_C3.Image = My.Resources.green
                        Case 3
                            led_C4.Image = My.Resources.green
                        Case 4
                            led_C5.Image = My.Resources.green
                        Case 5
                            led_C6.Image = My.Resources.green
                        Case 6
                            led_C7.Image = My.Resources.green
                        Case 7
                            led_C8.Image = My.Resources.green
                        Case 8
                            led_C9.Image = My.Resources.green
                        Case 9
                            led_C10.Image = My.Resources.green
                        Case 10
                            led_C11.Image = My.Resources.green
                        Case 11
                            led_C12.Image = My.Resources.green
                    End Select
                ElseIf ArrayManage(i).Equals("0") Then
                    Select Case i
                        Case 0
                            led_C1.Image = My.Resources.red
                        Case 1
                            led_C2.Image = My.Resources.red
                        Case 2
                            led_C3.Image = My.Resources.red
                        Case 3
                            led_C4.Image = My.Resources.red
                        Case 4
                            led_C5.Image = My.Resources.red
                        Case 5
                            led_C6.Image = My.Resources.red
                        Case 6
                            led_C7.Image = My.Resources.red
                        Case 7
                            led_C8.Image = My.Resources.red
                        Case 8
                            led_C9.Image = My.Resources.red
                        Case 9
                            led_C10.Image = My.Resources.red
                        Case 10
                            led_C11.Image = My.Resources.red
                        Case 11
                            led_C12.Image = My.Resources.red
                    End Select
                End If
            Next
        Catch ex As Exception
            SysLog("E: Excepción de medición #1 String led")
            'MsgBox("Message received with incorrect format", MsgBoxStyle.Critical, "Cemex")
            'com.Write("RST%")
            'VariableFuncionamiento = False
        End Try
        Return StrCompara
    End Function
    Function DatoEnRango(valor As Double, min As Integer, max As Integer)
        Dim EnRango As Boolean = False
        If valor > min And valor < max Then
            EnRango = True
        Else
            EnRango = False
        End If
        Return EnRango
    End Function
    Function ObtenerMedidas(cadena As String)
        'Tratamiento para presentación de datos en ToolStrip
        Dim datoMedido As Double = 0.0
        Dim datoMedidoDouble As Double = 0.0
        Dim rango As Boolean = False
        Dim cil5 As Double = 0.0
        Dim cil6 As Double = 0.0
        Dim medidaValida As Boolean = False
        Dim contadorRango As Integer = 0
        Dim limiteMedidaRepetida As Integer = 7 'De 60 a 10 #060920WCS de 10 a 7 #271020WCS
        Dim medidasComparacion(24) As Double
        Dim diferencia As Double = 0
        Dim diferenciaMaxima As Integer = 12 '#130720DA #CS
        Dim flagDiferencia As Boolean = True
        Dim contadorDiferencia As Integer = 0

        Dim flagMedidaRepetida As Boolean = False
        cadena = cadena.Remove(cadena.Count - 1) 'cambiar inData
        cadena = cadena.Replace("*,", String.Empty)
        cadena = cadena.Replace(",%", String.Empty)


        ToolStripStatusLabel5.Text = cadena

        For i = 1 To (ArrayIn.Count - 2)
            ArrayIn(i) = ArrayIn(i).Replace(".", ",")
        Next

        For i = 0 To 23
            If ArrayControlTableGraf(i) = 1 Then 'Solo llena las posiciones de la tabla configuradas para medir
                'arraypromedio llenado
                datoMedido = Decimal.Parse(ArrayIn(i + 1))
                datoMedidoDouble = Convert.ToDouble(ArrayIn(i + 1))
                'SysLog("Dato convertido : " + datoMedidoDouble.ToString)
                rango = DatoEnRango(datoMedido, rango_minimo, rango_maximo)
                'ingresar un datomedidocalibrado

                If rango Then 'Verifica que el dato esté en rango para decidir sin incluye el dato en el análisis
                    arregloMedidas.Item(i).add(datoMedido) 'Agrega el valor en la posición como decimal
                    medidasComparacion(i) = datoMedidoDouble 'arreglo para comparación de medidas #050520DA #WCS
                    'condicion modulo
                    ArrayIn(i + 1) = ArrayIn(i + 1).Remove(ArrayIn(i + 1).Length - 1)
                    CadenaTabla.Add(ArrayIn(i + 1))
                    Dim p As String = ArrayIn(i + 1)
                    p = p.Remove(p.Length - 1)
                    diferencia = medidasComparacion(i) - medidasComparacionAnterior(i) 'revisión de diferencia con respecto a medida anterior #130620DA #WCS
                    diferencia = Math.Abs(diferencia) 'valor absoluto de diferencia #130620DA #WCS
                    If flagMedidaAnterior Then
                        If diferencia > diferenciaMaxima Then '#130620DA #WCS
                            flagDiferencia = False
                            SysLog("E: ObtenerMedidas() - Valor de diferencia alto : " + diferencia.ToString + " - Canal : " + i.ToString) '#130620DA #WCS
                            contadorDiferencia += 1
                        End If
                    End If

                Else
                    contadorRango += 1
                    SysLog("E: ObtenerMedidas() - canal con medida fuera de rango " + i.ToString + " Valor medida : " + datoMedido.ToString)
                    Exit For
                End If
            End If
        Next


        If contadorRango = 0 And contadorDiferencia = 0 Then '#130620DA #WCS
            medidaValida = True
            If flagMedidaAnterior Then 'Solo realiza la comparación si hay medidas en el arreglo anterior
                For i = 0 To 23
                    If ArrayControlTableGraf(i) = 1 Then 'Solo llena las posiciones de la tabla configuradas para medir
                        If medidasComparacionAnterior(i) = medidasComparacion(i) Then 'REALIZAR COMPARACIÓN
                            contadorComparacion(i) += 1
                            If flagArchivo Then '#110620DA #WCS
                                SysLog("E: ObtenerMedidas() - contador de repetición valor : " + contadorComparacion(i).ToString) '#110620DA #WCS
                            Else
                                SysLog("E: ObtenerMedidas() - Medida con valor repetido en canal - " + i.ToString + " - " + medidasComparacion(i).ToString + " - Valor : " + medidasComparacionAnterior(i).ToString) '#070520DA #WCS
                            End If

                            If contadorComparacion(i) > limContadorComparacion Then
                                flagMedidaRepetida = True
                                contadorComparacion(i) = 0 '#051120 se encuentra que el problema es este contador, se debe resetear
                            End If
                        Else
                            contadorComparacion(i) = 0
                        End If

                    End If
                Next
            End If

            'COMPARACIÓN DE MEDIDAS ACTUAL Y ANTERIOR

            If flagMedidaAnterior And flagMedidaRepetida Then 'IF POSICION ONE WIRE N ACTUAL ES IGUAL A LA POSICIÓN ANTERIOR #070520DA #WCS
                medidaValida = False
                contadorMedidaInvalida += 1 '   INCREMENTAR CONTADOR
                'SysLog("E: Medidas invalidas one wire - OW5 (16) : " + cil5.ToString + " OW6 (17) : " + cil6.ToString) 'LOG DE VALOR EN POSICIÓN CUANDO SE IGUAL
                SysLog("E: medidas sin cambio flags - flagMedidaAnterior : " + flagMedidaAnterior.ToString + " flagMedidaRepetida : " + flagMedidaRepetida.ToString + " Contador medidas inválidas : " + (contadorMedidaInvalida).ToString)
                ToolStripStatusLabel5.Text = "Esperando nuevas medidas..." '#140520WCS
                Thread.Sleep(4000) '#140520WCS

                If contadorMedidaInvalida >= limiteMedidaRepetida Then 'IF CONTADOR LLEGA AL LIMITE 5 
                    flagMedidaRepetida = False 'Reset de flag por fallo de lógica #021020 #051120 el problema era el contador de comparación
                    com.Write("RST%")
                    ToolStripStatusLabel5.Text = "Reiniciando controlador flags..." '#140520WCS
                    Thread.Sleep(3000) '#140520WCS
                    SysLog("E: RST por congelación de datos en ObtenerMedidas()") '   GENERAR CICLO RST DEL ESP 32
                    contadorMedidaInvalida = 0
                End If
            Else
                'SysLog("D: Medidas validas one wire - OW5 (8) : " + promedio(arregloMedidas.Item(16)).ToString + " OW6 (9) : " + arregloMedidas.Item(17).ToString) 'LOG DE VALOR EN POSICIÓN CUANDO SE IGUAL
                SysLog("I: valor termos T1 : " + medidasComparacion(0).ToString + " Termo2 " + medidasComparacion(2).ToString + " Termo1Ant :" + medidasComparacionAnterior(0).ToString + " Termo2Ant " + medidasComparacionAnterior(1).ToString)
                contadorMedidaInvalida = 0
                medidaValida = True 'Esto solo funciona para cilindros 5 y 6 conectados - prueba 305
            End If 'ELIMINAR ESTA SECCIÓN -  SE HIZO SOLO PARA PRUEBA #030520 #WCS
            'LIMIPAR ARREGLO MEDIAS ANTERIOR PARA NUEVA ASIGNACIÓN SI SE TIENE UNA MEDIDA VÁLIDA
        Else
            medidaValida = False
            SysLog("E: Medida inválida ObtenerMedidas() - Contador rango : " + contadorRango.ToString + " - Contador diferencia : " + contadorDiferencia.ToString + " - Flag Diferencia : " + flagDiferencia.ToString) '#130620DA #WCS
        End If

        SysLog("I: Medida valida - " + medidaValida.ToString + " Número medidas invalidas : " + contadorRango.ToString)
        For i = 0 To 23 'For para asignar medidas configuradas
            If ArrayControlTableGraf(i) = 1 Then 'Solo llena las posiciones de la tabla configuradas para medir
                medidasComparacionAnterior(i) = medidasComparacion(i) 'REALIZAR ASIGNACIÓN DE MEDIDA ACTUAL A MEDIDA ANTERIOR
            End If
        Next
        If Not medidaValida Then
            Thread.Sleep(2000)
        End If
        If medidaValida Then
            flagMedidaAnterior = True
        End If

        Return medidaValida
    End Function

    Function recepcionCadena()
        Try
            inData = com.ReadLine
            SysLog("I: " + "recepcionCadena() : Cadena recibida - " + inData)
            contadorTimeOut = 0 '#030520DA #WCS
        Catch ex As Exception
            inData = String.Empty
            ToolStripStatusLabel5.Text = "Timeout de datos controlador" 'Carlos Andres: Mensaje en strip de timeout
            SysLog("E: Timeout hilo lectura ")
            Thread.Sleep(500)
            contadorTimeOut += 1
            If contadorTimeOut = 5 Then
                com.Write("RST%")
                ToolStripStatusLabel5.Text = "Reiniciando controlador recepcion..." '#140520WCS
                Thread.Sleep(3000) '#140520WCS
                SysLog("E: RST por timeout en recepcionCadena()")
            End If
        End Try
        Return inData
    End Function

    Function arregloConfiguracion()
        Dim funcionamiento As Boolean = True

        If CheckBoxModo.Checked Then
            ArrayConfig = String.Empty
            For j = 0 To 11
                If j = 0 Then
                    ArrayConfig += "$,"
                End If

                ArrayConfig += (ArrayListCsv.Item(lineaArchivo)(j) + 1000).ToString.Replace(",", ".")
                ArrayConfig += ","

                If j = 11 Then
                    ArrayConfig += "%"
                End If
            Next
            ArrayConfig = ConversorArrayConf(ArrayConfig)

            ArrayConfigCompara = ArrayConfig.Replace("$,", String.Empty)
            ArrayConfigCompara = ArrayConfigCompara.Replace(",%", String.Empty)
        Else
            SysLog("I: solicitud de config desde ESP32")
            ArrayConfig = "$," + My.Settings.C_1.ToString + "," + My.Settings.C_2.ToString + "," + My.Settings.C_3.ToString + "," + My.Settings.C_4.ToString + "," + My.Settings.C_5.ToString + "," + My.Settings.C_6.ToString + "," +
                      My.Settings.C_7.ToString + "," + My.Settings.C_8.ToString + "," + My.Settings.C_9.ToString + "," + My.Settings.C_10.ToString + "," + My.Settings.C_11.ToString + "," + My.Settings.C_12.ToString + ",%"

            ArrayConfig = ConversorArrayConf(ArrayConfig)

            ArrayConfigCompara = ArrayConfig.Replace("$,", String.Empty)
            ArrayConfigCompara = ArrayConfigCompara.Replace(",%", String.Empty)
        End If


        'SI ESTA SELECCIONADO EL CHECK MODO ARCHIVO
        'ARRAY CONFIG CON LOS DATOS DE LA LÍNEA DEL ARCHIVO
        'SINO: REALIZAR LA SIGUIENTE CONFIGURACIÓN:

        Return funcionamiento
    End Function
    Function validacionDatos(cadena As String)
        Dim ArrayInT() As String 'Arreglo para analizar los datos recibidos
        ArrayIn = Split(cadena, ",")
        If ArrayInT.ElementAt(0).Equals("*") And ArrayInT.ElementAt(ArrayInT.Count - 1).Contains("%") Then
            cadena = cadena.Remove(cadena.Count - 1)
            cadena = cadena.Replace("*,", String.Empty)
            cadena = cadena.Replace(",%", String.Empty)

            For i = 1 To (ArrayInT.Count - 2)
                ArrayInT(i) = ArrayInT(i).Replace(".", ",")
            Next


        End If

        For i = 0 To 23
            If ArrayControlTableGraf(i) = 1 Then 'Solo llena las posiciones de la tabla configuradas para medir
                'arraypromedio llenado

                Arraypromedio.Item(i).add(Decimal.Parse(ArrayIn(i + 1))) 'Agrega el valor en la posición como decimal
                'condicion modulo
            End If
        Next
    End Function
    'Función de validación de string recibida
    'Recibe Cadena - que ya se ha comprabado como nueva
    'Revisa los datos y determina que estén dentro de rango
    'Devuelve un boleano true si los datos están en rango

End Class
