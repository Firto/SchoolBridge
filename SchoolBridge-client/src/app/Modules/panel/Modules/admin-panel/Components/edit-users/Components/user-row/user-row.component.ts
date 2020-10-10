import { Component, OnInit, ChangeDetectionStrategy, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { ModalWindowComponent } from 'ngx-modal-window';
import { takeUntil } from 'rxjs/operators';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';
import { User } from 'src/app/Modules/panel/Clases/user.class';
import { OnUnsubscribe } from 'src/app/Services/super.controller';

@Component({
  selector: '[user-row]',
  templateUrl: './user-row.component.html',
  styleUrls: ['./user-row.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserRowComponent extends OnUnsubscribe implements OnInit {
  @Input('source') user: User;
  @Output('edit') editEvent: EventEmitter<void> = new EventEmitter<void>();
  @ViewChild('modalRoot') modal: ModalWindowComponent;

  constructor() {super(); }

  ngOnInit(): void {
    this.user.onlineStatusObs.pipe(takeUntil(this._destroy)).subscribe(x => markDirty(this));
  }

}
