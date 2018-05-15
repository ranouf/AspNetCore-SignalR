import { Component } from '@angular/core';
import { ChatHubService } from './services/hub/chat-hub.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ChatHubService]
})
export class AppComponent {
  title = 'app';
  public name: string;
  

  constructor(
    private chatHubService: ChatHubService
  ) {
  }

  join() {
    this.chatHubService.join(this.name);
  }
}
