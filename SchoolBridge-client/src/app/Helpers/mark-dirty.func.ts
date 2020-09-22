import { ɵdetectChanges, ɵmarkDirty } from '@angular/core';

export function markDirty(component: {}): void{
    console.log("refresh", component);
    ɵmarkDirty(component);
}

export function detectChanges(component: {}): void{
    console.log("BBBB", component);
    ɵdetectChanges(component);
}