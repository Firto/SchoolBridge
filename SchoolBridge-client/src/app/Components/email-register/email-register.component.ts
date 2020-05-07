import { Component, OnInit } from '@angular/core';
import { RegisterService } from 'src/app/Services/register.service';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-email-register',
  templateUrl: './email-register.component.html',
  styleUrls: ['./email-register.component.css']
})
export class EmailRegisterComponent implements OnInit {
  registerForm: FormGroup;
  
  constructor(private registerService: RegisterService,
              private fb: FormBuilder) { 
    
  }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      email: ['']
    });
  }
              

  public register(): void {
    this.registerService.start(this.registerForm.controls.email.value).subscribe(res => {
      if (res.ok != true && res.result.id == "v-dto-invalid"){
        for (const [key, value] of Object.entries(res.result.additionalInfo)) 
          this.registerForm.controls[key].setErrors({"err":value});
      }
    });
  }

}
