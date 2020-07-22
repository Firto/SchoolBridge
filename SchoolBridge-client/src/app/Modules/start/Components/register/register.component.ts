import { Component, OnInit } from '@angular/core';
import { EndRegister } from 'src/app/Modules/start/Models/endregister.model';
import { RegisterService } from 'src/app/Modules/start/Services/register.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  regToken: string = "";
  public model: EndRegister = new EndRegister(); 
  constructor(private registerService: RegisterService,
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
    this.registerService.end(this.model).subscribe(res => {
      if (res.ok == true)
        this.router.navigateByUrl('/');
      else if (res.result.id == "v-dto-invalid"){
        for (const [key, value] of Object.entries(res.result.additionalInfo)) 
          this.registerForm.controls[key].setErrors({"err":value});
      }
    });
  }

}
