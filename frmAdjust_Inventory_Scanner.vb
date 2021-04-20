Imports System.Collections.Generic

Public Class frmAdjust_Inventory_Scanner

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


    '' Mouse
    Private MOUSE_MOVE As System.Drawing.Point


    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ActiveForm.FormBorderStyle = FormBorderStyle.None

        '' Initialize Variables
        Initialize_Variables()

        Alert_Tb.Text = "How To:" + Environment.NewLine + "1. Enter Warehouse Area to proceed" + Environment.NewLine + "2. Start Scanning" + Environment.NewLine + "3. Update Augusta"
        Warehouse_Tb.Text = "Warehouse Area"

        '' Set Mode: COUNT
        Mode_Count()
        AutoResize_AllCells()
    End Sub

    '' This function initializes global variables
    Private Sub Initialize_Variables()
        ITEM_LAST_ADDED = ""
        Alert_Tb.Text = ""
        ACTUAL_DICT = New Dictionary(Of String, Integer)
        EXPECTED_DICT = New Dictionary(Of String, Integer)
        BARCODES_DICT = New Dictionary(Of String, String)
        ' Add any initialization after the InitializeComponent() call.
        Data_Grid.ScrollBars = ScrollBars.Both
        Data_Grid.Refresh()
    End Sub

    '' This method set focus on the Item_Number_Tb
    Private Sub Reset_Focus()
        '' Set Item_Number_Tb in focus
        If Item_Number_Tb.Text.Length > 0 Then
            Item_Number_Tb.Clear()
            Item_Number_Tb.Focus()
        End If
    End Sub

    '' This function turns a row
    '' Black:      if Expected Qty matches Scanned Qty
    '' LightCoral: if row hasn't been acknowledged
    '' Default (ForeColor: Black, BackColor: White)
    '' It also Updates the count 
    Private Sub Check_Cells()
        Check_Cells_Inventario(Data_Grid, ITEM_LAST_ADDED)
        Count_Lb.Text = CStr(Data_Grid.Rows.Count)
    End Sub

    '' GUI Helpers
    '' When the Load button is Pressed
    Private Sub Search_Enable()
        Warehouse_Tb.Text = ""
        Warehouse_Tb.Enabled = True
        Load_Bt.Enabled = True
        Submit_Bt.BackColor = Color.Transparent
        Cancel_Bt.Enabled = False
        DATA_TABLE = New DataTable
        Me.BindingSourceBerechnung.DataSource = DATA_TABLE
        Me.Data_Grid.DataSource = Me.BindingSourceBerechnung
    End Sub

    '' GUI Helpers
    '' When the Cancel button is pressed
    Private Sub Search_Disable()
        Warehouse_Tb.Enabled = False
        Load_Bt.Enabled = False
        Submit_Bt.BackColor = Color.Black
        Cancel_Bt.Enabled = True
    End Sub

    Private Sub Cancel_Helper()
        Search_Enable()
        Alert_Tb.Text = "Enter Warehouse Area to proceed..."
        Count_Lb.Text = "0"
        Submit_Bt.BackColor = Color.Transparent
        Mode_Count()
    End Sub

    Private Sub Alert_Tb_MouseClick(sender As Object, e As MouseEventArgs)
        Alert_Tb.Text = ""
    End Sub

    Private Sub GroupBox3_MouseClick(sender As Object, e As MouseEventArgs)
        Alert_Tb.Text = ""
    End Sub

    Private Sub Close_Form_Bt_Click(sender As Object, e As EventArgs) Handles Close_Form_Bt.Click
        Me.Close()
    End Sub

    Private Sub Minimize_Form_Bt_Click(sender As Object, e As EventArgs) Handles Minimize_Form_Bt.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Panel1_MouseDown(sender As Object, e As MouseEventArgs) Handles Panel1.MouseDown
        MOUSE_MOVE = New Point(-e.X, -e.Y)
    End Sub

    Private Sub Panel1_MouseMove(sender As Object, e As MouseEventArgs) Handles Panel1.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim mposition As Point
            mposition = Control.MousePosition
            mposition.Offset(MOUSE_MOVE.X, MOUSE_MOVE.Y)
            Me.Location = mposition
        End If
    End Sub

    '' This function loads warehouse items into DataGridView
    Private Sub Load_Bt_Click_1(sender As Object, e As EventArgs) Handles Load_Bt.Click
        Load()
    End Sub

    Private Sub Load()
        Alert_Tb.Text = ""
        '' Validate Input
        If Warehouse_Tb.Text = "" Then
            Exit Sub 'GoTo EventExitSub
        End If

        '' Reset Variables
        Initialize_Variables()

        '' SQL Commands
        Dim sqlstring As String = "EXEC [Inventory].[usp_Adjust_Read_w_Scanner] @pArea = '" + UCase(Warehouse_Tb.Text) + "', @pMainWarehouse = '" + txtMainWhs + "',@pLocation = '" + glbLocation + "'"
        DATA_TABLE = SQLGetDataTable(sqlstring, cnSQL)

        If DATA_TABLE.Rows.Count = 0 Then
            '' Empty Query
            Alert_Tb.Text = "Empty Query!"
            Exit Sub
        End If

        '' Enable editing
        DATA_TABLE.Columns("ItemNumber").ReadOnly = True
        DATA_TABLE.Columns("Description").ReadOnly = True
        DATA_TABLE.Columns("Barcodes").ReadOnly = False
        DATA_TABLE.Columns("Scanned_Qty").ReadOnly = False
        DATA_TABLE.Columns.Add("Binary")
        ''Load_Data_Table.Columns("Binary").ReadOnly = True



        '' Bind data table with dataGridView
        Me.BindingSourceBerechnung.DataSource = DATA_TABLE
        Me.Data_Grid.DataSource = Me.BindingSourceBerechnung

        With Data_Grid
            .Columns(0).Width = 200 '' Item_number
            ''.Columns(0).ReadOnly = True

            .Columns(1).Width = 50 '' Expected_Qty
            ''.Columns(1).ReadOnly = True

            .Columns(2).Width = 50 '' Scanned_Qty
            ''.Columns(2).ReadOnly = False

            .Columns(3).Width = 150 '' Area
            ''.Columns(3).ReadOnly = False

            .Columns(4).Width = 150 '' Bin
            ''.Columns(4).ReadOnly = False

            .Columns(5).Width = 350 '' Description
            ''.Columns(5).ReadOnly = True

            .Columns(6).Width = 350 '' Barcode 250
            ''.Columns(6).HeaderCell.Value = "Barcode List"

            .Columns(7).Width = 1 '' Binary
            .Refresh()
        End With

        Init_Data_Inventario(Data_Grid, Alert_Tb, BARCODES_DICT, EXPECTED_DICT)
        Search_Disable()
        Check_Cells()
        Item_Number_Tb.Focus()
        Data_Grid.Columns("Barcodes").DisplayIndex = 1
        Data_Grid.Columns("Description").DisplayIndex = 4
        Data_Grid.Columns("Binary").Visible = False
        Data_Grid.Columns("Scanned_Qty").DisplayIndex = 2
    End Sub

    Private Sub Cancel_Bt_Click_1(sender As Object, e As EventArgs) Handles Cancel_Bt.Click
        Cancel_Helper()
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

    Private Sub Delete_Empty_Row_Click_1(sender As Object, e As EventArgs) Handles Delete_Empty_Row.Click
        DELETE_ROWS(sender, e, "2", Data_Grid)
    End Sub

    Private Sub Check_Bt_Click_1(sender As Object, e As EventArgs) Handles Check_Bt.Click
        GetReport(sender, e, Data_Grid)
    End Sub

    Private Sub Process_Bt_Click_1(sender As Object, e As EventArgs) Handles Process_Bt.Click
        Fast_Process(sender, e, BARCODES_DICT, Data_Grid)
    End Sub

    '' This function updates Augusta
    Private Sub Submit_Bt_Click(sender As Object, e As EventArgs) Handles Submit_Bt.Click
        '' Make sure no cell is empty
        ''If isEmpty_Cells(sender, e, Data_Grid) Then
        ''     Dim isEmpty As DialogResult = MessageBox.Show("Area & Bin Cells Missing!", "Augusta", MessageBoxButtons.YesNo)
        ''Exit Sub
        ''End If

        '' Alert
        Dim result As DialogResult = MessageBox.Show("This action will permanently change the Database. Are you certain?", "Augusta", MessageBoxButtons.YesNo)
        If result = vbNo Or Warehouse_Tb.Text.Equals("") Then
            Exit Sub
        End If

        Dim item_List As String = GetString_To_Submit(sender, e, Data_Grid)
        Dim sqlString As String = "EXEC [Inventory].[usp_Adjust_Write_w_Scanner] @pItem_List = '" + item_List + "', @pMainWarehouse = '" + txtMainWhs + "', @pLocation = '" + glbLocation + "'"
        Submit_Inventario(sender, e, Data_Grid, sqlString, ITEM_LAST_ADDED)
        If Data_Grid.Rows.Count = 0 Then
            Cancel_Helper()
        End If
    End Sub

    '' This function updates an Item's Qty, Area & Bin
    Private Sub Update_Area_Bin_Click_1(sender As Object, e As EventArgs) Handles Update_Area_Bin.Click
        '' Validate Input
        Dim result_1 As DialogResult = MessageBox.Show("This action will permanently change the Database. Are you certain?", "Augusta", MessageBoxButtons.YesNo)
        If result_1 = vbNo Or New_Item_Number.Text.Equals("") Or New_Item_Qty.Text.Equals("") Or New_Item_Area.Text.Equals("") Or New_Item_Bin_Tb.Text.Equals("") Then
            Exit Sub
        End If

        Alert_Tb.Text = ""
        ITEM_NUMBER = New_Item_Number.Text

        If Not Check_Item_Number_Inventario(2, FULL_ITEM_NUMBER, ITEM_NUMBER, BARCODES_DICT) Then
            Alert_Tb.Text = "Item Not Found"
            Exit Sub
        End If

        Try
            '' Make string list
            Dim item_List As String = ITEM_NUMBER + "," + New_Item_Qty.Text + "," + New_Item_Area.Text + "," + New_Item_Bin_Tb.Text + "," + Barcode_Tb.Text
            '' Verify Action
            Dim result_2 As DialogResult = MessageBox.Show("Are you certain?", "Augusta", MessageBoxButtons.YesNo)
            If result_2 = vbYes Then
                Dim sqlString As String = "EXEC [Inventory].[usp_Adjust_Write_w_Scanner] @pItem_List = '" + item_List + "', @pMainWarehouse = '" + txtMainWhs + "', @pLocation = '" + glbLocation + "'"
                SQLExecuteNonQuery(sqlString, cnSQL)
                '' Clear Entered Text
                New_Item_Number.Text = ""
                New_Item_Qty.Text = ""
                New_Item_Area.Text = ""
                New_Item_Bin_Tb.Text = ""
                Barcode_Tb.Text = ""
            End If
        Catch ex As Exception
            Alert_Tb.Text = ex.StackTrace
        End Try
    End Sub

    Private Sub Panel2_MouseMove(sender As Object, e As MouseEventArgs) Handles Panel2.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim mposition As Point
            mposition = Control.MousePosition
            mposition.Offset(MOUSE_MOVE.X, MOUSE_MOVE.Y)
            Me.Location = mposition
        End If
    End Sub

    Private Sub Panel2_MouseDown(sender As Object, e As MouseEventArgs) Handles Panel2.MouseDown
        MOUSE_MOVE = New Point(-e.X, -e.Y)
    End Sub

    Private Sub Data_Grid_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles Data_Grid.CellContentClick
        Check_Cells()
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
                        If Mode_Bt.BackColor.Equals(Color.White) Then
                            Item_Number_Tb_Validating_Inventario_(sender, 1, FULL_ITEM_NUMBER, ITEM_LAST_ADDED, row.Cells(0).Value.ToString(), Alert_Tb, Data_Grid, BARCODES_DICT, EXPECTED_DICT, ACTUAL_DICT)
                            Item_Number_Tb.Clear()
                            Item_Number_Tb.Focus()
                        End If
                    End If
                        Exit Sub
                End If
            End If
        Next
    End Sub

    '' This function is executed after an item has been scanned
    '' The scanner adds a TAB at the end of the info it reads and 
    '' passes to the program
    '' That event triggers this function
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

    Private Sub New_Item_Number_Validated(sender As Object, e As EventArgs) Handles New_Item_Number.Validated
        New_Item_Number_Helper()
    End Sub

    Private Sub New_Item_Number_Helper()
        Alert_Tb.Text = ""
        '' Validate Input
        If New_Item_Number.Text = "" Then
            Exit Sub 'GoTo EventExitSub
        End If

        Try
            Dim sqlstring As String
            Dim dtOnOrder As DataTable
            Dim dtTemp As DataTable = GetItem("ItemNumber", New_Item_Number.Text)
            If dtTemp.Rows.Count = 0 Then
                Alert_Tb.Text = UCase(New_Item_Number.Text) + ": Not Found!"
                New_Item_Area.Text = ""
                New_Item_Bin_Tb.Text = ""
            Else
                New_Item_Number.Text = UCase(dtTemp.Rows(0)("ItemNumber").ToString)
                Alert_Tb.Text = "Item: " + UCase(dtTemp.Rows(0)("ItemNumber").ToString) + Environment.NewLine + "Description: " + dtTemp.Rows(0)("Description").ToString
                ''New_Item_Qty.Text = dtTemp.Rows(0)("Qty").ToString
                New_Item_Area.Text = dtTemp.Rows(0)("Area").ToString
                New_Item_Bin_Tb.Text = dtTemp.Rows(0)("Bin").ToString
            End If
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
        End Try
    End Sub

    Private Sub Warehouse_Tb_GotFocus(sender As Object, e As EventArgs) Handles Warehouse_Tb.GotFocus
        If Warehouse_Tb.Text.Equals("Warehouse Area") Then
            Warehouse_Tb.Text = ""
        End If
    End Sub

    Private Sub Help_Bt_Click(sender As Object, e As EventArgs) Handles Help_Bt.Click
        ShowWindow(frmHelp_Page)
    End Sub

    Private Sub Warehouse_Tb_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Warehouse_Tb.KeyPress
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



    Private Sub New_Item_Number_KeyPress(sender As Object, e As KeyPressEventArgs) Handles New_Item_Number.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Enter) Then
            ''Console.WriteLine("ENTERED")
            New_Item_Number_Helper()
        End If
    End Sub

    Private Sub Mode_Bt_Click(sender As Object, e As EventArgs) Handles Mode_Bt.Click
        If Not Data_Grid.Rows.Count = 0 Then
            Dim bt As Button = CType(sender, Button)
            If bt.BackColor.Equals(Color.White) Then
                Mode_Register_Barcodes(sender, e)
            Else
                Mode_Count()
            End If
        End If
    End Sub

    Private Sub Mode_Count()
        Mode_Bt.BackColor = Color.White
        Mode_Bt.ForeColor = Color.DimGray
        Mode_Bt.Text = "Mode: COUNT"
    End Sub

    Private Sub Mode_Register_Barcodes(sender As Object, e As EventArgs)
        Mode_Bt.BackColor = Color.Black
        Mode_Bt.ForeColor = Color.White
        Mode_Bt.Text = "Mode: Register Barcodes"
        Fast_Process(sender, e, BARCODES_DICT, Data_Grid)
        Dim result As DialogResult = MessageBox.Show("Warnning: Scanned_Qty has been set to Expected_Qty!", "Augusta", MessageBoxButtons.OK)
    End Sub
End Class