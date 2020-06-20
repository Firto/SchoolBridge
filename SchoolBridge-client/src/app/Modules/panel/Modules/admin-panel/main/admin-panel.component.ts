import { Component, AfterContentInit } from '@angular/core';

@Component({
    selector: "admin-panel",
    styleUrls: ['./admin-panel.component.css'],
    templateUrl: './admin-panel.component.html'
})

export class AdminPanelComponent implements AfterContentInit {

    ngAfterContentInit(): void {
        $('#sidebarCollapse').on('click', function () {
            $('#sidebar').toggleClass('active');
        });
    }
    
}