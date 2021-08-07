Public Class Form_NameFile
    Private Sub ok_Click(sender As Object, e As EventArgs) Handles ok.Click
        If Not TextBox1.Text.Equals(String.Empty) Or Not TextBox1.Text.Equals("") Then
            Form_Principal.ArchivoProyecto = TextBox1.Text
            Me.Close()
        Else
            MsgBox("invalid name", MsgBoxStyle.Critical, "Danger!!!")
        End If

    End Sub

    Private Sub Form_NameFile_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.Closing
        If Form_Principal.ArchivoProyecto.Equals(String.Empty) Or Form_Principal.ArchivoProyecto.Equals("") Then
            MsgBox("enter a valid name and press ok", MsgBoxStyle.Critical, "Danger!!!")
            Dim name As New Form_NameFile
            name.Show()
        End If

    End Sub


End Class