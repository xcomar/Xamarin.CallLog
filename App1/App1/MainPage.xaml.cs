using App1.Interfaces;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        private const string APP_NAME = "App Log";
        public MainPage()
        {
            InitializeComponent();

            LoadCallLog();

            // Attach Events
            CallLogList.Refreshing += CallLogList_Refreshing;
        }

        public async void LoadCallLog()
        {
            activity_indicator.IsRunning = true;
            activity_indicator.IsVisible = true;

            try
            {
                var statusContact = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
                var statusPhone = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Phone);
                if (statusContact != PermissionStatus.Granted || statusPhone != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Contacts))
                    {
                        await DisplayAlert(APP_NAME, "Se requiere permisos para acceder a los contactos", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Contacts, Permission.Phone);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Contacts))
                        statusContact = results[Permission.Contacts];
                    if (results.ContainsKey(Permission.Phone))
                        statusPhone = results[Permission.Phone];
                }

                if (statusContact == PermissionStatus.Granted && statusPhone == PermissionStatus.Granted)
                {
                    CallLogList.IsRefreshing = true;

                    var Logg = DependencyService.Get<ICallLog>().GetCallLogs();

                    CallLogList.IsRefreshing = false;
                    CallLogList.ItemsSource = Logg;
                }
                else if (statusContact != PermissionStatus.Unknown || statusPhone == PermissionStatus.Unknown)
                {
                    await DisplayAlert(APP_NAME, "El permiso fue negado. No podemos continuar, intente nuevamente.", "OK");
                }
            }
            catch (Exception es)
            {
                activity_indicator.IsRunning = false;
                activity_indicator.IsVisible = false;

                await DisplayAlert("Call Log", "Ha ocurrido un problema, comuniquese con el soporte al cliente. Reporte Técnico: " + es.Message, "OK");
            }
            finally
            {
                activity_indicator.IsRunning = false;
                activity_indicator.IsVisible = false;
            }
        }

        private void CallLogList_Refreshing(object sender, EventArgs e)
        {
            LoadCallLog();
        }
    }
}
