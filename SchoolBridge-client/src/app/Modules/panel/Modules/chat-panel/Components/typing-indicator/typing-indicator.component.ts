import { Component, OnInit, ChangeDetectionStrategy, Input, OnChanges, SimpleChange, SimpleChanges } from '@angular/core';
import { merge, Observable, Subscription } from 'rxjs';
import { debounceTime, filter, takeUntil, tap } from 'rxjs/operators';
import { detectChanges, markDirty } from 'src/app/Helpers/mark-dirty.func';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { DirectChat } from '../../Classes/direct-chat.class';

@Component({
  selector: 'typing-indicator',
  templateUrl: './typing-indicator.component.html',
  styleUrls: ['./typing-indicator.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TypingIndicatorComponent extends OnUnsubscribe implements OnInit, OnChanges {
  @Input('chat') chat: DirectChat;
  private _subs: Subscription;
  public typing: Boolean = false;
  constructor() { super();}

  ngOnChanges(ch: SimpleChanges){
    if (!ch.chat)
      return;
    if(this._subs)
      this._subs.unsubscribe();

    if (this.chat)
      this._subs =
      merge(
        this.chat.typingObs.pipe(tap(x => this.typing = x)),
        this.chat.typingObs.pipe(filter(x => x), debounceTime(2000), tap(x => this.chat.setTyping([]))),
      ).pipe(takeUntil(this._destroy)).subscribe(() => detectChanges(this));
  }

  ngOnInit(): void {

  }

}
