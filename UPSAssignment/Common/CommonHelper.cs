using System;
using System.Windows;

namespace UPSAssignment.Common
{
    public static class CommonHelper
    {
        #region Public Static Methods
        /// <summary>
        /// Method to display error message
        /// </summary>
        /// <param name="message">string content</param>
        public static void ShowErrorMessage(string message = "")
        {
            if (String.IsNullOrEmpty(message))
                message = "The application has encountered an unknown error. Please contact administrator";
            System.Windows.MessageBox.Show(message, "UPSAssignment-Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            return;
        }

        /// <summary>
        /// Method to display info message
        /// </summary>
        /// <param name="message">string content</param>
        public static void ShowInfoMessage(string message)
        {
            System.Windows.MessageBox.Show(message, "UPSAssignment-Info", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            return;
        }

        /// <summary>
        /// Method to display confirmation message
        /// </summary>
        /// <param name="message">string content</param>
        /// <returns>messagebox result</returns>
        public static MessageBoxResult ShowConfirmationMessage(string message)
        {
            var result = System.Windows.MessageBox.Show(message, "UPSAssignment-Confirm?", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
            return result;
        }

        /// <summary>
        /// Method to display error message
        /// </summary>
        /// <param name="message">string content</param>
        public static void ShowErrorMessage(Status statusType, string msg="")
        {
            var message = string.Empty;
            switch (statusType)
            {
                case Status.BadRequest:
                    message = "Please validate the entered user details, looks like bad request was mad";
                    break;
                case Status.FailedAuthentication:
                    message = "Failed to authenitcate the user, please use valid token";
                    break;
                case Status.UnauthorizedAccess:
                    message = "Unauthorized access is being made, please use valid token to perform the operation";
                    break;
                case Status.ResourceNotExists:
                    message = "Requested resource does not exists";
                    break;
                case Status.MethodNotAllowed:
                    message = "Please check the header to see allowed http methods";
                    break;
                case Status.UnsupportedMediaType:
                    message = "Unsupported media type. The requested content type or version number is invalid";
                    break;
                case Status.FailedDataValidation:
                    message = "Data validation failed for one or mode fields. Please validate the data once again.";
                    break;
                case Status.TooManyRequests:
                    message = "Too many requests. The request was rejected due to rate limiting.";
                    break;
                case Status.InternalServerError:
                    message = "Internal server error";
                    break;
                default:
                    break;
            }

            System.Windows.MessageBox.Show(message, "UPSAssignment-Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            return;
        }
        #endregion
    }
}
