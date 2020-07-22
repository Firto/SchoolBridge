import { Component, ViewChild, OnInit, AfterContentChecked, AfterViewChecked, AfterViewInit } from '@angular/core';
import { ToastContainerDirective, ToastrService } from 'ngx-toastr';
import { GlobalizationService } from './Modules/globalization/services/globalization.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit, AfterViewInit {
  title = 'SchoolBridge-client';

  @ViewChild(ToastContainerDirective, {static: true}) toastContainer: ToastContainerDirective;
 
  constructor(private toastrService: ToastrService,
              public globalizationService: GlobalizationService) {}

  ngOnInit() {
    this.toastrService.overlayContainer = this.toastContainer;
  }

  ngAfterViewInit(){
      console.log("mm");
  }
}
