# Implement Undo functionality in WPF MVVM using immutable models (C#7)

An example project that leverages model immutability in order to achieve Undo functionality.

The goal is to upgrate to C# 9 and use record types as models, and source generators in order to maximize productivity.

## How it works

Let's consider a model/view model pair, with the model immutable and the view model exposing the model properties for binding.

### The model:
```
public class Person : IStateObject
{
    public string FirstName { get; private set; }
...
```

The model is reather straightforward, note the ugly private setter needed for property initialization.

### The view model:
```
public class PresonViewModel : BaseViewModel, IStateObject
{
  private Person modelObject;
  
  public string FirstName 
  { 
      get => modelObject.FirstName; 
      set => SetProperty(ref modelObject, CreateAction(value) ); 
  }
  private Action CreateAction(object value, [CallerMemberName] string propertyName = null)
  {
      return () => modelObject = modelObject.CreateCopy(modelObject, propertyName, value) as Person;
  }
  ...
```

Here, the view model replicates the model properties (blah) and adds some functionality:

  1. the getter exposes the model property value to the view;
  2. whenever a property changes, a copy of the model is created;
  3. the viewmodel, the new and old models are passed to a 'state change tracker service';
  4. the notify property change is raised on the respective property;

### The magic

Whenever an undo is necessary, the previous state is popped from the stack of changes and the 'RestorePreviousState' is called on the view model:

```
public void Undo() 
{
    var undoData = UndoService.GetWindowPreviousState;
    undoData.viewModel.RestorePreviousState(undoData.propertyName, undoData.oldState, undoData.newState);
}
```

And in the view model:

```
public override void RestorePreviousState(string propertyName, object oldState, object newState)
{
    modelObject = oldState as Person;
    RaisePropertyChanged(propertyName);
}
```
