import { trigger, transition, style, query, animateChild, animate, group } from '@angular/animations';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ClientConnectionService } from 'src/app/Services/client-connection.service';
import { MdGlobalization } from '../../globalization/Services/md-globalization.service';
import { anim } from './start.cmp-anim';

@Component({
  selector: 'app-start',
  templateUrl: './start.component.html',
  styleUrls: ['./start.component.css'],
  providers: MdGlobalization('st', [{str: 'title', prefix: 'add'}]),
  animations: anim
})
export class StartComponent  {

  prepareRoute(outlet: RouterOutlet) {
    return outlet && outlet.activatedRouteData && outlet.activatedRouteData.animation;
  }
}
