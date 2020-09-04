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
  validating: ["v-str-email"],
  args: [
    'email'
  ]
})
export class EmailRegisterComponent implements OnInit {
  registerForm: FormGroup;
  
  constructor(private gbService: GlobalizationService,
              private registerService: RegisterService,
              private fb: FormBuilder) { 
    
  }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      email: ['']
    });
  }
              

  public register(): void {
    this.registerService.start(this.registerForm.controls.email.value).subscribe(
      res => {}, 
      (err: UserError) =>{
        if (err.id != "v-dto-invalid") return;
        for (const [key, value] of Object.entries(err.additionalInfo)) 
          this.registerForm.controls[key].setErrors({"err":value});
      }
    );
  }

}
