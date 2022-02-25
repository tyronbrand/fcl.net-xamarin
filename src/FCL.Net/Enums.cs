using System.Runtime.Serialization;

namespace FCL.Net
{
    public enum FCLServiceType
    {
        [EnumMember(Value = "authn")]
        Authn,

        [EnumMember(Value = "authz")]
        Authz,

        [EnumMember(Value = "pre-authz")]
        PreAuthz,

        [EnumMember(Value = "user-signature")]
        UserSignature,

        [EnumMember(Value = "back-channel-rpc")]
        BackChannel,

        [EnumMember(Value = "local-view")]
        LocalView,

        [EnumMember(Value = "open-id")]
        OpenID,

        [EnumMember(Value = "account-proof")]
        AccountProof
    }

    public enum Status
    {
        [EnumMember(Value = "PENDING")]
        Pending,

        [EnumMember(Value = "APPROVED")]
        Approved,

        [EnumMember(Value = "DECLINED")]
        Declined
    }

    public enum FCLServiceMethod
    {
        [EnumMember(Value = "HTTP/POST")]
        HttpPost,

        [EnumMember(Value = "HTTP/GET")]
        HttpGet,

        [EnumMember(Value = "VIEW/IFRAME")]
        Iframe,

        [EnumMember(Value = "IFRAME/RPC")]
        IframeRPC,

        [EnumMember(Value = "DATA")]
        Data
    }

    public enum FclConfigKey
    {
        [EnumMember(Value = "accessNode.api")]
        AccessNode,

        [EnumMember(Value = "app.detail.icon")]
        Icon,

        [EnumMember(Value = "app.detail.title")]
        Title,

        [EnumMember(Value = "challenge.handshake")]
        Handshake,

        [EnumMember(Value = "challenge.scope")]
        Scope,

        [EnumMember(Value = "discovery.wallet")]
        Wallet,

        [EnumMember(Value = "service.OpenID.scopes")]
        OpenIDScope,

        Authn,
        Env,
        Location
    }

    public enum FCLProvider
    {
        [EnumMember(Value = "GET")]
        Get,

        [EnumMember(Value = "POST")]
        Post
    }

    public enum DefaultFclWalletProvider
    {
        Blocto,
        Dapper
    }

    public enum ResultType
    {
        Success,
        HttpError,
        UserCancel,
        Timeout,
        UnknownError
    }
}
