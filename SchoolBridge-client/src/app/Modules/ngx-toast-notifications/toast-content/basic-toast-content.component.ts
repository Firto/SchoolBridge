import { Component, Input, OnInit } from '@angular/core';
import { finalize, takeUntil } from 'rxjs/operators';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { Toast } from '../toast';

@Component({
  templateUrl: './basic-toast-content.component.html',
  styleUrls: ['./basic-toast-content.component.scss'],
})
export class BasicToastContentComponent extends OnUnsubscribe implements OnInit {
  @Input() toast: Toast;

  ngOnInit(): void {
    this.toast.config.text.pipe(
      takeUntil(this._destroy),
      takeUntil(this.toast.onClose))
      .subscribe(() => markDirty(this));
  }
}
