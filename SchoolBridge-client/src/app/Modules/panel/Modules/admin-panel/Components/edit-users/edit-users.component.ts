import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { detectChanges, markDirty } from 'src/app/Helpers/mark-dirty.func';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';
import { User } from 'src/app/Modules/panel/Clases/user.class';
import { UsersService } from 'src/app/Modules/panel/Services/users.service';

@Component({
  selector: 'edit-users',
  templateUrl: './edit-users.component.html',
  styleUrls: ['./edit-users.component.css'],
  providers: MdGlobalization("edt-usr",
    [

    ]
  )
})
export class EditUsersComponent implements OnInit {
  public users: User[] = null;

  constructor(private _usersService: UsersService) { }

  ngOnInit(): void {
    if (!this.users)
      this.updateUsers();
  }

  public updateUsers(){
    this._usersService.getAll().subscribe(x => {
      this.users = x.map(r => this._usersService.getFull(r));
      detectChanges(this);
    });
  }

  public editUser(usr: User){
    (usr.banned ? this._usersService.unban(usr.id) : this._usersService.ban(usr.id, "no reason")).subscribe(() => {
      usr.banned = !usr.banned;
      markDirty(this);
    });
  }
}
