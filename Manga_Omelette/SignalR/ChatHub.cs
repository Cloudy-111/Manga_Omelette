using Manga_Omelette.Models;
using Manga_Omelette.Services;
using Microsoft.AspNetCore.SignalR;

namespace Manga_Omelette.SignalR
{
	public class ChatHub : Hub
	{
		private readonly StoryService _storyService;

		public ChatHub(StoryService storyService)
		{
			_storyService = storyService;
		}
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
		public async Task SendNotification(Notification notification)
		{
			await Clients.All.SendAsync("ReceiveNotification", notification);
		}
		public async Task SendNotificationForFollow(Notification notification, int storyId)
		{
			//await Clients.User("27d99b36-766f-49bb-a35b-9b1a0ddfc060").SendAsync("ReceiveNotification", notification);
			await Clients.Group(storyId.ToString()).SendAsync("ReceiveNotification", notification);
		}
		//This method just add user who add Story (new Add) to Group, not add user who add that Story before(exist in db)
		public async Task AddToGroupFollow(int storyId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, storyId.ToString());
        }
		public async Task RemoveFromGroupFollow(int storyId)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, storyId.ToString());
		}
		//Add user to Group Follow When Open Website
        public override async Task OnConnectedAsync()
        {
			var userId = Context.UserIdentifier;
			if(userId != null)
			{
				var followStories = _storyService.GetStoriesFollow(userId);
				foreach(var story in followStories)
				{
					await Groups.AddToGroupAsync(Context.ConnectionId, story.Id.ToString());
				}
			}

            await base.OnConnectedAsync();
        }
		//Remove User from Group When close website
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                var followStories = _storyService.GetStoriesFollow(userId);
                foreach (var story in followStories)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, story.Id.ToString());
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
