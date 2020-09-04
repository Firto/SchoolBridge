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
  errors: [],
  validating: []
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  regToken: string = "";
  
  public model: EndRegister = new EndRegister(); 

  constructor(private gbService: GlobalizationService,
              private registerService: RegisterService,
              private route: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) { 
    this.regToken = this.route.snapshot.queryParams['token'];
  }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      login: [''],
      name: [''],
      surname: [''],
      lastname: [''],
      password: [''],
      confirmPassword: [''],
      birthday: ['']
    });
  }

  public register(): void {
    this.model = <EndRegister>this.registerForm.getRawValue();
    this.model.registrationToken = this.regToken;
    this.registerService.end(this.model).subscribe(
      res => {
        this.router.navigateByUrl('/');
      },
      (err: UserError) => {
        if (err.id == "v-dto-invalid")
          for (const [key, value] of Object.entries(err.additionalInfo)) 
            this.registerForm.controls[key].setErrors({"err":value});
      });
  }

}
