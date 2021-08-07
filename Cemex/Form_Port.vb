Imports System.IO.Ports

Public Class Form_Port


    Private Sub Form_Port_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'consulta puertos com activos
        GetSerialPortNames()

        Try
            ComboBoxPuertos.SelectedIndex = 0
        Catch ex As Exception
            MsgBox("No se encuentra activo ningun puerto COM, por favor verifique conexiones e intente de nuevo.",
                       MsgBoxStyle.Critical, "Cuidado!!!")
            Me.Close()
        End Try
    End Sub


    '*********************************************************************
    '_______________________________METODOS_______________________________

    '*********************************************************************
    'Despliega los nombres de los COM`s disponibles para seleccionarlo
    Sub GetSerialPortNames()
        For Each sp As String In My.Computer.Ports.SerialPortNames
            ComboBoxPuertos.Items.Add(sp)
        Next
    End Sub

    Private Sub ComboBoxPuertos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxPuertos.SelectedIndexChanged
        'Al seleccionar el puerto se guarda en una variable para ceneccion posterior
        Form_Principal.puerto = ComboBoxPuertos.Text
    End Sub

    Private Sub ButtonConectarBarcode_Click(sender As Object, e As EventArgs) Handles ButtonConectarBarcode.Click
        'Si la variable puerto no esta vacia inicia proceso de coneccion
        If Form_Principal.puerto IsNot Nothing Then
            Form_Principal.com = New SerialPort(Form_Principal.puerto)

            Try
                With Form_Principal.com
                    If .IsOpen Then
                        .Close()
                    End If

                    .BaudRate = 115200
                    .DataBits = 8
                    .Parity = IO.Ports.Parity.None
                    .StopBits = IO.Ports.StopBits.One
                    .ReadTimeout = 5000
                    .Open()
                End With

                MsgBox("Conexion Exitosa con el puerto: " + Form_Principal.puerto.ToString, MsgBoxStyle.Information, "Cemex")
                Form_Principal.EstadoConnectPuerto = True

                Me.Close()

                Form_Principal.ToolStripStatusLabel1.Text = "COM - Port = " + Form_Principal.puerto.ToString

            Catch ex As Exception
                MsgBox("Error al abrir el puerto serial: " & ex.Message, MsgBoxStyle.Critical, "Cemex")
                Form_Principal.EstadoConnectPuerto = False

                Me.Close()
            End Try
        Else
            MessageBox.Show("Primero seleccione el puerto al cual desea conectarse.", "Error!!!", MessageBoxButtons.OK)
            Form_Principal.EstadoConnectPuerto = False
        End If
    End Sub

    Private Sub ButtonButtonCancelPort_Click(sender As Object, e As EventArgs) Handles ButtonCancelPort.Click
        Form_Principal.puerto = Nothing
        Me.Close()
        Form_Principal.ToolStripStatusLabel1.Text = "COM - Port = Undefined"
    End Sub

    Private Sub Form_Port_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.Closing
        Form_Principal.ToolStripStatusLabel1.Text = "COM - Port = Undefined"
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub
End Class