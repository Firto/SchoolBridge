import { Component, OnInit } from '@angular/core';
import { RegisterService } from 'src/app/Modules/start/Services/register.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { UserError } from 'src/app/Models/user-error.model';
import { Globalization } from 'src/app/Modules/globalization/Decorators/backend-strings.decorator';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';

@Component({
  selector: 'app-email-register',
  templateUrl: './email-register.component.html',
  styleUrls: ['./email-register.component.css']
})
@Globalization('cm-st-reg', {
  errors: [],
  validating: [
    "v-d-not-null",
    "v-str-email"
  ],
  args: [
    'email'
  ]
})
export class EmailRegisterComponent {
  [x: string]: any;
  public form: any;

  constructor(_gb: GlobalizationService,
              _fb: FormBuilder,
              private registerService: RegisterService) {}
              
  public register(): void {
    if (!this.form.valid) return;
    
    this.registerService.start(this.form.controls.email.value).subscribe(
      res => {}, 
      (err: UserError) =>{
        if (err.id == "v-dto-invalid") 
          this.validate(err);
      }
    );
  }

}
