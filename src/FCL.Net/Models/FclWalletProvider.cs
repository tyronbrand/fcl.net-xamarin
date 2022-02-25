using System;
using System.Collections.Generic;

namespace FCL.Net.Models
{
    public class FclWalletProvider
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public FclServiceMethod Method { get; set; }
        public Uri Endpoint { get; set; }

        public static FclWalletProvider GetDefaultWalletProvider(DefaultFclWalletProvider walletProvider)
        {
            switch (walletProvider)
            {
                case DefaultFclWalletProvider.Dapper:
                    return new FclWalletProvider
                    {
                        Id = "dapper",
                        Name = "Dapper",
                        Method = FclServiceMethod.HttpPost,
                        Endpoint = new Uri("https://dapper-http-post.vercel.app/api/authn")
                    };
                case DefaultFclWalletProvider.Blocto:
                    return new FclWalletProvider
                    {
                        Id = "blocto",
                        Name = "Blocto",
                        Method = FclServiceMethod.HttpPost,
                        Endpoint = new Uri("https://flow-wallet.blocto.app/api/flow/authn")
                    };
                default:
                    return null;
            }
        }

        public static List<FclWalletProvider> GetDefaultWalletProviders()
        {
            return new List<FclWalletProvider>
            {
                GetDefaultWalletProvider(DefaultFclWalletProvider.Dapper),
                GetDefaultWalletProvider(DefaultFclWalletProvider.Blocto)
            };
        }
    }
}
