using Newtonsoft.Json;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.ErrorHandling;
using SeaPublicWebsite.ExternalServices.FileRepositories;
using SeaPublicWebsite.Helpers;

namespace SeaPublicWebsite.DataStores
{
    public class UserDataStore
    {
        private readonly IFileRepository fileRepository;

        public UserDataStore(IFileRepository fileRepository)
        {
            this.fileRepository = fileRepository;
        }
        
        public UserDataModel LoadUserData(string reference)
        {
            if (!IsReferenceValid(reference))
            {
                throw new UserReferenceNotFoundException
                {
                    Reference = reference
                };
            }

            return JsonConvert.DeserializeObject<UserDataModel>(fileRepository.Read(reference.ToUpper()));
        }
        
        public bool IsReferenceValid(string reference)
        {
            return fileRepository.GetFiles("").Contains(reference.ToUpper());
        }
        
        public void SaveUserData(UserDataModel userDataModel)
        {
            fileRepository.Write(userDataModel.Reference, JsonConvert.SerializeObject(userDataModel, Formatting.Indented));
        }
        
        public string GenerateNewReferenceAndSaveEmptyUserData()
        {
            string reference;
            do
            {
                reference = RandomHelper.Generate8CharacterReference();
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