Public Class Form_Settings
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        '  Configuracion ComboBox Termocuplas
        cb_t1.Items.Add("Reader")
        cb_t1.Items.Add("Reference")
        cb_t1.Items.Add("None")
        cb_t1.SelectedIndex = cb_t1.FindStringExact(My.Settings.T_1)

        cb_t2.Items.Add("Reader")
        cb_t2.Items.Add("Reference")
        cb_t2.Items.Add("None")
        cb_t2.SelectedIndex = cb_t2.FindStringExact(My.Settings.T_2)

        cb_t3.Items.Add("Reader")
        cb_t3.Items.Add("Reference")
        cb_t3.Items.Add("None")
        cb_t3.SelectedIndex = cb_t3.FindStringExact(My.Settings.T_3)

        cb_t4.Items.Add("Reader")
        cb_t4.Items.Add("Reference")
        cb_t4.Items.Add("None")
        cb_t4.SelectedIndex = cb_t4.FindStringExact(My.Settings.T_4)

        cb_t5.Items.Add("Reader")
        cb_t5.Items.Add("Reference")
        cb_t5.Items.Add("None")
        cb_t5.SelectedIndex = cb_t5.FindStringExact(My.Settings.T_5)

        cb_t6.Items.Add("Reader")
        cb_t6.Items.Add("Reference")
        cb_t6.Items.Add("None")
        cb_t6.SelectedIndex = cb_t6.FindStringExact(My.Settings.T_6)

        cb_t7.Items.Add("Reader")
        cb_t7.Items.Add("Reference")
        cb_t7.Items.Add("None")
        cb_t7.SelectedIndex = cb_t7.FindStringExact(My.Settings.T_7)

        cb_t8.Items.Add("Reader")
        cb_t8.Items.Add("Reference")
        cb_t8.Items.Add("None")
        cb_t8.SelectedIndex = cb_t8.FindStringExact(My.Settings.T_8)

        cb_t9.Items.Add("Reader")
        cb_t9.Items.Add("Reference")
        cb_t9.Items.Add("None")
        cb_t9.SelectedIndex = cb_t9.FindStringExact(My.Settings.T_9)

        cb_t10.Items.Add("Reader")
        cb_t10.Items.Add("Reference")
        cb_t10.Items.Add("None")
        cb_t10.SelectedIndex = cb_t10.FindStringExact(My.Settings.T_10)

        cb_t11.Items.Add("Reader")
        cb_t11.Items.Add("Reference")
        cb_t11.Items.Add("None")
        cb_t11.SelectedIndex = cb_t11.FindStringExact(My.Settings.T_11)

        cb_t12.Items.Add("Reader")
        cb_t12.Items.Add("Reference")
        cb_t12.Items.Add("None")
        cb_t12.SelectedIndex = cb_t12.FindStringExact(My.Settings.T_12)

        cb_c1.Items.Add("None")
        cb_c2.Items.Add("None")
        cb_c3.Items.Add("None")
        cb_c4.Items.Add("None")
        cb_c5.Items.Add("None")
        cb_c6.Items.Add("None")
        cb_c7.Items.Add("None")
        cb_c8.Items.Add("None")
        cb_c9.Items.Add("None")
        cb_c10.Items.Add("None")
        cb_c11.Items.Add("None")
        cb_c12.Items.Add("None")

        cb_c1.SelectedIndex = cb_c1.FindStringExact(My.Settings.C_1)
        cb_c2.SelectedIndex = cb_c2.FindStringExact(My.Settings.C_2)
        cb_c3.SelectedIndex = cb_c3.FindStringExact(My.Settings.C_3)
        cb_c4.SelectedIndex = cb_c4.FindStringExact(My.Settings.C_4)
        cb_c5.SelectedIndex = cb_c5.FindStringExact(My.Settings.C_5)
        cb_c6.SelectedIndex = cb_c6.FindStringExact(My.Settings.C_6)
        cb_c7.SelectedIndex = cb_c7.FindStringExact(My.Settings.C_7)
        cb_c8.SelectedIndex = cb_c8.FindStringExact(My.Settings.C_8)
        cb_c9.SelectedIndex = cb_c9.FindStringExact(My.Settings.C_9)
        cb_c10.SelectedIndex = cb_c10.FindStringExact(My.Settings.C_10)
        cb_c11.SelectedIndex = cb_c11.FindStringExact(My.Settings.C_11)
        cb_c12.SelectedIndex = cb_c12.FindStringExact(My.Settings.C_12)



    End Sub

    Private Sub ButtonGuardar_Click(sender As Object, e As EventArgs) Handles ButtonGuardar.Click
        My.Settings.T_1 = cb_t1.Text.ToString
        My.Settings.T_2 = cb_t2.Text.ToString
        My.Settings.T_3 = cb_t3.Text.ToString
        My.Settings.T_4 = cb_t4.Text.ToString
        My.Settings.T_5 = cb_t5.Text.ToString
        My.Settings.T_6 = cb_t6.Text.ToString
        My.Settings.T_7 = cb_t7.Text.ToString
        My.Settings.T_8 = cb_t8.Text.ToString
        My.Settings.T_9 = cb_t9.Text.ToString
        My.Settings.T_10 = cb_t10.Text.ToString
        My.Settings.T_11 = cb_t11.Text.ToString
        My.Settings.T_12 = cb_t12.Text.ToString

        My.Settings.C_1 = cb_c1.Text.ToString
        My.Settings.C_2 = cb_c2.Text.ToString
        My.Settings.C_3 = cb_c3.Text.ToString
        My.Settings.C_4 = cb_c4.Text.ToString
        My.Settings.C_5 = cb_c5.Text.ToString
        My.Settings.C_6 = cb_c6.Text.ToString
        My.Settings.C_7 = cb_c7.Text.ToString
        My.Settings.C_8 = cb_c8.Text.ToString
        My.Settings.C_9 = cb_c9.Text.ToString
        My.Settings.C_10 = cb_c10.Text.ToString
        My.Settings.C_11 = cb_c11.Text.ToString
        My.Settings.C_12 = cb_c12.Text.ToString

        Me.Close()
        Application.Restart()

    End Sub

    Private Sub ButtonDefecto_Click(sender As Object, e As EventArgs) Handles ButtonDefecto.Click
        My.Settings.Reset()

        cb_t1.SelectedIndex = cb_t1.FindStringExact(My.Settings.T_1)
        cb_t2.SelectedIndex = cb_t1.FindStringExact(My.Settings.T_1)
        cb_t3.SelectedIndex = cb_t1.FindStringExact(My.Settings.T_1)
        cb_t4.SelectedIndex = cb_t1.FindStringExact(My.Settings.T_1)
        cb_t5.SelectedIndex = cb_t1.FindStringExact(My.Settings.T_1)
        cb_t6.SelectedIndex = cb_t1.FindStringExact(My.Settings.T_1)
        cb_t7.SelectedIndex = cb_t1.FindStringExact(My.Settings.T_1)
        cb_t8.SelectedIndex = cb_t1.FindStringExact(My.Settings.T_1)
        cb_t9.SelectedIndex = cb_t1.FindStringExact(My.Settings.T_1)
        cb_t10.SelectedIndex = cb_t1.FindStringExact(My.Settings.T_1)
        cb_t11.SelectedIndex = cb_t1.FindStringExact(My.Settings.T_1)
        cb_t12.SelectedIndex = cb_t1.FindStringExact(My.Settings.T_1)

        cb_c1.Items.Clear()
        cb_c2.Items.Clear()
        cb_c3.Items.Clear()
        cb_c4.Items.Clear()
        cb_c5.Items.Clear()
        cb_c6.Items.Clear()
        cb_c7.Items.Clear()
        cb_c8.Items.Clear()
        cb_c9.Items.Clear()
        cb_c10.Items.Clear()
        cb_c11.Items.Clear()
        cb_c12.Items.Clear()

        cb_c1.Items.Add(My.Settings.C_1)
        cb_c2.Items.Add(My.Settings.C_2)
        cb_c3.Items.Add(My.Settings.C_3)
        cb_c4.Items.Add(My.Settings.C_4)
        cb_c5.Items.Add(My.Settings.C_5)
        cb_c6.Items.Add(My.Settings.C_6)
        cb_c7.Items.Add(My.Settings.C_7)
        cb_c8.Items.Add(My.Settings.C_8)
        cb_c9.Items.Add(My.Settings.C_9)
        cb_c10.Items.Add(My.Settings.C_10)
        cb_c11.Items.Add(My.Settings.C_11)
        cb_c12.Items.Add(My.Settings.C_12)

        cb_c1.SelectedIndex = cb_c1.FindStringExact(My.Settings.C_1)
        cb_c2.SelectedIndex = cb_c2.FindStringExact(My.Settings.C_2)
        cb_c3.SelectedIndex = cb_c3.FindStringExact(My.Settings.C_3)
        cb_c4.SelectedIndex = cb_c4.FindStringExact(My.Settings.C_4)
        cb_c5.SelectedIndex = cb_c5.FindStringExact(My.Settings.C_5)
        cb_c6.SelectedIndex = cb_c6.FindStringExact(My.Settings.C_6)
        cb_c7.SelectedIndex = cb_c7.FindStringExact(My.Settings.C_7)
        cb_c8.SelectedIndex = cb_c8.FindStringExact(My.Settings.C_8)
        cb_c9.SelectedIndex = cb_c9.FindStringExact(My.Settings.C_9)
        cb_c10.SelectedIndex = cb_c10.FindStringExact(My.Settings.C_10)
        cb_c11.SelectedIndex = cb_c11.FindStringExact(My.Settings.C_11)
        cb_c12.SelectedIndex = cb_c12.FindStringExact(My.Settings.C_12)


    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        Me.Close()
        Application.Restart()
    End Sub

    Private Sub cb_t1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_t1.SelectedIndexChanged
        If cb_t1.Text.ToString.Equals("Reference") Then
            MetodoListas(1)
        Else
            MetodoQuitarListas(1)
        End If
    End Sub

    Private Sub cb_t2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_t2.SelectedIndexChanged
        If cb_t2.Text.ToString.Equals("Reference") Then
            MetodoListas(2)
        Else
            MetodoQuitarListas(2)
        End If
    End Sub

    Private Sub cb_t3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_t3.SelectedIndexChanged
        If cb_t3.Text.ToString.Equals("Reference") Then
            MetodoListas(3)
        Else
            MetodoQuitarListas(3)
        End If
    End Sub

    Private Sub cb_t4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_t4.SelectedIndexChanged
        If cb_t4.Text.ToString.Equals("Reference") Then
            MetodoListas(4)
        Else
            MetodoQuitarListas(4)
        End If
    End Sub

    Private Sub cb_t5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_t5.SelectedIndexChanged
        If cb_t5.Text.ToString.Equals("Reference") Then
            MetodoListas(5)
        Else
            MetodoQuitarListas(5)
        End If
    End Sub

    Private Sub cb_t6_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_t6.SelectedIndexChanged
        If cb_t6.Text.ToString.Equals("Reference") Then
            MetodoListas(6)
        Else
            MetodoQuitarListas(6)
        End If
    End Sub

    Private Sub cb_t7_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_t7.SelectedIndexChanged
        If cb_t7.Text.ToString.Equals("Reference") Then
            MetodoListas(7)
        Else
            MetodoQuitarListas(7)
        End If
    End Sub

    Private Sub cb_t8_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_t8.SelectedIndexChanged
        If cb_t8.Text.ToString.Equals("Reference") Then
            MetodoListas(8)
        Else
            MetodoQuitarListas(8)
        End If
    End Sub

    Private Sub cb_t9_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_t9.SelectedIndexChanged
        If cb_t9.Text.ToString.Equals("Reference") Then
            MetodoListas(9)
        Else
            MetodoQuitarListas(9)
        End If
    End Sub

    Private Sub cb_t10_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_t10.SelectedIndexChanged
        If cb_t10.Text.ToString.Equals("Reference") Then
            MetodoListas(10)
        Else
            MetodoQuitarListas(10)
        End If
    End Sub

    Private Sub cb_t11_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_t11.SelectedIndexChanged
        If cb_t11.Text.ToString.Equals("Reference") Then
            MetodoListas(11)
        Else
            MetodoQuitarListas(11)
        End If
    End Sub

    Private Sub cb_t12_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_t12.SelectedIndexChanged
        If cb_t12.Text.ToString.Equals("Reference") Then
            MetodoListas(12)
        Else
            MetodoQuitarListas(12)
        End If
    End Sub

    Sub MetodoListas(num As Integer)
        Dim cadena As String = "T - " + num.ToString
        cb_c1.Items.Add(cadena)
        cb_c2.Items.Add(cadena)
        cb_c3.Items.Add(cadena)
        cb_c4.Items.Add(cadena)
        cb_c5.Items.Add(cadena)
        cb_c6.Items.Add(cadena)
        cb_c7.Items.Add(cadena)
        cb_c8.Items.Add(cadena)
        cb_c9.Items.Add(cadena)
        cb_c10.Items.Add(cadena)
        cb_c11.Items.Add(cadena)
        cb_c12.Items.Add(cadena)
    End Sub

    Sub MetodoQuitarListas(num As Integer)
        Dim cadena As String = "T - " + num.ToString
        cb_c1.Items.Remove(cadena)
        cb_c2.Items.Remove(cadena)
        cb_c3.Items.Remove(cadena)
        cb_c4.Items.Remove(cadena)
        cb_c5.Items.Remove(cadena)
        cb_c6.Items.Remove(cadena)
        cb_c7.Items.Remove(cadena)
        cb_c8.Items.Remove(cadena)
        cb_c9.Items.Remove(cadena)
        cb_c10.Items.Remove(cadena)
        cb_c11.Items.Remove(cadena)
        cb_c12.Items.Remove(cadena)
    End Sub
End Class