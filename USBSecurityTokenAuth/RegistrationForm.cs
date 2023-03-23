using USBSecurityTokenAuth.Models;

namespace USBSecurityTokenAuth
{
    public partial class RegistrationForm : Form
    {
        private readonly ApiClient _apiClient;
        private readonly List<DriveData> _driveDataList;

        public RegistrationForm(ApiClient apiClient)
        {
            _driveDataList = Utils.GetPossibleDrivesData();
            _apiClient = apiClient;
            InitializeComponent();
        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {
            drivesComboBox.Items.AddRange(_driveDataList.Select(item => item.ToString()).ToArray());
        }

        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            var login = loginTextBox.Text;
            var loginAndPassword = new LoginAndPassword { Login = login, Password = passTextBox.Text };

            var selectedDriveIndex = drivesComboBox.SelectedIndex;

            var hash = Utils.CreateHash(loginAndPassword, _driveDataList[selectedDriveIndex].SerialNumber);
            var newUser = new NewUser
            {
                Login = login,
                Name = nameTextBox.Text,
                Surname = surnameTextBox.Text,
                Hash = hash
            };
            try
            {
                var result = await _apiClient.SendRegisterRequest(newUser);
                MessageBox.Show(result, "Успіх!", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Не вдалося зареєтрувати користувача!", "Помилка!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            try
            {
                await File.WriteAllTextAsync(_driveDataList[selectedDriveIndex].Letter + "\\.login-credentials.txt",
                    login + "\n" + loginAndPassword.Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не вдалося запистати дані входу на носій-ключ!", "Помилка!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}