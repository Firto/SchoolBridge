import { Component, OnInit, ChangeDetectionStrategy, Input, OnChanges, SimpleChanges } from '@angular/core';
import { merge, Observable, Subject } from 'rxjs';
import { debounceTime, mergeMap, takeUntil, tap } from 'rxjs/operators';
import { observed } from 'src/app/Decorators/observed.decorator';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';
import { User } from 'src/app/Modules/panel/Clases/user.class';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { DirectChat } from '../../Classes/direct-chat.class';

@Component({
  selector: 'direct-chat',
  templateUrl: './direct-chat.component.html',
  styleUrls: ['./direct-chat.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DirectChatComponent extends OnUnsubscribe implements OnInit, OnChanges {
  @Input() public source: DirectChat;
  @observed() public onChange: Observable<any>;
  constructor() {super(); }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes.source)
      this.addOnChnage();
  }

  public ngOnInit(): void {
    if (this.source)
      this.addOnChnage();
  }

  public addOnChnage(){
    this.onChange = merge(this.source.user.userObs.pipe(tap(x => {
      if(!x) return;
      x.onlineStatusObs.pipe(takeUntil(this._destroy)).subscribe(() => markDirty(this));
    })), this.source.lastMessageObs).pipe(takeUntil(this._destroy), debounceTime(300));
  }
}
