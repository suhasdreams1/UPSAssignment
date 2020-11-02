using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UPSAssignment.Interfaces.Common;
using UPSAssignment.Models;

namespace UPSAssignment.Common
{
    public class UserAPI : IUserAPI
    {
        #region Private Field
        private HttpClient userClient;
        #endregion

        #region Constructor
        public UserAPI(HttpClient client)
        {
            this.userClient = client;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Method that returns the url based on the type of the operation
        /// </summary>
        /// <param name="operationType">type of operation user wants to perform</param>
        /// <param name="searchName">string name to search</param>
        /// <param name="id">number to hold the page number or userid</param>
        /// <returns></returns>
        private static string GetURL(Operations operationType, string searchName = "", int id = 0)
        {
            var baseUri = "https://gorest.co.in/public-api/";
            switch (operationType)
            {
                case Operations.GetAllUsers:
                    id = id == 0 ? id = 1 : id;
                    return baseUri + "users?page=" + id;

                case Operations.GetNameMatchingUser:
                    return baseUri + "users?name=" + searchName + "&page=" + id;

                case Operations.CreateNewUser:
                    return baseUri + "users";

                case Operations.DeleteUser:
                case Operations.UpdateUser:
                    return baseUri + "users/" + id;

                default:
                    break;
            }
            return "";
        }
        #endregion

        #region IUserAPI Implementation methods
        /// <summary>
        /// Method to fetch the user details
        /// </summary>
        /// <param name="searchName">search user name</param>
        /// <param name="pageNumber">page number</param>
        /// <returns></returns>
        public async Task<UserData> GetUsers(string searchName = "", int pageNumber = 1)
        {
            var url = string.IsNullOrEmpty(searchName) ? GetURL(Operations.GetAllUsers, id: pageNumber) : GetURL(Operations.GetNameMatchingUser, searchName, pageNumber);
            using (var response = await userClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsAsync<UserData>();
                    return data;
                }
                throw new Exception(response.ReasonPhrase);
            }
        }

        /// <summary>
        /// Method to add or update user details
        /// </summary>
        /// <param name="operationType">type of operation (Add/Update)</param>
        /// <param name="jsonUserObject">user model object to store</param>
        /// <param name="id">id of the user</param>
        /// <returns>save was successful or not(true/false)</returns>
        public async Task<int> AddUpdateUser(Operations operationType, object jsonUserObject, int id = 0)
        {
            var jsonSerializedObject = JsonConvert.SerializeObject(jsonUserObject);
            var data = new StringContent(jsonSerializedObject, Encoding.UTF8, "application/json");

            var url = GetURL(operationType, id: id);
            using (var response = operationType == Operations.CreateNewUser ? await userClient.PostAsync(url, data) : await userClient.PutAsync(url, data))
            {
                var resultContent = await response.Content.ReadAsAsync<ResponseModel>();
                return resultContent == null ? (int)response.StatusCode : resultContent.Code;
            }
        }

        /// <summary>
        /// Method to delete the user
        /// </summary>
        /// <param name="id">id of the user</param>
        /// <returns>delete was successful or not(true/false)</returns>
        public async Task<int> DeleteUser(int id)
        {
            using (var response = await userClient.DeleteAsync(GetURL(Operations.DeleteUser, id: id)))
            {
                var resultContent = await response.Content.ReadAsAsync<ResponseModel>();
                return resultContent == null ? (int)response.StatusCode : resultContent.Code;
            }
        } 
        #endregion
    }
}
