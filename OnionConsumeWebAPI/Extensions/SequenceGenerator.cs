namespace OnionConsumeWebAPI.Extensions
{
    public static class SequenceGenerator
    {
        private static int currentSequenceNumber = 00001;

        public static string GetNextSequenceNumber()
        {
            currentSequenceNumber++;
            return currentSequenceNumber.ToString("D5"); // Format as a 4-digit number with leading zeros
        }
    }
}
