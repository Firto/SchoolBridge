import { Component, AfterViewInit } from '@angular/core';
import { LoginService } from 'src/app/Services/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'panel-navbar',
  templateUrl: './panel-navbar.component.html',
  styleUrls: ['./panel-navbar.component.css']
})
export class PanelNavbarComponent implements AfterViewInit{ 
  private checkbox: HTMLInputElement;

  constructor(private loginService: LoginService,
              private router: Router) { }

  onClickMenuItem(): void {
    if (this.checkbox.checked)
        this.checkbox.checked = false;
  }

  logout(): void {
    this.loginService.logout().subscribe();
  }

  ngAfterViewInit(): void {
    this.checkbox = <HTMLInputElement> document.getElementById("chkk");
  }

}
