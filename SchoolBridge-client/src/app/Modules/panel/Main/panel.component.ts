import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ClientConnectionService } from 'src/app/Services/client-connection.service';
import { MdGlobalization } from '../../globalization/Services/md-globalization.service';
import { UserPermissionService } from '../Modules/user-permission/user-permission.service';

@Component({
  selector: 'app-panel',
  templateUrl: './panel.component.html',
  styleUrls: ['./panel.component.css'],
  providers: MdGlobalization('pn')
})
export class PanelComponent {
  constructor(clConnection: ClientConnectionService,
    userPermisionService: UserPermissionService,
    router: Router) {
      
  }
}
