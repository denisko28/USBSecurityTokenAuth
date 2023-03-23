using Microsoft.Extensions.Configuration;
using USBSecurityTokenAuth.Models;

namespace USBSecurityTokenAuth;

public partial class Form1 : Form
{
    private readonly ApiClient _apiClient;

    public Form1(IConfiguration configuration)
    {
        _apiClient = new ApiClient(configuration);
        InitializeComponent();
    }

    private async void SignInClick(object sender, EventArgs e)
    {
        var driveData = Utils.IdentifyDrive();
        if (driveData == null)
        {
            MessageBox.Show("Виникла помилка ідентифікації носія даних для входу!", "Помилка даних для входу",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        string credentialsText;
        try
        {
            credentialsText = File.ReadAllText(driveData.Letter + "\\.login-credentials.txt");
        }
        catch (Exception)
        {
            MessageBox.Show("Помилка зчитування файлу даних для входу!", "Помилка даних для входу",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        string hashedCredentials;
        LoginAndPassword loginAndPassword;
        try
        {
            loginAndPassword = LoginAndPassword.ExtractFromText(credentialsText);
            hashedCredentials = Utils.CreateHash(loginAndPassword, driveData.SerialNumber);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Помилка даних для входу", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            var result = await _apiClient.SendSignInRequest(new SignInRequest
                { Login = loginAndPassword.Login, Hash = hashedCredentials });
            MessageBox.Show(result, "Успіх!!", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception)
        {
            MessageBox.Show("В доступі відмовлено!", "В доступі відмовлено!", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void RegisterButton_Click(object sender, EventArgs e)
    {
        RegistrationForm registrationForm = new RegistrationForm(_apiClient);
        // registrationForm.ApiClient = _apiClient;
        registrationForm.Show();
    }
}