
Namespace ActivationFunctionForMatrix

    ' Matrix implementation requires activation function expressed from 
    '  its direct function: f'(x)=g(f(x))

    Public Enum TActivationFunction
        Sigmoid = 1
        HyperbolicTangent = 2
        ''' <summary>
        ''' Exponential Linear Units
        ''' </summary>
        ELU = 3
        ''' <summary>
        ''' Rectified Linear Units (ReLU)
        ''' </summary>
        ReLU = 4
    End Enum

    ''' <summary>
    ''' Interface for all activation functions
    ''' </summary>
    Public Interface IActivationFunctionForMatrix
        ''' <summary>
        ''' Activation function
        ''' </summary>
        Function Activation#(x#, gain#, center#)
        ''' <summary>
        ''' Derivative function
        ''' </summary>
        Function Derivative#(x#, gain#)
    End Interface

    ''' <summary>
    ''' Implements f(x) = Sigmoid
    ''' </summary>
    Public Class SigmoidFunction : Implements IActivationFunctionForMatrix

        Public Function Activation#(x#, gain#, center#) Implements IActivationFunctionForMatrix.Activation

            Dim y# = 1 / (1 + Math.Exp(-(x - center)))
            Return y

        End Function

        Public Function Derivative#(x#, gain#) Implements IActivationFunctionForMatrix.Derivative

            Dim y# = x * (1 - x)
            Return y

        End Function

    End Class

    ''' <summary>
    ''' Implements f(x) = Hyperbolic Tangent
    ''' </summary>
    Public Class HyperbolicTangentFunction : Implements IActivationFunctionForMatrix

        Public Function Activation#(x#, gain#, center#) Implements IActivationFunctionForMatrix.Activation

            Dim xc# = x - center
            Dim y# = 2 / (1 + Math.Exp(-2 * xc)) - 1
            Return y

        End Function

        Public Function Derivative#(x#, gain#) Implements IActivationFunctionForMatrix.Derivative

            Dim y# = 1 - x * x
            Return y

        End Function

    End Class

    ''' <summary>
    ''' Implements f(x) = Exponential Linear Unit (ELU)
    ''' </summary>
    Public Class ELUFunction : Implements IActivationFunctionForMatrix

        Public Function Activation#(x#, gain#, center#) Implements IActivationFunctionForMatrix.Activation

            Dim xc# = x - center
            Dim y#
            If xc >= 0 Then
                y = xc
            Else
                y = gain * (Math.Exp(xc) - 1)
            End If
            Return y

        End Function

        Public Function Derivative#(x#, gain#) Implements IActivationFunctionForMatrix.Derivative

            If gain < 0 Then Return 0

            Dim y#
            If x >= 0 Then
                y = 1
            Else
                y = x + gain
            End If
            Return y

        End Function

    End Class

    ''' <summary>
    ''' Implements Rectified Linear Unit (ReLU)
    ''' </summary>
    Public Class ReluFunction : Implements IActivationFunctionForMatrix

        Public Function Activation#(x#, gain#, center#) Implements IActivationFunctionForMatrix.Activation
            Dim xc# = x - center
            Return Math.Max(xc * gain, 0)
        End Function

        Public Function Derivative#(x#, gain#) Implements IActivationFunctionForMatrix.Derivative
            If x >= 0 Then Return gain
            Return 0
        End Function

    End Class

End Namespace