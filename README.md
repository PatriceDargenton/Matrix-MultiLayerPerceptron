# Matrix-MultiLayerPerceptron

Matrix Multi-Layer Perceptron in VB .NET from C# version https://github.com/nlabiris/perceptrons

This is the classical XOR test.

# Versions

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