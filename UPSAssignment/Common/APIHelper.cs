using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace UPSAssignment.Common
{
    public class APIHelper 
    {
        #region Public Fields
        public HttpClient Client; 
        #endregion

        #region Private Fields
        private static APIHelper apiHelperObject;
        private string apiToken;
        private static readonly object lockObject = new object();
        #endregion

        #region Constructor
        private APIHelper()
        {
            apiToken = ConfigurationManager.AppSettings.Get("apiToken");
        }
        #endregion

        #region Public Static Method
        /// <summary>
        /// Method to initilize the API Helper object
        /// </summary>
        /// <returns>only instance of API Helper class</returns>
        public static APIHelper GetAPIHelper()
        {
            lock (lockObject)
            {
                if (apiHelperObject == null)
                {
                    apiHelperObject = new APIHelper();
                    apiHelperObject.SetupClient();
                }
            }
            return apiHelperObject;
        }
        #endregion

        #region Private Method
        /// <summary>
        /// Method to Setup the HttpClient
        /// </summary>
        private void SetupClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        } 
        #endregion
    }
}
