import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class Base64Service {

  private hubConnection?: HubConnection;

  private encodedDataSubject = new Subject<any>;
  private partialDataSubject = new Subject<any>;
  private busySubject = new Subject<boolean>;

  private _connectionId: any;

  constructor() { }

  startConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/base64', {
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
    this.hubConnection?.on("conversionStarted", (id) => {
      this.busySubject.next(true);
      this._connectionId = id;
    });
    this.hubConnection?.on("conversionUpdate", (i, c) => this.partialDataSubject.next(c));
    this.hubConnection?.on("conversionCompleted", (data) => {
      this.encodedDataSubject.next(data);
      this.busySubject.next(false);
    });

    this.hubConnection?.on("conversionCancelled", () => this.busySubject.next(false))
  }

  convertText(text: string) {
    this.hubConnection?.invoke("convert", text)
      .catch(err => console.error(err));
  }

  cancelConversion() {
    this.hubConnection?.invoke("cancel", this._connectionId)
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
