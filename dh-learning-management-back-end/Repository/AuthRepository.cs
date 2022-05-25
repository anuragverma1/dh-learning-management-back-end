﻿using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Dapper;
using dh_learning_management_back_end.Models;
using Microsoft.IdentityModel.Tokens;
using Npgsql;


namespace dh_learning_management_back_end.Repository;

public class AuthRepository
{
    private readonly string _connectionString;

    public AuthRepository() =>
        _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=L&D";

    private IDbConnection Connection => new NpgsqlConnection(_connectionString);

    public AuthResponseDto RegisterUser(UserRegisterDto request)
    {
        CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);

        var user = new User
        {
            Username = request.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Firstname = request.Firstname,
            Lastname = request.Lastname
        };

        if (AddUser(user))
        {
            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Account Created Successfully"
            };
        }

        return new AuthResponseDto
        {
            IsSuccess = false,
            Message = "Account Already Exists"
        };
    }

    public AuthResponseDto Login(UserLoginDto request)
    {
        var user = FindUser(request.Username);
        if (user == null)
        {
            return new AuthResponseDto { Message = "User Not Found" };
        }

        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return new AuthResponseDto() { Message = "Wrong Password" };
        }

        var token = CreateToken(user);

        return new AuthResponseDto
        {
            IsSuccess = true,
            Token = token,
            Message = "Login Successfull"
        };
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private Boolean AddUser(User user)
    {
        using var dbConnection = Connection;
        const string sQuery =
            @"INSERT INTO users(username, passwordhash, passwordsalt, firstname, lastname) VALUES (@username, @passwordhash, @passwordsalt, @firstname, @lastname)";
        dbConnection.Open();
        try
        {
            dbConnection.Execute(sQuery, user);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private User? FindUser(string username)
    {
        using var dbConnection = Connection;
        const string sQuery = @"Select * from Users where username=@username";
        dbConnection.Open();
        try
        {
            return dbConnection.QuerySingle<User>(sQuery, new { Username = username });
        }
        catch
        {
            return null;
        }
    }

    private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.GivenName, user.Firstname),
            new Claim(ClaimTypes.Surname, user.Lastname),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes("s1Wvvqh91qcQEbHMmSYwp3tLIhixrFjh"));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public IEnumerable<User> GetUsers()
    {
        using var dbConnection = Connection;
        const string sQuery = @"Select * from Users";
        return dbConnection.Query<User>(sQuery);
    }
}