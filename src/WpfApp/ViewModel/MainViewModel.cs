using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WpfApp.Model;

namespace WpfApp.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private PresonViewModel selectedPerson;
        private RelayCommand addUserCommand;
        private ImmutableList<PresonViewModel> persons = ImmutableList.Create(
            new PresonViewModel(new Person("Jean-Luc", "Picard", DateTime.Now, null)),
            new PresonViewModel(new Person("Luke", "Skywalker", DateTime.Now, null)));

        public PresonViewModel SelectedPerson 
        {
            get => selectedPerson; 
            set => SetProperty(ref selectedPerson, () => selectedPerson = value);
        }

        public ImmutableList<Country> Countries { get; }
            = ImmutableList.Create(new Country("ro", 1), new Country("en", 2));

        public ImmutableList<PresonViewModel> Persons 
        {
            get => persons;
            set => SetProperty(ref persons, () => persons = value);
        }

        public RelayCommand AddUserCommand => 
            addUserCommand != null ? addUserCommand : addUserCommand = new RelayCommand(AddUser);

        private void AddUser() 
        {
             Persons = persons.Add(new PresonViewModel(new Person("William", "Adama", DateTime.Now, Countries.First())));
        }

        public MainViewModel()
        {
            InitializeUndoActions();
        }

        #region undo
        Dictionary<string, Action<object>> undoableActions;

        private void InitializeUndoActions()
        {
            undoableActions = new Dictionary<string, Action<object>>() {
                { nameof(SelectedPerson), previousState => selectedPerson = previousState as PresonViewModel },
                { nameof(Persons), previousState => persons = previousState as ImmutableList<PresonViewModel> }
            };
        }

        public override void RestorePreviousState(string propertyName, object oldState, object newState) 
        {
            if (undoableActions.TryGetValue(propertyName, out var action))
            {
                action(oldState);
            }
            RaisePropertyChanged(propertyName);
        }
        #endregion
    }
}
