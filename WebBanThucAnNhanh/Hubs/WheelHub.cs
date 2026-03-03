using Microsoft.AspNetCore.SignalR;

namespace WebBanThucAnNhanh.Hubs
{
    /// <summary>
    /// SignalR Hub cho thông báo real-time khi có người trúng thưởng.
    /// Tất cả user đang online sẽ nhận được thông báo.
    /// </summary>
    public class WheelHub : Hub
    {
        // Client sẽ lắng nghe sự kiện "ReceivePrizeNotification"
        // Server gọi: Clients.All.SendAsync("ReceivePrizeNotification", userName, prizeName)
    }
}
