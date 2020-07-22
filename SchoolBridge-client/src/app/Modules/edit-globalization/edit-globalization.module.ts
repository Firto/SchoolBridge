import { NgModule } from '@angular/core';
import { GlobalizationModule } from '../globalization/globalization.module';
import { EditGlobalizationComponent } from './main/edit-globalization.component';
import { EditGlobalizationRoutingModule } from './edit-globalization-routing.module';
import { DefaultEditComponent } from './Components/default/default-edit.component';

@NgModule({
    declarations: [
        EditGlobalizationComponent,
        DefaultEditComponent
    ],
    imports: [
        GlobalizationModule,
        EditGlobalizationRoutingModule
    ],
    providers: [
         
    ],
    exports: [
        EditGlobalizationComponent 
    ]
}) 
export class EditGlobalizationModule  {} 