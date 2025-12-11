using DinuAlexandraLab7.Models;
using System.Collections.ObjectModel;

namespace DinuAlexandraLab7;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var items = await App.Database.GetShopsAsync();
        ShopPicker.ItemsSource = (System.Collections.IList)items;
        ShopPicker.ItemDisplayBinding = new Binding("ShopDetails");

        var shopl = (ShopList)BindingContext;
        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);

        if (shopl.ShopID != 0)
        {
            var currentShop = items.FirstOrDefault(s => s.ID == shopl.ShopID);

            if (currentShop != null)
            {
                ShopPicker.SelectedItem = currentShop;
            }
        }
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;

        Shop selectedShop = (ShopPicker.SelectedItem as Shop);
        if (selectedShop != null)
        {
            slist.ShopID = selectedShop.ID;
        }

        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }

    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext)
        {
            BindingContext = new Product()
        });
    }
}
