using System.Collections.Generic;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.ErrorHandling;
using SeaPublicWebsite.Helpers;

namespace SeaPublicWebsite.DataStores
{
    public class UserDataStore
    {
        private static readonly Dictionary<string, UserDataModel> userDataDatabase = new Dictionary<string, UserDataModel>();
        
        public UserDataModel LoadUserData(string reference)
        {
            if (!userDataDatabase.ContainsKey(reference))
            {
                throw new UserReferenceNotFoundException
                {
                    Reference = reference
                };
            }

            return userDataDatabase[reference];
        }
        
        public bool IsReferenceValid(string reference)
        {
            return userDataDatabase.ContainsKey(reference);
        }
        
        public void SaveUserData(UserDataModel userDataModel)
        {
            userDataDatabase[userDataModel.Reference] = userDataModel;
        }
        
        public string GenerateNewReferenceAndSaveEmptyUserData()
        {
            string reference;
            do
            {
                reference = RandomHelper.Generate8DigitReference();
            } while (IsReferenceValid(reference));

            var userDataModel = new UserDataModel
            {
                Reference = reference
            };
            SaveUserData(userDataModel);

            return reference;
        }
        
    }
}