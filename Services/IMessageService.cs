namespace NetworkScannerXTech.Services;
public interface IMessageService
{
    public bool ShowConfirmation(string message, string title);
    public void ShowError(string message);
}
