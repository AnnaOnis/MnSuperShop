using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyShop.HttpApiCient.Models;

namespace MyShopFrontend.Pages
{
    public partial class RegistrationPage
    {
        [Inject]
        private ISnackbar Snackbar { get; set; } = null!;
        [Inject]
        private IMyShopClient MyShopClient { get; set; } = null!;
        private CancellationTokenSource _cts = new();
        RegisterAccountForm model = new RegisterAccountForm();
        bool success;

        private async Task OnValidSubmit()
        {
            await MyShopClient.RegistrateAccountAsync(new Account() 
            { 
                Id = new Guid(), 
                Name = model.Username, 
                Email = model.Email, 
                Password = model.Password
            }, _cts.Token);

            Snackbar.Configuration.ShowCloseIcon = true;
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
            Snackbar.Add("Вы успешно зарегистрированы", Severity.Success);

            model.Username = "";
            model.Email = "";
            model.Password = "";
            model.Password2 = "";

            success = true;
            StateHasChanged();
        }
    }
}
