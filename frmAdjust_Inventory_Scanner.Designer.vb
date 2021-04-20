<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAdjust_Inventory_Scanner
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Barcode_Tb = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.New_Item_Qty = New System.Windows.Forms.TextBox()
        Me.New_Item_Area = New System.Windows.Forms.TextBox()
        Me.New_Item_Number = New System.Windows.Forms.TextBox()
        Me.Update_Area_Bin = New System.Windows.Forms.Button()
        Me.New_Item_Bin_Tb = New System.Windows.Forms.TextBox()
        Me.Quantity_Tb = New System.Windows.Forms.TextBox()
        Me.Warehouse_Tb = New System.Windows.Forms.TextBox()
        Me.Item_Number_Tb = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Count_Lb = New System.Windows.Forms.Label()
        Me.Auto_Resize_Bt = New System.Windows.Forms.Button()
        Me.Check_Bt = New System.Windows.Forms.Button()
        Me.Process_Bt = New System.Windows.Forms.Button()
        Me.Copy_Bt = New System.Windows.Forms.Button()
        Me.Delete_Empty_Row = New System.Windows.Forms.Button()
        Me.OK_Bt = New System.Windows.Forms.Button()
        Me.Load_Bt = New System.Windows.Forms.Button()
        Me.Cancel_Bt = New System.Windows.Forms.Button()
        Me.Submit_Bt = New System.Windows.Forms.Button()
        Me.Help_Bt = New System.Windows.Forms.Button()
        Me.Minimize_Form_Bt = New System.Windows.Forms.Button()
        Me.Close_Form_Bt = New System.Windows.Forms.Button()
        Me.Main_Panel = New System.Windows.Forms.Panel()
        Me.Mode_Bt = New System.Windows.Forms.Button()
        Me.Data_Grid = New System.Windows.Forms.DataGridView()
        Me.MessageBox_Tb = New System.Windows.Forms.TextBox()
        Me.Alert_Tb = New System.Windows.Forms.TextBox()
        Me.Header_Panel = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Augusta = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Main_Panel.SuspendLayout()
        CType(Me.Data_Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Header_Panel.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Augusta.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Barcode_Tb)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.New_Item_Qty)
        Me.GroupBox2.Controls.Add(Me.New_Item_Area)
        Me.GroupBox2.Controls.Add(Me.New_Item_Number)
        Me.GroupBox2.Controls.Add(Me.Update_Area_Bin)
        Me.GroupBox2.Controls.Add(Me.New_Item_Bin_Tb)
        Me.GroupBox2.Location = New System.Drawing.Point(17, 597)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(697, 74)
        Me.GroupBox2.TabIndex = 50
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Re-Location: Item_# Must Be Typed!"
        Me.ToolTip1.SetToolTip(Me.GroupBox2, "Update an Item Location")
        '
        'Barcode_Tb
        '
        Me.Barcode_Tb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Barcode_Tb.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Barcode_Tb.Location = New System.Drawing.Point(420, 38)
        Me.Barcode_Tb.Name = "Barcode_Tb"
        Me.Barcode_Tb.Size = New System.Drawing.Size(178, 30)
        Me.Barcode_Tb.TabIndex = 33
        Me.Barcode_Tb.Tag = "Bin"
        Me.ToolTip1.SetToolTip(Me.Barcode_Tb, "Item Barcode")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(462, 13)
        Me.Label1.TabIndex = 32
        Me.Label1.Text = "Item                                                 Qty                Area     " &
    "                  Bin                          Barcode"
        '
        'New_Item_Qty
        '
        Me.New_Item_Qty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.New_Item_Qty.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.New_Item_Qty.Location = New System.Drawing.Point(172, 38)
        Me.New_Item_Qty.Name = "New_Item_Qty"
        Me.New_Item_Qty.Size = New System.Drawing.Size(32, 30)
        Me.New_Item_Qty.TabIndex = 31
        Me.New_Item_Qty.Tag = "Area"
        Me.ToolTip1.SetToolTip(Me.New_Item_Qty, "Item Quantity")
        '
        'New_Item_Area
        '
        Me.New_Item_Area.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.New_Item_Area.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.New_Item_Area.Location = New System.Drawing.Point(234, 38)
        Me.New_Item_Area.Name = "New_Item_Area"
        Me.New_Item_Area.Size = New System.Drawing.Size(87, 30)
        Me.New_Item_Area.TabIndex = 30
        Me.New_Item_Area.Tag = "Area"
        Me.ToolTip1.SetToolTip(Me.New_Item_Area, "Item Area")
        '
        'New_Item_Number
        '
        Me.New_Item_Number.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.New_Item_Number.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.New_Item_Number.Location = New System.Drawing.Point(6, 38)
        Me.New_Item_Number.Name = "New_Item_Number"
        Me.New_Item_Number.Size = New System.Drawing.Size(160, 30)
        Me.New_Item_Number.TabIndex = 27
        Me.New_Item_Number.Tag = "Area"
        Me.ToolTip1.SetToolTip(Me.New_Item_Number, "Item Number")
        '
        'Update_Area_Bin
        '
        Me.Update_Area_Bin.BackColor = System.Drawing.Color.Transparent
        Me.Update_Area_Bin.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.Update_Area_Bin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.Update_Area_Bin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.Update_Area_Bin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Update_Area_Bin.Image = Global.Augusta.Resources.Up_Arrow_Icon
        Me.Update_Area_Bin.Location = New System.Drawing.Point(604, 38)
        Me.Update_Area_Bin.Name = "Update_Area_Bin"
        Me.Update_Area_Bin.Size = New System.Drawing.Size(87, 30)
        Me.Update_Area_Bin.TabIndex = 29
        Me.ToolTip1.SetToolTip(Me.Update_Area_Bin, "Update Item Location")
        Me.Update_Area_Bin.UseVisualStyleBackColor = False
        '
        'New_Item_Bin_Tb
        '
        Me.New_Item_Bin_Tb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.New_Item_Bin_Tb.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.New_Item_Bin_Tb.Location = New System.Drawing.Point(327, 38)
        Me.New_Item_Bin_Tb.Name = "New_Item_Bin_Tb"
        Me.New_Item_Bin_Tb.Size = New System.Drawing.Size(87, 30)
        Me.New_Item_Bin_Tb.TabIndex = 28
        Me.New_Item_Bin_Tb.Tag = "Bin"
        Me.ToolTip1.SetToolTip(Me.New_Item_Bin_Tb, "Item Bin")
        '
        'Quantity_Tb
        '
        Me.Quantity_Tb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Quantity_Tb.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Quantity_Tb.Location = New System.Drawing.Point(1192, 98)
        Me.Quantity_Tb.Name = "Quantity_Tb"
        Me.Quantity_Tb.Size = New System.Drawing.Size(87, 30)
        Me.Quantity_Tb.TabIndex = 45
        Me.ToolTip1.SetToolTip(Me.Quantity_Tb, "Quantity")
        '
        'Warehouse_Tb
        '
        Me.Warehouse_Tb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Warehouse_Tb.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Warehouse_Tb.Location = New System.Drawing.Point(17, 97)
        Me.Warehouse_Tb.Name = "Warehouse_Tb"
        Me.Warehouse_Tb.Size = New System.Drawing.Size(180, 30)
        Me.Warehouse_Tb.TabIndex = 47
        Me.ToolTip1.SetToolTip(Me.Warehouse_Tb, "Warehouse Area")
        '
        'Item_Number_Tb
        '
        Me.Item_Number_Tb.BackColor = System.Drawing.Color.White
        Me.Item_Number_Tb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Item_Number_Tb.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Item_Number_Tb.Location = New System.Drawing.Point(819, 98)
        Me.Item_Number_Tb.Name = "Item_Number_Tb"
        Me.Item_Number_Tb.Size = New System.Drawing.Size(367, 30)
        Me.Item_Number_Tb.TabIndex = 46
        Me.ToolTip1.SetToolTip(Me.Item_Number_Tb, "Scanner Input")
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Count_Lb)
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox1.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(1192, 446)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(87, 57)
        Me.GroupBox1.TabIndex = 44
        Me.GroupBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox1, "Count & Total Count")
        '
        'Count_Lb
        '
        Me.Count_Lb.AutoSize = True
        Me.Count_Lb.Enabled = False
        Me.Count_Lb.Font = New System.Drawing.Font("Consolas", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Count_Lb.Location = New System.Drawing.Point(29, 19)
        Me.Count_Lb.Name = "Count_Lb"
        Me.Count_Lb.Size = New System.Drawing.Size(25, 28)
        Me.Count_Lb.TabIndex = 0
        Me.Count_Lb.Text = "0"
        Me.ToolTip1.SetToolTip(Me.Count_Lb, "Count")
        '
        'Auto_Resize_Bt
        '
        Me.Auto_Resize_Bt.BackColor = System.Drawing.Color.Transparent
        Me.Auto_Resize_Bt.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.Auto_Resize_Bt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.Auto_Resize_Bt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.Auto_Resize_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Auto_Resize_Bt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Auto_Resize_Bt.Location = New System.Drawing.Point(1192, 415)
        Me.Auto_Resize_Bt.Name = "Auto_Resize_Bt"
        Me.Auto_Resize_Bt.Size = New System.Drawing.Size(87, 29)
        Me.Auto_Resize_Bt.TabIndex = 62
        Me.Auto_Resize_Bt.Text = "Resize"
        Me.ToolTip1.SetToolTip(Me.Auto_Resize_Bt, "Auto Resize Columns")
        Me.Auto_Resize_Bt.UseVisualStyleBackColor = False
        '
        'Check_Bt
        '
        Me.Check_Bt.BackColor = System.Drawing.Color.Transparent
        Me.Check_Bt.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.Check_Bt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.Check_Bt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.Check_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Check_Bt.Image = Global.Augusta.Resources.Check_Icon
        Me.Check_Bt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Check_Bt.Location = New System.Drawing.Point(1192, 345)
        Me.Check_Bt.Name = "Check_Bt"
        Me.Check_Bt.Size = New System.Drawing.Size(87, 29)
        Me.Check_Bt.TabIndex = 58
        Me.Check_Bt.Text = "Report"
        Me.Check_Bt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.Check_Bt, "Get Report")
        Me.Check_Bt.UseVisualStyleBackColor = False
        '
        'Process_Bt
        '
        Me.Process_Bt.BackColor = System.Drawing.Color.Transparent
        Me.Process_Bt.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.Process_Bt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.Process_Bt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.Process_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Process_Bt.Image = Global.Augusta.Resources.Process_Icon_1
        Me.Process_Bt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Process_Bt.Location = New System.Drawing.Point(1192, 380)
        Me.Process_Bt.Name = "Process_Bt"
        Me.Process_Bt.Size = New System.Drawing.Size(87, 29)
        Me.Process_Bt.TabIndex = 57
        Me.Process_Bt.Text = "Process"
        Me.Process_Bt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.Process_Bt, "Fast Process: Confirm that everything is present!")
        Me.Process_Bt.UseVisualStyleBackColor = False
        '
        'Copy_Bt
        '
        Me.Copy_Bt.BackColor = System.Drawing.Color.Transparent
        Me.Copy_Bt.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.Copy_Bt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.Copy_Bt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.Copy_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Copy_Bt.Image = Global.Augusta.Resources.Forward_Icon
        Me.Copy_Bt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Copy_Bt.Location = New System.Drawing.Point(1192, 170)
        Me.Copy_Bt.Name = "Copy_Bt"
        Me.Copy_Bt.Size = New System.Drawing.Size(87, 29)
        Me.Copy_Bt.TabIndex = 56
        Me.Copy_Bt.Text = "Copy"
        Me.Copy_Bt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.Copy_Bt, "Copy Item_Number Column to Barcodes Column without ""-""")
        Me.Copy_Bt.UseVisualStyleBackColor = False
        '
        'Delete_Empty_Row
        '
        Me.Delete_Empty_Row.BackColor = System.Drawing.Color.Transparent
        Me.Delete_Empty_Row.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.Delete_Empty_Row.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.Delete_Empty_Row.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.Delete_Empty_Row.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Delete_Empty_Row.Image = Global.Augusta.Resources.Delete_Icon2
        Me.Delete_Empty_Row.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Delete_Empty_Row.Location = New System.Drawing.Point(1192, 205)
        Me.Delete_Empty_Row.Name = "Delete_Empty_Row"
        Me.Delete_Empty_Row.Size = New System.Drawing.Size(87, 29)
        Me.Delete_Empty_Row.TabIndex = 55
        Me.Delete_Empty_Row.Text = "DEL"
        Me.Delete_Empty_Row.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.Delete_Empty_Row, "Delete Missing Inventory")
        Me.Delete_Empty_Row.UseVisualStyleBackColor = False
        '
        'OK_Bt
        '
        Me.OK_Bt.BackColor = System.Drawing.Color.Transparent
        Me.OK_Bt.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.OK_Bt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.OK_Bt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.OK_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.OK_Bt.Image = Global.Augusta.Resources.Add_Icon2
        Me.OK_Bt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.OK_Bt.Location = New System.Drawing.Point(1192, 135)
        Me.OK_Bt.Name = "OK_Bt"
        Me.OK_Bt.Size = New System.Drawing.Size(87, 29)
        Me.OK_Bt.TabIndex = 42
        Me.OK_Bt.Text = "Adjust"
        Me.OK_Bt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.OK_Bt, "Adjust Quantity")
        Me.OK_Bt.UseVisualStyleBackColor = False
        '
        'Load_Bt
        '
        Me.Load_Bt.BackColor = System.Drawing.Color.Transparent
        Me.Load_Bt.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.Load_Bt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.Load_Bt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.Load_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Load_Bt.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Load_Bt.ForeColor = System.Drawing.Color.DimGray
        Me.Load_Bt.Image = Global.Augusta.Resources.Download_Icon
        Me.Load_Bt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Load_Bt.Location = New System.Drawing.Point(203, 98)
        Me.Load_Bt.Name = "Load_Bt"
        Me.Load_Bt.Size = New System.Drawing.Size(96, 29)
        Me.Load_Bt.TabIndex = 48
        Me.Load_Bt.Tag = ""
        Me.Load_Bt.Text = "Load"
        Me.Load_Bt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.Load_Bt, "Load Warehouse Area")
        Me.Load_Bt.UseVisualStyleBackColor = False
        '
        'Cancel_Bt
        '
        Me.Cancel_Bt.BackColor = System.Drawing.Color.Transparent
        Me.Cancel_Bt.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.Cancel_Bt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.Cancel_Bt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.Cancel_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Cancel_Bt.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cancel_Bt.ForeColor = System.Drawing.Color.DimGray
        Me.Cancel_Bt.Image = Global.Augusta.Resources.Delete_Icon2
        Me.Cancel_Bt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Cancel_Bt.Location = New System.Drawing.Point(305, 97)
        Me.Cancel_Bt.Name = "Cancel_Bt"
        Me.Cancel_Bt.Size = New System.Drawing.Size(105, 30)
        Me.Cancel_Bt.TabIndex = 49
        Me.Cancel_Bt.Text = "Cancel"
        Me.Cancel_Bt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.Cancel_Bt, "Cancel")
        Me.Cancel_Bt.UseVisualStyleBackColor = False
        '
        'Submit_Bt
        '
        Me.Submit_Bt.BackColor = System.Drawing.Color.Transparent
        Me.Submit_Bt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.Submit_Bt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.Submit_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Submit_Bt.Image = Global.Augusta.Resources.Upload_Icon3
        Me.Submit_Bt.Location = New System.Drawing.Point(1085, 631)
        Me.Submit_Bt.Name = "Submit_Bt"
        Me.Submit_Bt.Size = New System.Drawing.Size(194, 40)
        Me.Submit_Bt.TabIndex = 43
        Me.ToolTip1.SetToolTip(Me.Submit_Bt, "Update Augusta")
        Me.Submit_Bt.UseVisualStyleBackColor = False
        '
        'Help_Bt
        '
        Me.Help_Bt.FlatAppearance.BorderSize = 0
        Me.Help_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Help_Bt.ForeColor = System.Drawing.Color.White
        Me.Help_Bt.Image = Global.Augusta.Resources.Help_Icon
        Me.Help_Bt.Location = New System.Drawing.Point(144, 26)
        Me.Help_Bt.Name = "Help_Bt"
        Me.Help_Bt.Size = New System.Drawing.Size(48, 32)
        Me.Help_Bt.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.Help_Bt, "Minimize")
        Me.Help_Bt.UseVisualStyleBackColor = True
        '
        'Minimize_Form_Bt
        '
        Me.Minimize_Form_Bt.FlatAppearance.BorderSize = 0
        Me.Minimize_Form_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Minimize_Form_Bt.ForeColor = System.Drawing.Color.White
        Me.Minimize_Form_Bt.Image = Global.Augusta.Resources.Minimize_24
        Me.Minimize_Form_Bt.Location = New System.Drawing.Point(198, 26)
        Me.Minimize_Form_Bt.Name = "Minimize_Form_Bt"
        Me.Minimize_Form_Bt.Size = New System.Drawing.Size(48, 32)
        Me.Minimize_Form_Bt.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.Minimize_Form_Bt, "Minimize")
        Me.Minimize_Form_Bt.UseVisualStyleBackColor = True
        '
        'Close_Form_Bt
        '
        Me.Close_Form_Bt.FlatAppearance.BorderSize = 0
        Me.Close_Form_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Close_Form_Bt.ForeColor = System.Drawing.Color.White
        Me.Close_Form_Bt.Image = Global.Augusta.Resources.Close_24
        Me.Close_Form_Bt.Location = New System.Drawing.Point(252, 26)
        Me.Close_Form_Bt.Name = "Close_Form_Bt"
        Me.Close_Form_Bt.Size = New System.Drawing.Size(48, 32)
        Me.Close_Form_Bt.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.Close_Form_Bt, "Close")
        Me.Close_Form_Bt.UseVisualStyleBackColor = True
        '
        'Main_Panel
        '
        Me.Main_Panel.BackColor = System.Drawing.Color.White
        Me.Main_Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Main_Panel.Controls.Add(Me.Mode_Bt)
        Me.Main_Panel.Controls.Add(Me.Auto_Resize_Bt)
        Me.Main_Panel.Controls.Add(Me.Data_Grid)
        Me.Main_Panel.Controls.Add(Me.Check_Bt)
        Me.Main_Panel.Controls.Add(Me.Process_Bt)
        Me.Main_Panel.Controls.Add(Me.Copy_Bt)
        Me.Main_Panel.Controls.Add(Me.Delete_Empty_Row)
        Me.Main_Panel.Controls.Add(Me.MessageBox_Tb)
        Me.Main_Panel.Controls.Add(Me.Alert_Tb)
        Me.Main_Panel.Controls.Add(Me.GroupBox2)
        Me.Main_Panel.Controls.Add(Me.OK_Bt)
        Me.Main_Panel.Controls.Add(Me.Load_Bt)
        Me.Main_Panel.Controls.Add(Me.Cancel_Bt)
        Me.Main_Panel.Controls.Add(Me.Quantity_Tb)
        Me.Main_Panel.Controls.Add(Me.Warehouse_Tb)
        Me.Main_Panel.Controls.Add(Me.Item_Number_Tb)
        Me.Main_Panel.Controls.Add(Me.Submit_Bt)
        Me.Main_Panel.Controls.Add(Me.GroupBox1)
        Me.Main_Panel.Controls.Add(Me.Header_Panel)
        Me.Main_Panel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Main_Panel.Location = New System.Drawing.Point(0, 0)
        Me.Main_Panel.Name = "Main_Panel"
        Me.Main_Panel.Size = New System.Drawing.Size(1292, 678)
        Me.Main_Panel.TabIndex = 0
        '
        'Mode_Bt
        '
        Me.Mode_Bt.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.Mode_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Mode_Bt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Mode_Bt.ForeColor = System.Drawing.Color.DimGray
        Me.Mode_Bt.Location = New System.Drawing.Point(603, 99)
        Me.Mode_Bt.Name = "Mode_Bt"
        Me.Mode_Bt.Size = New System.Drawing.Size(210, 30)
        Me.Mode_Bt.TabIndex = 63
        Me.Mode_Bt.Text = "Mode: Register Barcodes"
        Me.Mode_Bt.UseVisualStyleBackColor = True
        '
        'Data_Grid
        '
        Me.Data_Grid.AllowUserToAddRows = False
        DataGridViewCellStyle9.BackColor = System.Drawing.Color.DimGray
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Black
        Me.Data_Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle9
        Me.Data_Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Data_Grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.Data_Grid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.Data_Grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical
        Me.Data_Grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.Color.DimGray
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Data_Grid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle10
        Me.Data_Grid.ColumnHeadersHeight = 30
        Me.Data_Grid.Cursor = System.Windows.Forms.Cursors.Hand
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Data_Grid.DefaultCellStyle = DataGridViewCellStyle11
        Me.Data_Grid.EnableHeadersVisualStyles = False
        Me.Data_Grid.Location = New System.Drawing.Point(17, 135)
        Me.Data_Grid.Name = "Data_Grid"
        Me.Data_Grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.Info
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Data_Grid.RowHeadersDefaultCellStyle = DataGridViewCellStyle12
        Me.Data_Grid.RowHeadersVisible = False
        Me.Data_Grid.Size = New System.Drawing.Size(1169, 368)
        Me.Data_Grid.TabIndex = 59
        '
        'MessageBox_Tb
        '
        Me.MessageBox_Tb.BackColor = System.Drawing.Color.White
        Me.MessageBox_Tb.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.MessageBox_Tb.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MessageBox_Tb.Location = New System.Drawing.Point(536, 509)
        Me.MessageBox_Tb.Multiline = True
        Me.MessageBox_Tb.Name = "MessageBox_Tb"
        Me.MessageBox_Tb.Size = New System.Drawing.Size(337, 82)
        Me.MessageBox_Tb.TabIndex = 54
        '
        'Alert_Tb
        '
        Me.Alert_Tb.BackColor = System.Drawing.Color.White
        Me.Alert_Tb.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Alert_Tb.Enabled = False
        Me.Alert_Tb.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Alert_Tb.Location = New System.Drawing.Point(17, 509)
        Me.Alert_Tb.Multiline = True
        Me.Alert_Tb.Name = "Alert_Tb"
        Me.Alert_Tb.Size = New System.Drawing.Size(513, 82)
        Me.Alert_Tb.TabIndex = 53
        '
        'Header_Panel
        '
        Me.Header_Panel.BackColor = System.Drawing.Color.DimGray
        Me.Header_Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Header_Panel.Controls.Add(Me.Panel2)
        Me.Header_Panel.Controls.Add(Me.Panel1)
        Me.Header_Panel.Dock = System.Windows.Forms.DockStyle.Top
        Me.Header_Panel.Location = New System.Drawing.Point(0, 0)
        Me.Header_Panel.Name = "Header_Panel"
        Me.Header_Panel.Size = New System.Drawing.Size(1290, 91)
        Me.Header_Panel.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Augusta)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(999, 89)
        Me.Panel2.TabIndex = 1
        '
        'Augusta
        '
        Me.Augusta.Controls.Add(Me.Label2)
        Me.Augusta.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Augusta.ForeColor = System.Drawing.Color.White
        Me.Augusta.Location = New System.Drawing.Point(16, 10)
        Me.Augusta.Name = "Augusta"
        Me.Augusta.Size = New System.Drawing.Size(251, 59)
        Me.Augusta.TabIndex = 36
        Me.Augusta.TabStop = False
        Me.Augusta.Text = "Augusta"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Consolas", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(219, 43)
        Me.Label2.TabIndex = 36
        Me.Label2.Text = "Inventario"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Help_Bt)
        Me.Panel1.Controls.Add(Me.Minimize_Form_Bt)
        Me.Panel1.Controls.Add(Me.Close_Form_Bt)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel1.Location = New System.Drawing.Point(978, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(310, 89)
        Me.Panel1.TabIndex = 0
        '
        'frmAdjust_Inventory_Scanner
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1292, 678)
        Me.Controls.Add(Me.Main_Panel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmAdjust_Inventory_Scanner"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Adjust Inventory w Scanner"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Main_Panel.ResumeLayout(False)
        Me.Main_Panel.PerformLayout()
        CType(Me.Data_Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Header_Panel.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Augusta.ResumeLayout(False)
        Me.Augusta.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Main_Panel As Panel
    Friend WithEvents Header_Panel As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Close_Form_Bt As Button
    Friend WithEvents Minimize_Form_Bt As Button
    Friend WithEvents Data_Grid As DataGridView
    Friend WithEvents Check_Bt As Button
    Friend WithEvents Process_Bt As Button
    Friend WithEvents Copy_Bt As Button
    Friend WithEvents Delete_Empty_Row As Button
    Friend WithEvents MessageBox_Tb As TextBox
    Friend WithEvents Alert_Tb As TextBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Barcode_Tb As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents New_Item_Qty As TextBox
    Friend WithEvents New_Item_Area As TextBox
    Friend WithEvents New_Item_Number As TextBox
    Friend WithEvents Update_Area_Bin As Button
    Friend WithEvents New_Item_Bin_Tb As TextBox
    Friend WithEvents OK_Bt As Button
    Friend WithEvents Load_Bt As Button
    Friend WithEvents Cancel_Bt As Button
    Friend WithEvents Quantity_Tb As TextBox
    Friend WithEvents Warehouse_Tb As TextBox
    Friend WithEvents Item_Number_Tb As TextBox
    Friend WithEvents Submit_Bt As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Count_Lb As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Augusta As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Help_Bt As Button
    Friend WithEvents Auto_Resize_Bt As Button
    Friend WithEvents Mode_Bt As Button
End Class
