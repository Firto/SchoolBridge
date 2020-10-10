import { Component, AfterViewInit } from '@angular/core';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';

@Component({
  selector: 'start-navbar',
  templateUrl: './start-navbar.component.html',
  styleUrls: ['./start-navbar.component.css'],
  providers: MdGlobalization('nav')
})
//@Globalization('cm-st-nav', [])
export class StartNavbarComponent {

  constructor(private gbService: GlobalizationService) { }
}
