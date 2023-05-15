namespace Hatsukoi.Common
{
    public static class StringExtensions
    {
        public static string MaskedAccount(this string account)
        {
            if (account == null) return null;
            if (account.Length <= 2) return account;
            return string.Concat(account.Substring(0, 1),
                new string('*', account.Length - 2), account.Substring(account.Length - 1));
        }
    }
}
