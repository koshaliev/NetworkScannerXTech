using System.Windows;

namespace NetworkScannerXTech.Services;
public class MessageService : IMessageService
{
    public bool ShowConfirmation(string message, string title)
    {
        var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
        return result == MessageBoxResult.Yes;
    }

    public void ShowError(string message)
    {
        MessageBox.Show(message, "Ошибка при выполнении", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
    }
}
