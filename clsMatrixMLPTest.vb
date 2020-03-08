
Option Infer On

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace MatrixMLP

    <TestClass()>
    Public Class MultiLayerPerceptronTest

        Private m_mlp As New MultiLayerPerceptron

        Private InputValue, TargetValue As Single(,)

        <TestInitialize()>
        Public Sub Init()

            InputValue = New Single(,) {
                {0, 0},
                {0, 1},
                {1, 0},
                {1, 1}}

            TargetValue = New Single(,) {
                {0},
                {1},
                {1},
                {0}}

        End Sub

        <TestMethod()>
        Public Sub MatrixMLPXORSigmoid()

            Dim nbIterations% = 100000
            m_mlp.SetActivationFunction(ActivationFunctionForMatrix.TActivationFunction.Sigmoid)

            Dim NeuronCount = New Integer() {2, 2, 1}
            m_mlp.InitStruct(NeuronCount, addBiasColumn:=True)
            m_mlp.Init(learningRate:=0.1)
            'm_mlp.Init(inputNodes:=2, hiddenNodes:=2, outputNodes:=1, learningRate:=0.1)

            m_mlp.weights_ih = {
                {0.5194889!, 0.3375039!},
                {0.502262!, 0.1634872!},
                {0.03079244!, 0.2952372!}}
            m_mlp.weights_ho = {
                {0.4413446!, 0.4016011!, 0.1111653!}}
            m_mlp.bias_h = {
                {0.1586298!},
                {0.953395!},
                {0.460079!}}
            m_mlp.bias_o = {
                {0.3956134!}}

            m_mlp.TrainSystematic(InputValue, TargetValue, nbIterations)

            Dim nbOutput% = TargetValue.GetLength(1)
            Dim m As Matrix = m_mlp.TestAllSamples(InputValue, nbOutput)

            Dim expectedOutput = New Single(,) {
                 {0.0},
                 {0.99},
                 {0.99},
                 {0.02}}

            Dim sOutput = m.ToString()
            Dim expectedMatrix As Matrix = expectedOutput ' Single(,) -> Matrix
            Dim sExpectedOutput = expectedMatrix.ToString()
            Assert.AreEqual(sOutput, sExpectedOutput)

            Dim rExpectedLoss# = 0.01
            Dim rLoss! = m_mlp.ComputeAverageError(TargetValue)
            Dim rLossRounded# = Math.Round(rLoss, 2)
            Assert.AreEqual(rExpectedLoss, rLossRounded)

        End Sub

        <TestMethod()>
        Public Sub MatrixMLPXORSigmoidWithoutBias()

            Dim nbIterations% = 100000
            m_mlp.SetActivationFunction(ActivationFunctionForMatrix.TActivationFunction.Sigmoid)

            Dim NeuronCount = New Integer() {2, 2, 1}
            m_mlp.InitStruct(NeuronCount, addBiasColumn:=False)
            m_mlp.Init(learningRate:=0.1)
            'm_mlp.Init(inputNodes:=2, hiddenNodes:=2, outputNodes:=1, learningRate:=0.1,
            '    useBias:=False)

            m_mlp.weights_ih = {
                {0.76!, 0.81!},
                {0.09!, 0.16!}}
            m_mlp.weights_ho = {
                {0.17!, 0.17!}}

            m_mlp.TrainSystematic(InputValue, TargetValue, nbIterations)

            Dim nbOutput% = TargetValue.GetLength(1)
            Dim m As Matrix = m_mlp.TestAllSamples(InputValue, nbOutput)

            Dim expectedOutput = New Single(,) {
                 {0.01},
                 {0.92},
                 {0.92},
                 {0.06}}

            Dim sOutput = m.ToString()
            Dim expectedMatrix As Matrix = expectedOutput ' Single(,) -> Matrix
            Dim sExpectedOutput = expectedMatrix.ToString()
            Assert.AreEqual(sOutput, sExpectedOutput)

            Dim rExpectedLoss# = 0.06
            Dim rLoss! = m_mlp.ComputeAverageError(TargetValue)
            Dim rLossRounded# = Math.Round(rLoss, 2)
            Assert.AreEqual(rExpectedLoss, rLossRounded)

        End Sub

    End Class

End Namespace