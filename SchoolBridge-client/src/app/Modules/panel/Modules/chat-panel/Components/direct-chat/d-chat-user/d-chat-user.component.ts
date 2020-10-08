import { Component, OnInit, ChangeDetectionStrategy, Input, OnChanges, SimpleChanges } from '@angular/core';
import { merge, Observable } from 'rxjs';
import { debounceTime, takeUntil, tap } from 'rxjs/operators';
import { observed } from 'src/app/Decorators/observed.decorator';
import { User } from 'src/app/Modules/panel/Clases/user.class';
import { OnUnsubscribe } from 'src/app/Services/super.controller';

@Component({
  selector: 'd-chat-user',
  templateUrl: './d-chat-user.component.html',
  styleUrls: ['./d-chat-user.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DChatUserComponent extends OnUnsubscribe implements OnInit, OnChanges {
  @Input() public source: User;
  @observed() public onChnage: Observable<any>;
  public online: number = 2;

  constructor() {super() }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes.source)
      this.addOnChnage();
  }

  public ngOnInit(): void {
    if (this.source)
      this.addOnChnage();
  }

  public addOnChnage(){
    this.onChnage = merge(
      this.source.onlineStatus.pipe(tap(x => this.online = x))
    ).pipe(takeUntil(this._destroy), debounceTime(300));
  }

}
