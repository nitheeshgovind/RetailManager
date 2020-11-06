using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private IUserEndPoint _userEndPoint;
        private IWindowManager _windowManager;
        private BindingList<UserModel> _users;

        public BindingList<UserModel> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        public UserDisplayViewModel(IUserEndPoint userEndPoint, IWindowManager windowManager)
        {
            _userEndPoint = userEndPoint;
            _windowManager = windowManager;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadUsers();
        }

        public async Task LoadUsers()
        {
            try
            {
                Users = new BindingList<UserModel>(await _userEndPoint.GetAll());
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "Authorization Failed";

                StatusInfoViewModel status = new StatusInfoViewModel();

                if (ex.Message == "Unauthorized")
                    status.UpdateMessage("Unauthorized Access!!", "You do not have permission to access the Sales Screen.");
                else
                    status.UpdateMessage("Fatal Error", ex.Message);

                _windowManager.ShowDialog(status, null, settings);
                TryClose();
            }
        }
    }
}
