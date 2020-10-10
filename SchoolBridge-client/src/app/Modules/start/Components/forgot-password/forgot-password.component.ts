import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { takeUntil } from 'rxjs/operators';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';
import { UserError } from 'src/app/Models/user-error.model';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';
import { NgxFormModel } from 'src/app/Modules/ngx-form/ngx-form.model';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { RegisterService } from '../../Services/register.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css'],
  providers: MdGlobalization('for-pas', [
    'v-str-email',
    'pn-email',
    // Succesful sending email to
    'tst-suc-send-em',
    'tst-unsuc-send-em',
    // Wait sending email,
    'wait-snd-em'
  ]),
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ForgotPasswordComponent extends OnUnsubscribe implements OnInit {
  public form: NgxFormModel = new NgxFormModel("form", ["email"]);
  constructor(private registerService: RegisterService) { 
    super();
  }

  ngOnInit(): void {
    this.form.onChanged.pipe(takeUntil(this._destroy)).subscribe(() => {
      markDirty(this);
    });
  }

  public onChange(arg: string): void {
    if (!this.form.valid)
      this.form.args[arg].clearErrorsD();
  }

  public forgotPassword(): void {
    if (!this.form.valid) return;
    console.log(this.form.args.email.value);
    this.registerService.forgotPassword(this.form.args.email.value).subscribe(
      res => {},
      (err: UserError) =>{
        if (err.id == "v-dto-invalid")
          this.form.setErrors(err.additionalInfo);
      }
    );
  }


}
