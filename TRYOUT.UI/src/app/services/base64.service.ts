import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';
import { Subject } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class Base64Service {

  private hubConnection?: HubConnection;

  private encodedDataSubject = new Subject<any>;
  private partialDataSubject = new Subject<any>;
  private busySubject = new Subject<boolean>;

  constructor() { }

  startConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5001/encode', {
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
    this.hubConnection?.on("conversionStarted", (connectionId) => {
      this.busySubject.next(true);
      console.log("Process started", connectionId);
    });

    this.hubConnection?.on("conversionUpdate", (c) => this.partialDataSubject.next(c));
    
    this.hubConnection?.on("conversionCancelled", (connectionId) => {
      this.busySubject.next(false);
      console.log("Process cancelled", connectionId);
    });

    this.hubConnection?.on("conversionCompleted", (connectionId) => {
      this.busySubject.next(false);
      console.log("Process completed", connectionId);
    });
  }

  convertText(text: string) {
    this.hubConnection?.invoke("processConversion", text)
      .catch(err => console.error("convertText:ERROR:", err));
  }

  cancelConversion() {
    this.hubConnection?.invoke("cancelConversion")
      .catch(err => console.error(err));
  }

  getEncodedData() {
    return this.encodedDataSubject.asObservable();
  }

  getBusyData() {
    return this.busySubject.asObservable();
  }

  getPartialData() {
    return this.partialDataSubject.asObservable();
  }
}
