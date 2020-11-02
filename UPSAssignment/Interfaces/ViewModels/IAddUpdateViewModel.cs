using UPSAssignment.Common;

namespace UPSAssignment.Interfaces.ViewModels
{
    public interface IAddUpdateViewModel
    {
        #region Properties
        /// <summary>
        /// Property to hold Id field value
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// Property to hold User name
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Property to store email address
        /// </summary>
        string Email { get; set; }
        /// <summary>
        /// Property to store selected gender
        /// </summary>
        string Gender { get; set; }
        /// <summary>
        /// Property to hold the status
        /// </summary>
        string Status { get; set; }
        /// <summary>
        /// Property that shows if save was successful
        /// </summary>
        bool IsSaveSuccessful { get; set; }
        /// <summary>
        /// String property that displays the window caption
        /// </summary>
        string WindowCaption { get; set; }
        /// <summary>
        /// String property that displays the save button caption
        /// </summary>
        string SaveButtonCaption { get; set; }

        #endregion

        #region Command
        /// <summary>
        /// Command to save(add/update) the user
        /// </summary>
        SimpleCommand SaveCommand { get; set; } 
        #endregion
    }
}
