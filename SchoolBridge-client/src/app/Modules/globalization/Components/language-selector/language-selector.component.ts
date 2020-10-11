import { AfterViewInit, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GlobalizationService } from '../../Services/globalization.service';
import { GlobalizationInfoService } from '../../Services/globalization-info.service';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { debounceTime, delayWhen, filter, finalize, mergeMap, takeUntil, tap } from 'rxjs/operators';
import { GlobalizationInfo } from '../../Models/globalization-info.model';
import { observed } from 'src/app/Decorators/observed.decorator';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { GlobalizationStringService } from '../../Services/globalization-string.service';
import { IsLoading } from 'src/app/Helpers/is-loading.class';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.css']
})
export class LanguageSelectorComponent extends OnUnsubscribe implements OnInit  {
  public isLoading: IsLoading = new IsLoading();

  @observed() public isChangingObs: Observable<boolean> = this.isLoading.event;

  constructor(public gbService: GlobalizationService,
              public gbiService: GlobalizationInfoService,
              private _gbsService: GlobalizationStringService) {
    super();
  }

  ngOnInit(){
    this.gbiService.infoObs.pipe(
      takeUntil(this._destroy),
      delayWhen(() => this._gbsService.loading ? this._gbsService.loading.event.pipe(filter(x => !x)) : of())
    ).subscribe(x => {
      this.isLoading.status = false;
    })
  }

  public onSelect(e: MouseEvent){
    this.isLoading.status = true;
    this.gbService.changeLanguage((<any>e.target).classList[1])
    .pipe().subscribe();
  }
}
