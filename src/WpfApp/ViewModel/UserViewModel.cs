using System;
using System.Runtime.CompilerServices;
using WpfApp.Model;

namespace WpfApp.ViewModel
{
    public class UserViewModel : BaseViewModel, IStateObject
    {
        private Person modelObject;

        public string FirstName 
        { 
            get => modelObject.FirstName; 
            set => SetProperty(ref modelObject, CreateAction(value) ); 
        }

        public string LastName 
        {
            get => modelObject.LastName;
            set => SetProperty(ref modelObject, CreateAction(value));
        }

        public DateTime DateOfBirth 
        {
            get => modelObject.DateOfBirth;
            set => SetProperty(ref modelObject, CreateAction(value));
        }

        public Country Country 
        {
            get => modelObject.Country;
            set => SetProperty(ref modelObject, CreateAction(value));
        }

        public Func<IStateObject, string, object, IStateObject> CreateCopy => 
            throw new NotImplementedException();

        public UserViewModel(Person user)
        {
            this.modelObject = user;
        }

        private Action CreateAction(object value, [CallerMemberName] string propertyName = null)
        {
            return () => modelObject = modelObject.CreateCopy(modelObject, propertyName, value) as Person;
        }

        #region undo
        public override void RestorePreviousState(string propertyName, object oldState, object newState)
        {
            modelObject = oldState as Person;
            RaisePropertyChanged(propertyName);
        }
        #endregion
    }
}
