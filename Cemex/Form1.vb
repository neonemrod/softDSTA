'Option Strict On

Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Me.Location = New Point(CInt((Screen.PrimaryScreen.WorkingArea.Width / 2) - (Me.Width / 2)), CInt((Screen.PrimaryScreen.WorkingArea.Height / 2) - (Me.Height / 2)))
        Chart1.ChartAreas(0).AxisX.Interval = 10
        Chart1.ChartAreas(0).AxisY.Interval = 5
        Chart1.ChartAreas(0).AxisX.Minimum = 0
        Chart1.ChartAreas(0).AxisX.LabelStyle.Angle = 90
        'Chart1.ChartAreas(0).AxisX.IntervalType = DateTimeIntervalType.Hours
        'Chart1.ChartAreas(0).AxisY.Interval = 1
        Chart1.ChartAreas(0).AxisY.Minimum = 0
        'Chart1.ChartAreas(0).AxisX.ScaleView.Size = 100
        'Chart1.ChartAreas(0).AxisX.ScrollBar.Size = 20
        'Chart1.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
        Chart1.Series(0).IsVisibleInLegend = False
        'Chart1.ChartAreas(0).AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
        'Chart1.ChartAreas(0).AxisX.ScrollBar.IsPositionedInside = True
        'Chart1.ChartAreas(0).AxisX.ScrollBar.BackColor = Color.LightGray
        'Chart1.ChartAreas(0).AxisX.ScrollBar.ButtonColor = Color.Gray
        Timer1.Interval = 250


        Dim Arraypromedio As New ArrayList

        For i = 0 To 24
            Arraypromedio.Add(New ArrayList)
        Next

        For j = 0 To 24
            Arraypromedio.Item(j).add(1)
            Arraypromedio.Item(j).add(2)
            Arraypromedio.Item(j).add(3)
            Arraypromedio.Item(j).add(4)
            Arraypromedio.Item(j).add(5)
        Next

        Dim promedio As Integer = suma(Arraypromedio.Item(0))




        Timer1.Interval = 250

    End Sub

    Function suma(entra As ArrayList)
        Dim sumaRetorno As Integer = 0
        For i = 0 To (entra.Count - 1)
            sumaRetorno += entra.Item(i)
        Next
        Return sumaRetorno
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Timer1.Enabled = False Then
            Timer1.Enabled = True
        Else
            Timer1.Enabled = False
        End If
    End Sub

    Dim Rand As New Random
    Dim Counter As Double = 1

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Chart1.Series(0).Points.AddXY(Counter, CDbl(Rand.Next(0, 20)))
        Chart1.ChartAreas(0).AxisX.CustomLabels.Add(Counter - 1, Counter + 1, Now.ToString("HH:mm:ss fff"))
        Counter += 1
        Label1.Text = Counter.ToString
        'If Chart1.ChartAreas(0).AxisX.Maximum > Chart1.ChartAreas(0).AxisX.ScaleView.Size Then
        '    Chart1.ChartAreas(0).AxisX.ScaleView.Scroll(Chart1.ChartAreas(0).AxisX.Maximum)
        'End If
    End Sub

End Class