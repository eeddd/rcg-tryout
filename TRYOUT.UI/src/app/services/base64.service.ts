import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';


@Injectable({
  providedIn: 'root'
})
export class Base64Service {

  private hubConnection?: HubConnection;

  constructor() { }

  startConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:32778/base64', {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .build();

      this.hubConnection
        .start()
        .then(() => console.log("Connection Started"))
        .catch(err => console.error(err))
  }

  addListeners() {
    this.hubConnection?.on("conversionCompleted", (data) => console.log("encoded:",data));
  }

  convertText(text: string) {
    this.hubConnection?.invoke("convert", text)
      .catch(err => console.error(err));
  }
}
