import { NgModule } from '@angular/core';
import { GlobalizationService } from './services/globalization.service';
import { LanguageSelectorComponent } from './Components/language-selector/language-selector.component';
import { CommonModule } from '@angular/common';
import { DbStringDirective } from './Directives/dbstring.directive';
import { GlobalizationEdit } from './Components/globalization-edit/globalization-edit.component';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
    declarations: [
        LanguageSelectorComponent,
        GlobalizationEdit,
        DbStringDirective
    ],
    imports: [
        CommonModule
    ],
    exports: [ 
        LanguageSelectorComponent,
        GlobalizationEdit,
        DbStringDirective
    ]
})
export class GlobalizationModule  {
    static forRoot() {
        return {
            ngModule: GlobalizationModule,
            providers: [
                GlobalizationService
            ]
        };
    }
} 