<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPO_Receipt
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Help_Bt = New System.Windows.Forms.Button()
        Me.Minimize_Form_Bt = New System.Windows.Forms.Button()
        Me.Close_Form_Bt = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Augusta = New System.Windows.Forms.GroupBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Data_Grid = New System.Windows.Forms.DataGridView()
        Me.MessageBox_Tb = New System.Windows.Forms.TextBox()
        Me.Alert_Tb = New System.Windows.Forms.TextBox()
        Me.Count_Lb = New System.Windows.Forms.Label()
        Me.Quantity_Tb = New System.Windows.Forms.TextBox()
        Me.Receipt_Number_Tb = New System.Windows.Forms.TextBox()
        Me.Item_Number_Tb = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Header_Panel = New System.Windows.Forms.Panel()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GetReport_Bt = New System.Windows.Forms.Button()
        Me.Fast_Process_Bt = New System.Windows.Forms.Button()
        Me.Copy_Bt = New System.Windows.Forms.Button()
        Me.OK_Bt = New System.Windows.Forms.Button()
        Me.Load_Bt = New System.Windows.Forms.Button()
        Me.Cancel_Bt = New System.Windows.Forms.Button()
        Me.Submit_Bt = New System.Windows.Forms.Button()
        Me.Auto_Resize_Bt = New System.Windows.Forms.Button()
        Me.Main_Panel = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.Augusta.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.Data_Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.Header_Panel.SuspendLayout()
        Me.Main_Panel.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Help_Bt)
        Me.Panel1.Controls.Add(Me.Minimize_Form_Bt)
        Me.Panel1.Controls.Add(Me.Close_Form_Bt)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel1.Location = New System.Drawing.Point(972, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(310, 89)
        Me.Panel1.TabIndex = 0
        '
        'Help_Bt
        '
        Me.Help_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Help_Bt.ForeColor = System.Drawing.Color.White
        Me.Help_Bt.Image = Global.Augusta.Resources.Help_Icon
        Me.Help_Bt.Location = New System.Drawing.Point(27, 10)
        Me.Help_Bt.Name = "Help_Bt"
        Me.Help_Bt.Size = New System.Drawing.Size(87, 32)
        Me.Help_Bt.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.Help_Bt, "Minimize")
        Me.Help_Bt.UseVisualStyleBackColor = True
        '
        'Minimize_Form_Bt
        '
        Me.Minimize_Form_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Minimize_Form_Bt.ForeColor = System.Drawing.Color.White
        Me.Minimize_Form_Bt.Image = Global.Augusta.Resources.Minimize_24
        Me.Minimize_Form_Bt.Location = New System.Drawing.Point(120, 10)
        Me.Minimize_Form_Bt.Name = "Minimize_Form_Bt"
        Me.Minimize_Form_Bt.Size = New System.Drawing.Size(87, 32)
        Me.Minimize_Form_Bt.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.Minimize_Form_Bt, "Minimize")
        Me.Minimize_Form_Bt.UseVisualStyleBackColor = True
        '
        'Close_Form_Bt
        '
        Me.Close_Form_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Close_Form_Bt.ForeColor = System.Drawing.Color.White
        Me.Close_Form_Bt.Image = Global.Augusta.Resources.Close_24
        Me.Close_Form_Bt.Location = New System.Drawing.Point(213, 10)
        Me.Close_Form_Bt.Name = "Close_Form_Bt"
        Me.Close_Form_Bt.Size = New System.Drawing.Size(87, 32)
        Me.Close_Form_Bt.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.Close_Form_Bt, "Close")
        Me.Close_Form_Bt.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Consolas", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(219, 43)
        Me.Label2.TabIndex = 36
        Me.Label2.Text = "PO Receipt"
        '
        'Augusta
        '
        Me.Augusta.Controls.Add(Me.Label2)
        Me.Augusta.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Augusta.ForeColor = System.Drawing.Color.White
        Me.Augusta.Location = New System.Drawing.Point(16, 10)
        Me.Augusta.Name = "Augusta"
        Me.Augusta.Size = New System.Drawing.Size(256, 62)
        Me.Augusta.TabIndex = 36
        Me.Augusta.TabStop = False
        Me.Augusta.Text = "Augusta"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Augusta)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(985, 89)
        Me.Panel2.TabIndex = 1
        '
        'Data_Grid
        '
        Me.Data_Grid.AllowUserToAddRows = False
        Me.Data_Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Data_Grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.Data_Grid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.Data_Grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical
        Me.Data_Grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.DimGray
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Data_Grid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.Data_Grid.ColumnHeadersHeight = 30
        Me.Data_Grid.Cursor = System.Windows.Forms.Cursors.Hand
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Data_Grid.DefaultCellStyle = DataGridViewCellStyle2
        Me.Data_Grid.EnableHeadersVisualStyles = False
        Me.Data_Grid.Location = New System.Drawing.Point(17, 135)
        Me.Data_Grid.Name = "Data_Grid"
        Me.Data_Grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.Info
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Data_Grid.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.Data_Grid.RowHeadersVisible = False
        Me.Data_Grid.Size = New System.Drawing.Size(1169, 368)
        Me.Data_Grid.TabIndex = 59
        '
        'MessageBox_Tb
        '
        Me.MessageBox_Tb.BackColor = System.Drawing.Color.White
        Me.MessageBox_Tb.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.MessageBox_Tb.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MessageBox_Tb.Location = New System.Drawing.Point(536, 532)
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
        Me.Alert_Tb.Location = New System.Drawing.Point(17, 532)
        Me.Alert_Tb.Multiline = True
        Me.Alert_Tb.Name = "Alert_Tb"
        Me.Alert_Tb.Size = New System.Drawing.Size(513, 82)
        Me.Alert_Tb.TabIndex = 53
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
        'Receipt_Number_Tb
        '
        Me.Receipt_Number_Tb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Receipt_Number_Tb.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Receipt_Number_Tb.Location = New System.Drawing.Point(17, 97)
        Me.Receipt_Number_Tb.Name = "Receipt_Number_Tb"
        Me.Receipt_Number_Tb.Size = New System.Drawing.Size(180, 30)
        Me.Receipt_Number_Tb.TabIndex = 47
        Me.ToolTip1.SetToolTip(Me.Receipt_Number_Tb, "PO Receipt Number")
        '
        'Item_Number_Tb
        '
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
        'Header_Panel
        '
        Me.Header_Panel.BackColor = System.Drawing.Color.DimGray
        Me.Header_Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Header_Panel.Controls.Add(Me.Panel2)
        Me.Header_Panel.Controls.Add(Me.Panel1)
        Me.Header_Panel.Dock = System.Windows.Forms.DockStyle.Top
        Me.Header_Panel.Location = New System.Drawing.Point(0, 0)
        Me.Header_Panel.Name = "Header_Panel"
        Me.Header_Panel.Size = New System.Drawing.Size(1284, 91)
        Me.Header_Panel.TabIndex = 0
        '
        'GetReport_Bt
        '
        Me.GetReport_Bt.BackColor = System.Drawing.Color.Transparent
        Me.GetReport_Bt.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.GetReport_Bt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.GetReport_Bt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.GetReport_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GetReport_Bt.Image = Global.Augusta.Resources.Check_Icon
        Me.GetReport_Bt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.GetReport_Bt.Location = New System.Drawing.Point(1192, 341)
        Me.GetReport_Bt.Name = "GetReport_Bt"
        Me.GetReport_Bt.Size = New System.Drawing.Size(87, 29)
        Me.GetReport_Bt.TabIndex = 58
        Me.GetReport_Bt.Text = "Report"
        Me.GetReport_Bt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.GetReport_Bt, "Get Report")
        Me.GetReport_Bt.UseVisualStyleBackColor = False
        '
        'Fast_Process_Bt
        '
        Me.Fast_Process_Bt.BackColor = System.Drawing.Color.Transparent
        Me.Fast_Process_Bt.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.Fast_Process_Bt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.Fast_Process_Bt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.Fast_Process_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Fast_Process_Bt.Image = Global.Augusta.Resources.Process_Icon_1
        Me.Fast_Process_Bt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Fast_Process_Bt.Location = New System.Drawing.Point(1192, 376)
        Me.Fast_Process_Bt.Name = "Fast_Process_Bt"
        Me.Fast_Process_Bt.Size = New System.Drawing.Size(87, 29)
        Me.Fast_Process_Bt.TabIndex = 57
        Me.Fast_Process_Bt.Text = "Process"
        Me.Fast_Process_Bt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.Fast_Process_Bt, "Fast Process: Confirm that everything is present!")
        Me.Fast_Process_Bt.UseVisualStyleBackColor = False
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
        Me.ToolTip1.SetToolTip(Me.Load_Bt, "Load PO Receipt")
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
        Me.Cancel_Bt.Size = New System.Drawing.Size(110, 30)
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
        Me.Submit_Bt.Location = New System.Drawing.Point(1085, 647)
        Me.Submit_Bt.Name = "Submit_Bt"
        Me.Submit_Bt.Size = New System.Drawing.Size(194, 40)
        Me.Submit_Bt.TabIndex = 43
        Me.ToolTip1.SetToolTip(Me.Submit_Bt, "Update Augusta")
        Me.Submit_Bt.UseVisualStyleBackColor = False
        '
        'Auto_Resize_Bt
        '
        Me.Auto_Resize_Bt.BackColor = System.Drawing.Color.Transparent
        Me.Auto_Resize_Bt.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.Auto_Resize_Bt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.Auto_Resize_Bt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.Auto_Resize_Bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Auto_Resize_Bt.Location = New System.Drawing.Point(1192, 411)
        Me.Auto_Resize_Bt.Name = "Auto_Resize_Bt"
        Me.Auto_Resize_Bt.Size = New System.Drawing.Size(87, 29)
        Me.Auto_Resize_Bt.TabIndex = 61
        Me.Auto_Resize_Bt.Text = "Resize"
        Me.ToolTip1.SetToolTip(Me.Auto_Resize_Bt, "Auto Resize Columns")
        Me.Auto_Resize_Bt.UseVisualStyleBackColor = False
        '
        'Main_Panel
        '
        Me.Main_Panel.BackColor = System.Drawing.Color.White
        Me.Main_Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Main_Panel.Controls.Add(Me.Auto_Resize_Bt)
        Me.Main_Panel.Controls.Add(Me.Data_Grid)
        Me.Main_Panel.Controls.Add(Me.GetReport_Bt)
        Me.Main_Panel.Controls.Add(Me.Fast_Process_Bt)
        Me.Main_Panel.Controls.Add(Me.Copy_Bt)
        Me.Main_Panel.Controls.Add(Me.MessageBox_Tb)
        Me.Main_Panel.Controls.Add(Me.Alert_Tb)
        Me.Main_Panel.Controls.Add(Me.OK_Bt)
        Me.Main_Panel.Controls.Add(Me.Load_Bt)
        Me.Main_Panel.Controls.Add(Me.Cancel_Bt)
        Me.Main_Panel.Controls.Add(Me.Quantity_Tb)
        Me.Main_Panel.Controls.Add(Me.Receipt_Number_Tb)
        Me.Main_Panel.Controls.Add(Me.Item_Number_Tb)
        Me.Main_Panel.Controls.Add(Me.Submit_Bt)
        Me.Main_Panel.Controls.Add(Me.GroupBox1)
        Me.Main_Panel.Controls.Add(Me.Header_Panel)
        Me.Main_Panel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Main_Panel.Location = New System.Drawing.Point(0, 0)
        Me.Main_Panel.Name = "Main_Panel"
        Me.Main_Panel.Size = New System.Drawing.Size(1286, 700)
        Me.Main_Panel.TabIndex = 1
        '
        'frmPO_Receipt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1286, 700)
        Me.Controls.Add(Me.Main_Panel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmPO_Receipt"
        Me.Text = "frmPO_Receipt"
        Me.Panel1.ResumeLayout(False)
        Me.Augusta.ResumeLayout(False)
        Me.Augusta.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.Data_Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Header_Panel.ResumeLayout(False)
        Me.Main_Panel.ResumeLayout(False)
        Me.Main_Panel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Close_Form_Bt As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Minimize_Form_Bt As Button
    Friend WithEvents GetReport_Bt As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Fast_Process_Bt As Button
    Friend WithEvents Copy_Bt As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Augusta As GroupBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Data_Grid As DataGridView
    Friend WithEvents MessageBox_Tb As TextBox
    Friend WithEvents Alert_Tb As TextBox
    Friend WithEvents Count_Lb As Label
    Friend WithEvents OK_Bt As Button
    Friend WithEvents Load_Bt As Button
    Friend WithEvents Cancel_Bt As Button
    Friend WithEvents Quantity_Tb As TextBox
    Friend WithEvents Receipt_Number_Tb As TextBox
    Friend WithEvents Item_Number_Tb As TextBox
    Friend WithEvents Submit_Bt As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Header_Panel As Panel
    Friend WithEvents Main_Panel As Panel
    Friend WithEvents Help_Bt As Button
    Friend WithEvents Auto_Resize_Bt As Button
End Class
