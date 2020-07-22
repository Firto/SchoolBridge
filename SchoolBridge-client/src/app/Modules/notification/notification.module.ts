import { NgModule } from '@angular/core';
import { NotificationService } from './Services/notification.service';


@NgModule({
    declarations: [
        
    ],
    imports: [
        
    ],
    providers: [
        
    ],
    exports: [
        
    ]
})
export class NotificationModule  {
    static forRoot() {
        return {
            ngModule: NotificationModule,
            providers: [
                NotificationService
            ]
        };
    }
} 