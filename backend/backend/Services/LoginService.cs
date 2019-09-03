using DatabaseConnection.entities;
using DatabaseConnection.Repositories;
using System;
using System.Threading;

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
            //TESTOWANIE WATKOW
            // 0) ODKOMENTUJ SLEEPA
            // 1) OTWORZ STRONE LOGOWANIA I WYSZUKIWANIA
            // 2) PRZYGOTUJ DANE LOGOWANIA I DANE WYSZUKIANIA 
            // 3) KLIKNIJ ZALOGUJ, !!POTEM!! KLIKNIJ WYSZUKAJ
            // WYNIK: WYSZUKIWANIE POWINNO ZOSTAĆ ZWROCONE PRZED ZALOGOWANIEM
            //Thread.Sleep(15000);
            //Console.WriteLine("Login threads wakes up");
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