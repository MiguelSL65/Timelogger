using System.Collections.Generic;

namespace Timelogger.Api.Responses;

public class ApplicationError
{
    public IEnumerable<string> Messages { get; set; }
    
    private ApplicationError(){}
    
    public static ApplicationError ValidationException(
        IEnumerable<string> errorMessages)
    {
        return new ApplicationError
        {
            Messages = errorMessages
        };
    }
    
    public static ApplicationError InternalServerError(string errorMessage)
    {
        return new ApplicationError
        {
            Messages = new List<string> { errorMessage }
        };
    }
    
    public static ApplicationError EntityNotFound(
        string message)
    {
        return new ApplicationError
        {
            Messages = new List<string> { message }
        };
    }
}