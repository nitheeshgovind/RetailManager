using System;
using Caliburn.Micro;
using RMDesktopUI.EventModels;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEventModel>
    {
        private IEventAggregator _events;

        public ShellViewModel(LoginViewModel loginViewModel, SalesViewModel salesViewModel, IEventAggregator eventAggregator)
        {
            _events = eventAggregator;

            _events.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LogOnEventModel message)
        {
            ActivateItem(IoC.Get<LoginViewModel>());
        }
    }
}
