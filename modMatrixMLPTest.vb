
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
            nbIterations = 100000
            p.SetActivationFunction(ActivationFunctionForMatrix.TActivationFunction.HyperbolicTangent)

            ' Works fine
            'nbIterations = 100000
            'p.SetActivationFunction(ActivationFunctionForMatrix.TActivationFunction.ELU)

            ' Doesn't work
            'nbIterations = 1000000
            'p.SetActivationFunction(ActivationFunctionForMatrix.TActivationFunction.ReLU)

            'p.Init(inputNodes:=2, hiddenNodes:=2, outputNodes:=1, learningRate:=0.1!)
            Dim NeuronCount = New Integer() {2, 2, 1}
            p.InitStruct(NeuronCount, addBiasColumn:=True)
            p.Init(learningRate:=0.1)

            p.Randomize(-1, 2)
            Dim nbOutput% = 1
            Dim training As New ML_TrainingData(inputsLength:=2, targetsLength:=nbOutput)
            training.Create()
            Dim inputs!(,) = training.GetInputs
            Dim targets!(,) = training.GetOutputs

            Dim length% = inputs.GetLength(0)
            Dim nbInputs% = inputs.GetLength(1)

            For i As Integer = 0 To nbIterations - 1
                Dim r% = MultiLayerPerceptron.rng.Next(maxValue:=4) ' Stochastic learning

                Dim inp!(0 To nbInputs - 1)
                For k As Integer = 0 To nbInputs - 1
                    inp(k) = inputs(r, k)
                Next
                Dim target!(0 To nbOutput - 1)
                For k As Integer = 0 To nbOutput - 1
                    target(k) = targets(r, k)
                Next

                p.Train(inp, target)

                If i < 10 OrElse
                   (i + 1) Mod 10000 = 0 OrElse
                   ((i + 1) Mod 1000 = 0 AndAlso i < 10000) Then
                    p.ComputeAverageErrorFromLastError()
                    Dim sMsg$ = "Iteration n°" & i + 1 & "/" & nbIterations &
                        " : average error = " & p.averageError.ToString("0.00")
                    Console.WriteLine(sMsg)
                    Debug.WriteLine(sMsg)
                End If

            Next

            p.output = p.TestAllSamples(inputs, nbOutput)
            p.targetArray = targets
            p.ComputeAverageError()
            Debug.WriteLine("Result matrix: " & p.output.ToString())
            Debug.WriteLine("Average error = " & p.averageError.ToString("0.000000"))
            Console.WriteLine("Result matrix: " & p.output.ToString())
            Console.WriteLine("Average error = " & p.averageError.ToString("0.000000"))

        End Sub

    End Module

End Namespace