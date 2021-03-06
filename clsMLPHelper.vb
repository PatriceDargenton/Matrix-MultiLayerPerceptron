﻿
Imports System.ComponentModel ' DescriptionAttribute
Imports System.Text ' StringBuilder

Public Class clsMLPHelper

    Public Shared Function GetVector(array2D!(,), index%) As Single()

        Dim length = array2D.GetLength(1)
        Dim vect!(0 To length - 1)
        For k = 0 To length - 1
            vect(k) = array2D(index, k)
        Next
        Return vect

    End Function

    Public Shared Function GetColumn(array2D!(,), index%) As Single()

        Dim length = array2D.GetLength(0)
        Dim vect!(0 To length - 1)
        For k = 0 To length - 1
            vect(k) = array2D(k, index)
        Next
        Return vect

    End Function

    Public Shared Sub Fill2DArrayOfSingle(array2D!(,), array1D!(), index%)
        Dim nbItems = array1D.GetLength(0)
        For j = 0 To nbItems - 1
            array2D(index, j) = array1D(j)
        Next
    End Sub

    Public Shared Sub Fill2DArrayOfSingle2(array2D!(,), array1D!(), index%)
        Dim nbItems = array1D.GetLength(0)
        For j = 0 To nbItems - 1
            array2D(j, index) = array1D(j)
        Next
    End Sub

    Public Shared Sub Fill2DArrayOfDoubleByArrayOfSingle(array2D#(,), array1D!(), index%)
        Dim nbItems = array1D.GetLength(0)
        For j = 0 To nbItems - 1
            array2D(index, j) = array1D(j)
        Next
    End Sub

    Public Shared Sub Fill2DArrayOfDoubleByArrayOfSingle2(array2D#(,), array1D!(), index%)
        Dim nbItems = array2D.GetLength(1)
        For j = 0 To nbItems - 1
            array2D(index, j) = array1D(j + index * nbItems)
        Next
    End Sub

    Public Shared Sub Fill2DArrayOfDoubleByArray(array2D#(,), array1D#(), index%)
        Dim nbItems = array2D.GetLength(1)
        For j = 0 To nbItems - 1
            array2D(index, j) = array1D(j + index * nbItems)
        Next
    End Sub

    Public Shared Sub Fill2DArrayOfDouble(array2D#(,), array1D#(), index%)
        Dim nbItems = array2D.GetLength(1)
        For j = 0 To nbItems - 1
            array2D(index, j) = array1D(j)
        Next
    End Sub

    Public Shared Function Convert2DArrayOfSingleToDouble(array2D!(,)) As Double(,)
        Dim length0 = array2D.GetLength(0)
        Dim length1 = array2D.GetLength(1)
        Dim arr#(0 To length0 - 1, 0 To length1 - 1)
        For i = 0 To length0 - 1
            For j = 0 To length1 - 1
                arr(i, j) = array2D(i, j)
            Next
        Next
        Return arr
    End Function

    Public Shared Function Convert1DArrayOfSingleToDouble(array1D!()) As Double()
        Dim length0 = array1D.GetLength(0)
        Dim arr#(0 To length0 - 1)
        For i = 0 To length0 - 1
            arr(i) = array1D(i)
        Next
        Return arr
    End Function

    Public Shared Function Convert1DArrayOfDoubleToSingle(array1D#()) As Single()
        Dim length0 = array1D.GetLength(0)
        Dim arr!(0 To length0 - 1)
        For i = 0 To length0 - 1
            arr(i) = CSng(array1D(i))
        Next
        Return arr
    End Function

    Public Shared Function Transform2DArrayToJaggedArray(array2D#(,)) As Double()()

        ' Transform a 2D array into a jagged array

        Dim length0 = array2D.GetLength(0)
        Dim length1 = array2D.GetLength(1)
        Dim arr As Double()() = New Double(length0 - 1)() {}
        For i = 0 To length0 - 1
            arr(i) = New Double() {}
            ReDim arr(i)(length1 - 1)
            For j = 0 To length1 - 1
                arr(i)(j) = array2D(i, j)
            Next j
        Next i

        Return arr

    End Function

    Public Shared Function Transform2DArrayToJaggedArraySingle(array2D!(,)) As Single()()

        ' Transform a 2D array into a jagged array

        Dim length0 = array2D.GetLength(0)
        Dim length1 = array2D.GetLength(1)
        Dim arr As Single()() = New Single(length0 - 1)() {}
        For i = 0 To length0 - 1
            arr(i) = New Single() {}
            ReDim arr(i)(length1 - 1)
            For j = 0 To length1 - 1
                arr(i)(j) = array2D(i, j)
            Next j
        Next i

        Return arr

    End Function

    Public Shared Function Transform2DArrayDoubleToJaggedArraySingle(array2D#(,)) As Single()()

        ' Transform a 2D array into a jagged array

        Dim length0 = array2D.GetLength(0)
        Dim length1 = array2D.GetLength(1)
        Dim arr As Single()() = New Single(length0 - 1)() {}
        For i = 0 To length0 - 1
            arr(i) = New Single() {}
            ReDim arr(i)(length1 - 1)
            For j = 0 To length1 - 1
                arr(i)(j) = CSng(array2D(i, j))
            Next j
        Next i

        Return arr

    End Function

    Public Shared Function Transform2DArrayDoubleToArraySingle(array2D#(,)) As Single()

        ' Transform a 2D array into an array

        Dim length0 = array2D.GetLength(0)
        Dim length1 = array2D.GetLength(1)
        Dim arr!(length0 * length1 - 1)
        For i = 0 To length0 - 1
            For j = 0 To length1 - 1
                arr(i * length1 + j) = CSng(array2D(i, j))
            Next j
        Next i

        Return arr

    End Function

    Public Shared Function Transform2DArrayDoubleToArraySingle2(array2D#(,)) As Single()

        ' Transform a 2D array into an array

        Dim length0 = array2D.GetLength(0)
        Dim length1 = array2D.GetLength(1)
        Dim arr!(length0 * length1 - 1)
        For i = 0 To length1 - 1
            For j = 0 To length0 - 1
                arr(i * length1 + j) = CSng(array2D(j, i))
            Next j
        Next i

        Return arr

    End Function

    ' Inspired from:
    ' https://stackoverflow.com/questions/26291609/converting-jagged-array-to-2d-array-c-sharp
    Public Shared Function TransformArrayTo2DArray(Of T)(ByVal source() As T,
                firstDim%, secondDim%) As T(,)

        Try
            Dim result = New T(firstDim - 1, secondDim - 1) {}
            Dim i% = 0
            Do While i < firstDim
                Dim j% = 0
                Do While j < secondDim
                    result(i, j) = source(i * secondDim + j)
                    j += 1
                Loop
                i += 1
            Loop
            Return result
        Catch ex As InvalidOperationException
            Throw New InvalidOperationException("Wrong size!")
        End Try

    End Function

    Public Shared Function Swap2DArray(array2D#(,)) As Double(,)

        Dim length0 = array2D.GetLength(0)
        Dim length1 = array2D.GetLength(1)
        Dim arr#(length1 - 1, length0 - 1)
        For i = 0 To length0 - 1
            For j = 0 To length1 - 1
                arr(j, i) = array2D(i, j)
            Next j
        Next i

        Return arr

    End Function

    Public Shared Function Compare(val1#, val2#, dec%) As Boolean

        If Double.IsNaN(val1) AndAlso Double.IsNaN(val2) Then Return True
        Dim delta# = Math.Abs(Math.Round(val2 - val1, dec))
        If delta = 0 Then Return True
        Return False

    End Function

    Public Shared Function CompareArray(array2Da!(,), array2Db!(,)) As Boolean
        If IsNothing(array2Da) Then Return False
        If IsNothing(array2Db) Then Return False
        Dim length0a = array2Da.GetLength(0)
        Dim length1a = array2Da.GetLength(1)
        Dim length0b = array2Db.GetLength(0)
        Dim length1b = array2Db.GetLength(1)
        If length0a <> length0b Then Return False
        If length1a <> length1b Then Return False
        For i = 0 To length0a - 1
            For j = 0 To length1a - 1
                If array2Da(i, j) <> array2Db(i, j) Then Return False
            Next
        Next
        Return True
    End Function

    Public Shared Function CompareArray1D(array1Da#(), array1Db#()) As Boolean
        If IsNothing(array1Da) Then Return False
        If IsNothing(array1Db) Then Return False
        Dim length0a = array1Da.GetLength(0)
        Dim length0b = array1Db.GetLength(0)
        If length0a <> length0b Then Return False
        For i = 0 To length0a - 1
            Dim va = array1Da(i)
            Dim vb = array1Db(i)
            If va <> vb Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Shared Function CompareArray1DSingle(array1Da!(), array1Db!(),
        Optional lengthMax% = 0, Optional startingFrom% = 0) As Boolean
        If IsNothing(array1Da) Then Return False
        If IsNothing(array1Db) Then Return False
        Dim length0a = array1Da.GetLength(0)
        Dim length0b = array1Db.GetLength(0)
        If lengthMax > 0 AndAlso length0a > lengthMax Then length0a = lengthMax
        If lengthMax > 0 AndAlso length0b > lengthMax Then length0b = lengthMax
        If length0a <> length0b Then Return False
        For i = 0 To length0a - 1
            Dim va = array1Da(i)
            Dim vb = array1Db(i + startingFrom)
            If va <> vb Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Shared Function ReadEnumDescription$(myEnum As [Enum])

        Dim fi As Reflection.FieldInfo = myEnum.GetType().GetField(myEnum.ToString())
        Dim attr() As DescriptionAttribute = DirectCast(
           fi.GetCustomAttributes(GetType(DescriptionAttribute), False),
           DescriptionAttribute())
        If attr.Length > 0 Then
            Return attr(0).Description
        Else
            Return myEnum.ToString()
        End If

    End Function

    Public Shared Function ArrayToString$(singleArray!())
        Dim sb As New StringBuilder
        For i = 0 To singleArray.GetUpperBound(0)
            sb.Append(singleArray(i).ToString("0.00") & " ")
        Next
        Return sb.ToString
    End Function

    Public Shared Function ArrayToString$(intArray%())
        Dim sb As New StringBuilder("{")
        Dim upB = intArray.GetUpperBound(0)
        For i = 0 To upB
            sb.Append(intArray(i))
            If i < upB Then sb.Append(", ")
        Next
        sb.Append("}")
        Return sb.ToString
    End Function

End Class