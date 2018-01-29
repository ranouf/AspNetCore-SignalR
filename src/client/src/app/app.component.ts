import { Component, OnInit } from '@angular/core';
import { ValuesService } from './services/api.services';
import { Observable } from 'rxjs/Observable';
import { HubConnection } from '@aspnet/signalr-client';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ValuesService],
})
export class AppComponent implements OnInit {
  title = "AspNetCoreSignalR";
  apiUrl: string = environment.apiUrl;

  //Test API
  apiValues: string[] = [];

  //Test SignalR
  private _hubConnection: HubConnection;
  public async: any;
  message = '';
  messages: string[] = [];

  constructor(
    private valuesService: ValuesService
  ) {
    console.log(this.apiUrl);
  }

  public sendMessage(): void {
    const data = `Sent: ${this.message}`;

    this._hubConnection.invoke('Send', data);
    this.messages.push(data);
  }

  ngOnInit() {
    //API Initialization
    this.valuesService.getAll()
      .subscribe(result => {
        this.apiValues = result;
      }, error => {
        console.log("Error: " + error);
      });

    //SignalR Initialization
    this._hubConnection = new HubConnection(this.apiUrl + 'test');

    this._hubConnection.on('Send', (data: any) => {
      const received = `Received: ${data}`;
      this.messages.push(received);
    });

    this._hubConnection.start()
      .then(() => {
        console.log('Hub connection started')
      })
      .catch(err => {
        console.log('Error while establishing connection' + err)
      });
  }
}
