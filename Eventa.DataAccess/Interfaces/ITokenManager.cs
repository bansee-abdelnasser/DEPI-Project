using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Identity;

namespace Eventa.DataAccess.Repositories
{
    namespace Todo.DataAccess.Contracts
    {
        public interface ITokenManager
        {
            Task<TokenResult> GetTokenAsync(AppUser user);
            public TokenResult GetRefreshToken();

            Task<TokenResult> RefreshToken(string accessToken, string refreshToken);


        }
    }

}
