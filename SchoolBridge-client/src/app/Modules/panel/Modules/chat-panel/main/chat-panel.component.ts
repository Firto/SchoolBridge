import { Component, ElementRef, ViewChild } from '@angular/core';
import { ChatService } from '../Services/chat.service';
import { ChatMessageService } from '../Services/chat-message.service';
import { Chat } from '../Classes/chat.class';
import { BehaviorSubject } from 'rxjs';

@Component({
    selector: "chat-panel",
    styleUrls: ['./chat-panel.component.css'],
    templateUrl: './chat-panel.component.html'
})

export class ChatPanelComponent {
    public curChat: BehaviorSubject<Chat> = new BehaviorSubject<Chat>(null);

    constructor(public chatService: ChatService,
                public CMService: ChatMessageService){

    }

    public onSelectChat(chat: Chat){
        this.curChat.next(chat); 
    }
}