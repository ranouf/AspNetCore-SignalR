import { Injectable, EventEmitter } from '@angular/core';
import * as signalR from '@aspnet/signalr';

import { environment } from '../../../environments/environment';

import { IMessageDto, MessageDto, IPrivateMessageDto, PrivateMessageDto, IUserDto, UserDto, IUsersDto, UsersDto } from './../../common/types';

import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/distinctUntilChanged';

@Injectable()
export class ChatHubService {
  private _hubConnection: signalR.HubConnection;
  private _message: Subject<IMessageDto> = new Subject<IMessageDto>();
  private _privateMessage: Subject<IPrivateMessageDto> = new Subject<IPrivateMessageDto>();
  private _userJoined: Subject<IUsersDto> = new Subject<IUsersDto>();
  private _userLeft: Subject<IUsersDto> = new Subject<IUsersDto>();
  private _isConnected: Subject<boolean> = new Subject<boolean>();

  constructor(
  ) {
    this.connectToSignalServer();
    this.suscribeToMessageSentToAll();
    this.suscribeToPrivateMessageSent();
    this.suscribeToUserJoined();
    this.suscribeToUserLeft();
  }

  private connectToSignalServer(): void {

    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.apiUrl + "/chat")
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this._hubConnection.start().catch(err => console.error(err.toString()));

    //this._hubConnection.start()
    //  .then(() => {
    //    this._isConnected.next(true);
    //    console.log('[SignalR] Connection to /chat established.');
    //  })
    //  .catch(err => {
    //    this._isConnected.next(false);
    //    console.error('[SignalR] Error while establishing connection to /chat.');

    //    //console.log('[SignalR] Connection to /tiles will retry in 1 minute.');
    //    //this._hubConnection.stop();
    //    //let timer = Observable.timer(60000, 1);
    //    //timer.subscribe(tick => {
    //    //  this.connectToSignalServer();
    //    //});
    //  });

    //this._hubConnection.()
    //  .then(() => {
    //    console.log('[SignalR] Connection to /tiles will retry in 1 minute.');
    //    let timer = Observable.timer(60000, 1);
    //    timer.subscribe(tick => {
    //      this.connectToSignalServer();
    //    });
    //  })
    //  .catch(err => {
    //    console.error('[SignalR] Retry Connection to /tiles failed.');
    //  });
  }

  public join(name:string): void {
    this._hubConnection.invoke('Join', name);
  }

  public leave(name: string): void {
    this._hubConnection.invoke('Leave', name);
  }

  public sendMessageToAll(content: string): void {
    this._hubConnection.invoke('SendMessageToAll', name);
  }

  public sendPrivateMessage(content: string, id: string): void {
    this._hubConnection.invoke('SendPrivateMessage', name, id);
  }

  private suscribeToMessageSentToAll(): void {
    this._hubConnection.on('MessageSentToAll', (data: any) => {
      var message = MessageDto.fromJS(data);
      this._message.next(message);
      console.log("[SignalR] MessageSentToAll => '" + message.content + "', Author='" + message.author.name + "'.");
    });
  }

  private suscribeToPrivateMessageSent(): void {
    this._hubConnection.on('PrivateMessageSent', (data: any) => {
      var message = MessageDto.fromJS(data);
      this._privateMessage.next(message);
      console.log("[SignalR] PrivateMessageSent => '" + message.content + "', Author='" + message.author.name + "'.");
    });
  }

  private suscribeToUserJoined(): void {
    this._hubConnection.on('UserJoined', (data: any) => {
      var users = UsersDto.fromJS(data);
      this._userJoined.next(users);
      console.log("[SignalR] UserJoined => '" + users.user.name + "', Count='" + users.users.length + "'.");
    });
  }

  private suscribeToUserLeft(): void {
    this._hubConnection.on('UserLeft', (data: any) => {
      var users = UsersDto.fromJS(data);
      this._userLeft.next(users);
      console.log("[SignalR] UserLeft => '" + users.user.name + "', Count='" + users.users.length + "'.");
    });
  }

  public onNewMessage(): Observable<IMessageDto> {
    return this._message.distinctUntilChanged();
  }

  public onNewPrivateMessage(): Observable<IMessageDto> {
    return this._privateMessage.distinctUntilChanged();
  }

  public onUserJoin(): Observable<IUsersDto> {
    return this._userJoined.distinctUntilChanged();
  }

  public onUserLeave(): Observable<IUsersDto> {
    return this._userLeft.distinctUntilChanged();
  }

  public onConnect(): Observable<boolean> {
    return this._isConnected.distinctUntilChanged();
  }
}
