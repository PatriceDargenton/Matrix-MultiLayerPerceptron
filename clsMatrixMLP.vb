
' From : https://github.com/nlabiris/perceptrons : C# -> VB .NET conversion

Option Infer On ' Fct Lambda

Class MultiLayerPerceptron

    Public Shared rng As Random = New Random

    ''' <summary>
    ''' hidden x input weights matrix
    ''' </summary>
    Public weights_ih As Matrix

    ''' <summary>
    ''' ouput x hidden weights matrix
    ''' </summary>
    Public weights_ho As Matrix

    Public bias_h As Matrix

    Public bias_o As Matrix

    Public output As Matrix ' To compute error

    Public averageError!

    Private learningRate!

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="inputNodes"></param>
    ''' <param name="hiddenNodes"></param>
    ''' <param name="outputNodes"></param>
    Public Sub New(inputNodes%, hiddenNodes%, outputNodes%, learningRate!)

        MyBase.New()

        Me.weights_ih = New Matrix(hiddenNodes, inputNodes)
        Me.weights_ho = New Matrix(outputNodes, hiddenNodes)
        Me.bias_h = New Matrix(hiddenNodes, 1)
        Me.bias_o = New Matrix(outputNodes, 1)
        Me.learningRate = learningRate

    End Sub

    ''' <summary>
    ''' Randomize weights.
    ''' </summary>
    Public Sub Randomize()
        Me.weights_ih.Randomize()
        Me.weights_ho.Randomize()
        Me.bias_h.Randomize()
        Me.bias_o.Randomize()
    End Sub

    ''' <summary>
    ''' Sigmoid activation function.
    ''' </summary>
    ''' <param name="x"></param>
    ''' <returns></returns>
    Private Function sigmoid!(x!)
        Dim y# = 1 / (1 + Math.Exp(-x))
        Dim ySng! = CSng(y)
        'Dim ySng! = CType(y, Single)
        Return ySng
    End Function

    ''' <summary>
    ''' The derivative of sigmoid.
    ''' </summary>
    ''' <param name="y"></param>
    ''' <returns></returns>
    Private Function dsigmoid!(y!)
        Return y * (1 - y)
    End Function

    Public Function FeedForward(inputs_array!()) As Single()

        ' Generating the Hidden Outputs
        Dim inputs = Matrix.FromArray(inputs_array)
        Dim hidden = Matrix.Multiply(Me.weights_ih, inputs)
        hidden.Add(Me.bias_h)

        Dim fctLambdaSigmoid = Function(x!) sigmoid(x)

        ' activation function!
        hidden.Map(fctLambdaSigmoid)

        ' Generating the output's output!
        Dim output = Matrix.Multiply(Me.weights_ho, hidden)
        output.Add(Me.bias_o)
        output.Map(fctLambdaSigmoid)
        Me.output = output

        Dim aSng = output.ToVectorArray
        Return aSng

    End Function

    Public Sub Train(inputs_array!(), targets_array!())

        ' Generating the Hidden Outputs
        Dim inputs = Matrix.FromArray(inputs_array)
        Dim hidden = Matrix.Multiply(Me.weights_ih, inputs)
        hidden.Add(Me.bias_h)

        Dim lambdaFctSigmoid = Function(x!) sigmoid(x)

        ' activation function!
        hidden.Map(lambdaFctSigmoid)

        ' Generating the output's output!
        Dim outputs As Matrix = Matrix.Multiply(Me.weights_ho, hidden)
        outputs.Add(Me.bias_o)
        outputs.Map(lambdaFctSigmoid)
        Me.output = outputs

        ' Convert array to matrix object
        Dim targets As Matrix = Matrix.FromArray(targets_array)
        ' Calculate the error
        ' ERROR = TARGETS - OUTPUTS
        Dim output_errors As Matrix = Matrix.Subtract(targets, outputs)
        Me.averageError = Math.Abs(output_errors.Average)
        ' let gradient = outputs * (1 - outputs);

        Dim lambdaFctDSigmoid = Function(x!) dsigmoid(x)

        ' Calculate gradient
        Dim gradients As Matrix = Matrix.Map(outputs, lambdaFctDSigmoid)

        gradients.Multiply(output_errors)
        gradients.Multiply(Me.learningRate)
        ' Calculate hidden -> output delta weights
        Dim hidden_t As Matrix = Matrix.Transpose(hidden)
        Dim weight_ho_deltas As Matrix = Matrix.Multiply(gradients, hidden_t)
        ' Adjust the weights by deltas
        Me.weights_ho.Add(weight_ho_deltas)
        ' Adjust the bias by its deltas (which is just the gradients)
        Me.bias_o.Add(gradients)
        ' Calculate the hidden layer errors
        Dim weights_ho_t As Matrix = Matrix.Transpose(Me.weights_ho)
        Dim hidden_errors As Matrix = Matrix.Multiply(weights_ho_t, output_errors)

        ' Calculate hidden gradient
        Dim hidden_gradient As Matrix = Matrix.Map(hidden, lambdaFctDSigmoid)

        hidden_gradient.Multiply(hidden_errors)
        hidden_gradient.Multiply(Me.learningRate)
        ' Calculate input -> hidden delta weights
        Dim inputs_t As Matrix = Matrix.Transpose(inputs)
        Dim weight_ih_deltas As Matrix = Matrix.Multiply(hidden_gradient, inputs_t)
        Me.weights_ih.Add(weight_ih_deltas)
        ' Adjust the bias by its deltas (which is just the gradients)
        Me.bias_h.Add(hidden_gradient)

    End Sub

    Public Sub ComputeError(targets_array!())

        ' Convert array to matrix object
        Dim targets As Matrix = Matrix.FromArray(targets_array)
        ' Calculate the error
        ' ERROR = TARGETS - OUTPUTS
        Dim output_errors As Matrix = Matrix.Subtract(targets, Me.output)
        Me.averageError = Math.Abs(output_errors.Average)

    End Sub

    ''' <summary>
    ''' This is our test function.
    ''' </summary>
    ''' <returns>The number of correct answers.</returns>
    Public Function Test(inputs!()) As Single()
        Return Me.FeedForward(inputs)
    End Function

End Class