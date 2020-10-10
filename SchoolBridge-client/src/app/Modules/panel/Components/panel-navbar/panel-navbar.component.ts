import { Component, AfterViewInit } from '@angular/core';
import { LoginService } from 'src/app/Services/login.service';
import { Router } from '@angular/router';
//import { Globalization } from 'src/app/Modules/globalization/Decorators/backend-strings.decorator';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';

@Component({
  selector: 'panel-navbar',
  templateUrl: './panel-navbar.component.html',
  styleUrls: ['./panel-navbar.component.css'],
  providers: MdGlobalization('nv')
})
//@Globalization('cm-pn-nav', [])
export class PanelNavbarComponent{
  constructor(_gb: GlobalizationService,
              private loginService: LoginService,
              private router: Router) { }


  logout(): void {
    this.loginService.logout().subscribe(() => {
      this.router.navigateByUrl("/start");
    });
  }
}
