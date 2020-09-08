import { Component, AfterViewInit } from '@angular/core';
import { LoginService } from 'src/app/Services/login.service';
import { Router } from '@angular/router';
import { Globalization } from 'src/app/Modules/globalization/Decorators/backend-strings.decorator';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';

@Component({
  selector: 'panel-navbar',
  templateUrl: './panel-navbar.component.html',
  styleUrls: ['./panel-navbar.component.css']
})
@Globalization('cm-pn-nav', [])
export class PanelNavbarComponent implements AfterViewInit{ 
  private checkbox: HTMLInputElement;

  constructor(_gb: GlobalizationService,
              private loginService: LoginService,
              private router: Router) { }

  onClickMenuItem(): void {
    if (this.checkbox.checked)
        this.checkbox.checked = false;
  }

  logout(): void {
    this.loginService.logout().subscribe(() => {
      this.router.navigateByUrl("/start");
    });
  }

  ngAfterViewInit(): void {
    this.checkbox = <HTMLInputElement> document.getElementById("chkk");
  }

}
