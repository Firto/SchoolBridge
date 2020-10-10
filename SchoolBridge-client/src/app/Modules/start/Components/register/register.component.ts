import { Component, OnInit } from '@angular/core';
import { EndRegister } from 'src/app/Modules/start/Models/endregister.model';
import { RegisterService } from 'src/app/Modules/start/Services/register.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { UserError } from 'src/app/Models/user-error.model';
//import { Globalization } from 'src/app/Modules/globalization/Decorators/backend-strings.decorator';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';
import { NgxFormModel } from 'src/app/Modules/ngx-form/ngx-form.model';
import { takeUntil } from 'rxjs/operators';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';
import { OnUnsubscribe } from 'src/app/Services/super.controller';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  providers: MdGlobalization("e-reg",[
    "pn-login",
    "pn-name",
    "pn-surname",
    "pn-lastname",
    "pn-password",
    "pn-confirmPassword",
    "pn-birthday",

    "r-email-alrd-reg",

    "str-no-spc-ch-2",
    "str-no-spc-ch",
    "str-too-sh",
    "str-no-dig",
    "str-too-long",
    "r-date-birthday-incorrect",
    "str-inc-rep",

    "cl-r-token-already-used",
    "cl-r-token-inc"
  ])
})
export class RegisterComponent extends OnUnsubscribe implements OnInit {
  regToken: string = "";
  public form: NgxFormModel = new NgxFormModel("form",
    [
      "login",
      "name",
      "surname",
      "lastname",
      {name: "birthday", type: "date"},
      {name: "password", type: "password"},
      {name: "confirmPassword", type: "password"}
    ]
  );

  constructor(private registerService: RegisterService,
              private route: ActivatedRoute,
              private router: Router) {
    super();
    this.regToken = this.route.snapshot.queryParams['token'];
  }

  ngOnInit(): void {
    this.form.onChanged.pipe(takeUntil(this._destroy)).subscribe(() => {
      markDirty(this);
    });
  }

  onChange(arg: string){
    if (this.form.valid) return;
    this.form.args[arg].clearErrorsD();
  }

  public register(): void {
    if (!this.form.valid) return;

    const ss: any = this.form.createObj();
    ss.registrationToken = this.regToken;
    console.log(ss);
    if (!ss.birthday) {
      ss.birthday = new Date();
      ss.birthday.setMilliseconds(ss.birthday.getMilliseconds() + 100000);
    }
    this.registerService.end(ss).subscribe(
      res => {
        this.router.navigateByUrl('/');
      },
      (err: UserError) => {
        if (err.id == "v-dto-invalid")
          this.form.setErrors(err.additionalInfo);
      });
  }
}