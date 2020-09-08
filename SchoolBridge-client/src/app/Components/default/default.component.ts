import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/Services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-default',
  template: '',
})
export class DefaultComponent {

  constructor(userService: UserService,
              route: Router) { 
    if (userService.user)
      route.navigateByUrl('/panel');
    else route.navigateByUrl('/start');
  }

}
