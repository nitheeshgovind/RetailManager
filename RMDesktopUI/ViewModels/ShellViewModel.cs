using Caliburn.Micro;
using RMDesktopUI.EventModels;
using RMDesktopUI.Library.Api;
using System.Threading;
using System.Threading.Tasks;

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
        public bool IsLoggedOut
        {
            get { return !IsLoggedIn; }
        }

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            _events = eventAggregator;

            _events.SubscribeOnUIThread(this);

            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
        }

        public async Task ExitApplication()
        {
            await TryCloseAsync();
        }

        public async Task LogIn()
        {
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
            //IsLoggedIn = true;
            //NotifyOfPropertyChange(() => IsLoggedIn);
            //NotifyOfPropertyChange(() => IsLoggedOut);
        }

        public async Task LogOut()
        {
            (IoC.Get<IAPIHelper>()).LogOff();
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
            IsLoggedIn = false;
            NotifyOfPropertyChange(() => IsLoggedIn);
            NotifyOfPropertyChange(() => IsLoggedOut);
        }

        public async Task UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), new CancellationToken());
        }

        public void Handle(LogOnEventModel message)
        {
            
        }

        public async Task HandleAsync(LogOnEventModel message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
            IsLoggedIn = true;
            NotifyOfPropertyChange(() => IsLoggedIn);
            NotifyOfPropertyChange(() => IsLoggedOut);
        }
    }
}
