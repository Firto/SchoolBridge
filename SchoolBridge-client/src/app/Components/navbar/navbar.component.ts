import { Component, AfterViewInit } from '@angular/core';
import { UserService } from 'src/app/Services/user.service';
import { LoginService } from 'src/app/Services/login.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements AfterViewInit{
  private checkbox: HTMLInputElement;

  constructor(public userService: UserService,
              private loginService: LoginService) { }

  ngAfterViewInit(): void {
    this.checkbox = <HTMLInputElement> document.getElementById("chk");
    console.log(this.constructor.name); 
  }

  onClickMenuItem(): void {
    if (this.checkbox.checked)
        this.checkbox.checked = false;
  }

  logout(): void {
    this.loginService.logout().subscribe();
  }

}
