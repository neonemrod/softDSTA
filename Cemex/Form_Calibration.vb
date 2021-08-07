Public Class Form_Calibration
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        text_m1.Text = My.Settings.m_1.ToString
        text_m2.Text = My.Settings.m_2.ToString
        text_m3.Text = My.Settings.m_3.ToString
        text_m4.Text = My.Settings.m_4.ToString
        text_m5.Text = My.Settings.m_5.ToString
        text_m6.Text = My.Settings.m_6.ToString
        text_m7.Text = My.Settings.m_7.ToString
        text_m8.Text = My.Settings.m_8.ToString
        text_m9.Text = My.Settings.m_9.ToString
        text_m10.Text = My.Settings.m_10.ToString
        text_m11.Text = My.Settings.m_11.ToString
        text_m12.Text = My.Settings.m_12.ToString

        text_b1.Text = My.Settings.b_1.ToString
        text_b2.Text = My.Settings.b_2.ToString
        text_b3.Text = My.Settings.b_3.ToString
        text_b4.Text = My.Settings.b_4.ToString
        text_b5.Text = My.Settings.b_5.ToString
        text_b6.Text = My.Settings.b_6.ToString
        text_b7.Text = My.Settings.b_7.ToString
        text_b8.Text = My.Settings.b_8.ToString
        text_b9.Text = My.Settings.b_9.ToString
        text_b10.Text = My.Settings.b_10.ToString
        text_b11.Text = My.Settings.b_11.ToString
        text_b12.Text = My.Settings.b_12.ToString
    End Sub

    Private Sub ButtonDefecto_Click(sender As Object, e As EventArgs) Handles ButtonDefecto.Click
        My.Settings.Reset()

        text_m1.Text = My.Settings.m_1.ToString
        text_m2.Text = My.Settings.m_2.ToString
        text_m3.Text = My.Settings.m_3.ToString
        text_m4.Text = My.Settings.m_4.ToString
        text_m5.Text = My.Settings.m_5.ToString
        text_m6.Text = My.Settings.m_6.ToString
        text_m7.Text = My.Settings.m_7.ToString
        text_m8.Text = My.Settings.m_8.ToString
        text_m9.Text = My.Settings.m_9.ToString
        text_m10.Text = My.Settings.m_10.ToString
        text_m11.Text = My.Settings.m_11.ToString
        text_m12.Text = My.Settings.m_12.ToString

        text_b1.Text = My.Settings.b_1.ToString
        text_b2.Text = My.Settings.b_2.ToString
        text_b3.Text = My.Settings.b_3.ToString
        text_b4.Text = My.Settings.b_4.ToString
        text_b5.Text = My.Settings.b_5.ToString
        text_b6.Text = My.Settings.b_6.ToString
        text_b7.Text = My.Settings.b_7.ToString
        text_b8.Text = My.Settings.b_8.ToString
        text_b9.Text = My.Settings.b_9.ToString
        text_b10.Text = My.Settings.b_10.ToString
        text_b11.Text = My.Settings.b_11.ToString
        text_b12.Text = My.Settings.b_12.ToString

    End Sub

    Private Sub ButtonGuardar_Click(sender As Object, e As EventArgs) Handles ButtonGuardar.Click
        My.Settings.m_1 = text_m1.Text.ToString
        My.Settings.m_2 = text_m2.Text.ToString
        My.Settings.m_3 = text_m3.Text.ToString
        My.Settings.m_4 = text_m4.Text.ToString
        My.Settings.m_5 = text_m5.Text.ToString
        My.Settings.m_6 = text_m6.Text.ToString
        My.Settings.m_7 = text_m7.Text.ToString
        My.Settings.m_8 = text_m8.Text.ToString
        My.Settings.m_9 = text_m9.Text.ToString
        My.Settings.m_10 = text_m10.Text.ToString
        My.Settings.m_11 = text_m11.Text.ToString
        My.Settings.m_12 = text_m12.Text.ToString

        My.Settings.b_1 = text_b1.Text.ToString
        My.Settings.b_2 = text_b2.Text.ToString
        My.Settings.b_3 = text_b3.Text.ToString
        My.Settings.b_4 = text_b4.Text.ToString
        My.Settings.b_5 = text_b5.Text.ToString
        My.Settings.b_6 = text_b6.Text.ToString
        My.Settings.b_7 = text_b7.Text.ToString
        My.Settings.b_8 = text_b8.Text.ToString
        My.Settings.b_9 = text_b9.Text.ToString
        My.Settings.b_10 = text_b10.Text.ToString
        My.Settings.b_11 = text_b11.Text.ToString
        My.Settings.b_12 = text_b12.Text.ToString

        Me.Close()
        Application.Restart()
    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        Me.Close()
        Application.Restart()
    End Sub
End Class