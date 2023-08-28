using System.Security.Claims;
using Infrastructure.Constants;
using Infrastructure.Jwt;

var service = new JwtService();

var claims = new[]
{
    new Claim(JwtConstants.Claims.UserId, "123456789"),
    new Claim(JwtConstants.Claims.UserName, "黄深")
};

var token = service.CreateToken(claims);
Console.WriteLine("AccessToken: " + token);

var roomToken = service.CreateToken(new Claim(JwtConstants.Claims.RoomId, "d1d078da-37f9-406c-9078-da37f9506c60"));
Console.WriteLine("roomToken: " + roomToken);

var readClaims = service.ReadToken(roomToken);
foreach (var claim in readClaims)
{
    Console.WriteLine($"{claim.Type}: {claim.Value}");
}

