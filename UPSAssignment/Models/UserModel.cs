using System.Collections.Generic;

namespace UPSAssignment.Models
{
    /// <summary>
    /// Model class to hold the status code, meta and user collection 
    /// </summary>
    public class UserData
    {
        public int Code { get; set; }
        public MetaInfo Meta { get; set; }
        public List<UserModel> Data { get; set; }
    }

    /// <summary>
    /// Class to hold the pagination information
    /// </summary>
    public class MetaInfo
    {
        public PaginationInfo Pagination { get; set; }
    }

    /// <summary>
    /// Class to store the pagination information
    /// </summary>
    public class PaginationInfo
    {
        public int Total { get; set; }
        public int Pages { get; set; }
        public int Page { get; set; }
    }

    /// <summary>
    /// User model class that represents user details
    /// </summary>
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// Model class to capture the response
    /// </summary>
    public class ResponseModel
    {
        public int Code { get; set; } 
    }
}
