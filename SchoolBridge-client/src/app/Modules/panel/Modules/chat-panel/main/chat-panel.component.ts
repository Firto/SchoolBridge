import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { ChatService } from '../Services/chat.service';
import { ChatMessageService } from '../Services/chat-message.service';
import { Chat } from '../Classes/chat.class';
import { BehaviorSubject } from 'rxjs';
import { NgxScrollEvent } from 'src/app/Modules/ngx-scroll/ngx-scroll.directive';
import { finalize } from 'rxjs/operators';
//import { Globalization } from 'src/app/Modules/globalization/Decorators/backend-strings.decorator';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';

@Component({
    selector: "chat-panel",
    styleUrls: ['./chat-panel.component.css'],
    templateUrl: './chat-panel.component.html'
})
//@Globalization('cm-pn-chat', [])
export class ChatPanelComponent implements OnInit {
    public curChat: BehaviorSubject<Chat> = new BehaviorSubject<Chat>(null);
    public isLoadingMsgObs: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    public isLoadingChatObs: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    public currentMessage: string = "";
    @ViewChild("toBottomBtn") public toBottomBtn: ElementRef;
    private _isToBtnShowed: boolean = false;

    constructor(gbService: GlobalizationService,
                public chatService: ChatService,
                public CMService: ChatMessageService,
                private _renderer: Renderer2){

    }

    public ngOnInit(){
        this.isLoadingChatObs.next(true);
        this.chatService.getChats().pipe(finalize(() => this.isLoadingChatObs.next(false))).subscribe();
    }

    public onSelectChat(chat: Chat){
        this.curChat.next(chat); 
        this.curChat.value.getMoreMessages().subscribe();
    }

    public delectChat(){
        this.curChat.next(null); 
    }

    public onChatScroll(event: NgxScrollEvent){
       
        if (this.curChat.value && event.isReachingBottom && !this.isLoadingMsgObs.value){
            this.isLoadingMsgObs.next(true);
            this.curChat.value.getMoreMessages().pipe(finalize(() => this.isLoadingMsgObs.next(false))).subscribe();
        }

        if (!event.isReachingTop && event.isSrollingToTop && !this._isToBtnShowed){
            this._renderer.setStyle(this.toBottomBtn.nativeElement, 'bottom', '60px');
            this._isToBtnShowed = true;
        }else if ((event.isReachingTop || event.isSrollingToBottom) && this._isToBtnShowed){
            this._renderer.setStyle(this.toBottomBtn.nativeElement, 'bottom', '0px');
            this._isToBtnShowed = false;
        }
    }

    public toBottom(){
        console.log("asdad");
        $(".chat").animate({scrollTop: 0}, 500);
    }

    public sendMessage(){
        if (!this.curChat.value || 
            this.currentMessage.length < 1) return;

        this.CMService.sendMessage(this.curChat.value, this.currentMessage).subscribe(x => this.currentMessage = "");
    }

    public delectChat(){
        this.curChat.next(null); 
    }
}