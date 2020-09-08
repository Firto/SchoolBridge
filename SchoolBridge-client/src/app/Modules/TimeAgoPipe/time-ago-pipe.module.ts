import { NgModule } from '@angular/core';
import { TimeAgoPipe } from './time-ago.pipe';

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
export class TimeAgoPipeModule {}