import { Observable } from 'rxjs';
import { ToastNotificationsConfig } from './toast-notifications.config';

export interface ToastConfig extends ToastNotificationsConfig {
  text?: Observable<string>;
  caption?: string;
}
