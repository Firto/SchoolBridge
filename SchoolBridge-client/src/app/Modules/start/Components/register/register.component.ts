import { Component, OnInit } from '@angular/core';
import { EndRegister } from 'src/app/Modules/start/Models/endregister.model';
import { RegisterService } from 'src/app/Modules/start/Services/register.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { UserError } from 'src/app/Models/user-error.model';
//import { Globalization } from 'src/app/Modules/globalization/Decorators/backend-strings.decorator';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  providers: MdGlobalization("e-reg",[
    "pn-login",
    "pn-name",
    "pn-surname",
    "pn-lastname",
    "pn-password",
    "pn-confirmPassword",
    "pn-birthday",

    "r-email-alrd-reg",

    "str-no-spc-ch-2",
    "str-no-spc-ch",
    "str-too-sh",
    "str-no-dig",
    "str-too-long",
    "r-date-birthday-incorrect",
    "str-inc-rep"
  ])
})
export class RegisterComponent  {
  regToken: string = "";
  public model: EndRegister = new EndRegister();

  constructor(private registerService: RegisterService,
              private route: ActivatedRoute,
              private router: Router) {
    this.regToken = this.route.snapshot.queryParams['token'];
  }

  public register(): void {
  }

}
