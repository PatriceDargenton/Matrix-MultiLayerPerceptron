
' Patrice Dargenton
' Matrix-MultiLayerPerceptron
' From: https://github.com/nlabiris/perceptrons : C# -> VB .NET conversion

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
        'p.SetActivationFunction(ActivationFunctionForMatrix.TActivationFunction.Sigmoid)
        'p.SetActivationFunction(ActivationFunctionForMatrix.TActivationFunction.HyperbolicTangent)
        p.SetActivationFunction(ActivationFunctionForMatrix.TActivationFunction.ELU)
        p.Randomize(-1, 2)
        Dim training As New ML_TrainingData(inputsLength:=2, targetsLength:=1)
        training.Create()
        Dim inputs!(,) = training.GetInputs
        Dim outputs!(,) = training.GetOutputs

        Const nbIterations% = 100000
        For i As Integer = 0 To nbIterations - 1
            Dim r% = MultiLayerPerceptron.rng.Next(maxValue:=4) ' Stochastic learning
            Dim inp!() = New Single() {inputs(r, 0), inputs(r, 1)}
            Dim outp!() = New Single() {outputs(r, 0)}
            p.Train(inp, outp)

            If i < 10 OrElse
               (i + 1) Mod 10000 = 0 OrElse
               ((i + 1) Mod 1000 = 0 AndAlso i < 10000) Then
                Dim sMsg$ = "Iteration n°" & i + 1 & "/" & nbIterations &
                    " : average error = " & p.averageError.ToString("0.00")
                Console.WriteLine(sMsg)
                Debug.WriteLine(sMsg)
            End If

        Next

        Dim length% = training.data.GetLength(0)
        For i As Integer = 0 To length - 1
            Dim r1! = inputs(i, 0)
            Dim r2! = inputs(i, 1)
            Dim inp!() = New Single() {r1, r2}
            Dim ar!() = p.Test(inp)
            Dim r! = ar(0)

            Dim outp!() = New Single() {outputs(i, 0)}
            p.ComputeError(outp)

            Dim sMsg$ = r1 & ", " & r2 & " -> " & r.ToString("0.000000") &
                " : error = " & p.averageError.ToString("0.000000")
            Console.WriteLine(sMsg)
            Debug.WriteLine(sMsg)
        Next

    End Sub

End Module
