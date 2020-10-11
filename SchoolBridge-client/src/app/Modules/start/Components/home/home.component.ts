import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { extend } from 'jquery';
import { takeUntil } from 'rxjs/operators';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';
import { UserError } from 'src/app/Models/user-error.model';
import { NgxFormModel } from 'src/app/Modules/ngx-form/ngx-form.model';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { RegisterService } from '../../Services/register.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent extends OnUnsubscribe implements OnInit {

  public form: NgxFormModel = new NgxFormModel("form", ["email"]);

  constructor(private registerService: RegisterService) {
    super();
  }

  ngOnInit(){
    this.form.onChanged.pipe(takeUntil(this._destroy)).subscribe(() => {
      markDirty(this);
    });
  }

  public onChange(arg: string): void {
    if (!this.form.valid)
      this.form.args[arg].clearErrorsD();
  }

  public register(): void {
    if (!this.form.valid) return;

    this.registerService.start(this.form.args.email.value).subscribe(
      res => {},
      (err: UserError) =>{
        if (err.id == "v-dto-invalid")
          this.form.setErrors(err.additionalInfo);
      }
    );
  }

}
