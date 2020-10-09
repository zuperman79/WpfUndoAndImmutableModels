using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Runtime.CompilerServices;
using WpfApp.Model;
using WpfApp.Services;

namespace WpfApp.ViewModel
{
    //cool name, right?
    public abstract class BaseViewModel : ViewModelBase, IStateful
    {
        private RelayCommand undoCommand;
        public RelayCommand UndoCommand => 
            undoCommand != null ? undoCommand : undoCommand = new RelayCommand(Undo, CanUndo);

        public abstract void RestorePreviousState(string propertyName, object oldState, object newState);

        protected void SetProperty<T>(ref T model, Action setter, [CallerMemberName] string propertyName = null)
        {
            var oldState = model;
            setter();
            RaisePropertyChanged(propertyName);
            UndoService.AddUndoState(this, propertyName, oldState, model);
        }

        private bool CanUndo()
        {
            return UndoService.CanUndo;
        }

        public void Undo() 
        {
            var undoData = UndoService.GetWindowPreviousState;
            undoData.viewModel.RestorePreviousState(undoData.propertyName, undoData.oldState, undoData.newState);
        }
    }
}
