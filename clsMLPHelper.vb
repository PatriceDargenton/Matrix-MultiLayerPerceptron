﻿
'Option Infer On

Public Class clsMLPHelper

    Public Shared Function GetVector(singleArray!(,), index%) As Single()

        Dim length% = singleArray.GetLength(1)
        Dim vect!(0 To length - 1)
        For k As Integer = 0 To length - 1
            vect(k) = singleArray(index, k)
        Next
        Return vect

    End Function

    Public Shared Function FillArray(singleArray2D!(,), singleArray1D!(), index%) As Single(,)
        Dim nbItems% = singleArray1D.GetLength(0)
        For j As Integer = 0 To nbItems - 1
            singleArray2D(index, j) = singleArray1D(j)
        Next
        Return singleArray2D
    End Function

    Public Shared Function FillArray(doubleArray2D#(,), singleArray1D!(), index%) As Double(,)
        Dim nbItems% = singleArray1D.GetLength(0)
        For j As Integer = 0 To nbItems - 1
            doubleArray2D(j, index) = singleArray1D(j)
        Next
        Return doubleArray2D
    End Function

    Public Shared Function FillArray2(doubleArray2D#(,), singleArray1D!(), index%) As Double(,)
        Dim nbItems% = singleArray1D.GetLength(0)
        For j As Integer = 0 To nbItems - 1
            doubleArray2D(index, j) = singleArray1D(j)
        Next
        Return doubleArray2D
    End Function

    Public Shared Function ConvertSingleToDouble(inputs!(,)) As Double(,)
        Dim length0% = inputs.GetLength(0)
        Dim length1% = inputs.GetLength(1)
        Dim arr#(0 To length0 - 1, 0 To length1 - 1)
        For i As Integer = 0 To length0 - 1
            For j As Integer = 0 To length1 - 1
                arr(i, j) = inputs(i, j)
            Next
        Next
        Return arr
    End Function

    Public Shared Function ConvertDoubleToSingle(inputs#()) As Single()
        Dim length0% = inputs.GetLength(0)
        Dim arr!(0 To length0 - 1)
        For i As Integer = 0 To length0 - 1
            arr(i) = CSng(inputs(i))
        Next
        Return arr
    End Function

    Public Shared Function Compare(val1#, val2#, dec%) As Boolean

        If Double.IsNaN(val1) AndAlso Double.IsNaN(val2) Then Return True
        Dim delta# = Math.Abs(Math.Round(val2 - val1, dec))
        If delta = 0 Then Return True
        Return False

    End Function

End Class