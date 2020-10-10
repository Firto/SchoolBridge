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

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.css']
})
export class LanguageSelectorComponent extends OnUnsubscribe implements OnInit  {
  private _isChanging: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public info: GlobalizationInfo = null;

  @observed() public isChangingObs: Observable<boolean> = this._isChanging;

  constructor(public gbService: GlobalizationService,
              private _gbsService: GlobalizationStringService,
              private _gbiService: GlobalizationInfoService) {
    super();
  }

  ngOnInit(){
    this._gbiService.infoObs.pipe(
      takeUntil(this._destroy),
      delayWhen(() => this._gbsService.loading ? this._gbsService.loading.event.pipe(filter(x => !x)) : of())
    ).subscribe(x => {
      this.info = x;
      this._isChanging.next(false);
    })
    this.info = this._gbiService.info;
  }

  public onSelect(e: MouseEvent){
    this._isChanging.next(true);
    this.gbService.changeLanguage((<any>e.target).classList[1])
    .pipe().subscribe();
  }
}
