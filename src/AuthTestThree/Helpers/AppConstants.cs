using System;
using Microsoft.WindowsAzure.MobileServices;

namespace AuthTestThree.Helpers
{
    public static class AppConstants
    {
        // Put constants here that are not of a sensitive nature
        public static MobileServiceClient MobileService = new MobileServiceClient("https://azurebackendtestapp.azurewebsites.net");
        public static string ApplicationURL = @"https://azurebackendtestapp.azurewebsites.net";
        //TODO: figure out URL scheme and insert it
        public static string URLScheme = "AuthTestThree-ClassicalConversations";
    }
}
