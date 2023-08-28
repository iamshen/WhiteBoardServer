namespace Infrastructure.Constants;

public static class JwtConstants
{
    internal const string ClaimTypeNamespace = "sso:qq:";

    public const string UserIdKey = "userid"; // 用户id,在机构内唯一
    public const string UserNameKey = "user_name"; // 用户姓名昵称
    public const string RoleIdKey = "role_id"; // 角色id, 参考角色类型
    public const string CorpIdKey = "corpid"; // 教育号平台 CorpId
    public const string AvatarKey = "avatar"; // 用户头像链接
    public const string CorpTypeKey = "corp_type"; // 当前机构类型, 0:DEFAULT; 1:代理商; 2:上级单位; 4:学校; 5:超管;
    public const string CorpNameKey = "corp_name"; // 机构名称
    public const string ChildIdKey = "child_id"; // 孩子的用户id（家长身份登录时返回）
    public const string LoginFactorsKey = "login_factors"; // 登录因子
    public const string TenantIdKey = "tenant_id"; // 租户id
    public const string PhoneKey = "phone"; // 租户id

    public static class Claims
    {
        public const string UserId = ClaimTypeNamespace + UserIdKey; // 用户id,在机构内唯一
        public const string UserName = ClaimTypeNamespace + UserNameKey; // 用户姓名昵称
        public const string RoleId = ClaimTypeNamespace + RoleIdKey; // 角色id, 参考角色类型
        public const string Avatar = ClaimTypeNamespace + AvatarKey; // 用户头像链接
        public const string CorpId = ClaimTypeNamespace + CorpIdKey; // 教育号平台 CorpId
        public const string CorpName = ClaimTypeNamespace + CorpNameKey; // 机构名称
        public const string LoginFactors = ClaimTypeNamespace + LoginFactorsKey; // 登录因子
        public const string TenantId = ClaimTypeNamespace + TenantIdKey; // 租户id
        public const string Phone = ClaimTypeNamespace + PhoneKey; // 手机号码

        public const string RoleName = "roleName";
        public const string Status = "status";
        public const string StatusName = "statusName";
        public const string Gender = "gender"; // 性别
        public const string GenderName = "genderName";
        public const string Position = "position"; // 岗位
        public const string Post = "post";
        public const string PostName = "postName";
        public const string Dutie = "dutie";
        public const string DutieName = "dutieName";

        public const string BirthDay = "birthDay"; // 生日
        public const string Home = "home"; // 家乡
        public const string Interest = "interest"; // 兴趣
        public const string Motto = "motto"; //座右铭
        public const string MessageLink = "msglink"; //私信链接

        public const string RoomId = ClaimTypeNamespace +  "room:id";
    }
}