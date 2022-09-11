using System;
using System.Threading.Tasks;
using NUnit.Framework;
using AndriiCo.ConverterClient.ConverterApp.DataAccess;
using AndriiCo.ConverterClient.ConverterApp.ViewModel;
using Rhino.Mocks;

namespace ConverterAppTests
{
    [TestFixture]
    public class ConvertViewModelTests
    {
        [Test]
        public void ConvertExecute_CallsClientConvert_Behaviour()
        {
            // Arrange
            var client = MockRepository.GenerateMock<IConverterServiceClient>();
            var viewModel = new ConvertViewModel(client);

            client.Expect(x => x.ConvertAsync("AmountNumber")).Repeat.Once().IgnoreArguments();

            // Act
            viewModel.ConvertExecuteAsync();

            // Assert
            client.VerifyAllExpectations();
        }

        [Test]
        public void ConvertExecute_UpdatesViewModelCorrectly()
        {
            // Arrange
            var client = MockRepository.GenerateStub<IConverterServiceClient>();
            var viewModel = new ConvertViewModel(client) { AmountNumber = "AmountNumber" };

            string words = "AmountWords";
            var task = Task.FromResult(words);
                        
            client.Stub(x => x.ConvertAsync(viewModel.AmountNumber))
                .Return(task);

            // Act
            viewModel.ConvertExecuteAsync();

            // Assert
            Assert.AreEqual(words, viewModel.AmountWords);
            Assert.IsTrue(viewModel.CanRun);
            Assert.IsFalse(viewModel.IsProgressBarVisible);
            Assert.IsTrue(viewModel.CanClear);
        }

        [Test]
        public void ClearExecute_UpdatesViewModelCorrectly()
        {
            // Arrange
            var client = MockRepository.GenerateStub<IConverterServiceClient>();
            var viewModel = new ConvertViewModel(client) {AmountWords = "AmountWords", IsError = true};

            // Act
            viewModel.ClearExecute();

            // Assert
            Assert.IsFalse(viewModel.IsError);
            Assert.IsFalse(viewModel.CanClear);
            Assert.AreEqual(string.Empty, viewModel.AmountWords);
        }

        [Test]
        public void ConvertExecute_UpdatesViewModelCorrectly_WhenAnExceptionHappens()
        {
            // Arrange
            var client = MockRepository.GenerateMock<IConverterServiceClient>();
            var viewModel = new ConvertViewModel(client);

            const string Exception = "SomeException";            
            client.Expect(x => x.ConvertAsync("AmountNumber"))
                .Repeat.Once().Throw(new Exception(Exception)).IgnoreArguments();

            // Act
            viewModel.ConvertExecuteAsync();

            // Assert
            Assert.IsTrue(viewModel.IsError);
            Assert.IsNotEmpty(viewModel.AmountWords);
        }
    }
}
