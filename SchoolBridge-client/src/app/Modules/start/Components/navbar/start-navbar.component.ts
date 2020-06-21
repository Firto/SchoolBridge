import { Component, AfterViewInit } from '@angular/core';

@Component({
  selector: 'start-navbar',
  templateUrl: './start-navbar.component.html',
  styleUrls: ['./start-navbar.component.css']
})
export class StartNavbarComponent implements AfterViewInit{
  private checkbox: HTMLInputElement;

  constructor() { }

  onClickMenuItem(): void {
    if (this.checkbox.checked)
        this.checkbox.checked = false;
  }

  ngAfterViewInit(): void {
    this.checkbox = <HTMLInputElement> document.getElementById("chk");
  }

}