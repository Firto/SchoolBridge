import { Component, ElementRef, OnInit, ViewChild, ɵmarkDirty } from '@angular/core';
import { RegisterService } from 'src/app/Modules/start/Services/register.service';
import { UserError } from 'src/app/Models/user-error.model';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';
import { NgxFormModel } from 'src/app/Modules/ngx-form/ngx-form.model';
import { finalize, takeUntil, tap } from 'rxjs/operators';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';
import { observed } from 'src/app/Decorators/observed.decorator';

@Component({
  selector: 'app-email-register',
  templateUrl: './email-register.component.html',
  styleUrls: ['./email-register.component.css'],
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
  providers: MdGlobalization('rg', [
    'v-str-email',
    'pn-email',
  ])
})
/*@Globalization('cm-st-reg', {
  errors: [],
  validating: [
    "v-d-not-null",
    "v-str-email"
  ],
  args: [
    'email'
  ]
})*/
export class EmailRegisterComponent extends OnUnsubscribe implements OnInit {
  public form: NgxFormModel = new NgxFormModel("form", ["email"]);
  @ViewChild('email', {static: true}) public el_lg: ElementRef;
  
  constructor(private registerService: RegisterService) {
    super();
  }
  
  ngOnInit(){
    this.form.onChanged.pipe(takeUntil(this._destroy)).subscribe(() => {
      markDirty(this);
      console.log("RRR");
    });
  }

  public onChange(arg: string): void {
    if (this.form.valid) return;
    this.form.args[arg].clearErrorsD();
  }
              
  public register(): void {
    if (!this.form.valid) return;
    
    this.registerService.start(this.el_lg.nativeElement.value).subscribe(
      res => {}, 
      (err: UserError) =>{
        if (err.id == "v-dto-invalid") 
          this.form.setErrors(err.additionalInfo);
      }
    );
  }

}
