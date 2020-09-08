import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/Services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-default-edit',
  template: '',
})
export class DefaultEditComponent {

  constructor(userService: UserService,
              route: Router) { 
    if (userService.getUser())
      route.navigateByUrl('/ed-gb/panel');
    else route.navigateByUrl('/ed-gb/start');
  }

}
