import { Component, OnInit } from '@angular/core';
import { EndRegister } from 'src/app/Models/endregister.model';
import { RegisterService } from 'src/app/Services/register.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public model: EndRegister = new EndRegister(); 
  constructor(private registerService: RegisterService,
              private route: ActivatedRoute,
              private router: Router) { 
    this.model.registrationToken = this.route.snapshot.queryParams['token'];
  }

  ngOnInit(): void {
    
  }

  public register(): void {
    this.registerService.end(this.model).subscribe((x) => {
      if (x.ok)
        this.router.navigate(['/home']);
    });
  }

}
