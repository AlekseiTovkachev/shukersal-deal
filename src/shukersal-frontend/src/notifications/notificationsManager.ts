import { HubConnectionBuilder, HubConnection } from "@microsoft/signalr";
import { CHAT_URL } from "../_configuration";

let connection: HubConnection | null = null;

export const NTStart = async (
  memberId: number,
  onData: (data: string) => void
) => {
  connection = new HubConnectionBuilder()
    .withUrl(`${CHAT_URL}?userId=${memberId}`)
    .build();
  connection.on("receiveNotification", (data) => {
    console.log("[Notification] ", data);
    onData(data);
  });
  return await connection
    .start()
    .then(() => {
      console.log("Connected to SignalR hub.");
    })
    .catch((error) => {
      console.error("Error connecting to SignalR hub:", error);
    });
};

export const NTStop = async () => {
  await connection?.stop();
  connection = null;
};
