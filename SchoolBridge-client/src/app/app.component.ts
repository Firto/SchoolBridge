import { animate, group, query, style, transition, trigger } from '@angular/animations';
import { ApplicationRef, ChangeDetectionStrategy, ViewChild } from '@angular/core';
import { Component } from '@angular/core';
import { NavigationEnd, RouteConfigLoadEnd, RouteConfigLoadStart, Router, RouterEvent, RouterOutlet } from '@angular/router';
//import { ToastContainerDirective, ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { observed } from './Decorators/observed.decorator';
import { detectChanges, markDirty } from './Helpers/mark-dirty.func';
import { GlobalizationEditService } from './Modules/globalization/Services/globalization-edit.service';
import { GlobalizationStringService } from './Modules/globalization/Services/globalization-string.service';
import { GlobalizationService } from './Modules/globalization/Services/globalization.service';
import { MdGlobalization, RootMdGlobalization } from './Modules/globalization/Services/md-globalization.service';
import { LoaderService } from './Services/loader.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: RootMdGlobalization('cm', [
    'v-dto-invalid',
    'v-d-not-null',
    'load-page',
    'base-loading',
    'no-perm-page',
    'str-too-long'
  ] )
})

export class AppComponent {
  title = 'SchoolBridge';

  //@ViewChild(ToastContainerDirective, {static: true}) toastContainer: ToastContainerDirective;
  @observed() public isEditing$: Observable<boolean>;
  constructor(//private _toastrService: ToastrService,
              public gbeService: GlobalizationEditService,
              _app: ApplicationRef,
              _router: Router,
              _loaderService: LoaderService,
              _gbsService: GlobalizationStringService) {
    setTimeout(()=>{
      _app.tick();
    }, 1000);

    _router.events.subscribe(x => {
      switch (x.constructor){
        case RouteConfigLoadStart:
          _loaderService.showww("load-page");
          break;
        case RouteConfigLoadEnd:
          _loaderService.hide("load-page");
          break;
        case NavigationEnd:
          detectChanges(this);
          break;
      }
    });

    this.isEditing$ = gbeService.stateObs;

    //this._toastrService.overlayContainer = this.toastContainer;
  }

  prepareRoute(outlet: RouterOutlet) {
    return outlet;
  }
}
