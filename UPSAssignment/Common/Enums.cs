namespace UPSAssignment.Common
{
    /// <summary>
    /// Possible User API Operations
    /// </summary>
    public enum Operations
    { 
        GetAllUsers,
        GetNameMatchingUser,
        CreateNewUser,
        GetUserWithId,
        UpdateUser,
        DeleteUser
    }

    /// <summary>
    /// Pagination possible pages
    /// </summary>
    public enum Pages
    {
        First,
        Next,
        Previous,
        Last,
        All,
        Current
    }

    /// <summary>
    /// Status codes returned by API
    /// </summary>
    public enum Status
    { 
        OK = 200,
        CreatedOK = 201,
        DeletedOK = 204,
        ResourceUnmodified = 304,
        BadRequest = 400,
        FailedAuthentication = 401,
        UnauthorizedAccess = 403,
        ResourceNotExists = 404,
        MethodNotAllowed = 405,
        UnsupportedMediaType = 415,
        FailedDataValidation = 422,
        TooManyRequests = 429,
        InternalServerError = 500
    }
}
