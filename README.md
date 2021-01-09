# Matrix-MultiLayerPerceptron

Matrix Multi-Layer Perceptron in VB .NET from C# version https://github.com/nlabiris/perceptrons

This is the classical XOR test.

# Versions

09/01/2021 V1.26
- HTangent -> Tanh

08/01/2021 V1.25
- Matrix.ToArraySingle -> ToArrayOfSingle

08/01/2021 V1.24
- Dataset directory

03/01/2021 V1.23
- modMLPTest.TestMLP2XORSigmoid fixed
- modActivation: Sigmoid and Tanh limits fixed
- clsRndExtension: NextDoubleGreaterThanZero added (RProp MLP)
- clsMLPGeneric.ShowWeights added, to compare configurations
- clsMLPGeneric.classificationObjective added (RProp MLP)
- clsMLPGeneric.useNguyenWidrowWeightsInitialization added (RProp MLP)
- clsMLPGeneric.minRandomValue added (RProp MLP)

12/12/2020 V1.22
- clsMLPGeneric.averageError: Single -> Double

06/12/2020 V1.21
- clsMLPGeneric.InitializeStruct function

21/11/2020 V1.20
- Iris flower prediction analog test added
- clsMLPGeneric.GetActivationFunctionType() added with enumActivationFunctionType
- clsMLPGeneric.RoundWeights() added
- clsMLPGeneric.ComputeErrorOneSample(targetArray!(,)) added
- clsMLPGeneric.ComputeAverageErrorOneSample!(targetArray!(,)) added

09/10/2020 V1.19
- Iris flower prediction test added
- Hyperbolic Tangent (Tanh) derivative fixed
- clsMLPGeneric.TestAllSamples: simplified
- clsMLPGeneric.PrintParameters: minimalSuccessTreshold displayed
- clsMLPGeneric.ShowThisIteration: also for last iteration

20/09/2020 V1.18
- Hyperbolic Tangent (Tanh) gain inversion: gain:=-2 -> gain:=2
- clsMLPGeneric.Initialize: weightAdjustment optional
- 3XOR tests added with three activation functions
- Compute success and fails after Train()
- Iris flower test added: https://en.wikipedia.org/wiki/Iris_flower_data_set
- Activation function: gain and center optional
- PrintWeights added for one XOR tests
- PrintOutput: option force display added

21/08/2020 V1.17
- ComputeSuccess added
- Tests added: 2 XOR and 3 XOR
- Refactored code in clsMLPGeneric: PrintOutput(iteration%), ComputeError(), ComputeAverageErrorFromLastError(), ComputeAverageError() and TestOneSample(input!(), ByRef ouput!())

04/08/2020 V1.16
- ActivationFunctionForMatrix ->
  ActivationFunctionOptimized
- Sigmoid and Hyperbolic Tangent (Bipolar Sigmoid) activations: optimized also with gain<>1
- Hyperbolic Tangent (Bipolar Sigmoid) activation: input/2
- Matrix class using Math.Net

25/06/2020 V1.15
- Matrix.ToVectorArraySingle() -> ToArraySingle()
- clsMLPGeneric: output Matrix instead of ouput array
- Single Matrix class: 2 times faster

06/06/2020 V1.14
- Source code cleaned
- finally weightAdjustment is not used in this implementation (only learningRate)
- LinearAlgebra.Matrix: only for clsMLPGeneric.lastError (not used there, MultiLayerPerceptron.lastError_ as MatrixMLP.Matrix instead)
- ComputeAverageError: in generic class
- Tests added for semi-stochastic and stochastic learning mode
- TrainSemiStochastic: fixed

16/05/2020 V1.13
- Tests: Assert rounded loss <= expected loss (instead of equality) to test other implementation without exactly the same loss

10/05/2020 V1.12
- Homogenization of function names
- clsMLPGeneric: PrintParameters: parameters added

02/05/2020 V1.11
- PrintOutput fixed
- PrintParameters: activation function name displayed

17/04/2020 V1.10
- Print output standardized
- Variable names simplification

12/04/2020 V1.09 OOP paradigm
- Weight initialization: rounded, to reproduce functionnal tests exactly
- Generic code: see this repositery: https://github.com/PatriceDargenton/One-Ring-to-rule-them-all

21/03/2020 V1.08 Functionnal tests (2)
- MultiLayerPerceptron: Test -> TestOneSample
- MultiLayerPerceptron: ComputeError, ComputeAverageError
- PrintWeights(): Print weights for functionnal tests
- XOR test: sample order changed, input and target generalized
- Matrix: +-* operators

08/03/2020 V1.07 Average error
- ComputeAverageError: Compute first abs then average
- UnitTestFramework.dll: local path

21/02/2020 V1.06 Double data type
- Matrix.data: Single -> Double: No more need to append ! Single sign in functionnal tests
- Activation function: Single -> Double

16/02/2020 V1.05 Indentation
- Visual Studio 2019 Indentation

16/02/2020 V1.04 Functionnal tests
- Activation function added: Rectified Linear Units (ReLU) (doesn't work)
- Functionnal tests: XOR-Sigmoid with bias and without bias
- Print outputs
- bias can be disabled
- Matrix to array and vice versa
- Matrix.ToString(): Print using a ready to code format
- Matrix: Convert whole Matrix object to array of single
- Matrix: Create a Matrix object from a whole array of single
- Matrix: Implicit conversion operator !(,) -> Matrix
- Matrix: Implicit conversion operator Matrix -> !(,)

31/12/2019 V1.03 Activation functions
- Activation functions

31/12/2019 V1.02 Average error
- Compute average error

31/12/2019 V1.01 Initial commit: C# -> VB .NET