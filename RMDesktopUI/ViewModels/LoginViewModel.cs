using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.Helpers;

namespace RMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private IAPIHelper _apiHelper;
        private string _userName;
        private string _password;
        private string _errorMessage;

        /// <summary>
        /// Username of the user
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; NotifyOfPropertyChange(() => UserName); NotifyOfPropertyChange(() => CanLogin); }
        }

        /// <summary>
        /// Password for the user account
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; NotifyOfPropertyChange(() => Password);  NotifyOfPropertyChange(() => CanLogin); }
        }

        /// <summary>
        /// Flag to indicate if there is error in the Login action
        /// </summary>
        public bool IsErrorVisible
        {
            get { return !string.IsNullOrEmpty(ErrorMessage); }
        }

        /// <summary>
        /// Error message of the Login action
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);
            }
        }

        /// <summary>
        /// Flag to indentify if the logic action can be started
        /// </summary>
        /// <param name="userName">Username provided by the user</param>
        /// <param name="password">Password provided by the user</param>
        /// <returns></returns>
        public bool CanLogin
        {
            get
            {
                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public LoginViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task Login()
        {
            ErrorMessage = string.Empty;
            try
            {
                var result = await _apiHelper.Authenticate(UserName, Password);
            }
            catch(Exception e)
            {
                ErrorMessage = e.Message;
            }
        }
    }
}
