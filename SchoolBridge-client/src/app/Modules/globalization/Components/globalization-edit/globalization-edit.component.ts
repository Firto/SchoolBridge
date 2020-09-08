import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
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
  public errors: Record<string, Observable<string>> = {}
  constructor(public gbeService: GlobalizationEditService) {
    
  }

  edit(name: string){
  
    if (document.getElementById(name).classList.contains("ng-invalid")) return;

    this.gbeService.setString(name, (<HTMLInputElement>document.getElementById(name)).value).subscribe(null, err => {
        if (err.id !== 'v-dto-invalid') return;
        
        this.errors[name] = err.additionalInfo['string'];
    });
  }

  change(name: string){
    if (Object.keys(this.errors).includes(name))
      delete this.errors[name];
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
