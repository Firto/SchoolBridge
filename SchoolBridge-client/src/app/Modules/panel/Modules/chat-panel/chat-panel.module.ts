import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserPermissionModule } from '../user-permission/user-permission.module';
import { ChatPanelComponent } from './main/chat-panel.component';
import { ChatService } from './Services/chat.service';
import { ChatMessageService } from './Services/chat-message.service';
import { NgxScrollEventModule } from 'src/app/Modules/ngx-scroll/ngx-scroll.module';
import { TimeAgoDirectiveModule } from 'src/app/Modules/TimeAgoPipe/time-ago-directive.module';
import { GlobalizationModule } from 'src/app/Modules/globalization/globalization.module';
import { DirectChatComponent } from './Components/direct-chat/direct-chat.component';
import { DChatUserComponent } from './Components/direct-chat/d-chat-user/d-chat-user.component';

@NgModule({
    declarations: [
        ChatPanelComponent,
        DirectChatComponent,
        DChatUserComponent
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
