import { ChangeDetectorRef } from '@angular/core';
import { Input } from '@angular/core';
import { BehaviorSubject, Observable, Subject, Subscription } from 'rxjs'
import { debounceTime, finalize, tap } from 'rxjs/operators';

export class NgxFormArgModel{
    private _errors: Observable<string>[] = [];
    constructor(public readonly name: string,
                private readonly _sb: Subject<unknown>){
    } 

    public setErrors(errors: Observable<string>[]): void{
        this._errors = errors;
    }

    public clearErrors(): void{
        this._errors = [];
    }

    public clearErrorsD(): void{
        this._errors = [];
        this._sb.next();
    }

    public hasErrors(): boolean{
        return this._errors.length > 0;
    }

    public get errors() : Observable<string>[]{
        return this._errors;
    }
}

export class NgxFormModel{
    public readonly args: Record<string, NgxFormArgModel> = {};

    private _sb: Subject<unknown> = new Subject();
    public readonly onChanged: Observable<unknown> = this._sb.pipe(debounceTime(10));

    constructor(public readonly name: string, 
                public readonly args_: string[]){
        args_.forEach(x => this.args[x] = new NgxFormArgModel(x, this._sb));
    }

    public setErrors(errors: Record<string, Observable<string>[]>): void{
        Object.keys(errors).forEach(x => {
            this.args[x].setErrors(errors[x].map(r => r.pipe(tap(() => this._sb.next()))));
        });
        this._sb.next()
    }

    public clearErrors(): void{
        this.args_.forEach(x => {
            this.args[x].clearErrors();
        });
        this._sb.next();
    }

    public get valid(): boolean{
        return this.args_.filter(x => this.args[x].hasErrors()).length === 0;
    }
}