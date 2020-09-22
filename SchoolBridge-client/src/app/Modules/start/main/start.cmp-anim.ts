import { trigger, transition, style, query, animate, group, state } from '@angular/animations';

export const anim = [
  trigger('routeAnimations', [
    transition('LoginPage => RegisterPage', [
      style({ position: 'relative' }),
      query(':enter, :leave', [
        style({
          position: 'absolute',
          width: '100%'
        })
      ]),
      query(':enter', [
        style({
          left: '-100%',
          filter: 'blur(4px)',
          transform: 'scale(0.5)'
        })
      ]),
      query(':leave', [
        style({
          left: '0',
          opacity: '1',
          transform: 'scale(1)'
        }),
      ]),
      group([
        query(':leave', [
          animate('300ms ease', style({
            left: '100%',
            opacity: '0',
            filter: 'blur(4px)',
            transform: 'scale(0.5)'
          }))
        ]),
        query(':enter', [
          animate('300ms ease', style({
            left: '0',
            filter: 'none',
            transform: 'scale(1)'
          }))
        ])
      ]),
    ]),
    transition('RegisterPage => LoginPage', [
      style({ position: 'relative' }),
      query(':enter, :leave', [
        style({
          position: 'absolute',
          width: '100%'
        })
      ]),
      query(':enter', [
        style({
          left: '100%',
          filter: 'blur(4px)',
          transform: 'scale(0.5)'
        })
      ]),
      query(':leave', [
        style({
          left: '0',
          opacity: '1',
          transform: 'scale(1)'
        })
      ]),
      group([
        query(':leave', [
          animate('300ms ease', style({
            left: '-100%',
            opacity: '0',
            filter: 'blur(4px)',
            transform: 'scale(0.5)'
          }))
        ]),
        query(':enter', [
          animate('300ms ease', style({
            left: '0',
            filter: 'none',
            transform: 'scale(1)'
          }))
        ])
      ]),
    ])
  ])
]
