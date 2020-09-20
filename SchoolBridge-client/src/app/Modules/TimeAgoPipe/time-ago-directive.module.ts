import { NgModule } from '@angular/core';
import { TimeAgoDirective } from './time-ago.directive';
import { TimeAgoDirectiveService } from './time-ago-directive.service';

@NgModule({
  imports: [
    // dep modules
  ],
  declarations: [ 
    TimeAgoDirective
  ],
  exports: [
    TimeAgoDirective
  ]
})
export class TimeAgoDirectiveModule {
  static forRoot() {
    return {
        ngModule: TimeAgoDirectiveModule,
        providers: [
          TimeAgoDirectiveService
        ],
    };
} 
}