import { Component } from '@angular/core';
import { GlobalizationService } from '../../Services/globalization.service';
import { GlobalizationInfoService } from '../../Services/globalization-info.service';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.css']
})
export class LanguageSelectorComponent {
  
  constructor(public gbService: GlobalizationService,
              public gbiService: GlobalizationInfoService) { 
    
  }

  public onSelect(e: MouseEvent){
    this.gbService.changeLanguage((<any>e.target).classList[1]).subscribe();
  }
}
