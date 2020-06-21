import { Component, OnInit } from '@angular/core';
import { LoginService } from '../../../../Services/login.service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder} from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  private returnUrl: string;
  constructor(private authService: LoginService, 
              private router:Router,
              private route: ActivatedRoute,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    if (this.route.snapshot.queryParams['returnUrl'])
      this.returnUrl = this.route.snapshot.queryParams['returnUrl'];
    else this.returnUrl = "/panel";
    
    this.loginForm = this.fb.group({
      login: [''],
      password: ['']
    });
    
  }
              
  logon(){
    
    if (this.loginForm.valid) 
      this.authService.login(this.loginForm.controls.login.value, this.loginForm.controls.password.value).subscribe(
        result => {
          if (result.ok == true)
            this.router.navigateByUrl(this.returnUrl);
          else if (result.result.id == "v-dto-invalid"){
            for (const [key, value] of Object.entries(result.result.additionalInfo)) 
              this.loginForm.controls[key].setErrors({"err":value});
          }
      });
  }

}