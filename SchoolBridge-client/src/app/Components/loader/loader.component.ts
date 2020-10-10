import { ChangeDetectorRef, Component, OnInit, SkipSelf } from '@angular/core';
import { Observable } from 'rxjs';
import { finalize, takeUntil, tap } from 'rxjs/operators';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';
import { LoaderService, Loading } from 'src/app/Services/loader.service';
import { OnUnsubscribe } from 'src/app/Services/super.controller';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.css']
})
export class LoaderComponent extends OnUnsubscribe implements OnInit {
  isLoading: Loading = null;

  constructor(private _loaderService: LoaderService) {
    super();
  }

  ngOnInit(){
    this._loaderService.isLoading.pipe(takeUntil(this._destroy)).subscribe((x) => {
      this.isLoading = x;
      markDirty(this);
    });


  }
}
