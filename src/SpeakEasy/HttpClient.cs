using System;
using SpeakEasy.Extensions;

namespace SpeakEasy
{
    public class HttpClient : IHttpClient
    {
        /// <summary>
        /// Creates a new http client with default settings
        /// </summary>
        /// <param name="rootUrl">The root url that all requests will be relative to</param>
        /// <returns>A new http client</returns>
        public static IHttpClient Create(string rootUrl)
        {
            return Create(rootUrl, new HttpClientSettings());
        }

        /// <summary>
        /// Creates a new http client with custom settings
        /// </summary>
        /// <param name="rootUrl">The root url that all requests will be relative to</param>
        /// <param name="settings">The custom settings to use for this http client</param>
        /// <returns>A new http client</returns>
        public static IHttpClient Create(string rootUrl, HttpClientSettings settings)
        {
            settings.Validate();

            var transmissionSettings = new TransmissionSettings(settings.Serializers);

            var runner = new RequestRunner(
                transmissionSettings,
                settings.Authenticator);

            return new HttpClient(
                runner,
                settings.NamingConvention,
                settings.Logger,
                settings.UserAgent)
            {
                Root = new Resource(rootUrl),
                Logger = settings.Logger
            };
        }

        private readonly IRequestRunner requestRunner;

        private readonly IResourceMerger merger;

        public HttpClient(
            IRequestRunner requestRunner,
            INamingConvention namingConvention,
            ILogger logger,
            IUserAgent userAgent)
        {
            this.requestRunner = requestRunner;

            UserAgent = userAgent;
            Logger = logger;

            merger = new ResourceMerger(namingConvention);
        }

        public event EventHandler<BeforeRequestEventArgs> BeforeRequest;

        public event EventHandler<AfterRequestEventArgs> AfterRequest;

        public ILogger Logger { get; private set; }

        public Resource Root { get; set; }

        public IUserAgent UserAgent { get; private set; }

        public IHttpResponse Get(string relativeUrl, object segments = null)
        {
            var merged = BuildRelativeResource(relativeUrl, segments);
            var request = new GetRequest(merged);
            return Run(request);
        }

        public IHttpResponse Post(object body, string relativeUrl, object segments = null)
        {
            var merged = BuildRelativeResource(relativeUrl, segments ?? body, false);
            var request = new PostRequest(merged, new ObjectRequestBody(body));
            return Run(request);
        }

        public IHttpResponse Post(IFile file, string relativeUrl, object segments = null)
        {
            return Post(new[] { file }, relativeUrl, segments);
        }

        public IHttpResponse Post(IFile[] files, string relativeUrl, object segments = null)
        {
            var merged = BuildRelativeResource(relativeUrl, segments, false);
            var request = new PostRequest(merged, new FileUploadBody(merged, files));
            return Run(request);
        }

        public IHttpResponse Post(string relativeUrl, object segments = null)
        {
            var merged = BuildRelativeResource(relativeUrl, segments);
            var request = new PostRequest(merged);
            return Run(request);
        }

        public IHttpResponse Put(object body, string relativeUrl, object segments = null)
        {
            var merged = BuildRelativeResource(relativeUrl, segments ?? body, false);
            var request = new PutRequest(merged, new ObjectRequestBody(body));
            return Run(request);
        }

        public IHttpResponse Put(string relativeUrl, object segments = null)
        {
            var merged = BuildRelativeResource(relativeUrl, segments);
            var request = new PutRequest(merged);
            return Run(request);
        }

        public IHttpResponse Put(IFile file, string relativeUrl, object segments = null)
        {
            return Put(new[] { file }, relativeUrl, segments);
        }

        public IHttpResponse Put(IFile[] files, string relativeUrl, object segments = null)
        {
            var merged = BuildRelativeResource(relativeUrl, segments, false);
            var request = new PutRequest(merged, new FileUploadBody(merged, files));
            return Run(request);
        }

        public IHttpResponse Patch(object body, string relativeUrl, object segments = null)
        {
            var merged = BuildRelativeResource(relativeUrl, segments ?? body, false);
            var request = new PatchRequest(merged, new ObjectRequestBody(body));
            return Run(request);
        }

        public IHttpResponse Patch(string relativeUrl, object segments = null)
        {
            var merged = BuildRelativeResource(relativeUrl, segments);
            var request = new PatchRequest(merged);
            return Run(request);
        }

        public IHttpResponse Patch(IFile file, string relativeUrl, object segments = null)
        {
            return Patch(new[] { file }, relativeUrl, segments);
        }

        public IHttpResponse Patch(IFile[] files, string relativeUrl, object segments = null)
        {
            var merged = BuildRelativeResource(relativeUrl, segments, false);
            var request = new PatchRequest(merged, new FileUploadBody(merged, files));
            return Run(request);
        }

        public IHttpResponse Delete(string relativeUrl, object segments = null)
        {
            var merged = BuildRelativeResource(relativeUrl, segments);
            var request = new DeleteRequest(merged);
            return Run(request);
        }

        public IHttpResponse Head(string relativeUrl, object segments = null)
        {
            var merged = BuildRelativeResource(relativeUrl, segments);
            var request = new HeadRequest(merged);
            return Run(request);
        }

        public IHttpResponse Options(string relativeUrl, object segments = null)
        {
            var merged = BuildRelativeResource(relativeUrl, segments);
            var request = new OptionsRequest(merged);
            return Run(request);
        }

        public IAsyncHttpRequest GetAsync(string relativeUrl, object segments = null)
        {
            var merged = BuildRelativeResource(relativeUrl, segments);
            var request = new GetRequest(merged);
            return RunAsync(request);
        }

        public IHttpResponse Run<T>(T request)
            where T : IHttpRequest
        {
            request.UserAgent = UserAgent;

            OnBeforeRequest(request);
            var response = requestRunner.Run(request);

            OnAfterRequest(request, response);

            return response;
        }

        public IAsyncHttpRequest RunAsync<T>(T request)
            where T : IHttpRequest
        {
            request.UserAgent = UserAgent;

            return new AsyncHttpRequest<T>(requestRunner, request);
        }

        public Resource BuildRelativeResource(string relativeUrl, object segments, bool shouldMergeProperties = true)
        {
            var resource = Root.Append(relativeUrl);
            return merger.Merge(resource, segments, shouldMergeProperties);
        }

        private void OnBeforeRequest(IHttpRequest request)
        {
            Logger.BeforeRequest(request);
            BeforeRequest.Raise(this, new BeforeRequestEventArgs(request));
        }

        private void OnAfterRequest(IHttpRequest request, IHttpResponse response)
        {
            AfterRequest.Raise(this, new AfterRequestEventArgs(request, response));
            Logger.AfterRequest(request, response);
        }
    }
}