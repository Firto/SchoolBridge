import { Component, ElementRef, OnInit, ViewChild, ɵmarkDirty } from '@angular/core';
import { LoginService } from '../../../../Services/login.service';
import { Router, ActivatedRoute } from '@angular/router';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';
import { NgxFormModel } from 'src/app/Modules/ngx-form/ngx-form.model';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';
import { takeUntil } from 'rxjs/operators';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';
import { UserPermissionService } from 'src/app/Modules/panel/Modules/user-permission/user-permission.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: MdGlobalization('lg', [
    'cl-l-user-banned',
    'cl-l-pass-log-inc',
    'cl-l-too-many-devices',
    'pn-login',
    'pn-password'
  ])
})
export class LoginComponent extends OnUnsubscribe implements OnInit {
  public returnUrl: string = null;
  public form: NgxFormModel = new NgxFormModel("form", ["login", { name: "password", type: "password" }]);

  constructor(private authService: LoginService,
    private router: Router,
    private route: ActivatedRoute,
    private userPermisionService: UserPermissionService) {
    super();
  }

  ngOnInit(): void {
    if (this.route.snapshot.queryParams['returnUrl'])
      this.returnUrl = this.route.snapshot.queryParams['returnUrl'];
    else this.returnUrl = "/panel";

    this.form.onChanged.pipe(takeUntil(this._destroy)).subscribe(() => {
      markDirty(this);
    });
  }

  onChange(arg: string) {
    if (this.form.valid) return;
    this.form.args[arg].clearErrorsD();
  }

  logon() {
    if (this.form.valid)
      this.authService.login(this.form.args.login.value, this.form.args.password.value).subscribe(
        val => {
          console.log('!!!!!!!!!!!!!!!!!!!!!!  ' + this.returnUrl);
          if (!this.route.snapshot.queryParams['returnUrl']) {
            if (!this.userPermisionService.HasPermission(['GetAdminPanel']))
              this.router.navigateByUrl('/panel/subjects');
            else
              this.router.navigateByUrl('/panel');
          }
          else
            this.router.navigateByUrl(this.returnUrl);
        },
        err => {
          if (err.id == "v-dto-invalid")
            this.form.setErrors(err.additionalInfo);
        }
      );
  }

}
