using System;
using WpfApp.Model;
using WpfApp.ViewModel;

namespace WpfAppTests
{
    class TestViewModel : IStateful
    {
        public void RestorePreviousState(string propertyName, object oldState, object newState)
        {
            throw new NotImplementedException();
        }
    }
}
