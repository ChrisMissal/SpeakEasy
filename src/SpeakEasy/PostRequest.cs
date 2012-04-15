namespace SpeakEasy
{
    public sealed class PostRequest : PostLikeRequest
    {
        public PostRequest(Resource resource)
            : base(resource)
        {

        }

        public PostRequest(Resource resource, IRequestBody body)
            : base(resource, body)
        {

        }

        protected override string GetHttpMethod()
        {
            return "POST";
        }

        public override string ToString()
        {
            return string.Format("[PostRequest {0}]", Resource);
        }
    }
}