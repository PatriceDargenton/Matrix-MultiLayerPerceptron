
' Patrice Dargenton
' Matrix-MultiLayerPerceptron
' From : https://github.com/nlabiris/perceptrons : C# -> VB .NET conversion

Module modMatrixMLPTest

    Sub Main()
        Console.WriteLine("Matrix-MultiLayerPerceptron with the classical XOR test.")
        Console.WriteLine("Matrix-MLP may not converge each time, run again if not.")
        TestMLPMatrice()
        Console.WriteLine("Press a key to quit.")
        Console.ReadKey()
    End Sub

    Public Sub TestMLPMatrice()

        Dim p As New MultiLayerPerceptron(
            inputNodes:=2, hiddenNodes:=2, outputNodes:=1, learningRate:=0.1!)
        Dim training As New ML_TrainingData(inputsLength:=2, targetsLength:=1)
        training.Create()
        Dim inputs!(,) = training.GetInputs
        Dim outputs!(,) = training.GetOutputs

        For i As Integer = 0 To 100000 - 1
            Dim r As Integer = MultiLayerPerceptron.rng.Next(maxValue:=4)
            Dim inp!() = New Single() {inputs(r, 0), inputs(r, 1)}
            Dim outp!() = New Single() {outputs(r, 0)}
            p.Train(inp, outp)
        Next

        Dim length% = training.data.GetLength(0)
        For i As Integer = 0 To length - 1
            Dim r1! = inputs(i, 0)
            Dim r2! = inputs(i, 1)
            Dim inp!() = New Single() {r1, r2}
            Dim ar!() = p.Test(inp)
            Dim r! = ar(0)
            Console.WriteLine(r1 & ", " & r2 & " -> " & r)
        Next

    End Sub

End Module
