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

  private abortController?: AbortController;


  constructor() { }

  startConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7113/base64', {
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
    this.hubConnection?.on("conversionUpdate", (c) => this.partialDataSubject.next(c));
    this.hubConnection?.on("conversionCompleted", (encoded) => {
      this.encodedDataSubject.next(encoded);
      this.busySubject.next(false);
    });
  }

  convertText(text: string) {
    this.abortController = new AbortController();

    this.abortController?.signal.addEventListener('abort', (e) => {
      this.busySubject.next(false);
      console.log('aborted');
    });

    this.busySubject.next(true);

    this.hubConnection?.invoke("convertToBase64", text, this.abortController?.signal)
      .catch(err => console.error("convertText:ERROR:", err));
  }

  cancelConversion() {
    this.abortController?.abort();
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
