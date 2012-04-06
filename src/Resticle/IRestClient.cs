namespace Resticle
{
    /// <summary>
    /// The IRestClient is your entry point into a restful API. The methods map to HttpMethods methods on the server (GET/PUT/POST/PATCH/DELETE etc...) and 
    /// return a chainable rest response.
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// The root resource for this rest client, all calls will be relative to this resource
        /// </summary>
        Resource Root { get; }

        /// <summary>
        /// Executes an http get request
        /// </summary>
        /// <param name="relativeUrl">The relative url to get</param>
        /// <param name="segments">An object that contains any segments in the relativeUrl that need to be resolved</param>
        /// <returns>A chainable rest response</returns>
        IRestResponse Get(string relativeUrl, object segments = null);

        /// <summary>
        /// Executes an http post request
        /// </summary>
        /// <param name="body">An object that represents the body to post</param>
        /// <param name="relativeUrl">The relative url to post to</param>
        /// <param name="segments">An object that contains any segments in the relativeUrl that need to be resolved</param>
        /// <returns>A chainable rest response</returns>
        IRestResponse Post(object body, string relativeUrl, object segments = null);

        /// <summary>
        /// Executes an http post request without a body
        /// </summary>
        /// <param name="relativeUrl">The relative url to post to</param>
        /// <param name="segments">An object that contains any segments in the relativeUrl that need to be resolved</param>
        /// <returns>A chainable rest response</returns>
        IRestResponse Post(string relativeUrl, object segments = null);

        /// <summary>
        /// Executes an http put request
        /// </summary>
        /// <param name="body">An object that represents the body to put</param>
        /// <param name="relativeUrl">The relative url to put to</param>
        /// <param name="segments">An object that contains any segments in the relativeUrl that need to be resolved</param>
        /// <returns>A chainable rest response</returns>
        IRestResponse Put(object body, string relativeUrl, object segments = null);

        /// <summary>
        /// Executes an http put request without a body
        /// </summary>
        /// <param name="relativeUrl">The relative url to put to</param>
        /// <param name="segments">An object that contains any segments in the relativeUrl that need to be resolved</param>
        /// <returns>A chainable rest response</returns>
        IRestResponse Put(string relativeUrl, object segments = null);

        /// <summary>
        /// Executes an http delete request
        /// </summary>
        /// <param name="relativeUrl">The relative url to delete</param>
        /// <param name="segments">An object that contains any segments in the relativeUrl that need to be resolved</param>
        /// <returns>A chainable rest response</returns>
        IRestResponse Delete(string relativeUrl, object segments = null);

        /// <summary>
        /// Executes an http head request
        /// </summary>
        /// <param name="relativeUrl">The relative url to delete</param>
        /// <param name="segments">An object that contains any segments in the relativeUrl that need to be resolved</param>
        /// <returns>A chainable rest response</returns>
        IRestResponse Head(string relativeUrl, object segments = null);
    }
}

