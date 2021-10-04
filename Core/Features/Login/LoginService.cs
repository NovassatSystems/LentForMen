using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Core
{
    public interface ILoginService
    {
        Task<bool> CheckIfResetIsRequested();
        Task<bool> IsUserExist(string name);
        Task<bool> RegisterUser(UserModel user);
        Task<bool> RegisterWorkouts();
        Task<UserModel> LoginUser(string Usename, string Password);
        Task<FraseInicialModel> GetFraseInicial();
    }

    public class FraseInicialModel
    {
        public string Description { get; set; }
        public string ImageBackground { get; set; }
    }

    public class LoginService : ILoginService
    {
        FirebaseClient client;

        public LoginService()
        {
            client = new FirebaseClient("https://lentcontrolusers-default-rtdb.firebaseio.com/");
        }

        public async Task<FraseInicialModel> GetFraseInicial()
        {
            var frase = (await client.Child("FraseInicial").OnceAsync<FraseInicialModel>()).FirstOrDefault().Object;

            return frase;
        }
        public async Task<bool> IsUserExist(string name)
        {
            var user = (await client.Child("Users")
                .OnceAsync<UserModel>()).Where(u => u.Object.Username == name)
                .FirstOrDefault();

            return user != null;
        }

        public async Task<bool> RegisterUser(UserModel user)
        {
            if (!(await IsUserExist(user.Username)))
            {
                await client.Child("Users")
                    .PostAsync(new UserModel()
                    {
                        Username = user.Username,
                        Password = user.Password,
                        Reset = user.Reset,
                        DeviceModel = user.DeviceModel
                    });
                return true;
            }
            return false;
        }

        public async Task<bool> RegisterWorkouts()
        {
            await client.Child("Workouts")
                .PostAsync(new ExerciciosModel
                {
                    Name = "Workout5",
                    Days = "34-36-38-40-42-44-46",
                    URL = "https://esposopaieprovedor.com.br/wp-content/uploads/2021/03/quaresma-semana-4-e-5-B.pdf",
                    WorkoutList = new List<WorkoutModel>()
                    {
                        new WorkoutModel
                        {
                            Name = "POLICHINELO",
                            Prescrition = "Prescrição: 10 repetições",
                            Observations = new List<string> 
                            { 
                                "* Mantenha a velocidade constante.",
                                "NÃO FAÇA PAUSA. Inicie imediatamente o próximo exercício"
                            }
                        },
                        new WorkoutModel
                        {
                            Name = "ABDOMINAL MILITAR",
                            Prescrition = "Prescrição: 10 repetições",
                            Observations = new List<string>
                            {
                                "* Mantenha a velocidade constante.",
                                "NÃO FAÇA PAUSA. Inicie imediatamente o próximo exercício"
                            }
                        },
                        new WorkoutModel
                        {
                            Name = "AVANÇO PLIOMÉTRICO",
                            Prescrition = "1 série de 10 repetições total de 5 em cada perna.",
                            Observations = new List<string>
                            {
                                "* Realizar todos os movimentos com as pernas alternadamente e, ao trocar, deve dar um pequeno salto.",
                                "* NÃO FAÇA PAUSA. Inicie imediatamente o exercício abaixo"
                            }
                        },
                        new WorkoutModel
                        {
                            Name = "BURPEE",
                            Prescrition = "Prescrição: 5 repetições",
                            Observations = new List<string>
                            {
                                "* Mantenha a velocidade constante.",
                                "* Faça pausa de 40 segundos. Repita a sequência de exercícios 4x."
                            }
                        },
                        
                    }
                });
            return true;
        }

       

        async Task TryDownloadPDF()
        {

        }

        public async Task<bool> CheckIfResetIsRequested()
        {
            var user = Preferences.Get("User", null);
            if (user != null)
            {
                var credential = JsonExtensions.JsonToObject<UserModel>(user);
                var reseted = (await client.Child("Users").OnceAsync<UserModel>()).Where(w => w.Object.Username == credential.Username)
                    .FirstOrDefault().Object.Reset;
                return reseted;
            }
            return true;
        }

        public async Task<UserModel> LoginUser(string Username, string Password)
        {
            try
            {
                var DeviceModel = DeviceHelper.GetDeviceModel();
                if (Connectivity.NetworkAccess == NetworkAccess.Internet && CheckConnectionHelper.CheckInternet())
                {
                    var userLoged = (await client.Child("Users").OnceAsync<UserModel>()).Where(w => w.Object.Username == Username)
                        .FirstOrDefault()?.Object?.DeviceModel;

                    var alreadyLogged = userLoged != "zero" && userLoged != DeviceModel;

                    if (!alreadyLogged)
                    {
                        var userFirebase = (await client.Child("Users")
                            .OnceAsync<UserModel>())
                            .Where(u => u.Object.Username == Username)
                            .Where(u => u.Object.Password == Password)
                            .FirstOrDefault();
                        if (userFirebase != null)
                            if (userFirebase.Object != null)
                            {
                                await client.Child("Users").Child(userFirebase.Key).PutAsync(new UserModel { Username = Username, DeviceModel = DeviceModel, Password = Password, Reset = false });
                                Preferences.Set("User", JsonExtensions.ToJsonString(userFirebase.Object));
                                return userFirebase.Object;
                            }
                        return new UserModel { Username = "LUTERO" };
                    }
                    return new UserModel { Username = "CAMPANHAFRATERNIDADE" };
                }
                var e = new Exception("Deu ruim");
                Crashes.TrackError(e);
                return new UserModel { Username = "CALVINO" };
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return new UserModel { Username = "FEMINISTA" };
            }
        }
    }

    public class UserModel
    {

        public string Password { get; set; }
        public string Username { get; set; }
        public bool Reset { get; set; }
        public string DeviceModel { get; set; }
    }

    public static class DeviceHelper
    {
        public static string GetDeviceModel()
        {
            try
            {
                var device = DeviceInfo.Model;
                var manufacturer = DeviceInfo.Manufacturer;
                var deviceName = DeviceInfo.Name;

                return $"{device}{manufacturer}{deviceName}";
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return "LentForMen";
            }

        }
    }

    public static class CheckConnectionHelper
    {
        public static bool CheckInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("https://google.com/generate_204"))
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
