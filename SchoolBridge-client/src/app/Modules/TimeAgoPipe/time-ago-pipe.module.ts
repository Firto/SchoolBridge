import { NgModule } from '@angular/core';
import { TimeAgoPipe } from './time-ago.pipe';
import { TimeAgoPipeService } from './time-ago.service';

@NgModule({
  imports: [
    // dep modules
  ],
  declarations: [ 
    TimeAgoPipe
  ],
  exports: [
    TimeAgoPipe
  ]
})
export class TimeAgoPipeModule {
  static forRoot() {
    return {
        ngModule: TimeAgoPipeModule,
        providers: [
          TimeAgoPipeService
        ],
    };
}
}