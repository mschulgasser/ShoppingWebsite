﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Data
{
    public class AccountRepository
    {
        private string _connectionString;

        public AccountRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddAdmin(string firstName, string lastName, string email, string password)
        {
            string salt = GenerateSalt();
            string hash = HashPassword(password, salt);

            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                Admin admin = new Admin
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PasswordHash = hash,
                    PasswordSalt = salt
                };
                context.Admins.InsertOnSubmit(admin);
                context.SubmitChanges();
            }
        }

        public Admin Login(string emailAddress, string password)
        {
            Admin admin = GetAdmin(emailAddress);
            if (admin == null)
            {
                return null;
            }

            bool isMatch = IsMatch(password, admin.PasswordHash, admin.PasswordSalt);
            if (isMatch)
            {
                return admin;
            }

            return null;
        }

        private Admin GetAdmin(string email)
        {
            using (var context = new ShoppingDbDataContext(_connectionString))
            {
                return context.Admins.FirstOrDefault(a => a.Email == email);
            }

        }
        //public bool UserExists(string email)
        //{
        //    using (var context = new ShoppingDbDataContext(_connectionString))
        //    {
        //        return context.Admins.Any(a => a.Email == email);
        //    }
        //}
        private static string HashPassword(string password, string salt)
        {
            SHA256Managed crypt = new SHA256Managed();

            string combinedString = password + salt;
            byte[] combined = Encoding.Unicode.GetBytes(combinedString);

            byte[] hash = crypt.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }

        private static string GenerateSalt()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[10];
            provider.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static bool IsMatch(string passwordToCheck, string hashedPassword, string salt)
        {
            string hash = HashPassword(passwordToCheck, salt);
            return hash == hashedPassword;
        }
    }
}
