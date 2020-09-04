import { Component } from '@angular/core';
import { ChatService } from '../Services/chat.service';
import { environment } from 'src/environments/environment';
import { ChatMessageService } from '../Services/chat-message.service';

@Component({
    selector: "chat-panel",
    styleUrls: ['./chat-panel.component.css'],
    templateUrl: './chat-panel.component.html'
})

export class ChatPanelComponent {
    public apiUrl: string = environment.apiUrl;
    constructor(public chatService: ChatService,
                public CMService: ChatMessageService){

    }
}