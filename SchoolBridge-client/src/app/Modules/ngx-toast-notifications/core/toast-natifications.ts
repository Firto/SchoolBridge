import { Injectable } from '@angular/core';
import { ToastConfig } from '../toast.config';
import { ToastType } from '../toast-notifications.config';
import { Toaster } from '../toaster';
import { Observable } from 'rxjs';

@Injectable()
/**
 * @deprecated since version 1.0.0 use Toaster
 */
export class ToastNotifications {

  constructor(
      private _toaster: Toaster,
  ) {
  }

  /**
   * @deprecated since version 1.0.0
   */
  next(toast: {text: Observable<string>, caption?: string, type?: ToastType, lifetime?: number, duration?: number}) {
    const config: ToastConfig = {
      text: toast.text,
      caption: toast.caption,
      type: toast.type,
      duration: toast.duration || toast.lifetime,
      component: null,
    };
    this._toaster.open(config);
  }
}
