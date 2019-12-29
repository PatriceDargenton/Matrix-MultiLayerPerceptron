
' From: https://github.com/nlabiris/perceptrons : C# -> VB .NET conversion

Option Infer On ' Lambda function

''' <summary>
''' Multi-Layer Perceptron
''' </summary>
Class MultiLayerPerceptron

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
    ''' Learning rate of the MLP
    ''' </summary>
    Private learningRate!

    ''' <summary>
    ''' Activate function of each neuron of the MLP
    ''' </summary>
    Private activFct As ActivationFunctionForMatrix.IActivationFunctionForMatrix

    ''' <summary>
    ''' Lambda function for activate function
    ''' </summary>
    Private lambdaFct As Func(Of Single, Single)

    ''' <summary>
    ''' Lambda function for activate function derivate
    ''' </summary>
    Private lambdaFctD As Func(Of Single, Single)

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New(inputNodes%, hiddenNodes%, outputNodes%, learningRate!)

        Me.weights_ih = New Matrix(hiddenNodes, inputNodes)
        Me.weights_ho = New Matrix(outputNodes, hiddenNodes)
        Me.bias_h = New Matrix(hiddenNodes, 1)
        Me.bias_o = New Matrix(outputNodes, 1)
        Me.learningRate = learningRate

        Dim lambdaFct = Function(x!) activFct.Activation(x, gain:=1, center:=0)
        Dim lambdaFctD = Function(x!) activFct.Derivative(x, gain:=1)
        SetLambdaActivationFunction(lambdaFct, lambdaFctD)

    End Sub

    ''' <summary>
    ''' Set specific activation function
    ''' </summary>
    Public Sub SetLambdaActivationFunction(
        lambdaFct As Func(Of Single, Single),
        lambdaFctD As Func(Of Single, Single))
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
        Me.bias_h.Randomize(minValue, maxValue)
        Me.bias_o.Randomize(minValue, maxValue)
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
        lambdaFct As Func(Of Single, Single)) As Single()

        ' Generating the Hidden Outputs
        Dim inputs = Matrix.FromArray(inputs_array)
        Dim hidden = Matrix.MultiplyAddAndMap(Me.weights_ih, inputs, Me.bias_h, lambdaFct)

        ' Generating the output's output!
        Dim output = Matrix.MultiplyAddAndMap(Me.weights_ho, hidden, Me.bias_o, lambdaFct)
        Me.output = output

        Dim aSng = output.ToVectorArray()
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
        lambdaFct As Func(Of Single, Single),
        lambdaFctD As Func(Of Single, Single),
        backwardLearningRate!, forewardLearningRate!)

        Dim inputs = Matrix.FromArray(inputs_array)

        ' Generating the Hidden Outputs
        Dim hidden = Matrix.MultiplyAddAndMap(Me.weights_ih, inputs, Me.bias_h, lambdaFct)

        ' Generating the output's output!
        Dim outputs = Matrix.MultiplyAddAndMap(Me.weights_ho, hidden, Me.bias_o, lambdaFct)
        Me.output = outputs

        ' Calculate the error: ERROR = TARGETS - OUTPUTS
        Dim output_errors = ComputeError(targets_array)

        ' Calculate gradient
        ' Calculate hidden -> output delta weights
        ' Adjust the weights by deltas
        ' Calculate the hidden layer errors
        ComputeGradient(outputs, output_errors, hidden, lambdaFctD, backwardLearningRate,
            Me.weights_ho, Me.bias_o)

        ' Calculate the hidden layer errors
        Dim hidden_errors = Matrix.TransposeAndMultiply1(Me.weights_ho, output_errors)

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
        lambdaFctD As Func(Of Single, Single), learningRate!,
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
        bias.Add(gradient)

    End Sub

    ''' <summary>
    ''' Compute error from output and target matrices
    ''' </summary>
    Public Function ComputeError(targets_array!()) As Matrix

        ' Calculate the error: ERROR = TARGETS - OUTPUTS
        Dim output_errors = Matrix.SubtractFromArray(targets_array, Me.output)
        Me.averageError = Math.Abs(output_errors.Average)
        Return output_errors

    End Function

    ''' <summary>
    ''' This is our test function
    ''' </summary>
    Public Function Test(inputs!()) As Single()
        Return Me.FeedForward(inputs)
    End Function

End Class