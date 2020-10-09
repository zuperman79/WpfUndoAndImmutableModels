using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WpfApp.Services;

namespace WpfAppTests
{
    [TestClass]
    public class UndoServiceTests
    {
        const string window = "testWindow";
        const string propertyName = "testProperty";

        [TestInitialize]
        public void TestInitialize()
        {
            UndoService.ResetUndoStates();
            UndoService.CurrentWindow = window;
        }

        [TestMethod]
        public void UndoService_ResetUndoStates_ClearsUndoStates()
        {
            //arrange
            UndoService.ResetUndoStates();
            bool exceptionThrown = false;

            //act
            try 
            {
                var state = UndoService.GetWindowPreviousState;
            }
            catch (ArgumentException ex)
            {
                exceptionThrown = true;
                Assert.IsNotNull(ex);
            }

            //assert
            Assert.IsTrue(exceptionThrown);
        }


        [TestMethod]
        public void UndoService_AddUndoState_AddsStateToUndoStates()
        {
            //arrange
            var testViewModel = new TestViewModel();

            //act
            UndoService.AddUndoState(testViewModel, propertyName, 1, 2);
            var state = UndoService.GetWindowPreviousState;

            //assert
            Assert.AreEqual(testViewModel, state.viewModel);
            Assert.AreEqual(propertyName, state.propertyName);
            Assert.AreEqual(1, state.oldState);
            Assert.AreEqual(2, state.newState);
        }
    }
}
