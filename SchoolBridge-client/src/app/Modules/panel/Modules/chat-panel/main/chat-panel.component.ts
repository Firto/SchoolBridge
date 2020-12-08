import { ChangeDetectionStrategy, Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { ChatService } from '../Services/chat.service';
import { ChatMessageService } from '../Services/chat-message.service';
import { Chat } from '../Classes/chat.class';
import { BehaviorSubject, merge, Observable } from 'rxjs';
import { NgxScrollEvent } from 'src/app/Modules/ngx-scroll/ngx-scroll.directive';
import { debounceTime, finalize, takeUntil, tap } from 'rxjs/operators';
//import { Globalization } from 'src/app/Modules/globalization/Decorators/backend-strings.decorator';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';
import { observed } from 'src/app/Decorators/observed.decorator';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { detectChanges, markDirty } from 'src/app/Helpers/mark-dirty.func';

@Component({
  selector: "chat-panel",
  styleUrls: ['./chat-panel.component.css', './chat-panel.component.scss'],
  templateUrl: './chat-panel.component.html',
})
export class ChatPanelComponent extends OnUnsubscribe implements OnInit {
  public curChat: Chat = null;
  @observed() public isLoading: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  @observed() public onChange: Observable<any>;
  public chats: Chat[] = [];

  constructor(public chatService: ChatService,
              public CMService: ChatMessageService) { super(); }

  public ngOnInit(): void {
    this.onChange = merge(
      this.chatService.chats.pipe(tap(x => this.chats = x))
    ).pipe(takeUntil(this._destroy), debounceTime(300));
    this.isLoading.next(true);
    this.chatService.getChats()
      .subscribe(() => this.isLoading.next(false))
  }

  public selectChat(chat: Chat){
    console.log(chat);
    this.curChat = chat;
    detectChanges(this);
  }
}
