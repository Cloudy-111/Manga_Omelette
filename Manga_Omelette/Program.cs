using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Manga_Omelette.Data;
using Manga_Omelette.Areas.Identity.Data;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DotNetEnv;
using Manga_Omelette.Services;
using Manga_Omelette.SignalR;
using Manga_Omelette.Models_Secondary;
using Manga_Omelette.MongoDB;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Manga_OmeletteDBContextConnection") ?? throw new InvalidOperationException("Connection string 'Manga_OmeletteDBContextConnection' not found.");

builder.Services.AddDbContext<Manga_OmeletteDBContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<Manga_OmeletteDBContext>();

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddSingleton<CloudinaryService>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddSignalR(); //Real-time function
builder.Services.AddScoped<ChapterService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<StoryService>();
builder.Services.AddScoped<CloudinaryService>();
builder.Services.AddScoped<FavoriteService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<AuthorService>();

builder.Services.AddSignalR();

//Take information from appsetting.json
builder.Services.Configure<MongoDBSetting>(builder.Configuration.GetSection("MongoDB"));

//Configuration MongoDB Client
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var setting = sp.GetRequiredService<IOptions<MongoDBSetting>>().Value;
    return new MongoClient(setting.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var setting = sp.GetRequiredService<IOptions<MongoDBSetting>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(setting.DatabaseName);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Advance Search View",
        pattern: "/titles",
        defaults: new { controller = "Story", action = "SearchView"}
        );
    endpoints.MapControllerRoute(
        name: "Follow Page",
        pattern: "/follow",
        defaults: new { controller = "Story", action = "FollowList" }
        );
    endpoints.MapControllerRoute(
        name: "Latest Update",
        pattern: "/latest-update",
        defaults: new { controller = "Story", action = "Latest_Update" }
        );
    endpoints.MapControllerRoute(
        name: "Top Read",
        pattern: "/top-read",
        defaults: new { controller = "Story", action = "Top_Read" }
        );
    endpoints.MapControllerRoute(
        name: "Top Like",
        pattern: "/top-like",
        defaults: new { controller = "Story", action = "Top_Like" }
        );
    endpoints.MapControllerRoute(
        name: "For You",
        pattern: "/for-you",
        defaults: new { controller = "Story", action = "For_You" }
        );
    endpoints.MapControllerRoute(
        name: "Random",
        pattern: "/random",
        defaults: new { controller = "Story", action = "Random" }
        );
    endpoints.MapControllerRoute(
        name: "Details Story",
        pattern: "titles/{id}/{titles?}",
        defaults: new { controller = "Story", action = "Details_Story" }
        );
    endpoints.MapControllerRoute(
        name: "Setting",
        pattern: "/setting",
        defaults: new { controller = "Home", action = "Setting" }
        );
    endpoints.MapControllerRoute(
        name: "Details Chapter",
        pattern: "chapter/{id}",
        defaults: new { controller = "Chapter", action = "Index" }
        );

    //Config get Comment for each chapter
	endpoints.MapControllerRoute(
	    name: "Get Comments",
	    pattern: "chapter/{chapterId}/comments",
	    defaults: new { controller = "Chapter", action = "GetComments" }
    );
	endpoints.MapControllerRoute(
		name: "Get Comment Replies",
		pattern: "chapter/{chapterId}/comment_replies",
		defaults: new { controller = "Chapter", action = "GetRepliesComment" }
	);
	endpoints.MapControllerRoute(
        name: "Super Admin Dashboard",
        pattern: "/superadmin/dashboard",
        defaults: new { controller = "Administration", action = "DashboardSuperAdmin" }
        );
    endpoints.MapControllerRoute(
        name: "Manage User",
        pattern: "/superadmin/manage_user",
        defaults: new { controller = "Administration", action = "ManageUser" }
        );
    endpoints.MapControllerRoute(
        name: "Manage Story",
        pattern: "/superadmin/manage_story",
        defaults: new { controller = "Administration", action = "ManageStory" }
        );
    endpoints.MapControllerRoute(
        name: "Manage Notification",
        pattern: "/superadmin/manage_notification",
        defaults: new { controller = "Administration", action = "ManageNotification" }
        );
    endpoints.MapControllerRoute(
        name: "Manage System",
        pattern: "/superadmin/manage_system",
        defaults: new { controller = "Administration", action = "ManageSystem" }
        );
    endpoints.MapControllerRoute(
        name: "Upload Cloudinary",
        pattern: "/superadmin/upload_cloudinary",
        defaults: new { controller = "Administration", action = "UploadImagetoCloudinary" }
        );
    endpoints.MapControllerRoute(
        name: "Create Chapter",
        pattern: "admin/{storyId}/create_chapter",
        defaults: new { controller = "Chapter", action = "CreateChapter" }
        );
	endpoints.MapControllerRoute(
		name: "Edit Chapter",
		pattern: "admin/chapter/{chapterId}/edit_chapter",
		defaults: new { controller = "Chapter", action = "EditChapter" }
		);
	//Configure Endpoint For ChatHub
	endpoints.MapHub<ChatHub>("/chat");
	endpoints.MapControllerRoute(
		name: "Delete Comment",
		pattern: "/deleteComment",
		defaults: new { controller = "Comment", action = "DeleteComment" }
		);
	endpoints.MapControllerRoute(
		name: "Post Comment",
		pattern: "/postComment",
		defaults: new { controller = "Comment", action = "PostComment" }
		);
	endpoints.MapControllerRoute(
		name: "Get UserNameDisplay",
		pattern: "/getUserName",
		defaults: new { controller = "Comment", action = "GetUserName" }
		);
	endpoints.MapControllerRoute(
		name: "Rate Story",
		pattern: "/rate_story",
		defaults: new { controller = "Story", action = "RateStory" }
		);
	endpoints.MapControllerRoute(
		name: "Edit Rate Story",
		pattern: "/edit_rate_story",
		defaults: new { controller = "Story", action = "EditRateStory" }
		);
	endpoints.MapControllerRoute(
		name: "Delete Rate Story",
		pattern: "/api/delete_rate_story/{userId}/{storyId}",
		defaults: new { controller = "Story", action = "DeleteRateStory" }
		);
    endpoints.MapControllerRoute(
        name: "Add Story To Follow List",
        pattern: "/addToFollowList",
        defaults: new { controller = "Story", action = "AddToFollowList" }
        );
    endpoints.MapControllerRoute(
        name: "Remove Story From Follow List",
        pattern: "/removeFromFollowList",
        defaults: new { controller = "Story", action = "RemoveFromList" }
        );
	endpoints.MapControllerRoute(
		name: "Share Story",
		pattern: "/share/{storyId}",
		defaults: new { controller = "Story", action = "IncreaseShares" }
		);
    endpoints.MapControllerRoute(
        name: "Remove Update Chapter",
        pattern: "/removeupdatenewchapter/{storyId}/{userId}",
        defaults: new { controller = "Favorite", action = "RemoveUpdateNewChapter" }
        );
    endpoints.MapControllerRoute(
        name: "Create Notification",
        pattern: "/create_notification",
        defaults: new { controller = "Notifications", action = "Create" }
        );
    endpoints.MapControllerRoute(
        name: "List Notification of Type",
        pattern: "/notifications/{typeId}",
        defaults: new { controller = "TypeNotis", action = "NotificationInType" }
        );
    endpoints.MapControllerRoute(
        name: "Get Type Of Notification",
        pattern: "/getTypeNotification/{notificationId}",
        defaults: new { controller = "Notifications", action = "GetNotificationType" }
        );
    endpoints.MapControllerRoute(
        name: "Get Notifications In Header",
        pattern: "/getNotificationHeader",
        defaults: new { controller = "Notifications", action = "GetNotificationsInIcon" }
		);
    //CLEAR ALL NOTIFICATIONS --- DANGER
    endpoints.MapControllerRoute(
        name: "Clear Notificaiton",
        pattern: "/clearNotificaiton",
        defaults: new { controller = "Notifications", action = "DeleteAllNotification" }
        );
    endpoints.MapControllerRoute(
        name: "Mark As Read Single Notification",
        pattern: "/markAsReadSingleNotification",
        defaults: new { controller = "Notifications", action = "MarkAsReadSignleNotification" }
        );
    endpoints.MapControllerRoute(
        name: "Mark As Read All Notification",
        pattern: "/markAllAsRead",
        defaults: new { controller = "Notifications", action = "MarkAsReadAllNotification" }
        );
    endpoints.MapControllerRoute(
        name: "Live Search Ajax",
        pattern: "/Home/Search",
        defaults: new { controller = "Home", action = "Search" }
        );
    endpoints.MapControllerRoute(
        name: "Filter Story",
        pattern: "/filter",
        defaults: new { controller = "Story", action = "FilterStory" }
        );
});
#pragma warning restore ASP0014 // Suggest using top level route registrations

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
