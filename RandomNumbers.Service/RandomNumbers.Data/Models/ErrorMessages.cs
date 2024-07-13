using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomNumbers.Data.Models;

public static class ErrorMessages
{
    #region Authorization Errors

    public const string ERR401000 = "Verification token is not provided. Add it to the HTTP request header with the 'Authorization' key. Authentication type = 'Bearer'";
    public const string ERR401001 = "Your authentication method is incorrect, the correct value is => 'Bearer <your_token_value>'";
    public const string ERR401002 = "The verification token is incorrect, please enter a valid token.";
    public const string ERR401003 = "User not found. Please enter a new key.";
    public const string ERR401004 = "Password has been updated. Please generate a new key.";
    public const string ERR401005 = "Cannot access the ActionDescriptor value. Please inform your administrator.";


    #endregion

    #region User Errors
    public const string ERR101000 = "Username information cannot be null.";
    public const string ERR101001 = "Username information cannot be empty.";
    public const string ERR101002 = "Password information cannot be null.";
    public const string ERR101003 = "Password information cannot be empty.";
    public const string ERR101004 = "User not found.";

    public const string ERR101100 = "Username information cannot be empty.";
    public const string ERR101101 = "The username '{PropertyValue}' has already been used.";
    public const string ERR101102 = "Password information cannot be empty.";
    public const string ERR101103 = "Password cannot be shorter than 6 characters.";
    public const string ERR101104 = "User can not register";
    public const string ERR101105 = "The username '{0}' has already been used.";


    public const string ERR101200 = "Password information cannot be null.";
    public const string ERR101201 = "Password information cannot be empty.";
    public const string ERR101202 = "Password cannot be shorter than 6 characters.";
    public const string ERR101203 = "User not found.";

    #endregion

    #region Match Errors

    public const string ERR111000 = "User has already played this match.";
    public const string ERR111001 = "Match does not exist or has already expired.";

    #endregion
}
