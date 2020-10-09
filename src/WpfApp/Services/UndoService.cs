using System;
using System.Collections.Generic;
using WpfApp.Model;

namespace WpfApp.Services
{
    public static class UndoService
    {
        public static string CurrentWindow { get; set; }

        private static Dictionary<string, Stack<(IStateful, string, object, object )>> UndoStates { get; set; } 
            = new Dictionary<string, Stack<(IStateful, string, object, object )>>();

        public static void AddUndoState(IStateful viewModel, string propertyName, object oldState, object newState)
        {
            if (UndoStates.TryGetValue(CurrentWindow, out var windowState))
            {
                windowState.Push((viewModel, propertyName, oldState, newState));
            }
            else
            {
                var stack = new Stack<(IStateful viewModel, string propertyName, object oldState, object newState)>();
                stack.Push((viewModel, propertyName, oldState, newState));
                UndoStates.Add(CurrentWindow, stack);
            }
        }
        
        public static (IStateful viewModel, string propertyName, object oldState, object newState) GetWindowPreviousState => 
            UndoStates.TryGetValue(CurrentWindow, out var windowState) ? windowState.Pop(): throw new ArgumentException();


        public static bool CanUndo => 
            UndoStates.TryGetValue(CurrentWindow, out var windowState) ? windowState.Count > 0 : false;

        public static void ResetUndoStates()
        {
            UndoStates.Clear();
        }
    }
}
