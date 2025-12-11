using DinuAlexandraLab7.Models;
using Plugin.LocalNotification;
using Microsoft.Maui.Devices.Sensors;

namespace DinuAlexandraLab7;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }

    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Adress;

        try
        {
            var locationsEnumerable = await Geocoding.GetLocationsAsync(address);
            var locations = locationsEnumerable?.ToList();

            var shoplocation = locations?.FirstOrDefault();

            if (shoplocation != null)
            {
                var options = new MapLaunchOptions { Name = "Magazinul meu preferat" };

                var myLocation = await Geolocation.GetLocationAsync();
                if (myLocation != null)
                {
                    var distance = myLocation.CalculateDistance(shoplocation, DistanceUnits.Kilometers);
                    if (distance < 5)
                    {
                        var request = new NotificationRequest
                        {
                            NotificationId = 100,
                            Title = "Ai de facut cumparaturi in apropiere!",
                            Description = address,
                            Schedule = new NotificationRequestSchedule
                            {
                                NotifyTime = DateTime.Now.AddSeconds(1)
                            }
                        };
                        await LocalNotificationCenter.Current.Show(request);
                    }
                }
                await Map.OpenAsync(shoplocation, options);
            }
            else
            {
                await DisplayAlert("Eroare", "Nu s-a putut găsi locația pentru această adresă.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"A apărut o problemă la hartă: {ex.Message}", "OK");
        }
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;

        if (shop.ID != 0)
        {
            bool answer = await DisplayAlert("Confirmare", "Sigur dorești să ștergi acest magazin?", "Da", "Nu");

            if (answer)
            {
                await App.Database.DeleteShopAsync(shop);
                await Navigation.PopAsync();
            }
        }
        else
        {
            await Navigation.PopAsync();
        }
    }
}