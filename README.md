# Implement Undo functionality in WPF MVVM with immutable models and some party tricks

An example project that leverages model immutability in order to achieve Undo functionality (C# 7).

The goal is to upgrate to C# 9 and use record types as models, and source generators in order to maximize productivity.

## How it works

Let's consider a model/view model pair, with the model immutable and the view model exposing the model properties for binding.

### The model:
```c#
public class Person : IStateObject
{
    public string FirstName { get; private set; }
...
```

The model is reather straightforward, note the ugly private setter needed for property initialization.

### The view model:
```c#
public class PresonViewModel : BaseViewModel, IStateObject
{
  private Person modelObject;
  
  public string FirstName 
  { 
      get => modelObject.FirstName; 
      set => SetProperty(ref modelObject, CreateModelCopyAction(value) ); 
  }
  private Action CreateModelCopyAction(object value, [CallerMemberName] string propertyName = null)
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

```c#
public void Undo() 
{
    var undoData = UndoService.GetWindowPreviousState;
    undoData.viewModel.RestorePreviousState(undoData.propertyName, undoData.oldState, undoData.newState);
}
```

And in the view model:

```c#
public override void RestorePreviousState(string propertyName, object oldState, object newState)
{
    modelObject = oldState as Person;
    RaisePropertyChanged(propertyName);
}
```

In this scenario, features announced in C# 9 will be handy:

1. private setters on the model's properties are used in order to be able to create copies of the model using reflection. This is where the record types of C# 9 and the `with` keyword would be useful.
2. the replication of properties on the view model could be done by source generators.

## More:

The undo functionality is implemented also in view models that do not have a backing immutable model (like the MainViewModel) and keeps track of property changes. Each undoable property change needs to be stated, see the `Dictionary<string, Action<object>> undoableActions`. 
One of the properties of the main view model where undo is implemented is the list of persons: `ImmutableList<PresonViewModel> Persons`. Using an immutable list is an elegant way of keeping track of collection changes that eliminates all the messy code associated with implementing a handler for the `OnCollectionChanged` event.


