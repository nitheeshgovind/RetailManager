using System;
using Caliburn.Micro;
using RMDesktopUI.EventModels;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEventModel>
    {
        private IEventAggregator _events;

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                _isLoggedIn = value;
                NotifyOfPropertyChange(() => IsLoggedIn);
            }
        }

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            _events = eventAggregator;

            _events.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void ExitApplication()
        {
            TryClose();
        }

        public void LogOut()
        {
            (IoC.Get<ILoggedInUserModel>()).LogOff();
            ActivateItem(IoC.Get<LoginViewModel>());
            IsLoggedIn = false;
        }

        public void Handle(LogOnEventModel message)
        {
            ActivateItem(IoC.Get<SalesViewModel>());
            IsLoggedIn = true;
        }
    }
}
