import { trigger, transition, style, query, animateChild, animate, group } from '@angular/animations';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MdGlobalization } from '../../globalization/Services/md-globalization.service';

@Component({
  selector: 'app-start',
  templateUrl: './start.component.html',
  styleUrls: ['./start.component.css'],
  providers: MdGlobalization('st', [{str: 'title', prefix: 'add'}]),
  animations: [
    trigger('routeAnimations', [
      transition('LoginPage <=> RegisterPage', [
        style({ position: 'relative' }),
        query(':enter, :leave', [ 
          style({
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%'
          })
        ]),
        query(':enter', [
          style({ left: '-100%' })
        ]),
        query(':leave', animateChild()),
        group([ 
          query(':leave', [
            animate('300ms ease-out', style({ left: '100%' }))
          ]),
          query(':enter', [
            animate('300ms ease-out', style({ left: '0%' }))
          ])
        ]),
        query(':enter', animateChild()),
      ]),
      transition('* <=> FilterPage', [
        style({ position: 'relative' }),
        query(':enter, :leave', [
          style({
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%'
          })
        ]),
        query(':enter', [
          style({ left: '-100%' })
        ]),
        query(':leave', animateChild()),
        group([
          query(':leave', [
            animate('200ms ease-out', style({ left: '100%' }))
          ]),
          query(':enter', [
            animate('300ms ease-out', style({ left: '0%' }))
          ])
        ]),
        query(':enter', animateChild()),
      ])
    ])
  ]
})
export class StartComponent  {
  prepareRoute(outlet: RouterOutlet) {
    return outlet && outlet.activatedRouteData && outlet.activatedRouteData.animation;
  }
} 