using System;
using Caliburn.Micro;
using RMDesktopUI.EventModels;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEventModel>
    {
        private LoginViewModel _loginViewModel;
        private IEventAggregator _events;
        private SalesViewModel _salesViewModel;

        public ShellViewModel(LoginViewModel loginViewModel, SalesViewModel salesViewModel, IEventAggregator eventAggregator)
        {
            _loginViewModel = loginViewModel;
            _salesViewModel = salesViewModel;
            _events = eventAggregator;

            _events.Subscribe(this);

            ActivateItem(_loginViewModel);
        }

        public void Handle(LogOnEventModel message)
        {
            ActivateItem(_salesViewModel);
        }
    }
}
