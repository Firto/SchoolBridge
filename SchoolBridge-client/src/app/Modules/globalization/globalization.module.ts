import { NgModule } from '@angular/core';
import { LanguageSelectorComponent } from './Components/language-selector/language-selector.component';
import { CommonModule } from '@angular/common';
import { DbStringDirective } from './Directives/dbstring.directive';
import { GlobalizationEdit } from './Components/globalization-edit/globalization-edit.component';
import { GlobalizationInterceptor } from './Interceptors/globalization.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { GlobalizationInfoService } from './Services/globalization-info.service';
import { HttpGlobalizationService } from './Services/http-globalization.service';
import { GlobalizationStringService } from './Services/globalization-string.service';
import { GlobalizationEditService } from './Services/globalization-edit.service';
import { GlobalizationService } from './Services/globalization.service';


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
                GlobalizationEditService,
                GlobalizationService,
                GlobalizationInfoService,
                GlobalizationStringService,
                HttpGlobalizationService,
                //{ provide: HTTP_INTERCEPTORS, useClass: GlobalizationInterceptor, multi: true }
            ]
        };
    }
} 