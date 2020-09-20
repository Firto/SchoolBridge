import { Component } from '@angular/core';
//import { Globalization } from 'src/app/Modules/globalization/Decorators/backend-strings.decorator';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';

@Component({
  selector: 'select-panel',
  templateUrl: './select-panel.component.html',
  styleUrls: ['./select-panel.component.css'],
  providers: MdGlobalization('sl')
})
//@Globalization('cm-pn-sel', [])
export class SelectPanelComponent {
  constructor(_gb: GlobalizationService){}
}
