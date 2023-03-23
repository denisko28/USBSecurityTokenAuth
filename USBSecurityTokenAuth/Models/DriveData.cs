namespace USBSecurityTokenAuth.Models;

public class DriveData
{
    public string Letter { get; set; }
    public string SerialNumber { get; set; }

    public override string ToString()
    {
        return "Drive letter: " + Letter + ", Serial number: " + SerialNumber;
    }
}