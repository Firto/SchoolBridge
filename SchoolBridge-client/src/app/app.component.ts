import { Component, ViewChild, OnInit, AfterContentChecked, AfterViewChecked, AfterViewInit } from '@angular/core';
import { ToastContainerDirective, ToastrService } from 'ngx-toastr';
import { GlobalizationEditService } from './Modules/globalization/Services/globalization-edit.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit, AfterViewInit {
  title = 'SchoolBridge-client';

  @ViewChild(ToastContainerDirective, {static: true}) toastContainer: ToastContainerDirective;
 
  constructor(private toastrService: ToastrService,
              public gbeService: GlobalizationEditService) {}

  ngOnInit() {
    this.toastrService.overlayContainer = this.toastContainer;
  }

  ngAfterViewInit(){
      console.log("mm");
  }
}
