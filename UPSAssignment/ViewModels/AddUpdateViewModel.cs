using log4net;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;
using UPSAssignment.Common;
using UPSAssignment.Interfaces.Common;
using UPSAssignment.Interfaces.ViewModels;
using UPSAssignment.Models;

namespace UPSAssignment.ViewModels
{
    public class AddUpdateViewModel : ViewModelBase, IAddUpdateViewModel
    {
        #region Private fields
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Operations operationType;
        private string windowCaption;
        private string saveButtonCaption;
        private IUserAPI userApi;
        private UserModel model;
        #endregion

        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public bool IsSaveSuccessful { get; set; }
        public string WindowCaption { get { return windowCaption; } set { windowCaption = value; OnPropertyChanged(nameof(WindowCaption)); } }
        public string SaveButtonCaption { get { return saveButtonCaption; } set { saveButtonCaption = value; OnPropertyChanged(nameof(SaveButtonCaption)); } }
        public string[] Genders { get { return new string[] { "Male", "Female" }; } }
        public string[] Statuses { get { return new string[] { "Active", "Inactive" }; } }
        #endregion

        #region Command
        public SimpleCommand SaveCommand { get; set; }
        #endregion

        #region Constructor
        public AddUpdateViewModel(IUserAPI api, UserModel userModel = null)
        {
            userApi = api;
            model = userModel;
            IsSaveSuccessful = false;
            operationType = userModel == null ? Operations.CreateNewUser : Operations.UpdateUser;
            WindowCaption = userModel == null ? "Add New User" : "Update User";
            SaveButtonCaption = userModel == null ? "Add" : "Update";
            SaveCommand = new SimpleCommand(OnSave, CanSave);
            if (userModel != null)
                SetUserModel(userModel);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// wrap the user details in the object to store
        /// </summary>
        /// <returns></returns>
        private object GetUserModel()
        {
            return new
            {
                name = Name,
                email = Email,
                gender = Gender,
                status = Status
            };
        }

        /// <summary>
        /// Set the local properties from user model object
        /// </summary>
        /// <param name="userModel">user model object to update</param>
        private void SetUserModel(UserModel userModel)
        {
            Name = userModel.Name;
            Email = userModel.Email;
            Gender = userModel.Gender;
            Status = userModel.Status;
            Id = userModel.Id;

            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Gender));
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(Id));
        } 
        #endregion

        #region Command Methods
        /// <summary>
        /// Command method to add/update the user details
        /// </summary>
        /// <param name="param">command parameter</param>
        public void OnSave(object param)
        {
            try
            {
                var worker = new BackgroundWorker() { WorkerReportsProgress = true };
                worker.DoWork += async (sender, e) =>
                {
                    var result = await userApi.AddUpdateUser(operationType, GetUserModel(), Id);
                    if (result == (int)Common.Status.CreatedOK || result == (int)Common.Status.OK)
                    {
                        CommonHelper.ShowInfoMessage("Entered User Details have been saved successfully!");
                        IsSaveSuccessful = true;
                        ((Window)param).Dispatcher.Invoke(() => { ((Window)param)?.Close(); }, DispatcherPriority.Normal);
                    }
                    else
                    {
                        CommonHelper.ShowErrorMessage((Status)result);
                    }
                };

                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                log.Error("Exception : " + ex.Message);
                CommonHelper.ShowErrorMessage();
            }
        }

        /// <summary>
        /// Method that decides if user can be added/updated or not
        /// </summary>
        /// <param name="param">command parameter</param>
        /// <returns>can add/update user (true/false)</returns>
        public bool CanSave(object param)
        {
            bool isEmail = string.IsNullOrEmpty(Email) ? false : Regex.IsMatch(Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Gender) || string.IsNullOrEmpty(Status) || !isEmail)
                return false;

            return true;
        } 
        #endregion

    }
}
