using System.Collections.Generic;

namespace ProjetoArtCouro.Web.Infra.Service
{
    public class TokenModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string user_name { get; set; }
        public string roles { get; set; }
    }
}