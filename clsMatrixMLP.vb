﻿
' From: https://github.com/nlabiris/perceptrons : C# -> VB .NET conversion

Imports Perceptron.Utility ' Matrix
Imports Perceptron.MLP.ActivationFunction

Namespace MatrixMLP

    ' Note: Me.weightAdjustment is not used in this implementation

    ''' <summary>
    ''' Multi-Layer Perceptron
    ''' </summary>
    Class MultiLayerPerceptron : Inherits clsMLPGeneric

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

        Private input, hidden As Matrix

        ' ''' <summary>
        ' ''' Output matrix (returned to compute average error, and discrete error)
        ' ''' </summary>
        'Public output As Matrix

        Public Overrides Sub InitializeStruct(neuronCount%(), addBiasColumn As Boolean)

            Me.nbInputNeurons = neuronCount(0)
            Me.nbHiddenNeurons = neuronCount(1)
            Me.layerCount = neuronCount.Length
            Me.neuronCount = neuronCount
            Me.weightAdjustment = 0 ' Not used

            If Me.layerCount <> 3 Then
                ' ToDo: declare and use Me.weights_ih2 to compute 2 hidden layers
                MsgBox("This Matrix implementation can only compute one hidden layer!",
                    MsgBoxStyle.Exclamation)
                Me.layerCount = 3
            End If

            Me.nbOutputNeurons = neuronCount(Me.layerCount - 1)

            'Me.weights_ih = New Matrix(hiddenNodes, inputNodes)
            Dim dbleArray_ih#(Me.nbHiddenNeurons - 1, Me.nbInputNeurons - 1)
            Me.weights_ih = dbleArray_ih

            'Me.weights_ho = New Matrix(outputNodes, hiddenNodes)
            Dim dbleArray_ho#(Me.nbOutputNeurons - 1, Me.nbHiddenNeurons - 1)
            Me.weights_ho = dbleArray_ho

            Me.useBias = addBiasColumn
            If Me.useBias Then
                'Me.bias_h = New Matrix(hiddenNodes, 1)
                'Me.bias_o = New Matrix(outputNodes, 1)
                Dim dbleArray_bh#(Me.nbHiddenNeurons - 1, 0)
                Dim dbleArray_bo#(Me.nbOutputNeurons - 1, 0)
                Me.bias_h = dbleArray_bh
                Me.bias_o = dbleArray_bo
            End If

        End Sub

        ''' <summary>
        ''' Randomize weights
        ''' </summary>
        Public Overrides Sub Randomize(Optional minValue! = 0, Optional maxValue! = 1)

            Me.rnd = New Random()

            Me.weights_ih.Randomize(Me.rnd, minValue, maxValue)
            Me.weights_ho.Randomize(Me.rnd, minValue, maxValue)

            If Me.useBias Then
                Me.bias_h.Randomize(Me.rnd, minValue, maxValue)
                Me.bias_o.Randomize(Me.rnd, minValue, maxValue)
            End If

        End Sub

        Public Overrides Sub InitializeWeights(layer%, weights#(,))
            Throw New NotImplementedException(
                "InitializeWeights() is not implemented for clsMatrixMLP!")
        End Sub

        ''' <summary>
        ''' Test one sample
        ''' </summary>
        Public Overrides Sub TestOneSample(input!())
            ForwardPropogateSignal(input)
            Me.lastOutputArray1DSingle = Me.output.ToArraySingle()
        End Sub

        ''' <summary>
        ''' Propagate the input signal into the MLP
        ''' </summary>
        Private Sub ForwardPropogateSignal(inputsArray!())

            ' Generating the Hidden Outputs
            Me.input = Matrix.FromArraySingle(inputsArray)
            If Me.useBias Then
                Me.hidden = Matrix.MultiplyAddAndMap(Me.weights_ih, Me.input, Me.bias_h, Me.lambdaFnc)
            Else
                Me.hidden = Matrix.MultiplyAndMap(Me.weights_ih, Me.input, Me.lambdaFnc)
            End If

            ' Generating the output's output!
            If Me.useBias Then
                Me.output = Matrix.MultiplyAddAndMap(Me.weights_ho, Me.hidden, Me.bias_o, Me.lambdaFnc)
            Else
                Me.output = Matrix.MultiplyAndMap(Me.weights_ho, Me.hidden, Me.lambdaFnc)
            End If

        End Sub

        Private Sub BackwardPropagateError()

            ' Calculate gradient
            ' Calculate hidden -> output delta weights
            ' Adjust the weights by deltas
            ' Calculate the hidden layer errors
            ' Me.weightAdjustment is not used in this implementation
            BackwardPropagateErrorComputeGradientAndAdjustWeights(
                Me.output, Me.lastError, Me.hidden, Me.learningRate,
                Me.weights_ho, Me.bias_o)

            ' Calculate the hidden layer errors
            Dim hidden_errors = Matrix.TransposeAndMultiply1(Me.weights_ho, Me.lastError)

            ' Calculate hidden gradient
            ' Calculate input -> hidden delta weights
            ' Adjust the bias by its deltas (which is just the gradients)
            BackwardPropagateErrorComputeGradientAndAdjustWeights(
                Me.hidden, hidden_errors, Me.input, Me.learningRate,
                Me.weights_ih, Me.bias_h)

        End Sub

        ''' <summary>
        ''' Train MLP with one sample
        ''' </summary>
        Public Overrides Sub TrainOneSample(inputsArray!(), targetsArray!())

            ForwardPropogateSignal(inputsArray)

            ' Calculate the error: ERROR = TARGETS - OUTPUTS
            ComputeErrorOneSample(targetsArray)
            ComputeAverageErrorFromLastError()

            BackwardPropagateError()

        End Sub

        ''' <summary>
        ''' Gradient descend: Compute gradient and adjust weights
        ''' </summary>
        Public Sub BackwardPropagateErrorComputeGradientAndAdjustWeights(
            final As Matrix, error_ As Matrix, original As Matrix, adjustment!,
            ByRef weight As Matrix, ByRef bias As Matrix)

            ' Calculate gradient
            Dim gradient = Matrix.Map(final, lambdaFncDFOF)
            gradient *= error_
            gradient *= adjustment

            ' Calculate original -> final delta weights
            Dim weight_deltas = Matrix.TransposeAndMultiply2(original, gradient)

            ' Adjust the weights by deltas
            weight += weight_deltas

            ' Adjust the bias by its deltas (which is just the gradients)
            If Me.useBias Then bias += gradient

        End Sub

        ''' <summary>
        ''' Compute error from output and target matrices
        ''' </summary>
        Public Sub ComputeErrorOneSample(target!())

            ' Calculate the error: ERROR = TARGETS - OUTPUTS
            Me.lastError = Matrix.SubtractFromArraySingle(target, Me.output)

        End Sub

        Public Overrides Sub TrainSystematic(inputs!(,), targets!(,),
            Optional learningMode As enumLearningMode = enumLearningMode.Defaut)

            MyBase.TrainSystematic(inputs, targets, learningMode)

        End Sub

        ''' <summary>
        ''' Print weights for functionnal test
        ''' </summary>
        Public Overrides Sub PrintWeights()

            Me.PrintParameters()

            'Dim inputNodes = Me.weights_ih.r
            'Dim hiddenNodes = Me.weights_ih.c
            Dim inputNodes = Me.nbInputNeurons
            Dim hiddenNodes = Me.nbHiddenNeurons
            Dim outputNodes = Me.weights_ho.r
            For i = 0 To Me.layerCount - 1
                Dim iNeuronCount = inputNodes
                If i > 0 Then iNeuronCount = hiddenNodes
                If i >= Me.layerCount - 1 Then iNeuronCount = outputNodes
                ShowMessage("Neuron count(" & i & ")=" & iNeuronCount)
            Next

            ShowMessage("")
            ShowMessage("Me.weights_ih=" & Me.weights_ih.ToString())
            ShowMessage("Me.weights_ho=" & Me.weights_ho.ToString())

            If Me.useBias Then
                ShowMessage("Me.bias_h=" & Me.bias_h.ToString())
                ShowMessage("Me.bias_o=" & Me.bias_o.ToString())
            End If

        End Sub

    End Class

End Namespace