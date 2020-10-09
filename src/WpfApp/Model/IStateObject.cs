using System;

namespace WpfApp.Model
{
    public interface IStateObject 
    {
        Func<IStateObject, string, object, IStateObject> CreateCopy { get; }
    }
}