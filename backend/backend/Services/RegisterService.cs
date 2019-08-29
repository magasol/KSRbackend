using DatabaseConnection.entities;
using DatabaseConnection.Repositories;
using System;

namespace backend
{
    class RegisterService
    {
        public enum RegisterParam { Login = 0, Password = 1, FirstName = 2, LastName = 3, Email = 4 };

        public static string PrepareRegisterResponse(string registerRequest)
        {
            string[] registerParams = registerRequest.Split(',');
            Console.WriteLine(" Register credentials: ({0})", registerRequest);

            TravellerRepository travellerRepository = new TravellerRepository();
            Traveller traveller = travellerRepository.FindUserByLogin(registerParams[(int)RegisterParam.Login]);
            if (traveller is null)
            {
                traveller = new Traveller
                {
                    id = travellerRepository.NextId(),
                    first_name = registerParams[(int)RegisterParam.FirstName],
                    last_name = registerParams[(int)RegisterParam.LastName],
                    email = registerParams[(int)RegisterParam.Email],
                    login = registerParams[(int)RegisterParam.Login],
                    password = registerParams[(int)RegisterParam.Password]
                };
                travellerRepository.Add(traveller);
                return new UserDataResponse(true,
                    traveller.id, traveller.first_name, traveller.last_name, traveller.email, traveller.login).ToString();
            }

            return new UserDataResponse(false).ToString();
        }

        public static string PrepareEmptyRegisterRepsonse()
        {
            return new UserDataResponse(false).ToString();
        }
    }
}