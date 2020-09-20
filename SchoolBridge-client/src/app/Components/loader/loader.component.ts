import { ChangeDetectorRef, Component, OnInit, SkipSelf } from '@angular/core';
import { Observable } from 'rxjs';
import { finalize, takeUntil, tap } from 'rxjs/operators';
import { LoaderService } from 'src/app/Services/loader.service';
import { OnUnsubscribe } from 'src/app/Services/super.controller';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.css']
})
export class LoaderComponent extends OnUnsubscribe implements OnInit {
  isLoading: string = null;

  constructor(private _loaderService: LoaderService,
              private _ch: ChangeDetectorRef) {
    super();
  }

  ngOnInit(){
    this._loaderService.isLoading.pipe(takeUntil(this._destroy)).subscribe((x) => {
      this.isLoading = x;
      this._ch.detectChanges();
    });
  }
}
