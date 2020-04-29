import { Component, OnInit } from '@angular/core';
import { LoaderService } from 'src/app/Helpers/loader.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.css']
})
export class LoaderComponent {
  isLoading: Observable<string>;

  constructor(private loaderService: LoaderService) {
    this.isLoading = loaderService.isLoading;
  }
}
