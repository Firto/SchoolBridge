import { Component, OnInit } from '@angular/core';
import { EndRegister } from 'src/app/Modules/start/Models/endregister.model';
import { RegisterService } from 'src/app/Modules/start/Services/register.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { UserError } from 'src/app/Models/user-error.model';
import { Globalization } from 'src/app/Modules/globalization/Decorators/backend-strings.decorator';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

@Globalization('cm-reg-end', {
  args: [
    "login",
    "name",
    "surname",
    "lastname",
    "password",
    "confirmPassword",
    "birthday"
  ],
  errors: [
    "v-d-not-null",
    "r-email-alrd-reg"
  ],
  validating: [
    "str-no-spc-ch-2",
    "str-no-spc-ch",
    "str-too-sh",
    "str-no-dig",
    "str-too-long",
    "r-date-birthday-incorrect",
    "str-inc-rep"
  ]
})
export class RegisterComponent  {
  [x: string]: any;
  regToken: string = "";
  public form: any;
  public model: EndRegister = new EndRegister(); 

  constructor(private _gb: GlobalizationService,
              private _fb: FormBuilder,
              private registerService: RegisterService,
              private route: ActivatedRoute,
              private router: Router) { 
    this.regToken = this.route.snapshot.queryParams['token'];
  }

  public register(): void {
    if (!this.form.valid) return; 

    this.model = <EndRegister>this.form.getRawValue();
    this.model.registrationToken = this.regToken;
    if (!this.model.birthday) {
      this.model.birthday = new Date();
      this.model.birthday.setMilliseconds(this.model.birthday.getMilliseconds() + 100000);
    }
    this.registerService.end(this.model).subscribe(
      res => {
        this.router.navigateByUrl('/');
      },
      (err: UserError) => {
        if (err.id == "v-dto-invalid")
            this.validate(err);
      });
  }

}
