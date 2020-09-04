import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { DbStringDirective } from '../../Directives/dbstring.directive';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { takeWhile, takeUntil } from 'rxjs/operators';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GlobalizationEditService } from '../../Services/globalization-edit.service';


@Component({
  selector: 'gb-edit',
  templateUrl: './globalization-edit.component.html',
  styleUrls: ['./globalization-edit.component.css']
})
export class GlobalizationEdit {
  constructor(public gbeService: GlobalizationEditService) {
    
  }

  edit(name: string){
    if (document.getElementById(name).classList.contains("ng-invalid")) return;

    this.gbeService.setString(name, (<HTMLInputElement>document.getElementById(name)).value).subscribe(null, err => {
        if (!('id' in err) || err.id !== 'v-dto-invalid') return;
        
        document.getElementById('p-'+name).innerHTML = JSON.stringify(err.additionalInfo['string']);
        document.getElementById(name).classList.add("ng-invalid");
    });
  }

  change(name: string){
    document.getElementById('p-'+name).innerHTML = '';
    document.getElementById(name).classList.remove("ng-invalid");
  }

  disableEditing(){
    this.gbeService.state = false;
  }

  updateBaseupdateid(){
    this.gbeService.updateBaseUpdateId().subscribe();
  }

  close(){
    this.gbeService.changeEditing(null);
  }

}
