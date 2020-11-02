using System.Windows;
using UPSAssignment.Common;
using UPSAssignment.ViewModels;
using UPSAssignment.Views;

namespace UPSAssignment
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Overridden Method
        /// <summary>
        /// Executes the method on application startup to display the landing window
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var userAPI = new UserAPI(APIHelper.GetAPIHelper().Client);
            var mainViewModel = new MainWindowViewModel(userAPI);
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        } 
        #endregion
    }
}