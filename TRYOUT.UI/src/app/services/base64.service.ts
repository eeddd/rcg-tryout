import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType, AbortSignal } from '@microsoft/signalr';
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
  
  private connectionId: string = '';

  constructor() { }

  startConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:5001/base64', {
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
      this.connectionId = id;
      console.log("connectionId:", this.connectionId);
    });

    this.hubConnection?.on("conversionUpdate", (c) => this.partialDataSubject.next(c));
    
    this.hubConnection?.on("conversionCancelled", (id) => {
      console.log("Cancelled:", id);
      this.busySubject.next(false);
    });

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

    this.hubConnection?.invoke("processConversion", text)
      .catch(err => console.error("convertText:ERROR:", err));
  }

  cancelConversion() {
    // this.abortController?.abort();
    console.log('clicked Cancel')
    this.hubConnection?.invoke("cancelConversion", this.connectionId)
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
