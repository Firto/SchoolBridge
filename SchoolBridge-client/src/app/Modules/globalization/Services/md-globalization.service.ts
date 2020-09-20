import { Inject, Injectable, InjectionToken, Optional, SkipSelf } from '@angular/core';  
import { GlobalizationStringService } from './globalization-string.service';
import { Observable } from 'rxjs';
import { isString } from 'util';
  
const GB_STRINGS: InjectionToken<string[]> = new InjectionToken<(string | {str: string, prefix: 'add'})[]>("GB_STRINGS");
const GB_PREFIX: InjectionToken<string> = new InjectionToken<string>("GB_PREFIX");

export function MdGlobalization(prefix: string, strings: (string | {str: string, prefix: 'add'})[] = []): {provide: any, useValue?: any, useClass?: any}[]{
    return [
        {provide: GB_PREFIX, useValue: prefix},
        {provide: GB_STRINGS, useValue: strings},
        {provide: MdGlobalizationService, useClass: MdGlobalizationService}
    ];
} 

@Injectable()
export class MdGlobalizationService {
    public readonly prefix: string;
    public readonly usedStrings: string[] = [];
    public readonly constStrings: string[];
    public get allStrings(): string[]{
        return this.usedStrings.concat(this.constStrings);
    }
    constructor(private _gbsService: GlobalizationStringService, 
                @Inject(GB_STRINGS) constStrings: (string | {str: string, prefix: 'add'})[],
                @Inject(GB_PREFIX) public readonly prefix_: string,
                @SkipSelf() @Optional() private readonly _parent: MdGlobalizationService){
        this.prefix = this.createPrefix();
        this.constStrings = constStrings.map((x: any) => {
            if (typeof x !== 'string')
                return this.prefix + '-' + x.str;
            return <string>x;
        });
        this._gbsService.initConstStrings(this.constStrings);
    }

    public useString(str: string, prefix: boolean = true): Observable<string>{
        if (prefix)
            str = this.prefix + '-' + str;
        if (!this.usedStrings.includes(str)) 
            this.usedStrings.push(str);
        return this._gbsService.getStringObs(str);
    }

    public useStrings(strs: string[], prefix: boolean = true): Record<string, Observable<string>>{
        if (prefix)
            strs = strs.map(x => this.prefix + '-' + x);
        this.usedStrings.push(...strs.filter(x => !this.usedStrings.includes(x)));
        return this._gbsService.getStringsObs(strs);
    }

    public createPrefix(): string{
        if (!this._parent) return this.prefix_;
        return this._parent.createPrefix() + '-' + this.prefix_;
    }
}    