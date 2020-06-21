import { NgModule } from '@angular/core';
import { GlobalizationService } from './services/globalization.service';
import { LanguageSelectorComponent } from './Components/language-selector/language-selector.component';
import { CommonModule } from '@angular/common';


@NgModule({
    declarations: [
        LanguageSelectorComponent
    ],
    imports: [
        CommonModule
    ],
    providers: [
        GlobalizationService
    ],
    exports: [
        LanguageSelectorComponent
    ]
})
export class GlobalizationModule  {} 