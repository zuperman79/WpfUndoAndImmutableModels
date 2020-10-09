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
        private UserViewModel selectedUser;
        private RelayCommand addUserCommand;
        private ImmutableList<UserViewModel> users = ImmutableList.Create(
            new UserViewModel(new Person("gogu", "fanel", DateTime.Now, null)),
            new UserViewModel(new Person("gigi", "ninel", DateTime.Now, null)));

        public UserViewModel SelectedUser 
        {
            get => selectedUser; 
            set => SetProperty(ref selectedUser, () => selectedUser = value);
        }

        public ImmutableList<Country> Countries { get; }
            = ImmutableList.Create(new Country("ro", 1), new Country("en", 2));

        public ImmutableList<UserViewModel> Users 
        {
            get => users;
            set => SetProperty(ref users, () => users = value);
        }

        public RelayCommand AddUserCommand => 
            addUserCommand != null ? addUserCommand : addUserCommand = new RelayCommand(AddUser);

        private void AddUser() 
        {
             Users = users.Add(new UserViewModel(new Person("x", "x", DateTime.Now, Countries.First())));
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
                { nameof(SelectedUser), previousState => selectedUser = previousState as UserViewModel },
                { nameof(Users), previousState => users = previousState as ImmutableList<UserViewModel> }
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
