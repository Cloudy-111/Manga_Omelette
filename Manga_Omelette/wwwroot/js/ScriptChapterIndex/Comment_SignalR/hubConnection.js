//Create Connection Server-Client
export const connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

//Start  Connection
export function startConnection() {
	connection.start().catch(function (err) {
		return console.error(err.toString());
	});
}