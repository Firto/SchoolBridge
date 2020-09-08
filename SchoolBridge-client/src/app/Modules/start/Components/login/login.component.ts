import { Component, OnInit } from '@angular/core';
import { LoginService } from '../../../../Services/login.service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder} from '@angular/forms';
import { Globalization } from 'src/app/Modules/globalization/Decorators/backend-strings.decorator';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';
import { GlobalizationStringService } from 'src/app/Modules/globalization/Services/globalization-string.service';
import { ParentComponent } from 'src/app/Services/parent.component';

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
  validating:[ 
    'v-d-not-null'
  ],
  args: [
    'login',
    'password'
  ]
})
export class LoginComponent extends ParentComponent implements OnInit {
  [x: string]: any;
  public form: any;
  
  private returnUrl: string;
  constructor(gbService: GlobalizationService,
              fb: FormBuilder,
              private authService: LoginService, 
              private router:Router,
              private route: ActivatedRoute,
              ) {super();}

  ngOnInit(): void {
    if (this.route.snapshot.queryParams['returnUrl'])
      this.returnUrl = this.route.snapshot.queryParams['returnUrl'];
    else this.returnUrl = "/panel";
  }
              
  logon(){
    if (this.form.valid)
      this.authService.login(this.form.controls.login.value, this.form.controls.password.value).subscribe(
        val => {
          this.router.navigateByUrl(this.returnUrl);
        },
        err => {
          if (err.id == "v-dto-invalid")
            this.validate(err);
        }
      );
  }

}