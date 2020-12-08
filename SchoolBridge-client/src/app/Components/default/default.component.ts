import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/Services/user.service';
import { Router } from '@angular/router';
import { UserPermissionService } from 'src/app/Modules/panel/Modules/user-permission/user-permission.service';

@Component({
  selector: 'app-default',
  template: '',
})
export class DefaultComponent {

  constructor(userService: UserService,
    userPermisionService: UserPermissionService,
    route: Router) {
    if (userService.user) {
      if (!userPermisionService.HasPermission(['GetAdminPanel']))
        route.navigateByUrl('/panel/subjects');
      else
        route.navigateByUrl('/panel');
    }
    else route.navigateByUrl('/start');
  }

}
