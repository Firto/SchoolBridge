import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserPermissionModule } from '../user-permission/user-permission.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ChatPanelComponent } from './main/chat-panel.component';
import { ChatService } from './Services/chat.service';
import { ChatMessageService } from './Services/chat-message.service';
import { NgxScrollEventModule } from 'src/app/Modules/ngx-scroll/ngx-scroll.module';
import { TimeAgoDirectiveModule } from 'src/app/Modules/TimeAgoPipe/time-ago-directive.module';
import { GlobalizationModule } from 'src/app/Modules/globalization/globalization.module';

@NgModule({
    declarations: [
        ChatPanelComponent
    ],
    imports: [
        CommonModule,
        UserPermissionModule,
        TimeAgoDirectiveModule,
        NgxScrollEventModule,
        GlobalizationModule
    ],
    providers: [
        ChatService,
        ChatMessageService
    ],
    exports: [
        ChatPanelComponent
    ]
})
export class ChatPanelModule  {} 