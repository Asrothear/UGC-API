namespace UGC_API.Models.v1_0
{
    public class AuthModel
    {
        /// <summary>
        /// Returns the ID of the Service, if Service unkown id is 0.
        /// </summary>
        public int id { get; set; } = 0;
        /// <summary>
        /// Response Body. Contains Information refering the AuthToken.
        /// </summary>
        public Response response { get; set; } = new();
        public class Response
        {
            /// <summary>
            /// Token Vallid
            /// </summary>
            public bool Valid { get; set; } = false;
            /// <summary>
            /// Token refers to an active CMDr
            /// </summary>
            public bool Cmdr { get; set; } = false;
            /// <summary>
            /// Token ist Blocked for the requesting Service
            /// </summary>
            public bool Blocked { get; set; } = false;
        }
    }
}
