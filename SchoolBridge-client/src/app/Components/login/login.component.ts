import { Component, OnInit } from '@angular/core';
import { LoginService } from 'src/app/Services/login.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public login:string = "";
  public password: string = "";
  public errorMessage: string = null;

  private returnUrl: string;
  constructor(private authService: LoginService, 
              private router:Router,
              private route: ActivatedRoute) { }

  ngOnInit(): void {
    if (this.route.snapshot.queryParams['returnUrl'] && 
      this.router.config.find(route => "/"+route.path == this.route.snapshot.queryParams['returnUrl']))

      this.returnUrl = this.route.snapshot.queryParams['returnUrl'];
    else this.returnUrl = "/home";
  }
              
  logon(){
    this.authService.login(this.login, this.password).subscribe(
      result => {
        if (result.ok == true)
          this.router.navigate([this.returnUrl]);
      }
    )
  }

}