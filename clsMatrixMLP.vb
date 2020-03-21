
' From: https://github.com/nlabiris/perceptrons : C# -> VB .NET conversion

Option Infer On ' Lambda function

Namespace MatrixMLP

    ''' <summary>
    ''' Multi-Layer Perceptron
    ''' </summary>
    Class MultiLayerPerceptron

        Public m_useBias As Boolean

        Public targetArray As Single(,)

        ''' <summary>
        ''' Random generator
        ''' </summary>
        Public Shared rng As New Random

        ''' <summary>
        ''' hidden x input weights matrix
        ''' </summary>
        Public weights_ih As Matrix

        ''' <summary>
        ''' ouput x hidden weights matrix
        ''' </summary>
        Public weights_ho As Matrix

        ''' <summary>
        ''' Hidden bias matrix
        ''' </summary>
        Public bias_h As Matrix

        ''' <summary>
        ''' Output bias matrix
        ''' </summary>
        Public bias_o As Matrix

        ''' <summary>
        ''' Output matrix (returned to compute average error, and discrete error)
        ''' </summary>
        Public output As Matrix

        ''' <summary>
        ''' Average error of the output matrix
        ''' </summary>
        Public averageError!

        ''' <summary>
        ''' Last error of the output matrix
        ''' </summary>
        Public lastError As Matrix

        ''' <summary>
        ''' Learning rate of the MLP
        ''' </summary>
        Private learningRate!

        ''' <summary>
        ''' Activate function of each neuron of the MLP
        ''' </summary>
        Private activFct As ActivationFunctionForMatrix.IActivationFunctionForMatrix

        ''' <summary>
        ''' Lambda function for the activation function
        ''' </summary>
        Private lambdaFct As Func(Of Double, Double)

        ''' <summary>
        ''' Lambda function for the derivative of the activation function
        ''' </summary>
        Private lambdaFctD As Func(Of Double, Double)

        ''' <summary>
        ''' Constructor
        ''' </summary>
        Public Sub New()

        End Sub

        Public Sub InitStruct(aiNeuronCount%(), addBiasColumn As Boolean)

            Dim inputNodes% = aiNeuronCount(0)
            Dim hiddenNodes% = aiNeuronCount(1)
            Dim layerCount% = aiNeuronCount.Length
            Dim outputNodes% = aiNeuronCount(layerCount - 1)
            Me.weights_ih = New Matrix(hiddenNodes, inputNodes)
            Me.weights_ho = New Matrix(outputNodes, hiddenNodes)

            Me.m_useBias = addBiasColumn
            If Me.m_useBias Then
                Me.bias_h = New Matrix(hiddenNodes, 1)
                Me.bias_o = New Matrix(outputNodes, 1)
            End If

        End Sub

        Public Sub Init(learningRate!)

            Me.learningRate = learningRate

            Dim lambdaFct = Function(x#) activFct.Activation(x, gain:=1, center:=0)
            Dim lambdaFctD = Function(x#) activFct.Derivative(x, gain:=1)
            SetLambdaActivationFunction(lambdaFct, lambdaFctD)

        End Sub

        ''' <summary>
        ''' Set specific activation function
        ''' </summary>
        Public Sub SetLambdaActivationFunction(
            lambdaFct As Func(Of Double, Double),
            lambdaFctD As Func(Of Double, Double))
            Me.lambdaFct = lambdaFct
            Me.lambdaFctD = lambdaFctD
        End Sub

        ''' <summary>
        ''' Set registered activation function
        ''' </summary>
        Public Sub SetActivationFunction(fctAct As ActivationFunctionForMatrix.TActivationFunction)

            Select Case fctAct
                Case ActivationFunctionForMatrix.TActivationFunction.Sigmoid
                    Me.activFct = New ActivationFunctionForMatrix.SigmoidFunction
                Case ActivationFunctionForMatrix.TActivationFunction.HyperbolicTangent
                    Me.activFct = New ActivationFunctionForMatrix.HyperbolicTangentFunction
                Case ActivationFunctionForMatrix.TActivationFunction.ELU
                    Me.activFct = New ActivationFunctionForMatrix.ELUFunction
                Case ActivationFunctionForMatrix.TActivationFunction.ReLU
                    Me.activFct = New ActivationFunctionForMatrix.ReluFunction
                Case Else
                    Me.activFct = Nothing
            End Select

        End Sub

        ''' <summary>
        ''' Randomize weights
        ''' </summary>
        Public Sub Randomize(minValue!, maxValue!)

            Me.weights_ih.Randomize(minValue, maxValue)
            Me.weights_ho.Randomize(minValue, maxValue)

            If Me.m_useBias Then
                Me.bias_h.Randomize(minValue, maxValue)
                Me.bias_o.Randomize(minValue, maxValue)
            End If

        End Sub

        ''' <summary>
        ''' Print weights for functionnal test
        ''' </summary>
        Public Sub PrintWeights()
            Debug.WriteLine("Me.weights_ih=" & Me.weights_ih.ToString())
            Debug.WriteLine("Me.weights_ho=" & Me.weights_ho.ToString())
            If Me.m_useBias Then
                Debug.WriteLine("Me.bias_h=" & Me.bias_h.ToString())
                Debug.WriteLine("Me.bias_o=" & Me.bias_o.ToString())
            End If
        End Sub

        ''' <summary>
        ''' Propagate the input signal into the MLP
        ''' </summary>
        Public Function FeedForward(inputs_array!()) As Single()

            Return FeedForward_internal(inputs_array, Me.lambdaFct)

        End Function

        ''' <summary>
        ''' Propagate the input signal into the MLP using actual activation function
        ''' </summary>
        Public Function FeedForward_internal(inputs_array!(),
            lambdaFct As Func(Of Double, Double)) As Single()

            ' Generating the Hidden Outputs
            Dim inputs = Matrix.FromArraySingle(inputs_array)
            Dim hidden As Matrix
            If Me.m_useBias Then
                hidden = Matrix.MultiplyAddAndMap(Me.weights_ih, inputs, Me.bias_h, lambdaFct)
            Else
                hidden = Matrix.MultiplyAndMap(Me.weights_ih, inputs, lambdaFct)
            End If

            ' Generating the output's output!
            Dim output As Matrix
            If Me.m_useBias Then
                output = Matrix.MultiplyAddAndMap(Me.weights_ho, hidden, Me.bias_o, lambdaFct)
            Else
                output = Matrix.MultiplyAndMap(Me.weights_ho, hidden, lambdaFct)
            End If
            Me.output = output

            Dim aSng = output.ToVectorArraySingle()
            Return aSng

        End Function

        ''' <summary>
        ''' Train MLP with one sample
        ''' </summary>
        Public Sub Train(inputs_array!(), targets_array!())

            Train_internal(inputs_array, targets_array, Me.lambdaFct, Me.lambdaFctD,
                backwardLearningRate:=Me.learningRate, forewardLearningRate:=Me.learningRate)

        End Sub

        ''' <summary>
        ''' Train MLP with one sample using actual activation function
        ''' </summary>
        Private Sub Train_internal(inputs_array!(), targets_array!(),
            lambdaFct As Func(Of Double, Double),
            lambdaFctD As Func(Of Double, Double),
            backwardLearningRate!, forewardLearningRate!)

            Dim inputs = Matrix.FromArraySingle(inputs_array)

            ' Generating the Hidden Outputs
            Dim hidden As Matrix
            If Me.m_useBias Then
                hidden = Matrix.MultiplyAddAndMap(Me.weights_ih, inputs, Me.bias_h, lambdaFct)
            Else
                hidden = Matrix.MultiplyAndMap(Me.weights_ih, inputs, lambdaFct)
            End If

            ' Generating the output's output!
            Dim outputs As Matrix
            If Me.m_useBias Then
                outputs = Matrix.MultiplyAddAndMap(Me.weights_ho, hidden, Me.bias_o, lambdaFct)
            Else
                outputs = Matrix.MultiplyAndMap(Me.weights_ho, hidden, lambdaFct)
            End If
            Me.output = outputs

            ' Calculate the error: ERROR = TARGETS - OUTPUTS
            ComputeErrorOneSample(targets_array)

            ' Calculate gradient
            ' Calculate hidden -> output delta weights
            ' Adjust the weights by deltas
            ' Calculate the hidden layer errors
            ComputeGradient(outputs, Me.lastError, hidden, lambdaFctD, backwardLearningRate,
                Me.weights_ho, Me.bias_o)

            ' Calculate the hidden layer errors
            Dim hidden_errors = Matrix.TransposeAndMultiply1(Me.weights_ho, Me.lastError)

            ' Calculate hidden gradient
            ' Calculate input -> hidden delta weights
            ' Adjust the bias by its deltas (which is just the gradients)
            ComputeGradient(hidden, hidden_errors, inputs, lambdaFctD, forewardLearningRate,
                Me.weights_ih, Me.bias_h)

        End Sub

        ''' <summary>
        ''' Compute gradient and return weight and bias matrices
        ''' </summary>
        Public Sub ComputeGradient(final As Matrix, error_ As Matrix, original As Matrix,
            lambdaFctD As Func(Of Double, Double), learningRate!,
            ByRef weight As Matrix, ByRef bias As Matrix)

            ' Calculate gradient
            Dim gradient = Matrix.Map(final, lambdaFctD)
            gradient.Multiply(error_)
            gradient.Multiply(learningRate)

            ' Calculate original -> final delta weights
            Dim weight_deltas = Matrix.TransposeAndMultiply2(original, gradient)

            ' Adjust the weights by deltas
            weight.Add(weight_deltas)

            ' Adjust the bias by its deltas (which is just the gradients)
            If Me.m_useBias Then bias.Add(gradient)

        End Sub

        ''' <summary>
        ''' Compute error from output and target matrices
        ''' </summary>
        Public Sub ComputeErrorOneSample(targets_array!())

            ' Calculate the error: ERROR = TARGETS - OUTPUTS
            Me.lastError = Matrix.SubtractFromArraySingle(targets_array, Me.output)

        End Sub

        Public Sub ComputeError()
            ' Calculate the error: ERROR = TARGETS - OUTPUTS
            Dim m As Matrix = Me.targetArray
            Me.lastError = m - Me.output
        End Sub

        Public Sub ComputeAverageErrorFromLastError()

            ' Compute first abs then average:
            'Me.averageError = CSng(Matrix.Abs(Me.lastError).Average)
            Me.averageError = CSng(Me.lastError.Abs.Average)

        End Sub

        Public Function ComputeAverageError!()

            Me.ComputeError()
            Me.ComputeAverageErrorFromLastError()
            Return Me.averageError

        End Function

        Public Sub TrainStochastic(inputs!(,), targets!(,), nbIterations%)

            Dim nbLines% = inputs.GetLength(0)
            Dim nbInputs% = inputs.GetLength(1)
            Dim nbTargets% = targets.GetLength(1)
            For i As Integer = 0 To nbIterations - 1

                ' Stochastic learning
                Dim r% = MultiLayerPerceptron.rng.Next(maxValue:=nbLines)

                Dim inp!(0 To nbInputs - 1)
                For k As Integer = 0 To nbInputs - 1
                    inp(k) = inputs(r, k)
                Next
                Dim targ!(0 To nbTargets - 1)
                For k As Integer = 0 To nbTargets - 1
                    targ(k) = targets(r, k)
                Next

                Train(inp, targ)
            Next

        End Sub

        Public Sub TrainSemiStochastic(inputs!(,), targets!(,), nbIterations%)

            Dim nbLines% = inputs.GetLength(0)
            Dim nbInputs% = inputs.GetLength(1)
            Dim nbTargets% = targets.GetLength(1)
            For i As Integer = 0 To nbIterations - 1

                ' Semi-stochastic learning
                Dim r% = MultiLayerPerceptron.rng.Next(maxValue:=nbLines)

                Dim inp!(0 To nbInputs - 1)
                For k As Integer = 0 To nbInputs - 1
                    inp(k) = inputs(r, k)
                Next
                Dim targ!(0 To nbTargets - 1)
                For k As Integer = 0 To nbTargets - 1
                    targ(k) = targets(r, k)
                Next
                Train(inp, targ)
            Next

        End Sub

        Public Sub TrainSystematic(inputs!(,), targets!(,), nbIterations%)

            Dim nbTargets% = targets.GetLength(1)
            For i As Integer = 0 To nbIterations - 1
                TrainAllSamples(inputs, targets)
            Next
            TestAllSamples(inputs, nbTargets)

        End Sub

        Public Sub TrainAllSamples(inputs!(,), targets!(,))

            Dim nbLines% = inputs.GetLength(0)
            Dim nbInputs% = inputs.GetLength(1)
            Dim nbTargets% = targets.GetLength(1)

            For j As Integer = 0 To nbLines - 1 ' Systematic learning

                Dim inp!(0 To nbInputs - 1)
                For k As Integer = 0 To nbInputs - 1
                    inp(k) = inputs(j, k)
                Next
                Dim targ!(0 To nbTargets - 1)
                For k As Integer = 0 To nbTargets - 1
                    targ(k) = targets(j, k)
                Next

                Train(inp, targ)
            Next

        End Sub

        ''' <summary>
        ''' Test one sample and return the sample output
        ''' </summary>
        Public Function TestOneSample(inputs!()) As Single()
            Return Me.FeedForward(inputs)
        End Function

        ''' <summary>
        ''' Test all samples and return output matrix for all samples
        ''' </summary>
        Public Function TestAllSamples(inputs!(,), nbOutputs%) As Single(,)

            Dim length% = inputs.GetLength(0)
            Dim nbInputs% = inputs.GetLength(1)
            Dim outputs!(0 To length - 1, 0 To nbOutputs - 1)
            For i As Integer = 0 To length - 1
                Dim inp!(0 To nbInputs - 1)
                For k As Integer = 0 To nbInputs - 1
                    inp(k) = inputs(i, k)
                Next
                Dim output!() = TestOneSample(inp)
                For j As Integer = 0 To output.GetLength(0) - 1
                    outputs(i, j) = output(j)
                Next
            Next
            Return outputs

        End Function

    End Class

End Namespace