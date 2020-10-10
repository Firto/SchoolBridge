import { DOCUMENT } from '@angular/common';
import { ElementRef, EventEmitter, Inject, OnChanges, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { Component, OnInit, ChangeDetectionStrategy, Input, Output } from '@angular/core';
import { BehaviorSubject, merge, Observable, Subject, Subscription } from 'rxjs';
import { debounceTime, finalize, mergeMap, takeUntil, tap, throttleTime } from 'rxjs/operators';
import { observed } from 'src/app/Decorators/observed.decorator';
import { IsLoading } from 'src/app/Helpers/is-loading.class';
import { detectChanges, markDirty } from 'src/app/Helpers/mark-dirty.func';
import { NgxScrollEvent } from 'src/app/Modules/ngx-scroll/ngx-scroll.directive';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { Chat } from '../../Classes/chat.class';
import { DirectChat } from '../../Classes/direct-chat.class';
import { ChatMessageService } from '../../Services/chat-message.service';
import { ChatService } from '../../Services/chat.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MessagesComponent extends OnUnsubscribe implements OnInit, OnChanges {
  @Input("source") chat: DirectChat = null;
  @Output("back") backEvent: EventEmitter<any> = new EventEmitter<any>(false);
  private _senderbase$: Subject<any> = new Subject<void>();
  private _sender$: Observable<any> = this._senderbase$.pipe(throttleTime(1000), mergeMap(x => this._chatService.typing(this.chat.id)));

  private _onChange$: Subject<void> = new Subject<void>();
  private _onChange$$: Observable<void> = this._onChange$.pipe(debounceTime(50));
  private _subs: Subscription = null;

  public isLoading: IsLoading = new IsLoading();

  @ViewChild("toBottomBtn") public toBottomBtn: ElementRef;
  private _isToBtnShowed: boolean = false;
  constructor(@Inject(DOCUMENT) private _document: Document,
              private _msgService: ChatMessageService,
              private _chatService: ChatService,
              private _renderer: Renderer2){
    super();
    this.isLoading.event.subscribe(() => markDirty(this));
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!this.chat) {
      this._subs?.unsubscribe();
      return;
    }
    this._subs = merge(this.chat.messagesObs,
                      this.chat.user.userObs.pipe(
                        tap(x => {
                          if (!x) return;
                          x.onlineStatusObs.pipe(takeUntil(this._destroy)).subscribe(() => this._onChange$.next())
                        }))
                  ).pipe(takeUntil(this._destroy)).subscribe(() => this._onChange$.next());

    this.isLoading.status = true;
    this.chat.getMoreMessages().pipe(finalize(() => this.isLoading.status = false)).subscribe();
  }

  public onChatScroll(event: NgxScrollEvent){

    if (this.chat && event.isReachingBottom && !this.isLoading.status){
      this.isLoading.status = true;
      this.chat.getMoreMessages().pipe(finalize(() => this.isLoading.status = false)).subscribe();
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
    $(".chat").animate({scrollTop: 0}, 500);
  }

  ngOnInit(): void {
    this._onChange$$.pipe(takeUntil(this._destroy)).subscribe(() => detectChanges(this));
    this._sender$.pipe(takeUntil(this._destroy)).subscribe();
  }

  change(){
    this._senderbase$.next();
  }

  sendBtn(){
    const val: string = (<any>this._document.getElementById("inp_msg")).value;
    if (!val || val.length == 0)
      return;
    this._msgService.sendMessage(this.chat, val).subscribe(() => (<any>this._document.getElementById("inp_msg")).value = "");
  }
}
