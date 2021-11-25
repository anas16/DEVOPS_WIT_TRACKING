using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DEVOPS_V2.View_Models
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public class RelayCommand : ICommand
        {
            private readonly Action cmdactionnone;
            private readonly Action<object> cmdaction;

            public RelayCommand(Action cmdactionnone)
            {
                this.cmdactionnone = cmdactionnone;
            }

            public RelayCommand(Action<object> cmdaction)
            {
                this.cmdaction = cmdaction;
            }

            public event EventHandler CanExecuteChanged
            {
                add
                {
                    CommandManager.RequerySuggested += value;
                }

                remove
                {
                    CommandManager.RequerySuggested -= value;
                }
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (parameter != null)
                {
                    cmdaction(parameter);
                }
                else
                {
                    cmdactionnone?.Invoke();
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;

            if (changed == null) return;
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

