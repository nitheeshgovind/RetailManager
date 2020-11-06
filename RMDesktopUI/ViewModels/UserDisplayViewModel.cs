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
        private UserModel _selectedUser;

        private string _selectedUserName;
        private BindingList<string> _selectedUserRoles;
        private BindingList<string> _availableRoles;
        private string _selectedRoleToRemove;
        private string _selectedRoleToAdd;

        public BindingList<UserModel> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }
        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                SelectedUserName = value.EmailAddress;                
                SelectedUserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                LoadRoles();
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }       
        public string SelectedUserName
        {
            get { return _selectedUserName; }
            set { _selectedUserName = value; NotifyOfPropertyChange(() => SelectedUserName); }
        }       
        public BindingList<string> SelectedUserRoles
        {
            get { return _selectedUserRoles; }
            set { _selectedUserRoles = value; NotifyOfPropertyChange(() => SelectedUserRoles); }
        }
        public BindingList<string> AvailableRoles
        {
            get { return _availableRoles; }
            set { _availableRoles = value; NotifyOfPropertyChange(() => AvailableRoles); }
        }
        public string SelectedRoleToRemove
        {
            get { return _selectedRoleToRemove; }
            set
            {
                _selectedRoleToRemove = value;
                NotifyOfPropertyChange(() => SelectedRoleToRemove);
            }
        }
        public string SelectedRoleToAdd
        {
            get { return _selectedRoleToAdd; }
            set
            {
                _selectedRoleToAdd = value;
                NotifyOfPropertyChange(() => SelectedRoleToAdd);
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

        private async Task LoadRoles()
        {
            var roles = await _userEndPoint.GetAllRoles();
            foreach (var role in roles)
            {
                if (SelectedUserRoles.Contains(role.Value))
                {
                    AvailableRoles.Add(role.Value);
                }
            }
        }

        public async Task RemoveSelectedRole()
        {
            await _userEndPoint.RemoveUserFromRole(SelectedUser.Id, SelectedRoleToRemove);
            SelectedUserRoles.Remove(SelectedRoleToRemove);
            AvailableRoles.Add(SelectedRoleToRemove);
        }

        public async Task AddSelectedRole()
        {
            await _userEndPoint.AddUserToRole(SelectedUser.Id, SelectedRoleToAdd);
            SelectedUserRoles.Add(SelectedRoleToAdd);
            AvailableRoles.Remove(SelectedRoleToAdd);
        }
    }
}
