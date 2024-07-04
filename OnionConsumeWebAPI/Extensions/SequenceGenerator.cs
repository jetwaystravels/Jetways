﻿namespace OnionConsumeWebAPI.Extensions
{
    public static class SequenceGenerator
    {
        private static int currentSequenceNumber = 00001;

        public static string GetNextSequenceNumber()
        {
            currentSequenceNumber++;
            return currentSequenceNumber.ToString("D5"); 
        }
    }
}
