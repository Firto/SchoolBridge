import { Component, ViewChild, OnInit } from '@angular/core';
import { ToastContainerDirective, ToastrService } from 'ngx-toastr';
import { GlobalizationService } from './Modules/globalization/services/globalization.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  title = 'SchoolBridge-client';

  @ViewChild(ToastContainerDirective, {static: true}) toastContainer: ToastContainerDirective;
 
  constructor(private toastrService: ToastrService,
              languageStringService: GlobalizationService) {}

  ngOnInit() {
    this.toastrService.overlayContainer = this.toastContainer;
  }
}
