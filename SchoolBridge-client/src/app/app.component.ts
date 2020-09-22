import { animate, group, query, style, transition, trigger } from '@angular/animations';
import { ApplicationRef } from '@angular/core';
import { Component } from '@angular/core';
import { NavigationEnd, RouteConfigLoadEnd, RouteConfigLoadStart, Router, RouterEvent, RouterOutlet } from '@angular/router';
import { Observable } from 'rxjs';
import { observed } from './Decorators/observed.decorator';
import { detectChanges, markDirty } from './Helpers/mark-dirty.func';
import { GlobalizationEditService } from './Modules/globalization/Services/globalization-edit.service';
import { MdGlobalization, RootMdGlobalization } from './Modules/globalization/Services/md-globalization.service';
import { LoaderService } from './Services/loader.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: RootMdGlobalization('cm', [
    'v-dto-invalid',
    'v-d-not-null'
  ])
})

export class AppComponent {
  title = 'SchoolBridge';

  //@ViewChild(ToastContainerDirective, {static: true}) toastContainer: ToastContainerDirective;
  @observed() public isEditing$: Observable<boolean>;
  constructor(//private _toastrService: ToastrService,
              _gbeService: GlobalizationEditService,
              _app: ApplicationRef,
              _router: Router,
              _loaderService: LoaderService) {
    setTimeout(()=>{
      _app.tick();
    }, 1000);

    _router.events.subscribe(x => {
      switch (x.constructor){
        case RouteConfigLoadStart:
          _loaderService.show("Loading page...");
          break;
        case RouteConfigLoadEnd:
          _loaderService.hide();
          break;
        case NavigationEnd:
          detectChanges(this);
          break;
      }
    });

    this.isEditing$ = _gbeService.stateObs;
  }

  prepareRoute(outlet: RouterOutlet) {
    return outlet;
  }
}
