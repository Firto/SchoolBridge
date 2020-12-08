import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxScrollDirective } from './ngx-scroll.directive';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    NgxScrollDirective,
  ],
  exports: [
    NgxScrollDirective,
  ]
})
export class NgxScrollEventModule { }