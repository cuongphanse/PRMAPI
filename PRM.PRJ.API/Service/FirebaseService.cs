using Firebase.Auth;
using Firebase.Storage;
using PRM.PRJ.API.Models.ViewModel;

namespace PRM.PRJ.API.Service
{
    public static class FirebaseService
    {
        private static string ApiKey = "AIzaSyBLDYkXtfdYnKseDJbz6J72lousbPIrniE";
        private static string Bucket = "bmosfile.appspot.com";
        private static string AuthEmail = "unimarket@gmail.com";
        private static string AuthPassword = "123456";

        public static async Task<string> UploadImage(IFormFile file)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

            // get authentication token
            var authResultTask = auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            var authResult = await authResultTask;
            var token = authResult.FirebaseToken;


            string imageLink = "";
            FirebaseImageModel firebaseImageModel = new FirebaseImageModel();
                if (file.Length > 0)
                {
                    firebaseImageModel.ImageFile = file;
                    var stream = firebaseImageModel.ImageFile.OpenReadStream();
                    //you can use CancellationTokenSource to cancel the upload midway
                    var cancellation = new CancellationTokenSource();

                    var result = await new FirebaseStorage(Bucket,
                         new FirebaseStorageOptions
                         {
                             AuthTokenAsyncFactory = () => Task.FromResult(token)
                         })
                       .Child("items")
                       .Child(firebaseImageModel.ImageFile.FileName)
                       .PutAsync(stream, cancellation.Token);

                    cancellation.Cancel();
                return result;

            }

            return imageLink;

        }
    }
}
