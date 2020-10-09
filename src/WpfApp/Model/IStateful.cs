using System;

namespace WpfApp.Model
{
    public interface IStateful
    {
        void RestorePreviousState(string propertyName, object oldState, object newState);
    }

    public interface IStateObject 
    {
        Func<IStateObject, string, object, IStateObject> CreateCopy { get; }
    }
}