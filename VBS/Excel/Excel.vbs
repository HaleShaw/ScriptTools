Sub ExcelScript()
	Set objExcel = CreateObject("Excel.Application")
	Set objWorkbook = objExcel.Workbooks.Open("C:\Users\username\Desktop\test.xlsx")
	objExcel.Visible = True

	' Copy Column
	For i=5 To 1084 Step 4
		objExcel.Worksheets(6).Columns(1).Copy (objExcel.Worksheets(6).Columns(i))
		objExcel.Worksheets(6).Columns(4).Copy (objExcel.Worksheets(6).Columns(i+3))
	Next
	
	' Copy Range
	For i=117 To 51040 Step 110
		objExcel.Worksheets(6).Range("E1:H110").Copy
		objExcel.Worksheets(6).Range("A"&i).PasteSpecial
		
		' Delete Column
		objExcel.Worksheets(6).Range("E1").EntireColumn.Delete
		objExcel.Worksheets(6).Range("E1").EntireColumn.Delete
		objExcel.Worksheets(6).Range("E1").EntireColumn.Delete
		objExcel.Worksheets(6).Range("E1").EntireColumn.Delete
	Next
	
	' Set Cell Value
	For i=2 To 1729 Step 4
		objExcel.Cells(1,i).Value = objExcel.Cells(1,i).Value + objExcel.Cells(116,i).Value
		objExcel.Cells(19,i).Value = objExcel.Cells(19,i).Value + objExcel.Cells(109,i).Value
		objExcel.Cells(24,i).Value = objExcel.Cells(24,i).Value + objExcel.Cells(111,i).Value
		objExcel.Cells(26,i).Value = objExcel.Cells(26,i).Value + objExcel.Cells(112,i).Value
		objExcel.Cells(41,i).Value = objExcel.Cells(41,i).Value + objExcel.Cells(113,i).Value
		objExcel.Cells(13,i).Value = objExcel.Cells(13,i).Value + objExcel.Cells(114,i).Value
	Next

End Sub
