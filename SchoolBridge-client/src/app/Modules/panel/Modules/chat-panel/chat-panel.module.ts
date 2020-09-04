import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserPermissionModule } from '../user-permission/user-permission.module';
import { ReactiveFormsModule } from '@angular/forms';
import { ChatPanelComponent } from './main/chat-panel.component';
import { TimeAgoPipeModule } from 'src/app/Modules/TimeAgoPipe/time-ago-pipe.module';
import { ChatService } from './Services/chat.service';
import { ChatMessageService } from './Services/chat-message.service';

@NgModule({
    declarations: [
        ChatPanelComponent
    ],
    imports: [
        CommonModule,
        UserPermissionModule,
        ReactiveFormsModule,
        TimeAgoPipeModule
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