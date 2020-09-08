import { Component, AfterViewInit } from '@angular/core';
import { Globalization } from 'src/app/Modules/globalization/Decorators/backend-strings.decorator';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';

@Component({
  selector: 'start-navbar',
  templateUrl: './start-navbar.component.html',
  styleUrls: ['./start-navbar.component.css']
})
@Globalization('cm-st-nav', [])
export class StartNavbarComponent implements AfterViewInit{
  private checkbox: HTMLInputElement;

  constructor(private gbService: GlobalizationService) { }

  onClickMenuItem(): void {
    if (this.checkbox.checked)
        this.checkbox.checked = false;
  }

  ngAfterViewInit(): void {
    this.checkbox = <HTMLInputElement> document.getElementById("chk");
  }

}
