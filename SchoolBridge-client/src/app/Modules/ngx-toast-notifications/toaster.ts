import { Inject, Injectable, Type } from '@angular/core';
import { ToastConfig } from './toast.config';
import { ToastContainerService } from './toast-container.service';
import { TOAST_NOTIFICATIONS_CONFIG, ToastNotificationsConfig } from './toast-notifications.config';
import { BasicToastContentComponent } from './toast-content/basic-toast-content.component';
import { Toast } from './toast';
import { Observable } from 'rxjs';

const DEFAULT_CONFIG: ToastConfig = {
  autoClose: true,
  duration: 8000,
  type: 'light',
  position: 'bottom-right',
  component: BasicToastContentComponent,
};

@Injectable()
export class Toaster {

  constructor(
      @Inject(TOAST_NOTIFICATIONS_CONFIG) private _config: ToastNotificationsConfig,
      private _containerService: ToastContainerService,
  ) {
  }

  open(config: ToastConfig): Toast | null;
  open(text: Observable<string>, config?: ToastConfig): Toast | null;
  open(component: Type<any>, config?: ToastConfig): Toast | null;
  open(config: ToastConfig | Observable<string> | Type<any>, componentConfig?: ToastConfig): Toast | null {

    if (config instanceof Observable) {
      config = {text: config, ...componentConfig};
    }
    if (config instanceof Type) {
      config = {...componentConfig, component: config as Type<any>};
    }
    config = {...DEFAULT_CONFIG, ...this._config, ...config};
    return this._containerService.ref.instance.add(config);
  }
}
