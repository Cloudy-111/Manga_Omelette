# MANGA OMELETTE
## Description
MANGA OMELETTE is a web application that allows users to read manga, interact with other readers, and receive real-time notifications. Admins can manage manga content, chapters, and notifications. Super Admins have control over user roles.

## Current Features:

### User Features:

- User Login and Signup
- Search and read manga
- Post comments on manga chapters
- Receive notifications from Admin or System (real-time with SignalR)

### Admin Features:

- Create and manage manga
- Update manga chapters
- Create and send notifications to users (real-time with SignalR)
### Super Admin Features:

- Create roles
- Assign or revoke roles for each user

## Usage
After successfully setting up and running the project, follow these steps:

### Users:

- Create an account by signing up or log in using existing credentials.
- Browse the manga collection, search for specific titles, and start reading.
- You can also post comments on manga chapters, save manga on your favorite, evaluate manga from 1 - 10.
- Notifications about new chapters or system updates will be sent to you in real-time via SignalR.
### Admins:

- Log in with admin credentials to access the admin panel.
- Create new manga, upload chapters, and manage the manga database.
- Send notifications to users about new manga chapters or important updates.
### Super Admins:

- Log in with super admin credentials to manage user roles.
- You can create roles and assign them to users (such as admin, user, or other custom roles).
## Technologies Used
### Frontend:

- HTML
- CSS
- JavaScript
### Backend:

- ASP.NET
### Database:

- SQL Server
- MongoDB
### Cloud:

- Cloudinary (for managing images like manga covers, chapter images)
### Real-time Communication:

- SignalR (for real-time notifications and updates)
