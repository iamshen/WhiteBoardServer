// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using IdentityServer4;

namespace IdentityServerHost.Quickstart.UI
{
    public class TestUsers
    {
        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    countryCode="zh-cn",
                    country="中国",
                    provinceCode="440000000000",
                    province="广东省",
                    cityCode="440300000000",
                    city="深圳市",
                    areaCode="440303000000",
                    area="罗湖区",
                    streetCode="",
                    street="",
                    villageCode="",
                    village="",
                    detailed="翠竹街道贝丽北路97号水贝银座大厦",
                    longtitude=114.12097653219412,
                    latitude=22.57600259638566
                };
                
                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "234B5BAA-2636-4084-AB6B-85D4571545D2",
                        Username = "zhuyuanzhang",
                        Password = "Local@2023",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "朱元璋"),
                            new Claim(JwtClaimTypes.NickName, "洪武大帝"),
                            new Claim(JwtClaimTypes.PhoneNumber, "15958044099"),
                            new Claim(JwtClaimTypes.PhoneNumberVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.Email, "zhuyuanzhang@email.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "https://www.zhangsan.com"),
                            new Claim(JwtClaimTypes.Picture, "https://api.dicebear.com/6.x/lorelei/svg?flip=false"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "662B18FA-7739-433E-993C-77F3DA9D98D0",
                        Username = "judy",
                        Password = "Local@2023",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "朱棣"),
                            new Claim(JwtClaimTypes.NickName, "征北大将军"),
                            new Claim(JwtClaimTypes.PhoneNumber, "18938925740"),
                            new Claim(JwtClaimTypes.PhoneNumberVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.Email, "judy@email.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "https://www.lisi.com"),
                            new Claim(JwtClaimTypes.Picture, "https://api.dicebear.com/6.x/lorelei/svg?flip=false"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                        }
                    }
                };
            }
        }
    }
}