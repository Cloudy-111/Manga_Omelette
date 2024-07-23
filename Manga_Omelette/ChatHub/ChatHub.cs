using Microsoft.AspNetCore.SignalR;

namespace Manga_Omelette.ChatHub
{
	public class ChatHub : Hub
	{
		public async Task SendComment(string user, string content, string userId, int commentId)
		{
			await Clients.All.SendAsync("ReceiveComment", user, content, userId, commentId);
		}
		public async Task DeleteComment(int commentId)
		{
			await Clients.All.SendAsync("ReceiveDeletedComment", commentId);
		}
		public async Task SendReplyComment(string user, string content, string userId, int commentId, int parentId)
		{
			await Clients.All.SendAsync("ReceiveReplyComment", user, content, userId, commentId, parentId);
		}
	}
}
