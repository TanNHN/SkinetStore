import { Injectable, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { environment } from '../../../environments/environment.development';
import { Order } from '../../shared/models/order';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  hubUrl = environment.hubUrl;
  hubConnection?: HubConnection;
  order = signal<Order | null>(null);

  createHubConnection() {
    this.hubConnection = new HubConnectionBuilder().withUrl(this.hubUrl, {
      withCredentials: true
    }).withAutomaticReconnect().build();

    this.hubConnection.start().catch(
      error => console.error(error)
    );

    this.hubConnection.on('OrderCompleteNotification', (orderNotifyData: Order) => {
      this.order.set(orderNotifyData);
    });
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch(error => console.log(error));
    }
  }
}
