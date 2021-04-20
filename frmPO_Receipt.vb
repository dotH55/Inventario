Imports System.Collections.Generic

Public Class frmPO_Receipt

    '' Previous Item Added
    Private ITEM_LAST_ADDED As String
    '' Value Holders
    Private ITEM_NUMBER As String
    Private FULL_ITEM_NUMBER As String
    '' Load DataTable
    Private DATA_TABLE As DataTable
    '' Barcodes
    Private BARCODES_DICT As Dictionary(Of String, String)
    '' What's in the Warehouse
    Private ACTUAL_DICT As New Dictionary(Of String, Integer)
    '' What's in the Database
    Private EXPECTED_DICT As New Dictionary(Of String, Integer)
    '' Binding 
    Private BindingSourceBerechnung As New BindingSource()
    '' Sound wav path
    Private Const ERROR_SOUND_PATH As String = "H:\\AUGUSTA.net\\PackingSlip_Scans_Program\\Incorrect_Entry_Sound.wav"
    '' AutoSizeColumn
    Private AutoSizeCol As Boolean

    '' Static VAR
    Private ROW_BEFORE As DataGridViewRow
    Private ROW_AFTER As DataGridViewRow

    '' Mouse
    Private MOUSE_MOVE As System.Drawing.Point

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        '' Initialize Variables
        Initialize_Variables()

        Alert_Tb.Text = "How To:" + Environment.NewLine + "1. Enter PO_# to proceed" + Environment.NewLine + "2. Start Scanning" + Environment.NewLine + "3. Update Augusta"
        Receipt_Number_Tb.Text = "PO Number"
    End Sub

    '' This method set focus on the Item_Number_Tb
    Private Sub Reset_Focus()
        '' Set Item_Number_Tb in focus
        If Item_Number_Tb.Text.Length > 0 Then
            Item_Number_Tb.Clear()
            Item_Number_Tb.Focus()
        End If
    End Sub

    '' GUI Helpers
    Private Sub Search_Enable()
        Receipt_Number_Tb.Text = ""
        Receipt_Number_Tb.Enabled = True
        Load_Bt.Enabled = True
        Submit_Bt.BackColor = Color.Transparent
        Cancel_Bt.Enabled = False
        DATA_TABLE = New DataTable
        Me.BindingSourceBerechnung.DataSource = DATA_TABLE
        Me.Data_Grid.DataSource = Me.BindingSourceBerechnung
    End Sub

    '' GUI Helpers
    Private Sub Search_Disable()
        Receipt_Number_Tb.Enabled = False
        Load_Bt.Enabled = False
        Submit_Bt.BackColor = Color.Black
        Cancel_Bt.Enabled = True
    End Sub

    '' This function initializes global variables
    Private Sub Initialize_Variables()
        ITEM_LAST_ADDED = ""
        Alert_Tb.Text = ""
        AutoSizeCol = False
        BARCODES_DICT = New Dictionary(Of String, String)
        ACTUAL_DICT = New Dictionary(Of String, Integer)
        EXPECTED_DICT = New Dictionary(Of String, Integer)
        ' Add any initialization after the InitializeComponent() call.
        Data_Grid.ScrollBars = ScrollBars.Both
        Data_Grid.Refresh()
    End Sub

    Private Sub Cancel_Helper()
        Search_Enable()
        Alert_Tb.Text = "Enter Warehouse Area to proceed..."
        Count_Lb.Text = "0"
        Submit_Bt.BackColor = Color.Transparent
    End Sub

    Private Function Check_Item_Number(x As Integer) As Boolean

        Check_Item_Number_Inventario(x, FULL_ITEM_NUMBER, ITEM_NUMBER, BARCODES_DICT)
    End Function

    '' This function turns a row
    '' Black:      if Expected Qty matches Scanned Qty
    '' LightCoral: if row hasn't been acknowledged
    '' Default (ForeColor: Black, BackColor: White)
    '' It also Updates the count 
    Private Sub Check_Cells()
        Check_Cells_Inventario(Data_Grid, ITEM_LAST_ADDED)
        Count_Lb.Text = CStr(Data_Grid.Rows.Count)
    End Sub

    Private Sub Alert_Tb_MouseClick(sender As Object, e As MouseEventArgs)
        Alert_Tb.Text = ""
    End Sub

    Private Sub Load_Bt_Click(sender As Object, e As EventArgs) Handles Load_Bt.Click
        Load()
    End Sub

    Private Sub Load()
        Alert_Tb.Text = ""

        '' Validate Input
        If Receipt_Number_Tb.Text = "" Or Not IsNumeric(Receipt_Number_Tb.Text) Then
            Alert_Tb.Text = "Please pass only the number without ATH OR AUG"
            Exit Sub 'GoTo EventExitSub
        End If

        '' Reset Variables
        Initialize_Variables()

        '' SQL
        Dim sqlstring As String = "EXEC [Inventory].[usp_PO_Read_w_Scanner] @pPONumber = '" + Receipt_Number_Tb.Text + "', @pLocation = '" + glbLocation + "'"
        DATA_TABLE = SQLGetDataTable(sqlstring, cnSQL) ' SQLExecuteNonQuery(sqlstring, cnSQL)

        If DATA_TABLE.Rows.Count = 0 Then
            '' Empty Query
            Alert_Tb.Text = "Empty Query!"
            Exit Sub
        End If

        '' Enable editing
        DATA_TABLE.Columns.Add("Binary")
        DATA_TABLE.Columns("ItemNumber").ReadOnly = True
        DATA_TABLE.Columns("Description").ReadOnly = True
        DATA_TABLE.Columns("Barcodes").ReadOnly = False
        DATA_TABLE.Columns("Scanned_Qty").ReadOnly = False
        ''Load_Data_Table.Columns("Binary").ReadOnly = True

        '' Bind data table with dataGridView
        Me.BindingSourceBerechnung.DataSource = DATA_TABLE
        Me.Data_Grid.DataSource = Me.BindingSourceBerechnung

        '' Adjust Width
        With Data_Grid
            .Columns(0).Width = 200 '' Item_number
            ''.Columns(0).ReadOnly = True

            .Columns(1).Width = 50 '' Expected_Qty
            ''.Columns(1).ReadOnly = True

            .Columns(2).Width = 50 '' Scanned_Qty
            ''.Columns(2).ReadOnly = False

            .Columns(3).Width = 125 '' Area
            ''.Columns(3).ReadOnly = False

            .Columns(4).Width = 125 '' Bin
            ''.Columns(4).ReadOnly = False

            .Columns(5).Width = 300 '' Description
            ''.Columns(5).ReadOnly = True

            .Columns(6).Width = 250 '' Barcode 250
            ''.Columns(6).HeaderCell.Value = "Barcode List"

            .Columns(7).Width = 10 '' Binary
            .Refresh()
        End With

        Init_Data_Inventario(Data_Grid, Alert_Tb, BARCODES_DICT, EXPECTED_DICT)
        Search_Disable()
        Check_Cells()
        '' Set Focus
        Item_Number_Tb.Focus()
        Data_Grid.Columns("Barcodes").DisplayIndex = 1
        Data_Grid.Columns("Description").DisplayIndex = 4
        Data_Grid.Columns("Scanned_Qty").DisplayIndex = 2
        Data_Grid.Columns("Binary").Visible = False
    End Sub

    Private Sub Cancel_Bt_Click(sender As Object, e As EventArgs) Handles Cancel_Bt.Click
        Cancel_Helper()
    End Sub

    Private Sub Item_Number_Tb_Validated(sender As Object, e As EventArgs) Handles Item_Number_Tb.Validated
        '' Validate Input
        If Item_Number_Tb.Text = "" Then
            Exit Sub 'GoTo EventExitSub
        End If

        '' Helper function
        ITEM_NUMBER = Item_Number_Tb.Text
        Item_Number_Tb_Validating_Inventario(sender, e, 2, FULL_ITEM_NUMBER, ITEM_LAST_ADDED, ITEM_NUMBER, Alert_Tb, Data_Grid, BARCODES_DICT, EXPECTED_DICT, ACTUAL_DICT)
        Reset_Focus()
    End Sub

    '' This function sets the Item_Last_Add's value
    '' to the value of Quantity_Tb, then redraws the Listview
    Private Sub OK_Bt_Click_1(sender As Object, e As EventArgs) Handles OK_Bt.Click
        '' August_Module
        Adjust_Quantity_Inventario(sender, e, Data_Grid, ACTUAL_DICT, ITEM_LAST_ADDED, Quantity_Tb, Alert_Tb, FULL_ITEM_NUMBER)
    End Sub

    Private Sub Copy_Bt_Click_1(sender As Object, e As EventArgs) Handles Copy_Bt.Click
        Copy_Barcode(sender, e, BARCODES_DICT, Data_Grid)
    End Sub

    Private Sub GetReport_Bt_Click(sender As Object, e As EventArgs) Handles GetReport_Bt.Click
        GetReport(sender, e, Data_Grid)
    End Sub

    Private Sub Fast_Process_Bt_Click(sender As Object, e As EventArgs) Handles Fast_Process_Bt.Click
        Fast_Process(sender, e, BARCODES_DICT, Data_Grid)
    End Sub

    Private Sub Submit_Bt_Click_1(sender As Object, e As EventArgs) Handles Submit_Bt.Click
        '' Make sure no cell is empty
        ''If isEmpty_Cells(sender, e, Data_Grid) Then
        ''  Dim isEmpty As DialogResult = MessageBox.Show("Area & Bin Cells Missing!", "Augusta", MessageBoxButtons.YesNo)
        ''  Exit Sub
        ''End If

        '' Alert
        Dim result As DialogResult = MessageBox.Show("This action will permanently change the Database. Are you certain?", "Augusta", MessageBoxButtons.YesNo)
        If result = vbNo Or Receipt_Number_Tb.Text.Equals("") Then
            Exit Sub
        End If

        Dim item_List As String = GetString_To_Submit(sender, e, Data_Grid)
        Dim sqlString As String = "EXEC [Inventory].[usp_PO_Write_w_Scanner] @pPONumber = '" + Receipt_Number_Tb.Text + "', @pItem_List = '" + item_List + "', @pMainWarehouse = '" + txtMainWhs + "', @pLocation = '" + glbLocation + "'"
        Submit_Inventario(sender, e, Data_Grid, sqlString, ITEM_LAST_ADDED)

        Print_Report(Receipt_Number_Tb.Text)

        If Data_Grid.Rows.Count = 0 Then
            Cancel_Helper()
        End If
        '' Print PO Receipt

    End Sub

    Private Sub Close_Form_Bt_Click(sender As Object, e As EventArgs) Handles Close_Form_Bt.Click
        Me.Close()
    End Sub

    Private Sub Minimize_Form_Bt_Click(sender As Object, e As EventArgs) Handles Minimize_Form_Bt.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Panel2_MouseDown(sender As Object, e As MouseEventArgs) Handles Panel2.MouseDown
        MOUSE_MOVE = New Point(-e.X, -e.Y)
    End Sub

    Private Sub Panel2_MouseMove(sender As Object, e As MouseEventArgs) Handles Panel2.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim mposition As Point
            mposition = Control.MousePosition
            mposition.Offset(MOUSE_MOVE.X, MOUSE_MOVE.Y)
            Me.Location = mposition
        End If
    End Sub

    Private Sub Data_Grid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles Data_Grid.CellContentClick
        Check_Cells()
    End Sub

    Private Sub Data_Grid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles Data_Grid.CellClick
        Check_Cells()
    End Sub

    Private Sub Data_Grid_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles Data_Grid.CellEndEdit
        Cell_Acknowledgement(sender, Data_Grid.CurrentRow.Index, Data_Grid.CurrentCell.ColumnIndex, Data_Grid)
    End Sub

    '' This functions acknowledges a row
    '' A row is acknowledged if user changes the Scanned_Qty
    '' by either cliking on the cell or Scanning the Item's barcode
    Public Sub Cell_Acknowledgement(sender As Object, ByRef index As Integer, ByRef columnIndex As Integer, ByRef Data_Grid As DataGridView)
        For Each row As DataGridViewRow In Data_Grid.Rows
            If Not row.Cells(0).Value Is Nothing Then
                If row.Index = index And columnIndex = 2 Then
                    row.Cells(7).Value = 1
                    Try
                        If ACTUAL_DICT.ContainsKey(row.Cells(0).Value) Then
                            ACTUAL_DICT(row.Cells(0).Value) = CInt(row.Cells(2).Value)
                        End If
                    Catch ex As Exception
                        Alert_Tb.Text = ex.StackTrace
                    End Try
                End If

                If row.Index = index And columnIndex = 6 Then
                    If Not row.Cells(6).Value.ToString().Equals("") Then
                        Item_Number_Tb_Validating_Inventario_(sender, 1, FULL_ITEM_NUMBER, ITEM_LAST_ADDED, row.Cells(0).Value.ToString(), Alert_Tb, Data_Grid, BARCODES_DICT, EXPECTED_DICT, ACTUAL_DICT)
                        Item_Number_Tb.Clear()
                        Item_Number_Tb.Focus()
                    End If
                    Exit Sub
                End If
            End If
        Next
    End Sub

    '' This method displays an Help_Page
    Private Sub Help_Bt_Click(sender As Object, e As EventArgs) Handles Help_Bt.Click
        ShowWindow(frmHelp_Page)
    End Sub

    Private Sub Receipt_Number_Tb_GotFocus(sender As Object, e As EventArgs) Handles Receipt_Number_Tb.GotFocus
        If Receipt_Number_Tb.Text.Equals("PO Number") Then
            Receipt_Number_Tb.Text = ""
        End If
    End Sub

    Private Sub Receipt_Number_Tb_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Receipt_Number_Tb.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Enter) Then
            ''Console.WriteLine("Enter Pressed")
            Load()
        End If
    End Sub

    Private Sub Auto_Resize_Click(sender As Object, e As EventArgs) Handles Auto_Resize_Bt.Click
        If Data_Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells Then
            AutoResize_None()
        Else
            AutoResize_AllCells()
        End If
    End Sub

    Private Sub AutoResize_AllCells()
        Data_Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Auto_Resize_Bt.BackColor = Color.White
        Auto_Resize_Bt.ForeColor = Color.Black
        Auto_Resize_Bt.FlatAppearance.MouseOverBackColor = Color.White
        Auto_Resize_Bt.Text = "Resize"
    End Sub

    Private Sub AutoResize_None()
        Data_Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        Auto_Resize_Bt.BackColor = Color.Black
        Auto_Resize_Bt.ForeColor = Color.White
        Auto_Resize_Bt.FlatAppearance.MouseOverBackColor = Color.Black
        Auto_Resize_Bt.Text = "None"
    End Sub
End Class