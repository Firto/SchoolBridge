import {  Component, ElementRef, OnInit, ViewChild, ɵmarkDirty } from '@angular/core';
import { LoginService } from '../../../../Services/login.service';
import { Router, ActivatedRoute } from '@angular/router';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';
import { NgxFormModel } from 'src/app/Modules/ngx-form/ngx-form.model';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';
import { takeUntil } from 'rxjs/operators';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  /*animations: [
    trigger('openClose', [
      transition('open <=> closed', [
        animate('0.5s')
      ])
    ]),
    state('open', style({
      opacity: 1,
    })),
    state('closed', style({
      opacity: 0,
    })),
  ],*/
  providers: MdGlobalization('lg', [
    'l-user-banned',
    'l-pass-log-inc',
    'l-too-many-devices',
    'pn-login',
    'pn-password'
  ])
})
/*@Globalization('cm-lg', {
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
})*/
export class LoginComponent extends OnUnsubscribe implements OnInit {
  public returnUrl: string = null;
  public form: NgxFormModel = new NgxFormModel("form", ["login", "password"]);
  @ViewChild('login', {static: true}) public el_lg: ElementRef;
  @ViewChild('password', {static: true}) public el_ps:  ElementRef;

  constructor(private authService: LoginService, 
              private router:Router,
              private route: ActivatedRoute) {
    super();
    this.form.onChanged.pipe(takeUntil(this._destroy)).subscribe(() => {
      markDirty(this);
      console.log("RRR");
    });
  }

  ngOnInit(): void {
    if (this.route.snapshot.queryParams['returnUrl'])
      this.returnUrl = this.route.snapshot.queryParams['returnUrl'];
    else this.returnUrl = "/panel";
  }
  
  onChange(arg: string){
    if (this.form.valid) return;
    this.form.args[arg].clearErrorsD();
  }

  logon(){
    if (this.form.valid)
      this.authService.login(this.el_lg.nativeElement.value, this.el_ps.nativeElement.value).subscribe(
        val => {
          this.router.navigateByUrl(this.returnUrl);
        },
        err => {
          if (err.id == "v-dto-invalid")
            this.form.setErrors(err.additionalInfo);
        } 
      );
  }

}