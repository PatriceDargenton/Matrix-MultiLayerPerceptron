
' From : https://github.com/nlabiris/perceptrons : C# -> VB .NET conversion

Imports System.Text ' StringBuilder
Imports System.Threading.Tasks ' Parallel.For (for previous Visual Studio)

''' <summary>
''' Contains matrix operations.
''' </summary>
Partial Class Matrix

    ''' <summary>
    ''' Rows
    ''' </summary>
    Private ReadOnly m_rows%

    ''' <summary>
    ''' Columns
    ''' </summary>
    Private ReadOnly m_cols%

    ''' <summary>
    ''' Array data
    ''' </summary>
    Private data!(,)

    ''' <summary>
    ''' Random number generator
    ''' </summary>
    Private Shared rng As New Random

    ''' <summary>
    ''' Rows
    ''' </summary>
    Public ReadOnly Property Rows%
        Get
            Return Me.m_rows
        End Get
    End Property

    ''' <summary>
    ''' Columns
    ''' </summary>
    Public ReadOnly Property Cols%
        Get
            Return Me.m_cols
        End Get
    End Property

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="rows">Rows</param>
    ''' <param name="cols">Columns</param>
    Public Sub New(rows%, cols%)

        MyBase.New()

        Me.m_rows = rows
        Me.m_cols = cols

        Me.data = New Single(rows - 1, cols - 1) {}

    End Sub

    ''' <summary>
    ''' Create a Matrix object from an array.
    ''' </summary>
    ''' <param name="inputs">Array of data</param>
    ''' <returns>Returns a Matrix object filled with the array's data.</returns>
    Public Shared Function FromArray(inputs!()) As Matrix

        Dim m As New Matrix(inputs.Length, 1)

        For i As Integer = 0 To inputs.Length - 1
            m.data(i, 0) = inputs(i)
        Next

        Return m

    End Function

    ''' <summary>
    ''' Convert the first vector of the Matrix to array.
    ''' </summary>
    ''' <returns>Returns an array.</returns>
    Public Function ToVectorArray() As Single()

        'Dim array!() = New Single(Me.data.Length - 1) {}
        Dim array!() = New Single(Me.data.GetLength(0) - 1) {}

        For i As Integer = 0 To array.Length - 1
            array(i) = Me.data(i, 0)
        Next

        Return array

    End Function

    ''' <summary>
    ''' Fill Matrix with random data.
    ''' </summary>
    Public Sub Randomize()

        Parallel.For(0, Me.Rows,
            Sub(i)
                Parallel.For(0, Me.Cols,
                    Sub(j)
                        Me.data(i, j) = rng.NextFloat(-1.0!, 2.0!)
                    End Sub)
            End Sub)

    End Sub

    ''' <summary>
    ''' Override <c>ToString()</c> method to pretty-print a Matrix object.
    ''' </summary>
    ''' <returns>Returns Matrix object as string.</returns>
    Public Overrides Function ToString$()

        Dim sb As New StringBuilder

        For i As Integer = 0 To Me.m_rows - 1
            For j As Integer = 0 To Me.m_cols - 1
                sb.Append(Me.data(i, j) & " ")
            Next
            sb.AppendLine()
        Next

        Dim s$ = sb.ToString
        Return s

    End Function

    ''' <summary>
    ''' Add a number to each element of the array.
    ''' </summary>
    ''' <param name="n">Scalar number</param>
    Public Overloads Sub Add(n%)

        For i As Integer = 0 To Me.m_rows - 1
            For j As Integer = 0 To Me.m_cols - 1
                Me.data(i, j) += n
            Next
        Next

    End Sub

    ''' <summary>
    ''' Add each element of the Matrices.
    ''' </summary>
    ''' <param name="m">Matrix object</param>
    Public Overloads Sub Add(m As Matrix)

        For i As Integer = 0 To Me.m_rows - 1
            For j As Integer = 0 To Me.m_cols - 1
                Me.data(i, j) += m.data(i, j)
            Next
        Next

    End Sub

    ''' <summary>
    ''' Subtract a number to each element of the array.
    ''' </summary>
    ''' <param name="n">Scalar number.</param>
    Public Overloads Sub Subtract(n%)

        For i As Integer = 0 To Me.m_rows - 1
            For j As Integer = 0 To Me.m_cols - 1
                Me.data(i, j) -= n
            Next
        Next

    End Sub

    ''' <summary>
    ''' Subtract each element of the Matrices.
    ''' </summary>
    ''' <param name="m">Matrix object</param>
    Public Overloads Sub Subtract(m As Matrix)

        For i As Integer = 0 To Me.m_rows - 1
            For j As Integer = 0 To Me.m_cols - 1
                Me.data(i, j) -= m.data(i, j)
            Next
        Next

    End Sub

    ''' <summary>
    ''' Subtract 2 Matrices and return a new object.
    ''' </summary>
    ''' <param name="a">Matrix object</param>
    ''' <param name="b">Matrix object</param>
    ''' <returns>Return a new Matrix object.</returns>
    Public Overloads Shared Function Subtract(a As Matrix, b As Matrix) As Matrix

        Dim c As New Matrix(a.Rows, a.Cols)

        For i As Integer = 0 To c.Rows - 1
            For j As Integer = 0 To c.Cols - 1
                c.data(i, j) = a.data(i, j) - b.data(i, j)
            Next
        Next

        Return c

    End Function

    ''' <summary>
    ''' Scalar product.
    ''' Multiply each element of the array with the given number.
    ''' </summary>
    ''' <param name="n">Number</param>
    Public Overloads Sub Multiply(n!)

        For i As Integer = 0 To Me.m_rows - 1
            For j As Integer = 0 To Me.m_cols - 1
                Me.data(i, j) *= n
            Next
        Next

    End Sub

    ''' <summary>
    ''' Hadamard product (element-wise multiplication).
    ''' Multiply each element of the array with each element of the given array.
    ''' </summary>
    ''' <param name="m">Matrix object</param>
    Public Overloads Sub Multiply(m As Matrix)

        For i As Integer = 0 To Me.m_rows - 1
            For j As Integer = 0 To Me.m_cols - 1
                Me.data(i, j) *= m.data(i, j)
            Next
        Next

    End Sub

    ''' <summary>
    ''' Matrix product.
    ''' </summary>
    ''' <param name="a">Matrix object</param>
    ''' <param name="b">Matrix object</param>
    ''' <returns>Returns a new Matrix object.</returns>
    Public Overloads Shared Function Multiply(a As Matrix, b As Matrix) As Matrix

        If a.Cols <> b.Rows Then
            Throw New Exception("Columns of A must match columns of B")
        End If

        Dim c As New Matrix(a.Rows, b.Cols)

        For i As Integer = 0 To c.Rows - 1
            For j As Integer = 0 To c.Cols - 1
                Dim sum! = 0
                For k As Integer = 0 To a.Cols - 1
                    sum += a.data(i, k) * b.data(k, j)
                Next
                c.data(i, j) = sum
            Next
        Next

        Return c

    End Function

    ''' <summary>
    ''' Average value.
    ''' </summary>
    ''' <returns>Returns the average value.</returns>
    Public Overloads Function Average!()

        Dim nbElements% = Me.m_rows * Me.m_cols
        Dim sum! = 0
        For i As Integer = 0 To Me.m_rows - 1
            For j As Integer = 0 To Me.m_cols - 1
                sum += Me.data(i, j)
            Next
        Next

        Dim rAverage! = 0
        If nbElements <= 1 Then
            rAverage = sum
        Else
            rAverage = sum / nbElements
        End If

        Return rAverage

    End Function

    ''' <summary>
    ''' Transpose a Matrix.
    ''' </summary>
    ''' <param name="m">Matrix object</param>
    ''' <returns>Returns a new Matrix object.</returns>
    Public Overloads Shared Function Transpose(m As Matrix) As Matrix

        Dim c As New Matrix(m.Cols, m.Rows)

        For i As Integer = 0 To m.Rows - 1
            For j As Integer = 0 To m.Cols - 1
                c.data(j, i) = m.data(i, j)
            Next
        Next

        Return c

    End Function

    ''' <summary>
    ''' Transpose a Matrix.
    ''' </summary>
    Public Overloads Sub Transpose()

        For i As Integer = 0 To Me.Rows - 1
            For j As Integer = 0 To Me.Cols - 1
                Me.data(j, i) = Me.data(i, j)
            Next
        Next

    End Sub

    ''' <summary>
    ''' Apply a function to every element of the array.
    ''' </summary>
    ''' <param name="lambdaFct">Delegate with encapsulates a method</param>
    Public Sub Map(lambdaFct As Func(Of Single, Single))

        For i As Integer = 0 To Me.Rows - 1
            For j As Integer = 0 To Me.Cols - 1
                Me.data(i, j) = lambdaFct.Invoke(Me.data(i, j))
            Next
        Next

    End Sub

    ''' <summary>
    ''' Apply a function to every element of the array.
    ''' </summary>
    ''' <param name="m">Matrix object</param>
    ''' <param name="lambdaFct">Delegate with encapsulates a method</param>
    ''' <returns>Returns a new Matrix object.</returns>
    Public Shared Function Map(m As Matrix, lambdaFct As Func(Of Single, Single)) As Matrix

        Dim c As New Matrix(m.Rows, m.Cols)

        For i As Integer = 0 To m.Rows - 1
            For j As Integer = 0 To m.Cols - 1
                c.data(i, j) = lambdaFct.Invoke(m.data(i, j))
            Next
        Next

        Return c

    End Function

End Class