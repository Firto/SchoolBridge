import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';
import { UserError } from 'src/app/Models/user-error.model';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';
import { NgxFormModel } from 'src/app/Modules/ngx-form/ngx-form.model';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { RegisterService } from '../../Services/register.service';

@Component({
  selector: 'app-end-forgot-password',
  templateUrl: './end-forgot-password.component.html',
  styleUrls: ['./end-forgot-password.component.css'],
  providers: MdGlobalization("e-for-pass",[
    "str-no-spc-ch-2",
    "str-no-spc-ch",
    "str-too-sh",
    "str-no-dig",
    "str-too-long",
    "str-inc-rep",

    "cl-r-token-already-used",
    "cl-r-token-inc"
  ]),
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class EndForgotPasswordComponent extends OnUnsubscribe implements OnInit {
  changePasswordToken: string = "";
  public form: NgxFormModel = new NgxFormModel("form",
    [
      {name: "password", type: "password"},
      {name: "confirmPassword", type: "password"}
    ]
  );
  
  constructor(private registerService: RegisterService,
    private route: ActivatedRoute,
    private router: Router) {
      super();
    this.changePasswordToken = this.route.snapshot.queryParams['token'];
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

  public endForgotPassword(): void {
    if (!this.form.valid) return;

    const ss: any = this.form.createObj();
    ss.changePasswordToken = this.changePasswordToken;
    
    this.registerService.endForgotPassword(ss).subscribe(
      res => {
        this.router.navigateByUrl('/');
      },
      (err: UserError) => {
        if (err.id == "v-dto-invalid")
          this.form.setErrors(err.additionalInfo);
      });
  }
}