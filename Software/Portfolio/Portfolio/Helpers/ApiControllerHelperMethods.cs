using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Portfolio.Helpers
{
    /// <summary>
    /// This class provides static methods that are common between the various controllers in order
    /// to reduce code duplication.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class ApiControllerHelperMethods
    {
        /// <summary>
        /// Creates a content result with an object that is serialized to json, having all names set to
        /// camelCase instead of PascalCase. 
        /// This is done to make deserialization easier on the client side, where camelCase is preferred for variables.
        /// </summary>
        /// <param name="data">The object to serialize</param>
        /// <param name="statusCode">200 (OK) by default, the status code to return to the browser</param>
        /// <returns>A content result with serialized json using camelCase for keys</returns>
        public static ContentResult JsonCamelCaseContentResult(Object data, int statusCode = 200)
        {
            return new ContentResult
            {
                ContentType = "application/json",
                StatusCode = statusCode,
                Content = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
            };
        }
    }
}
