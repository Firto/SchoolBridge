import { Directive, HostListener, Output, EventEmitter, Input } from '@angular/core';

export type NgxScrollEvent = {
  isReachingBottom: boolean,
  isReachingTop: boolean,
  isSrollingToTop: boolean,
  isSrollingToBottom: boolean,
  originalEvent: Event,
  isWindowEvent: boolean
};

declare const window: Window;

@Directive({
  selector: '[libScrollEvent]'
})
export class NgxScrollDirective {
  @Output() public onScroll = new EventEmitter<NgxScrollEvent>();
  @Input() public bottomOffset: number = 100;
  @Input() public topOffset: number = 100;
  private _lastScrollTop: number = 0;

  constructor() { }

  // handle host scroll
  @HostListener('scroll', ['$event']) public scrolled($event: Event) {
    this.elementScrollEvent($event);
  }

  // handle window scroll
  @HostListener('window:scroll', ['$event']) public windowScrolled($event: Event) {
    this.windowScrollEvent($event);
  }

  protected windowScrollEvent($event: Event) {
    const target = <Document>$event.target;
    const scrollTop = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;
    const isReachingTop = scrollTop < this.topOffset;
    const isReachingBottom = ( target.body.offsetHeight - (window.innerHeight + scrollTop) ) < this.bottomOffset;
    const isSrollingToTop: boolean = scrollTop <= this._lastScrollTop;
    const isSrollingToBottom: boolean = !isSrollingToTop;
    this._lastScrollTop = scrollTop;
    const emitValue: NgxScrollEvent = {isReachingBottom, isReachingTop, originalEvent: $event, isWindowEvent: true, isSrollingToTop: isSrollingToTop, isSrollingToBottom: isSrollingToBottom};
    this.onScroll.emit(emitValue);
  }

  protected elementScrollEvent($event: Event) {
    const target = <HTMLElement>$event.target;
    const scrollPosition = target.scrollHeight - target.scrollTop;
    const offsetHeight = target.offsetHeight;
    const isReachingTop = target.scrollTop < this.topOffset;
    const isReachingBottom = (scrollPosition - offsetHeight) < this.bottomOffset;
    const isSrollingToTop: boolean = target.scrollTop <= this._lastScrollTop;
    const isSrollingToBottom: boolean = !isSrollingToTop;
    this._lastScrollTop = target.scrollTop;
    const emitValue: NgxScrollEvent = {isReachingBottom, isReachingTop, originalEvent: $event, isWindowEvent: false, isSrollingToTop: isSrollingToTop, isSrollingToBottom: isSrollingToBottom};
    this.onScroll.emit(emitValue);
  }

}