
' Patrice Dargenton
' Matrix-MultiLayerPerceptron
' From: https://github.com/nlabiris/perceptrons : C# -> VB .NET conversion

Option Infer On

Namespace MatrixMLP

    Module modMatrixMLPTest

        Sub Main()
            Console.WriteLine("Matrix-MultiLayerPerceptron with the classical XOR test.")
            Console.WriteLine("Matrix-MLP may not converge each time, run again if not.")
            MatrixMLPTest()
            Console.WriteLine("Press a key to quit.")
            Console.ReadKey()
        End Sub

        Public Sub MatrixMLPTest()

            Dim p As New MultiLayerPerceptron()

            Dim nbIterations%

            ' Sometimes works
            'nbIterations = 100000
            'p.SetActivationFunction(ActivationFunctionForMatrix.TActivationFunction.Sigmoid)

            ' Works fine
            'nbIterations = 1000000
            'p.SetActivationFunction(ActivationFunctionForMatrix.TActivationFunction.HyperbolicTangent)

            ' Works fine
            nbIterations = 100000
            p.SetActivationFunction(ActivationFunctionForMatrix.TActivationFunction.ELU)

            ' Doesn't work
            'nbIterations = 1000000
            'p.SetActivationFunction(ActivationFunctionForMatrix.TActivationFunction.ReLU)

            'p.Init(inputNodes:=2, hiddenNodes:=2, outputNodes:=1, learningRate:=0.1!)
            Dim NeuronCount = New Integer() {2, 2, 1}
            p.InitStruct(NeuronCount, addBiasColumn:=False)
            p.Init(learningRate:=0.1)

            p.Randomize(-1, 2)
            Dim nbOutput% = 1
            Dim training As New ML_TrainingData(inputsLength:=2, targetsLength:=nbOutput)
            training.Create()
            Dim inputs!(,) = training.GetInputs
            Dim outputs!(,) = training.GetOutputs

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
                Dim averageError = p.ComputeAverageError(outp)

                Dim sMsg$ = r1 & ", " & r2 & " -> " & r.ToString("0.000000") &
                    " : error = " & averageError.ToString("0.000000")
                Console.WriteLine(sMsg)
                Debug.WriteLine(sMsg)
            Next

            Dim m As Matrix = p.TestAllSamples(inputs, nbOutput)
            Debug.WriteLine("Result matrix: " & m.ToString())

        End Sub

    End Module

End Namespace