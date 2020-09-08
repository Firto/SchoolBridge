import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { DbStringDirective } from '../../Directives/dbstring.directive';
import { GlobalizationService } from '../../services/globalization.service';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { takeWhile, takeUntil } from 'rxjs/operators';
import { FormBuilder, FormGroup } from '@angular/forms';


@Component({
  selector: 'gb-edit',
  templateUrl: './globalization-edit.component.html',
  styleUrls: ['./globalization-edit.component.css']
})
export class GlobalizationEdit extends OnUnsubscribe implements OnInit {
  public cur: BehaviorSubject<DbStringDirective> = new BehaviorSubject<DbStringDirective>(null);
  constructor(public globalizationService: GlobalizationService) {
    super();
  }

  ngOnInit(){
    this.globalizationService.onStringEditWindow.pipe(takeUntil(this._destroy)).subscribe(x => {
        this.cur.next(x);
    });
  }

  edit(name: string){
    if (document.getElementById(name).classList.contains("ng-invalid")) return;

    this.globalizationService.setString(name, (<HTMLInputElement>document.getElementById(name)).value).subscribe(null, err => {
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
    this.globalizationService.setEditingState(false);
  }

  close(){
    this.cur.next(null);
  }

}
