using System.Collections.ObjectModel;
using UPSAssignment.Common;
using UPSAssignment.Models;

namespace UPSAssignment.Interfaces.ViewModels
{
   public interface IMainWindowViewModel
    {
        #region Properties
        /// <summary>
        /// String property to display status message
        /// </summary>
        string StatusText { get; set; }
        /// <summary>
        /// Property that holds the Current Page Number
        /// </summary>
        int CurrentPageNumber { get; set; }
        /// <summary>
        /// Property that points to Last Page Number
        /// </summary>
        int LastPageNumber { get; set; }
        /// <summary>
        /// Read-Only Property to display page information on the UI
        /// </summary>
        string PageInformation { get; }
        /// <summary>
        /// Read-Only Property to display total record count
        /// </summary>
        string TotalRecordsText { get; }
        /// <summary>
        /// Property that holds the Search field text
        /// </summary>
        string SearchName { get; set; }
        /// <summary>
        /// Property that points to seleccted user model on the datagrid
        /// </summary>
        UserModel SelectedUserModel { get; set; }
        /// <summary>
        /// Numeric property that holds the value of Total number of records
        /// </summary>
        int TotalRecords { get; set; }
        /// <summary>
        /// User model collection to display the records on UI
        /// </summary>
        ObservableCollection<UserModel> Users { get; set; }
        #endregion

        #region Commands
        /// <summary>
        /// Command to add/create new user
        /// </summary>
        SimpleCommand AddUserCommand { get; set; }
        /// <summary>
        /// Command to modify the existing user
        /// </summary>
        SimpleCommand UpdateUserCommand { get; set; }
        /// <summary>
        /// Command to delete the user
        /// </summary>
        SimpleCommand DeleteUserCommand { get; set; }
        /// <summary>
        /// Command to fetch the users data
        /// </summary>
        SimpleCommand GetUsersCommand { get; set; }
        /// <summary>
        /// Command to search the user having matching name
        /// </summary>
        SimpleCommand SearchUserCommand { get; set; }
        /// <summary>
        /// Command to display users of 1st page
        /// </summary>
        SimpleCommand FirstPageCommand { get; set; }
        /// <summary>
        /// Command to display users on privious page
        /// </summary>
        SimpleCommand PreviousPageCommand { get; set; }
        /// <summary>
        /// Command to display users on next page
        /// </summary>
        SimpleCommand NextPageCommand { get; set; }
        /// <summary>
        /// Command to display users on last page
        /// </summary>
        SimpleCommand LastPageCommand { get; set; }
        #endregion

    }
}
