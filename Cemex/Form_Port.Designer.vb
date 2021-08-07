<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Port
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_Port))
        Me.ButtonCancelPort = New System.Windows.Forms.Button()
        Me.ButtonConectarBarcode = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ComboBoxPuertos = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'ButtonCancelPort
        '
        Me.ButtonCancelPort.BackColor = System.Drawing.SystemColors.Window
        Me.ButtonCancelPort.Font = New System.Drawing.Font("Segoe UI Emoji", 9.75!, System.Drawing.FontStyle.Bold)
        Me.ButtonCancelPort.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.ButtonCancelPort.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonCancelPort.Location = New System.Drawing.Point(178, 120)
        Me.ButtonCancelPort.Name = "ButtonCancelPort"
        Me.ButtonCancelPort.Size = New System.Drawing.Size(102, 40)
        Me.ButtonCancelPort.TabIndex = 8
        Me.ButtonCancelPort.Text = "Cancelar"
        Me.ButtonCancelPort.UseVisualStyleBackColor = False
        '
        'ButtonConectarBarcode
        '
        Me.ButtonConectarBarcode.BackColor = System.Drawing.SystemColors.Window
        Me.ButtonConectarBarcode.Font = New System.Drawing.Font("Segoe UI Emoji", 9.75!, System.Drawing.FontStyle.Bold)
        Me.ButtonConectarBarcode.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.ButtonConectarBarcode.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonConectarBarcode.Location = New System.Drawing.Point(56, 120)
        Me.ButtonConectarBarcode.Name = "ButtonConectarBarcode"
        Me.ButtonConectarBarcode.Size = New System.Drawing.Size(102, 40)
        Me.ButtonConectarBarcode.TabIndex = 7
        Me.ButtonConectarBarcode.Text = "Conectar"
        Me.ButtonConectarBarcode.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(44, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(258, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Seleccione El puerto COM al cual desea conectarce."
        '
        'ComboBoxPuertos
        '
        Me.ComboBoxPuertos.FormattingEnabled = True
        Me.ComboBoxPuertos.Location = New System.Drawing.Point(108, 74)
        Me.ComboBoxPuertos.Name = "ComboBoxPuertos"
        Me.ComboBoxPuertos.Size = New System.Drawing.Size(121, 21)
        Me.ComboBoxPuertos.TabIndex = 5
        '
        'Form_Port
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(346, 209)
        Me.Controls.Add(Me.ButtonCancelPort)
        Me.Controls.Add(Me.ButtonConectarBarcode)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBoxPuertos)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form_Port"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cemex"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ButtonCancelPort As Button
    Friend WithEvents ButtonConectarBarcode As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents ComboBoxPuertos As ComboBox
End Class
