import { Component, AfterContentInit, ViewChild, ElementRef, Renderer2 } from '@angular/core';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';

@Component({
    selector: "admin-panel",
    styleUrls: ['./admin-panel.component.css'],
    templateUrl: './admin-panel.component.html',
    providers: MdGlobalization("adm-panel")
})
export class AdminPanelComponent {
  @ViewChild("navBar") nav: ElementRef;

  constructor(private _render: Renderer2){}

  hide(){
    this._render.addClass(this.nav.nativeElement, "active");
  }

  show(){
    this._render.removeClass(this.nav.nativeElement, "active");
  }
}
