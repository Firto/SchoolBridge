import { Component, OnInit } from '@angular/core';
import { LoginService } from '../../../../Services/login.service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder} from '@angular/forms';
import { Globalization } from 'src/app/Modules/globalization/Decorators/backend-strings.decorator';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
@Globalization('cm-lg', {
  errors: [
    'l-user-banned',
    'l-pass-log-inc',
    'l-too-many-devices',
    'v-dto-invalid'
  ],
  validating:[ 'v-d-not-null'],
  args: [
    'login',
    'password'
  ]
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  private returnUrl: string;
  constructor(private gbService: GlobalizationService,
              private authService: LoginService, 
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

    //console.log((<any>this).__proto__.constructor._usedBackendStrings, this);
  }
              
  logon(){
    this.authService.login(this.loginForm.controls.login.value, this.loginForm.controls.password.value).subscribe(
      val => {
        this.router.navigateByUrl(this.returnUrl);
      },
      err => {
        if (err.id == "v-dto-invalid")
          for (const [key, value] of Object.entries(err.additionalInfo)) 
            this.loginForm.controls[key].setErrors({"err":value});
      }
    );
  }

}