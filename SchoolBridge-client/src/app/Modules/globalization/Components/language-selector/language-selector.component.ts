import { Component } from '@angular/core';
import { GlobalizationService } from '../../services/globalization.service';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.css']
})
export class LanguageSelectorComponent {
  
  constructor(public globalizationService: GlobalizationService) { 
    
  }

  public onSelect(e: MouseEvent){
    this.globalizationService.changeLanguage((<any>e.target).classList[1]).subscribe();
  }
}
