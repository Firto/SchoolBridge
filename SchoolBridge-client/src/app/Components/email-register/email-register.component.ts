import { Component, OnInit } from '@angular/core';
import { RegisterService } from 'src/app/Services/register.service';

@Component({
  selector: 'app-email-register',
  templateUrl: './email-register.component.html',
  styleUrls: ['./email-register.component.css']
})
export class EmailRegisterComponent implements OnInit {

  public email: string = "";
  constructor(private registerService: RegisterService) { }

  ngOnInit(): void {
    
  }

  public register(): void {
      this.registerService.start(this.email).subscribe();
  }

}
