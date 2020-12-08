import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserPermissionModule } from '../user-permission/user-permission.module';
import { ChatPanelComponent } from './main/chat-panel.component';
import { ChatService } from './Services/chat.service';
import { ChatMessageService } from './Services/chat-message.service';
import { NgxScrollEventModule } from 'src/app/Modules/ngx-scroll/ngx-scroll.module';
import { TimeAgoDirectiveModule } from 'src/app/Modules/TimeAgoPipe/time-ago-directive.module';
import { GlobalizationModule } from 'src/app/Modules/globalization/globalization.module';
import { MessagesComponent } from './Components/messages/messages.component';
import { MessageComponent } from './Components/message/message.component';
import { DirectChatComponent } from './Components/direct-chat/direct-chat.component';
import { TypingIndicatorComponent } from './Components/typing-indicator/typing-indicator.component';

@NgModule({
    declarations: [
        ChatPanelComponent,
        DirectChatComponent,
        MessagesComponent,
        MessageComponent,
        TypingIndicatorComponent
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
