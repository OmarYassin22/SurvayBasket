namespace Core.Abestraction.Errors;
public static class UserErrors
{
    public static readonly Error UserNotFound = new Error("user_not_found", "User not found");
    public static readonly Error InvaliedCredintioals = new Error("user_invalid_credentials", "Invalid Username/Password");
}
