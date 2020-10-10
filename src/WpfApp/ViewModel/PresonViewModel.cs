using System;
using System.Runtime.CompilerServices;
using WpfApp.Model;

namespace WpfApp.ViewModel
{
    public class PresonViewModel : BaseViewModel, IStateObject
    {
        private Person modelObject;

        public string FirstName 
        { 
            get => modelObject.FirstName; 
            set => SetProperty(ref modelObject, CreateModelCopyAction(value) ); 
        }

        public string LastName 
        {
            get => modelObject.LastName;
            set => SetProperty(ref modelObject, CreateModelCopyAction(value));
        }

        public DateTime DateOfBirth 
        {
            get => modelObject.DateOfBirth;
            set => SetProperty(ref modelObject, CreateModelCopyAction(value));
        }

        public Country Country 
        {
            get => modelObject.Country;
            set => SetProperty(ref modelObject, CreateModelCopyAction(value));
        }

        public Func<IStateObject, string, object, IStateObject> CreateCopy => 
            throw new NotImplementedException();

        public PresonViewModel(Person user)
        {
            this.modelObject = user;
        }

        private Action CreateModelCopyAction(object value, [CallerMemberName] string propertyName = null)
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
