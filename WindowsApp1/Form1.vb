Imports MySql.Data
Imports MySql.Data.MySqlClient

Public Class Form1
    Dim conn As New MySqlConnection
    Dim cmd As New MySqlCommand()
    Dim bt_status As Boolean = False

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ConnectVB()
        '  dgvCaseLst1.Rows(Pi_CaseIndex).Selected = True
        ReadDatabase()
        cleartxt()
        Button1.Enabled = True
    End Sub

    Private Sub save_editDB()
        Dim str = TextBox1.Text + TextBox2.Text + TextBox3.Text + TextBox4.Text + TextBox5.Text + TextBox6.Text + TextBox7.Text
        'MessageBox.Show(str)

        Label6.Text = str

        ' ==============================


        cmd.Connection = conn

        Dim sql As String = ""

        Try
            If bt_status = True Then
                sql = "INSERT INTO fw_master_cus(FMC_CUSID, FMC_CUSNAME, FMC_CUSPHONE,FMC_TAXID, FMC_EMAIL, FMC_CUSADR, FMC_CUSCNUM, FMC_CUSCHOL,FMC_MODDATE, FMC_MODTIME) 
                           VALUES(uuid(), @FMC_CUSNAME, @FMC_CUSPHONE, @FMC_TAXID, @FMC_EMAIL, @FMC_CUSADR, @FMC_CUSCNUM, @FMC_CUSCHOL, CURDATE(), CURTIME())"
            Else

                If CheckDatabase(TextBox8.Text.ToString.Trim) = False Then
                    MessageBox.Show("ข้อมูลไม่สามารถบันทึกได้ เนื่องจากเกิดข้อผิดพลาด")
                    Exit Sub
                End If

                sql = "UPDATE fw_master_cus SET FMC_CUSNAME = @FMC_CUSNAME ,FMC_CUSPHONE =@FMC_CUSPHONE  ,
                        FMC_TAXID = @FMC_TAXID ,FMC_EMAIL =@FMC_EMAIL ,FMC_CUSADR =@FMC_CUSADR ,FMC_CUSCHOL =@FMC_CUSCHOL
                        ,FMC_CUSCNUM = @FMC_CUSCNUM  WHERE FMC_CUSID =@FMC_CUSID; "


            End If

            cmd.CommandText = sql
            cmd.CommandType = CommandType.Text

            cmd.Prepare()
            cmd.Parameters.Clear()
            If bt_status = False Then
                cmd.Parameters.AddWithValue("@FMC_CUSID", TextBox8.Text.ToString.Trim())
            End If
            cmd.Parameters.AddWithValue("@FMC_CUSNAME", TextBox1.Text.ToString.Trim())
            cmd.Parameters.AddWithValue("@FMC_TAXID", TextBox2.Text.ToString.Trim())
            cmd.Parameters.AddWithValue("@FMC_EMAIL", TextBox3.Text.ToString.Trim())
            cmd.Parameters.AddWithValue("@FMC_CUSADR", TextBox4.Text.ToString.Trim())
            cmd.Parameters.AddWithValue("@FMC_CUSPHONE", TextBox5.Text.ToString.Trim())
            cmd.Parameters.AddWithValue("@FMC_CUSCHOL", TextBox6.Text.ToString.Trim())
            cmd.Parameters.AddWithValue("@FMC_CUSCNUM", TextBox7.Text.ToString.Trim())

            'Dim regDate As Date = Date.Now()
            'cmd.Parameters.AddWithValue("@FMC_MODDATE", regDate)
            'cmd.Parameters.AddWithValue("@FMC_MODTIME",  )


            cmd.ExecuteNonQuery()
            MessageBox.Show("บันทึกข้อมูลสำเร็จ", "Save")
        Catch ex As Exception
            MessageBox.Show("ข้อมูลไม่สามารถบันทึกได้ เนื่องจากเกิดข้อผิดพลาด")
        End Try
        ReadDatabase()
    End Sub

    Function CheckDatabase(str As String)

        Dim stm As String = "Select * FROM fw_master_cus order where FMC_CUSID='" + str + "'; "
        Dim cmd As MySqlCommand = New MySqlCommand(stm, conn)


        Dim da As New MySqlDataAdapter
        da.SelectCommand = cmd

        Dim table As New DataTable()
        da.Fill(table)
        Dim i As Int32 = table.Rows.Count
        If i > 0 Then
            Return True
        Else
            Return False
        End If

    End Function


    Private Sub ReadDatabase()

        '   Dim stm As String = "Select * FROM fw_master_cus order by FMC_MODDATE, FMC_MODTIME "
        Dim stm As String = "call Select_All_Master_Cus; "
        cmd = New MySqlCommand()
        With cmd


            .Connection = conn
            .CommandType = CommandType.StoredProcedure
            .CommandText = "Select_All_Master_Cus; "
            .ExecuteNonQuery()


        End With

        Dim da As New MySqlDataAdapter
        da.SelectCommand = cmd

        Dim table As New DataTable()
        da.Fill(table)

        DataGridView1.DataSource = table

        With DataGridView1
            '.Columns("FMC_CUSID").HeaderText = "ID"
            .Columns("FMC_CUSNAME").HeaderText = "Name"
            .Columns("FMC_CUSPHONE").HeaderText = "Telephone Number"
            .Columns("FMC_TAXID").HeaderText = "Tax ID"
            .Columns("FMC_EMAIL").HeaderText = "Email"
            .Columns("FMC_CUSADR").HeaderText = "Address"
            .Columns("FMC_CUSCNUM").HeaderText = "Card Number"
            .Columns("FMC_CUSCHOL").HeaderText = "Card Name"

            .Columns("FMC_MODDATE").HeaderText = "Modify Date"
            .Columns("FMC_MODTIME").HeaderText = "Modify Time"
            .Columns("FMC_MODEMP").HeaderText = "Modify Employee"

            .Columns("FMC_CUSID").Visible = False
        End With


    End Sub

    Private Sub ConnectVB()
        Dim myConnectionString As String

        myConnectionString = "server= 127.0.0.1;" _
                    & "uid=root;" _
                    & "pwd='';SslMode=none;" _
                    & "database=p_quality;"

        Try
            conn.ConnectionString = myConnectionString
            conn.Open()

        Catch ex As MySqlClient.MySqlException
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        'If (e.RowIndex >= 0) Then
        '    Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
        '    MessageBox.Show(row.Cells("FMC_CUSNAME").Value.ToString())

        'End If
        If bt_status = False Then
            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If



    End Sub

    Private Sub cleartxt()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = Nothing
        TextBox8.Text = ""

        Label10.Text = ""
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        If DataGridView1.SelectedCells.Count > 0 Then
            'อ่าน row
            Dim i As Integer = DataGridView1.SelectedCells(0).RowIndex
            'MessageBox.Show(i)
            bt_status = True
            Button1.Enabled = False
            TextBox1.Text = DataGridView1.Rows(i).Cells("FMC_CUSNAME").Value.ToString()
            TextBox2.Text = DataGridView1.Rows(i).Cells("FMC_TAXID").Value.ToString()
            TextBox3.Text = DataGridView1.Rows(i).Cells("FMC_EMAIL").Value.ToString()
            TextBox4.Text = DataGridView1.Rows(i).Cells("FMC_CUSADR").Value.ToString()
            TextBox5.Text = DataGridView1.Rows(i).Cells("FMC_CUSPHONE").Value.ToString()
            TextBox6.Text = DataGridView1.Rows(i).Cells("FMC_CUSCHOL").Value.ToString()
            TextBox7.Text = DataGridView1.Rows(i).Cells("FMC_CUSCNUM").Value.ToString()

            Label10.Text = DataGridView1.Rows(i).Cells("FMC_CUSID").Value.ToString()
            TextBox8.Text = DataGridView1.Rows(i).Cells("FMC_CUSID").Value.ToString()
            'Button1.Text = "อัฟเดตข้อมูล"
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        bt_status = True
        save_editDB()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        bt_status = False
        Button1.Enabled = True
        cleartxt()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        bt_status = False
        save_editDB()
    End Sub
End Class
