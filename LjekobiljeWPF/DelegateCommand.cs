using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ljekobilje
{
    public class DelegateCommand<T> : ICommand where T :class
    {
        public event EventHandler CanExecuteChanged;

        private Action<T> _command;

        private bool _ignoreUserType;

        public DelegateCommand(Action<T> command)
        {
            _ignoreUserType = false;
            _command = command;
        }

        public DelegateCommand(Action<T> command, Boolean ignoreUser)
        {
            _ignoreUserType = ignoreUser;
            _command = command;
        }

        public bool CanExecute(object parameter)
        {
            if (_ignoreUserType)
                return true;
            return (Application.Current as App).CurrentUser.UserType > 1;
        }

        public void Execute(object parameter)
        {
            _command.Invoke(parameter as T);
        }
    }
}
