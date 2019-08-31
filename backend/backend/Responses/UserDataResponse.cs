﻿
namespace backend
{
    public class UserDataResponse
    {
        public UserDataResponse(bool Status,
            int UserID = 0, 
            string FirstName = "",
            string LastName = "", 
            string Email = "", 
            string Login = "")
        {
            this.Status = Status;
            this.UserID = UserID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Login = Login;
        }

        public bool Status { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }

        public override string ToString()
        {
            return Status.ToString() + "?"
                + UserID.ToString() + "?"
                + FirstName + "?"
                + LastName + "?"
                + Email + "?"
                + Login;

        }
    }
}