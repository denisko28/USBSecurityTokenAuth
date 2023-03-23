using System.Management;
using System.Security.Cryptography;
using System.Text;
using USBSecurityTokenAuth.Models;

namespace USBSecurityTokenAuth;

public static class Utils
{
    private static ManagementObjectCollection GetPossibleDrives()
    {
        var ms = new ManagementObjectSearcher(
            "Select DriveLetter, serialnumber from Win32_Volume Where Label='MYUSBKEY'");
        return ms.Get();
    }

    public static List<DriveData> GetPossibleDrivesData()
    {
        var drivesList = GetPossibleDrives();
        var drivesDataList = new List<DriveData>();
        foreach (var mo in drivesList)
        {
            drivesDataList.Add(new DriveData
                { Letter = mo["DriveLetter"].ToString()!, SerialNumber = mo["serialnumber"].ToString()! });
        }

        return drivesDataList;
    }

    public static DriveData? IdentifyDrive()
    {
        DriveData? driveData = null;

        var drivesList = GetPossibleDrives();
        foreach (var mo in drivesList)
        {
            var driveLetter = mo["DriveLetter"].ToString();
            var driveSerial = mo["serialnumber"].ToString();
            if (driveLetter == null || driveSerial == null)
                driveData = null;
            else
                driveData = new DriveData { Letter = driveLetter, SerialNumber = driveSerial };
            break;
        }

        return driveData;
    }

    public static string CreateHash(LoginAndPassword loginAndPassword, string driveSerial)
    {
        using var sha256 = SHA256.Create();

        var passwordBytes = Encoding.UTF8.GetBytes(loginAndPassword.Password);
        var loginBytes = Encoding.UTF8.GetBytes(loginAndPassword.Login);
        var serialBytes = Encoding.UTF8.GetBytes(driveSerial);

        var combinedBytes = new byte[passwordBytes.Length + loginBytes.Length + serialBytes.Length];
        Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
        Array.Copy(loginBytes, 0, combinedBytes, loginBytes.Length,
            loginBytes.Length);
        Array.Copy(serialBytes, 0, combinedBytes, serialBytes.Length,
            loginBytes.Length);

        return Convert.ToBase64String(sha256.ComputeHash(combinedBytes));
    }
}