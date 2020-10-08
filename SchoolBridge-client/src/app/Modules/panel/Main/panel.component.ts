import { Component } from '@angular/core';
import { ClientConnectionService } from 'src/app/Services/client-connection.service';
import { MdGlobalization } from '../../globalization/Services/md-globalization.service';

@Component({
  selector: 'app-panel',
  templateUrl: './panel.component.html',
  styleUrls: ['./panel.component.css'],
  providers: MdGlobalization('pn')
})
export class PanelComponent  {
  constructor (clConnection: ClientConnectionService){}
}
