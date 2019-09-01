using DatabaseConnection.entities;
using DatabaseConnection.Repositories;
using System;

namespace backend
{
    class LoginService
    {
        public enum LoginParam { Login = 0, Password = 1 };

        public static string PrepareLoginResponse(string loginRequest)
        {
            string[] loginParams = loginRequest.Split('?');
            string login = loginParams[(int)LoginParam.Login];
            string password = loginParams[(int)LoginParam.Password];
            Console.WriteLine(" Login credentials: ({0})", login);

            TravellerRepository travellerRepository = new TravellerRepository();
            Traveller traveller = travellerRepository.FindUserByLogin(login);
            if (traveller == null)
                return new UserDataResponse(false).ToString();
            UserDataResponse loginResponse = traveller.password == password
                ? new UserDataResponse(true,
                    traveller.id, traveller.first_name, traveller.last_name, traveller.email, traveller.login)
                : new UserDataResponse(false);
            return loginResponse.ToString();
        }

        public static string PrepareEmptyLoginRepsonse()
        {
            return new UserDataResponse(false).ToString();
        }
    }
}