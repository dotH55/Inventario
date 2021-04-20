Option Strict Off
Option Explicit On
Imports System.Configuration
Imports VB = Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Net.Mail
Imports System.Collections.Generic


Module Module1

    Private Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Integer)

    'Public dbAugusta As dao.Database
    '    Public rsInventory As DataTable 'DataTable
    'Public rsVendor As DataTable 'DataTable

    Public txtSearchResults As String
    Public txtMainWhs As String
    Public glbUserName As String
    Public glbFontSize As Int16 = 8
    Public glbAllowBilling As Boolean
    Public glbAllowPayments As Boolean
    Public glbAllowMaint As Boolean
    Public glbAllowAP As Boolean
    Public glbLocation As String
    Public glbComputerName As String
    Public glbUser_Computer As String
    Public glbProgram_Location As String

    'Define Variable to make the WantOrdered Form accessible from multiple forms
    Public glbItemNumber As String
    Public glbDescription As String

    'Define Variable for Passwords
    Public glbEmailAddress As String = ""
    Public glbEmailPassword As String = ""

    'Define some report variables for Company info
    Public glbCompanyAddress As String
    Public glbCompanyCSZ As String
    Public glbCompanyPhoneFax As String
    Public glbCompanyCity As String
    Public glbCompanyYear As String

    Public UserForeColor As System.Drawing.Color

    'Public ReportAppl As New CRAXDRT.Application
    Public glbReport As CrystalDecisions.CrystalReports.Engine.ReportDocument = New CrystalDecisions.CrystalReports.Engine.ReportDocument

    Public Col_Type(500, 2) As String


    Public Function CustomerNameSearch(ByRef prmCustomerName As String) As DataTable
        Dim sqlString As String
        Dim rsTemp As New DataTable ''DataTable 'DataTable

        ' doing this so we will return 0 rows
        If prmCustomerName Is Nothing Then
            'Hit cancel during search
            Return rsTemp
            Exit Function
        End If
        prmCustomerName = prmCustomerName.ToUpper
        If prmCustomerName = "" Then
            prmCustomerName = "-1"
        End If

        If IsNumeric(prmCustomerName) Then
            'Entered a Customer Number instead of Customer Name
            sqlString = "SELECT * FROM Customer " & _
                "WHERE CustomerNumber = " & prmCustomerName & " AND Location = '" & glbLocation & "'"
            rsTemp = SQLGetDataTable(sqlString, cnSQL)
            If rsTemp.Rows.Count > 0 Then
                prmCustomerName = rsTemp.Rows(0)("Name").ToString
                sqlString = "SELECT * FROM Customer " & _
                    "WHERE Name = '" & prmCustomerName & "' AND Location = '" & glbLocation & "'"
                rsTemp = SQLGetDataTable(sqlString, cnSQL)
                If rsTemp.Rows.Count = 0 Then
                    'MsgBox("No Customer Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                    'Found = False
                Else
                    'Found = True
                End If
            Else
                'Found = False
                MsgBox("No Customer Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            End If
        Else ' txt entered
            sqlString = "SELECT * FROM Customer " & _
                "WHERE Name = '" & prmCustomerName & "' AND Location = '" & glbLocation & "'"
            rsTemp = SQLGetDataTable(sqlString, cnSQL)
            If rsTemp.Rows.Count = 0 Then
                rsTemp = CustomerKeywordSearch(prmCustomerName)
                If rsTemp.Rows.Count = 0 Then
                    MsgBox("No Customer Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                Else
                    rsTemp = CustomerNameSearch(prmCustomerName)
                    If rsTemp.Rows.Count = 0 Then
                        MsgBox("No Customer Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                    End If
                End If
            End If

        End If

        Return rsTemp

    End Function

    Public Function GetItemNumber_ItemDescription(ByRef ItemNo As TextBox, ByRef ItemDescript As TextBox) As Boolean
        'Found = False
        Dim rsInventory As DataTable
        Dim KeepFocus As Boolean = True

        rsInventory = InventorySearch(ItemNo.Text, ItemDescript.Text)
        If ItemDescript.Text = "USER HIT CANCEL" Or ItemNo.Text = "USER HIT CANCEL" Then
            ItemNo.Text = ""
            ItemDescript.Text = ""
            ItemNo.Focus()
        ElseIf rsInventory.Rows.Count = 0 Then
            ItemNo.Focus()
        Else
            ItemDescript.Text = rsInventory.Rows(0)("Description").ToString
            ItemNo.Text = rsInventory.Rows(0)("ItemNumber").ToString

            KeepFocus = False
        End If

EventExitSub:
        Return KeepFocus
        'EventArgs.Cancel = KeepFocus

    End Function

    Public Function InventorySearch(ByRef prmInventoryNumber As String, ByRef prmInventoryDescription As String) As DataTable
        Dim sqlString As String
        Dim rsTemp As New DataTable ''DataTable 'DataTable

        ' doing this so we will return 0 rows
        If prmInventoryNumber Is Nothing Then
            prmInventoryNumber = ""
        End If
        prmInventoryNumber = prmInventoryNumber.ToUpper
        If prmInventoryNumber = "" Then
            prmInventoryNumber = "-1"
        End If

        'First need to see if the Item Number passed is a HIT
        'If so this is our Hard Exit of the Recursive Call.
        sqlString = "SELECT * FROM Inventory WHERE ItemNumber = '" & prmInventoryNumber & "';"
        rsTemp = SQLGetDataTable(sqlString, cnSQL)
        If prmInventoryNumber = "USER HIT CANCEL" Or prmInventoryDescription = "USER HIT CANCEL" Then
            Return rsTemp
            Exit Function
        End If
        If rsTemp.Rows.Count > 0 Then
            prmInventoryNumber = rsTemp.Rows(0)("ItemNumber").ToString
            prmInventoryDescription = rsTemp.Rows(0)("Description").ToString
            Return rsTemp
            Exit Function
        End If
        If IsNumeric(prmInventoryNumber) Then
            'Entered a Customer Number instead of Customer Name
            sqlString = "SELECT * FROM Inventory WHERE ItemNumber = '" & prmInventoryNumber & "';"
            rsTemp = SQLGetDataTable(sqlString, cnSQL)
            If rsTemp.Rows.Count > 0 Then
                prmInventoryNumber = rsTemp.Rows(0)("ItemNumber").ToString
                prmInventoryDescription = rsTemp.Rows(0)("Description").ToString
                'sqlString = "SELECT * FROM Inventory WHERE ItemNumber = '" & prmInventoryNumber & "';"
                'rsTemp = SQLGetDataTable(sqlString, cnSQL)
            Else
                'Found = False
                If MsgBox("No Item Found.  Would you like a Search Screen?", vbYesNo) = vbYes Then
                    rsTemp = InventoryKeywordSearch(prmInventoryNumber, prmInventoryDescription)
                    If rsTemp.Rows.Count = 0 Then
                        MsgBox("No Item Number Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                    Else
                        If prmInventoryNumber = "USER HIT CANCEL" Or prmInventoryDescription = "USER HIT CANCEL" Then 'Already shown search screen and they hit Cancel Button
                            prmInventoryDescription = "USER HIT CANCEL"
                            Return rsTemp
                            Exit Function
                        End If
                        rsTemp = InventorySearch(prmInventoryNumber, prmInventoryDescription)
                        If rsTemp.Rows.Count = 0 Then
                            MsgBox("No Item Number Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                        End If
                    End If
                Else
                    MsgBox("No Item Number Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                End If
            End If
        Else ' txt entered
            sqlString = "SELECT * FROM Inventory WHERE Description = '" & prmInventoryNumber & "';"
            rsTemp = SQLGetDataTable(sqlString, cnSQL)
            If rsTemp.Rows.Count = 0 Then
                rsTemp = InventoryKeywordSearch(prmInventoryNumber, prmInventoryDescription)
                If rsTemp.Rows.Count = 0 Then
                    MsgBox("No Item Number Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                Else
                    rsTemp = InventorySearch(prmInventoryNumber, prmInventoryDescription)
                    If rsTemp.Rows.Count = 0 Then
                        MsgBox("No Item Number Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                    End If
                End If
            End If

        End If

        Return rsTemp

    End Function

    Private Function InventoryKeywordSearch(ByRef prmInventoryNumber As String, ByRef prmInventoryDescription As String) As DataTable '', ByRef rsCustomer As DataTable) As Object

        ' First see if any item number begin with the partial/complete
        ' item number entered.
        '
        ' Second if not a valid part number then do a search on any description
        ' which begins with the letters entered.

        Dim rsSearch As DataTable ''DataTable 'DataTable
        ' First see if any item number begin with the partial/complete
        ' item number entered.
        '
        ' Second if not a valid part number then do a search on any description
        ' which begins with the letters entered.

        rsSearch = PartialItem("ItemNumber", prmInventoryNumber, "Inventory") 'PartialItem("Name", prmInventoryNumber, "Customer")

        If rsSearch.Rows.Count = 0 Then ' no record selected, search description
            MsgBox("No Inventory Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            Return rsSearch
            Exit Function
        End If

        Select Case rsSearch.Rows.Count
            Case 1 ' there is only one record
                prmInventoryNumber = rsSearch.Rows(0)("ItemNumber").ToString
            Case Else

                Dim fSearch As New frmSearch
                fSearch.pSQL = ""
                fSearch.FormCalling = "Inventory"

                'frmSearch.rsSearch = rsSerialized
                If IsNumeric(prmInventoryNumber) Then
                    fSearch.txtItemNumber.Text = prmInventoryNumber
                Else
                    fSearch.txtItemDescription.Text = prmInventoryNumber
                End If
                fSearch.ShowDialog()
                If txtSearchResults Is Nothing Then
                    txtSearchResults = "USER HIT CANCEL"
                End If
                prmInventoryNumber = txtSearchResults

        End Select
        Return rsSearch
    End Function

    Public Function GetCustomerNumber_CustomerName(ByRef CustNo As TextBox, ByRef CustName As TextBox) As Boolean
        Dim rsCustomer As DataTable
        Dim KeepFocus As Boolean = True

        If CustName.Text = "" Then
            CustName.Text = CustNo.Text
        End If
        rsCustomer = CustomerNameSearch(CustName.Text)
        If rsCustomer.Rows.Count = 0 Then
            KeepFocus = True
        Else
            CustName.Text = rsCustomer.Rows(0)("Name").ToString
            CustNo.Text = rsCustomer.Rows(0)("CustomerNumber").ToString

            KeepFocus = False
        End If
        Return KeepFocus

    End Function

    Private Function CustomerKeywordSearch(ByRef prmCustomerName As String) As DataTable '', ByRef rsCustomer As DataTable) As Object

        ' First see if any item number begin with the partial/complete
        ' item number entered.
        '
        ' Second if not a valid part number then do a search on any description
        ' which begins with the letters entered.

        Dim rsSearch As DataTable ''DataTable 'DataTable
        ' First see if any item number begin with the partial/complete
        ' item number entered.
        '
        ' Second if not a valid part number then do a search on any description
        ' which begins with the letters entered.

        rsSearch = PartialItem("Name", prmCustomerName, "Customer")

        If rsSearch.Rows.Count = 0 Then ' no record selected, search description
            MsgBox("No Customer Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            Return rsSearch
            Exit Function
        End If

        Select Case rsSearch.Rows.Count
            Case 1 ' there is only one record
                prmCustomerName = rsSearch.Rows(0)("Name").ToString
            Case Else

                Dim fSearch As New frmSearch
                fSearch.pSQL = ""
                fSearch.FormCalling = "Customer"

                'frmSearch.rsSearch = rsSerialized
                If IsNumeric(prmCustomerName) Then
                    fSearch.txtItemNumber.Text = prmCustomerName
                Else
                    fSearch.txtItemDescription.Text = prmCustomerName
                End If
                fSearch.ShowDialog()
                prmCustomerName = txtSearchResults

        End Select
        Return rsSearch
    End Function

    Public Function GetVendorNumber_VendorName(ByRef VendorNo As TextBox, ByRef VendorName As TextBox) As Boolean
        Dim rsCustomer As DataTable
        Dim KeepFocus As Boolean = True

        rsCustomer = VendorNameSearch(VendorName.Text)
        If rsCustomer.Rows.Count = 0 Then
            KeepFocus = True
        Else
            VendorName.Text = rsCustomer.Rows(0)("Name").ToString
            VendorNo.Text = rsCustomer.Rows(0)("VendorNumber").ToString

            KeepFocus = False
        End If
        Return KeepFocus
    End Function

    Public Function VendorNameSearch(ByRef prmVendorName As String) As DataTable
        Dim sqlString As String
        Dim rsTemp As New DataTable ''DataTable 'DataTable

        ' doing this so we will return 0 rows
        If prmVendorName = Nothing Then
            prmVendorName = "-1"
        End If
        prmVendorName = prmVendorName.ToUpper
        If prmVendorName = "" Then
            prmVendorName = "-1"
        End If

        If IsNumeric(prmVendorName) Then
            'Entered a Customer Number instead of Customer Name
            sqlString = "SELECT * FROM Vendors WHERE VendorNumber = " & prmVendorName & ";"
            rsTemp = SQLGetDataTable(sqlString, cnSQL)
            If rsTemp.Rows.Count > 0 Then
                prmVendorName = rsTemp.Rows(0)("Name").ToString
                sqlString = "SELECT * FROM Vendors WHERE Name = '" & prmVendorName & "';"
                rsTemp = SQLGetDataTable(sqlString, cnSQL)
                If rsTemp.Rows.Count = 0 Then
                    'MsgBox("No Customer Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                    'Found = False
                Else
                    'Found = True
                End If
            Else
                'Found = False
                MsgBox("No Vendor Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            End If
        Else ' txt entered
            sqlString = "SELECT * FROM Vendors WHERE Name = '" & prmVendorName & "';"
            rsTemp = SQLGetDataTable(sqlString, cnSQL)
            If rsTemp.Rows.Count = 0 Then
                rsTemp = VendorKeywordSearch(prmVendorName)
                If rsTemp.Rows.Count = 0 Then
                    MsgBox("No Vendor Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                Else
                    rsTemp = VendorNameSearch(prmVendorName)
                    If prmVendorName = "-1" Then
                        'Cancel during search screen
                        Return rsTemp
                        Exit Function
                    End If
                    If rsTemp.Rows.Count = 0 Then
                        MsgBox("No Vendor Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                    End If
                End If
            End If

        End If

        Return rsTemp

    End Function

    Private Function VendorKeywordSearch(ByRef prmVendorName As String) As DataTable '', ByRef rsCustomer As DataTable) As Object

        ' First see if any item number begin with the partial/complete
        ' item number entered.
        '
        ' Second if not a valid part number then do a search on any description
        ' which begins with the letters entered.

        Dim rsSearch As DataTable ''DataTable 'DataTable
        ' First see if any item number begin with the partial/complete
        ' item number entered.
        '
        ' Second if not a valid part number then do a search on any description
        ' which begins with the letters entered.

        rsSearch = PartialItem("Name", prmVendorName, "Vendors")

        If rsSearch.Rows.Count = 0 Then ' no record selected, search description
            MsgBox("No Vendor Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            Return rsSearch
            Exit Function
        End If

        Select Case rsSearch.Rows.Count
            Case 1 ' there is only one record
                prmVendorName = rsSearch.Rows(0)("Name").ToString
            Case Else

                Dim fSearch As New frmSearch
                fSearch.pSQL = ""
                fSearch.FormCalling = "Vendors"

                'frmSearch.rsSearch = rsSerialized
                fSearch.txtItemNumber.Text = prmVendorName
                'If IsNumeric(prmVendorName) Then
                '    fSearch.txtItemNumber.Text = prmVendorName
                'Else
                '    fSearch.txtItemDescription.Text = prmVendorName
                'End If
                fSearch.ShowDialog()
                prmVendorName = txtSearchResults

        End Select
        Return rsSearch
    End Function

    Public Function PadLeft(ByRef txtstring As String, ByRef nbrstringlength As Short) As String
        ' Pad Left the given string (txtstring) and make the ending string nbrstringlength long
        Dim x As Short
        Dim blankstring As String

        blankstring = ""
        For x = 1 To nbrstringlength
            blankstring &= " "
        Next
        'PadLeft = Right(blankstring & Trim(txtstring), nbrstringlength - Len(Trim(txtstring)))
        PadLeft = Right(blankstring & Trim(txtstring), nbrstringlength)
    End Function

#Region "RecWrite Procs"

    'Public Function RecWriteSQL(frmName As Form, tblName As String, rsName As Recordset) As String
    '    On Error GoTo RecWriteErr
    '    '
    '    Dim ctlTemp As Control
    '    Dim cTag As String
    '    Dim nbrFieldsInSql As Integer
    '    Dim blnFound As Boolean
    '    Dim sqlstring As String
    '    Dim rs As New ADODB.Recordset
    '    Dim Col_Type(100, 2) As String
    '    Dim x, nbrMaxFields As Integer
    '
    '    x = 0
    '    tblName = "[" & tblName & "]"
    '
    '    Set rs = cnQuote.OpenSchema(adSchemaColumns, _
    ''        Array(Empty, Empty, tblName, Empty))
    '
    '    Do While Not rs.EOF
    '        Debug.Print rs!column_name ' lstFields.AddItem rs!COLUMN_NAME
    '        Col_Type(x, 0) = UCase(rs!column_name)
    '        cTag = rs!column_name
    '        Col_Type(x, 1) = rsName.Fields(cTag).Type
    '        rs.MoveNext
    '        x = x + 1
    '    Loop
    '    nbrMaxFields = x - 1
    '
    '    sqlstring = "Update " & tblName & " SET "
    '    nbrFieldsInSql = 0
    '
    '    For Each ctlTemp In frmName.Controls
    '        cTag = UCase(Trim(ctlTemp.Tag))
    '        If Len(cTag) <> 0 Then
    '            x = 0
    '            blnFound = False
    '            Do While (x <= nbrMaxFields And Not blnFound)
    '                If cTag = Col_Type(x, 0) Then
    '                    blnFound = True
    '                    'Going to add another field to sqlstring add "," if not first entry
    '                    If nbrFieldsInSql >= 1 Then
    '                        sqlstring = sqlstring & ", "
    '                    End If
    '                    nbrFieldsInSql = nbrFieldsInSql + 1
    '                    Select Case Col_Type(x, 1)
    '                        Case adVarWChar, adLongVarChar, adLongVarWChar, adChar, adWChar
    '                          'String
    '                            sqlstring = sqlstring & tblName & "." & "[" & cTag & "] = '" & Replace(ctlTemp.Text, "'", "^") & "'"
    '                        Case adSmallInt, adInteger, adSingle, adDouble, adCurrency, adDecimal, adTinyInt, adUnsignedTinyInt, adUnsignedSmallInt, adUnsignedInt, adBigInt, adUnsignedBigInt
    '                          'Number of some type
    '                            sqlstring = sqlstring & tblName & "." & "[" & cTag & "] = " & Format(ctlTemp.Text, "########0.#####")
    '                        Case adDate, adDBDate, adDBTime, adDBTimeStamp
    '                          'Date
    '                            sqlstring = sqlstring & tblName & "." & "[" & cTag & "] = '" & ctlTemp.Text & "'"
    '                        Case Else
    '                            sqlstring = sqlstring & tblName & "." & "[" & cTag & "] = " & ctlTemp.Text
    '                    End Select
    '                End If
    '                x = x + 1
    '            Loop
    '        End If
    '    Next
    ''    For Each ctlTemp In frmName.Controls
    ''        cTag = UCase(Trim(ctlTemp.Tag))
    ''        rs.MoveFirst
    ''        If Len(cTag) <> 0 Then
    ''            blnFound = False
    ''Debug.Print "Doing ctag = " & cTag
    ''            Do While (Not rs.EOF And Not blnFound)
    ''Debug.Print "     Cycle thru Field = " & rs!column_name
    ''
    ''                If cTag = UCase(rs!column_name) Then
    ''                    'Debug.Print rs!column_name & rsName.Fields(cTag).Type
    ''                    blnFound = True
    ''                    'Going to add another field to sqlstring add "," if not first entry
    ''                    If nbrFieldsInSql >= 1 Then
    ''                        sqlstring = sqlstring & ", "
    ''                    End If
    ''                    nbrFieldsInSql = nbrFieldsInSql + 1
    ''                    Select Case rsName.Fields(cTag).Type
    ''                        Case adVarWChar, adLongVarChar, adLongVarWChar, adChar, adWChar
    ''                          'String
    ''                            sqlstring = sqlstring & cTag & " = '" & Replace(ctlTemp.Text, "'", "^") & "'"
    ''                        Case adSmallInt, adInteger, adSingle, adDouble, adCurrency, adDecimal, adTinyInt, adUnsignedTinyInt, adUnsignedSmallInt, adUnsignedInt, adBigInt, adUnsignedBigInt
    ''                          'Number of some type
    ''                            sqlstring = sqlstring & cTag & " = " & Format(ctlTemp.Text, "########0.#####")
    ''                        Case adDate, adDBDate, adDBTime, adDBTimeStamp
    ''                          'Date
    ''                            sqlstring = sqlstring & cTag & " = '" & ctlTemp.Text & "'"
    ''                        Case Else
    ''                            sqlstring = sqlstring & cTag & " = " & ctlTemp.Text
    ''                    End Select
    ''                End If
    ''                rs.MoveNext
    ''            Loop
    ''        End If
    ''    Next
    ''Constant    Value   Description
    ''adEmpty     0   No value
    ''adSmallInt  2   A 2-byte signed integer.
    ''adInteger   3   A 4-byte signed integer.
    ''adSingle    4   A single-precision floating-point value.
    ''adDouble    5   A double-precision floating-point value.
    ''adCurrency  6   A currency value
    ''adDate  7   The number of days since December 30, 1899 + the fraction of a day.
    ''adBSTR  8   A null-terminated character string.
    ''adIDispatch     9   A pointer to an IDispatch interface on a COM object. Note: Currently not supported by ADO.
    ''adError     10  A 32-bit error code
    ''adBoolean   11  A boolean value.
    ''adVariant   12  An Automation Variant. Note: Currently not supported by ADO.
    ''adIUnknown  13  A pointer to an IUnknown interface on a COM object. Note: Currently not supported by ADO.
    ''adDecimal   14  An exact numeric value with a fixed precision and scale.
    ''adTinyInt   16  A 1-byte signed integer.
    ''adUnsignedTinyInt   17  A 1-byte unsigned integer.
    ''adUnsignedSmallInt  18  A 2-byte unsigned integer.
    ''adUnsignedInt   19  A 4-byte unsigned integer.
    ''adBigInt    20  An 8-byte signed integer.
    ''adUnsignedBigInt    21  An 8-byte unsigned integer.
    ''adFileTime  64  The number of 100-nanosecond intervals since January 1,1601
    ''adGUID  72  A globally unique identifier (GUID)
    ''adBinary    128     A binary value.
    ''adChar  129     A string value.
    ''adWChar     130     A null-terminated Unicode character string.
    ''adNumeric   131     An exact numeric value with a fixed precision and scale.
    ''adUserDefined   132     A user-defined variable.
    ''adDBDate    133     A date value (yyyymmdd).
    ''adDBTime    134     A time value (hhmmss).
    ''adDBTimeStamp   135     A date/time stamp (yyyymmddhhmmss plus a fraction in billionths).
    ''adChapter   136     A 4-byte chapter value that identifies rows in a child rowset
    ''adPropVariant   138     An Automation PROPVARIANT.
    ''adVarNumeric    139     A numeric value (Parameter object only).
    ''adVarChar   200     A string value (Parameter object only).
    ''adLongVarChar   201     A long string value.
    ''adVarWChar  202     A null-terminated Unicode character string.
    ''adLongVarWChar  203     A long null-terminated Unicode string value.
    ''adVarBinary     204     A binary value (Parameter object only).
    ''adLongVarBinary     205     A long binary value.
    ''AdArray     0x2000  A flag value combined with another data type constant. Indicates an array of that other data type.
    '
    '    RecWriteSQL = sqlstring
    '    GoTo RecWriteExit
    '
    'RecWriteErr:
    '    RecError Err, Error$, "RecWrite"
    '    RecWriteSQL = Err
    '    GoTo RecWriteExit
    '
    'RecWriteExit:
    '    Set rs = Nothing
    'End Function
    '
    '

    '	Public Function RecWrite(ByRef frmName As System.Windows.Forms.Form, ByRef rsName As DataTable) As Short
    '		'
    '		' update current record with data from controls
    '		'
    '		' Inputs:
    '		'   frmName     Name of form w/ bound controls
    '		'   rsName      Name of recordset to update
    '		'
    '		' Outputs:
    '		'   RecWrite    recOK - if no errors
    '		'
    '		On Error GoTo RecWriteErr
    '		'
    '		Dim ctlTemp As System.Windows.Forms.Control
    '		Dim cTag As String
    '		Dim lAttrib As Integer
    '		'

    '		On Error Resume Next

    '		For	Each ctlTemp In frmName.Controls
    '			cTag = UCase(Trim(ctlTemp.Tag))
    '			If Len(cTag) <> 0 Then
    '				lAttrib = rsName.Fields(cTag).Attributes
    '				If (lAttrib And dao.FieldAttributeEnum.dbAutoIncrField) = False Then
    '					If rsName.Fields(cTag).Type = dao.DataTypeEnum.dbDate Then
    '						'I am getting an error using the dtDate fields
    '						'They do not have the properties as an normal Date field.
    '						'so I am going to trap for errors myself and try to overcome
    '						'this.  I would like for this code to do all the forms
    '						'                    On Error GoTo 0
    '						'                    On Error Resume Next
    '						'                    rsName.Fields(cTag) = ctlTemp.Text
    '						'                    If Err.Number = 438 Then
    '						
    '						rsName.Fields(cTag).Value = ctlTemp.Value
    '						'                    ElseIf Err.Number = 3265 Then
    '						'                        'do nothing see not below
    '						'                    End If
    '						'                    On Error GoTo RecWriteErr
    '						
    '					ElseIf TypeOf ctlTemp Is System.Windows.Forms.CheckBox Then 
    '						
    '						If ctlTemp.Value = System.Windows.Forms.CheckState.Checked Then
    '							rsName.Fields(cTag).Value = True
    '						Else
    '							rsName.Fields(cTag).Value = False
    '						End If
    '					Else
    '						'I am getting an error in Invoicing, concerning the Shipto screen
    '						'I want to use the same shipto screen for both customer and Invoicing
    '						'There are fields with tags in the screen for customer which are not used
    '						'in Invoicing (ie Contact, fax, phone, ect..)
    '						'So I am going to trap for error 3265, which is field is not in collection
    '						'It will just skip the item
    '						'                    On Error GoTo 0
    '						'                    On Error Resume Next
    '						rsName.Fields(cTag).Value = ctlTemp.Text
    '						'                    If Err.Number <> 0 Then
    '						'                        If Err.Number = 3265 Then
    '						'                            GoTo RecWriteErr
    '						'                            'rsName.Fields(cTag) = ctlTemp.Value
    '						'                        End If
    '						'                    End If
    '						'                    On Error GoTo RecWriteErr
    '					End If
    '				End If
    '			End If
    '		Next ctlTemp

    '		RecWrite = True ' all ok
    '		GoTo RecWriteExit

    'RecWriteErr: 
    '		If Err.Number = 3265 Then
    '			Resume Next
    '		End If
    '		RecError(Err, ErrorToString(), "RecWrite")
    '		RecWrite = Err.Number
    '		Resume Next
    '		'GoTo RecWriteExit

    'RecWriteExit: 
    '	End Function
    '    Public Function RecWriteADO(ByRef frmName As System.Windows.Forms.Form, ByRef tblName As String, ByRef rsName As ADODB.Recordset) As String
    '        On Error GoTo RecWriteErr
    '        '
    '        Dim ctlTemp, ctlTempGroup As System.Windows.Forms.Control
    '        Dim cTag As String
    '        Dim nbrFieldsInSql As Int16
    '        Dim blnFound As Boolean
    '        Dim sqlString As String
    '        Dim rs As New ADODB.Recordset
    '        'Dim Col_Type(100, 2) As String
    '        Dim x As Int16
    '        Dim nbrMaxFields As Int16

    '        
    '        x = 0
    '        tblName = "[" & tblName & "]"

    '        
    '        rs = cnDSI.OpenSchema(ADODB.SchemaEnum.adSchemaColumns, New Object() {Nothing, Nothing, tblName, Nothing})

    '        Do While Not rs.EOF
    '            ''Debug.Print(rs.Fields("column_name").Value) ' lstFields.AddItem rs!COLUMN_NAME
    '            
    '            Col_Type(x, 0) = UCase(rs.Fields("column_name").Value)
    '            cTag = rs.Fields("column_name").Value
    '            
    '            Col_Type(x, 1) = CStr(rsName.Fields.Item(cTag).Type)  ' CStr(rsName.Fields(cTag).Type)
    '            rs.MoveNext()
    '            
    '            x = x + 1
    '        Loop
    '        
    '        nbrMaxFields = x - 1

    '        sqlString = "Update " & tblName & " SET "
    '        nbrFieldsInSql = 0

    '        For Each ctlTemp In frmName.Controls
    '            cTag = UCase(Trim(ctlTemp.Tag))
    '            If TypeOf ctlTemp Is System.Windows.Forms.GroupBox Then
    '                ctlTempGroup = Nothing
    '                For Each ctlTempGroup In ctlTemp.Controls
    '                    x = x + 1
    '                    cTag = UCase(Trim(ctlTempGroup.Tag))
    '                    ''Debug.Print(CStr(x) & " " & cTag & "  -  " & ctlTempGroup.Name)
    '                    '                    ctltempGroup.Text = "x"
    '                    If Len(cTag) > 0 Then
    '                        recWrite_Element(cTag, nbrMaxFields, ctlTempGroup, tblName, sqlString, nbrFieldsInSql)
    '                    End If
    '                Next ctlTempGroup
    '            Else
    '                If Len(cTag) > 0 Then
    '                    recWrite_Element(cTag, nbrMaxFields, ctlTemp, tblName, sqlString, nbrFieldsInSql)
    '                End If
    '            End If
    '        Next ctlTemp
    '        'Constant    Value   Description
    '        'adEmpty     0   No value
    '        'adSmallInt  2   A 2-byte signed integer.
    '        'adInteger   3   A 4-byte signed integer.
    '        'adSingle    4   A single-precision floating-point value.
    '        'adDouble    5   A double-precision floating-point value.
    '        'adCurrency  6   A currency value
    '        'adDate  7   The number of days since December 30, 1899 + the fraction of a day.
    '        'adBSTR  8   A null-terminated character string.
    '        'adIDispatch     9   A pointer to an IDispatch interface on a COM object. Note: Currently not supported by ADO.
    '        'adError     10  A 32-bit error code
    '        'adBoolean   11  A boolean value.
    '        'adVariant   12  An Automation Variant. Note: Currently not supported by ADO.
    '        'adIUnknown  13  A pointer to an IUnknown interface on a COM object. Note: Currently not supported by ADO.
    '        'adDecimal   14  An exact numeric value with a fixed precision and scale.
    '        'adTinyInt   16  A 1-byte signed integer.
    '        'adUnsignedTinyInt   17  A 1-byte unsigned integer.
    '        'adUnsignedSmallInt  18  A 2-byte unsigned integer.
    '        'adUnsignedInt   19  A 4-byte unsigned integer.
    '        'adBigInt    20  An 8-byte signed integer.
    '        'adUnsignedBigInt    21  An 8-byte unsigned integer.
    '        'adFileTime  64  The number of 100-nanosecond intervals since January 1,1601
    '        'adGUID  72  A globally unique identifier (GUID)
    '        'adBinary    128     A binary value.
    '        'adChar  129     A string value.
    '        'adWChar     130     A null-terminated Unicode character string.
    '        'adNumeric   131     An exact numeric value with a fixed precision and scale.
    '        'adUserDefined   132     A user-defined variable.
    '        'adDBDate    133     A date value (yyyymmdd).
    '        'adDBTime    134     A time value (hhmmss).
    '        'adDBTimeStamp   135     A date/time stamp (yyyymmddhhmmss plus a fraction in billionths).
    '        'adChapter   136     A 4-byte chapter value that identifies rows in a child rowset
    '        'adPropVariant   138     An Automation PROPVARIANT.
    '        'adVarNumeric    139     A numeric value (Parameter object only).
    '        'adVarChar   200     A string value (Parameter object only).
    '        'adLongVarChar   201     A long string value.
    '        'adVarWChar  202     A null-terminated Unicode character string.
    '        'adLongVarWChar  203     A long null-terminated Unicode string value.
    '        'adVarBinary     204     A binary value (Parameter object only).
    '        'adLongVarBinary     205     A long binary value.
    '        'AdArray     0x2000  A flag value combined with another data type constant. Indicates an array of that other data type.

    '        RecWriteADO = sqlString
    '        GoTo RecWriteExit

    'RecWriteErr:
    '        RecError(Err, ErrorToString(), "RecWrite")
    '        RecWriteADO = CStr(Err.Number)
    '        GoTo RecWriteExit

    'RecWriteExit:
    '        'UPGRADE_NOTE: Object rs may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
    '        rs = Nothing
    '    End Function

    Private Sub recWrite_Element(ByRef ctag As String, ByRef nbrMaxFields As Int16, ByVal ctlTemp As System.Windows.Forms.Control, ByRef tblName As String, ByRef sqlstring As String, ByRef nbrFieldsInSql As Int16)
        Dim blnFound As Boolean
        '        Dim sqlstring As String
        Dim x As Int16


        If Len(ctag) <> 0 Then

            x = 0
            blnFound = False

            Do While (x <= nbrMaxFields And Not blnFound)

                If ctag = Col_Type(x, 0) Then
                    blnFound = True
                    'Going to add another field to sqlstring add "," if not first entry
                    If nbrFieldsInSql >= 1 Then
                        sqlstring &= ", "
                    End If
                    nbrFieldsInSql += 1

                    Select Case Col_Type(x, 1)
                        Case CStr(ADODB.DataTypeEnum.adVarChar), CStr(ADODB.DataTypeEnum.adVarWChar), CStr(ADODB.DataTypeEnum.adLongVarChar), CStr(ADODB.DataTypeEnum.adLongVarWChar), CStr(ADODB.DataTypeEnum.adChar), CStr(ADODB.DataTypeEnum.adWChar)
                            'String
                            sqlstring = sqlstring & tblName & "." & "[" & ctag & "] = '" & Replace(ctlTemp.Text, "'", "^") & "'"
                        Case CStr(ADODB.DataTypeEnum.adSmallInt), CStr(ADODB.DataTypeEnum.adInteger), CStr(ADODB.DataTypeEnum.adSingle), CStr(ADODB.DataTypeEnum.adDouble), CStr(ADODB.DataTypeEnum.adCurrency), CStr(ADODB.DataTypeEnum.adDecimal), CStr(ADODB.DataTypeEnum.adTinyInt), CStr(ADODB.DataTypeEnum.adUnsignedTinyInt), CStr(ADODB.DataTypeEnum.adUnsignedSmallInt), CStr(ADODB.DataTypeEnum.adUnsignedInt), CStr(ADODB.DataTypeEnum.adBigInt), CStr(ADODB.DataTypeEnum.adUnsignedBigInt)
                            'Number of some type
                            sqlstring = sqlstring & tblName & "." & "[" & ctag & "] = " & Format_No_Comma_x_Decimals(ctlTemp.Text, 5)
                        Case CStr(ADODB.DataTypeEnum.adDate), CStr(ADODB.DataTypeEnum.adDBDate), CStr(ADODB.DataTypeEnum.adDBTime), CStr(ADODB.DataTypeEnum.adDBTimeStamp)
                            'Date
                            sqlstring = sqlstring & tblName & "." & "[" & ctag & "] = '" & ctlTemp.Text & "'"
                        Case Else
                            sqlstring = sqlstring & tblName & "." & "[" & ctag & "] = " & ctlTemp.Text
                    End Select
                End If

                x += 1
            Loop
        End If

    End Sub

    Public Function RecWriteSQL(ByRef frmName As System.Windows.Forms.Form, ByRef tblName As String, ByRef rsName As DataTable) As String
        On Error GoTo RecWriteErr
        '
        Dim ctlTemp, ctlTempGroup, ctlSubTempGroup As System.Windows.Forms.Control
        Dim cTag As String
        Dim nbrFieldsInSql As Int16
        Dim blnFound As Boolean
        Dim sqlString As String
        Dim rs As New DataTable ''ADODB.Recordset
        'Dim Col_Type(100, 2) As String
        Dim x As Int16
        Dim nbrMaxFields As Int16
        Dim tempDR As SqlClient.SqlDataReader
        Dim tempCMD As New SqlClient.SqlCommand()
        Dim ctltab As System.Windows.Forms.TabControl

        x = 0
        tblName = "[" & tblName & "]"

        sqlString = "select * from " & tblName & " where 1 = 0"
        tempCMD = New SqlCommand(sqlString, cnSQL)
        tempDR = tempCMD.ExecuteReader(CommandBehavior.KeyInfo)
        rs = tempDR.GetSchemaTable()
        tempDR.Close()
        tempCMD.Dispose()

        'Do While Not rs.EOF
        '    Debug.Print(rs.Fields("column_name").Value) ' lstFields.AddItem rs!COLUMN_NAME
        '    
        '    Col_Type(x, 0) = UCase(rs.Fields("column_name").Value)
        '    cTag = rs.Fields("column_name").Value
        '    
        '    Col_Type(x, 1) = CStr(rsName.Fields.Item(cTag).Type)  ' CStr(rsName.Fields(cTag).Type)
        '    rs.MoveNext()
        '    
        '    x = x + 1
        'Loop
        'For Each dr As DataRow In rs.Rows
        '    ''Debug.Print(dr("column_name").Value) ' lstFields.AddItem rs!COLUMN_NAME
        '    Col_Type(x, 0) = UCase(dr("columnname").Value)
        '    cTag = dr("column_name").Value
        '    Col_Type(x, 1) = CStr(dr.Item(cTag).Type)  ' CStr(rsName.Fields(cTag).Type)
        '    'rs.MoveNext()
        '    x = x + 1
        'Next
        For Each dr As DataRow In rs.Rows
            Col_Type(x, 0) = UCase(dr("ColumnName").ToString)
            cTag = dr("ColumnName").ToString

            Col_Type(x, 1) = dr("ProviderType").ToString ''dr("ProviderSpecificDataType").ToString ''dr("DataTypeName").ToString ' could be 24 +		System.Data.DataColumn	{ProviderSpecificDataType}	System.Data.DataColumn
            Col_Type(x, 2) = dr("ProviderSpecificDataType").ToString
            x += 1
        Next

        nbrMaxFields = x - 1

        sqlString = "Update " & tblName & " SET "
        nbrFieldsInSql = 0

        For Each ctlTemp In frmName.Controls  '' 1
            cTag = UCase(Trim(ctlTemp.Tag))

            If TypeOf ctlTemp Is System.Windows.Forms.TabControl Then
                ctltab = ctlTemp
                For Each tp As TabPage In ctltab.TabPages
                    For Each ctl As Control In tp.Controls
                        If TypeOf ctl Is System.Windows.Forms.GroupBox Then
                            'MessageBox.Show(ctl.Controls.Count.ToString())
                            ctlTempGroup = Nothing
                            For Each ctlTempGroup In ctl.Controls
                                x += 1
                                cTag = UCase(Trim(ctlTempGroup.Tag))
                                If TypeOf ctlTempGroup Is System.Windows.Forms.GroupBox Then

                                    For Each ctlSubTempGroup In ctlTempGroup.Controls
                                        x += 1
                                        cTag = UCase(Trim(ctlSubTempGroup.Tag))
                                        If Len(cTag) > 0 Then
                                            recWrite_ElementSQL(cTag, nbrMaxFields, ctlSubTempGroup, tblName, sqlString, nbrFieldsInSql)
                                        End If
                                    Next ctlSubTempGroup
                                End If
                                'If cTag = "ComboMeter" Then
                                '    MessageBox.Show("ComboMeter")
                                'End If

                                ''Debug.Print(CStr(x) & " " & cTag & "  -  " & ctlTempGroup.Name)
                                If Len(cTag) > 0 Then
                                    recWrite_ElementSQL(cTag, nbrMaxFields, ctlTempGroup, tblName, sqlString, nbrFieldsInSql)
                                End If
                            Next ctlTempGroup
                        Else
                            cTag = UCase(Trim(ctl.Tag))
                            If Len(cTag) > 0 Then
                                recWrite_ElementSQL(cTag, nbrMaxFields, ctl, tblName, sqlString, nbrFieldsInSql)
                            End If
                        End If
                    Next
                Next
            End If
            If TypeOf ctlTemp Is System.Windows.Forms.GroupBox Then
                ctlTempGroup = Nothing
                For Each ctlTempGroup In ctlTemp.Controls ''2
                    x += 1
                    cTag = UCase(Trim(ctlTempGroup.Tag))
                    ''Debug.Print(CStr(x) & " " & cTag & "  -  " & ctlTempGroup.Name)
                    '                    ctltempGroup.Text = "x"
                    If Len(cTag) > 0 Then
                        recWrite_ElementSQL(cTag, nbrMaxFields, ctlTempGroup, tblName, sqlString, nbrFieldsInSql)
                    End If
                Next ctlTempGroup ''2
            Else
                If Len(cTag) > 0 Then
                    recWrite_ElementSQL(cTag, nbrMaxFields, ctlTemp, tblName, sqlString, nbrFieldsInSql)
                End If
            End If
        Next ctlTemp '' 1

        'Constant    Value   Description
        'adEmpty     0   No value
        'adSmallInt  2   A 2-byte signed integer.
        'adInteger   3   A 4-byte signed integer.
        'adSingle    4   A single-precision floating-point value.
        'adDouble    5   A double-precision floating-point value.
        'adCurrency  6   A currency value
        'adDate  7   The number of days since December 30, 1899 + the fraction of a day.
        'adBSTR  8   A null-terminated character string.
        'adIDispatch     9   A pointer to an IDispatch interface on a COM object. Note: Currently not supported by ADO.
        'adError     10  A 32-bit error code
        'adBoolean   11  A boolean value.
        'adVariant   12  An Automation Variant. Note: Currently not supported by ADO.
        'adIUnknown  13  A pointer to an IUnknown interface on a COM object. Note: Currently not supported by ADO.
        'adDecimal   14  An exact numeric value with a fixed precision and scale.
        'adTinyInt   16  A 1-byte signed integer.
        'adUnsignedTinyInt   17  A 1-byte unsigned integer.
        'adUnsignedSmallInt  18  A 2-byte unsigned integer.
        'adUnsignedInt   19  A 4-byte unsigned integer.
        'adBigInt    20  An 8-byte signed integer.
        'adUnsignedBigInt    21  An 8-byte unsigned integer.
        'adFileTime  64  The number of 100-nanosecond intervals since January 1,1601
        'adGUID  72  A globally unique identifier (GUID)
        'adBinary    128     A binary value.
        'adChar  129     A string value.
        'adWChar     130     A null-terminated Unicode character string.
        'adNumeric   131     An exact numeric value with a fixed precision and scale.
        'adUserDefined   132     A user-defined variable.
        'adDBDate    133     A date value (yyyymmdd).
        'adDBTime    134     A time value (hhmmss).
        'adDBTimeStamp   135     A date/time stamp (yyyymmddhhmmss plus a fraction in billionths).
        'adChapter   136     A 4-byte chapter value that identifies rows in a child rowset
        'adPropVariant   138     An Automation PROPVARIANT.
        'adVarNumeric    139     A numeric value (Parameter object only).
        'adVarChar   200     A string value (Parameter object only).
        'adLongVarChar   201     A long string value.
        'adVarWChar  202     A null-terminated Unicode character string.
        'adLongVarWChar  203     A long null-terminated Unicode string value.
        'adVarBinary     204     A binary value (Parameter object only).
        'adLongVarBinary     205     A long binary value.
        'AdArray     0x2000  A flag value combined with another data type constant. Indicates an array of that other data type.

        RecWriteSQL = sqlString
        GoTo RecWriteExit

RecWriteErr:
        RecError(Err, ErrorToString(), "RecWrite")
        RecWriteSQL = "" 'CStr(Err.Number)
        GoTo RecWriteExit

RecWriteExit:
        'UPGRADE_NOTE: Object rs may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rs = Nothing
    End Function

    Private Sub recWrite_ElementSQL(ByRef ctag As String, ByRef nbrMaxFields As Int16, ByVal ctlTemp As System.Windows.Forms.Control, ByRef tblName As String, ByRef sqlstring As String, ByRef nbrFieldsInSql As Int16)
        Dim blnFound As Boolean
        '        Dim sqlstring As String
        Dim x As Int16


        If Len(ctag) <> 0 Then
            x = 0
            blnFound = False
            Do While (x <= nbrMaxFields And Not blnFound)
                If ctag = Col_Type(x, 0) Then
                    blnFound = True
                    'Going to add another field to sqlstring add "," if not first entry
                    If nbrFieldsInSql >= 1 Then
                        sqlstring &= ", "
                    End If
                    nbrFieldsInSql += 1
                    Select Case Col_Type(x, 1)
                        Case CStr(SqlDbType.VarChar), CStr(SqlDbType.NText) ''CStr(ADODB.DataTypeEnum.adVarChar), CStr(ADODB.DataTypeEnum.adVarWChar), CStr(ADODB.DataTypeEnum.adLongVarChar), CStr(ADODB.DataTypeEnum.adLongVarWChar), CStr(ADODB.DataTypeEnum.adChar), CStr(ADODB.DataTypeEnum.adWChar)
                            'String
                            sqlstring = sqlstring & tblName & "." & "[" & ctag & "] = '" & Replace(ctlTemp.Text, "'", "^") & "'"
                        Case CStr(SqlDbType.SmallInt), CStr(SqlDbType.Int), CStr(SqlDbType.TinyInt), CStr(SqlDbType.BigInt)
                            ''CStr(ADODB.DataTypeEnum.adSmallInt), CStr(ADODB.DataTypeEnum.adInteger), CStr(ADODB.DataTypeEnum.adSingle), CStr(ADODB.DataTypeEnum.adDouble), CStr(ADODB.DataTypeEnum.adCurrency), CStr(ADODB.DataTypeEnum.adDecimal), CStr(ADODB.DataTypeEnum.adTinyInt), CStr(ADODB.DataTypeEnum.adUnsignedTinyInt), CStr(ADODB.DataTypeEnum.adUnsignedSmallInt), CStr(ADODB.DataTypeEnum.adUnsignedInt), CStr(ADODB.DataTypeEnum.adBigInt), CStr(ADODB.DataTypeEnum.adUnsignedBigInt)
                            'Number of some integer type
                            sqlstring = sqlstring & tblName & "." & "[" & ctag & "] = " & CInt(IIf(ctlTemp.Text = "", 0, ctlTemp.Text))  ''ctlTemp.Text.Format("########0.#####") ''.ToString("########0.#####") ''VB6.Format(ctlTemp.Text, "########0.#####")
                        Case CStr(SqlDbType.Real), CStr(SqlDbType.Float), CStr(SqlDbType.Money), CStr(SqlDbType.Decimal)
                            ''CStr(ADODB.DataTypeEnum.adSmallInt), CStr(ADODB.DataTypeEnum.adInteger), CStr(ADODB.DataTypeEnum.adSingle), CStr(ADODB.DataTypeEnum.adDouble), CStr(ADODB.DataTypeEnum.adCurrency), CStr(ADODB.DataTypeEnum.adDecimal), CStr(ADODB.DataTypeEnum.adTinyInt), CStr(ADODB.DataTypeEnum.adUnsignedTinyInt), CStr(ADODB.DataTypeEnum.adUnsignedSmallInt), CStr(ADODB.DataTypeEnum.adUnsignedInt), CStr(ADODB.DataTypeEnum.adBigInt), CStr(ADODB.DataTypeEnum.adUnsignedBigInt)
                            'Number of some decimal type
                            sqlstring = sqlstring & tblName & "." & "[" & ctag & "] = " & CDec(IIf(ctlTemp.Text = "", 0, ctlTemp.Text))  ''ctlTemp.Text.Format("########0.#####") ''.ToString("########0.#####") ''VB6.Format(ctlTemp.Text, "########0.#####")
                        Case CStr(SqlDbType.DateTime)  ''CStr(ADODB.DataTypeEnum.adDate), CStr(ADODB.DataTypeEnum.adDBDate), CStr(ADODB.DataTypeEnum.adDBTime), CStr(ADODB.DataTypeEnum.adDBTimeStamp)
                            'Date
                            sqlstring = sqlstring & tblName & "." & "[" & ctag & "] = '" & ctlTemp.Text.ToString & "'" 'ctlTemp.Text.ToString
                        Case CStr(SqlDbType.Bit)  ''CStr(ADODB.DataTypeEnum.adBinary), CStr(ADODB.DataTypeEnum.adBoolean)
                            If TypeOf ctlTemp Is System.Windows.Forms.CheckBox Then
                                Dim MyCheckBox As CheckBox
                                MyCheckBox = ctlTemp
                                If MyCheckBox.Checked = False Then
                                    sqlstring = sqlstring & tblName & "." & "[" & ctag & "] = " & 0
                                Else
                                    sqlstring = sqlstring & tblName & "." & "[" & ctag & "] = " & 1
                                End If
                            Else
                                If ctlTemp.Text = "False" Then
                                    sqlstring = sqlstring & tblName & "." & "[" & ctag & "] = " & 0
                                Else
                                    sqlstring = sqlstring & tblName & "." & "[" & ctag & "] = " & 1
                                End If
                            End If
                        Case Else
                            sqlstring = sqlstring & tblName & "." & "[" & ctag & "] = " & ctlTemp.Text
                    End Select
                    'Select Col_Type(x, 1)
                    ' '' ADODB.DataTypeEnum.adVarWChar), CStr(ADODB.DataTypeEnum.adLongVarChar), CStr(ADODB.DataTypeEnum.adLongVarWChar), CStr(ADODB.DataTypeEnum.adChar), CStr(ADODB.DataTypeEnum.adWChar)
                    'Case CStr(SqlDbType.VarChar)
                    '    'String
                    '    
                    '    If TypeOf ctlTemp Is System.Windows.Forms.ComboBox Then
                    '        
                    '        Dim MyComboBox As ComboBox
                    '        MyComboBox = ctlTemp
                    '        MyComboBox.SelectedItem = 0
                    '    ElseIf TypeOf ctlTemp Is System.Windows.Forms.MaskedTextBox Then
                    '        Dim MyMaskedTextBox As MaskedTextBox
                    '        MyMaskedTextBox = ctlTemp
                    '        'If SqlDbType.Text Then ''ADODB.DataTypeEnum.adLongVarWChar <> ADODB.DataTypeEnum.adLongVarWChar Then
                    '        '    MyMaskedTextBox.MaxLength = rsName.Rows(0)(ctag).DefinedSize

                    '        'End If
                    '        MyMaskedTextBox.Text = String.Empty
                    '    ElseIf TypeOf ctlTemp Is System.Windows.Forms.CheckBox Then
                    '        Dim MyCheckBox As CheckBox
                    '        MyCheckBox = ctlTemp
                    '        MyCheckBox.Checked = False
                    '    Else
                    '        Dim MyTextBox As TextBox
                    '        MyTextBox = ctlTemp
                    '        'If SqlDbType.Text Then ''ADODB.DataTypeEnum.adLongVarWChar <> ADODB.DataTypeEnum.adLongVarWChar Then
                    '        '    
                    '        '    MyTextBox.MaxLength = rsName.Rows(0)(ctag).DefinedSize

                    '        'End If
                    '        MyTextBox.Text = String.Empty
                    '    End If
                    'Case CStr(SqlDbType.SmallInt), CStr(SqlDbType.Int), CStr(SqlDbType.Real), CStr(SqlDbType.Float), CStr(SqlDbType.Money), CStr(SqlDbType.Decimal), CStr(SqlDbType.TinyInt), CStr(SqlDbType.BigInt)
                    '    ''CStr(ADODB.DataTypeEnum.adSmallInt), CStr(ADODB.DataTypeEnum.adInteger), CStr(ADODB.DataTypeEnum.adSingle), CStr(ADODB.DataTypeEnum.adDouble), CStr(ADODB.DataTypeEnum.adCurrency), 
                    '    ''CStr(ADODB.DataTypeEnum.adDecimal), CStr(ADODB.DataTypeEnum.adTinyInt), CStr(ADODB.DataTypeEnum.adUnsignedTinyInt), CStr(ADODB.DataTypeEnum.adUnsignedSmallInt), 
                    '    ''CStr(ADODB.DataTypeEnum.adUnsignedInt), CStr(ADODB.DataTypeEnum.adBigInt), CStr(ADODB.DataTypeEnum.adUnsignedBigInt)
                    '    If TypeOf ctlTemp Is System.Windows.Forms.MaskedTextBox Then
                    '        Dim MyMaskedTextBox As MaskedTextBox
                    '        MyMaskedTextBox = ctlTemp
                    '        'If SqlDbType.Text Then ''ADODB.DataTypeEnum.adLongVarWChar <> ADODB.DataTypeEnum.adLongVarWChar Then
                    '        '    MyMaskedTextBox.MaxLength = rsName.Rows(0)(ctag).DefinedSize
                    '        'End If
                    '        MyMaskedTextBox.Text = 0
                    '    ElseIf TypeOf ctlTemp Is System.Windows.Forms.ComboBox Then
                    '        Dim MyComboBox As ComboBox
                    '        MyComboBox = ctlTemp
                    '        MyComboBox.SelectedItem = 0
                    '    ElseIf TypeOf ctlTemp Is System.Windows.Forms.CheckBox Then
                    '        Dim MyCheckBox As CheckBox
                    '        MyCheckBox = ctlTemp
                    '        MyCheckBox.Checked = False
                    '    Else
                    '        Dim MyTextBox As TextBox
                    '        MyTextBox = ctlTemp
                    '        'Number of some type
                    '        ctlTemp.Text = 0 ''CStr(0)
                    '        
                    '        ''  MyTextBox.MaxLength = rsName.Rows(0)(ctag).DefinedSize + 3
                    '    End If
                    'Case CStr(SqlDbType.DateTime) ''CStr(ADODB.DataTypeEnum.adDate)
                    '    'Date
                    '    Dim MyDTP As DateTimePicker
                    '    MyDTP = ctlTemp
                    '    MyDTP.Value = Today
                    'Case CStr(SqlDbType.Bit) ''"bit" ''CStr(SqlDbType.Bit)
                    '    'check box
                    '    Dim mycheckbox As CheckBox
                    '    If TypeOf ctlTemp Is System.Windows.Forms.CheckBox Then
                    '        mycheckbox = ctlTemp
                    '        mycheckbox.Checked = False
                    '    End If
                    'Case Else
                    '    Dim MyTextBox As TextBox
                    '    If TypeOf ctlTemp Is System.Windows.Forms.ComboBox Then
                    '        ' set the default to the first element in a combo box
                    '        
                    '        Dim MyComboBox As ComboBox
                    '        MyComboBox = ctlTemp
                    '        MyComboBox.SelectedItem = 0
                    '    Else
                    '        MyTextBox = ctlTemp
                    '        MyTextBox.Text = String.Empty
                    '    End If

                End If
                x += 1
            Loop
        End If
        ctag = ""

    End Sub

#End Region

#Region "RecInit Procs"

    '	Public Sub RecInit(ByRef frmName As System.Windows.Forms.Form, ByRef rsName As DataTable)
    '		'
    '		' clears any values from bound controls
    '		'
    '		' Inputs:
    '		'   frmName     name of form to initialize
    '		'
    '		' Outputs:
    '		'   RecInit     recOK if no errors
    '		'
    '		'Dim rsName As Recordset  'DataTable
    '		'Set rsName = rsInventory
    '		On Error GoTo RecInitErr

    '		Dim ctlTemp As System.Windows.Forms.Control
    '		Dim cTag As String
    '		'
    '		' Set up the default screen & clear values
    '		' also set the button
    '		For	Each ctlTemp In frmName.Controls
    '			cTag = UCase(Trim(ctlTemp.Tag))
    '			
    '			If TypeOf ctlTemp Is System.Windows.Forms.TextBox Then
    '				'if it is a display textbox only
    '				'for example for description of replaces/replacedby
    '				'descriptions for inventory screen
    '				'
    '				'clear out then fill with different data
    '				'if it is a field in the data table
    '				ctlTemp.Text = ""
    '			End If

    '			If Len(cTag) <> 0 Then
    '				
    '				If TypeOf ctlTemp Is System.Windows.Forms.TextBox Or TypeOf ctlTemp Is System.Windows.Forms.MaskedTextBox Then
    '					If rsName.Fields(cTag).Type = dao.DataTypeEnum.dbInteger Or rsName.Fields(cTag).Type = dao.DataTypeEnum.dbSingle Or rsName.Fields(cTag).Type = dao.DataTypeEnum.dbLong Or rsName.Fields(cTag).Type = dao.DataTypeEnum.dbDouble Then
    '						ctlTemp.Text = CStr(0)
    '						If cTag = "CONTROLID" Then
    '							'Doing this because Control ID 0, brings
    '							'up a machine.  Kinda bad
    '							ctlTemp.Text = ""
    '						End If
    '					ElseIf rsName.Fields(cTag).Type = dao.DataTypeEnum.dbDate Then 
    '						ctlTemp.Text = CStr(Today)
    '					Else
    '						
    '						ctlTemp.MaxLength = rsName.Fields(cTag).Size
    '						ctlTemp.Text = ""
    '					End If
    '					
    '				ElseIf TypeOf ctlTemp Is System.Windows.Forms.ComboBox Then 
    '					' set the default to the first element in a combo box
    '					
    '					ctlTemp.ListIndex = 0
    '					
    '				ElseIf TypeOf ctlTemp Is System.Windows.Forms.CheckBox Then 
    '					
    '					ctlTemp.Value = System.Windows.Forms.CheckState.Unchecked
    '				End If
    '			End If
    '		Next ctlTemp

    '		'    RecInit = True ' all ok
    '		GoTo RecInitExit

    'RecInitErr: 
    '		RecError(Err, ErrorToString(), "RecInit")
    '		'    RecInit = Err ' report error
    '		GoTo RecInitExit
    'RecInitExit: 
    '	End Sub


    '    Public Sub RecInit(ByRef frmName As System.Windows.Forms.Form, ByRef tblName As String, ByRef rsName As ADODB.Recordset)
    '        '
    '        ' clears any values from bound controls
    '        '
    '        ' Inputs:
    '        '   frmName     name of form to initialize
    '        '
    '        ' Outputs:
    '        '   RecInit     recOK if no errors
    '        '
    '        On Error GoTo RecInitErr

    '        Dim ctlTemp As System.Windows.Forms.Control
    '        Dim cTag As String
    '        Dim ctlTempGroup As System.Windows.Forms.Control
    '        Dim nbrFieldsInSql As Int32
    '        Dim blnFound As Boolean
    '        Dim sqlString As String
    '        Dim rs As New ADODB.Recordset
    '        Dim x, nbrMaxFields As Int32
    '        Dim nbrControls As Int32

    '        x = 0
    '        rs = cnDSI.OpenSchema(ADODB.SchemaEnum.adSchemaColumns, New Object() {Nothing, Nothing, tblName, Nothing})

    '        Do While Not rs.EOF
    '            Col_Type(x, 0) = UCase(rs.Fields("column_name").Value)
    '            cTag = rs.Fields("column_name").Value
    '            Col_Type(x, 1) = CStr(rsName.Fields(cTag).Type)
    '            rs.MoveNext()
    '            x = x + 1
    '        Loop
    '        nbrMaxFields = x - 1
    '        x = 0
    '        For Each ctlTemp In frmName.Controls
    '            cTag = UCase(Trim(ctlTemp.Tag))
    '            If TypeOf ctlTemp Is System.Windows.Forms.GroupBox Then
    '                ctlTempGroup = Nothing
    '                For Each ctlTempGroup In ctlTemp.Controls
    '                    x = x + 1
    '                    cTag = UCase(Trim(ctlTempGroup.Tag))
    '                    Debug.Print(CStr(x) & " " & cTag & "  -  " & ctlTempGroup.Name)
    '                    '                    ctltempGroup.Text = "x"
    '                    If Len(cTag) > 0 Then
    '                        RecInit_Element(cTag, ctlTempGroup, rsName, nbrMaxFields)
    '                    End If
    '                Next ctlTempGroup
    '            End If
    '            x = x + 1
    '            cTag = UCase(Trim(ctlTemp.Tag))
    '            Debug.Print(CStr(x) & " " & cTag & "  -  " & ctlTemp.Name)
    '            If Len(cTag) > 0 Then
    '                RecInit_Element(cTag, ctlTemp, rsName, nbrMaxFields)
    '            End If
    '        Next ctlTemp
    '        '    RecInit = True ' all ok
    '        GoTo RecInitExit

    'RecInitErr:
    '        RecError(Err, ErrorToString(), "RecInit")
    '        '    RecInit = Err ' report error
    '        GoTo RecInitExit
    'RecInitExit:
    '        rs = Nothing
    '    End Sub

    Public Sub RecInit_Element(ByRef ctag As String, ByRef ctltemp As System.Windows.Forms.Control, ByRef rsName As ADODB.Recordset, ByRef nbrMaxFields As Int32)
        Dim blnFound As Boolean
        Dim x As Int32

        If Len(ctag) <> 0 Then
            blnFound = False

            If TypeOf ctltemp Is System.Windows.Forms.TextBox Then
                'if it is a display textbox only
                'for example for description of replaces/replacedby
                'descriptions for inventory screen
                '
                'clear out then fill with different data
                'if it is a field in the data table
                'ctlTemp.Text = ""
                Dim MyTextBox As TextBox
                MyTextBox = ctltemp
                MyTextBox.Text = String.Empty
                'CType(frmName.ctlTemp, TextBox).Text = ""
                'frmName.Controls(ctlTemp.Name).Text = ""

            End If
            x = 0
            Do While (x <= nbrMaxFields And Not blnFound)
                If ctag = Col_Type(x, 0) Then
                    'Debug.Print rs!column_name & rsName.Fields(cTag).Type
                    blnFound = True
                    Select Case Col_Type(x, 1)
                        Case CStr(ADODB.DataTypeEnum.adVarWChar), CStr(ADODB.DataTypeEnum.adLongVarChar), CStr(ADODB.DataTypeEnum.adLongVarWChar), CStr(ADODB.DataTypeEnum.adChar), CStr(ADODB.DataTypeEnum.adWChar)
                            'String
                            If TypeOf ctltemp Is System.Windows.Forms.ComboBox Then
                                Dim MyComboBox As ComboBox
                                MyComboBox = ctltemp
                                MyComboBox.SelectedItem = 0
                            Else
                                Dim MyTextBox As TextBox
                                MyTextBox = ctltemp
                                If ADODB.DataTypeEnum.adLongVarWChar <> ADODB.DataTypeEnum.adLongVarWChar Then
                                    MyTextBox.MaxLength = rsName.Fields(ctag).DefinedSize

                                End If
                                MyTextBox.Text = String.Empty
                            End If
                        Case CStr(ADODB.DataTypeEnum.adSmallInt), CStr(ADODB.DataTypeEnum.adInteger), CStr(ADODB.DataTypeEnum.adSingle), CStr(ADODB.DataTypeEnum.adDouble), CStr(ADODB.DataTypeEnum.adCurrency), CStr(ADODB.DataTypeEnum.adDecimal), CStr(ADODB.DataTypeEnum.adTinyInt), CStr(ADODB.DataTypeEnum.adUnsignedTinyInt), CStr(ADODB.DataTypeEnum.adUnsignedSmallInt), CStr(ADODB.DataTypeEnum.adUnsignedInt), CStr(ADODB.DataTypeEnum.adBigInt), CStr(ADODB.DataTypeEnum.adUnsignedBigInt)
                            Dim MyTextBox As TextBox
                            MyTextBox = ctltemp
                            'Number of some type
                            ctltemp.Text = CStr(0)

                            MyTextBox.MaxLength = rsName.Fields(ctag).DefinedSize + 3

                        Case CStr(ADODB.DataTypeEnum.adDate)
                            'Date
                            Dim MyTextBox As TextBox
                            MyTextBox = ctltemp
                            ctltemp.Text = CStr(#1/1/1980#)
                        Case Else
                            Dim MyTextBox As TextBox
                            If TypeOf ctltemp Is System.Windows.Forms.ComboBox Then
                                ' set the default to the first element in a combo box
                                Dim MyComboBox As ComboBox
                                MyComboBox = ctltemp
                                MyComboBox.SelectedItem = 0
                            Else
                                MyTextBox = ctltemp
                                MyTextBox.Text = String.Empty
                            End If
                    End Select
                End If
                x += 1
            Loop
        End If

    End Sub


    Public Sub RecInitSQL(ByRef frmName As System.Windows.Forms.Form, ByRef tblName As String, ByRef rsName As DataTable)
        '
        ' clears any values from bound controls
        '
        ' Inputs:
        '   frmName     name of form to initialize
        '
        ' Outputs:
        '   RecInit     recOK if no errors
        '
        ''On Error GoTo RecInitErr

        Dim ctlTemp As System.Windows.Forms.Control
        Dim cTag As String
        Dim ctlTempGroup As System.Windows.Forms.Control
        Dim ctltab As System.Windows.Forms.TabControl
        Dim sqlString As String
        Dim rs As New DataTable ''ADODB.Recordset
        Dim x, nbrMaxFields As Int32

        Dim tempDR As SqlClient.SqlDataReader
        Dim tempCMD As New SqlClient.SqlCommand()

        Try


            x = 0
            tblName = "[" & tblName & "]"
            'rs = cnDSI.OpenSchema(ADODB.SchemaEnum.adSchemaColumns, New Object() {Nothing, Nothing, tblName, Nothing})
            sqlString = "select * from " & tblName & " where 1 = 0"
            tempCMD = New SqlCommand(sqlString, cnSQL)
            tempDR = tempCMD.ExecuteReader(CommandBehavior.KeyInfo)
            rs = tempDR.GetSchemaTable()
            tempDR.Close()
            tempCMD.Dispose()

            'Do While Not rs.EOF
            '    
            '    Col_Type(x, 0) = UCase(rs.Fields("column_name").Value)
            '    cTag = rs.Fields("column_name").Value
            '    
            '    Col_Type(x, 1) = CStr(rsName.Fields(cTag).Type)
            '    rs.MoveNext()
            '    
            '    x = x + 1
            'Loop
            For Each dr As DataRow In rs.Rows
                Col_Type(x, 0) = UCase(dr("ColumnName").ToString)
                cTag = dr("ColumnName").ToString

                Col_Type(x, 1) = dr("ProviderType").ToString ''dr("ProviderSpecificDataType").ToString ''dr("DataTypeName").ToString ' could be 24 +		System.Data.DataColumn	{ProviderSpecificDataType}	System.Data.DataColumn
                Col_Type(x, 2) = dr("ProviderSpecificDataType").ToString
                x += 1
            Next


            nbrMaxFields = x - 1
            x = 0
            For Each ctlTemp In frmName.Controls
                cTag = UCase(Trim(ctlTemp.Tag))
                If TypeOf ctlTemp Is System.Windows.Forms.TabControl Then
                    ctltab = ctlTemp
                    For Each tp As TabPage In ctltab.TabPages
                        For Each ctl As Control In tp.Controls
                            If TypeOf ctl Is System.Windows.Forms.GroupBox Then
                                'MessageBox.Show(ctl.Controls.Count.ToString())
                                ctlTempGroup = Nothing
                                For Each ctlTempGroup In ctl.Controls
                                    x += 1
                                    cTag = UCase(Trim(ctlTempGroup.Tag))
                                    'If cTag = "ComboMeter" Then
                                    '    MessageBox.Show("ComboMeter")
                                    'End If

                                    ''Debug.Print(CStr(x) & " " & cTag & "  -  " & ctlTempGroup.Name)
                                    If Len(cTag) > 0 Then
                                        RecInit_ElementSQL(cTag, ctlTempGroup, rsName, nbrMaxFields)
                                    End If
                                Next ctlTempGroup
                            Else
                                cTag = UCase(Trim(ctl.Tag))
                                If Len(cTag) > 0 Then
                                    RecInit_ElementSQL(cTag, ctl, rsName, nbrMaxFields)
                                End If

                            End If
                        Next
                    Next
                End If
                If TypeOf ctlTemp Is System.Windows.Forms.GroupBox Then
                    ctlTempGroup = Nothing
                    For Each ctlTempGroup In ctlTemp.Controls
                        x += 1
                        cTag = UCase(Trim(ctlTempGroup.Tag))
                        ''Debug.Print(CStr(x) & " " & cTag & "  -  " & ctlTempGroup.Name)
                        '                    ctltempGroup.Text = "x"
                        If Len(cTag) > 0 Then
                            RecInit_ElementSQL(cTag, ctlTempGroup, rsName, nbrMaxFields)
                        End If
                    Next ctlTempGroup
                End If
                x += 1
                cTag = UCase(Trim(ctlTemp.Tag))
                ''Debug.Print(CStr(x) & " " & cTag & "  -  " & ctlTemp.Name)
                If Len(cTag) > 0 Then
                    RecInit_ElementSQL(cTag, ctlTemp, rsName, nbrMaxFields)
                End If
            Next ctlTemp
            '    RecInit = True ' all ok
            'GoTo RecInitExit
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        'RecInitErr:
        '       RecError(Err, ErrorToString(), "RecInit")
        '    RecInit = Err ' report error
        '      GoTo RecInitExit
        'RecInitExit:
        'UPGRADE_NOTE: Object rs may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '       rs = Nothing
    End Sub

    Public Sub RecInit_ElementSQL(ByRef ctag As String, ByRef ctltemp As System.Windows.Forms.Control, ByRef rsName As DataTable, ByRef nbrMaxFields As Int32)
        Dim blnFound As Boolean
        Dim x As Int32
        Try

            If Len(ctag) <> 0 Then
                blnFound = False

                If TypeOf ctltemp Is System.Windows.Forms.TextBox Then
                    'if it is a display textbox only
                    'for example for description of replaces/replacedby
                    'descriptions for inventory screen
                    '
                    'clear out then fill with different data
                    'if it is a field in the data table
                    'ctlTemp.Text = ""
                    Dim MyTextBox As TextBox
                    MyTextBox = ctltemp
                    MyTextBox.Text = String.Empty

                End If

                x = 0


                Do While (x <= nbrMaxFields And Not blnFound)


                    If ctag = Col_Type(x, 0) Then
                        'Debug.Print rs!column_name & rsName.Fields(cTag).Type
                        blnFound = True

                        Select Case Col_Type(x, 1)
                            '' ADODB.DataTypeEnum.adVarWChar), CStr(ADODB.DataTypeEnum.adLongVarChar), CStr(ADODB.DataTypeEnum.adLongVarWChar), CStr(ADODB.DataTypeEnum.adChar), CStr(ADODB.DataTypeEnum.adWChar)
                            Case CStr(SqlDbType.VarChar)
                                'String

                                If TypeOf ctltemp Is System.Windows.Forms.ComboBox Then

                                    Dim MyComboBox As ComboBox
                                    MyComboBox = ctltemp
                                    MyComboBox.SelectedItem = 0
                                ElseIf TypeOf ctltemp Is System.Windows.Forms.MaskedTextBox Then
                                    Dim MyMaskedTextBox As MaskedTextBox
                                    MyMaskedTextBox = ctltemp
                                    'If SqlDbType.Text Then ''ADODB.DataTypeEnum.adLongVarWChar <> ADODB.DataTypeEnum.adLongVarWChar Then
                                    '    MyMaskedTextBox.MaxLength = rsName.Rows(0)(ctag).DefinedSize

                                    'End If
                                    MyMaskedTextBox.Text = String.Empty
                                ElseIf TypeOf ctltemp Is System.Windows.Forms.CheckBox Then
                                    Dim MyCheckBox As CheckBox
                                    MyCheckBox = ctltemp
                                    MyCheckBox.Checked = False
                                Else
                                    Dim MyTextBox As TextBox
                                    MyTextBox = ctltemp
                                    'If SqlDbType.Text Then ''ADODB.DataTypeEnum.adLongVarWChar <> ADODB.DataTypeEnum.adLongVarWChar Then
                                    '    
                                    '    MyTextBox.MaxLength = rsName.Rows(0)(ctag).DefinedSize

                                    'End If
                                    MyTextBox.Text = String.Empty
                                End If
                            Case CStr(SqlDbType.SmallInt), CStr(SqlDbType.Int), CStr(SqlDbType.Real), CStr(SqlDbType.Float), CStr(SqlDbType.Money), CStr(SqlDbType.Decimal), CStr(SqlDbType.TinyInt), CStr(SqlDbType.BigInt)
                                ''CStr(ADODB.DataTypeEnum.adSmallInt), CStr(ADODB.DataTypeEnum.adInteger), CStr(ADODB.DataTypeEnum.adSingle), CStr(ADODB.DataTypeEnum.adDouble), CStr(ADODB.DataTypeEnum.adCurrency), 
                                ''CStr(ADODB.DataTypeEnum.adDecimal), CStr(ADODB.DataTypeEnum.adTinyInt), CStr(ADODB.DataTypeEnum.adUnsignedTinyInt), CStr(ADODB.DataTypeEnum.adUnsignedSmallInt), 
                                ''CStr(ADODB.DataTypeEnum.adUnsignedInt), CStr(ADODB.DataTypeEnum.adBigInt), CStr(ADODB.DataTypeEnum.adUnsignedBigInt)
                                If TypeOf ctltemp Is System.Windows.Forms.MaskedTextBox Then
                                    Dim MyMaskedTextBox As MaskedTextBox
                                    MyMaskedTextBox = ctltemp
                                    'If SqlDbType.Text Then ''ADODB.DataTypeEnum.adLongVarWChar <> ADODB.DataTypeEnum.adLongVarWChar Then
                                    '    MyMaskedTextBox.MaxLength = rsName.Rows(0)(ctag).DefinedSize
                                    'End If
                                    MyMaskedTextBox.Text = 0
                                ElseIf TypeOf ctltemp Is System.Windows.Forms.ComboBox Then
                                    Dim MyComboBox As ComboBox
                                    MyComboBox = ctltemp
                                    MyComboBox.SelectedItem = 0
                                ElseIf TypeOf ctltemp Is System.Windows.Forms.CheckBox Then
                                    Dim MyCheckBox As CheckBox
                                    MyCheckBox = ctltemp
                                    MyCheckBox.Checked = False
                                Else
                                    Dim MyTextBox As TextBox
                                    MyTextBox = ctltemp
                                    'Number of some type
                                    ctltemp.Text = String.Empty ''CStr(0)

                                    ''  MyTextBox.MaxLength = rsName.Rows(0)(ctag).DefinedSize + 3
                                End If
                            Case CStr(SqlDbType.DateTime) ''CStr(ADODB.DataTypeEnum.adDate)
                                'Date
                                Dim MyDTP As DateTimePicker
                                MyDTP = ctltemp
                                MyDTP.Value = Today
                            Case CStr(SqlDbType.Bit) ''"bit" ''CStr(SqlDbType.Bit)
                                'check box
                                Dim mycheckbox As CheckBox
                                If TypeOf ctltemp Is System.Windows.Forms.CheckBox Then
                                    mycheckbox = ctltemp
                                    mycheckbox.Checked = False
                                End If
                            Case Else
                                Dim MyTextBox As TextBox
                                If TypeOf ctltemp Is System.Windows.Forms.ComboBox Then
                                    ' set the default to the first element in a combo box

                                    Dim MyComboBox As ComboBox
                                    MyComboBox = ctltemp
                                    MyComboBox.SelectedItem = 0
                                Else
                                    MyTextBox = ctltemp
                                    MyTextBox.Text = String.Empty
                                End If
                        End Select
                    End If


                    x += 1
                Loop
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
#End Region

    'Public Sub RecClear(ByRef frmName As System.Windows.Forms.Form, ByRef rsName As DataTable)
    '    '
    '    ' clears any values from bound controls
    '    '
    '    ' Inputs:
    '    '   frmName     name of form to initialize
    '    '
    '    ' Outputs:
    '    '   RecInit     recOK if no errors
    '    '
    '    On Error Resume Next

    '    Dim ctlTemp As System.Windows.Forms.Control
    '    Dim cTag As String
    '    '
    '    ' Set up the default screen & clear values
    '    ' also set the button
    '    For Each ctlTemp In frmName.Controls
    '        cTag = UCase(Trim(ctlTemp.Tag))
    '        
    '        If TypeOf ctlTemp Is System.Windows.Forms.TextBox Then
    '            'if it is a display textbox only
    '            'for example for description of replaces/replacedby
    '            'descriptions for inventory screen
    '            '
    '            'clear out then fill with different data
    '            'if it is a field in the data table
    '            ctlTemp.Text = ""
    '        End If

    '        If Len(cTag) <> 0 Then
    '            
    '            If TypeOf ctlTemp Is System.Windows.Forms.TextBox Or TypeOf ctlTemp Is System.Windows.Forms.MaskedTextBox Then
    '                If rsName.Fields(cTag).Type = dao.DataTypeEnum.dbInteger Or rsName.Fields(cTag).Type = dao.DataTypeEnum.dbSingle Or rsName.Fields(cTag).Type = dao.DataTypeEnum.dbLong Or rsName.Fields(cTag).Type = dao.DataTypeEnum.dbDouble Then
    '                    ctlTemp.Text = CStr(0)
    '                    If cTag = "CONTROLID" Then
    '                        'Doing this because Control ID 0, brings
    '                        'up a machine.  Kinda bad
    '                        ctlTemp.Text = ""
    '                    End If
    '                ElseIf rsName.Fields(cTag).Type = dao.DataTypeEnum.dbDate Then
    '                    ctlTemp.Text = CStr(Today)
    '                Else
    '                    
    '                    Dim MyTextBox As TextBox
    '                    MyTextBox = ctlTemp
    '                    MyTextBox.MaxLength = rsName.Fields(cTag).Size
    '                    MyTextBox.Text = ""
    '                End If
    '                
    '            ElseIf TypeOf ctlTemp Is System.Windows.Forms.ComboBox Then
    '                ' set the default to the first element in a combo box
    '                
    '                Dim MyComboBox As ComboBox
    '                MyComboBox = ctlTemp
    '                MyComboBox.SelectedItem = 0
    '                
    '            ElseIf TypeOf ctlTemp Is System.Windows.Forms.CheckBox Then
    '                
    '                Dim MyCheckBox As CheckBox
    '                MyCheckBox = ctlTemp
    '                MyCheckBox.Checked = System.Windows.Forms.CheckState.Unchecked
    '            End If
    '        End If
    '    Next ctlTemp
    'End Sub
#Region "recRead Procs"
    '    Function recRead(ByRef frmName As System.Windows.Forms.Form, ByRef rsName As DataTable) As Short
    '        '
    '        ' read a record of data and update the controls
    '        '
    '        ' Inputs:
    '        '   frmName     Name of form to load
    '        '   rsName      Name of recordset to read
    '        '
    '        ' Outputs:
    '        '   RecRead     recOk - if no errors
    '        '
    '        On Error GoTo RecReadErr

    '        Dim ctlTemp As System.Windows.Forms.Control
    '        Dim cTag As String
    '        Dim cfldname As String
    '        Dim x As Short


    '        On Error Resume Next

    '        For Each ctlTemp In frmName.Controls
    '            cTag = UCase(Trim(ctlTemp.Tag))
    '            If Len(cTag) <> 0 Then
    '                
    '                
    '                If IsDBNull(rsName.Fields(cTag).Value) And (TypeOf ctlTemp Is System.Windows.Forms.TextBox) Then
    '                    
    '                    If TypeOf ctlTemp Is System.Windows.Forms.ComboBox Then
    '                        
    '                        'ctlTemp.ListIndex = 0
    '                        Dim MyComboBox As ComboBox
    '                        MyComboBox = ctlTemp
    '                        MyComboBox.SelectedItem = 0
    '                    Else
    '                        ctlTemp.Text = ""
    '                    End If
    '                Else
    '                    
    '                    If TypeOf ctlTemp Is System.Windows.Forms.ComboBox Then
    '                        'ctlTemp.ListIndex = 0
    '                        
    '                        Dim MyComboBox As ComboBox
    '                        MyComboBox = ctlTemp
    '                        For x = 0 To MyComboBox.SelectedItem - 1
    '                            
    '                            If Trim(rsName.Fields(cTag).Value) = MyComboBox.Items(x) Then
    '                                
    '                                MyComboBox.SelectedItem = x
    '                            End If
    '                        Next
    '                        
    '                    ElseIf TypeOf ctlTemp Is System.Windows.Forms.CheckBox Then
    '                        If rsName.Fields(cTag).Value = True Then
    '                            
    '                            Dim MyCheckBox As CheckBox
    '                            MyCheckBox = ctlTemp
    '                            MyCheckBox.Checked = System.Windows.Forms.CheckState.Unchecked
    '                            'ctlTemp.Value = System.Windows.Forms.CheckState.Checked
    '                        Else
    '                            
    '                            Dim MyCheckBox As CheckBox
    '                            MyCheckBox = ctlTemp
    '                            MyCheckBox.Checked = System.Windows.Forms.CheckState.Unchecked
    '                            'ctlTemp.Value = System.Windows.Forms.CheckState.Unchecked
    '                        End If
    '                    Else
    '                        'I am getting an error using the dtDate fields
    '                        'They do not have the properties as an normal Date field.
    '                        'so I am going to trap for errors myself and try to overcome
    '                        'this.  I would like for this code to do all the forms
    '                        '                    On Error GoTo 0
    '                        '                    On Error Resume Next
    '                        ctlTemp.Text = rsName.Fields(cTag).Value
    '                        '                    If Err.Number = 438 Then
    '                        '                        ctlTemp.Value = rsName.Fields(cTag)
    '                        '                    End If
    '                        '                    On Error GoTo RecReadErr
    '                        ' ctlTemp.MaxLength = rsName.Fields(cTag).Size
    '                        If rsName.Fields(cTag).Type = dao.DataTypeEnum.dbLong Or rsName.Fields(cTag).Type = dao.DataTypeEnum.dbSingle Or rsName.Fields(cTag).Type = dao.DataTypeEnum.dbDouble Or rsName.Fields(cTag).Type = dao.DataTypeEnum.dbInteger Then
    '                            
    '                            ctlTemp.MaxLength = 0
    '                            ctlTemp.Text = CStr(CSng(rsName.Fields(cTag).Value))
    '                        ElseIf rsName.Fields(cTag).Type = dao.DataTypeEnum.dbDate Then
    '                            'ctlTemp.MaxLength = 10
    '                            
    '                            ctlTemp.Value = rsName.Fields(cTag).Value
    '                        Else
    '                            
    '                            ctlTemp.MaxLength = rsName.Fields(cTag).Size
    '                        End If
    '                        '                    Debug.Print "Field " & cTag & ", value " & ctlTemp.Text
    '                    End If
    '                End If
    '                ctlTemp.ForeColor = UserForeColor
    '            End If
    '        Next ctlTemp
    '        recRead = True ' all ok
    '        GoTo RecReadExit

    'RecReadErr:
    '        RecError(Err, ErrorToString(), "RecRead")
    '        recRead = Err.Number
    '        GoTo RecReadExit
    'RecReadExit:
    '    End Function

    '    Function recRead(ByRef frmName As System.Windows.Forms.Form, ByRef rsName As ADODB.Recordset) As Short
    '        '
    '        ' read a record of data and update the controls
    '        '
    '        ' Inputs:
    '        '   frmName     Name of form to load
    '        '   rsName      Name of recordset to read
    '        '
    '        ' Outputs:
    '        '   RecRead     recOk - if no errors
    '        '
    '        On Error GoTo RecReadErr

    '        Dim ctlTemp, ctlTempGroup As System.Windows.Forms.Control
    '        Dim cTag As String
    '        Dim cfldname As String
    '        Dim x As Short

    '        For Each ctlTemp In frmName.Controls
    '            cTag = UCase(Trim(ctlTemp.Tag)) '  added tostring()
    '            If TypeOf ctlTemp Is System.Windows.Forms.GroupBox Then
    '                ctlTempGroup = Nothing
    '                For Each ctlTempGroup In ctlTemp.Controls
    '                    x = x + 1
    '                    cTag = UCase(Trim(ctlTempGroup.Tag))
    '                    ''Debug.Print(CStr(x) & " " & cTag & "  -  " & ctlTempGroup.Name)
    '                    If Len(cTag) > 0 Then
    '                        RecRead_Element(cTag, ctlTempGroup, rsName)
    '                    End If
    '                Next ctlTempGroup
    '            Else
    '                If Len(cTag) > 0 Then
    '                    RecRead_Element(cTag, ctlTemp, rsName)
    '                End If
    '            End If
    '        Next ctlTemp
    '        recRead = True ' all ok
    '        GoTo RecReadExit

    'RecReadErr:
    '        RecError(Err, ErrorToString(), "RecRead")
    '        recRead = Err.Number
    '        GoTo RecReadExit
    'RecReadExit:
    '    End Function

    'Private Sub RecRead_Element(ByRef ctag As String, ByRef ctlTemp As System.Windows.Forms.Control, ByVal rsName As ADODB.Recordset)
    '    Dim x As Int16

    '    If Len(ctag) <> 0 Then
    '        
    '        
    '        If IsDBNull(rsName.Fields(ctag).Value) And (TypeOf ctlTemp Is System.Windows.Forms.TextBox) Then
    '            
    '            If TypeOf ctlTemp Is System.Windows.Forms.ComboBox Then
    '                
    '                Dim MyComboBox As ComboBox
    '                MyComboBox = ctlTemp
    '                MyComboBox.SelectedIndex = 0

    '            Else
    '                ctlTemp.Text = String.Empty
    '            End If
    '        Else
    '            
    '            If TypeOf ctlTemp Is System.Windows.Forms.ComboBox Then
    '                
    '                Dim MyComboBox As ComboBox
    '                MyComboBox = ctlTemp
    '                MyComboBox.SelectedIndex = 0

    '                
    '                For x = 0 To MyComboBox.Items.Count() - 1
    '                    
    '                    'If Trim(UCase(rsName.Fields(cTag).Value)) = Trim(UCase(ctlTemp.List(x))) Then
    '                    If Trim(UCase(rsName.Fields(ctag).Value)) = Trim(UCase(MyComboBox.Items(x))) Then
    '                        
    '                        MyComboBox.SelectedIndex = x
    '                    End If
    '                Next
    '            Else
    '                ctlTemp.Text = Trim(rsName.Fields(ctag).Value)
    '                ctlTemp.Text = Replace(ctlTemp.Text, "^", "'")
    '                If rsName.Fields(ctag).Type = ADODB.DataTypeEnum.adInteger Then ' dao.DataTypeEnum.dbSingle Then
    '                    ctlTemp.Text = Trim(CStr(CSng(rsName.Fields(ctag).Value)))
    '                ElseIf rsName.Fields(ctag).Type = ADODB.DataTypeEnum.adDBDate Then ' dao.DataTypeEnum.dbDate Then
    '                Else
    '                End If
    '            End If
    '        End If
    '    End If
    'End Sub

    Function recReadSQL(ByRef frmName As System.Windows.Forms.Form, ByRef rsName As DataTable) As Boolean
        '
        ' read a record of data and update the controls
        '
        ' Inputs:
        '   frmName     Name of form to load
        '   rsName      Name of recordset to read
        '
        ' Outputs:
        '   RecRead     recOk - if no errors
        '
        'On Error GoTo RecReadErr
        Dim ctlTemp, ctlTempGroup As System.Windows.Forms.Control
        Dim ctltab As System.Windows.Forms.TabControl
        Dim cTag As String
        Dim x As Short
        Try
            For Each ctlTemp In frmName.Controls
                cTag = UCase(Trim(ctlTemp.Tag)) '  added tostring()
                'If cTag = "ComboMeter" Or ctlTemp.Text = "Base Contract Info" Or ctlTemp.Name = "Frame4" Then
                '    MessageBox.Show("ComboMeter")
                'End If

                If TypeOf ctlTemp Is System.Windows.Forms.TabControl Then
                    ctltab = ctlTemp
                    For Each tp As TabPage In ctltab.TabPages
                        For Each ctl As Control In tp.Controls
                            If TypeOf ctl Is System.Windows.Forms.GroupBox Then
                                'MessageBox.Show(ctl.Controls.Count.ToString())
                                ctlTempGroup = Nothing
                                For Each ctlTempGroup In ctl.Controls
                                    x += 1
                                    cTag = UCase(Trim(ctlTempGroup.Tag))
                                    'If cTag = "ComboMeter" Then
                                    '    MessageBox.Show("ComboMeter")
                                    'End If

                                    ''Debug.Print(CStr(x) & " " & cTag & "  -  " & ctlTempGroup.Name)
                                    If Len(cTag) > 0 Then
                                        RecRead_ElementSQL(cTag, ctlTempGroup, rsName)
                                    End If
                                Next ctlTempGroup
                            Else
                                cTag = UCase(Trim(ctl.Tag))
                                If Len(cTag) > 0 Then
                                    RecRead_ElementSQL(cTag, ctl, rsName)
                                End If
                            End If
                        Next
                    Next
                ElseIf TypeOf ctlTemp Is System.Windows.Forms.GroupBox Then
                    ctlTempGroup = Nothing
                    For Each ctlTempGroup In ctlTemp.Controls
                        x += 1
                        cTag = UCase(Trim(ctlTempGroup.Tag))
                        'If cTag = "ComboMeter" Then
                        '    MessageBox.Show("ComboMeter")
                        'End If

                        ''Debug.Print(CStr(x) & " " & cTag & "  -  " & ctlTempGroup.Name)
                        If Len(cTag) > 0 Then
                            RecRead_ElementSQL(cTag, ctlTempGroup, rsName)
                        End If
                    Next ctlTempGroup
                Else
                    If Len(cTag) > 0 Then
                        RecRead_ElementSQL(cTag, ctlTemp, rsName)
                    End If
                End If
            Next ctlTemp
            recReadSQL = True ' all ok

        Catch ex As Exception
            MsgBox(ex.Message)

            'RecError(Err, ErrorToString(), "RecRead")
            ''recReadSQL = Err.Number
            'recReadSQL = False

        End Try

        '        GoTo RecReadExit

        'RecReadErr:
        '        RecError(Err, ErrorToString(), "RecRead")
        '        'recReadSQL = Err.Number
        '        recReadSQL = False
        '        GoTo RecReadExit
        'RecReadExit:
    End Function

    Private Sub RecRead_ElementSQL(ByRef ctag As String, ByRef ctlTemp As System.Windows.Forms.Control, ByVal rsName As DataTable)
        Dim x As Int16
        Try
            If Len(ctag) <> 0 Then

                If IsDBNull(rsName.Rows(0)(ctag)) And (TypeOf ctlTemp Is System.Windows.Forms.TextBox) Then
                    If TypeOf ctlTemp Is System.Windows.Forms.ComboBox Then
                        Dim MyComboBox As ComboBox
                        MyComboBox = ctlTemp
                        MyComboBox.SelectedIndex = 0
                    ElseIf TypeOf ctlTemp Is System.Windows.Forms.DateTimePicker Then
                        Dim myDTP As DateTimePicker
                        myDTP = ctlTemp
                        myDTP = rsName.Rows(0)(ctag)
                    Else
                        ctlTemp.Text = String.Empty
                    End If
                Else
                    If TypeOf ctlTemp Is System.Windows.Forms.ComboBox Then
                        Dim MyComboBox As ComboBox
                        MyComboBox = ctlTemp
                        MyComboBox.SelectedIndex = 0

                        For x = 0 To MyComboBox.Items.Count() - 1
                            If Trim(UCase(rsName.Rows(0)(ctag).ToString)) = Trim(UCase(MyComboBox.Items(x))) Then
                                MyComboBox.SelectedIndex = x
                            End If
                        Next
                    ElseIf TypeOf ctlTemp Is System.Windows.Forms.CheckBox Then
                        Dim MyCheckBox As CheckBox
                        MyCheckBox = ctlTemp
                        If rsName.Rows(0)(ctag) = True Then
                            MyCheckBox.Checked = True
                        Else
                            MyCheckBox.Checked = False
                        End If
                    ElseIf TypeOf ctlTemp Is System.Windows.Forms.DateTimePicker Then
                        Dim myDTP As DateTimePicker
                        myDTP = ctlTemp
                        myDTP.Value = IIf(IsDBNull(rsName.Rows(0)(ctag)) = True, Today, rsName.Rows(0)(ctag))
                    Else
                        ctlTemp.Text = Trim(rsName.Rows(0)(ctag).ToString)
                        ctlTemp.Text = Replace(ctlTemp.Text, "^", "'")
                        If rsName.Rows(0)(ctag).GetType.ToString() = "System.Decimal" Then ''CStr(SqlDbType.Decimal) Then  ''dao.DataTypeEnum.dbSingle Then
                            ctlTemp.Text = Trim(CStr(CSng(rsName.Rows(0)(ctag).ToString)))
                        ElseIf rsName.Rows(0)(ctag).GetType.ToString() = "System.Date" Then  ''dao.DataTypeEnum.dbDate Then
                        Else
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub
#End Region

    Function recColor(ByRef frmName As System.Windows.Forms.Form) As Short
        '
        ' Set the color to the users choice
        '
        ' Inputs:
        '   frmName     Name of form to load
        '
        ' Outputs:
        '   RecColor     recOk - if no errors
        '
        On Error GoTo RecReadErr

        Dim ctlTemp, ctlTempGroup As System.Windows.Forms.Control
        Dim ctltab As System.Windows.Forms.TabControl
        Dim cTag As String
        Dim cfldname As String
        Dim x As Short
        'IIf(glbUserName = "EDNA", 12, 8)
        Dim fBold As New Font("Arial", glbFontSize, FontStyle.Bold) ''Arial, 8pt

        On Error Resume Next

        For Each ctlTemp In frmName.Controls

            'Debug.Print("RecColor: " & ctlTemp.Name.ToString)
            If TypeOf ctlTemp Is System.Windows.Forms.GroupBox Then
                ctlTempGroup = Nothing
                For Each ctlTempGroup In ctlTemp.Controls
                    'Debug.Print("RecColor: " & ctlTemp.Name.ToString & " " & ctlTempGroup.Name.ToString)
                    If TypeOf ctlTempGroup Is System.Windows.Forms.TextBox Then
                        Dim MyTextBox As TextBox
                        MyTextBox = ctlTempGroup
                        MyTextBox.ForeColor = UserForeColor
                        MyTextBox.Font = fBold 'VB6.FontChangeBold(ctlTemp.Font, True)
                    End If
                    If TypeOf ctlTempGroup Is System.Windows.Forms.ComboBox Then
                        Dim MyComboBox As ComboBox
                        MyComboBox = ctlTempGroup
                        MyComboBox.ForeColor = UserForeColor
                        MyComboBox.Font = fBold 'VB6.FontChangeBold(ctlTemp.Font, True)
                    End If
                    If TypeOf ctlTempGroup Is System.Windows.Forms.CheckBox Then
                        Dim MyCheckBox As CheckBox
                        MyCheckBox = ctlTempGroup
                        MyCheckBox.ForeColor = UserForeColor
                        MyCheckBox.Font = fBold 'VB6.FontChangeBold(ctlTemp.Font, True)
                    End If
                Next
            ElseIf TypeOf ctlTemp Is System.Windows.Forms.TabControl Then
                ctltab = ctlTemp
                    For Each tp As TabPage In ctltab.TabPages
                    For Each ctl As Control In tp.Controls
                        If TypeOf ctl Is System.Windows.Forms.TextBox Then
                            Dim MyTextBox As TextBox
                            MyTextBox = ctl
                            MyTextBox.ForeColor = UserForeColor
                            MyTextBox.Font = fBold 'VB6.FontChangeBold(ctlTemp.Font, True)
                        End If
                        If TypeOf ctl Is System.Windows.Forms.ComboBox Then
                            Dim MyComboBox As ComboBox
                            MyComboBox = ctl
                            MyComboBox.ForeColor = UserForeColor
                            MyComboBox.Font = fBold 'VB6.FontChangeBold(ctlTemp.Font, True)
                        End If
                        If TypeOf ctl Is System.Windows.Forms.CheckBox Then
                            Dim MyCheckBox As CheckBox
                            MyCheckBox = ctl
                            MyCheckBox.ForeColor = UserForeColor
                            MyCheckBox.Font = fBold 'VB6.FontChangeBold(ctlTemp.Font, True)
                        End If
                    Next
                Next
                Else
                If TypeOf ctlTemp Is System.Windows.Forms.TextBox Then
                    Dim MyTextBox As TextBox
                    MyTextBox = ctlTemp
                    MyTextBox.ForeColor = UserForeColor
                    MyTextBox.Font = fBold 'VB6.FontChangeBold(ctlTemp.Font, True)
                End If
                If TypeOf ctlTemp Is System.Windows.Forms.ComboBox Then
                    Dim MyComboBox As ComboBox
                    MyComboBox = ctlTemp
                    MyComboBox.ForeColor = UserForeColor
                    MyComboBox.Font = fBold 'VB6.FontChangeBold(ctlTemp.Font, True)
                End If
                If TypeOf ctlTemp Is System.Windows.Forms.CheckBox Then
                    Dim MyCheckBox As CheckBox
                    MyCheckBox = ctlTemp
                    MyCheckBox.ForeColor = UserForeColor
                    MyCheckBox.Font = fBold 'VB6.FontChangeBold(ctlTemp.Font, True)
                End If

            End If
        Next ctlTemp
        recColor = True ' all ok
        GoTo RecReadExit

RecReadErr:
        RecError(Err, ErrorToString(), "RecRead")
        recColor = Err.Number
        GoTo RecReadExit
RecReadExit:
    End Function


    Function RecEnable(ByRef frmName As System.Windows.Forms.Form, ByRef nToggle As Object) As Short
        '
        ' toggles input controls on/off
        '
        ' Inputs:
        '   frmName     form with bound controls
        '   nToggle     enable on/off (True/False)
        '
        ' Outputs:
        '   RecEnable   recOK if no errors
        '
        On Error GoTo RecEnableErr

        Dim ctlTemp, ctlTempGroup As System.Windows.Forms.Control
        Dim cTag As String

        For Each ctlTemp In frmName.Controls
            If TypeOf ctlTemp Is System.Windows.Forms.GroupBox Or TypeOf ctlTemp Is System.Windows.Forms.TabControl Then
                ctlTempGroup = Nothing
                For Each ctlTempGroup In ctlTemp.Controls
                    cTag = UCase(Trim(ctlTempGroup.Tag))
                    If Len(cTag) <> 0 Then
                        ctlTempGroup.Enabled = nToggle
                    End If
                Next
            Else
                cTag = UCase(Trim(ctlTemp.Tag))
                If Len(cTag) <> 0 Then
                    ctlTemp.Enabled = nToggle
                End If
            End If
        Next ctlTemp

        RecEnable = True ' all ok
        GoTo RecEnableExit
RecEnableErr:
        RecError(Err, ErrorToString(), "RecEnable")
        RecEnable = Err.Number
        GoTo RecEnableExit
RecEnableExit:
    End Function

    Public Sub RecError(ByRef nErr As ErrObject, ByRef cError As Object, ByRef cOpName As Object)
        '
        ' report trapped error to user
        '
        ' Inputs:
        '   nErr      Error Number
        '   cError    Error Message
        '   cOpName   Function/Sub that raised error
        '
        Dim cErrMsg As String


        cErrMsg = "Error:" & Chr(9) & nErr.Number.ToString & Chr(13)

        cErrMsg = cErrMsg & "Text:" & Chr(9) + cError.ToString + Chr(13)

        cErrMsg = cErrMsg & "Module:" & Chr(9) + cOpName
        'nErr.
        Select Case nErr.Number
            Case 3163
                cErrMsg = "String Entered to long to fit in data field"
        End Select
        MsgBox(cErrMsg, MsgBoxStyle.Critical + MsgBoxStyle.OkCancel, "RecError")
    End Sub

    Public Function PartialItem(ByRef txtField As String, ByRef txtSearchFor As String, ByRef TableToQuery As String) As DataTable
        'This will return the subset of the table according to the variables
        'Then use the subset to generate report or put it into a dbgrid to
        'select from, like the inventory screen
        '
        'txtfield = the field name in the table to query (ie ItemNumber or Description)
        'txtSearchFor = the text to match (ie ItemNumber like 'Toner')
        'dbToQuery =  Database to query from (ie pull data out of Inventory or Invoice Detail)
        '
        Dim Tables_Without_Location As String = "INVENTORY, VENDORS, COUNTY"
        Dim rsKeyword As New DataTable
        Dim sqlStatement As String

        sqlStatement = "SELECT * FROM " & TableToQuery & _
            " WHERE " & txtField & " LIKE '%" & txtSearchFor & "%'"
        If Not Tables_Without_Location.Contains(TableToQuery.ToUpper) Then
            'Invnetory & Vendors are the only MAIN Table that does NOT have Location field
            'Only because we split it up into multiple tables
            sqlStatement = sqlStatement & " AND Location = '" & glbLocation & "'"
        End If
        PartialItem = SQLGetDataTable(sqlStatement, cnSQL)

    End Function

    'Public Function PartialEmployee(ByRef txtField As String, ByRef txtSearchFor As String, ByRef TableToQuery As String, ByRef dbPayroll As dao.Database) As DataTable
    '    'This will return the subset of the table according to the variables
    '    'Then use the subset to generate report or put it into a dbgrid to
    '    'select from, like the inventory screen
    '    '
    '    'txtfield = the field name in the table to query (ie ItemNumber or Description)
    '    'txtSearchFor = the text to match (ie ItemNumber like 'Toner')
    '    'dbToQuery =  Database to query from (ie pull data out of Inventory or Invoice Detail)
    '    '
    '    
    '    Dim rsKeyword As DataTable 'DataTable
    '    Dim sqlStatement As String

    '    If txtField = "Description" Then 'does description contain txtSearchFor?
    '        sqlStatement = "Select * from " & TableToQuery & " where instr(" & txtField & ",'" & txtSearchFor & "') > 0;"
    '    Else 'ItemNumber Like
    '        sqlStatement = "Select * from " & TableToQuery & " where " & txtField & " like '" & txtSearchFor & "*';"
    '    End If

    '    rsKeyword = dbPayroll.OpenRecordset(sqlStatement)

    '    PartialEmployee = rsKeyword
    'End Function

    'Public Function GetInStockQty(ByRef Item As String, ByRef rsQty As DataTable) As DataTable
    '    Dim sqlString As String

    '    sqlString = "SELECT * FROM InStock WHERE ItemNumber = '" & Item & "' Order By Warehouse"
    '    rsQty = dbAugusta.OpenRecordset(sqlString, DataTableTypeEnum.dbOpenDynaset, DataTableOptionEnum.dbReadOnly)
    'End Function


    Public Sub GetPODetail(ByRef LstBox As System.Windows.Forms.ListBox, ByRef ItemNumber As String)
        'This procedure is for populating the ListBox for PoDetail
        'List PO# / Date / Qty
        Dim rsTemp As DataTable = Nothing
        Dim sqlString As String

        sqlString = "SELECT PODetail.PONumber, CONVERT(varchar(10), POMaster.PODate, 101) as PoDate, (PODetail.Qty-PODetail.Received) AS OutStanding " & _
            "FROM PODetail Join POMaster ON PODetail.PoNumber = POMaster.PoNumber " & _
            "WHERE ItemNumber = '" & ItemNumber & "' AND " & "PODetail.Qty > PODetail.Received;"
        rsTemp = SQLGetDataTable(sqlString, cnSQL)

        LstBox.Items.Clear() 'Clear out any data
        Dim row As DataRow
        For Each row In rsTemp.Rows
            LstBox.Items.Add(("   " + row("PONumber").ToString + Chr(9) + row("PODate") + Chr(9) + row("OutStanding").ToString))
        Next
    End Sub

    Public Function CompletePO(ByRef PoNo As String) As String
        Dim sqlstring As String
        Dim dtTemp As DataTable
        sqlstring = "SELECT * FROM TempPoD WHERE PoNumber = '" & PoNo & "' AND UserName = '" & glbUser_Computer & "' AND (Qty > (Received + Receiving))"
        dtTemp = SQLGetDataTable(sqlstring, cnSQL)
        If dtTemp.Rows.Count > 0 Then
            CompletePO = "Incomplete"
        End If
        CompletePO = "Complete"
    End Function

    'Public Function CompletePO(ByRef PoDetail As DataTable) As Boolean
    '    With PoDetail
    '        'Data1.Recordset
    '        .MoveFirst()
    '        While Not .EOF
    '            If (.Fields("Received").Value + .Fields("Receiving").Value) < .Fields("Qty").Value Then
    '                CompletePO = False
    '                Exit Function
    '            End If
    '            .MoveNext()
    '        End While
    '    End With
    '    CompletePO = True
    'End Function

    'Public Function OnOrder(ByRef rsPoDetail As DataTable) As Short
    '    'Count qty outstanding for a specific recordset.
    '    'rsPoDetail in the function should be a sql select statement for a specific item
    '    'number.  So the recordset will have all '37041013' items for example on order.

    '    Dim intTotal As Short

    '    intTotal = 0

    '    If rsPoDetail.RecordCount = 0 Then
    '        OnOrder = intTotal
    '        Exit Function
    '    End If

    '    With rsPoDetail
    '        .MoveFirst()
    '        While Not .EOF
    '            If (.Fields("Qty").Value - .Fields("Received").Value) > 0 Then
    '                intTotal = intTotal + (.Fields("Qty").Value - .Fields("Received").Value)
    '            End If
    '            .MoveNext()
    '        End While
    '    End With
    '    OnOrder = intTotal
    'End Function

    Public Function MainWhsInStock(ByRef txtItem As String) As Short
        'Count qty outstanding for a specific recordset.
        'rsPoDetail in the function should be a sql select statement for a specific item
        'number.  So the recordset will have all '37041013' items for example on order.

        Dim sqlString As String
        Dim rsTemp As DataTable = Nothing

        'get the main & other numbers
        sqlString = "SELECT isnull(sum(Qty),0) as Qty FROM InStock WHERE Warehouse = '" & txtMainWhs & "' and ItemNumber = '" & txtItem & "'"
        rsTemp = SQLGetDataTable(sqlString, cnSQL)
        MainWhsInStock = Val(rsTemp.Rows(0)("Qty").ToString)
    End Function

    'Public Function WantOrdered(ByRef rsWantDetail As DataTable) As Short
    '    'Count qty outstanding for a specific recordset.
    '    'rsPoDetail in the function should be a sql select statement for a specific item
    '    'number.  So the recordset will have all '37041013' items for example on order.

    '    Dim intTotal As Short

    '    intTotal = 0

    '    If rsWantDetail.RecordCount = 0 Then
    '        WantOrdered = intTotal
    '        Exit Function
    '    End If

    '    With rsWantDetail
    '        .MoveFirst()
    '        While Not .EOF
    '            
    '            If IsDBNull(.Fields("Qty").Value) Then
    '                'do nothing
    '            Else
    '                intTotal = intTotal + .Fields("Qty").Value
    '            End If
    '            .MoveNext()
    '        End While
    '    End With
    '    WantOrdered = intTotal
    'End Function

    'Public Sub CheckInStockQty(ByRef LstBox As System.Windows.Forms.ListBox, ByRef SearchString As String, ByRef RemovingQty As String)
    '    Dim x As Short
    '    Dim intTab As Short
    '    Dim Found As Boolean

    '    intTab = 1
    '    Found = False
    '    For x = 0 To LstBox.Items.Count - 1
    '        If InStr(1, LstBox.Items(x).text, SearchString) Then  ' VB6.GetItemString(LstBox, x)
    '            'Both ways I format a lstbox have 2 tabs in it
    '            '   {tab}AUG{tab}10
    '            '   AUG{tab}serno{tab}1
    '            'So I should find the second tab and there is my qty number
    '            intTab = InStr(intTab, VB6.GetItemString(LstBox, x), vbTab)
    '            intTab = InStr(intTab + 1, VB6.GetItemString(LstBox, x), vbTab)
    '            Found = True
    '            If Val(Right(VB6.GetItemString(LstBox, x), Len(VB6.GetItemString(LstBox, x)) - (intTab + 1))) < CDbl(RemovingQty) Then
    '                MsgBox("WARNING:  Quantity entered exceeds the Warehouse Quantity for this Item!", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Quantity Error!")
    '                Exit Sub
    '            End If
    '        End If
    '    Next
    '    If Not Found Then
    '        MsgBox("WARNING:  Quantity entered exceeds the Warehouse Quantity for this Item!", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Quantity Error!")
    '    End If
    'End Sub

    Public Sub IsNumber(ByRef KeyAscii As Short, ByRef FieldEditing As String)
        'I think keyAscii 46 is the decimal key on the number pad or Enter Key
        If (IsNumeric(Chr(KeyAscii)) Or KeyAscii = System.Windows.Forms.Keys.Back Or KeyAscii = 46 Or KeyAscii = 45 Or KeyAscii = 13) Then
            If KeyAscii = 13 Then
                KeyAscii = 0
                System.Windows.Forms.SendKeys.Send(("{TAB}"))
            ElseIf KeyAscii = 46 And InStr(FieldEditing, ".") Then
                KeyAscii = False
            Else
                KeyAscii = KeyAscii
            End If
            MDIMain.StatusBar.Text = ""
            'MDIMain.StatusBar.Panels(1).Text = ""
        Else
            Beep()
            'MDIMain.StatusBar.Panels(1).Text = "Numeric Keys ONLY"
            MDIMain.StatusBar.Text = "Numeric Keys ONLY"
            KeyAscii = False
        End If
    End Sub

    Public Function UpperCase(ByRef KeyAscii As Short, ByRef CurrentForm As System.Windows.Forms.Form) As Short
        'UPGRADE_NOTE: char was upgraded to char_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim char_Renamed As String
        char_Renamed = Chr(KeyAscii)

        'If we are in a combo box and Space Bar hit I want to drop down the list
        If (TypeOf CurrentForm.ActiveControl Is System.Windows.Forms.ComboBox Or TypeOf CurrentForm.ActiveControl Is System.Windows.Forms.DateTimePicker) And KeyAscii = 32 Then
            System.Windows.Forms.SendKeys.Send("%{DOWN}")
            KeyAscii = 0
            '        MsgBox "spacebar"
            Exit Function
        End If

        If KeyAscii = 13 Then 'Enter Key
            KeyAscii = 0
            System.Windows.Forms.SendKeys.Send("{TAB}")
        Else
            If KeyAscii = 10 Then 'Ctrl-Enter Key
                UpperCase = 13
            Else
                If char_Renamed = "'" Or char_Renamed = "[" Or char_Renamed = "]" Then 'Do NOT allow "'" they mess things up
                    Beep()
                    UpperCase = 0
                Else
                    UpperCase = Asc(UCase(char_Renamed))
                End If
            End If 'Ctrl-Enter
        End If
    End Function

    Public Function TabEnter(ByRef KeyAscii As Short) As Short
        'UPGRADE_NOTE: char was upgraded to char_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim char_Renamed As String
        char_Renamed = Chr(KeyAscii)
        If char_Renamed = vbTab Then
            TabEnter = vbCrLf 'vbEnter
        Else
            TabEnter = Asc(char_Renamed)
        End If
    End Function

    Public Sub PopulateCombo(ByRef Combo As System.Windows.Forms.ComboBox, ByRef txtField As String, ByRef txtTable As String)
        Dim dtTemp As DataTable = Nothing
        Dim sqlstring As String = ""

        sqlstring = "Select * from " & txtTable
        If txtTable.ToUpper = "WAREHOUSE" Then
            If Combo.Name.ToUpper = "CMBSALESPERSON" Or Combo.Name.ToUpper = "CMBCUSTOMER" Or Combo.Name.ToUpper = "CMBCUSTOMERS" Then
                sqlstring = sqlstring & " WHERE Location = '" & glbLocation & "' AND Active = 1 AND (Tech = 0 OR Warehouse =  (CASE WHEN Location = 'Athens' THEN 'ATH' ELSE 'AUG' END))"
            Else
                sqlstring = sqlstring & " WHERE Active = 1 AND (Tech = 1 AND Location = '" & glbLocation & "')"
                If Combo.Name = "cmbTo" Then
                    sqlstring &= " OR Warehouse IN ('ATH', 'AUG')"
                End If
            End If
        ElseIf txtTable.ToUpper = "CUSTOMER" Then
            sqlstring = sqlstring & " WHERE Location = '" & glbLocation & "'"
        End If
        sqlstring = sqlstring & " ORDER BY " & txtField
        ' MsgBox(sqlstring)
        dtTemp = SQLGetDataTable(sqlstring, cnSQL)
        Combo.Items.Clear()
        Combo.Items.Add("")
        For Each dr As DataRow In dtTemp.Rows
            Combo.Items.Add(dr(txtField).ToString)
        Next
        Combo.SelectedIndex = 0

    End Sub

    Public Sub PopulateContractType(ByRef Combo As System.Windows.Forms.ComboBox)
        Dim dtTemp As DataTable = Nothing
        Dim sqlstring As String = ""

        sqlstring = "Select ContractType, Description from ContractType"
        dtTemp = SQLGetDataTable(sqlstring, cnSQL)
        Combo.Items.Clear()
        For Each dr As DataRow In dtTemp.Rows
            Combo.Items.Add(dr("ContractType").ToString & " - " & dr("Description").ToString)
        Next
        Combo.SelectedIndex = 0
    End Sub

    Public Sub ShowWindow(ByRef CurrentWindow As System.Windows.Forms.Form)
        'Place the form on top
        'Does not matter if it exists yet or not
        'I still want it on top
        On Error GoTo errorhandle
        CurrentWindow.BringToFront()
        If CurrentWindow.WindowState = System.Windows.Forms.FormWindowState.Minimized Then
            CurrentWindow.WindowState = System.Windows.Forms.FormWindowState.Normal
        Else 'not open yet
            CurrentWindow.Left = 0
            CurrentWindow.Top = 0
            CurrentWindow.Show()
            CurrentWindow.Refresh()
        End If
errorhandle:

    End Sub

    Public Function EditNewestItem(ByRef CurrInventoryNumber As String) As String
        'Dim txtReplacedBy As String
        Dim rsTemp As DataTable = Nothing
        'Dim sqlString As String
        'Dim Results As String = ""

        'Return the NEWEST Number for the Item provided
        rsTemp = SQLGetDataTable("EXEC [Inventory].[usp_Newest_ItemNumber] @pItemNumber = '" & CurrInventoryNumber & "', @pLocation = '" & glbLocation & "'", cnSQL)
        If rsTemp.Rows.Count > 0 Then
            Return rsTemp.Rows(0).Item("newest_ItemNumber").ToString
        Else
            Return ""
        End If

        ''return the value we came in with if not replaced
        'sqlString = "Select Inventory.ItemNumber, InventoryDetail.ReplacedBy " &
        '    "From Inventory LEFT JOIN InventoryDetail ON Inventory.ItemNumber = InventoryDetail.ItemNumber AND InventoryDetail.MainWarehouse = '" & txtMainWhs & "' " &
        '    "WHERE Inventory.ItemNumber = '" & CurrInventoryNumber & "'"
        'rsTemp = SQLGetDataTable(sqlString, cnSQL)
        'If rsTemp.Rows.Count > 0 Then
        '    Results = rsTemp.Rows(0).Item("ItemNumber").ToString
        '    txtReplacedBy = rsTemp.Rows(0).Item("ReplacedBy").ToString
        '    If txtReplacedBy <> "" Then
        '        Results = EditNewestItem(txtReplacedBy)
        '    End If
        '    Return Results
        'Else
        '    Return CurrInventoryNumber
        'End If
    End Function

    Public Function GetReplacementString(ByVal txtItem As String) As String
        'Public Function txtItemNumberLeave(ByVal txtItem As String, ByVal strSearchField As String, ByVal strSearchTable As String) As DataTable
        '  this function will serve as the default "onLeave" action
        '  when the cursor leaves the txtItemNumber Field.  If the 
        '  item is found this will return true so form fields can be populated.
        '  this will return a possible 3 options.  
        '  0 = item not found
        '  1 = item found
        '  9 = itemnumber empty so we can leave the field
        Dim dttemp, dtSearch As New DataTable
        Dim tmpSearchItem As String = ""

        If txtItem = "" Then
            GetReplacementString = tmpSearchItem
        End If

        'First need to find the FIRST/Original item
        '  We might start at the first, last or middle.  Just don't know, so lets get to First then start.
        'Once we have the FIRST we can flow the chain and build the Replacement String
        dttemp = GetItem("ItemNumber", txtItem)
        If dttemp.Rows.Count = 0 Then
            While dttemp.Rows(0)("Replaces").ToString <> ""
                txtItem = dttemp.Rows(0)("Replaces").ToString
                dttemp = GetItem("ItemNumber", txtItem)
            End While
        End If

        If dttemp.Rows.Count = 0 Then
            While dttemp.Rows(0)("ReplacedBy").ToString <> ""
                If tmpSearchItem = "" Then
                    tmpSearchItem = txtItem
                Else
                    tmpSearchItem = tmpSearchItem & ", " & txtItem
                End If
                txtItem = dttemp.Rows(0)("ReplacedBy").ToString
                dttemp = GetItem("ItemNumber", txtItem)
            End While
        End If
        GetReplacementString = tmpSearchItem
    End Function

    Public Function CheckNull(ByRef fldChecking As Object) As String
        If IsDBNull(fldChecking) Then
            CheckNull = ""
        Else
            CheckNull = fldChecking.ToString
        End If

    End Function

    Public Function CheckNullSQL(ByRef fldChecking As Object) As String

        If IsDBNull(fldChecking) Then
            CheckNullSQL = ""
        Else
            CheckNullSQL = fldChecking.ToString
        End If

    End Function


    Public Function CheckNull_Nbr(ByRef fldChecking As Object) As String

        If IsDBNull(fldChecking) Then
            CheckNull_Nbr = "0"
        Else
            If fldChecking.ToString = "" Then
                CheckNull_Nbr = "0"
            Else
                CheckNull_Nbr = fldChecking
            End If
        End If

    End Function

    Public Function CheckNull_NbrSQL(ByRef fldChecking As Object) As String

        If IsDBNull(fldChecking) Then
            CheckNull_NbrSQL = "0"
        Else
            If fldChecking.ToString = "" Then
                CheckNull_NbrSQL = "0"
            Else
                CheckNull_NbrSQL = fldChecking.ToString
            End If
        End If

    End Function

    Public Sub CheckWords(ByRef CheckAmt As Integer, ByRef Phrase As String)
        If CheckAmt >= 1000 Then 'Thousands - upto 900,000
            CheckWords((CheckAmt \ 1000), Phrase)
            Phrase &= " Thousand"
            CheckAmt -= ((CheckAmt \ 1000) * 1000)
        End If
        If CheckAmt >= 100 Then 'Hundreds
            CheckWords((CheckAmt \ 100), Phrase)
            Phrase &= " Hundred"
            CheckAmt -= ((CheckAmt \ 100) * 100)
        End If
        If CheckAmt >= 20 Then
            Select Case (CheckAmt \ 10)
                Case 2
                    Phrase &= " Twenty"
                Case 3
                    Phrase &= " Thirty"
                Case 4
                    Phrase &= " Forty"
                Case 5
                    Phrase &= " Fifty"
                Case 6
                    Phrase &= " Sixty"
                Case 7
                    Phrase &= " Seventy"
                Case 8
                    Phrase &= " Eighty"
                Case 9
                    Phrase &= " Ninety"
            End Select
            CheckAmt -= ((CheckAmt \ 10) * 10)
        End If
        If CheckAmt >= 10 Then
            Select Case CheckAmt
                Case 10
                    Phrase &= " Ten"
                Case 11
                    Phrase &= " Eleven"
                Case 12
                    Phrase &= " Twelve"
                Case 13
                    Phrase &= " Thirteen"
                Case 14
                    Phrase &= " Fourteen"
                Case 15
                    Phrase &= " Fifteen"
                Case 16
                    Phrase &= " Sixteen"
                Case 17
                    Phrase &= " Seventeen"
                Case 18
                    Phrase &= " Eighteen"
                Case 19
                    Phrase &= " Nineteen"
            End Select
        Else
            Select Case CheckAmt
                Case 0
                    ' do nothing
                Case 1
                    Phrase &= " One"
                Case 2
                    Phrase &= " Two"
                Case 3
                    Phrase &= " Three"
                Case 4
                    Phrase &= " Four"
                Case 5
                    Phrase &= " Five"
                Case 6
                    Phrase &= " Six"
                Case 7
                    Phrase &= " Seven"
                Case 8
                    Phrase &= " Eight"
                Case 9
                    Phrase &= " Nine"
            End Select
        End If
    End Sub

    Public Sub UpdateMeter(ByRef SerialNo As String, ByRef MtrDate As Date, ByRef BWmeter As Integer, ByRef Clrmeter As Integer, ByRef Source As String, ByRef ObtainedBy As String)
        Dim sqlString As String

        sqlString = "INSERT INTO Meter (SerialNumber, MeterDate, BWMeter, ColorMeter, Source, ObtainedBy) " & "VALUES ('" & SerialNo & "', '" & MtrDate & "', " & BWmeter & ", " & Clrmeter & ", '" & Source & "', '" & ObtainedBy & "')"
        'dbAugusta.Execute(sqlString)
        DBCmd = New SqlCommand(sqlString, cnSQL)
        DBCmd.ExecuteNonQuery()

    End Sub

    Public Function CustomerSearch(ByRef txtSearchingFor As String, ByRef rsTabletoSearch As DataTable) As Boolean

        Dim rsSearch As DataTable ''DataTable 'DataTable
        Dim sqlString As String

        Dim rsTemp As DataTable = Nothing  ''DataTable 'DataTable

        ' First see if any item number begin with the partial/complete
        ' item number entered.
        '
        ' Second if not a valid part number then do a search on any description
        ' which begins with the letters entered.

        rsSearch = PartialItem("Name", txtSearchingFor, "Customer")

        If rsSearch.Rows.Count = 0 Then ' no record selected, search description
            MsgBox("No Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            CustomerSearch = False
            Exit Function
        End If

        ' Populate the recordset.
        'rsSearch.MoveLast()
        'rsSearch.MoveFirst()

        Select Case rsSearch.Rows.Count
            Case 1 ' there is only one record
                txtSearchingFor = rsSearch.Rows(0)("Name").ToString
                rsTabletoSearch = rsSearch
            Case Else
                'frmSearch.rsSearch = rsTabletoSearch
                ''Check and see if they entered a number (customer number) or part of a name
                'If IsNumeric(txtSearchingFor) Then
                '	frmSearch.txtItemNumber.Text = txtSearchingFor
                'Else 'name
                '	frmSearch.txtItemDescription.Text = txtSearchingFor
                'End If
                'frmSearch.ShowDialog()
                'Returns the customer number


                'Commenting out to test entering name not number
                sqlString = "SELECT * FROM Customer WHERE CustomerNumber = " & txtSearchResults & ";"
                rsTemp = SQLGetDataTable(sqlString, cnSQL)
                If rsTemp.Rows.Count > 0 Then
                    'rsTemp.MoveLast()
                    txtSearchingFor = rsSearch.Rows(0)("Name").ToString ' rsTemp.Rows(0)("Name").ToString
                End If
        End Select
        CustomerSearch = True
    End Function

    Public Function ReturnCustNumber(ByRef rsTemp As DataTable, ByRef txtCustName As String) As String
        'Dim rsTemp As DataTable = Nothing
        Dim sqlString As String

        txtCustName = UCase(txtCustName)

        If IsNumeric(txtCustName) Then
            'Entered a Customer Number instead of Customer Name
            sqlString = "SELECT * FROM Customer WHERE CustomerNumber = " & txtCustName & " AND Location = '" & glbLocation & "'"
            'rsTemp = dbAugusta.OpenRecordset(sqlString, DataTableTypeEnum.dbOpenDynaset)
            rsTemp = SQLGetDataTable(sqlString, cnSQL)

            If rsTemp.Rows.Count > 0 Then
                'rsTemp.MoveLast()
                ReturnCustNumber = rsTemp.Rows(0)("CustomerNumber").ToString
            Else
                ReturnCustNumber = ""
            End If
        Else 'Entered Customer Name
            'rsTemp = dbAugusta.OpenRecordset("Customer", DataTableTypeEnum.dbOpenTable)
            'rsTemp.Index = "PrimaryKey"
            'rsTemp.Seek("=", txtCustName)
            'If rsTemp.NoMatch Then
            '    If CustomerSearch(txtCustName, rsTemp) = True Then
            '        rsTemp.Seek("=", txtCustName)
            '        ReturnCustNumber = rsTemp.Fields("CustomerNumber").Value
            '    Else
            '        ReturnCustNumber = ""
            '    End If
            'Else
            '    'Found
            '    ReturnCustNumber = rsTemp.Fields("CustomerNumber").Value
            'End If
            sqlString = "SELECT * FROM Customer where [name] = '" & txtCustName & "' AND Location = '" & glbLocation & "'"
            rsTemp = SQLGetDataTable(sqlString, cnSQL)
            'rsTemp = dbAugusta.OpenRecordset("Customer", DataTableTypeEnum.dbOpenTable)
            'rsTemp.Index = "PrimaryKey"
            'rsTemp.Seek("=", txtCustName)
            If rsTemp.Rows.Count = 0 Then
                If CustomerSearch(txtCustName, rsTemp) = True Then
                    'rsTemp.Seek("=", txtCustName)
                    ReturnCustNumber = rsTemp.Rows(0)("CustomerNumber").ToString
                Else
                    ReturnCustNumber = ""
                End If
            Else
                'Found
                ReturnCustNumber = rsTemp.Rows(0)("CustomerNumber").ToString
            End If

        End If
    End Function

    Public Sub Report9(ByRef ReportName As String, ByRef SelectString As String)
        'Dim Report As New CRAXDRT.Report

        'Report = ReportAppl.OpenReport(ReportName)
        'Report.RecordSelectionFormula = SelectString
        'Report.PrintOut(False)
    End Sub

    Public Sub Load_Report(ByRef ReportName As String)
        'ONLY reason for this is that if the preview loads the same reprot with same parameters, it will not refresh the report.
        'OLD data (previous preview is displayed
        glbReport.Load(glbCrystalFileLocation & "BlankReport.rpt")
        glbReport.Load(glbCrystalFileLocation & ReportName)

    End Sub

    Public Sub Pause(ByRef DelaySeconds As Single)
        Const OneSecond As Double = 1.0# / (24.0# * 60.0# * 60.0#)
        Dim WaitUntil As Date

        '        Dim sttimer As Single
        WaitUntil = System.DateTime.FromOADate(Now.ToOADate + (CDbl(DelaySeconds) * OneSecond))

        Do While Now < WaitUntil
            Sleep(100)
            System.Windows.Forms.Application.DoEvents()
        Loop

    End Sub

    Public Sub FakeSQL(ByRef tmpTable As String, ByRef tmpFieldName As String)
        Dim sqlString As String

        sqlString = "UPDATE " & tmpTable & " SET " & tmpFieldName & " = '' WHERE 1=2"
        'dbAugusta.Execute(sqlString)
        SQLExecuteNonQuery(sqlString, cnSQL)

    End Sub


    Public Function CheckDate(ByRef CycleMonth As Short, ByRef CycleYear As Integer, ByRef ContractEndDate As Date) As Short
        'Checking for the last day of the month
        'ie 5/31/01 would become 6/31/01, which is invalid
        'Only a possible problem if greater than 28, Feb 28

        'But by the same token, if the contract is setup to go from 8/31/00 thru 8/31/01 then
        'If possible I want the day to be the 31st.
        'I am going to do this by looking at the day of the End Date.

        Dim tmpDay As Short

        tmpDay = VB.Day(ContractEndDate)
        CheckDate = tmpDay
        'Only months Jan, Mar, May, Jul, Aug, Oct, Dec
        Select Case CycleMonth ' Evaluate Number.
            Case 1, 3, 5, 7, 8, 10, 12
                'Do nothing every day up to 31 is a valid day
                CheckDate = tmpDay
            Case 4, 6, 9, 11 '30 Day Months
                If tmpDay > 30 Then
                    CheckDate = 30
                End If
            Case Else ' Feb. could be 28 or 29 (Leap Year)
                If (CycleYear Mod 4) = 0 Then 'Leap year
                    If tmpDay > 28 Then
                        CheckDate = 29
                    End If
                Else ' Not Leap Year
                    If tmpDay >= 28 Then
                        CheckDate = 28
                    End If
                End If
        End Select
    End Function

    Public Function MeterNewMonth(ByRef MeterCycle As String, ByRef MeterDate As Date, ByRef NewYear As Short) As Short
        'Need to check and make sure that a valid month has been entered.
        'Example if billed quarterly and last billed 1/1/80, then next meter date can not be 3/1/80
        'I do not want to let this happen.
        'I want to only allow even periods entered.
        Dim txtOldMonth As String
        Dim tmpNewMonth As String

        txtOldMonth = CStr(Month(MeterDate))
        tmpNewMonth = txtOldMonth

        'Don't really have to do anything for Monthly or Daily
        If MeterCycle = "Quarterly" Then
            If Val(txtOldMonth) >= 10 Then ' Will need to adjust for new Year
                tmpNewMonth = Str(Val(txtOldMonth) + 3 - 12)
                NewYear += 1
            Else
                tmpNewMonth = Str(Val(txtOldMonth) + 3)
            End If
        ElseIf MeterCycle = "Semi-Annual" Then
            If Val(txtOldMonth) >= 7 Then ' Will need to adjust for new Year
                tmpNewMonth = Str(Val(txtOldMonth) + 6 - 12)
                NewYear += 1
            Else
                tmpNewMonth = Str(Val(txtOldMonth) + 6)
            End If
        ElseIf MeterCycle = "Yearly" Then
            tmpNewMonth = Str(Val(txtOldMonth))
            NewYear += 1
        Else ' Monthly
            If Val(txtOldMonth) = 12 Then
                tmpNewMonth = "1"
                NewYear += 1
            Else
                tmpNewMonth = Str(Val(txtOldMonth) + 1)
            End If
        End If
        MeterNewMonth = Val(tmpNewMonth)
    End Function

    Public Function GetConnectionString(ByVal strConnection As String) As String
        'Declare a string to hold the connection string
        'Dim sReturn As New String("")
        Dim sReturn As String
        'Check to see if they provided a connection string name
        If Not String.IsNullOrEmpty(strConnection) Then
            'Retrieve the connection string fromt he app.config
            'Dim DBConnection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("Augusta.MySettings.Setting").ConnectionString)
            'configurationm
            ConfigurationManager.RefreshSection("connectionString")

            '2020-07
            'Debug to write what the app.config file has
            'Had issue where the program was reading some old app.config
            '-Deleted the app.config file completely 
            '-Created new one and pasted back the same content
            '-Went under My Project and added a Setting to make sure it created the item in my app.config file, it did
            '
            'Dim conn_string_List As ConnectionStringSettingsCollection = ConfigurationManager.ConnectionStrings
            'For Each x As ConnectionStringSettings In conn_string_List
            '    Console.WriteLine(x.Name + "  --  " + x.ConnectionString)
            'Next

            sReturn = ConfigurationManager.ConnectionStrings(strConnection).ConnectionString

        Else
            'Since they didnt provide the name of the connection string
            'just grab the default on from app.config
            sReturn = ConfigurationManager.ConnectionStrings("YourConnectionString").ConnectionString
        End If
        'Return the connection string to the calling method
        Return sReturn
    End Function
    Public Function GetAppConfigValue(ByVal strToken As String) As String
        'Declare a string to hold the connection string
        Dim sReturn As New String("")
        'Check to see if they provided a connection string name
        If Not String.IsNullOrEmpty(strToken) Then
            'Retrieve the connection string fromt he app.config
            sReturn = ConfigurationManager.AppSettings(strToken).ToString
        Else
            'Since they didnt provide the name of the connection string
            'just grab the default on from app.config
            sReturn = ConfigurationManager.AppSettings("CSV").ToString
        End If
        'Return the connection string to the calling method
        Return sReturn
    End Function


    Public Function TwipsToPixels(ByVal intPixels As Integer, ByRef frmName As System.Windows.Forms.Form) As Integer

        Dim pixles As Integer = 0
        Dim g As Graphics = frmName.CreateGraphics()

        pixles = intPixels * g.DpiX / 1440
        g.Dispose()
        Return pixles

    End Function

    Public Function SQLExecuteNonQuery(ByVal pSQL As String, ByVal pConn As SqlConnection, Optional ByVal pTransaction As SqlTransaction = Nothing) As Integer
        'You can use the ExecuteNonQuery to perform catalog operations (for example, querying the structure of a database or creating database objects such as tables), 
        'or to change the data in a database without using a DataSet by executing UPDATE, INSERT, or DELETE statements.
        'Although the ExecuteNonQuery returns no rows, any output parameters or return values mapped to parameters are populated with data.
        'For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command. When a trigger exists on a table being 
        'inserted or updated, the return value includes the number of rows affected by both the insert or update operation and the number of rows affected by the trigger or triggers. For all other types of statements, the return value is -1. If a rollback occurs, the return value is also -1.

        Try
            DBCmd = New SqlCommand(pSQL, pConn)
            DBCmd.Transaction = pTransaction
            Try
                SQLExecuteNonQuery = DBCmd.ExecuteNonQuery

            Catch sqlex As SqlException
                MsgBox(pSQL & vbNewLine & vbNewLine & sqlex.Message)
            Catch ex As Exception
                MsgBox(ex.Message)

            End Try
        Catch ex As Exception
            MsgBox(ex.Message)
            SQLExecuteNonQuery = -1
        End Try
    End Function

    Public Function SQLExecuteScalar(ByVal pSQL As String, ByVal pConn As SqlConnection) As Integer
        'Use the ExecuteScalar method to retrieve a single value (for example, an aggregate value) from a database. 
        'This requires less code than using the ExecuteReader method, and then performing the operations that you need to generate the single value using the data returned by a SqlDataReader.

        Try
            DBCmd = New SqlCommand(pSQL, pConn)
            SQLExecuteScalar = DBCmd.ExecuteScalar
        Catch ex As Exception
            MsgBox(pSQL)
        End Try
    End Function

    Public Function GetUniqueNumber(ByVal fldName As String, ByVal pLocation As String) As Integer
        Try
            DBCmd = New SqlCommand()
            DBCmd.Connection = cnSQL
            DBCmd.CommandType = CommandType.StoredProcedure
            DBCmd.CommandText = "usp_GetUniqueNumbers"
            DBCmd.Parameters.Add("@colName", SqlDbType.VarChar, 50).Value = fldName
            If fldName.ToUpper = "VENDORNUMBER" Then
                DBCmd.Parameters.Add("@colLocation", SqlDbType.VarChar, 50).Value = "Athens"
            Else
                DBCmd.Parameters.Add("@colLocation", SqlDbType.VarChar, 50).Value = pLocation
            End If
            DBCmd.Parameters.Add("@curNumber", SqlDbType.Int)
            DBCmd.Parameters("@curNumber").Direction = ParameterDirection.Output
            DBCmd.ExecuteNonQuery()
            GetUniqueNumber = DBCmd.Parameters("@curNumber").Value
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function SQLGetDataTable(ByVal pSearchTable As String, ByVal pConn As SqlConnection) As DataTable

        Dim dtTemp As New DataTable
        Dim tempDR As SqlClient.SqlDataReader
        Dim tempCMD As New SqlClient.SqlCommand()

        tempDR = Nothing
        tempCMD = New SqlCommand(pSearchTable, pConn)
        tempDR = tempCMD.ExecuteReader()
        dtTemp.Load(tempDR)
        tempDR.Close()
        tempCMD.Dispose()

        Return dtTemp

    End Function

    Public Function SQLExecuteSP(ByVal pSQL As String, ByVal pConn As SqlConnection, Optional ByVal pParams As ArrayList = Nothing) As Integer

        Try
            DBCmd = New SqlCommand(pSQL, pConn)
            DBCmd.CommandType = CommandType.StoredProcedure
            DBCmd.CommandText = pSQL

            If Not pParams Is Nothing Then
                ' load up parameters
                For Each p As SqlParameter In pParams
                    'Dim param As New SqlParameter(p.ParameterName, p.SqlDbType, p.Size)
                    'param.Direction = ParameterDirection.Input
                    'param.SqlDbType = SqlDbType.VarChar
                    DBCmd.Parameters.Add(p)
                Next
            End If
            SQLExecuteSP = DBCmd.ExecuteNonQuery
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function SQLExecuteSPScalar(ByVal pSQL As String, ByVal pConn As SqlConnection, Optional ByVal pParams As ArrayList = Nothing) As Integer
        'Use the ExecuteScalar method to retrieve a single value (for example, an aggregate value) from a database. 
        'This requires less code than using the ExecuteReader method, and then performing the operations that you need to generate the single value using the data returned by a SqlDataReader.
        ' this can be used if you want to call a stored procedure to return a count, not the number of rows affected
        Try
            DBCmd = New SqlCommand(pSQL, pConn)
            DBCmd.CommandType = CommandType.StoredProcedure
            DBCmd.CommandText = pSQL

            If Not pParams Is Nothing Then
                ' load up parameters
                For Each p As SqlParameter In pParams
                    'Dim param As New SqlParameter(p.ParameterName, p.SqlDbType, p.Size)
                    'param.Direction = ParameterDirection.Input
                    'param.SqlDbType = SqlDbType.VarChar
                    DBCmd.Parameters.Add(p)
                Next
            End If
            SQLExecuteSPScalar = DBCmd.ExecuteScalar
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function GetItem(ByVal pSearchTable As String, ByVal pItem As String) As DataTable

        Dim dtTemp As New DataTable
        Dim sqlStatement As String
        Dim txtNewNumber As String

        Select Case pSearchTable
            Case "SerialNumber", "Serialized"
                sqlStatement = "select * from dbo.Serialized where SerialNumber = '" & pItem & "' "
            Case "ControlID"
                sqlStatement = "select * from dbo.Serialized where ControlID = '" & pItem & "'"
            Case "ItemNumber"
                'sqlStatement = "select * from dbo.Inventory where ItemNumber = '" & pItem & "'"
                '                sqlStatement = "select ItemNumber, Description from dbo.Inventory where ItemNumber = '" & pItem & "'"
                sqlStatement = "select * from dbo.Inventory a LEFT JOIN dbo.InventoryDetail b ON " & _
                    "a.itemnumber = b.itemnumber AND b.MainWarehouse = '" & txtMainWhs & "' where a.ItemNumber = '" & pItem & "'"
            Case "Contracts"
                sqlStatement = "Select * From Contracts Where SerialNumber = '" & pItem & "'"
            Case Else
                sqlStatement = "select * from dbo.Serialized where ControlID = '" & pItem & "'"
        End Select

        DBCmd = New SqlCommand(sqlStatement, cnSQL)
        DBDR = DBCmd.ExecuteReader()
        dtTemp.Load(DBDR)
        DBDR.Close()

        'Check if Replaced By New Items
        If pSearchTable = "ItemNumber" Then
            If dtTemp.Rows.Count > 0 Then
                If Not IsDBNull(dtTemp.Rows(0)("ReplacedBy")) Then
                    If Trim(dtTemp.Rows(0)("ReplacedBy").ToString) <> "" Then
                        If MsgBox("This product code has been replaced by a new number. " & Chr(10) & Chr(13) & _
                                      "Would you like to work with the newest product code?", MsgBoxStyle.YesNo, "Changed To Number Exists") = MsgBoxResult.Yes Then
                            txtNewNumber = EditNewestItem(dtTemp.Rows(0)("ReplacedBy").ToString)
                            If txtNewNumber = "" Then
                                'nothing
                                'this may be wrong, probably should return nothing, but i think the prior search would be better
                            Else
                                dtTemp = GetItem("ItemNumber", txtNewNumber)
                            End If
                        End If
                    End If
                End If
            End If
        End If

        Return dtTemp

    End Function

    Public Function ItemSearch(ByRef pItemNumber As String, ByRef pFormCalling As String) As String
        Dim rsSearch As DataTable
        ' First see if any item number begin with the partial/complete
        ' item number entered.
        '
        ' Second if not a valid part number then do a search on any description
        ' which begins with the letters entered.

        rsSearch = PartialItem("ItemNumber", pItemNumber, "Inventory")

        If rsSearch.Rows.Count = 0 Then ' no record selected, search description
            MsgBox("No Records found matching the search criteria", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            ItemSearch = ""
            Exit Function
        ElseIf rsSearch.Rows.Count = 1 Then
            ItemSearch = rsSearch.Rows(0)("ItemNumber").ToString
        Else
            Dim fSearch As New frmSearch
            '            fSearch.pItemNumber = txtItemNumber.Text
            fSearch.pSQL = ""
            fSearch.FormCalling = pFormCalling
            fSearch.txtItemNumber.Text = pItemNumber
            fSearch.ShowDialog()
            ItemSearch = txtSearchResults
        End If

    End Function

    Public Function Format_No_Comma(ByVal p_int As String) As String
        Dim tmpString As String
        Dim tmpDouble As Double

        tmpString = p_int.ToString
        tmpString = tmpString.Replace(",", "")
        tmpString = tmpString.Replace("$", "")
        tmpDouble = Val(tmpString)
        tmpString = tmpDouble.ToString("######0.00;-######0.00") ' String.Format("{0:f2}", Val(tmpString))
        Return tmpString
    End Function

    Public Function Format_With_Comma(ByVal p_int As String) As String
        Dim tmpString As String
        Dim tmpDouble As Double

        tmpString = p_int.ToString
        tmpString = tmpString.Replace(",", "")
        tmpString = tmpString.Replace("$", "")
        tmpDouble = Val(tmpString)
        tmpString = tmpDouble.ToString("#,###,##0.00;-#,###,##0.00") ' String.Format("{0:f2}", Val(tmpString))
        'tmpString = String.Format("{0:c2}", Val(tmpString))
        Return tmpString
    End Function

    Public Function Format_No_Decimal(ByVal p_int As String) As String
        Dim tmpString As String
        tmpString = p_int.ToString
        tmpString = tmpString.Replace(",", "")
        tmpString = tmpString.Replace("$", "")
        tmpString = String.Format("{0:F0}", Val(tmpString))
        Return tmpString
    End Function

    Public Function Format_No_Comma_x_Decimals(ByVal p_int As String, ByVal x As Short) As String
        Dim tmpString As String
        Dim formatstring As String = ""
        formatstring = "{0:f" & x & "}"
        tmpString = p_int.ToString
        tmpString = tmpString.Replace(",", "")
        tmpString = tmpString.Replace("$", "")
        tmpString = String.Format(formatstring, Val(tmpString))
        Return tmpString
    End Function

    Public Function ControlID_to_SerialNumber(ByVal tmpControlID As String) As String
        Dim sqlstring As String
        Dim rstemp As DataTable

        If tmpControlID.ToString = "" Or tmpControlID.ToString = "0" Then
            Return ""
        Else
            sqlstring = "SELECT SerialNumber FROM Serialized WHERE ControlID = '" & tmpControlID & "' AND Location = '" & glbLocation & "'"
            rstemp = SQLGetDataTable(sqlstring, cnSQL)
            If rstemp.Rows.Count = 0 Then
                Return ""
            Else
                Return rstemp.Rows(0)("SerialNumber").ToString
            End If
        End If
    End Function

    Public Function txtItemNumberLeave(ByVal txtItem As String, ByVal strSearchField As String, ByVal strSearchTable As String) As DataTable
        '  this function will serve as the default "onLeave" action
        '  when the cursor leaves the txtItemNumber Field.  If the 
        '  item is found this will return true so form fields can be populated.
        '  this will return a possible 3 options.  
        '  0 = item not found
        '  1 = item found
        '  9 = itemnumber empty so we can leave the field
        Dim rstemp, rsSearch As New DataTable
        Dim tmpSearchItem As String

        If txtItem = "" Then
            Return Nothing
        End If

        rstemp = GetItem("ItemNumber", txtItem)
        If rstemp.Rows.Count = 0 Then
            tmpSearchItem = ItemSearch(txtItem, strSearchTable)

            If tmpSearchItem <> "" Then
                rstemp = GetItem("ItemNumber", tmpSearchItem)
            End If
        End If

        Return rstemp

    End Function

    Public Function txtVendorNameLeave(ByVal txtItem As String, ByVal strSearchField As String, ByVal strSearchTable As String) As DataTable
        '  this function will serve as the default "onLeave" action
        '  when the cursor leaves the txtItemNumber Field.  If the 
        '  item is found this will return true so form fields can be populated.
        '  this will return a possible 3 options.  
        '  0 = item not found
        '  1 = item found
        '  9 = itemnumber empty so we can leave the field
        Dim rstemp, rsSearch As New DataTable
        Dim tmpSearchItem As String

        If txtItem = "" Then
            Return Nothing
        End If

        rstemp = GetItem("ItemNumber", txtItem)
        If rstemp.Rows.Count = 0 Then
            tmpSearchItem = ItemSearch(txtItem, strSearchTable)
            'rsSearch = PartialItem(strSearchField, txtItem, strSearchTable)

            If tmpSearchItem <> "" Then
                'txtItemNumber_Leave(txtItemNumber, New System.EventArgs())
                rstemp = GetItem("ItemNumber", tmpSearchItem)
                'Else
                '    txtItemNumber.SelectionStart = 0
                '    txtItemNumber.SelectionLength = Len(Trim(txtItemNumber.Text))
                '    txtItemNumber.Focus()
                '    Cancel = True
            End If
        End If

        Return rstemp

    End Function

    Public Sub StockData(ByRef frmName As System.Windows.Forms.Form, ByVal txtItemNumber As String, ByRef ListInStock As ListBox, ByRef MainWhsQty As Label, ByRef OtherWhsQty As Label)
        'ByRef rsStockQty As DataTable, ByRef LstBox As System.Windows.Forms.ListBox, ByRef ItemNumber As String, ByRef Main As System.Windows.Forms.Label, ByRef AllOther As System.Windows.Forms.Label)

        'This procedure is for populating the ListBox and Labels found on the Inventory and
        'Issue screen (so far will be on others).  It will receive a recordset
        Dim dtTemp As DataTable
        Dim sqlstring As String

        ListInStock.Items.Clear() 'Clear out any data
        sqlstring = "SELECT ItemNumber,Warehouse,Qty,isnull(SerialNumber,'') as SerialNumber FROM InStock WHERE ItemNumber = '" & txtItemNumber & "'"
        dtTemp = SQLGetDataTable(sqlstring, cnSQL)

        Dim row As DataRow
        For Each row In dtTemp.Rows
            'Add the list to the lstInStock List box for Detail Info
            ListInStock.Items.Add(row("Warehouse").ToString & Chr(9) + row("SerialNumber").ToString & Chr(9) & row("Qty").ToString)
        Next
        'get the main & other numbers
        sqlstring = "SELECT isnull(sum(Qty),0) as Qty FROM InStock WHERE Warehouse = '" & txtMainWhs & "' and ItemNumber = '" & txtItemNumber & "'"
        dtTemp = SQLGetDataTable(sqlstring, cnSQL)
        MainWhsQty.Text = dtTemp.Rows(0)("Qty").ToString

        sqlstring = "SELECT isnull(sum(Qty),0) as Qty FROM InStock WHERE Warehouse <> '" & txtMainWhs & "' and ItemNumber = '" & txtItemNumber & "'"
        dtTemp = SQLGetDataTable(sqlstring, cnSQL)
        OtherWhsQty.Text = dtTemp.Rows(0)("Qty").ToString

        dtTemp = Nothing
    End Sub

    Public Sub Print_ReportToPrinter(ByRef pCrystalReport As CrystalDecisions.CrystalReports.Engine.ReportDocument)

        Dim objTableLogonInfo As CrystalDecisions.Shared.TableLogOnInfo

        '\ Report objects
        Dim objDatabaseTable As CrystalDecisions.CrystalReports.Engine.Table
        'Dim objCrSection As CrystalDecisions.CrystalReports.Engine.Section
        'Dim objCrReportObject As CrystalDecisions.CrystalReports.Engine.ReportObject
        'Dim objCrSubreportObject As CrystalDecisions.CrystalReports.Engine.SubreportObject
        'Dim objCrSubreport As CrystalDecisions.CrystalReports.Engine.ReportDocument

        '\ Call you code to get the connection info from the INI or txt file
        objTableLogonInfo = New CrystalDecisions.Shared.TableLogOnInfo
        objTableLogonInfo.ConnectionInfo.UserID = "DSIEmployee"
        'objTableLogonInfo.ConnectionInfo.Password = "dsi!@#"
        objTableLogonInfo.ConnectionInfo.Password = "dsi123!!"
        'objTableLogonInfo.ConnectionInfo.ServerName = "DSSQL\DSI"
        objTableLogonInfo.ConnectionInfo.ServerName = "DSSQL-Hyper"
        objTableLogonInfo.ConnectionInfo.DatabaseName = "DSI"
        objTableLogonInfo.ConnectionInfo.IntegratedSecurity = False
        objTableLogonInfo.ConnectionInfo.Type = ConnectionInfoType.SQL
        'objTableLogonInfo.ConnectionInfo.DBConnHandler = 


        '\ Loop through the tables in the database and set the connection properties of each
        'HAVE TO DO BOTH !!!!
        'Took 5 Hours to figure this out....
        'If you only apply then it will fail with 2812 error
        'If you combine in one FOR loop it fails with 2812 error
        For Each objDatabaseTable In pCrystalReport.Database.Tables
            objDatabaseTable.LogOnInfo.ConnectionInfo = objTableLogonInfo.ConnectionInfo
        Next objDatabaseTable

        For Each objDatabaseTable In pCrystalReport.Database.Tables
            objDatabaseTable.ApplyLogOnInfo(objTableLogonInfo)
        Next objDatabaseTable

        ''DOES NOT WORK WITH THIS CODE UNCOMMENTED
        '  DON'T KNOW WHY YET



        ''\ Now do the same for the subreports(if any)
        ''\ Loop through the sections in the report
        'For Each objCrSection In pCrystalReport.ReportDefinition.Sections

        '    '\ Loop through the collection
        '    For Each objCrReportObject In objCrSection.ReportObjects

        '        '\ If the report object is a subreport
        '        If objCrReportObject.Kind = CrystalDecisions.Shared.ReportObjectKind.SubreportObject Then

        '            '\ Get a reference to it
        '            objCrSubreportObject = objCrReportObject

        '            '\ Open it
        '            objCrSubreport = objCrSubreportObject.OpenSubreport(objCrSubreportObject.SubreportName)

        '            '\ Set logon info for each table in the subreport
        '            For Each objDatabaseTable In objCrSubreport.Database.Tables
        '                objDatabaseTable.ApplyLogOnInfo(objTableLogonInfo)
        '            Next objDatabaseTable

        '        End If
        '    Next objCrReportObject
        'Next objCrSection

        If pCrystalReport.FileName.Contains("Pack_Slip_Half_Size_Landscape.rpt") Then
            pCrystalReport.PrintToPrinter(2, True, 0, 0)
        ElseIf pCrystalReport.FileName.Contains("Contract_Form.rpt") Then
            'Declare a printoptions object of the objReportObjects print option.  Set the settings.
            Dim PrintOptions As CrystalDecisions.CrystalReports.Engine.PrintOptions = pCrystalReport.PrintOptions
            If glbLocation = "Athens" Then
                PrintOptions.PrinterName = glbAthens_Maint_Printer ' "\\DS2008\Maint"
            Else
                PrintOptions.PrinterName = glbGrovetown_Maint_Printer ' "\\GrovetownServer\Maint"
            End If
            PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.DefaultPaperOrientation
            PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize
            PrintOptions.PrinterDuplex = CrystalDecisions.Shared.PrinterDuplex.Vertical
            PrintOptions.PaperSource = CrystalDecisions.Shared.PaperSource.Auto
            pCrystalReport.PrintToPrinter(1, True, 0, 0)
        ElseIf pCrystalReport.FileName.Contains("SvcTicket.rpt") Or pCrystalReport.FileName.Contains("SvcHistory_Mini.rpt") Then
            'Declare a printoptions object of the objReportObjects print option.  Set the settings.
            Dim PrintOptions As CrystalDecisions.CrystalReports.Engine.PrintOptions = pCrystalReport.PrintOptions
            If glbLocation = "Athens" Then
                PrintOptions.PrinterName = glbAthens_Svc_Printer ' "\\DS2008\Srv-Slip" 
            Else
                '
                PrintOptions.PrinterName = glbGrovetown_Svc_Printer ' "\\GrovetownServer\Srv-Slip"
                PrintOptions.PaperSource = CrystalDecisions.Shared.PaperSource.Auto
            End If

            PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape
            PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperStatement
            PrintOptions.PrinterDuplex = CrystalDecisions.Shared.PrinterDuplex.Simplex
            pCrystalReport.PrintToPrinter(1, True, 0, 0)
        Else
            If pCrystalReport.Rows.Count = 0 And pCrystalReport.FileName.Contains("Invoice") Then
                'skip blank invoices
            Else
                pCrystalReport.PrintToPrinter(1, True, 0, 0)
            End If
        End If
    End Sub

    Public Sub Export_ReportToPrinter(ByRef pCrystalReport As CrystalDecisions.CrystalReports.Engine.ReportDocument, ByRef pFull_FileName_Path As String)

        Dim objTableLogonInfo As CrystalDecisions.Shared.TableLogOnInfo
        Dim objDatabaseTable As CrystalDecisions.CrystalReports.Engine.Table

        '\ Call you code to get the connection info from the INI or txt file
        objTableLogonInfo = New CrystalDecisions.Shared.TableLogOnInfo
        objTableLogonInfo.ConnectionInfo.UserID = "DSIEmployee"
        'objTableLogonInfo.ConnectionInfo.Password = "dsi!@#"
        objTableLogonInfo.ConnectionInfo.Password = "dsi123!!"
        'objTableLogonInfo.ConnectionInfo.ServerName = "DSSQL\DSI"
        objTableLogonInfo.ConnectionInfo.ServerName = "DSSQL-Hyper"
        objTableLogonInfo.ConnectionInfo.DatabaseName = "DSI"
        objTableLogonInfo.ConnectionInfo.IntegratedSecurity = False
        objTableLogonInfo.ConnectionInfo.Type = ConnectionInfoType.SQL


        '\ Loop through the tables in the database and set the connection properties of each
        'HAVE TO DO BOTH !!!!
        'Took 5 Hours to figure this out....
        'If you only apply then it will fail with 2812 error
        'If you combine in one FOR loop it fails with 2812 error
        For Each objDatabaseTable In pCrystalReport.Database.Tables
            objDatabaseTable.LogOnInfo.ConnectionInfo = objTableLogonInfo.ConnectionInfo
        Next objDatabaseTable

        For Each objDatabaseTable In pCrystalReport.Database.Tables
            objDatabaseTable.ApplyLogOnInfo(objTableLogonInfo)
        Next objDatabaseTable

        Dim txtPathOnly As String = System.IO.Path.GetDirectoryName(pFull_FileName_Path)
        If (Not System.IO.Directory.Exists(txtPathOnly)) Then
            System.IO.Directory.CreateDirectory(txtPathOnly)
        End If

        If My.Computer.FileSystem.FileExists(pFull_FileName_Path) Then
            Dim txtNewFileName As String = ""
            Dim FileInfo As New System.IO.FileInfo(pFull_FileName_Path)

            txtNewFileName = FileInfo.Name.Substring(0, FileInfo.Name.Length - 4) &
                " - old created - " & FileInfo.CreationTime.ToString.Replace(":", " ") & ".pdf"
            txtNewFileName = txtNewFileName.Replace("/", "-")
            My.Computer.FileSystem.RenameFile(pFull_FileName_Path, txtNewFileName)
        End If
        pCrystalReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pFull_FileName_Path)
    End Sub

    Public Function Read_PrintTracker_Alert_File(ByVal tmpfilename As String) As String
        Dim tmpStreamReader As System.IO.StreamReader
        Dim tmpFileInfo As New System.IO.FileInfo(tmpfilename)

        Dim strContents As String = ""

        If System.IO.File.Exists(tmpfilename) Then
            tmpStreamReader = System.IO.File.OpenText(tmpfilename)
            strContents += tmpStreamReader.ReadToEnd()
            tmpStreamReader.Close()
        End If

        Return strContents

    End Function

    Public Sub MeterHistoryAvg(ByRef txtSN As String, ByRef BW_Day_Avg As Double, ByRef Clr_Day_Avg As Double)
        'Get the machines Avg Meter usage
        Dim dtMeterAvg As DataTable
        Dim sqlString As String
        Dim NumberofDays As Double
        Dim BWCopiesMade As Double
        Dim ClrCopiesMade As Double

        If Trim(txtSN) = "" Then
            Exit Sub
        End If


        sqlString = "SELECT DISTINCT DateDiff(day, Min(MeterDate), Max(MeterDate)) AS [DifDate], " &
            "(MAX([BWMeter]) - Min([BWMeter])) AS [BWCopiesMade], " &
            "(MAX([ColorMeter]) - Min([Meter].ColorMeter)) AS [ColorCopiesMade] " &
            "From Meter " &
            "WHERE Meter.SerialNumber = '" & txtSN & "' " & "AND (Meter.MeterDate > GETDATE() - 730) and Meter.BWMeter > 0 " &
            "GROUP BY Meter.SerialNumber;"
        dtMeterAvg = SQLGetDataTable(sqlString, cnSQL)

        If dtMeterAvg.Rows.Count > 0 Then
            NumberofDays = dtMeterAvg.Rows(0)("DifDate")
            BWCopiesMade = dtMeterAvg.Rows(0)("BWCopiesMade")
            ClrCopiesMade = dtMeterAvg.Rows(0)("ColorCopiesMade")
            If NumberofDays > 0 Then
                BW_Day_Avg = BWCopiesMade / NumberofDays
                Clr_Day_Avg = ClrCopiesMade / NumberofDays
            Else
                BW_Day_Avg = 0
                Clr_Day_Avg = 0
            End If
        End If
    End Sub

    Public Sub Email_Invoices(ByRef directory_name As String)
        Dim dtTemp As DataTable
        Dim sqlstring As String
        Dim dr As DataRow
        Dim txtTo As String
        Dim dir_info As New IO.DirectoryInfo(directory_name)
        Dim file_infos As IO.FileInfo() = dir_info.GetFiles()
        Dim strHTML As String
        Dim txtPre_Subject As String = ""
        Dim x As Int16
        Dim txtProcessingFileNameOnly As String
        Dim txtSubject_Line As String = ""



        'Read Maint Invoice Directory and email out any files that exist.
        'This will try to re-try anyone who may have failed last event, also
        My.Computer.FileSystem.CurrentDirectory = directory_name
        file_infos = dir_info.GetFiles()
        For x = 0 To file_infos.Length - 1
            '            Me.Text = "Action Expediting - Treeno Upload" & "  - " & file_infos.Length.ToString & " Remaining Files"
            If file_infos(x).Exists Then
                'Email file
                txtProcessingFileNameOnly = file_infos(x).ToString.Substring(0, file_infos(x).ToString.Length - file_infos(x).Extension.Length)
                sqlstring = "SELECT InvoiceNumber, EmailedTo FROM Invoice " &
                     "WHERE Location = '" & glbLocation & "' AND InvoiceNumber = '" & txtProcessingFileNameOnly & "' AND EmailedTo > ''"
                dtTemp = SQLGetDataTable(sqlstring, cnSQL)
                For Each dr In dtTemp.Rows
                    Try
                        Dim eMailMessage As MailMessage = New MailMessage()

                        '*** Subject ***
                        txtSubject_Line = ""
                        txtSubject_Line = "DSI Invoice - " & dr("InvoiceNumber").ToString  ' & "  -- Try Again"
                        eMailMessage.Subject = txtSubject_Line


                        '*** To Email: ***
                        txtTo = ""
                        txtTo = dr("EmailedTo").ToString
                        'Email address can now have a ' in it, so I need to put it back prior to email.
                        txtTo = Replace(txtTo, "^", "'")
                        eMailMessage.To.Clear()
                        eMailMessage.To.Add(txtTo)


                        '*** Body: ***
                        strHTML = "<HTML>"
                        strHTML &= "<HEAD>"
                        strHTML &= "<BODY><body bgcolor='#d0d0d0'>"
                        'Error from 2019-07-30
                        'strHTML = strHTML & "<br><b> Sorry of the delay, but we have found that many invoices dated July 30, 2019</b><br>"
                        'strHTML = strHTML & "<br><b> where not emailed as thought.  </b><br>"
                        'strHTML = strHTML & "<br><b>   </b><br>"
                        'strHTML = strHTML & "<br><b> Please review</b><br>"
                        'strHTML = strHTML & "<br><b> Again we appology for this error.  </b><br>"
                        'strHTML = strHTML & "<br><b>   </b><br>"
                        'strHTML = strHTML & "<br><b>   </b><br>"
                        ''
                        strHTML &= "<br><b> Please see Attached Invoice for Payment.</b><br>"
                        strHTML &= "</p><br><b>Thank you for your business.</b>"
                        strHTML = strHTML & "</p><br><b>Credit Card Payments can be submitted at <a href=""" & "http://www.dsihelps.com" & """>DSIHelps.com</a></b>"
                        strHTML &= "</BODY>"
                        strHTML &= "</HTML>"

                        'Setup the mail message as an html view.
                        Dim htmlView As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(strHTML,
                                Nothing, System.Net.Mime.MediaTypeNames.Text.Html)

                        eMailMessage.AlternateViews.Add(htmlView)

                        eMailMessage.Attachments.Add(New Attachment(directory_name & dr("InvoiceNumber") & ".PDF"))

                        Send_Email(eMailMessage, "DSI Invoice Attached")

                        For Each a As Attachment In eMailMessage.Attachments
                            a.Dispose()
                        Next
                        eMailMessage.Attachments.Dispose()
                        eMailMessage.Dispose()
                        eMailMessage = Nothing
                        Try
                            My.Computer.FileSystem.DeleteFile(directory_name & dr("InvoiceNumber") & ".PDF")
                        Catch a As Exception
                            Console.WriteLine(a.ToString())
                        End Try

                        htmlView.Dispose()
                        htmlView = Nothing
                    Catch err As Exception
                        MsgBox("EXCEPTION " + err.Message + vbCrLf + vbNewLine + vbNewLine + txtSubject_Line)
                    End Try

                Next
            End If
        Next x

    End Sub

    Public Sub Send_Email(ByRef eMailMessage As MailMessage, ByRef SenderFriendlyName As String)
        Dim ErrorMessage As String
        '2019-08-28 trying to solve email not getting to usda
        'Dim smtp As New System.Net.Mail.SmtpClient("smtp.gmail.com")
        Dim smtp As New System.Net.Mail.SmtpClient("smtp-relay.gmail.com")

        smtp.EnableSsl = True
        smtp.Port = 587
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network
        Dim txtsender As String = ""
        Dim txtpass As String = ""

        If glbLocation = "Athens" Then
            txtsender = "tbliss"
            txtpass = "dsibill1969!@#" '"amblag81617"
            txtsender &= "@duplicatingsystems.com"
        Else ' Augusta
            txtsender = "kroberts"
            txtpass = "NEWme3*2020" '"moss1991"
            txtsender &= "@duplicatingsystems.com"
        End If

        'frmEmail_User_Password.ShowDialog()

        smtp.Credentials = New System.Net.NetworkCredential(txtsender, txtpass)

        'Set Sender Info
        eMailMessage.From = New System.Net.Mail.MailAddress(txtsender, SenderFriendlyName)
        'eMailMessage.ReplyToList.Clear()
        'eMailMessage.ReplyToList.Add(txtsender)
        'eMailMessage.To.Add("ken@duplicatingsystems.com")

        Try
            smtp.Send(eMailMessage)
        Catch a As SmtpException
            ErrorMessage = "Error Sending Email" & vbCrLf & "Error : " & a.Message
            If Not (a.InnerException Is Nothing) Then
                ErrorMessage = ErrorMessage & " " & a.InnerException.Message
            End If
        End Try
        smtp = Nothing

    End Sub

    Public Sub Check_AutoMeter_Import_Needed()
        Dim sqlstring As String

        DBCmd = New SqlCommand()
        DBCmd.Connection = cnSQL
        DBCmd.CommandType = CommandType.StoredProcedure
        DBCmd.CommandText = "[AR].[usp_Check_Maint_Invoice_Billed]"
        DBCmd.Parameters.Add("@pLocation", SqlDbType.VarChar, 50).Value = glbLocation
        DBCmd.Parameters.Add("@pReturn", SqlDbType.Int)
        DBCmd.Parameters("@pReturn").Direction = ParameterDirection.Output
        DBCmd.ExecuteNonQuery()
        Dim tmpBilledMaint As Integer = DBCmd.Parameters("@pReturn").Value

        If tmpBilledMaint = 0 Then
            'Me.StatusBar.Text = customTab & "Updating Meter from PT / @Remote / Canon / etc..."

            sqlstring = "EXEC [Service].[usp_Update_Seialized_atRemote_PrintTracker]"
            DBCmd = New SqlCommand(sqlstring, cnSQL)
            DBCmd.CommandTimeout = 600
            DBCmd.ExecuteNonQuery()

            sqlstring = "EXEC [Contracts].[usp_Delete_Meter_Dups]"
            DBCmd = New SqlCommand(sqlstring, cnSQL)
            DBCmd.CommandTimeout = 600
            DBCmd.ExecuteNonQuery()

        End If

    End Sub

    Public Sub Update_ControlID_From_Serialized()
        'Check_AutoMeter_Import_Needed()
        Dim sqlstring As String

        DBCmd = New SqlCommand()
        DBCmd.Connection = cnSQL
        DBCmd.CommandType = CommandType.StoredProcedure

        sqlstring = "[Contracts].[usp_Update_ControlID_From_Serialized]"
        DBCmd.CommandText = sqlstring
        DBCmd.ExecuteNonQuery()
    End Sub

    Public Sub Estimate_Meters(ByVal txtSN As String, ByRef nbrEst_BW As Integer, ByRef nbrEst_Color As Integer)
        Dim Cmd As New SqlCommand
        Cmd.CommandText = "[Service].[usp_Estimate_Meter_Reading]"
        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.Add("@pSN", SqlDbType.VarChar, 15)
        Cmd.Parameters("@pSN").Value = txtSN
        Cmd.Parameters.Add("@pEst_BW", SqlDbType.Int)
        Cmd.Parameters("@pEst_BW").Direction = ParameterDirection.Output
        Cmd.Parameters.Add("@pEst_Clr", SqlDbType.Int)
        Cmd.Parameters("@pEst_Clr").Direction = ParameterDirection.Output
        Cmd.Connection = cnSQL
        Cmd.ExecuteScalar()
        nbrEst_BW = Cmd.Parameters("@pEst_BW").Value
        nbrEst_Color = Cmd.Parameters("@pEst_Clr").Value
    End Sub

    Public Function Get_OrderDetails_Tax(ByVal txtInvoiceNumber As String, ByVal txtLocation As String) As Double
        Dim Cmd As New SqlCommand
        Cmd.CommandText = "[Contracts].[usp_TaxTotal_OrderDetail]"
        Cmd.CommandType = CommandType.StoredProcedure
        Cmd.Parameters.Add("@pInvoiceNumber", SqlDbType.VarChar, 30)
        Cmd.Parameters("@pInvoiceNumber").Value = txtInvoiceNumber
        Cmd.Parameters.Add("@pLocation", SqlDbType.VarChar, 50)
        Cmd.Parameters("@pLocation").Value = txtLocation
        Cmd.Parameters.Add("@pTaxTotal", SqlDbType.Float)
        Cmd.Parameters("@pTaxTotal").Direction = ParameterDirection.Output

        Cmd.Connection = cnSQL
        Cmd.ExecuteScalar()
        Get_OrderDetails_Tax = Cmd.Parameters("@pTaxTotal").Value
    End Function

    '' This function returns a string of all the items to submit
    '' minus the ones that have zero for both Expected & Scanned Qty
    Public Function GetString_To_Submit(ByRef sender As Object, ByRef e As EventArgs, ByRef Data_Grid As DataGridView) As String
        Dim return_String As String = ""
        For Each row As DataGridViewRow In Data_Grid.Rows
            If Not row.Cells(0).Value Is Nothing Then
                '' Value 1: Acknoledged
                '' Value 2: Empty
                '' Value 3: Area Updated
                If row.Cells(7).Value.ToString.Equals("1") Or row.Cells(7).Value.ToString.Equals("3") Then
                    '' ItemNumber,Qty,Area,Bin,Barcodes (COMMA SEPARATED)
                    '' Example: '1,qty,area,bin,111;112;113|2,qty,area,bin,2222;3333;4443'
                    return_String += row.Cells(0).Value.ToString + "," + row.Cells(2).Value.ToString + "," + row.Cells(3).Value.ToString + "," + row.Cells(4).Value.ToString + "," + row.Cells(6).Value.ToString + "|"
                End If
            End If
        Next
        '' Remove Last Pipe "|"
        Return return_String.Substring(0, return_String.Length - 1)
    End Function

    '' This function checks for rows to delete
    Public Function IS_DEL_ROW(ByRef str As String, ByRef Data_Grid As DataGridView) As Boolean
        For Each row As DataGridViewRow In Data_Grid.Rows
            If row.Cells(7).Value.ToString.Equals(str) Then
                Return True
            End If
        Next
        Return False
    End Function

    '' This function deletes rows based on the parameter str passed
    '' str -> 0: Not Acknowledged Row
    '' str -> 1: Acknowledged Row
    '' str -> 2: Rows with zero for both Expected & Scanned Rows
    Public Sub DEL_ROW(ByRef sender As Object, ByRef e As EventArgs, ByRef str As String, ByRef Data_Grid As DataGridView)
        For Each row As DataGridViewRow In Data_Grid.Rows
            If Not row.Cells(0).Value Is Nothing Then
                If row.Cells(7).Value.ToString.Equals(str) Then
                    Data_Grid.Rows.Remove(row)
                End If
            End If
        Next
    End Sub

    '' This Recursive function deletes rows
    '' This function deletes rows based on the parameter str passed
    '' str -> 0: Not Acknowledged Row
    '' str -> 1: Acknowledged Row
    '' str -> 2: Rows with zero for both Expected & Scanned Rows
    Public Sub DELETE_ROWS(ByRef sender As Object, ByRef e As EventArgs, ByRef str As String, ByRef Data_Grid As DataGridView)
        Try
            DEL_ROW(sender, e, str, Data_Grid)
            If IS_DEL_ROW(str, Data_Grid) Then
                DELETE_ROWS(sender, e, str, Data_Grid)
            End If
        Catch ex As Exception
            DELETE_ROWS(sender, e, str, Data_Grid)
        End Try
    End Sub

    '' This function updates Augusta
    '' Only updates Acknowledged rows
    '' Removes Updated rows after
    Public Sub Submit_Inventario(ByRef sender As Object, ByRef e As EventArgs, ByRef Data_Grid As DataGridView, ByRef sqlString As String, ByRef Item_Last_Add As String)
        Try
            DELETE_ROWS(sender, e, "1", Data_Grid)
            DELETE_ROWS(sender, e, "2", Data_Grid)
            DELETE_ROWS(sender, e, "3", Data_Grid)
            SQLExecuteNonQuery(sqlString, cnSQL)
            Check_Cells_Inventario(Data_Grid, Item_Last_Add)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace + Environment.NewLine + ex.Message)
        End Try
    End Sub

    '' This function turns a row
    '' Black:      if Expected Qty matches Scanned Qty
    '' LightCoral: if row hasn't been acknowledged
    '' LightGreen: if Row has been acknowledge
    '' Default (ForeColor: Black, BackColor: White)
    '' It also Updates the count 
    Public Sub Check_Cells_Inventario(ByRef Data_Grid As DataGridView, ByRef Item_Last_Add As String)
        Try
            Dim i As Integer
            ''Dim All_Check As Boolean = True
            For i = 0 To Data_Grid.Rows.Count - 1
                If Not Data_Grid.Rows(i).Cells(0).Value Is Nothing Then
                    '' Highlight Missing Inventory (Expected Qty = Scanned Qty = 0)
                    If Data_Grid.Rows(i).Cells(1).Value = Data_Grid.Rows(i).Cells(2).Value And Data_Grid.Rows(i).Cells(1).Value.ToString.Equals("0") Then
                        '' Set Empty
                        Data_Grid.Rows(i).Cells(7).Value = 2
                    End If

                    If Data_Grid.Rows(i).Cells(2).Value = 0 And Data_Grid.Rows(i).Cells(1).Value > Data_Grid.Rows(i).Cells(2).Value Then
                        '' Highlight Red
                        Data_Grid.Rows(i).DefaultCellStyle.BackColor = Color.LightCoral
                    ElseIf Not Data_Grid.Rows(i).Cells(2).Value = 0 And Data_Grid.Rows(i).Cells(1).Value > Data_Grid.Rows(i).Cells(2).Value Then
                        '' Highlight Green
                        Data_Grid.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                    ElseIf Data_Grid.Rows(i).Cells(1).Value = Data_Grid.Rows(i).Cells(2).Value Then
                        '' Black:      if Expected Qty matches Scanned Qty
                        Data_Grid.Rows(i).DefaultCellStyle.BackColor = Color.Black
                        Data_Grid.Rows(i).DefaultCellStyle.ForeColor = Color.White
                    Else
                        '' Default
                        Data_Grid.Rows(i).DefaultCellStyle.BackColor = Color.White
                        Data_Grid.Rows(i).DefaultCellStyle.ForeColor = Color.Black
                    End If

                    If Not Item_Last_Add.Equals("") And Data_Grid.Rows(i).Cells(0).Value.ToString.Equals(Item_Last_Add) Then
                        '' Item_Last_Added is colored Crimson & Visible to the user
                        Data_Grid.Rows(i).DefaultCellStyle.BackColor = Color.Crimson
                        '' Make it visible
                        Data_Grid.FirstDisplayedScrollingRowIndex = i
                        '' Acknowledge Last_Item_Add
                        Data_Grid.Rows(i).Cells(7).Value = 1
                    End If
                Else
                    '' Delete Empty Rows
                    Data_Grid.Rows.RemoveAt(i)
                End If
            Next
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
        End Try
    End Sub

    '' This function adjust a rows Qty with the provided Qty from Quantity_Tb
    Public Sub Adjust_Quantity_Inventario(ByRef sender As Object, ByRef e As EventArgs, ByRef Data_Grid As DataGridView, ByRef Dictionary_Actual As Dictionary(Of String, Integer), ByRef Item_Last_Add As String, ByRef Quantity_Tb As TextBox, ByRef Alert_Tb As TextBox, ByRef FULL_ITEM_NUMBER As String)
        '' Validate Input
        If Quantity_Tb.Text = "" Then
            Exit Sub 'GoTo EventExitSub
        End If

        '' Clear Alert_Tb
        Alert_Tb.Text = ""

        Try
            If Dictionary_Actual.ContainsKey(Item_Last_Add) Then
                If IsNumeric(Quantity_Tb.Text) Then
                    Dictionary_Actual(Item_Last_Add) = CInt(Quantity_Tb.Text)
                    Quantity_Tb.Text = ""
                    Redrawn_Inventario(Data_Grid, Dictionary_Actual, Item_Last_Add, FULL_ITEM_NUMBER)
                    Check_Cells_Inventario(Data_Grid, Item_Last_Add)
                Else
                    Alert_Tb.Text = "Incorrect Input!"
                End If
            Else
                Alert_Tb.Text = "Null Pointer Exception!"
            End If
        Catch ex As Exception
            Alert_Tb.Text = ex.StackTrace
        End Try
    End Sub

    '' This functions redraws the DataGridView 
    '' More specifically, it adjusts data between the dictionary & the DataGridView
    '' A dictionary is used to avoid having to iterate through the data
    Public Sub Redrawn_Inventario(ByRef Data_Grid As DataGridView, ByRef Dictionary_Actual As Dictionary(Of String, Integer), ByRef Item_Last_Add As String, ByRef FULL_ITEM_NUMBER As String)
        '' Enable EditProgrammatically
        Data_Grid.EditMode = DataGridViewEditMode.EditProgrammatically
        Try
            Dim row As DataGridViewRow
            For Each row In Data_Grid.Rows
                If Not row.Cells("ItemNumber").Value Is Nothing Then
                    '' Adjust 
                    If Dictionary_Actual.ContainsKey(row.Cells("ItemNumber").Value) Then
                        '' Change Value
                        row.Cells("Scanned_Qty").Value = Dictionary_Actual(row.Cells("ItemNumber").Value.ToString)
                    End If

                    '' Add Full_Item_Number to Barcodes
                    If row.Cells("Barcodes").Value.ToString.Equals("") Then
                        If row.Cells("ItemNumber").Value.ToString.Equals(FULL_ITEM_NUMBER) Or row.Cells("ItemNumber").Value.ToString.Equals(FULL_ITEM_NUMBER.Substring(0, FULL_ITEM_NUMBER.Length - 1)) Or row.Cells("ItemNumber").Value.ToString.Equals(FULL_ITEM_NUMBER.Substring(0, FULL_ITEM_NUMBER.Length - 2)) Then
                            If FULL_ITEM_NUMBER.Equals(Item_Last_Add) Then
                                row.Cells("Barcodes").Value = FULL_ITEM_NUMBER
                            Else
                                row.Cells("Barcodes").Value = FULL_ITEM_NUMBER + ";" + Item_Last_Add
                            End If
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            ''Alert_Tb.Text = ex.StackTrace
            Console.WriteLine("**********************************************************************************" + ex.StackTrace)
        End Try

        '' Disable EditProgrammatically
        Data_Grid.EditMode = DataGridViewEditMode.EditOnEnter
        Check_Cells_Inventario(Data_Grid, Item_Last_Add)
    End Sub

    Public Function Check_Item_Number_Inventario(ByRef x As Integer, ByRef FULL_ITEM_NUMBER As String, ByRef ITEM_NUMBER As String, ByRef BARCODES_DICT As Dictionary(Of String, String)) As Boolean

        '' Hold Value
        FULL_ITEM_NUMBER = ITEM_NUMBER

        If BARCODES_DICT.ContainsKey(ITEM_NUMBER) Then
            ITEM_NUMBER = BARCODES_DICT(ITEM_NUMBER)
            Return True
        End If

        Dim Data_Table As DataTable
        While x > 0
            Data_Table = GetItem("ItemNumber", ITEM_NUMBER)
            If Data_Table.Rows.Count = 0 Then
                '' Not a valid number
                '' Delete last index & try agin
                x -= 1
                ITEM_NUMBER = ITEM_NUMBER.Substring(0, ITEM_NUMBER.Length - 1)
                Continue While
            Else
                '' Found a valid number
                Return True
            End If
        End While
        Return False
    End Function

    '' Helper Function
    '' This function checks if the scanned item is valid
    '' If not, decrement and try again
    Public Sub Item_Number_Tb_Validating_Inventario_(ByRef sender As Object, ByRef x As Integer, ByRef FULL_ITEM_NUMBER As String, ByRef Item_Last_Add As String, ByRef ITEM_NUMBER As String, ByRef Alert_Tb As TextBox, ByRef Data_Grid As DataGridView, ByRef BARCODES_DICT As Dictionary(Of String, String), ByRef Dictionary_Expected As Dictionary(Of String, Integer), ByRef Dictionary_Actual As Dictionary(Of String, Integer))
        ''Dim found As Boolean = False
        Alert_Tb.Text = ""
        Try
            If Check_Item_Number_Inventario(x, FULL_ITEM_NUMBER, ITEM_NUMBER, BARCODES_DICT) Then
                If Dictionary_Expected.ContainsKey(ITEM_NUMBER) Then
                    '' This If Statement makes the difference between Item_Number_Tb_Validating_Inventario & Item_Number_Tb_Validating_Inventario_
                    If Not Dictionary_Actual.ContainsKey(ITEM_NUMBER) Then
                        Dictionary_Actual.Add(ITEM_NUMBER, 1)
                        ''Console.WriteLine("Added To Actual: " + ITEM_NUMBER)
                    End If
                    Item_Last_Add = ITEM_NUMBER
                Else
                    '' Item is at the wrong area
                    Alert_Tb.Text = UCase("This item is at the wrong area!")
                    '' Alert
                    Play_Error_Sound()
                End If
                Redrawn_Inventario(Data_Grid, Dictionary_Actual, Item_Last_Add, FULL_ITEM_NUMBER)
            Else
                Alert_Tb.Text = UCase("Item Not Found!")
                Play_Error_Sound()
            End If

        Catch ex As Exception
            ''Alert_Tb.Text = ex.StackTrace
        End Try
    End Sub

    '' Helper Function
    '' This function checks if the scanned item is valid
    '' If not, decrement and try again
    Public Sub Item_Number_Tb_Validating_Inventario(ByRef sender As Object, ByRef e As EventArgs, ByRef x As Integer, ByRef FULL_ITEM_NUMBER As String, ByRef Item_Last_Add As String, ByRef ITEM_NUMBER As String, ByRef Alert_Tb As TextBox, ByRef Data_Grid As DataGridView, ByRef BARCODES_DICT As Dictionary(Of String, String), ByRef Dictionary_Expected As Dictionary(Of String, Integer), ByRef Dictionary_Actual As Dictionary(Of String, Integer))
        ''Dim found As Boolean = False
        Alert_Tb.Text = ""
        Try
            If Check_Item_Number_Inventario(x, FULL_ITEM_NUMBER, ITEM_NUMBER, BARCODES_DICT) Then
                If Dictionary_Expected.ContainsKey(ITEM_NUMBER) Then
                    '' Item is at the right area
                    If Dictionary_Actual.ContainsKey(ITEM_NUMBER) Then
                        '' Increment
                        Dictionary_Actual(ITEM_NUMBER) += 1
                    Else
                        Dictionary_Actual.Add(ITEM_NUMBER, 1)
                        ''Console.WriteLine("Added To Actual: " + ITEM_NUMBER)
                    End If
                    Item_Last_Add = ITEM_NUMBER
                Else
                    '' Item is at the wrong area
                    Alert_Tb.Text = UCase("This item is at the wrong area!")
                    '' Alert
                    Play_Error_Sound()
                End If
                Redrawn_Inventario(Data_Grid, Dictionary_Actual, Item_Last_Add, FULL_ITEM_NUMBER)
            Else
                Alert_Tb.Text = UCase("Item Not Found!")
                Play_Error_Sound()
            End If

        Catch ex As Exception
            ''Alert_Tb.Text = ex.StackTrace
        End Try
    End Sub

    '' This function plays a wav file
    Public Sub Play_Error_Sound()
        My.Computer.Audio.Play("H:\\AUGUSTA.net\\PackingSlip_Scans_Program\\Incorrect_Entry_Sound.wav", AudioPlayMode.WaitToComplete)
    End Sub

    Public Sub Init_Data_Inventario(ByRef Data_Grid As DataGridView, ByRef Alert_Tb As TextBox, ByRef BARCODES_DICT As Dictionary(Of String, String), ByRef Dictionary_Expected As Dictionary(Of String, Integer))
        '' Enable EditProgrammatically
        Data_Grid.EditMode = DataGridViewEditMode.EditProgrammatically
        Try
            Dim row As DataGridViewRow
            For Each row In Data_Grid.Rows
                If Not row.Cells("ItemNumber").Value Is Nothing Then
                    row.Cells("Binary").Value = 0
                    '' Add to Expected Dictionary
                    If Not Dictionary_Expected.ContainsKey(row.Cells("ItemNumber").Value.ToString) Then
                        '' If True: Add
                        Dictionary_Expected.Add(row.Cells("ItemNumber").Value.ToString, row.Cells("Expected_Qty").Value)
                    End If

                    If Not row.Cells("Barcodes").Value.ToString.Equals("") Then
                        ''Console.WriteLine
                        Dim barcodes As String() = row.Cells("Barcodes").Value.ToString.Split(New Char() {";"c})
                        Dim barcode As String
                        For Each barcode In barcodes
                            If Not BARCODES_DICT.ContainsKey(barcode) Then
                                BARCODES_DICT.Add(barcode, row.Cells("ItemNumber").Value.ToString)
                            End If
                        Next
                    End If

                    If row.Cells("ItemNumber").Value.ToString.Contains("-") Then
                        If Not BARCODES_DICT.ContainsKey(row.Cells("ItemNumber").Value.ToString.Replace("-", "")) Then
                            BARCODES_DICT.Add(row.Cells("ItemNumber").Value.ToString.Replace("-", ""), row.Cells("ItemNumber").Value.ToString)
                        End If
                    End If

                End If
            Next
        Catch ex As Exception
            Alert_Tb.Text = ex.Message
        End Try

        '' Disable EditProgrammatically
        Data_Grid.EditMode = DataGridViewEditMode.EditOnEnter
    End Sub

    '' This functions acknowledges a row
    '' A row is acknowledged if user changes the Scanned_Qty
    '' by either cliking on the cell or Scanning the Item's barcode
    Public Sub Cell_Acknowledgement(ByRef index As Integer, ByRef Data_Grid As DataGridView)
        For Each row As DataGridViewRow In Data_Grid.Rows
            If Not row.Cells(0).Value Is Nothing Then
                If row.Index = index Then
                    row.Cells(7).Value = 1
                End If
            End If
        Next
    End Sub


    Public Sub Cell_Acknowledgement_(ByRef ROW_BEFORE As DataGridViewRow, ByRef ROW_AFTER As DataGridViewRow, ByRef Data_Grid As DataGridView)
        Try
            If ROW_BEFORE.Cells(0).Value.ToString.Equals(ROW_AFTER.Cells(0).Value.ToString) Then
                For Each row As DataGridViewRow In Data_Grid.Rows
                    If Not row.Cells(0).Value Is Nothing Then
                        '' Was Scanned_Qty Changed?
                        If Not ROW_BEFORE.Cells(2).Value = ROW_AFTER.Cells(2).Value Then
                            row.Cells(7).Value = 1
                        End If
                        '' Was Area Updated?
                        If Not ROW_BEFORE.Cells(4).Value = ROW_AFTER.Cells(4).Value Or Not ROW_BEFORE.Cells(5).Value = ROW_AFTER.Cells(5).Value Then
                            row.Cells(7).Value = 3
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            Console.WriteLine("*****************************Method: Cell_Acknowledgement: " + ex.StackTrace + Environment.NewLine + ex.Message)
        End Try
    End Sub

    '' This function copies elements of Item_Number Column to Barcode Column without "-" if present
    Public Sub Copy_Barcode(ByRef sender As Object, ByRef e As EventArgs, ByRef BARCODES_DICT As Dictionary(Of String, String), ByRef Data_Grid As DataGridView)
        Try
            Dim temp_Str As String = ""
            For Each row As DataGridViewRow In Data_Grid.Rows
                If Not row.Cells(0).Value Is Nothing Then
                    temp_Str = row.Cells(0).Value.ToString.Replace("-", "")
                    row.Cells(6).Value = temp_Str
                    If Not BARCODES_DICT.ContainsKey(temp_Str) Then
                        BARCODES_DICT.Add(temp_Str, row.Cells("ItemNumber").Value.ToString)
                    End If
                End If
            Next
        Catch ex As Exception
            Console.WriteLine("*****************************Method: Copy_Barcode: " + ex.StackTrace + Environment.NewLine + ex.Message)
        End Try
    End Sub

    '' This function sets Scanned_Qty to Expected_Qty
    Public Sub Fast_Process(ByRef sender As Object, ByRef e As EventArgs, ByRef BARCODES_DICT As Dictionary(Of String, String), ByRef Data_Grid As DataGridView)
        Try
            '' Total Item Count
            Dim total_Count As Integer = 0
            For Each row As DataGridViewRow In Data_Grid.Rows
                '' Count every item
                If Not row.Cells(0).Value Is Nothing Then
                    total_Count += row.Cells(1).Value
                End If
            Next

            '' Prompt User
            Dim error_Dialog As DialogResult = MessageBox.Show("Inventario" + Environment.NewLine + "Do you have " + Data_Grid.Rows.Count.ToString + "  specific Items for a Total Count of " + total_Count.ToString + " Items?", "Augusta", MessageBoxButtons.YesNo)
            If error_Dialog = vbYes Then
                For Each row As DataGridViewRow In Data_Grid.Rows
                    If Not row.Cells(0).Value Is Nothing Then
                        '' Fast Processing
                        '' User still need to acknowledge each item
                        row.Cells(2).Value = row.Cells(1).Value
                        ''row.Cells(7).Value = 1
                    End If
                Next
            Else
                Exit Sub
            End If
        Catch ex As Exception
            Console.WriteLine("*****************************Method: Fast_Process: " + ex.StackTrace + Environment.NewLine + ex.Message)
        End Try
    End Sub

    '' This method provides a report to the user
    Public Sub GetReport(ByRef sender As Object, ByRef e As EventArgs, ByRef Data_Grid As DataGridView)
        Try
            '' Quick Check 
            If Data_Grid.Rows.Count = 0 Then
                Exit Sub
            End If

            '' Report
            Dim report As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer)
            report.Add("EMPTY", 0)
            report.Add("LESS", 0)
            report.Add("MORE", 0)
            report.Add("FOUND", 0)
            report.Add("EQUAL", 0)
            report.Add("AREA_UPDATE", 0)

            Dim report_Empty_Str As String = Environment.NewLine
            Dim report_Less_Str As String = Environment.NewLine
            Dim report_More_Str As String = Environment.NewLine
            Dim report_Equal_Str As String = Environment.NewLine
            Dim report_Area_Update_Str As String = Environment.NewLine

            Dim expected_count As Integer = 0
            Dim scanned_count As Integer = 0

            For Each row As DataGridViewRow In Data_Grid.Rows
                If Not row.Cells(0).Value Is Nothing Then
                    expected_count += row.Cells(1).Value
                    scanned_count += row.Cells(2).Value
                    If row.Cells(1).Value = row.Cells(2).Value Then
                        If row.Cells(1).Value = 0 Then
                            report("EMPTY") += 1
                            report_Empty_Str += "Item: " + row.Cells(0).Value.ToString + "Expected_Qty: " + row.Cells(1).Value.ToString + " Scanned_Qty: " + row.Cells(2).Value.ToString + " Desciption: " + row.Cells(5).Value.ToString + " Barcode: " + row.Cells(6).Value.ToString + Environment.NewLine
                        Else
                            report("EQUAL") += 1
                            report_Equal_Str += "Item: " + row.Cells(0).Value.ToString + "Expected_Qty: " + row.Cells(1).Value.ToString + " Scanned_Qty: " + row.Cells(2).Value.ToString + " Desciption: " + row.Cells(5).Value.ToString + " Barcode: " + row.Cells(6).Value.ToString + Environment.NewLine
                        End If
                    End If
                    If row.Cells(1).Value > row.Cells(2).Value And row.Cells(1).Value > 0 Then
                        report("LESS") += 1
                        report_Less_Str += "Item: " + row.Cells(0).Value.ToString + "Expected_Qty: " + row.Cells(1).Value.ToString + " Scanned_Qty: " + row.Cells(2).Value.ToString + " Desciption: " + row.Cells(5).Value.ToString + " Barcode: " + row.Cells(6).Value.ToString + Environment.NewLine
                    End If
                    If row.Cells(1).Value < row.Cells(2).Value And row.Cells(1).Value > 0 Then
                        report("MORE") += 1
                        report_More_Str += "Item: " + row.Cells(0).Value.ToString + "Expected_Qty: " + row.Cells(1).Value.ToString + " Scanned_Qty: " + row.Cells(2).Value.ToString + " Desciption: " + row.Cells(5).Value.ToString + " Barcode: " + row.Cells(6).Value.ToString + Environment.NewLine
                    End If
                    If row.Cells(1).Value < row.Cells(2).Value And row.Cells(1).Value = 0 Then
                        report("FOUND") += 1
                        report_More_Str += "Item: " + row.Cells(0).Value.ToString + "Expected_Qty: " + row.Cells(1).Value.ToString + " Scanned_Qty: " + row.Cells(2).Value.ToString + " Desciption: " + row.Cells(5).Value.ToString + " Barcode: " + row.Cells(6).Value.ToString + Environment.NewLine
                    End If
                    If row.Cells(7).Value = 3 Then
                        report("AREA_UPDATE") += 1
                        report_Area_Update_Str += "Item: " + row.Cells(0).Value.ToString + "Expected_Qty: " + row.Cells(1).Value.ToString + " Scanned_Qty: " + row.Cells(2).Value.ToString + " Desciption: " + row.Cells(5).Value.ToString + " Barcode: " + row.Cells(6).Value.ToString + Environment.NewLine
                    End If
                End If
            Next

            '' CStr((scanned_count / expected_count) * 100)
            Dim report_str = "REPORT: " + CStr((scanned_count / expected_count) * 100) + " Accuracy" + Environment.NewLine +
                "Expected_Qty =  Scanned_Qty = && Expected_Qty = 0 (Empty): " + report("EMPTY").ToString + Environment.NewLine +
                "Expected_Qty <  Scanned_Qty && Expected_Qty = 0 (Found): " + report("FOUND").ToString + Environment.NewLine +
                "Expected_Qty >  Scanned_Qty: " + report("LESS").ToString + Environment.NewLine +
                "Expected_Qty <  Scanned_Qty: " + report("MORE").ToString + Environment.NewLine +
                "Expected_Qty =  Scanned_Qty(Match): " + report("EQUAL").ToString + Environment.NewLine +
                "Area Updates: " + report("AREA_UPDATE").ToString + Environment.NewLine

            report_str += "REPORT" + CStr((scanned_count / expected_count) * 100) + " Accuracy" + Environment.NewLine +
                "Expected_Qty =  Scanned_Qty = && Expected_Qty = 0 (Empty): " + report("EMPTY").ToString + report_Empty_Str + Environment.NewLine +
                "Expected_Qty <  Scanned_Qty && Expected_Qty = 0 (Found): " + report("EMPTY").ToString + report_Empty_Str + Environment.NewLine +
                "Expected_Qty >  Scanned_Qty: " + report("LESS").ToString + report_Less_Str + Environment.NewLine +
                "Expected_Qty <  Scanned_Qty: " + report("MORE").ToString + report_More_Str + Environment.NewLine +
                "Expected_Qty =  Scanned_Qty(Match): " + report("EQUAL").ToString + Environment.NewLine +
                "Area Updates: " + report("AREA_UPDATE").ToString + report_Area_Update_Str + Environment.NewLine

            '' Show Alert
            Dim error_Dialog As DialogResult = MessageBox.Show(report_str, "Augusta", MessageBoxButtons.YesNo)

        Catch ex As Exception
            Console.WriteLine("*****************************Method: GetReport: " + ex.StackTrace + Environment.NewLine + ex.Message)
        End Try
    End Sub

    Public Function isEmpty_Cells(ByRef sender As Object, ByRef e As EventArgs, ByRef Data_Grid As DataGridView)
        Try
            For Each row As DataGridViewRow In Data_Grid.Rows
                If row.Cells(3).Value.ToString.Equals("") Or row.Cells(4).Value.ToString.Equals("") Then
                    Return True
                End If
            Next

        Catch ex As Exception
            Console.WriteLine("*****************************Method: isEmpty_Cells: " + ex.StackTrace + Environment.NewLine + ex.Message)
        End Try
        Return False
    End Function

    '' This Function prints a Report
    Public Sub Print_Report(ByRef po_number As String)
        Dim intResult As MsgBoxResult
        intResult = MsgBox("Print Puchase Order Receipt?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Print receipt")
        If intResult = MsgBoxResult.Yes Then
            Load_Report("PoRec.rpt")
            With glbReport
                .SetParameterValue("@pLocation", glbLocation)
                .SetParameterValue("@pPoNumber", po_number)
            End With
            Print_ReportToPrinter(glbReport)
            '' Is it done printing?
            intResult = MsgBox("IS IT DONE PRINTING? CLICK YES WHEN PRINTER IS DONE", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Print Status")
            If intResult = MsgBoxResult.Yes Then
                Dim sqlString As String = "DELETE TempPoD WHERE PONumber = '" & po_number & "' AND Location = '" & glbLocation & "'"
                SQLExecuteNonQuery(sqlString, cnSQL)
                sqlString = "DELETE TempPoM WHERE PONumber = '" & po_number & "' AND Location = '" & glbLocation & "'"
                SQLExecuteNonQuery(sqlString, cnSQL)
            End If
        End If
    End Sub
End Module