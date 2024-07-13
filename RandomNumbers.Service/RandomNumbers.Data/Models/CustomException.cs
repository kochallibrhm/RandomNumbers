using System;

namespace RandomNumbers.Data.Models;


public class CustomException : Exception
{
    public string ErrorCode { get; set; }

    public string ErrorMessage { get; set; }

}
