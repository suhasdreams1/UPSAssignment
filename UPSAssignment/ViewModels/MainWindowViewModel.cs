using log4net;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using UPSAssignment.Interfaces.ViewModels;
using UPSAssignment.Models;
using UPSAssignment.Common;
using UPSAssignment.Interfaces.Common;
using System.ComponentModel;
using System.Linq;
using UPSAssignment.Views;

namespace UPSAssignment.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        #region Private Fields
        private UserData userData;
        private BackgroundWorker worker;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserAPI userApi;
        private string searchName;
        private string statusText;
        #endregion

        #region Properties
        public string StatusText
        {
            get { return statusText; }
            set
            {
                statusText = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }
        public int CurrentPageNumber { get; set; }
        public int LastPageNumber { get; set; }
        public string PageInformation { get { return "Page " + CurrentPageNumber + " of " + LastPageNumber; } }
        public string TotalRecordsText { get { return "Total Records : " + TotalRecords; } }
        public string SearchName { get { return searchName; } set { searchName = value; OnPropertyChanged(nameof(SearchName)); } }
        public UserModel SelectedUserModel { get; set; }
        public int TotalRecords { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }
        #endregion

        #region Commands
        public SimpleCommand AddUserCommand { get; set; }
        public SimpleCommand UpdateUserCommand { get; set; }
        public SimpleCommand DeleteUserCommand { get; set; }
        public SimpleCommand GetUsersCommand { get; set; }
        public SimpleCommand SearchUserCommand { get; set; }
        public SimpleCommand FirstPageCommand { get; set; }
        public SimpleCommand PreviousPageCommand { get; set; }
        public SimpleCommand NextPageCommand { get; set; }
        public SimpleCommand LastPageCommand { get; set; }
        #endregion

        #region Constructor
        public MainWindowViewModel(IUserAPI api)
        {
            SetDefaults();

            //initillize the user api
            userApi = api;

            //fetch the users data on load
            OnGetUsers("First");
            
            //Instantiate all the commands
            SearchUserCommand = new SimpleCommand(OnSearchUser, CanSearchUser);
            GetUsersCommand = new SimpleCommand(OnGetUsers, CanGetUsers);
            FirstPageCommand = new SimpleCommand(OnGetUsers, CanGetUsers);
            PreviousPageCommand = new SimpleCommand(OnGetUsers, CanGetUsers);
            NextPageCommand = new SimpleCommand(OnGetUsers, CanGetUsers);
            LastPageCommand = new SimpleCommand(OnGetUsers, CanGetUsers);
            AddUserCommand = new SimpleCommand(OnAddUpdateUser, CanAddUpdateUser);
            UpdateUserCommand = new SimpleCommand(OnAddUpdateUser, CanAddUpdateUser);
            DeleteUserCommand = new SimpleCommand(OnDeleteUser, CanDeleteUser);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Method to set the default values
        /// </summary>
        private void SetDefaults()
        {
            //Set the default values of the properties
            CurrentPageNumber = 0;
            LastPageNumber = 0;
            TotalRecords = 0;
            StatusText = "Fetching the users data...";
        }

        /// <summary>
        /// Method to update the Page information on the ui
        /// </summary>
        /// <param name="data"></param>
        private void SetPageInfo(UserData data)
        {
            LastPageNumber = data.Meta.Pagination.Pages;
            CurrentPageNumber = data.Meta.Pagination.Page;
            TotalRecords = data.Meta.Pagination.Total;
            OnPropertyChanged(nameof(PageInformation));
            OnPropertyChanged(nameof(TotalRecordsText));
        }

        /// <summary>
        /// Method to get the page number
        /// </summary>
        /// <param name="param">command parameter</param>
        /// <returns>page number</returns>
        private int GetPageNumer(string param)
        {
            var page = Enum.Parse(typeof(Pages), param);
            switch (page)
            {
                case Pages.First:
                    return 1;
                case Pages.Next:
                    return CurrentPageNumber < LastPageNumber ? CurrentPageNumber + 1 : LastPageNumber;
                case Pages.Previous:
                    return CurrentPageNumber == 1 ? 1 : CurrentPageNumber - 1;
                case Pages.Last:
                    return LastPageNumber;
                case Pages.All:
                    return 1;
                case Pages.Current:
                    return CurrentPageNumber;
                default:
                    return 1;
            }
        }
        #endregion

        #region Command Methods
        /// <summary>
        /// Command method to delete the user
        /// </summary>
        /// <param name="param"></param>
        public void OnDeleteUser(object param)
        {
            try
            {
                MessageBoxResult messageResult = CommonHelper.ShowConfirmationMessage("Do you really want to delete the salected entry (with id:" + SelectedUserModel.Id + ")?");
                if (messageResult == MessageBoxResult.Yes)
                {
                    worker = new BackgroundWorker() { WorkerReportsProgress = true };
                    worker.DoWork += async (sender, e) =>
                    {
                        var result = await userApi.DeleteUser(SelectedUserModel.Id);
                        if (result == (int)Status.DeletedOK)
                        {
                            CommonHelper.ShowInfoMessage("Selected user has been deleted successfully!");
                            OnGetUsers(Convert.ToString(Pages.Current));
                            log.Info("Record with id " + SelectedUserModel.Id + " has been deleted successfully");
                            StatusText = "User record has been deleted successfully!!";
                        }
                        else
                        {
                            CommonHelper.ShowErrorMessage((Status)result);
                        }
                    };
                    worker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception : " + ex.Message);
                CommonHelper.ShowErrorMessage();
            }
        }

        /// <summary>
        /// Method that decides if Delete command can be executed
        /// </summary>
        /// <param name="param">command parameter</param>
        /// <returns>can delete user (true/false)</returns>
        public bool CanDeleteUser(object param)
        {
            if (SelectedUserModel == null)
                return false;
            return true;
        }

        /// <summary>
        /// Command method to update the user
        /// </summary>
        /// <param name="param">command parameter</param>
        public void OnAddUpdateUser(object param)
        {
            try
            {
                var id = SelectedUserModel == null ? 0 : SelectedUserModel.Id;
                var addUpdateVM = new AddUpdateViewModel(userApi, Convert.ToString(param) == "Add" ? null : SelectedUserModel);
                var addUpdateWindow = new AddUpdateView(addUpdateVM);
                addUpdateWindow.ShowDialog();
                if (addUpdateVM.IsSaveSuccessful)
                {
                    OnGetUsers(Convert.ToString(Pages.Current));
                    StatusText = "User record has been saved successfully!";
                    log.Info(Convert.ToString(param) == "Add" ? "New user has been added successfully" : "User record has been updated successfully (with id " + id+")");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception : " + ex.Message);
                CommonHelper.ShowErrorMessage();
            }
        }

        /// <summary>
        /// Method that decides if Add/Update command can be executed
        /// </summary>
        /// <param name="param">command parameter</param>
        /// <returns>can add/update user (true/false)</returns>
        public bool CanAddUpdateUser(object param)
        {
            if (SelectedUserModel == null && Convert.ToString(param) == "Update")
                return false;
            return true;
        }

        /// <summary>
        /// Command method to search the user
        /// </summary>
        /// <param name="param">command parameter</param>
        public void OnSearchUser(object param)
        {
            try
            {
                worker = new BackgroundWorker() { WorkerReportsProgress = true };
                worker.DoWork += async (sender, e) =>
                {
                    userData = await userApi.GetUsers(searchName: SearchName);
                    if (userData.Data != null && userData.Data.Count > 0)
                    {
                        Users = new ObservableCollection<UserModel>(userData.Data.ToList());
                        SetPageInfo(userData);
                        StatusText = "Displaying matching entries!";
                        OnPropertyChanged(nameof(Users));
                        log.Info("Displaying matching entries for search text :" + SearchName + ", total records found :" + userData.Meta.Pagination.Total);
                    }
                    else
                    {
                        StatusText = "No matching users found!";
                        CommonHelper.ShowInfoMessage("No matching user found !");
                        log.Info("NO matching entries found for search text :" + SearchName);
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
        /// Method that decides if search command can be executed
        /// </summary>
        /// <param name="param">command parameter</param>
        /// <returns>can search user (true/false)</returns>
        public bool CanSearchUser(object param)
        {
            return !string.IsNullOrEmpty(SearchName);
        }

        /// <summary>
        /// Command method to fetch the users data
        /// </summary>
        /// <param name="param">Command Parameter</param>
        public void OnGetUsers(object param)
        {
            try
            {
                var pageNum = GetPageNumer(Convert.ToString(param));
                if (Convert.ToString(param) == Pages.All.ToString())
                    SearchName = string.Empty;

                worker = new BackgroundWorker() { WorkerReportsProgress = true };
                worker.DoWork += async (sender, e) =>
                {
                    userData = await userApi.GetUsers(searchName: SearchName, pageNumber: pageNum);
                    if (userData.Data != null && userData.Data.Count > 0)
                    {
                        Users = new ObservableCollection<UserModel>(userData.Data.ToList());
                        SetPageInfo(userData);
                        StatusText = "Displaying users data on page " + CurrentPageNumber;
                        OnPropertyChanged(nameof(Users));
                        log.Info("Displaying users data on page number " + CurrentPageNumber + ", total records found :" + userData.Meta.Pagination.Total);
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
        /// Method that decides if get users command can be executed
        /// </summary>
        /// <param name="param">Command parameter</param>
        /// <returns>can get user (true/false)</returns>
        public bool CanGetUsers(object param)
        {
            var pageNum = GetPageNumer(Convert.ToString(param));
            var page = Enum.Parse(typeof(Pages), Convert.ToString(param));

            switch (page)
            {
                case Pages.First:
                    return !(CurrentPageNumber == 1);
                case Pages.Next:
                    return !(CurrentPageNumber == LastPageNumber);
                case Pages.Previous:
                    return !(CurrentPageNumber == 1);
                case Pages.Last:
                    return !(CurrentPageNumber == LastPageNumber);
                case Pages.All:
                    return true;
                case Pages.Current:
                    return true;
            }
            return false;
        } 
        #endregion
    }
}
